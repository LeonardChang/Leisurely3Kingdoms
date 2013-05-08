using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID

namespace PPP.Unity3D.Plugins.Billing.Android {
    
public class PPPGFanBillingAndroid
{
	private static AndroidJavaClass _pluginClass;
	//private static AndroidJavaObject _plugin;
		
	static PPPGFanBillingAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin class
		_pluginClass = new AndroidJavaClass( "com.ppp.gfanbilling.PPPGFanBillingPlugin" );
		
		// find the plugin instance
		//using( var _pluginClass = new AndroidJavaClass( "com.ppp.gfanbilling.PPPGFanBillingPlugin" ) )
			//_plugin = _pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	// 初始化 //
	public static void init()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanBillingAndroid.Init is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "init" );
	}
	
	// 购买 //
	public static void pay(int priceValue)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanBillingAndroid.pay1 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "pay", priceValue );
	}
	
	// 购买 //
	public static void pay(string itemName, string itemDesc, int priceValue)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanBillingAndroid.pay2 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "pay", itemName, itemDesc, priceValue );
	}
	
	// 机锋点数充值 //
	public static void startGfanCharge()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanBillingAndroid.startGfanCharge is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "startGfanCharge" );
	}
	
}
    
}

#endif
