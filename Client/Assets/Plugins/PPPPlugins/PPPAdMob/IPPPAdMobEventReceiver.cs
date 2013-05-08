using UnityEngine;
using System.Collections;

public interface IPPPAdMobEventReceiver {

	// Use this for initialization
	void onReceiveAd ();
	
	// Update is called once per frame
	void onFailedToReceiveAd (string errorString);
}
