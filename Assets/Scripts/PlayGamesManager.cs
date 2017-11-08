
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class PlayGamesManager : MonoBehaviour, RealTimeMultiplayerListener
{
	public MenuControl MainMenu;
	
	const int MinOpponents = 1, MaxOpponents = 1;
	const int GameVariant = 0;
	
	private string _log;
	private static PlayGamesManager _instance;
	private int _updateMessageLength = 17;
	private List<byte> _updateMessage;
	private byte[] _messageToSend;
	private IMPUpdateListener _gameManager;

	// Use this for initialization
	void Awake() {
		if (_instance == null && _instance != this)
		{
			_instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		Authenticate();

		_updateMessage = new List<byte>(_updateMessageLength);
		_messageToSend = new byte[_updateMessageLength];
	}

	public void Authenticate()
	{
		if (!PlayGamesPlatform.Instance.IsAuthenticated())
		{
			PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();

			PlayGamesPlatform.InitializeInstance(config);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();

			PlayGamesPlatform.Instance.Authenticate((bool sucess) =>
			{
				if (sucess)
				{
					ShowMPStatus("Autenticado com sucesso.");
				}
				else
				{
					ShowMPStatus("Falha na autenticação.");
				}
			});
		}
	}
	
	public void SignInAndStartMpGame() {
		if (! PlayGamesPlatform.Instance.localUser.authenticated) {
			PlayGamesPlatform.Instance.localUser.Authenticate((bool success) => {
				if (success) {
					ShowMPStatus("We're signed in! Welcome " + 
					             PlayGamesPlatform.Instance.localUser.userName);
				} else {
					ShowMPStatus("Oh... we're not signed in.");
				}
			});
		} else {
			ShowMPStatus("You're already signed in.");
		}
	}

	public void StartMatchMaking()
	{
		PlayGamesPlatform.Instance.RealTime.CreateQuickGame(MinOpponents, MaxOpponents,
			GameVariant, _instance);
	}

	public void SignOut()
	{
		PlayGamesPlatform.Instance.SignOut();
		ShowMPStatus("Deslogando");
	}

	public bool IsAuthenticated()
	{
		return PlayGamesPlatform.Instance.localUser.authenticated;
	}

	private void ShowMPStatus(string message)
	{
		_log += message + "\n";
		MainMenu.ShowDebugLog(_log);
		Debug.Log(message);
	}

	public List<Participant> GetAllPlayers()
	{
		return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
	}

	public string GetMyParticipantId()
	{
		return PlayGamesPlatform.Instance.RealTime.GetSelf().ParticipantId;
	}
	
	
	
	#region RealTimeMultiplayerListener implementation

	public void OnRoomSetupProgress(float progress) {
		// show the default waiting room.
		MainMenu.ShowWaitingRoomUI();
		ShowMPStatus("Buscando outros jogadores."); 
	}

	public void OnRoomConnected(bool success)
	{
		if (success)
		{
			MainMenu.HideWaitingRoomUI();
			SceneManager.LoadScene("Online");
			
		} else {
			ShowMPStatus ("Não foi possivel se conectar.");
		}
	}

	public void OnLeftRoom()
	{
		ShowMPStatus ("We have left the room. We should probably perform some clean-up tasks.");
	}

	public void OnParticipantLeft(Participant participant)
	{
		throw new System.NotImplementedException();
	}

	public void OnPeersConnected(string[] participantIDs)
	{
		foreach (string participantId in participantIDs)
		{
			ShowMPStatus("Player " + participantId + " has joined.");
		}
	}

	public void OnPeersDisconnected(string[] participantIDs)
	{
		foreach (string participantId in participantIDs)
		{
			ShowMPStatus("Player " + participantId + " has left.");
		}
	}

	public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
	{
		string sender = senderId;
		
		char messageType = (char)data[0];
		if (messageType == 'U' && data.Length == _updateMessageLength)
		{
			float posX = System.BitConverter.ToSingle(data, 1);
			float posY = System.BitConverter.ToSingle(data, 5);
			float velX = System.BitConverter.ToSingle(data, 9);
			float velY = System.BitConverter.ToSingle(data, 13);
			
			if (_gameManager == null)
			{
				_gameManager = FindObjectOfType<GameManager>();
			}
			else
			{
				_gameManager.UpdateReceived(senderId, posX, posY, velX, velY);	
			}
		}	
	}

	#endregion

	
	public void SendMyUpdate(float posX, float posY, Vector2 velocity)
	{
		_updateMessage.Clear ();
		_updateMessage.Add((byte) 'U');
		_updateMessage.AddRange(System.BitConverter.GetBytes(posX));
		_updateMessage.AddRange(System.BitConverter.GetBytes(posY));
		_updateMessage.AddRange(System.BitConverter.GetBytes(velocity.x));
		_updateMessage.AddRange(System.BitConverter.GetBytes(velocity.y));
		_messageToSend = _updateMessage.ToArray();
		PlayGamesPlatform.Instance.RealTime.SendMessageToAll(false, _messageToSend);
	}

	
}

