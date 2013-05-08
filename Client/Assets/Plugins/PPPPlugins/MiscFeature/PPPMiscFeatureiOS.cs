using UnityEngine;
using System.Runtime.InteropServices;

#if UNITY_IPHONE

public enum PPPiOS_PhotoPromptType
{
	CameraAndAlbum = 0,
	Camera = 1,
	Album = 2,
};

/// <summary>
/// Activates and Deactivates (shows and hides) the native UI via native code in UIBinding.m
/// </summary>
public class PPPMiscFeatureiOS
{
	#region Camera and Photo Library
	
//	[DllImport("__Internal")]
//	private static extern void _PPPUnityMiscFeature_promptForPhoto();
//
//	public static void promptForPhoto()
//	{
//		if (Application.platform == RuntimePlatform.IPhonePlayer) {
//			PPPDebug.Log("PPPMiscFeatureiOS.promptForPhoto is called.");
//			_PPPUnityMiscFeature_promptForPhoto();
//		}
//		else
//		{
//			PPPDebug.Log("PPPMiscFeatureiOS.promptForPhoto is called.This function works on iOS platforms only.");
//		}
//	}

	[DllImport("__Internal")]
//	private static extern void _PPPUnityMiscFeature_promptForPhotoWithType( float scaledToSize, int promptType );
	private static extern void _PPPUnityMiscFeature_promptForPhotoWithType( int promptType, int targetWidth, int targetHeight, int resizeDirection, int resizeAspectType );

	public static void promptForPhoto( int targetWidth, int targetHeight, int resizeDirection, int resizeAspectType )
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("PPPMiscFeatureiOS.promptToTakePhoto is called.");
			_PPPUnityMiscFeature_promptForPhotoWithType((int)PPPiOS_PhotoPromptType.Camera, targetWidth, targetHeight, resizeDirection, resizeAspectType);
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.promptToTakePhoto is called.This function works on iOS platforms only.");
		}
	}

	public static void promptForPictureFromAlbum( int targetWidth, int targetHeight, int resizeDirection, int resizeAspectType )
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("PPPMiscFeatureiOS.promptForPictureFromAlbum is called.");
			_PPPUnityMiscFeature_promptForPhotoWithType((int)PPPiOS_PhotoPromptType.Album, targetWidth, targetHeight, resizeDirection, resizeAspectType);
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.promptForPictureFromAlbum is called.This function works on iOS platforms only.");
		}
	}
	#endregion
	
	#region ActivityView/Progress Dialog
	
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_HideActivityView();
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_ShowActivityView();
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_ShowActivityViewWithLabel( string label );
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_ShowBezelActivityViewWithLabel( string label );
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_ShowBezelActivityViewWithImage( string label, string imagePath );
	
	public static void hideActivityInProgressNotification()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("PPPMiscFeatureiOS.hideActivityInProgressNotification is called.This function works on iOS platforms only.");
			return;
		}
		
		_PPPUnityMiscFeature_HideActivityView();
	}

	public static void showActivityInProgressNotification()
	{
		showActivityInProgressNotification(null);
	}

	public static void showActivityInProgressNotification(string label)
	{
		showActivityInProgressNotification(label, false);
	}
	public static void showActivityInProgressNotification(string label, bool isBezelStyle)
	{
		showActivityInProgressNotification(label, isBezelStyle, null);
	}

	public static void showActivityInProgressNotification(string label, bool isBezelStyle, string imagePath)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("PPPMiscFeatureiOS.showActivityInProgressNotification is called.This function works on iOS platforms only.");
			return;
		}
		
		if (label == null)
		{
			_PPPUnityMiscFeature_ShowActivityView();
		}
		else
		{
			// force bezel style if there is image
			if (imagePath != null)
			{
				isBezelStyle = true;
				_PPPUnityMiscFeature_ShowBezelActivityViewWithImage(label, imagePath);
			}
			else if (isBezelStyle)
			{
				_PPPUnityMiscFeature_ShowBezelActivityViewWithLabel(label);
			}
			else
			{
				_PPPUnityMiscFeature_ShowActivityViewWithLabel(label);
			}
		}
	}
	
	#endregion
	
	#region PPPWebController

	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_ShowWebControllerWithUrl( string url, bool showControls );

	public static void showWebControllerWithUrl( string url, bool showControls)
	{
		if( Application.platform != RuntimePlatform.IPhonePlayer )
		{
			PPPDebug.Log("PPPMiscFeatureiOS.showWebControllerWithUrl is called.This function works on iOS platforms only.");
			return;
		}
		
		_PPPUnityMiscFeature_ShowWebControllerWithUrl( url, showControls );
	}
	
	#endregion
	
	#region Inline web view
	
	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_InlineWebViewShow( int x, int y, int width, int height );

	// Shows the inline web view. Remember, iOS uses points not pixels for positioning and layout!
	public static void inlineWebViewShow( string url, int x, int y, int width, int height )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewShow is called.");
			_PPPUnityMiscFeature_InlineWebViewShow( x, y, width, height );
			inlineWebViewSetUrl(url);
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewShow is called.This function works on iOS platforms only.");
		}
	}

	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_InlineWebViewClose();

	// Closes the inline web view
	public static void inlineWebViewClose()
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewClose is called.");
			_PPPUnityMiscFeature_InlineWebViewClose();
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewClose is called.This function works on iOS platforms only.");
		}
	}

	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_InlineWebViewSetUrl( string url );

	// Sets the current url for the inline web view
	public static void inlineWebViewSetUrl( string url )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewSetUrl is called.");
			_PPPUnityMiscFeature_InlineWebViewSetUrl( url );
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewSetUrl is called.This function works on iOS platforms only.");
		}
	}

	[DllImport("__Internal")]
	private static extern void _PPPUnityMiscFeature_InlineWebViewSetFrame( int x, int y, int width, int height );

	// Sets the current frame for the inline web view
	public static void inlineWebViewSetFrame( int x, int y, int width, int height )
	{
		if( Application.platform == RuntimePlatform.IPhonePlayer )
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewSetFrame is called.");
			_PPPUnityMiscFeature_InlineWebViewSetFrame( x, y, width, height );
		}
		else
		{
			PPPDebug.Log("PPPMiscFeatureiOS.inlineWebViewSetFrame is called.This function works on iOS platforms only.");
		}
	}
	
	#endregion
}
#endif