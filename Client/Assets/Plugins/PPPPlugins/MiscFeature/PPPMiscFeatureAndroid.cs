using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID

public class PPPMiscFeatureAndroid
{
	private static AndroidJavaObject _plugin;
		
	static PPPMiscFeatureAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin instance
		using( var pluginClass = new AndroidJavaClass( "com.ppp.miscfeature.PPPMiscPlugin" ) )
			_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	#region Camera and Photo Library
	// Prompts the user to take a photo then resizes it to the dimensions passed in
	public static void promptToTakePhoto( int desiredWidth, int desiredHeight, int resizeDirection, int resizeAspectType )
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.promptToTakePhoto is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "promptToTakePhoto", desiredWidth, desiredHeight, resizeDirection, resizeAspectType );
	}

	// Prompts the user to choose an image from the photo album and resizes it to the dimensions passed in
	public static void promptForPictureFromAlbum( int desiredWidth, int desiredHeight, int resizeDirection, int resizeAspectType )
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.promptForPictureFromAlbum is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "promptForPictureFromAlbum", desiredWidth, desiredHeight, resizeDirection, resizeAspectType );
	}
	
	// Prompts the user to take a video and records it saving with the given name (no file extension is needed for the name)
	public static void promptToTakeVideo( string name )
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.promptToTakeVideo is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "promptToTakeVideo", name );
	}
	#endregion
	
	
	#region ActivityView/Progress Dialog
	
	public static void hideActivityInProgressNotification()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.hideActivityInProgressNotification is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call("hideProgressDialog");
	}

	public static void showActivityInProgressNotification(string captionText, string labelText)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.showActivityInProgressNotification is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call("showProgressDialog", captionText, labelText);
	}
	
	#endregion
	
	#region ShowAlert
	public static void showAlert( string title, string message, string positiveButtonText)
	{
		showAlert( title, message, positiveButtonText, string.Empty );
	}

	public static void showAlert( string title, string message, string positiveButtonText, string negativeButtonText)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.showAlert is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call("showAlert", title, message, positiveButtonText, negativeButtonText);
	}
	#endregion
	
	#region PPPWebController
	
	public static void showWebControllerWithUrl( string url, bool showControls )
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.showWebControllerWithUrl is called.This function works on Android platforms only.");
			return;
		}
		
		bool disableTitle = true;
		bool disableBackButton = false;
		_plugin.Call( "showCustomWebView", url, disableTitle, disableBackButton );
	}
	#endregion
	
	#region Inline web view
	
	public static void inlineWebViewShow( string url, int x, int y, int width, int height)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.inlineWebViewShow is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "inlineWebViewShow", url, x, y, width, height );
	}
	
	public static void inlineWebViewClose()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.inlineWebViewClose is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "inlineWebViewClose" );
	}

    public static void inlineWebViewSetUrl( string url )
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.inlineWebViewSetUrl is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "inlineWebViewSetUrl", url );
	}
	
	public static void inlineWebViewSetFrame( int x, int y, int width, int height)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMiscFeatureAndroid.inlineWebViewSetFrame is called.This function works on Android platforms only.");
			return;
		}
		
		_plugin.Call( "inlineWebViewSetFrame", x, y, width, height );
	}
	
	#endregion
}
#endif
