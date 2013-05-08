using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;


public class PPPMMBillingEventManager : MonoBehaviour, IPPPMMBillingEventListener
{
	// 申请安全凭证之后，开发者可在此结束等待
	public static event Action onAfterApplyEvent;
	
	// 下载版权声明完成之后，开发者可在此结束等待
	public static event Action onAfterDownloadEvent;
	
	// 申请安全凭证之前，由于申请安全凭证时间可能较长，开发者可提示用户等待
	public static event Action onBeforeApplyEvent;
	
	// 下载版权声明之前，开发者可提示用户等待
	public static event Action onBeforeDownloadEvent;
    
    public static event Action<PPPMMResultInitFinish> onInitFinishEvent;
    
    public static event Action<PPPMMResultBillingFinish> onBillingFinishEvent;
	
    public static event Action<PPPMMResultQueryFinish> onQueryFinishEvent;

	#region IPPPMMBillingEventListener
	
	public void onAfterApply()
	{
		if( onAfterApplyEvent != null )
			onAfterApplyEvent();
	}

	public void onAfterDownload()
	{
		if( onAfterDownloadEvent != null )
			onAfterDownloadEvent();
	}

	public void onBeforeApply()
	{
		if( onBeforeApplyEvent != null )
			onBeforeApplyEvent();
	}

	public void onBeforeDownload()
	{
		if( onBeforeDownloadEvent != null )
			onBeforeDownloadEvent();
	}
	
	public void onInitFinish( string encodedParameter )
	{
		PPPDebug.Log("onInitFinish:"+encodedParameter);
        
        PPPMMResultInitFinish result = JsonMapper.ToObject<PPPMMResultInitFinish>(encodedParameter);
        
        if (onInitFinishEvent != null)
            onInitFinishEvent(result);
	}
	
	public void onBillingFinish( string encodedParameter )
	{
		PPPDebug.Log("onBillingFinish:"+encodedParameter);
        
        PPPMMResultBillingFinish result = JsonMapper.ToObject<PPPMMResultBillingFinish>(encodedParameter);
        
        if (onBillingFinishEvent != null)
            onBillingFinishEvent(result);
	}
	
	public void onQueryFinish( string encodedParameter )
	{
		PPPDebug.Log("onQueryFinish:"+encodedParameter);

        PPPMMResultQueryFinish result = JsonMapper.ToObject<PPPMMResultQueryFinish>(encodedParameter);
        
        if (onQueryFinishEvent != null)
            onQueryFinishEvent(result);
    }
	
	#endregion
}
