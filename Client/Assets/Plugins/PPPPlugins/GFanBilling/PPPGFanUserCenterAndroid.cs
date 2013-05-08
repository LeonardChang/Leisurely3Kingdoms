using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID
namespace PPP.Unity3D.Plugins.Billing.Android {

public class PPPGFanUserCenterAndroid
{
	private static AndroidJavaClass _pluginClass;
	//private static AndroidJavaObject _plugin;
		
	static PPPGFanUserCenterAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin class
		_pluginClass = new AndroidJavaClass( "com.ppp.gfanbilling.PPPGFanUserCenterPlugin" );
		
		// find the plugin instance
		//using( var _pluginClass = new AndroidJavaClass( "com.ppp.gfanbilling.PPPGFanUserCenterPlugin" ) )
			//_plugin = _pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	// 用户注销 //
	public static void startGfanLogout()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanUserCenterAndroid.startGfanLogout is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "startGfanLogout" );
	}
	
	// 用户登录 //
	public static void startGfanLogin()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanUserCenterAndroid.startGfanLogin is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "startGfanLogin" );
	}

	// 用户注册 //
	public static void startGfanRegister()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPGFanUserCenterAndroid.startGfanRegister is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "startGfanRegister" );
	}
}
    
}
#endif
