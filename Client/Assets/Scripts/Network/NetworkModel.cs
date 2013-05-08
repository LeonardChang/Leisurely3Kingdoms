using UnityEngine;
using System.Collections;
using System;


#region �������Ĺ���ģ��

/// <summary>
/// ��װģ����
/// </summary>
public class PackageModel
{
    /// <summary>
    /// ��ȡ������������ͷ�����ݽṹ
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Response GetResponse(string text)
    {
        // �������������ͷ�Լ�ֵ����
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

        // ���������ݣ��˹����ͷ����������
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
/// ��ͷ
/// </summary>
public class Head
{
    public string name;
    public int err_msg;
    public string version;
}

/// <summary>
/// ������
/// </summary>
public struct PkgResponse
{
    public Head head;
    public string value;
}

/// <summary>
/// �������
/// </summary>
public class Response
{
    public Head head;
    public Texture texture;
    public string value;
}

#endregion

/// <summary>
/// ������ƴװ��
/// </summary>
public class PostParam
{
    string paramString = "";

    /// <summary>
    /// ��Ӳ�����
    /// </summary>
    /// <param name="name">������</param>
    /// <param name="value">����ֵ</param>
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
    /// ��ȡƴ�Ӻõ��ַ���
    /// </summary>
    /// <returns></returns>
    public string GetParamString()
    {
        return paramString;
    }
}

/// <summary>
/// ������������
/// </summary>
public class PostEvent
{
    string m_UrlString;                                 // �����ַ
    public WWW m_UrlPost;                               // ����������
    public ResponseHandler m_Handler;                   // �ص�

    /// <summary>
    /// ���캯��������һ������������
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
    /// ���캯��������һ������������
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
