using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

public class GameManager : MonoBehaviour, IMPUpdateListener
{
	public GameObject OtherPlayerPrefab;
	public GameObject PlayerPrefab;
	public Sprite[] PlayerSprite; 
	public GameObject[] SpawningPoints;

	private GameObject _localPlayer;
	private GameObject _otherPlayer;
	private PlayGamesManager _playGamesManager;
	private bool _multiplayerReady;
	private string _myParticipantId;
	private string _otherParticipantId;
	
	// Use this for initialization
	void Start ()
	{
		_playGamesManager = GameObject.Find("PlayGamesManager").GetComponent<PlayGamesManager>();
		SetupMultiplayerGame();
	}
	
	// Update is called once per frame
	void Update ()
	{
		SendPlayerUpdate();
	}

	public void SetupMultiplayerGame()
	{
		_myParticipantId = _playGamesManager.GetMyParticipantId();
		List<Participant> allPlayers = _playGamesManager.GetAllPlayers();

		for (int i = 0; i < allPlayers.Count; i++)
		{
			Vector3 spawningPoint = SpawningPoints[i].transform.position;
			
			string nextParticipantId = allPlayers[i].ParticipantId;
			if (nextParticipantId == _myParticipantId)
			{
				_localPlayer =  Instantiate(PlayerPrefab, spawningPoint, Quaternion.identity);
				_localPlayer.GetComponent<SpriteRenderer>().sprite = PlayerSprite[i];
				Camera.main.GetComponent<MainCameraController>().SetLocalPlayer(_localPlayer);
			}
			else
			{
				_otherPlayer =  Instantiate(OtherPlayerPrefab, spawningPoint, Quaternion.identity);
				_otherPlayer.GetComponent<SpriteRenderer>().sprite = PlayerSprite[i];
				_otherParticipantId = nextParticipantId;
			}
		}

		_multiplayerReady = true;
	}

	private void SendPlayerUpdate()
	{
		float posX = _localPlayer.transform.position.x;
		float posY = _localPlayer.transform.position.y;
		Vector2 velocity = _localPlayer.GetComponent<Rigidbody2D>().velocity;
		
		_playGamesManager.SendMyUpdate(posX, posY, velocity);
	}

	public void UpdateReceived(string participantId, float posX, float posY, float velX, float velY)
	{
		if (_multiplayerReady)
		{
			if (_otherPlayer != null)
			{
				_otherPlayer.transform.position = new Vector3(posX, posY, 0);
				_otherPlayer.GetComponent<Rigidbody2D>().velocity = new Vector2(velX, velY);
			}
		}

	}
}
