using UnityEngine;
using System.Collections;


public class PPPMMBilling : MonoBehaviour {
	
	// Make a GameObject in static constructor.
	// Because static constructor is called before any of the other static method in this class.
	// The GameObject will receive Unity Message sent from native plugin.
	// This GameObject is set to be never destroyed on scene/level loading.
	static PPPMMBilling()
	{
		GameObject _gameobj_mmbillingevtmgr = GameObject.Find("PPP_PluginObject_MMBillingEventManager");
		if (_gameobj_mmbillingevtmgr == null)
		{
			_gameobj_mmbillingevtmgr = new GameObject("PPP_PluginObject_MMBillingEventManager");
			GameObject.DontDestroyOnLoad(_gameobj_mmbillingevtmgr);
		}
		Component _component_mmbillingevtmgr = _gameobj_mmbillingevtmgr.GetComponent("PPPMMBillingEventManager");
		if (_component_mmbillingevtmgr == null)
		{
			_gameobj_mmbillingevtmgr.AddComponent("PPPMMBillingEventManager");
		}
	}
	
	public static void init(string appId, string appKey)
	{
#if UNITY_ANDROID
		PPPDebug.Log("PPPMMBilling.init is called!");
		PPPMMBillingAndroid.init( appId, appKey);
#else
		PPPDebug.Log("PPPMMBilling.init is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void order(string paycode)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.order( paycode );
#else
		PPPDebug.Log("PPPMMBilling.order1 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void order(string paycode, int orderCount)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.order( paycode, orderCount );
#else
		PPPDebug.Log("PPPMMBilling.order2 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void order(string paycode, int orderCount, bool autoRenew)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.order( paycode, orderCount, autoRenew );
#else
		PPPDebug.Log("PPPMMBilling.order3 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void query(string paycode)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.query( paycode );
#else
		PPPDebug.Log("PPPMMBilling.query1 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void query(string paycode, string tradeId)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.query( paycode, tradeId );
#else
		PPPDebug.Log("PPPMMBilling.query2 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static string getSDKVersion()
	{
#if UNITY_ANDROID
		return PPPMMBillingAndroid.getSDKVersion();
#else
		PPPDebug.Log("PPPMMBilling.getSDKVersion is called.This function works on mobile platforms only.Please change the build settings.");
        return "Failed to get SDK version on non-Android platform.";
#endif
	}

	public static void setTimeout(int connTimeout, int dataTimeout)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.setTimeout( connTimeout, dataTimeout);
#else
		PPPDebug.Log("PPPMMBilling.setTimeout is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	public static void enableCache(bool enable)
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.enableCache( enable );
#else
		PPPDebug.Log("PPPMMBilling.enableCache is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	public static void clearCache()
	{
#if UNITY_ANDROID
		PPPMMBillingAndroid.clearCache();
#else
		PPPDebug.Log("PPPMMBilling.clearCache is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}

}
