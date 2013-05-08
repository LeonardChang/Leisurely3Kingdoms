using UnityEngine;
using System.Collections;
#if UNITY_ANDROID
    using PPP.Unity3D.Plugins.Billing.Android;
#else
#endif

namespace PPP.Unity3D.Plugins.Billing {
    
public class PPPGFanBilling {
	
	// Make a GameObject in static constructor.
	// Because static constructor is called before any of the other static method in this class.
	// The GameObject will receive Unity Message sent from native plugin.
	// This GameObject is set to be never destroyed on scene/level loading.
	static PPPGFanBilling()
	{
		// Game object for recieve GFan IAP event.
		GameObject _gameobj_gfanbillingevtmgr = GameObject.Find("PPP_PluginObject_GFanBillingEventManager");
		if (_gameobj_gfanbillingevtmgr == null)
		{
			_gameobj_gfanbillingevtmgr = new GameObject("PPP_PluginObject_GFanBillingEventManager");
			GameObject.DontDestroyOnLoad(_gameobj_gfanbillingevtmgr);
		}
		Component _component_gfanbillingevtmgr = _gameobj_gfanbillingevtmgr.GetComponent("PPPGFanBillingEventManager");
		if (_component_gfanbillingevtmgr == null)
		{
			_gameobj_gfanbillingevtmgr.AddComponent("PPPGFanBillingEventManager");
		}
	}
	
	public static void init()
	{
#if UNITY_ANDROID
		PPPGFanBillingAndroid.init();
#else
		PPPDebug.Log("PPPGFanBilling.init is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void pay(int priceValue)
	{
#if UNITY_ANDROID
		PPPGFanBillingAndroid.pay(priceValue);
#else
		PPPDebug.Log("PPPGFanBilling.pay1 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void pay(string itemName, string itemDesc, int priceValue)
	{
#if UNITY_ANDROID
		PPPGFanBillingAndroid.pay( itemName, itemDesc, priceValue );
#else
		PPPDebug.Log("PPPGFanBilling.pay2 is called.This function works on Android only.Please change the build settings.");
#endif
	}

	public static void startGfanCharge()
	{
#if UNITY_ANDROID
		PPPGFanBillingAndroid.startGfanCharge();
#else
		PPPDebug.Log("PPPGFanBilling.startGfanCharge is called.This function works on Android only.Please change the build settings.");
#endif
	}

}

}