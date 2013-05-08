using UnityEngine;
using System.Collections;
using System.Xml;
using System.IO;

public class PubilshSettingData
{
    private static volatile PubilshSettingData instance;
    private static object syncRoot = new System.Object();

    public static PubilshSettingData Instance
    {
        get
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new PubilshSettingData();
                    }
                }
            }
            instance.LoadFile();
            return instance;
        }
    }

    private bool mIsDebugVersion = true;
    private string mPublishVersion = "12";
    private string mSaveFileVersion = "46";
    private int mSelectedPublisher = 0;
    private int mSelectedMarket = 0;
    private int mSelectedAdmobAD = 2;

    private bool mAlreadyLoad = false;

    /// <summary>
    /// 是否是Debug版本
    /// </summary>
    public bool IsDebugVersion
    {
        get
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mIsDebugVersion;
        }
        set { mIsDebugVersion = value; }
    }

    /// <summary>
    /// 服务端版本号
    /// </summary>
    public string PublishVersion
    {
        get
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mPublishVersion;
        }
        set { mPublishVersion = value; }
    }

    /// <summary>
    /// 存档版本号
    /// </summary>
    public string SaveFileVersion
    {
        get
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mSaveFileVersion;
        }
        set { mSaveFileVersion = value; }
    }

    /// <summary>
    /// 发布商ID
    /// </summary>
    public int SelectedPublisher
    {
        get
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mSelectedPublisher;
        }
        set { mSelectedPublisher = value; }
    }

    /// <summary>
    /// 市场ID
    /// </summary>
    public int SelectedMarket
    {
        get 
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mSelectedMarket;
        }
        set { mSelectedMarket = value; }
    }

    /// <summary>
    /// Admob的启用模式
    /// </summary>
    public int SelectedAdmobAD
    {
        get
        {
            if (!mAlreadyLoad)
            {
                LoadFile();
            }
            return mSelectedAdmobAD; 
        }
        set { mSelectedAdmobAD = value; }
    }

    public PubilshSettingData()
    {
        mIsDebugVersion = true;
        mPublishVersion = "12";
        mSaveFileVersion = "46";
        mSelectedPublisher = 0;
        mSelectedMarket = 0;
        mSelectedAdmobAD = 2;

        mAlreadyLoad = false;

        LoadFile();
    }

    private bool SaveFile()
    {
        if (Application.isEditor)
        {
            string path = Application.dataPath + "/Resources/PublishSetting.xml";

            XmlDocument doc = new XmlDocument();
            XmlNode node = doc.CreateNode(XmlNodeType.XmlDeclaration, "", "");
            doc.AppendChild(node);

            XmlNode root = doc.CreateElement("Settings");
            doc.AppendChild(root);

            XmlNode elem = doc.CreateElement("IsDebugVersion");
            elem.InnerText = IsDebugVersion.ToString();
            root.AppendChild(elem);

            elem = doc.CreateElement("PublishVersion");
            elem.InnerText = PublishVersion.ToString();
            root.AppendChild(elem);

            elem = doc.CreateElement("SaveFileVersion");
            elem.InnerText = SaveFileVersion.ToString();
            root.AppendChild(elem);

            elem = doc.CreateElement("SelectedPublisher");
            elem.InnerText = SelectedPublisher.ToString();
            root.AppendChild(elem);

            elem = doc.CreateElement("SelectedMarket");
            elem.InnerText = SelectedMarket.ToString();
            root.AppendChild(elem);

            elem = doc.CreateElement("SelectedAdmobAD");
            elem.InnerText = SelectedAdmobAD.ToString();
            root.AppendChild(elem);

            doc.Save(path);

            return true;
        }

        return false;
    }

    public bool LoadFile()
    {
        if (mAlreadyLoad)
        {
            return false;
        }

        mAlreadyLoad = true;
        Debug.Log("Loading Publish Setting...");

        TextAsset text = Resources.Load("PublishSetting") as TextAsset;
        if (text == null)
        {
            if (!SaveFile())
            {
                return false;
            }
            else
            {
                text = Resources.Load("PublishSetting") as TextAsset;
            }
        }

        try
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(text.text);

            XmlNode node = doc.SelectSingleNode("Settings");
            IsDebugVersion = bool.Parse(node.SelectSingleNode("IsDebugVersion").InnerText);
            PublishVersion = node.SelectSingleNode("PublishVersion").InnerText;
            SaveFileVersion = node.SelectSingleNode("SaveFileVersion").InnerText;
            SelectedPublisher = int.Parse(node.SelectSingleNode("SelectedPublisher").InnerText);
            SelectedMarket = int.Parse(node.SelectSingleNode("SelectedMarket").InnerText);
            SelectedAdmobAD = int.Parse(node.SelectSingleNode("SelectedAdmobAD").InnerText);
        }
        catch (System.Exception ex)
        {
            Debug.Log(ex.Message);
        }

        return true;
    }
}