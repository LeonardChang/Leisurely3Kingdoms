#if UNITY_IPHONE
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Activates and Deactivates (shows and hides) the native UI via native code in UIBinding.m
/// </summary>
public class PPPAdMobiOS
{
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewInit();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewDeinit();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewReinit();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetAdUnitID(string adUnitID);
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetRequestIntervalInSeconds(int intervalInSeconds);
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewUpdateAppearance();
	[DllImport("__Internal")]
	private static extern bool _PPPUnityAdMobViewGetVisibility();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetVisibility(bool paraVisible);
	[DllImport("__Internal")]
	private static extern float _PPPUnityAdMobViewGetPositionX(int anchorType);
	[DllImport("__Internal")]
	private static extern float _PPPUnityAdMobViewGetPositionY(int anchorType);
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetPosition(float x,float y,int anchorType);
	[DllImport("__Internal")]
	private static extern int _PPPUnityAdMobViewGetSizeID();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetSizeID(int sizeId);
	[DllImport("__Internal")]
	private static extern int _PPPUnityAdMobViewGetDockType();
	[DllImport("__Internal")]
	private static extern void _PPPUnityAdMobViewSetDockType(int dockType);

	public static void init()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Init AdView.");
			_PPPUnityAdMobViewInit();
		}
	}

	public static void deinit()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Deinit AdView.");
			_PPPUnityAdMobViewDeinit();
		}
	}

	public static void reinit()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Reinit AdView.");
			_PPPUnityAdMobViewReinit();
		}
	}
	
	public static void setAdUnitID(string adUnitID)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Set AdUnitID.");
			_PPPUnityAdMobViewSetAdUnitID(adUnitID);
		}
	}
	
	public static void setRequestInterval(int intervalInSeconds)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Set Request Interval in Seconds.");
			_PPPUnityAdMobViewSetRequestIntervalInSeconds(intervalInSeconds);
		}
	}

	public static void refreshAD()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("Refresh AdView UI.");
			_PPPUnityAdMobViewUpdateAppearance();
		}
	}
	
	public static bool getVisibility()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("AdMobiOS.getVisibility Called");
			return _PPPUnityAdMobViewGetVisibility();
		}
		return false;
	}
	
	public static void setVisibility(bool isVisible)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_PPPUnityAdMobViewSetVisibility(isVisible);
		}
	}
		
	public static Vector2 getPosition(PPPAdMobAnchorPointType anchorPtType)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			return new Vector2(_PPPUnityAdMobViewGetPositionX((int)anchorPtType),_PPPUnityAdMobViewGetPositionY((int)anchorPtType));
		}
		return new Vector2(0,0);
	}

	public static void setOrigin(Vector2 paraOriginPoint,PPPAdMobAnchorPointType anchorPtType)
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			_PPPUnityAdMobViewSetPosition(paraOriginPoint.x, paraOriginPoint.y, (int)anchorPtType);
		}
	}
		
	public static PPPAdMobDockType Dock
	{
		get
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return (PPPAdMobDockType)_PPPUnityAdMobViewGetDockType();
			}
			return PPPAdMobDockType.Custom;
		}
		set
		{
			if (Application.platform == RuntimePlatform.IPhonePlayer) {
				_PPPUnityAdMobViewSetDockType((int)value);
			}
		}
	}
	
	/*
	 * SizeID:
	 * -1:Unknown size(No AdView)
	 * 0: 320x50
	 * 1: 320x250
	 * 2: 468x60
	 * 3: 728x90
	 * 4: 120x600
	 * 5: 50 pixels tall on an iPhone/iPod UI.90 pixels tall on an iPad UI.
	 * 6: 32 pixels tall on an iPhone/iPod UI.90 pixels tall on an iPad UI.
	 * Other: equals case 0.
	 */
	public static PPPAdMobSizeType getSizeID()
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
            PPPDebug.Log("PPPAdMobiOS.getSizeID is called.This function works on iOS platforms only.");
			return PPPAdMobSizeType.UnknownSize_or_NoAdView;
		}
		return ((PPPAdMobSizeType)(_PPPUnityAdMobViewGetSizeID()));
	}
	
	/*
	 * SizeID:
	 * 0: 320x50
	 * 1: 320x250
	 * 2: 468x60
	 * 3: 728x90
	 * 4: 120x600
	 * 5: 50 pixels tall on an iPhone/iPod UI.90 pixels tall on an iPad UI.
	 * 6: 32 pixels tall on an iPhone/iPod UI.90 pixels tall on an iPad UI.
	 * Other: equals case 0.
	 */
	public static void setSizeID(PPPAdMobSizeType paraSizeID)
	{
		if (Application.platform != RuntimePlatform.IPhonePlayer) {
			PPPDebug.Log("PPPAdMobiOS.setSizeID is called.This function works on iOS platforms only.");
			return;
		}
		_PPPUnityAdMobViewSetSizeID((int)paraSizeID);
	}
}
#endif