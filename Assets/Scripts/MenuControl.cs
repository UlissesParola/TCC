using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour 
	{
	public GameObject StartMenu;
	public GameObject WaitingRoom;
	public GameObject Debug;
	public Text StartMenuDebugText;
	public Text WaitingRoomDebugText;
	
	private bool _showingWaitingRoom = false;

	private void Start()
	{
		GameObject.Find("PlayGamesManager").GetComponent<PlayGamesManager>().MainMenu = this;
	}

	// Update is called once per frame
	public void ShowWaitingRoomUI()
	{
		_showingWaitingRoom = true;
		StartMenu.SetActive(false);
		Debug.SetActive(false);
		WaitingRoom.SetActive(true);
	}
	
	public void HideWaitingRoomUI()
	{
		_showingWaitingRoom = false;
		StartMenu.SetActive(true);
		Debug.SetActive(true);
		WaitingRoom.SetActive(false);
	}

	public void ShowDebugLog(string log)
	{
		if (_showingWaitingRoom)
		{
			WaitingRoomDebugText.text = log;
		}
		else
		{
			StartMenuDebugText.text = log;
		}
	}
}

