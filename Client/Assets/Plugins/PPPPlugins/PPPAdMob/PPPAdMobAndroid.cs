using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID

public class PPPAdMobAndroid
{
	private static AndroidJavaClass _pluginClass;
	private static AndroidJavaObject _plugin;
		
	static PPPAdMobAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPAdMobAndroid constructor function is called.This function works on Android platforms only.");
			return;
		}
		
		// find the plugin instance
		_pluginClass = new AndroidJavaClass( "com.ppp.admob.PPPAdMobPlugin" );
		_plugin = _pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}

	
	
    #region PPPAdMob
    
    // Initialize PPPAdMobAndroid Library
    public static void init()
    {
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.init is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("init");
    }

    public static void deinit()
    {
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.deinit is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("deinit");
    }
	
    public static void reinit()
    {
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.reinit is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("reinit");
    }
	
	public static void setAdUnitID(string adUnitID)
    {
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.setAdUnitID is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("setAdUnitID");
    }
	
	public static void setRequestInterval(int intervalInSeconds)
    {
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.setRequestInterval is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("setRequestInterval");
    }
	
	public static void refreshAD()
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.refreshAD is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("refreshAD");
	}
	
	public static bool getVisibility()
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.getVisibility is called.This function works on Android platforms only.");
            return false;
        }
        return _plugin.Call<bool>("getVisibility");
	}
	
	public static void setVisibility(bool isVisible)
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.setVisibility is called.This function works on Android platforms only.");
            return;
        }
        _plugin.Call("setVisibility", isVisible);
	}
	
	// TODO: Not implemented
	public static Vector2 getPosition(PPPAdMobAnchorPointType anchorPtType)
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.getPosition is called.This function works on Android platforms only.");
            return new Vector2(0,0);
        }
        return new Vector2(0,0);
	}
	
	// TODO: Not implemented
	public static void setOrigin(Vector2 paraOriginPoint,PPPAdMobAnchorPointType anchorPtType)
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.getVisibility is called.This function works on Android platforms only.");
            return;
        }
        return;
	}
	
	// TODO: Not implemented
	public static PPPAdMobDockType Dock
	{
		get
		{
			if (Application.platform != RuntimePlatform.Android) {
				PPPDebug.Log("PPPAdMobAndroid.Dock.get is called.This function works on Android platforms only.");
				return PPPAdMobDockType.Custom;
			}
			return PPPAdMobDockType.Custom;
		}
		set
		{
			if (Application.platform != RuntimePlatform.Android) {
				PPPDebug.Log("PPPAdMobAndroid.Dock.set is called.This function works on Android platforms only.");
				return;
			}
			return;
		}
	}
	
	// TODO: Not implemented
	/*
	 * SizeID:
	 * -1:Unknown size(No AdView)
	 * 0: 320x50
	 * 1: 320x250
	 * 2: 468x60
	 * 3: 728x90
	 * 7: SmartBanner
	 */
	public static PPPAdMobSizeType getSizeID()
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.getSizeID is called.This function works on Android platforms only.");
			return PPPAdMobSizeType.UnknownSize_or_NoAdView;
        }
		return ((PPPAdMobSizeType)(_plugin.Call<int>("getAdSize")));
	}
	
	// TODO: Not implemented
	/*
	 * SizeID:
	 * 0: 320x50
	 * 1: 320x250
	 * 2: 468x60
	 * 3: 728x90
	 * Other: SmartBanner.
	 */
	public static void setSizeID(PPPAdMobSizeType paraSizeID)
	{
        if( Application.platform != RuntimePlatform.Android )
        {
            PPPDebug.Log("PPPAdMobAndroid.setSizeID is called.This function works on Android platforms only.");
			return;
        }
		_plugin.Call("setAdSize", (int)paraSizeID);
	}
	
    #endregion
}
#endif
