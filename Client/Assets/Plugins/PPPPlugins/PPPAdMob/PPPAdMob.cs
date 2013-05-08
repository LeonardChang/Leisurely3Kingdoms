using UnityEngine;
using System.Collections;

public enum PPPAdMobSizeType
{
	UnknownSize_or_NoAdView       = -1,
	Banner_320x50                 = 0,
	MediumRectangle_320x250       = 1,
	FullBanner_468x60             = 2,
	Leaderboard_728x90            = 3,
	Skyscraper_iOS_120x600        = 4,
	SmartBanner_iOS_Portrait      = 5,	//50 pixels tall on an iPhone/iPod UI.
										//90 pixels tall on an iPad UI.
	SmartBanner_iOS_Landscape     = 6,	//32 pixels tall on an iPhone/iPod UI.
										//90 pixels tall on an iPad UI.
	SmartBanner_Android           = 7,
}

public enum PPPAdMobAnchorPointType
{
	TopLeft,
	TopRight,
	BottomLeft,
	BottomRight,
	Center,
}

public enum PPPAdMobDockType
{
	Custom,
	Top,
	Bottom,
	Left,
	Right,
}

public class PPPAdMob : MonoBehaviour {

    #region PPPAdMob
	// Make a GameObject in static constructor.
	// Because static constructor is called before any of the other static method in this class.
	// The GameObject will receive Unity Message sent from native plugin.
	// This GameObject is set to be never destroyed on scene/level loading.
	static PPPAdMob()
	{
		GameObject _gameobj_admobmgr = GameObject.Find("PPP_PluginObject_AdMobManager");
		if (_gameobj_admobmgr == null)
		{
			_gameobj_admobmgr = new GameObject("PPP_PluginObject_AdMobManager");
			GameObject.DontDestroyOnLoad(_gameobj_admobmgr);
		}
		Component _component_admobmgr = _gameobj_admobmgr.GetComponent("PPPAdMobEventManager");
		if (_component_admobmgr == null)
		{
			_gameobj_admobmgr.AddComponent("PPPAdMobEventManager");
		}
	}
    
    // Initialize PPPAdMob Library
    public static void init()
    {
#if UNITY_ANDROID
        PPPAdMobAndroid.init();
#elif UNITY_IPHONE
		PPPAdMobiOS.init();
#else
        PPPDebug.Log("PPPAdMob.init is called.This function works on mobile platform only.Please change the build settings.");
        return;
#endif
    }
    
    // Deinitialize PPPAdMob Library
	public static void deinit()
	{
#if UNITY_ANDROID
		PPPAdMobAndroid.deinit();
#elif UNITY_IPHONE
		PPPAdMobiOS.deinit();
#else
		PPPDebug.Log("PPPAdMob.deinit is called.This function works on mobile platform only.Please change the build settings.");
        return;
#endif
	}
	
    // Deinitialize PPPAdMob Library
	public static void reinit()
	{
#if UNITY_ANDROID
		PPPAdMobAndroid.reinit();
#elif UNITY_IPHONE
		PPPAdMobiOS.reinit();
#else
		PPPDebug.Log("PPPAdMob.reinit is called.This function works on mobile platform only.Please change the build settings.");
        return;
#endif
	}

	public static void refreshAD()
	{
#if UNITY_ANDROID
		PPPAdMobAndroid.refreshAD();
#elif UNITY_IPHONE
		PPPAdMobiOS.refreshAD();
#else
		PPPDebug.Log("PPPAdMob.refreshAD is called.This function works on mobile platform only.Please change the build settings.");
        return;
#endif
	}
	
	public static Vector2 getPosition(PPPAdMobAnchorPointType anchorPtType)
	{
#if UNITY_ANDROID
		return PPPAdMobAndroid.getPosition(anchorPtType);
#elif UNITY_IPHONE
		return PPPAdMobiOS.getPosition(anchorPtType);
#else
		PPPDebug.Log("PPPAdMob.getPosition is called.This function works on mobile platform only.Please change the build settings.");
        return new Vector2(0,0);
#endif
	}

	public static void setPosition(PPPAdMobAnchorPointType anchorPtType, Vector2 targetPos)
	{
#if UNITY_ANDROID
		PPPAdMobAndroid.setOrigin(targetPos,anchorPtType);
#elif UNITY_IPHONE
		PPPAdMobiOS.setOrigin(targetPos,anchorPtType);
#else
		PPPDebug.Log("PPPAdMob.setPosition is called.This function works on mobile platform only.Please change the build settings.");
        return;
#endif
	}
	
	public static PPPAdMobDockType Dock
	{
#if UNITY_ANDROID
		get
		{
			return PPPAdMobAndroid.Dock;
		}
		set
		{
			PPPAdMobAndroid.Dock = value;
		}
#elif UNITY_IPHONE
		get
		{
			return PPPAdMobiOS.Dock;
		}
		set
		{
			PPPAdMobiOS.Dock = value;
		}
#else
		get
		{
			PPPDebug.Log("PPPAdMob.Dock is called.This function works on mobile platform only.Please change the build settings.");
			return PPPAdMobDockType.Custom;
		}
		set
		{
		}
#endif
	}
	
	public static PPPAdMobSizeType SizeType
	{
#if UNITY_ANDROID
		get
		{
			return PPPAdMobAndroid.getSizeID();
		}
		set
		{
			PPPAdMobAndroid.setSizeID(value);
		}
#elif UNITY_IPHONE
		get
		{
			return PPPAdMobiOS.getSizeID();
		}
		set
		{
			PPPAdMobiOS.setSizeID(value);
		}
#else
		get
		{
			PPPDebug.Log("PPPAdMob.SizeType is called.This function works on mobile platform only.Please change the build settings.");
			return PPPAdMobSizeType.UnknownSize_or_NoAdView;
		}
		set
		{
		}
#endif
	}
	
	public static bool Visible
	{
#if UNITY_ANDROID
		get{
			return PPPAdMobAndroid.getVisibility();
		}
		set{
			PPPAdMobAndroid.setVisibility(value);
			refreshAD();
		}
#elif UNITY_IPHONE
		get{
			return PPPAdMobiOS.getVisibility();
		}
		set{
			PPPAdMobiOS.setVisibility(value);
			refreshAD();
		}
#else
		get{
			PPPDebug.Log("PPPAdMob.Visible is called.This function works on mobile platform only.Please change the build settings.");
			return false;
		}
		set{
		}
#endif
	}
	
	public static string AdUnitID
	{
#if UNITY_ANDROID
		set {
			PPPAdMobAndroid.setAdUnitID(value);
		}
#elif UNITY_IPHONE
		set {
			PPPAdMobiOS.setAdUnitID(value);
		}
#else
		set{
			PPPDebug.Log("PPPAdMob.AdUnitID is called.This function works on mobile platform only.Please change the build settings.");
		}
#endif
	}
	
	public static int RequestIntervalInSeconds
	{
#if UNITY_ANDROID
		set {
			PPPAdMobAndroid.setRequestInterval(value);
		}
#elif UNITY_IPHONE
		set {
			PPPAdMobiOS.setRequestInterval(value);
		}
#else
		set{
			PPPDebug.Log("PPPAdMob.RequestIntervalInSeconds is called.This function works on mobile platform only.Please change the build settings.");
		}
#endif
	}
	
    #endregion
}
