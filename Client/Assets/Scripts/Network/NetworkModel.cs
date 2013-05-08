using UnityEngine;
using System.Collections;
using System;


#region 解析包的公共模型

/// <summary>
/// 包装模型类
/// </summary>
public class PackageModel
{
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

/// <summary>
/// 包头
/// </summary>
public class Head
{
    public string name;
    public int err_msg;
    public string version;
}

/// <summary>
/// 反馈包
/// </summary>
public struct PkgResponse
{
    public Head head;
    public string value;
}

/// <summary>
/// 反馈结果
/// </summary>
public class Response
{
    public Head head;
    public Texture texture;
    public string value;
}

#endregion

/// <summary>
/// 参数对拼装类
/// </summary>
public class PostParam
{
    string paramString = "";

    /// <summary>
    /// 添加参数对
    /// </summary>
    /// <param name="name">变量名</param>
    /// <param name="value">变量值</param>
    public void AddPair(string name, string value)
    {
        paramString += "&";
        paramString += name;
        paramString += "=";
        paramString += WWW.EscapeURL(value);
        //Debug.Log(WWW.EscapeURL(value));
    }

    public void AddPair(string name, int value)
    {
        paramString += "&";
        paramString += name;
        paramString += "=";
        paramString += WWW.EscapeURL(value.ToString());
    }

    /// <summary>
    /// 获取拼接好的字符串
    /// </summary>
    /// <returns></returns>
    public string GetParamString()
    {
        return paramString;
    }
}

/// <summary>
/// 短连接请求类
/// </summary>
public class PostEvent
{
    string m_UrlString;                                 // 请求地址
    public WWW m_UrlPost;                               // 短连接请求
    public ResponseHandler m_Handler;                   // 回调

    /// <summary>
    /// 构造函数，构造一个短连接请求
    /// </summary>
    /// <param name="url"></param>
    public PostEvent(PostParam param, ResponseHandler handler)
    {
        m_UrlString = NetworkUtil.UrlWithSession + param.GetParamString();
        NetworkUtil.Post(out m_UrlPost, m_UrlString);
        if (handler != null)
        {
            m_Handler += handler;
        }
    }

    /// <summary>
    /// 构造函数，构造一个短连接请求
    /// </summary>
    /// <param name="url"></param>
    public PostEvent(PostParam param, ResponseHandler handler, bool isWithId)
    {
        if (isWithId)
            m_UrlString = NetworkUtil.UrlWithSession + param.GetParamString();
        else
            m_UrlString = NetworkUtil.UrlWithDeviceId + param.GetParamString();

        NetworkUtil.Post(out m_UrlPost, m_UrlString);
        if (handler != null)
        {
            m_Handler += handler;
        }
    }
}
