using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;
using PPP.Unity3D.Plugins.Billing.GFan;

namespace PPP.Unity3D.Plugins.Billing {

public class PPPGFanUserCenterEventManager : MonoBehaviour, IPPPGFanUserCenterEventListener
{
    public static event Action<PPPGFanResultUser> onLoginSuccessEvent;
    
    public static event Action<PPPGFanResultUser> onRegisterSuccessEvent;
	
	public static event Action onLoginErrorEvent;

	public static event Action onRegisterErrorEvent;


	#region IPPPGFanUserCenterEventListener

	public void onLoginSuccess( string encodedParameter )
	{
		PPPDebug.Log("onLoginSuccess:"+encodedParameter);
        
        PPPGFanResultUser result = JsonMapper.ToObject<PPPGFanResultUser>(encodedParameter);
        
        if (onLoginSuccessEvent != null)
            onLoginSuccessEvent(result);
	}
	
	public void onRegisterSuccess( string encodedParameter )
	{
		PPPDebug.Log("onRegisterSuccess:"+encodedParameter);
        
        PPPGFanResultUser result = JsonMapper.ToObject<PPPGFanResultUser>(encodedParameter);
        
        if (onRegisterSuccessEvent != null)
            onRegisterSuccessEvent(result);
	}
	
	public void onLoginError()
	{
		PPPDebug.Log("onLoginError:");

		if( onLoginErrorEvent != null )
			onLoginErrorEvent();
	}
	
	public void onRegisterError()
	{
		PPPDebug.Log("onRegisterError:");

		if( onRegisterErrorEvent != null )
			onRegisterErrorEvent();
	}

	#endregion
}

}