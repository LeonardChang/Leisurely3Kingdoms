using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PPPAdMobEventManager : MonoBehaviour, IPPPAdMobEventReceiver
{
	// On receive AD
	public static event Action onReceiveAdEvent;
	
	// On failed to receive AD
	public static event Action<string> onFailedToReceiveAdEvent;
	
	
	
	#region IPPPAdMobEventReceiver implementation
	public void onReceiveAd ()
	{
		if( onReceiveAdEvent != null )
			onReceiveAdEvent(  );
	}

	public void onFailedToReceiveAd (string errorString)
	{
		if( onFailedToReceiveAdEvent != null )
			onFailedToReceiveAdEvent( errorString );
	}
	#endregion
}
