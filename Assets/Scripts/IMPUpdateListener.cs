using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMPUpdateListener 
{
	void UpdateReceived(string participantId, float posX, float posY, float velX, float velY);
}

