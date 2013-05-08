using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// ���繤����
/// </summary>
public class NetworkUtil
{
    //private static string m_ServerIp = "http://192.168.10.171:103/";                              // ��վ�ĸ���ַ
    //private static string m_BaseUrl = "http://192.168.10.171:103/index.php?";                     // �ύ���ݵ�ҳ���ַ

    //private static string m_ServerIp = "http://192.168.10.74:35585/";                             // ��վ�ĸ���ַ
    //private static string m_BaseUrl = "http://192.168.10.74:35585/index.php?";                    // �ύ���ݵ�ҳ���ַ

    private static string m_ServerIp = "http://127.0.0.1:35585/";                                   // ��վ�ĸ���ַ
    private static string m_BaseUrl = "http://127.0.0.1:35585/index.php?";                          // �ύ���ݵ�ҳ���ַ

    /// <summary>
    /// ���÷���������ַ,���Զ�ƴװ������ַ�Լ��豸��Ϣ
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
    /// ҳ���ַ
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
    /// ����UID�ĵ�ַ�ַ���
    /// </summary>
    public static string UrlWithDeviceId
    {
        get
        {
            return (m_BaseUrl + "idDevice=" + SystemInfo.deviceUniqueIdentifier);
        }
    }

    /// <summary>
    /// �����˻���session�ĵ�ַ�ַ���
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
    /// ��������
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
    /// ����
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public static string Encryt(string content)
    {
        // ȡ�������ֽ��м��ܡ���ַ���ֲ�����
        //string result = content.Substring(0 , content.IndexOf('?') + 1);
        //Debug.Log(result);
        //string contentNeedEncryt = content.Substring(content.IndexOf('?') + 1);
        //Debug.Log(contentNeedEncryt);
        return content;
    }


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