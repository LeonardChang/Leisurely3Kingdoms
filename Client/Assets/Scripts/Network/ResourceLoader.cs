using UnityEngine;
using System.Collections.Generic;
using System.IO;

public delegate void ResourceHandler(Texture2D t2d);

/// <summary>
/// 贴图类型
/// </summary>
public enum TextureType
{
    Building,
    City,
    Other,
};

/// <summary>
/// 加载过程
/// </summary>
public struct LoadingProcess
{
    public WWW www;                                 // 请求类
    public bool isLocal;                            // 是否本地加载
    public string filename;                         // 文件名
}

/// <summary>
/// 加载资源
/// </summary>
public static class ResourceLoader
{
    public static WWW m_GetVersionWWW;                                  // 获取列表的请求
    public static List<LoadingProcess> m_ProcessList;                   // 加载资源任务
    public static int m_TotalProcess = 0;                               // 本次任务总的进程数，以便于计算百分比

    /// <summary>
    /// 初始化
    /// </summary>
    public static void Init()
    {
        m_ProcessList = new List<LoadingProcess>();
        m_TotalProcess = 0;
    }


    /// <summary>
    /// 更新加载进程
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
    /// 加载进程检测
    /// </summary>
    public static void LoadingProcessCheck()
    {
        LoadImageHandler(m_ProcessList[0]);
    }

    /// <summary>
    /// 加载图片处理
    /// </summary>
    public static void LoadImageHandler(LoadingProcess pro)
    {
        // 判断当前正在下载的图片是否下载完成
        if (pro.www.isDone)
        {
            m_ProcessList.RemoveAt(0);
            if (pro.www.texture != null)
            {
                // 只有从服务器拽取的时候才保存到本地
                if (!pro.isLocal)
                {
                    SaveImage(pro.www.texture, pro.filename);
                }
            }
        }
    }

    /// <summary>
    /// 加载图片
    /// </summary>
    public static void LoadImage(string name)
    {
        LoadingProcess lp = new LoadingProcess();
        // 从本地读取
        if (File.Exists(Application.persistentDataPath + "/" + name))
        {
            string localPath = "file:/" + Application.persistentDataPath + "/" + name;
            lp.www = new WWW(localPath);
            lp.isLocal = true;
        }
        // 从服务器拉取
        else
        {
            Debug.Log("Load resource from server");
            lp.www = new WWW(NetworkUtil.ServerIp + name);
            lp.isLocal = false;
        }
        m_ProcessList.Add(lp);
    }

    /// <summary>
    /// 保存图片
    /// </summary>
    public static void SaveImage(Texture2D t2d , string relFile)
    {
        Debug.Log(relFile);

        // 拆分出路径
        /* 形如 "patha/pathb/file.a"  需要拆分出"patha/pathb" 以判断是否需要创建路径 ， 然后再在该路径下保存"file.a"文件 */

        string relPath;
        string[] dir = relFile.Split('/');
        relPath = "";
        for (int i = 0; i < dir.Length - 1; i++)
        {
            relPath += dir[i] + '/';
        }

        string path = Application.persistentDataPath + "/" + relPath;
        string filePath = Application.persistentDataPath + "/" + relFile;

        // 判断路径是否存在，不存在则创建
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        // 转成二进制数据，并写入文件
        byte[] tempByte = t2d.EncodeToPNG();
        File.WriteAllBytes(filePath, tempByte);
    }
}