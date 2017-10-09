using System.Collections;
using System.Collections.Generic;
using GooglePlayGames.BasicApi.Multiplayer;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public GameObject OtherPlayerPrefab;
	public GameObject PlayerPrefab;
	public GameObject[] SpawningPoints;

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
	void Update () {
		
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
				Instantiate(PlayerPrefab, spawningPoint, Quaternion.identity);
			}
			else
			{
				Instantiate(OtherPlayerPrefab, spawningPoint, Quaternion.identity);
				_otherParticipantId = nextParticipantId;
			}
		}

		_multiplayerReady = true;
	}
}
