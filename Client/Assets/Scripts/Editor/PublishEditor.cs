using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.ComponentModel;

public class PublishEditor : EditorWindow
{
    // Add menu named "My Window" to the Window menu
    [MenuItem("Publish/PublishSetting")]
    static void Init()
    {
        PubilshSettingData.Instance.LoadFile();
        PublishEditor wnd = EditorWindow.GetWindow(typeof(PublishEditor)) as PublishEditor;
        wnd.LoadData();
    }

    private bool mIsDebugVersion = true;
    private string mPublishVersion = "12";
    private string mSaveFileVersion = "46";
    private int mSelectedPublisher = 0;
    private int mSelectedMarket = 0;
    private int mSelectedAdmobAD = 2;

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

                return instance;
            }
        }

        private bool mIsDebugVersion = true;
        private string mPublishVersion = "12";
        private string mSaveFileVersion = "46";
        private int mSelectedPublisher = 0;
        private int mSelectedMarket = 0;
        private int mSelectedAdmobAD = 2;

        public bool IsDebugVersion
        {
            get { return mIsDebugVersion; }
            set { mIsDebugVersion = value; }
        }

        public string PublishVersion
        {
            get { return mPublishVersion; }
            set { mPublishVersion = value; }
        }

        public string SaveFileVersion
        {
            get { return mSaveFileVersion; }
            set { mSaveFileVersion = value; }
        }

        public int SelectedPublisher
        {
            get { return mSelectedPublisher; }
            set { mSelectedPublisher = value; }
        }

        public int SelectedMarket
        {
            get { return mSelectedMarket; }
            set { mSelectedMarket = value; }
        }

        public int SelectedAdmobAD
        {
            get { return mSelectedAdmobAD; }
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

            LoadFile();
        }

        public bool SaveFile()
        {
            Debug.Log("Saveing Publish Setting...");

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
                AssetDatabase.ImportAsset("Assets/Resources/PublishSetting.xml");

                {
                    string definepath = Application.dataPath + "/Scripts/GlobalModule.cs";
                    StreamReader reader = new StreamReader(definepath, System.Text.Encoding.Unicode);
                    string line1 = reader.ReadLine();
                    string line2 = reader.ReadLine();
                    string line3 = reader.ReadToEnd();
                    reader.Close();

                    switch ((Publisher)SelectedPublisher)
                    {
                        case Publisher.Manloo_MM:
                            line2 = "#define MANLOO_VERSION";
                            break;
                        case Publisher.Mobage_CN:
                            line2 = "#define MOBAGE_VERSION";
                            break;
                        case Publisher.Mobage_TW:
                            line2 = "#define MOBAGE_TW_VERSION";
                            break;
                        case Publisher.PlayPlusPlus:
                            line2 = "#define PPP_VERSION";
                            break;
                        case Publisher.Manloo_G:
                            line2 = "#define MANLOO_G10086_VERSION";
                            break;
                        case Publisher.Manloo_GLOBAL:
                            line2 = "#define MANLOO_GLOBAL_VERSION";
                            break;

                        case Publisher.BBG_JP:
                            line2 = "#define BBG_JP_VERSION";
                            break;
                        case Publisher.BBG_JP_Yahoo:
                            line2 = "#define BBG_JP_YAHOO_VERSION";
                            break;
                        case Publisher.BBG_JP_Samsung:
                            line2 = "#define BBG_JP_SAMSUNG_VERSION";
                            break;
                        case Publisher.BBG_JP_Amazon:
                            line2 = "#define BBG_JP_AMAZON_VERSION";
                            break;
                        case Publisher.BBG_JP_Au:
                            line2 = "#define BBG_JP_AUMARKET_VERSION";
                            break;
                    }

                    StreamWriter writer = new StreamWriter(definepath, false, System.Text.Encoding.Unicode);
                    writer.WriteLine(line1);
                    writer.WriteLine(line2);
                    writer.Write(line3);
                    writer.Flush();
                    writer.Close();

                    AssetDatabase.ImportAsset("Assets/Scripts/GlobalModule.cs");
                }
                
                return true;
            }

            return false;
        }

        public bool LoadFile()
        {
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

    public void LoadData()
    {
        PubilshSettingData data = PubilshSettingData.Instance;
        data.LoadFile();

        mIsDebugVersion = data.IsDebugVersion;
        mPublishVersion = data.PublishVersion;
        mSaveFileVersion = data.SaveFileVersion;
        mSelectedPublisher = data.SelectedPublisher;
        mSelectedMarket = data.SelectedMarket;
        mSelectedAdmobAD = data.SelectedAdmobAD;
    }

    void OnGUI()
    {
        BeginWindows();

        GUILayout.Label("Debug", EditorStyles.boldLabel);
        mIsDebugVersion = GUILayout.Toggle(mIsDebugVersion, "Is Debug Version?");

        GUILayout.Space(20);

        GUILayout.Label("Version", EditorStyles.boldLabel);
        mPublishVersion = GUILayout.TextField(mPublishVersion, 2);

        GUILayout.Space(20);

        GUILayout.Label("Save File Version", EditorStyles.boldLabel);
        mSaveFileVersion = GUILayout.TextField(mSaveFileVersion, 3);

        GUILayout.Space(20);

        GUILayout.Label("Publisher", EditorStyles.boldLabel);
        string[] sel = new string[(int)Publisher.Max];
        for (int i = 0; i < (int)Publisher.Max; i++ )
        {
            sel[i] = GetEnumDescription((Publisher)i);
        }
        mSelectedPublisher = GUILayout.SelectionGrid(mSelectedPublisher, sel, 2);

        GUILayout.Space(20);

        GUILayout.Label("Market", EditorStyles.boldLabel);
        sel = new string[(int)Market.Max];
        for (int i = 0; i < (int)Market.Max; i++)
        {
            sel[i] = ((Market)i).ToString();
        }
        mSelectedMarket = GUILayout.SelectionGrid(mSelectedMarket, sel, 2);

        GUILayout.Space(20);

        GUILayout.Label("Admob", EditorStyles.boldLabel);
        sel = new string[]{"Open", "Close", "Server Setting"};
        mSelectedAdmobAD = GUILayout.SelectionGrid(mSelectedAdmobAD, sel, 2);

        GUILayout.Space(20);
        if (GUILayout.Button("Apply"))
        {
            ApplySetting();
        }

        EndWindows();
    }

    private void OnSelectionChange()
    {
        Repaint();
    }

    private void ApplySetting()
    {
        PubilshSettingData data = PubilshSettingData.Instance;

        data.IsDebugVersion = mIsDebugVersion;
        data.PublishVersion = mPublishVersion;
        data.SaveFileVersion = mSaveFileVersion;
        data.SelectedPublisher = mSelectedPublisher;
        data.SelectedMarket = mSelectedMarket;
        data.SelectedAdmobAD = mSelectedAdmobAD;

        data.SaveFile();
    }

    /// <summary>
    /// 获取枚举类子项描述信息
    /// </summary>
    /// <param name="enumSubitem">枚举类子项</param>        
    private static string GetEnumDescription(Enum enumSubitem)
    {
        string strValue = enumSubitem.ToString();

        System.Reflection.FieldInfo fieldinfo = enumSubitem.GetType().GetField(strValue);
        System.Object[] objs = fieldinfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (objs == null || objs.Length == 0)
        {
            return strValue;
        }
        else
        {
            DescriptionAttribute da = (DescriptionAttribute)objs[0];
            return da.Description;
        }

    }
}