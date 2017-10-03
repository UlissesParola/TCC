using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraController : MonoBehaviour
{
	public GameObject _player;
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, this.transform.position.z);
	}

	public void SetLocalPlayer(GameObject localPlayer)
	{
		_player = localPlayer;
	}
}
