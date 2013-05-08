using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// 网络工具类
/// </summary>
public class NetworkUtil
{
    //private static string m_ServerIp = "http://192.168.10.171:103/";                              // 网站的根地址
    //private static string m_BaseUrl = "http://192.168.10.171:103/index.php?";                     // 提交数据的页面地址

    //private static string m_ServerIp = "http://192.168.10.74:35585/";                             // 网站的根地址
    //private static string m_BaseUrl = "http://192.168.10.74:35585/index.php?";                    // 提交数据的页面地址

    private static string m_ServerIp = "http://127.0.0.1:35585/";                                   // 网站的根地址
    private static string m_BaseUrl = "http://127.0.0.1:35585/index.php?";                          // 提交数据的页面地址

    /// <summary>
    /// 设置服务器根地址,并自动拼装基本地址以及设备信息
    /// </summary>
    /// <param name="rooturl"></param>
    public static string ServerIp
    {
        set
        {
            m_ServerIp = value;
            m_BaseUrl = m_ServerIp + "/index.php?";
        }
        get
        {
            return m_ServerIp;
        }

    }

    /// <summary>
    /// 页面地址
    /// </summary>
    public static string BaseUrl
    {
        set
        {
            m_BaseUrl = value;
        }
        get
        {
            return m_BaseUrl;
        }
    }

    /// <summary>
    /// 带有UID的地址字符串
    /// </summary>
    public static string UrlWithDeviceId
    {
        get
        {
            return (m_BaseUrl + "idDevice=" + SystemInfo.deviceUniqueIdentifier);
        }
    }

    /// <summary>
    /// 带有账户与session的地址字符串
    /// </summary>
    public static string UrlWithSession
    {
        get
        {
            string resultUrl = m_BaseUrl;
            //resultUrl += "idAccount=" + LocalConfig.accoutId;
            //resultUrl += "&SessionID=" + LocalConfig.sessionId;
            //if (PlayerInfo.characterId > 0)
            //    resultUrl += "&idCharacter=" + PlayerInfo.characterId;
            return resultUrl;
        }
    }

    /// <summary>
    /// 发送请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="text"></param>
    public static void Post(out WWW url, string text)
    {
        Debug.Log(text);
        text = Encryt(text);
        url = new WWW(text);
    }

    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Encryt(string content)
    {
        // 取参数部分进行加密。地址部分不加密
        //string result = content.Substring(0 , content.IndexOf('?') + 1);
        //Debug.Log(result);
        //string contentNeedEncryt = content.Substring(content.IndexOf('?') + 1);
        //Debug.Log(contentNeedEncryt);
        return content;
    }


    /// <summary>
    /// 获取反馈包，分离头与数据结构
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Response GetResponse(string text)
    {
        // 正常情况。返回头以及值数据
        try
        {
            string whiteString = WWW.UnEscapeURL(text);
            Debug.Log(whiteString);
            PkgResponse fb = JsonUtil.UnpackageHead(whiteString);

            Response result = new Response();
            result.head = fb.head;
            if (fb.value != null)
                result.value = fb.value;
            return result;
        }

        // 非正常数据，人工添加头部报错数据
        catch (Exception)
        {
            //Debug.Log(ex);
            Response dummy = new Response();
            if (dummy.head == null)
            {
                dummy.head = new Head();
                dummy.head.err_msg = -100;
                Debug.LogError("No Head");
            }
            dummy.value = WWW.UnEscapeURL(text);
            return dummy;
        }
    }
}