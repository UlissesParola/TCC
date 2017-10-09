using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class LocalPlayerController : MonoBehaviour
{
	public float MovimentSpeed = 5f;
	public float BoostMultiplier = 2f;
	
	private Rigidbody2D _playerRB;
	// Use this for initialization
	void Start ()
	{
		_playerRB = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//Vector3 moviment = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"),CrossPlatformInputManager.GetAxis("Vertical"), 0 ) * Time.deltaTime * MovimentSpeed;
		//transform.position += moviment;
	}

	private void FixedUpdate()
	{
		_playerRB.velocity = new Vector3(CrossPlatformInputManager.GetAxis("Horizontal"),CrossPlatformInputManager.GetAxis("Vertical"), 0 ) * MovimentSpeed;
		Debug.Log(CrossPlatformInputManager.GetAxis("Vertical"));
		//bool isBoost = CrossPlatformInputManager.GetButton("Boost");
	}
}
