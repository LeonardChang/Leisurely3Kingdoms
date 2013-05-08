using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
    using PPP.Unity3D.Plugins.Billing.Android;
#else
#endif


public class PPPGFanUserCenter {
	
	// Make a GameObject in static constructor.
	// Because static constructor is called before any of the other static method in this class.
	// The GameObject will receive Unity Message sent from native plugin.
	// This GameObject is set to be never destroyed on scene/level loading.
	static PPPGFanUserCenter()
	{
		// Game object for recieve GFan UserCenter event.
		GameObject _gameobj_gfanusercenterevtmgr = GameObject.Find("PPP_PluginObject_GFanUserCenterEventManager");
		if (_gameobj_gfanusercenterevtmgr == null)
		{
			_gameobj_gfanusercenterevtmgr = new GameObject("PPP_PluginObject_GFanUserCenterEventManager");
			GameObject.DontDestroyOnLoad(_gameobj_gfanusercenterevtmgr);
		}
		Component _component_gfanusercenterevtmgr = _gameobj_gfanusercenterevtmgr.GetComponent("PPPGFanUserCenterEventManager");
		if (_component_gfanusercenterevtmgr == null)
		{
			_gameobj_gfanusercenterevtmgr.AddComponent("PPPGFanUserCenterEventManager");
		}
	}
	
	public static void startGfanLogout()
	{
#if UNITY_ANDROID
		PPPGFanUserCenterAndroid.startGfanLogout();
#else
		PPPDebug.Log("PPPGFanUserCenter.startGfanLogout is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void startGfanLogin()
	{
#if UNITY_ANDROID
		PPPGFanUserCenterAndroid.startGfanLogin();
#else
		PPPDebug.Log("PPPGFanUserCenter.startGfanLogin is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void startGfanRegister()
	{
#if UNITY_ANDROID
		PPPGFanUserCenterAndroid.startGfanRegister();
#else
		PPPDebug.Log("PPPGFanUserCenter.startGfanRegister is called.This function works on Android only.Please change the build settings.");
#endif
	}

}
