using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
	private GameObject _player;
	// Use this for initialization
	void Start () {
		
		if (isLocalPlayer)
		{
			GetComponent<LocalPlayerController>().enabled = true;
			gameObject.tag = "LocalPlayer";
			Camera.main.GetComponent<MainCameraController>().SetLocalPlayer(this.gameObject);
		}
		else
		{
			GetComponent<LocalPlayerController>().enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
