using UnityEngine;
using System.Collections;

public enum PPPResizeDirection
{
	EnlargeOrReduce = 0,
	EnlargeOnly = 1,
	ReduceOnly = 2,
}

public enum PPPResizeAspectType
{
	Custom = 0,
	KeepAspect_WidthOnly = 1,
	KeepAspect_HeightOnly = 2,
	KeepAspect_WidthOrHeight_Smallest = 3,
	KeepAspect_WidthOrHeight_Largest = 4,
}

public class PPPMiscFeature : MonoBehaviour {
	
	// Make a GameObject in static constructor.
	// Because static constructor is called before any of the other static method in this class.
	// The GameObject will receive Unity Message sent from native plugin.
	// This GameObject is set to be never destroyed on scene/level loading.
	static PPPMiscFeature()
	{
		GameObject _gameobj_miscfeatureevtmgr = GameObject.Find("PPP_PluginObject_MiscFeatureEventManager");
		if (_gameobj_miscfeatureevtmgr == null)
		{
			_gameobj_miscfeatureevtmgr = new GameObject("PPP_PluginObject_MiscFeatureEventManager");
			GameObject.DontDestroyOnLoad(_gameobj_miscfeatureevtmgr);
		}
		Component _component_miscfeatureevtmgr = _gameobj_miscfeatureevtmgr.GetComponent("PPPMiscFeatureEventManager");
		if (_component_miscfeatureevtmgr == null)
		{
			_gameobj_miscfeatureevtmgr.AddComponent("PPPMiscFeatureEventManager");
		}
	}
	
	#region Camera and Photo Library
	/*
	public static void promptForPictureFromAlbum()
	{
		promptForPictureFromAlbum("ppp_unknown.jpg", 512, 512, PPPResizeDirection.EnlargeOrReduce, PPPResizeAspectType.KeepAspect_WidthOnly );
	}
	*/
	
	public static void promptForPictureFromAlbum( int desiredWidth, int desiredHeight, PPPResizeDirection resizeDirection, PPPResizeAspectType resizeAspectType )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.promptForPictureFromAlbum(desiredWidth, desiredHeight,(int)resizeDirection, (int)resizeAspectType);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.promptForPictureFromAlbum( desiredWidth, desiredHeight, (int)resizeDirection, (int)resizeAspectType);
#else
		PPPDebug.Log("PPPMiscFeature.promptForPhoto is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	/*
	public static void promptForPhoto()
	{
		promptForPhoto("ppp_unknown.jpg", 512, 512, PPPResizeDirection.EnlargeOrReduce, PPPResizeAspectType.KeepAspect_WidthOnly );
	}
	*/

	public static void promptForPhoto( int desiredWidth, int desiredHeight, PPPResizeDirection resizeDirection, PPPResizeAspectType resizeAspectType )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.promptForPhoto(desiredWidth, desiredHeight,(int)resizeDirection, (int)resizeAspectType);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.promptToTakePhoto( desiredWidth, desiredHeight, (int)resizeDirection, (int)resizeAspectType );
#else
		PPPDebug.Log("PPPMiscFeature.promptForPhoto is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	public static void promptToTakeVideo( string name )
	{
#if UNITY_IPHONE
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.promptToTakeVideo( name );
#else
		PPPDebug.Log("PPPMiscFeature.promptForPhoto is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	#endregion
	
	#region PPPWebController
	public static void showWebControllerWithUrl( string url, bool showControls)
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.showWebControllerWithUrl(url, showControls);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.showWebControllerWithUrl(url, showControls);
#else
		PPPDebug.Log("PPPMiscFeature.inlineWebViewShow is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	#endregion
	
	#region ActivityView/Progress Dialog
	public static void hideActivityInProgressNotification()
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.hideActivityInProgressNotification();
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.hideActivityInProgressNotification();
#else
		PPPDebug.Log("PPPMiscFeature.hideActivityInProgressNotification is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}

	public static void showActivityInProgressNotification( string captionText, string labelText )
	{
		showActivityInProgressNotification(captionText, labelText, false, null);
	}
	
	public static void showActivityInProgressNotification( string captionText, string labelText, bool isBezelStyle, string imagePath )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.showActivityInProgressNotification(labelText, isBezelStyle, imagePath);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.showActivityInProgressNotification(captionText, labelText);
#else
		PPPDebug.Log("PPPMiscFeature.showActivityInProgressNotification is called.This function works on mobile platforms only.Please change the build settings.");
#endif		
	}
	#endregion
	
	#region ShowAlert
	public static void showAlert( string title, string message, string positiveButtonText)
	{
#if UNITY_IPHONE
		;
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.showAlert(title, message, positiveButtonText);
#else
		PPPDebug.Log("PPPMiscFeature.hideActivityInProgressNotification is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}

	public static void showAlert( string title, string message, string positiveButtonText, string negativeButtonText)
	{
#if UNITY_IPHONE
		;
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.showAlert(title, message, positiveButtonText, negativeButtonText);
#else
		PPPDebug.Log("PPPMiscFeature.hideActivityInProgressNotification is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	#endregion
	
	#region Inline web view
	public static void inlineWebViewShow( string url, int x, int y, int width, int height )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.inlineWebViewShow(url, x, y, width, height);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.inlineWebViewShow(url, x, y, width, height);
#else
		PPPDebug.Log("PPPMiscFeature.inlineWebViewShow is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	public static void inlineWebViewClose()
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.inlineWebViewClose();
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.inlineWebViewClose();
#else
		PPPDebug.Log("PPPMiscFeature.inlineWebViewClose is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	
	public static void inlineWebViewSetUrl( string url )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.inlineWebViewSetUrl(url);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.inlineWebViewSetUrl(url);
#else
		PPPDebug.Log("PPPMiscFeature.inlineWebViewSetUrl is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}

	public static void inlineWebViewSetFrame( int x, int y, int width, int height )
	{
#if UNITY_IPHONE
		PPPMiscFeatureiOS.inlineWebViewSetFrame(x,y,width,height);
#elif UNITY_ANDROID
		PPPMiscFeatureAndroid.inlineWebViewSetFrame(x,y,width,height);
#else
		PPPDebug.Log("PPPMiscFeature.inlineWebViewSetFrame is called.This function works on mobile platforms only.Please change the build settings.");
#endif
	}
	#endregion
}
