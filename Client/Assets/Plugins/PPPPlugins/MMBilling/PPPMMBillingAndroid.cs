using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID

public class PPPMMBillingAndroid
{
	private static AndroidJavaClass _pluginClass;
	//private static AndroidJavaObject _plugin;
		
	static PPPMMBillingAndroid()
	{
		if( Application.platform != RuntimePlatform.Android )
			return;
		
		// find the plugin class
		_pluginClass = new AndroidJavaClass( "com.ppp.mmbilling.PPPMMBillingPlugin" );
		
		// find the plugin instance
		//using( var _pluginClass = new AndroidJavaClass( "com.ppp.mmbilling.PPPMMBillingPlugin" ) )
			//_plugin = _pluginClass.CallStatic<AndroidJavaObject>( "instance" );
	}
	
	// 初始化 //
	public static void init(string appId, string appKey)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.Init is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "init", appId, appKey );
	}
	
	// 购买单个非可续订物品 //
	public static void order(string paycode)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.order1 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "order", paycode );
	}
	
	// 购买多个非可续订物品 //
	public static void order(string paycode, int orderCount)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.order2 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "order", paycode, orderCount );
	}
	
	// 购买多个可续订物品 //
	public static void order(string paycode, int orderCount, bool autoRenew)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.order3 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "order", paycode, orderCount, autoRenew );
	}
	
	// 查询商品是否已经订购 //
	public static void query(string paycode)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.query1 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "query", paycode );
	}
	
	// 根据订单号查询商品是否已经订购，须指定交易ID。这个接口可以通过交易ID查询过去已经订购的商品 //
	public static void query(string paycode, string tradeId)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.query2 is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "query", paycode, tradeId );
	}
	
	// 获取SDK版本 //
	public static string getSDKVersion()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.getSDKVersion is called.This function works on Android platforms only.");
			return "";
		}
		return _pluginClass.CallStatic<string>( "getSDKVersion" );
	}
	
	// 设置连接超时和数据传输的超时 //
	public static void setTimeout(int connTimeout, int dataTimeout)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.setTimeout is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "setTimeout", connTimeout, dataTimeout );
	}
	
	// 是否由SDK缓存授权文件(建议只对于单词永久有效的计费点采用) //
	public static void enableCache(bool enable)
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.enableCache is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "enableCache", enable );
	}
	
	// 清除SDK缓存授权文件 //
	public static void clearCache()
	{
		if( Application.platform != RuntimePlatform.Android )
		{
			PPPDebug.Log("PPPMMBillingAndroid.clearCache is called.This function works on Android platforms only.");
			return;
		}
		_pluginClass.CallStatic( "clearCache" );
	}
}
#endif
