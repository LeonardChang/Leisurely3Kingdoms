using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class KOZNet : MonoBehaviour
{
    #region 参数
    /// <summary>
    /// 是否是Debug版本
    /// 正式发布时请改为false
    /// </summary>
    public static bool IsDebugVersion
    {
        get
        {
            System.DateTime date = System.DateTime.Now;
            if (date.Year == 2013 && date.Month <= 3)
            {
                return PubilshSettingData.Instance.IsDebugVersion;
            }

            return false;
        }
    }

    /// <summary>
    /// 游戏子版本号
    /// 每次更新请提升该数字
    /// 该版本号与更新日志对应
    /// </summary>
    private static int SubVersion
    {
        get
        {
            return int.Parse(PubilshSettingData.Instance.PublishVersion);
        }
    }

    /// <summary>
    /// 服务器地址
    /// </summary>
    public static string IP
    {
        get
        {
            return "stats.aoide.playplusplus.com";
            //return "192.168.10.171:90"; 
        }
    }

    /// <summary>
    /// 游戏ID
    /// 此ID在服务端定义，不会改变
    /// </summary>
    public static int GameID
    {
        get { return 6; }
    }

    /// <summary>
    /// 设备ID
    /// 设备ID在第一次与服务器通讯时由服务器返回
    /// 在尚未获取设备ID时，提交数据ID填写"0"
    /// 每个客户端的ID全球唯一
    /// </summary>
    public string DeviceID
    {
        get
        {
            if (PlayerPrefs.HasKey("Devive"))
            {
                return PlayerPrefs.GetString("Devive");
            }
            return "0";
        }

        set
        {
            PlayerPrefs.SetString("Devive", value);
            PlayerPrefs.Save();
        }
    }

    /// <summary>
    /// 设备版本，根据平台自动获取
    /// </summary>
#if UNITY_IPHONE
    private int IOSVersion
    {
        get
        {
            Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
            int val = 0;

            switch (publisherID)
            {
                case Publisher.Mobage_CN:
                    val = 52000;
                    break;
                case Publisher.Mobage_TW:
                    val = 53000;
                    break;
                case Publisher.PlayPlusPlus:
                    val = 71000;
                    break;
                case Publisher.BBG_JP:
                    val = 25000;
                    break;
                default:
                    val = 11000;
                    break;
            }

            return val;
        }
    }
#elif UNITY_ANDROID
    private int AndroidVersion
    {
        get
        {
            //Market marketID = (Market)PubilshSettingData.Instance.SelectedMarket;
            Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;

            int val = 0;
            switch (publisherID)
            {
                case Publisher.Manloo_MM:
                    val = 30000;
                    break;
                case Publisher.Manloo_G:
                    val = 31000;
                    break;
                case Publisher.Manloo_GLOBAL:
                    val = 32000;
                    break;

                case Publisher.PlayPlusPlus:
                    val = 70000;
                    break;

                case Publisher.Mobage_CN:
                    val = 50000;
                    break;
                case Publisher.Mobage_TW:
                    val = 51000;
                    break;

                case Publisher.BBG_JP:
                    val = 20000;
                    break;
                case Publisher.BBG_JP_Yahoo:
                    val = 21000;
                    break;
                case Publisher.BBG_JP_Samsung:
                    val = 22000;
                    break;
                case Publisher.BBG_JP_Amazon:
                    val = 23000;
                    break;
                case Publisher.BBG_JP_Au:
                    val = 24000;
                    break;

                default:
                    val = 10000;
                    break;
            }

            return val;
        }
    }
#else
    private int OtherVersion
    {
        get { return 0; }
    }
#endif

    /// <summary>
    /// 获取完整版本号
    /// </summary>
    public string VersionID
    {
        get
        {
            // Debug版本返回固定版本号
            if (IsDebugVersion)
            {
                return "6000000";
            }

#if UNITY_IPHONE
            int mainVersion = IOSVersion;
#elif UNITY_ANDROID
            int mainVersion = AndroidVersion;
#else
            int mainVersion = OtherVersion;
#endif

            return (GameID * 100000 + mainVersion + SubVersion).ToString();
        }
    }

    /// <summary>
    /// 服务端控制Admob是否开启的网页URL
    /// </summary>
    private string AdmobShowTestURL
    {
        get { return "http://www.playplusplus.com/backend/remoteswitch/koz/admob.html"; }
    }

    #endregion

    /// <summary>
    /// 账户被冻结的事件
    /// 参数为是否被冻结
    /// </summary>
    public static event System.Action<bool> FreezeDeviceEvent;

    /// <summary>
    /// 获取服务器Admob状态结果
    /// </summary>
    public static event System.Action<bool> AdmobShowEvent;

    /// <summary>
    /// 获取公告结果
    /// </summary>
    public static event System.Action<int> BillboardEvent;

    private Dictionary<int, BillboardData> mBillboardList = new Dictionary<int, BillboardData>();
    public BillboardData GetBillboardData(int _id)
    {
        if (mBillboardList.ContainsKey(_id))
        {
            return mBillboardList[_id];
        }
        return null;
    }

    // Use this for initialization
	void Start ()
    {
        InvokeRepeating("Refresh", 1, 1);
	}
	
	// Update is called once per frame
	void Update () 
    {
	}

    void OnEnable()
    {
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    private void Refresh()
    {
        if (!mPosting)
        {
            TryPost();
        }
    }

    private List<string> mPostTempList = new List<string>();

    private bool mPosting = false;

    /// <summary>
    /// 提交一个数据
    /// </summary>
    /// <param name="data">参数表</param>
    public void Post(string[] data, bool _force)
    {
        string url = "&idOperation=" + WWW.EscapeURL(data[0]) + "&idOperationEmun=" + WWW.EscapeURL(data[1]) + "&Version=" + VersionID;
        url = url.Replace(" ", "");
        //url = WWW.EscapeURL(url);

        if (_force)
        {
            StartCoroutine(StartPost("http://" + IP + "/log.php?op=AddLog&idGame=" + GameID.ToString() + "&idDevice=" + DeviceID + url));
        }
        else
        {
            mPostTempList.Add(url);
        }
    }

    public void Post(string[] data)
    {
        Post(data, false);
    }

    /// <summary>
    /// 查询是否有未被post的log，有的话尝试post一条
    /// </summary>
    private bool TryPost()
    {
        if (mPostTempList.Count > 0)
        {
            string url = mPostTempList[0];
            mPostTempList.RemoveAt(0);
            StartCoroutine(StartPost("http://" + IP + "/log.php?op=AddLog&idGame=" + GameID.ToString() + "&idDevice=" + DeviceID + url));

            return true;
        }

        return false;
    }

    /// <summary>
    /// 开始提交数据
    /// </summary>
    /// <param name="_url"></param>
    /// <returns></returns>
    private IEnumerator StartPost(string _url)
    {
        mPosting = true;

        print(">>>>>>>>>>>>>>>>> Net Log");
        print("StartPost: " + _url);

        WWW www = new WWW(_url);
        yield return www;

        if (www.error != null)
        {
            print("FinishPost - error: " + _url);
            print(www.error);
            PlayerPrefs.SetInt("PostErrorTimes", PlayerPrefs.GetInt("PostErrorTimes") + 1);
            PlayerPrefs.Save();
        }
        else
        {
            string result = WWW.UnEscapeURL(www.text).ToString();

            print("FinishPost: " + _url);
            print(result);
            print("<<<<<<<<<<<<<<<<< Net Log");

            XmlDocument doc = new XmlDocument();
            byte[] encodedString = System.Text.Encoding.UTF8.GetBytes(result);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            doc.Load(ms);
            ms.Close();

            XmlNode node = doc.SelectSingleNode("data");
            if (node != null)
            {
                XmlNode subNode = node.SelectSingleNode("err_msg");
                if (subNode != null)
                {
                    int errorID = int.Parse(subNode.InnerText);
                    if (errorID != 0)
                    {
                        print("Server return an error: " + subNode.InnerText);
                    }
                }
                else
                {
                    print("Error: Can't find node <err_msg>");
                }

                {
                    subNode = node.SelectSingleNode("value");
                    if (subNode != null)
                    {
                        XmlNode subsubNode = subNode.SelectSingleNode("idDevice");
                        if (DeviceID == "0")
                        {
                            if (subsubNode != null)
                            {
                                DeviceID = subsubNode.InnerText;
                                print("DeviceID is: " + DeviceID);
                            }
                            else
                            {
                                print("Error: Can't find node <idDevice>");
                            }
                        }
                        
                        subsubNode = subNode.SelectSingleNode("t");
                        if (subsubNode != null)
                        {
                            bool locked = subsubNode.InnerText == "1";
                            if (locked)
                            {
                                print("Error: Account is locked!");
                            }

                            if (FreezeDeviceEvent != null)
                            {
                                FreezeDeviceEvent(locked);
                            }
                        }
                        else
                        {
                            print("Error: Can't find node <idDevice>");
                        }
                    }
                    else
                    {
                        print("Error: Can't find node <value>");
                    }
                }
            }
            else
            {
                print("Error: Can't find node <data>");
            }
        }

        mPosting = false;
    }

    /// <summary>
    /// 链接到跳转页
    /// </summary>
    public void LinkToPublishPage ()
	{
        Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
        string url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600001";

        switch (publisherID)
        {
            case Publisher.Manloo_MM:
                url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600001";
                break;
            case Publisher.Manloo_G:
                url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600004";
                break;
            case Publisher.Manloo_GLOBAL:
                url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600001";
                break;
            case Publisher.PlayPlusPlus:
                url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600002";
                break;
            case Publisher.Mobage_CN:
            case Publisher.Mobage_TW:
                url = "http://" + IP + "/Redirect.aspx?idGame=" + GameID.ToString() + "&Version=600003";
                break;
        }

#if UNITY_IPHONE || UNITY_ANDROID
        PPPMiscFeature.showWebControllerWithUrl(url, true);
#else
        Application.OpenURL(url);
#endif
    }

    /// <summary>
    /// 链接到分享页
    /// </summary>
    public void LinkToSharePage(int _id)
    {
        Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
        string url = _id == 0 ? "http://www.playplusplus.com/share/koz1.html" : "http://www.playplusplus.com/share/koz2.html";

        switch (publisherID)
        {
            case Publisher.Manloo_MM:
                url = _id == 0 ? "http://www.playplusplus.com/share/koz1.html" : "http://www.playplusplus.com/share/koz2.html";
                break;
            case Publisher.Manloo_G:
                url = _id == 0 ? "http://www.playplusplus.com/share/koz1_g.html" : "http://www.playplusplus.com/share/koz2_g.html";
                break;
            case Publisher.Manloo_GLOBAL:
                url = _id == 0 ? "http://www.playplusplus.com/share/koz1.html" : "http://www.playplusplus.com/share/koz2.html";
                break;
            case Publisher.Mobage_CN:
                url = _id == 0 ? "http://www.playplusplus.com/share/koz1_m.html" : "http://www.playplusplus.com/share/koz2_m.html";
                break;
            case Publisher.Mobage_TW:
                url = _id == 0 ? "http://www.playplusplus.com/share/koz1_m_hk.html" : "http://www.playplusplus.com/share/koz2_m_hk.html";
                break;
        }

#if UNITY_IPHONE || UNITY_ANDROID
        PPPMiscFeature.showWebControllerWithUrl(url, true);
#else
        Application.OpenURL(url);
#endif
    }

    /// <summary>
    /// 获取是否允许打开广告
    /// </summary>
    public void GetAdmob()
    {
        StartCoroutine(StartGetAdmob(AdmobShowTestURL));
    }

    /// <summary>
    /// 开始获取广告的协程
    /// </summary>
    /// <param name="_url"></param>
    /// <returns></returns>
    private IEnumerator StartGetAdmob(string _url)
    {
        WWW www = new WWW(_url);
        yield return www;

        if (www.error != null)
        {
            print(www.error);

            if (AdmobShowEvent != null)
            {
                AdmobShowEvent(false);
            }
            print("Not allow show Admob");
        }
        else
        {
            string result = WWW.UnEscapeURL(www.text).ToString();

            XmlDocument doc = new XmlDocument();
            byte[] encodedString = System.Text.Encoding.UTF8.GetBytes(result);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            doc.Load(ms);
            ms.Close();

            XmlNode node = doc.SelectSingleNode("value");
            if (node != null)
            {
                if (node.InnerText == "1")
                {
                    if (AdmobShowEvent != null)
                    {
                        AdmobShowEvent(true);
                    }
                    print("Allow show Admob");
                }
                else
                {
                    if (AdmobShowEvent != null)
                    {
                        AdmobShowEvent(false);
                    }
                    print("Not allow show Admob");
                }
            }
            else
            {
                if (AdmobShowEvent != null)
                {
                    AdmobShowEvent(false);
                }
                print("Not allow show Admob");
            }
        }
    }

    /// <summary>
    /// 获取服务器公告
    /// </summary>
    public void GetBillboard()
    {
        Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
        if (publisherID == Publisher.Mobage_CN || publisherID == Publisher.Mobage_TW)
        {
            StartCoroutine(StartGetBillboard("http://www.playplusplus.com/adv/adv_m.xml"));
        }
        else
        {
            StartCoroutine(StartGetBillboard("http://www.playplusplus.com/adv/adv.xml"));
        }
    }

    /// <summary>
    /// 开始获取公告
    /// </summary>
    /// <param name="_url"></param>
    /// <returns></returns>
    private IEnumerator StartGetBillboard(string _url)
    {
        WWW www = new WWW(_url);
        yield return www;

        if (www.error != null)
        {
            print(www.error);
        }
        else
        {
            string result = WWW.UnEscapeURL(www.text).ToString();
            UnityEngine.Debug.Log(result);

            XmlDocument doc = new XmlDocument();
            byte[] encodedString = System.Text.Encoding.UTF8.GetBytes(result);
            MemoryStream ms = new MemoryStream(encodedString);
            ms.Flush();
            ms.Position = 0;
            doc.Load(ms);
            ms.Close();

            try
            {
                XmlNode node = doc.SelectSingleNode("root");
                if (node != null)
                {
                    mBillboardList.Clear();
                    foreach (XmlNode subnode in node.SelectNodes("value"))
                    {
                        XmlNode idNode = subnode.SelectSingleNode("id");
                        XmlNode urlNode = subnode.SelectSingleNode("url");
                        XmlNode imageNode = subnode.SelectSingleNode("image");
                        if (idNode == null || urlNode == null || imageNode == null)
                        {
                            UnityEngine.Debug.LogError("Adv xml file format is wrong: " + _url);
                            continue;
                        }

                        int idStr = int.Parse(idNode.InnerText);
                        string urlStr = urlNode.InnerText;
                        string imageStr = imageNode.InnerText;

                        if (!mBillboardList.ContainsKey(idStr))
                        {
                            BillboardData data = new BillboardData();
                            data.ID = idStr;
                            data.URL = urlStr;
                            data.ImageURL = imageStr;

                            mBillboardList[idStr] = data;
                        }
                    }

                    GetImages();
                }
            }
            catch (System.Exception ex)
            {
                if (ex != null)
                {
                    UnityEngine.Debug.LogError(ex.Message);
                }
            }
        }
    }

    /// <summary>
    /// 获取公告板图片（优先访问本地缓存）
    /// </summary>
    private void GetImages()
    {
        foreach (int id in mBillboardList.Keys)
        {
            if (File.Exists(GameDataCenter.DataFilePath + "/Billboard_" + id.ToString()))
            {
                StartCoroutine(StartGetLocalImages(id));
            }
            else
            {
                StartCoroutine(StartGetImages(id));
            }
        }
    }

    /// <summary>
    /// 开始获取一张服务器的公告板图片
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private IEnumerator StartGetImages(int _id)
    {
        WWW www = new WWW(mBillboardList[_id].ImageURL);
        yield return www;

        if (www.error != null)
        {
            print(www.error);
        }
        else
        {
            if (www.texture != null)
            {
                mBillboardList[_id].Texture = www.texture;

                byte[] tempByte = www.texture.EncodeToPNG();
                File.WriteAllBytes(GameDataCenter.DataFilePath + "/Billboard_" + _id.ToString() + ".png", tempByte);

                if (BillboardEvent != null)
                {
                    BillboardEvent(_id);
                }
            }
        }
    }

    /// <summary>
    /// 开始获取一张本地的公告板图片
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    private IEnumerator StartGetLocalImages(int _id)
    {
        WWW www = new WWW("file:/" + GameDataCenter.DataFilePath + "\\Billboard_" + _id.ToString());
        yield return www;

        if (www.error != null)
        {
            print(www.error);
        }
        else
        {
            if (www.texture != null)
            {
                mBillboardList[_id].Texture = www.texture;

                if (BillboardEvent != null)
                {
                    BillboardEvent(_id);
                }
            }
        }
    }
}

public class BillboardData
{
    private int mID = 0;
    private string mURL = "";
    private string mImageURL = "";
    private Texture2D mTexture = null;

    public int ID
    {
        get { return mID; }
        set { mID = value; }
    }
    public string URL
    {
        get { return mURL; }
        set { mURL = value; }
    }
    public string ImageURL
    {
        get { return mImageURL; }
        set { mImageURL = value; }
    }
    public Texture2D Texture
    {
        get { return mTexture; }
        set { mTexture = value; }
    }
}