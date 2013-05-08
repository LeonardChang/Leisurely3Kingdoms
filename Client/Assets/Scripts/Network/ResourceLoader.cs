using UnityEngine;
using System.Collections.Generic;
using System.IO;

public delegate void ResourceHandler(Texture2D t2d);

/// <summary>
/// ��ͼ����
/// </summary>
public enum TextureType
{
    Building,
    City,
    Other,
};

/// <summary>
/// ���ع���
/// </summary>
public struct LoadingProcess
{
    public WWW www;                                 // ������
    public bool isLocal;                            // �Ƿ񱾵ؼ���
    public string filename;                         // �ļ���
}

/// <summary>
/// ������Դ
/// </summary>
public static class ResourceLoader
{
    public static WWW m_GetVersionWWW;                                  // ��ȡ�б������
    public static List<LoadingProcess> m_ProcessList;                   // ������Դ����
    public static int m_TotalProcess = 0;                               // ���������ܵĽ��������Ա��ڼ���ٷֱ�

    /// <summary>
    /// ��ʼ��
    /// </summary>
    public static void Init()
    {
        m_ProcessList = new List<LoadingProcess>();
        m_TotalProcess = 0;
    }


    /// <summary>
    /// ���¼��ؽ���
    /// </summary>
    public static void UpdateProcess()
    {
        if (m_ProcessList.Count == 0)
        {
            return;
        }
        LoadingProcessCheck();
    }

    /// <summary>
    /// ���ؽ��̼��
    /// </summary>
    public static void LoadingProcessCheck()
    {
        LoadImageHandler(m_ProcessList[0]);
    }

    /// <summary>
    /// ����ͼƬ����
    /// </summary>
    public static void LoadImageHandler(LoadingProcess pro)
    {
        // �жϵ�ǰ�������ص�ͼƬ�Ƿ��������
        if (pro.www.isDone)
        {
            m_ProcessList.RemoveAt(0);
            if (pro.www.texture != null)
            {
                // ֻ�дӷ�����קȡ��ʱ��ű��浽����
                if (!pro.isLocal)
                {
                    SaveImage(pro.www.texture, pro.filename);
                }
            }
        }
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public static void LoadImage(string name)
    {
        LoadingProcess lp = new LoadingProcess();
        // �ӱ��ض�ȡ
        if (File.Exists(Application.persistentDataPath + "/" + name))
        {
            string localPath = "file:/" + Application.persistentDataPath + "/" + name;
            lp.www = new WWW(localPath);
            lp.isLocal = true;
        }
        // �ӷ�������ȡ
        else
        {
            Debug.Log("Load resource from server");
            lp.www = new WWW(NetworkUtil.ServerIp + name);
            lp.isLocal = false;
        }
        m_ProcessList.Add(lp);
    }

    /// <summary>
    /// ����ͼƬ
    /// </summary>
    public static void SaveImage(Texture2D t2d , string relFile)
    {
        Debug.Log(relFile);

        // ��ֳ�·��
        /* ���� "patha/pathb/file.a"  ��Ҫ��ֳ�"patha/pathb" ���ж��Ƿ���Ҫ����·�� �� Ȼ�����ڸ�·���±���"file.a"�ļ� */

        string relPath;
        string[] dir = relFile.Split('/');
        relPath = "";
        for (int i = 0; i < dir.Length - 1; i++)
        {
            relPath += dir[i] + '/';
        }

        string path = Application.persistentDataPath + "/" + relPath;
        string filePath = Application.persistentDataPath + "/" + relFile;

        // �ж�·���Ƿ���ڣ��������򴴽�
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // ת�ɶ��������ݣ���д���ļ�
        byte[] tempByte = t2d.EncodeToPNG();
        File.WriteAllBytes(filePath, tempByte);
    }
}