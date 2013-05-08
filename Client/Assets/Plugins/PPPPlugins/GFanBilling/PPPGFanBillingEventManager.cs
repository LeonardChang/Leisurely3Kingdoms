using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using PPP.Unity3D.Plugins.Billing.GFan;

namespace PPP.Unity3D.Plugins.Billing {
    
public class PPPGFanBillingEventManager : MonoBehaviour, IPPPGFanBillingEventListener
{
    public static event Action<PPPGFanResultUserWithOrder> onIAPSuccessEvent;
    
    public static event Action<PPPGFanResultUser> onIAPErrorEvent;
	
	public static event Action onIAPErrorUserNotLoggedInEvent;

    public static event Action<PPPGFanResultUser> onGFanChargeSuccessEvent;
    
    public static event Action<PPPGFanResultUser> onGFanChargeErrorEvent;
	
	public static event Action onGFanChargeErrorUserNotLoggedInEvent;


	#region IPPPGFanBillingEventListener

	public void onIAPSuccess( string encodedParameter )
	{
		PPPDebug.Log("onIAPSuccess:"+encodedParameter);
        
        PPPGFanResultUserWithOrder result = JsonMapper.ToObject<PPPGFanResultUserWithOrder>(encodedParameter);
        
        if (onIAPSuccessEvent != null)
            onIAPSuccessEvent(result);
	}
	
	public void onIAPError( string encodedParameter )
	{
		PPPDebug.Log("onIAPError:"+encodedParameter);
        
        PPPGFanResultUser result = JsonMapper.ToObject<PPPGFanResultUser>(encodedParameter);
        
        if (onIAPErrorEvent != null)
            onIAPErrorEvent(result);
	}
	
	public void onIAPErrorUserNotLoggedIn()
	{
		PPPDebug.Log("onIAPErrorUserNotLoggedIn:");

		if( onIAPErrorUserNotLoggedInEvent != null )
			onIAPErrorUserNotLoggedInEvent();
	}
	
	public void onGFanChargeSuccess( string encodedParameter )
	{
		PPPDebug.Log("onGFanChargeSuccess:"+encodedParameter);
        
        PPPGFanResultUser result = JsonMapper.ToObject<PPPGFanResultUser>(encodedParameter);
        
        if (onGFanChargeSuccessEvent != null)
            onGFanChargeSuccessEvent(result);
	}

	public void onGFanChargeError( string encodedParameter )
	{
		PPPDebug.Log("onGFanChargeError:"+encodedParameter);
        
        PPPGFanResultUser result = JsonMapper.ToObject<PPPGFanResultUser>(encodedParameter);
        
        if (onGFanChargeErrorEvent != null)
            onGFanChargeErrorEvent(result);
	}

	public void onGFanChargeErrorUserNotLoggedIn()
	{
		PPPDebug.Log("onGFanChargeErrorUserNotLoggedIn:");

		if( onGFanChargeErrorUserNotLoggedInEvent != null )
			onGFanChargeErrorUserNotLoggedInEvent();
	}

	#endregion
}

}