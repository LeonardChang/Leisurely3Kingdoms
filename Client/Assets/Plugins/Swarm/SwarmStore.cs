using UnityEngine;
using System.Collections;
using System;

/**
 * 
 * @author SwarmConnect
 * The SwarmStore "Proxy" class masking the Android Java code.
 *
 */
public class SwarmStore : MonoBehaviour {
	
	public static int PURCHASE_FAILED = 0;
	public static int PURCHASE_SUCCEEDED = 1;
	
	#if UNITY_ANDROID
	/**
	 * Reference the SwarmUnityInterface class
	 */
	public static IntPtr swarmUnityInterface = AndroidJNI.FindClass("com/swarmconnect/SwarmUnityInterface");

	/**
	 * Reference the necessary SwarmUnityInterface method
	 */
	public static IntPtr purchaseItemListingMethod = AndroidJNI.GetStaticMethodID(swarmUnityInterface, "purchaseItemListing", "(ILjava/lang/String;)V");
	#endif
	
	private System.Action<int> callbackAction;
		
	/**
	 * Purchase an item based on a listing id number.
	 * 
	 * @param listingId The id number of the item listing of which to purchase.
	 * @param action The action to be performed (delegate to be called) when the callback is complete (the user data is returned).
	 */	
	public static void purchaseItemListing (int listingId, System.Action<int> action) {

		#if UNITY_ANDROID		
		string objName = "SwarmStorePurchaseItemListing."+listingId+"."+DateTime.Now.Ticks;
		GameObject gameObj = new GameObject(objName);
		DontDestroyOnLoad(gameObj);
		SwarmStore component = gameObj.AddComponent<SwarmStore>();
		component.callbackAction = action;
	
		AndroidJavaObject callback = new AndroidJavaObject("java.lang.String", objName);
		
		jvalue[] args = new jvalue[2];
		args[0].i = listingId;
		args[1].l = callback.GetRawObject();		

		AndroidJNI.CallStaticVoidMethod(swarmUnityInterface, purchaseItemListingMethod, args);
		#endif
	}
	
	/**
	 * This callback is called when the item is purchased.
	 * 
	 * @param result Returns "Purchase Successful" if the purchase is successful, returns "Purchase Failed" otherwise
	 */ 
	public void itemPurchased(string statusCode) {

		if (callbackAction != null) {
			
			if (statusCode != null) {
				
				if (statusCode == "SUCCESS") {
					callbackAction(PURCHASE_SUCCEEDED);
				} else {
					callbackAction(PURCHASE_FAILED);
				}
			} 
		}
		
		GameObject.Destroy(this.gameObject);
	}
}