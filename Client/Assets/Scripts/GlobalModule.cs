// 用于游戏中的发布商宏动态生成定义
#define MANLOO_VERSION

#if MANLOO_VERSION
    // 漫乐版本的宏定义
    #define MMPAY            // - 移动支付
    #define MMPAY_MLEYUAN    // - 移动购买(M乐园支付点)

    #define SWARM_ACTIVE     // - 激活swarm

#elif MANLOO_G10086_VERSION
    // 漫乐游戏基地版本的宏定义
    #define SWARM_ACTIVE     // - 激活swarm

#elif MANLOO_GLOBAL_VERSION
    // 漫乐通用市场版本
    #define SWARM_ACTIVE     // - 激活swarm

#elif MOBAGE_VERSION
    // 梦宝谷版本的宏定义

#elif MOBAGE_TW_VERSION
    // 梦宝谷繁体版本的宏定义

#elif PPP_VERSION
    // Play++版本的宏定义
    //#define MMPAY            // - 移动支付
    //#define MMPAY_GIANTOWN   // - 移动购买(巨唐支付点)
    //#define GFPAY

    #define SWARM_ACTIVE     // - 激活swarm

#elif BBG_JP_VERSION
    // BBG的宏定义

#elif BBG_JP_YAHOO_VERSION
#elif BBG_JP_SAMSUNG_VERSION
#elif BBG_JP_AMAZON_VERSION
#elif BBG_JP_AUMARKET_VERSION

#else
    // 其他版本的宏定义

#endif

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if GFPAY
using PPP.Unity3D.Plugins.Billing;
#endif

public class GlobalModule : MonoBehaviour {

    private static volatile GlobalModule mInstance = null;
    public static GlobalModule Instance
    {
        get
        {
            if (mInstance == null)
            {
                // 查找是否存在GlobalModule对象，若不存在则创建一个
                GameObject obj = GameObject.Find("GlobalModule");
                if (obj == null)
                {
                    obj = GameObject.Instantiate(GlobalModule.Instance.LoadResource("Prefabs/GlobalModule"), Vector3.zero, Quaternion.identity) as GameObject;
                    obj.name = "GlobalModule";
                    obj.GetComponent<AutoScaleCenter>().NeedReset = true;
                }

                mInstance = obj.GetComponent<GlobalModule>();
            }

            return mInstance;
        }
    }

	public AudioSource BGMPlayer;
	public AudioSource[] SEPlayers;

    public UILabel DebugLabel;
    public UILabel FPSLabel;

    public GameObject MousePerfab;
    public Transform MousePanel;

    public Camera FrontCamera;
    public Camera BackgroundCamera;
    public Camera MaskCamera;

    public UILabel LoadingLabel;
    public UISprite[] LoadingSprites;
    public Collider LoadingCollider;
	
    public MessageBox MSGBox;
    public MessageBoxSpecial MSGBoxSpecial;
    public AchievementUI AchievementNode;

    public GtGamecenter GameCenterMgr;
    public GtGameStore GameStore;

    public GameObject ShowSwarmLoginPanel;
    public UILabel ShowSwarmLoginBtnLabel;
    public UILabel ShowSwarmStartBtnLabel;

    public GameObject WelcomePanel;
	public UISysFontLabel WelcomeSysLabel;

    public Collider FlowerCollider;

    private float mSoundVolume = 1;
    private float mMusicVolume = 1;
    private AsyncOperation mAsyncOperation = null;
    private string[] mLoadingWord = null;
    private bool mAllowEscapeButton = true;
    private bool mShowLinkWWW = false;

    public PerfabsPool Pool;
        
    /// <summary>
    /// 是否允许响应Escape按键
    /// MessageBox、网页等情况下不允许按
    /// </summary>
    public bool AllowEscapeButton
    {
        get 
        { 
            return mAllowEscapeButton;
            //return true;
        }
    }

    #region Event

    /// <summary>
    /// MessageBox按键的委托事件
    /// </summary>
    public static event System.Action<string> alertButtonClickedEvent;

    /// <summary>
    /// 游戏内MessageBox按键的委托事件
    /// </summary>
    public static event System.Action<string> InGameMessageBoxButtonClickedEvent;

    /// <summary>
    /// 购买水晶事件
    /// </summary>
    public static event System.Action<int> AddCrystalEvent;

    #endregion

    #region MonoBehaviour

    private Dictionary<AchievementEnum, int> mSwarmAchievementID = new Dictionary<AchievementEnum, int>();
    private bool mIsFirstRunning = false;
    
    void Awake()
    {
        // 强制读取一下版本配置文件
        PubilshSettingData pd = PubilshSettingData.Instance;

        CheckLanguage();

        InvokeRepeating("CleanAudioPlayer", 1, 1);

        // 不销毁本节点
        DontDestroyOnLoad(gameObject);

        TextAsset asset = GlobalModule.Instance.LoadResource("Data/" + StringTable.GetString(EStringIndex.Path_LoadingText)) as TextAsset;
        string word = asset.text;
        mLoadingWord = word.Split('\n');

        mSwarmAchievementID[AchievementEnum.ZombieArmy] = 5521;          // 僵尸军团
        mSwarmAchievementID[AchievementEnum.GreatFarmer] = 5523;         // 高级农夫
        mSwarmAchievementID[AchievementEnum.Richer] = 5525;              // 大富豪
        mSwarmAchievementID[AchievementEnum.DeathLeader] = 5527;         // 死亡领主
        mSwarmAchievementID[AchievementEnum.King] = 5529;                // 国王
        mSwarmAchievementID[AchievementEnum.Fans] = 5531;                // 爱好者
        mSwarmAchievementID[AchievementEnum.VIP] = 5533;                 // VIP
        mSwarmAchievementID[AchievementEnum.Collector] = 5535;           // 收藏家
        mSwarmAchievementID[AchievementEnum.Mummy] = 5537;               // 木乃伊
        mSwarmAchievementID[AchievementEnum.FireBird] = 5539;            // 朱雀之谜

        // Debug模式，注册一个信息回调事件
        if (KOZNet.IsDebugVersion)
        {
            //Application.RegisterLogCallback(LogCallback);
            //DebugLabel.active = true;
            FPSLabel.gameObject.active = true;
        }
        
        // 显示广告
        InitialAd();
        EnableAd = true;
                        		
        // 第一次启动游戏初始化音量设置
		if (!PlayerPrefs.HasKey("NotFirstGame"))
        {
            mSoundVolume = 1;
            mMusicVolume = 1;
            PlayerPrefs.SetFloat("SoundVolume", mSoundVolume);
            PlayerPrefs.SetFloat("MusicVolume", mMusicVolume);
            PlayerPrefs.SetInt("NotFirstGame", 1);
            PlayerPrefs.SetInt("RatingMe", 0);
            PlayerPrefs.SetInt("OpenGame", 1);
            PlayerPrefs.SetString("LoginName", SystemInfo.deviceName);
            PlayerPrefs.SetInt("PostErrorTimes", 0);
            SaveSetting();

            SendClientMessage(ClientMessageEnum.FirstOpenGame, Screen.width.ToString() + "_" + Screen.height.ToString(), true);
            SendClientMessage(ClientMessageEnum.OpenGame, "1");

            string systemInfomation = SystemInfo.deviceModel + "_" + SystemInfo.operatingSystem + "_" + SystemInfo.graphicsDeviceVersion + "_" + SystemInfo.systemMemorySize.ToString() + "_" + SystemInfo.graphicsMemorySize.ToString() + "_" + SystemInfo.processorType + "_" + SystemInfo.graphicsDeviceName;
            SendClientMessage(ClientMessageEnum.SystemInfomation, systemInfomation);
            print(systemInfomation);

            // 备份存档并删除
            GameDataCenter.DeleteSave();

            mIsFirstRunning = true;
        }
        else
        {
            SoundVolume = PlayerPrefs.GetFloat("SoundVolume");
            MusicVolume = PlayerPrefs.GetFloat("MusicVolume");

            int open_game = PlayerPrefs.GetInt("OpenGame") + 1;
            PlayerPrefs.SetInt("OpenGame",open_game);
            SendClientMessage(ClientMessageEnum.OpenGame, open_game.ToString(), true);

#if SWARM_ACTIVE
            if (PlayerPrefs.HasKey("LoginSwarm") && PlayerPrefs.GetInt("LoginSwarm") == 1)
            {
                LoginSwarm();
            }
#endif

#if UNITY_ANDROID || UNITY_IPHONE
            string curName = "";
            if (PlayerPrefs.HasKey("LoginName"))
            {
                curName = PlayerPrefs.GetString("LoginName");
            }
            ShowWelcome(curName);
#else
            ShowWelcome("");
#endif
        }

        print("VersionID: " + NetComponent.VersionID.ToString());

        // 刷新系统公告
        NetComponent.GetBillboard();
        
        // 载入第一个场景
		LoadScene("Title");
	}

    public bool IsCanClick = true;
    float mClickDelay = 0;
    public void Click()
    {
        mClickDelay = 0.5f;
        IsCanClick = false;
    }

    void CheckLanguage()
    {
        string txt = GlobalStaticData.GetOptionValue("Language");
        switch(txt)
        {
            case "Chinese":
                StringTable.mStringType = ELocalizationTyp.Chinese;
                break;
            case "English":
                StringTable.mStringType = ELocalizationTyp.English;
                break;
            case "Japanese":
                StringTable.mStringType = ELocalizationTyp.Japanese;
                break;
            case "ChineseTw":
                StringTable.mStringType = ELocalizationTyp.ChineseTw;
                break;
            case "All":
                if(Application.systemLanguage == SystemLanguage.Chinese)
                {
                    StringTable.mStringType = ELocalizationTyp.Chinese;
                }
                else if(Application.systemLanguage == SystemLanguage.Japanese)
                {
                    StringTable.mStringType = ELocalizationTyp.Japanese;
                }
                else
                {
                    StringTable.mStringType = ELocalizationTyp.English;
                }
                break;
        }
    }

	// Use this for initialization
	void Start () 
    {
        //Debug.Log("Start");
        Debug.Log("Device ID: " + NetComponent.DeviceID);

		IniBilling();
	}
	
	/// <summary>
	/// Inis the billing.
	/// </summary>
	void IniBilling()
	{
#if UNITY_ANDROID
        if (GlobalStaticData.GetOptionValue("ShopType") == "1")
        {
#if MMPAY
            PPPMMBilling.init(MMAPPID, MMAPPKEY);
#endif

			
#if GFPAY
            PPPGFanBilling.init();
#endif
		}
#endif	
	}

	// Update is called once per frame
	void Update () {
        if(!IsCanClick)
        {
            mClickDelay -= Time.deltaTime;
            if(mClickDelay < 0)
            {
                IsCanClick = true;
            }
        }

        UpdateLoadLevel();

#if UNITY_IPHONE || (UNITY_ANDROID && !UNITY_EDITOR)
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                CreateMousePerfab(touch.position);
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            CreateMousePerfab(Input.mousePosition);
        }
#endif

        if (mShowLinkWWW && Input.GetKeyDown(KeyCode.Escape))
        {
            mAllowEscapeButton = true;
            mShowLinkWWW = false;
        }

#if SWARM_ACTIVE
        if (mStoreLeaderboardColdingTime > 0)
        {
            mStoreLeaderboardColdingTime -= Time.deltaTime;
            if (mStoreLeaderboardColdingTime <= 0)
            {
                mStoreLeaderboardColdingTime = 0;
            }
        }
        else if (mStoreLeaderboardTime > 0)
        {
            mStoreLeaderboardTime -= Time.deltaTime;
            if (mStoreLeaderboardTime <= 0)
            {
                mStoreLeaderboardTime = 0;

                if (Swarm.isEnabled() && Swarm.isInitialized() && Swarm.isLoggedIn())
                {
                    SubmitAndroidLeaderboard();
                }
            }
        }
#endif
    }

    private void UpdateLoadLevel()
    {
        if (mAsyncOperation != null && mAsyncOperation.isDone)
        {
            mAsyncOperation = null;
            MaskCamera.enabled = false;

#if SWARM_ACTIVE
            if (mNeedShowSwarm && mIsFirstRunning)
            {
                mIsFirstRunning = false;
                ShowLoginSwarm = true;
            }
#endif

            LoadingLabel.gameObject.transform.localScale = LoadingLabel.gameObject.transform.localScale;
            TweenScaleEx.Begin(LoadingLabel.gameObject, 1, new Vector3(50, 50, 1), new Vector3(1, 1, 1), 0.5f);

            int rtype = Random.Range(0, 3);
            switch (rtype)
            {
                case 0:
                    {
                        Vector3 ls = new Vector3(200, 200, 1);
                        Vector3 le = new Vector3(0, 0, 0);
                        Vector3 tls = new Vector3(1, 1, 1);
                        foreach (UISprite sp in LoadingSprites)
                        {
                            sp.gameObject.transform.localEulerAngles = le;
                            sp.gameObject.transform.localScale = ls;

                            TweenRotation.Begin(sp.gameObject, 0.5f, Quaternion.Euler(0, 0, 180));
                            TweenScale.Begin(sp.gameObject, 0.5f, tls);
                        }
                    }
                    break;
                case 1:
                    {
                        Vector3 ls = new Vector3(200, 200, 1);
                        Vector3 le = new Vector3(0, 0, 0);
                        Vector3 tls = new Vector3(1, 1, 1);
                        foreach (UISprite sp in LoadingSprites)
                        {
                            sp.gameObject.transform.localEulerAngles = le;
                            sp.gameObject.transform.localScale = ls;

                            TweenScale.Begin(sp.gameObject, 0.5f, tls);
                        }
                    }
                    break;
                default:
                    {
                        Vector3 ls = new Vector3(200, 200, 1);
                        Vector3 le = Vector3.zero;
                        foreach (UISprite sp in LoadingSprites)
                        {
                            sp.gameObject.transform.localScale = ls;
                            sp.gameObject.transform.localEulerAngles = le;
                            TweenRotation.Begin(sp.gameObject, 0.5f, Quaternion.Euler(45, 90, 45));
                        }
                    }
                    break;
            }
            Invoke("EndLoadScene", 1.0f);
        }
    }

    private void QuitButtonClick(string _button)
    {
        alertButtonClickedEvent -= QuitButtonClick;
        if (_button == "Yes")
        {
            Application.Quit();
        }
    }

    private void CreateMousePerfab(Vector2 _position)
    {
        GameObject obj = Instantiate(MousePerfab) as GameObject;
        obj.transform.position = FrontCamera.ScreenToWorldPoint(_position);
        obj.transform.parent = MousePanel;
    }

    void OnEnable ()
    {
#if UNITY_IPHONE
        PPPMiscFeatureEventManager.alertButtonClickedEvent += alertButtonClicked;
        GtGameStore.GtPurchaseSuccessfulEvent += PurchaseSuccessful;
        GtGameStore.GtPurchaseFailedEvent += PurchaseFailed;
        GtGameStore.GtPurchaseCancelledEvent += PurchaseCancelled;
        GtGamecenter.LoginEvent += GameCenterLoginOK;
#elif UNITY_ANDROID
        PPPMiscFeatureEventManager.alertButtonClickedEvent += alertButtonClicked;

        if (GlobalStaticData.GetOptionValue("ShopType") == "1")//中国移动&机锋
        {
#if MMPAY
            PPPMMBillingEventManager.onInitFinishEvent += onInitFinishEvent;
            PPPMMBillingEventManager.onBillingFinishEvent += onBillingFinishEvent;
            PPPMMBillingEventManager.onQueryFinishEvent += onQueryFinishEvent;
#endif

			
#if GFPAY
            PPPGFanBillingEventManager.onIAPSuccessEvent += onGFSuccessEvent;
            PPPGFanBillingEventManager.onIAPErrorEvent += onGFErrorEvent;
            PPPGFanBillingEventManager.onIAPErrorUserNotLoggedInEvent += onGFErrorUserNotLoggedInEvent;
#endif
        }
#endif

		MessageBox.alertButtonClickedEvent += InGameMessageBoxClicked;
	}

    void OnDisable()
    {
#if UNITY_IPHONE
        PPPMiscFeatureEventManager.alertButtonClickedEvent -= alertButtonClicked;
        GtGameStore.GtPurchaseSuccessfulEvent -= PurchaseSuccessful;
        GtGameStore.GtPurchaseFailedEvent -= PurchaseFailed;
        GtGameStore.GtPurchaseCancelledEvent -= PurchaseCancelled;
        GtGamecenter.LoginEvent -= GameCenterLoginOK;
#elif UNITY_ANDROID
        PPPMiscFeatureEventManager.alertButtonClickedEvent -= alertButtonClicked;

        if (GlobalStaticData.GetOptionValue("ShopType") == "1")//中国移动&机锋
        {
#if MMPAY

            PPPMMBillingEventManager.onInitFinishEvent -= onInitFinishEvent;
            PPPMMBillingEventManager.onBillingFinishEvent -= onBillingFinishEvent;
            PPPMMBillingEventManager.onQueryFinishEvent -= onQueryFinishEvent;
#endif

			
#if GFPAY
            PPPGFanBillingEventManager.onIAPSuccessEvent -= onGFSuccessEvent;
            PPPGFanBillingEventManager.onIAPErrorEvent -= onGFErrorEvent;
            PPPGFanBillingEventManager.onIAPErrorUserNotLoggedInEvent -= onGFErrorUserNotLoggedInEvent;
#endif
        }
#endif

        MessageBox.alertButtonClickedEvent -= InGameMessageBoxClicked;
    }

    /// <summary>
    /// Debug消息回调
    /// </summary>
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (string.IsNullOrEmpty(condition))
        {
            return;
        }

        string info = "<" + type.ToString() + ">" + condition;
        switch (type)
        {
            case LogType.Error:
                info = "[FFFF00]" + info;
                break;
            case LogType.Assert:
                info = "[909090]" + info;
                break;
            case LogType.Warning:
                info = "[909090]" + info;
                break;
            case LogType.Log:
                info = "[FFFFFF]" + info;
                break;
            case LogType.Exception:
                info = "[FF0000]" + info;
                break;
        }

        if (DebugLabel.text.Length > 500)
        {
            DebugLabel.text = info;
        }
        else
        {
            DebugLabel.text = info + "\n" + DebugLabel.text;
        }
    }

    #endregion

    #region Pay

    /// <summary>
    /// 僵尸中国移动 APP ID
    /// </summary>
    const string MMAPPID = "300002722890";
    /// <summary>
    /// 僵尸 中国移动 APP KEY
    /// </summary>
    const string MMAPPKEY = "C34C6C118B7EFFF5";
#if MMPAY_MLEYUAN
    /// <summary>
    /// 中国移动 计费点代码(M乐园)
    /// </summary>
    const string MMBUY30 = "30000272289001";
    const string MMBUY55 = "30000272289002";
    const string MMBUY115 = "30000272289003";
    const string MMBUY360 = "30000272289004";
#elif MMPAY_GIANTOWN
	//MMPAY_GIANTOWN
    /// <summary>
    /// 中国移动 计费点代码(巨唐)
    /// </summary>
    const string MMBUY30 = "30000275246901";
    const string MMBUY55 = "30000275246902";
    const string MMBUY115 = "30000275246903";
    const string MMBUY360 = "30000275246904";
#endif
    //当前购买的计费点代码
    string MM_CurrentBuy = "";
    //移动购买回馈码
    const int MM_BUY_SUCCESS = 102;
    const int MM_QUERY_SUCCESS = 101;
    const int MM_INI_SUCCESS = 100;

    //机锋购买所需机锋点数
    const int GFBUY30 = 30;
    const int GFBUY55 = 50;
    const int GFBUY115 = 100;
    const int GFBUY360 = 300;

    public bool mIsGFiniSuccess = false;
#if GFPAY
    void onGFSuccessEvent(PPPGFanResultUserWithOrder eventArg)
    {
        switch(eventArg.price)
		{
		case GFBUY30:
			PurchaseSuccessful((int)AppStoreItemID.BuyMM30Crystal);
			break;
		case GFBUY55:
			PurchaseSuccessful((int)AppStoreItemID.BuyMM55Crystal);
			break;
		case GFBUY115:
			PurchaseSuccessful((int)AppStoreItemID.BuyMM115Crystal);
			break;
		case GFBUY360:
			PurchaseSuccessful((int)AppStoreItemID.BuyMM360Crystal);
			break;
		}
    }
    void onGFErrorEvent(PPPGFanResultUser eventArg)
    {
        ShowInGameMessageBox("提示", "购买失败", "OK");
    }
    void onGFErrorUserNotLoggedInEvent()
    { 
        
    }
#endif

    public bool mIsMMInitSuccess = false;
#if MMPAY
    // MM Billing SDK初始化完成，result中的code是返回的状态码，目前已知的只有100是成功初始化，其他状态码未知。
    private void onInitFinishEvent(PPPMMResultInitFinish result)
    {
        Debug.Log("PPPMMBillingSDK: User level onInitFinishEvent:" + result.code.ToString() + "," + result.devReason + "," + result.usrReason);

        if (result.code == MM_INI_SUCCESS)
        {
            mIsMMInitSuccess = true;
        }
        else
        {
            SendClientMessage(ClientMessageEnum.MMInitError, result.code.ToString());
            
        }
    }

    // 支付完成时的回调
    private void onBillingFinishEvent(PPPMMResultBillingFinish result)
    {
        Debug.Log("PPPMMBillingSDK: User level onBillingFinishEvent:" + result.code.ToString() + "," + result.devReason + "," + result.usrReason + "," + result.leftDay + "," + result.orderID + "," + result.payCode + "," + result.tradeID);

        if(result.code == MM_BUY_SUCCESS)
        {
            //PPPMMBilling.query(result.payCode, result.tradeID);

            
            if (MM_CurrentBuy == MMBUY30)
            {
                PurchaseSuccessful((int)AppStoreItemID.BuyMM30Crystal);
            }
            else if (MM_CurrentBuy == MMBUY55)
            {
                PurchaseSuccessful((int)AppStoreItemID.BuyMM55Crystal);
            }
            else if(MM_CurrentBuy == MMBUY115)
            {
                PurchaseSuccessful((int)AppStoreItemID.BuyMM115Crystal);
            }
            else if(MM_CurrentBuy == MMBUY360)
            {
                PurchaseSuccessful((int)AppStoreItemID.BuyMM360Crystal);
            }
            else
            {
                
            }
            Debug.Log("当前购买物品:" + MM_CurrentBuy);
        }
        else
        {
            ShowInGameMessageBox("提示", "购买失败", "OK");
        }
    }

    // 查询完成时的回调,开发者需要实现此函数告知用户查询结果。如果该商品已经订购，则告知用户已订购的订单号。
    private void onQueryFinishEvent(PPPMMResultQueryFinish result)
    {
        Debug.Log("PPPMMBillingSDK: User level onQueryFinishEvent: " + result.code.ToString() + "," + result.devReason + "," + result.usrReason + "," + result.leftDay + "," + result.orderID + "," + result.payCode + "," + result.tradeID);
    }
#endif

    #endregion

    #region Load Level

    /// <summary>
    /// 载入一个场景(additive)
    /// </summary>
    /// <param name="_scene">Level ID</param>
    public void LoadScene(string _scene)
    {
        LoadScene(_scene, true);
    }

    /// <summary>
    /// 载入一个场景(no additive)
    /// </summary>
    /// <param name="_scene"></param>
    public void LoadSceneN(string _scene)
    {
        LoadScene(_scene, false);
    }

#if SWARM_ACTIVE
    private bool mNeedShowSwarm = false;
#endif

    private void LoadScene(string _scene, bool _additive)
    {
        if (!Application.CanStreamedLevelBeLoaded(_scene))
        {
            print("Can't load level: " + _scene);
            return;
        }

        PlayerPrefs.SetString("TargetScene", _scene);
        if (_scene != "Title")
        {
#if SWARM_ACTIVE
            mNeedShowSwarm = false;
            CloseLoginSwarm();
#endif
        }
        else
        {
#if SWARM_ACTIVE
            mNeedShowSwarm = true;
#endif
        }

        mAsyncOperation = null;

        LoadingLabel.gameObject.active = true;
        LoadingLabel.enabled = true;
        LoadingCollider.enabled = true;
        foreach (UISprite sp in LoadingSprites)
        {
            sp.gameObject.active = true;
        }

        LoadingLabel.text = mLoadingWord[Random.Range(0, mLoadingWord.Length)];

        LoadingLabel.gameObject.transform.localScale = new Vector3(0, 0, 1);
        TweenScaleEx.Begin(LoadingLabel.gameObject, 1, new Vector3(55, 55, 1), new Vector3(50, 50, 1), 0.75f);

        int rtype = Random.Range(0, 3);
        switch (rtype)
        {
            case 0:
                {
                    Vector3 ls = new Vector3(1, 1, 1);
                    Vector3 le = new Vector3(0, 0, 180);
                    Vector3 tls = new Vector3(200, 200, 1);
                    foreach (UISprite sp in LoadingSprites)
                    {
                        sp.gameObject.transform.localEulerAngles = le;
                        sp.gameObject.transform.localScale = ls;

                        TweenRotation.Begin(sp.gameObject, 0.5f, Quaternion.Euler(0, 0, 0));
                        TweenScale.Begin(sp.gameObject, 0.5f, tls);
                    }
                }
                break;
            case 1:
                {
                    Vector3 ls = new Vector3(1, 1, 1);
                    Vector3 le = new Vector3(0, 0, 0);
                    Vector3 tls = new Vector3(200, 200, 1);
                    foreach (UISprite sp in LoadingSprites)
                    {
                        sp.gameObject.transform.localEulerAngles = le;
                        sp.gameObject.transform.localScale = ls;

                        TweenScale.Begin(sp.gameObject, 0.5f, tls);
                    }
                }
                break;
            default:
                {
                    Vector3 ls = new Vector3(200, 200, 1);
                    Vector3 le = new Vector3(45, 90, 45);
                    foreach (UISprite sp in LoadingSprites)
                    {
                        sp.gameObject.transform.localEulerAngles = le;
                        sp.gameObject.transform.localScale = ls;

                        TweenRotation.Begin(sp.gameObject, 0.5f, Quaternion.Euler(0, 0, 0));
                    }
                }
                break;
        }
        
        if (_additive)
        {
            Invoke("BeginLoadSceneAdditive", 1.0f);
        }
        else
        {
            Invoke("BeginLoadScene", 1.0f);
        }
    }
    
    private void BeginLoadSceneAdditive()
    {
        BeginLoadScene(true);
    }

    private void BeginLoadScene()
    {
        BeginLoadScene(false);
    }

    private IEnumerable LoadEmptyScene()
    {
        AsyncOperation ao = Application.LoadLevelAsync("Empty");
        if (ao.isDone)
        {
            yield return ao;
        }
    }
    
    /// <summary>
    /// 开始场景加载
    /// </summary>
    /// <param name="_additive"></param>
    private void BeginLoadScene(bool _additive)
    {
        MaskCamera.enabled = true;

        if (!_additive)
        {
            LoadEmptyScene();
        }
        Resources.UnloadUnusedAssets();

        string targetScene = PlayerPrefs.GetString("TargetScene");
        if (_additive)
        {
            mAsyncOperation = Application.LoadLevelAdditiveAsync(targetScene);
        }
        else
        {
            mAsyncOperation = Application.LoadLevelAsync(targetScene);
        }

        TweenScaleEx tse = LoadingLabel.GetComponent<TweenScaleEx>();
        tse.enabled = true;
        tse.style = UITweener.Style.Loop;
        tse.from = new Vector3(50, 50, 1);
        tse.mid = new Vector3(40, 40, 1);
        tse.to = new Vector3(50, 50, 1);
        tse.Reset();
        tse.Play(true);
    }

    /// <summary>
    /// 结束场景加载
    /// </summary>
    private void EndLoadScene()
    {
        LoadingLabel.gameObject.active = false;

        foreach (UISprite sp in LoadingSprites)
        {
            sp.gameObject.active = false;
        }

        LoadingCollider.enabled = false;
    }

    #endregion

    #region ResourcesManager

    private ResourcesMgr mResourcesMgr = null;

    /// <summary>
    /// 资源管理器
    /// </summary>
    public ResourcesMgr ResourcesManager
    {
        get
        {
            if (mResourcesMgr == null)
            {
                mResourcesMgr = gameObject.GetComponent<ResourcesMgr>();
            }
            return mResourcesMgr;
        }
    }

    /// <summary>
    /// 载入一个资源
    /// </summary>
    /// <param name="_path"></param>
    /// <returns></returns>
    public UnityEngine.Object LoadResource(string _path)
    {
        return ResourcesManager.Load(_path);
    }

    public UnityEngine.Object LoadResource(string _path, System.Type _type)
    {
        return ResourcesManager.Load(_path, _type);
    }

    #endregion

    #region Sound
    
    /// <summary>
    /// 设置SE音量
    /// </summary>
    public float SoundVolume
    {
        get { return mSoundVolume; }
        set
        { 
            if (mSoundVolume != value)
            {
                mSoundVolume = value;
                if (mSoundVolume > 1)
                {
                    mSoundVolume = 1;
                }
                else if (mSoundVolume < 0)
                {
                    mSoundVolume = 0;
                }
                PlayerPrefs.SetFloat("SoundVolume", mSoundVolume);

                foreach (AudioSource SEPlayer in SEPlayers)
                {
                    if (SEPlayer.isPlaying)
                    {
                        SEPlayer.volume = mSoundVolume;
                    }
                }
            }
        }
    }
    
    /// <summary>
    /// 设置BGM音量
    /// </summary>
    public float MusicVolume
    {
        get { return mMusicVolume; }
        set
        { 
            if (mMusicVolume != value)
            {
                mMusicVolume = value;
                if (mMusicVolume > 1)
                {
                    mMusicVolume = 1;
                }
                else if (mMusicVolume < 0)
                {
                    mMusicVolume = 0;
                }
                PlayerPrefs.SetFloat("MusicVolume", mMusicVolume);

                AudioSource BGMPlayer = GameObject.Find("BGM").GetComponent<AudioSource>();
                if (BGMPlayer.isPlaying)
                {
                    BGMPlayer.volume = mMusicVolume;
                }
            }
        }
    }
	
	/// <summary>
    /// 保存设置
    /// </summary>
    public void SaveSetting()
    {
        PlayerPrefs.Save();
    }

    private AudioSource FreeSEPlayer
    {
        get
        {
            foreach (AudioSource source in SEPlayers)
            {
                if (source.clip == null || !source.isPlaying)
                {
                    return source;
                }
            }

            return null;
        }
    }
	
	/// <summary>
    /// 播放SE
    /// </summary>
    /// <param name="_clip"></param>
    /// <returns></returns>
    public bool PlayBGM(AudioClip _clip)
    {
        return PlayBGM(_clip, 1);
    }

    public bool PlayBGM(AudioClip _clip, float _volume)
    {
        return PlayBGM(_clip, _volume, 0);
    }

    public bool PlayBGM(AudioClip _clip, float _volume, float _time)
    {
        AudioSource BGMPlayer = gameObject.GetComponent<AudioSource>();
        if (BGMPlayer.clip != _clip)
        {
            BGMPlayer.clip = _clip;
            BGMPlayer.volume = MusicVolume * _volume;
            BGMPlayer.time = _time;
            BGMPlayer.Play();

            return true;
        }

        return false;
    }

    /// <summary>
    /// 播放BGM
    /// </summary>
    /// <param name="_clip"></param>
    /// <returns></returns>
    public AudioSource PlaySE(AudioClip _clip)
    {
        return PlaySE(_clip, 1);
    }

    public AudioSource PlaySE(AudioClip _clip, float _volume)
    {
        return PlaySE(_clip, _volume, false);
    }

    /// <summary>
    /// 播放音效
    /// 若所有播放器均忙，返回null
    /// </summary>
    /// <param name="_clip"></param>
    /// <param name="_volume"></param>
    /// <param name="_loop"></param>
    /// <returns></returns>
    public AudioSource PlaySE(AudioClip _clip, float _volume, bool _loop)
    {
        AudioSource source = FreeSEPlayer;
        if (source != null)
        {
            if (!source.enabled)
            {
                source.enabled = true;
            }

            source.clip = _clip;
            source.volume = SoundVolume * _volume;
            source.loop = _loop;
            source.pitch = 1;
            source.Play();
            return source;
        }

        return null;
    }

    /// <summary>
    /// 重播音效
    /// 若一个音效没有被播放，则播放
    /// 若该音效已经在播放中，尝试重播他
    /// </summary>
    /// <param name="_clip">音效文件</param>
    /// <param name="_maxCount">允许占用的播放器数最大值，超过该值且无法播放的音效返回null</param>
    /// <param name="_time">已播放了超过这个时间的音效才允许重播</param>
    /// <returns></returns>
    public AudioSource ReplaySE(AudioClip _clip, int _maxCount, float _time)
    {
        int count = 0;
        foreach (AudioSource source in SEPlayers)
        {
            if (source.clip == _clip)
            {
                count++;

                if (source.time >= _time)
                {
                    if (!source.enabled)
                    {
                        source.enabled = true;
                    }
                    source.Play();
                    return source;
                }
            }
        }

        return count >= _maxCount ? null : PlaySE(_clip);
    }

    public AudioSource ReplaySE(AudioClip _clip)
    {
        return ReplaySE(_clip, 1, 0);
    }

    private void CleanAudioPlayer()
    {
        foreach (AudioSource SEPlayer in SEPlayers)
        {
            if (SEPlayer.clip != null && !SEPlayer.loop && !SEPlayer.isPlaying)
            {
                SEPlayer.clip = null;
                SEPlayer.enabled = false;
            }
        }
    }

    #endregion;

    #region Internet

    private KOZNet mNetComponent = null;
    private KOZNet NetComponent
    {
        get
        {
            if (mNetComponent == null)
            {
                mNetComponent = gameObject.GetComponent<KOZNet>();
            }
            return mNetComponent;
        }
    }

    private string LiteMoregames
    {
        get
        {
            return "http://www.playplusplus.com/moregames_m/phone_moregames.html";
        }
    }

    private string Moregames
    {
        get
        {
            return "http://www.playplusplus.com/moregames/moregames.html";
        }
    }

    /// <summary>
    /// 向服务器发送一条统计消息
    /// </summary>
    /// <param name="_key">信息类型</param>
    /// <param name="_value">数据串</param>
    public void SendClientMessage(ClientMessageEnum _key, string _value, bool _force)
    {
#if !UNITY_EDITOR
        string[] data = new string[2];
        data[0] = ((int)_key).ToString();
        data[1] = _value;
        NetComponent.Post(data, _force);
#endif
    }

    public void SendClientMessage(ClientMessageEnum _key, string _value)
    {
        SendClientMessage(_key, _value, false);
    }

    /// <summary>
    /// 连接到某个网站
    /// </summary>
    /// <param name="_type">目标网站</param>
    public void LinkToWWW(LinkToWWWEnum _type)
    {
        switch (_type)
        {
            case LinkToWWWEnum.ProductPage:
                // 链接到更多产品
#if UNITY_IPHONE
                if (iPhone.generation == iPhoneGeneration.iPad1Gen
                    || iPhone.generation == iPhoneGeneration.iPad2Gen
                    || iPhone.generation == iPhoneGeneration.iPad3Gen)
                {
                    PPPMiscFeature.showWebControllerWithUrl(Moregames, true);
                }
                else
                {
                    PPPMiscFeature.showWebControllerWithUrl(LiteMoregames, true);
                }
#elif UNITY_ANDROID
                if (Screen.height / Screen.width <= 4f / 3f + 0.01f)
                {
                    PPPMiscFeature.showWebControllerWithUrl(Moregames, true);
                }
                else
                {
                    PPPMiscFeature.showWebControllerWithUrl(LiteMoregames, true);
                }
#else
			    Application.OpenURL(Moregames);
#endif
                mAllowEscapeButton = false;
                mShowLinkWWW = true;
                break;
            case LinkToWWWEnum.ReleasePage:
                // 链接到发布页
                NetComponent.LinkToPublishPage();
                mAllowEscapeButton = false;
                mShowLinkWWW = true;
                break;
            case LinkToWWWEnum.ShareMe1:
                // 链接到分享
                NetComponent.LinkToSharePage(0);
                mAllowEscapeButton = false;
                mShowLinkWWW = true;
                break;
            case LinkToWWWEnum.ShareMe2:
                // 链接到分享
                NetComponent.LinkToSharePage(1);
                mAllowEscapeButton = false;
                mShowLinkWWW = true;
                break;
        }
    }

    public void LinkToURL(string _url)
    {
#if UNITY_IPHONE
            PPPMiscFeature.showWebControllerWithUrl(_url, true);
#elif UNITY_ANDROID
        PPPMiscFeature.showWebControllerWithUrl(_url, true);
#else
        	Application.OpenURL(Moregames);
#endif
    }

    /// <summary>
    /// 返回设备ID
    /// </summary>
    public string DeviceID
    {
        get 
        {
            return NetComponent.DeviceID;
        }
    }

    #endregion

    #region Achievement and Leaderboard

    /// <summary>
    /// 向Game Center发送排行榜信息
    /// </summary>
    /// <param name="_data">排行榜数据</param>
    public void SendGameCenterLeaderboard(int _index, int _data)
    {
        if (KOZNet.IsDebugVersion)
        {
            return;
        }

#if !UNITY_EDITOR && UNITY_IPHONE
        GameCenterMgr.PostLeaderBoadrd(_index,_data);
#elif !UNITY_EDITOR && UNITY_ANDROID
        if (_data > mTodayGainCoinMax)
        {
            mTodayGainCoinMax = _data;
            mStoreLeaderboardTime = 5;
        }
#endif
    }

#if SWARM_ACTIVE
    private void SubmitAndroidLeaderboard()
    {
        SwarmLeaderboard.submitScore(3715, mTodayGainCoinMax);
        mStoreLeaderboardColdingTime = 30;
    }

    private float mStoreLeaderboardTime = 0;
    private float mStoreLeaderboardColdingTime = 0;

    private int mTodayGainCoinMax = 0;
#endif

    public void ForceClearTodayGainCoin()
    {
#if SWARM_ACTIVE
        mTodayGainCoinMax = 0;
#endif
    }

    /// <summary>
    /// 向Game Center发送成就信息
    /// </summary>
    /// <param name="_achievement">成就ID</param>
    /// <param name="_data">成就数据</param>
    public void SendGameCenterAchievement(AchievementEnum _achievement, float _percent)
    {
        if (KOZNet.IsDebugVersion)
        {
            return;
        }

#if UNITY_IPHONE
        Debug.Log(_achievement.ToString() + ":" + _percent.ToString());
        GameCenterMgr.PostAchievement((int)_achievement, _percent);
#elif SWARM_ACTIVE
        if (Swarm.isEnabled() && Swarm.isInitialized() && Swarm.isLoggedIn() && _percent >= 1.0f)
        {
            SwarmAchievement.unlockAchievement(mSwarmAchievementID[_achievement]);
        }
#endif
    }

    /// <summary>
    /// 打开Game Center Leaderboard
    /// </summary>
    public bool OpenGameCenterLeaderboard()
    {
#if UNITY_IPHONE
        GameCenterMgr.OpenLeaderBoard();
        return true;
#elif SWARM_ACTIVE
        if (!Swarm.isEnabled() || !Swarm.isInitialized() || !Swarm.isLoggedIn())
        {
            LoginSwarm();
        }
        else
        {
            Swarm.showLeaderboards();
        }
        return true;
#else
        return false;
#endif
    }

    /// <summary>
    /// 打开Game Center Achievement
    /// </summary>
    public bool OpenGameCenterAchievement()
    {
#if UNITY_IPHONE
        GameCenterMgr.OpenAchievement();
        return true;
#elif SWARM_ACTIVE
        if (!Swarm.isEnabled() || !Swarm.isInitialized() || !Swarm.isLoggedIn())
        {
            LoginSwarm();
        }
        else
        {
            Swarm.showAchievments();
        }
        return true;
#else
        return false;
#endif
    }

#if UNITY_IPHONE
    private void GameCenterLoginOK()
    {

        PlayerPrefs.SetString("LoginName", GameCenterMgr.GetName());
        SaveSetting();
    }
#endif

    /// <summary>
    /// 显示成就解锁界面
    /// </summary>
    /// <param name="_achievementName"></param>
    public void ShowAchievbementUnlocked(string _achievementName)
    {
#if UNITY_IPHONE
        AchievementNode.ShowAchievement(_achievementName);
#endif
    }

    #endregion

    #region AdMob

    //远端广告开关的未初始化时(或无法连接远端服务器时)的默认值
    bool mRemoteSwitchAdmob = false;

    /// <summary>
    /// 当前版本是否在编译时启用广告组件（彻底屏蔽广告）
    /// </summary>
    private bool isAdMobEnabledInBuild ()
	{
		bool enableAdMobConfig = true;	// Default enable AdMob
		string enableAdMobConfigStr = GlobalStaticData.GetOptionValue ("AdMob");
		
		if (enableAdMobConfigStr != string.Empty) {
			enableAdMobConfig = (1 == int.Parse (enableAdMobConfigStr));
		}
		
		return enableAdMobConfig;
	}
	
	/// <summary>
	/// 获取远端广告内容
	/// </summary>
	private void requestRemoteSwitchAdmob()
	{
        if (PubilshSettingData.Instance.SelectedAdmobAD != 2)
        {
            return;
        }

		// URL:http://www.playplusplus.com/backend/remoteswitch/koz/admob.html
		// TODO:获取远端内容，完毕后调用parseRemoteSwitchAdmob，以设定广告开关
        KOZNet.AdmobShowEvent += GetAdmonResult;
        NetComponent.GetAdmob();
	}

    private void GetAdmonResult(bool _show)
    {
        if (PubilshSettingData.Instance.SelectedAdmobAD != 2)
        {
            return;
        }

        KOZNet.AdmobShowEvent -= GetAdmonResult;
        mRemoteSwitchAdmob = _show;
        InitialAd2ndPass();
    }
	
    /// <summary>
    /// 当前版本是否在运行时启用广告（会检测网络远端的开关）
    /// </summary>
	private bool isAdMobEnabledInRuntime()
	{
		bool adEnabledInBuild = isAdMobEnabledInBuild();
		if (!adEnabledInBuild)
		{
			return false;
		}

        if (PubilshSettingData.Instance.SelectedAdmobAD == 2)
        {
            return mRemoteSwitchAdmob;
        }
        else
        {
            return adEnabledInBuild;
        }
	}
	
    /// <summary>
    /// 初始化广告
    /// </summary>
    private bool mInitialAd = false;
    private void InitialAd()
    {
        if (mInitialAd)
        {
            return;
        }

        mInitialAd = true;

        if (PubilshSettingData.Instance.SelectedAdmobAD != 2)
        {
            InitialAd2ndPass();
        }
        else
        {
            //获取远端广告开关
            requestRemoteSwitchAdmob();
        }
    }
	
	/// <summary>
	/// 实际的初始化广告模块的代码
	/// </summary>
	private void InitialAd2ndPass()
	{
#if UNITY_IPHONE || UNITY_ANDROID
		if (isAdMobEnabledInRuntime())
		{
	        PPPAdMob.Visible = false;
	        PPPAdMob.Dock = PPPAdMobDockType.Bottom;

	        //if ((float)Screen.height / (float)Screen.width < 1.5f)
	        //{
	            PPPAdMob.SizeType = PPPAdMobSizeType.SmartBanner_iOS_Portrait;
	        //}
	        //else
	        //{
	        //    PPPAdMob.SizeType = PPPAdMobSizeType.Banner_320x50;
	        //}
			PPPAdMob.AdUnitID = "a150c05ba34573f";	// a150c05ba34573f is admob id for Kindom of Zombie
	        PPPAdMob.init();
	    }
#endif
	}

    /// <summary>
    /// 显示/隐藏广告
    /// </summary>
    private bool mEnableAd = false;
    public bool EnableAd
    {
        set
        {
            mEnableAd = value;

#if UNITY_IPHONE || UNITY_ANDROID
			if (isAdMobEnabledInRuntime())
			{
	            PPPAdMob.Visible = mEnableAd;
	        }
#endif
        }

        get
        {
            return mEnableAd;
        }
    }

    #endregion
    
    #region MessageBox

    /// <summary>
    /// 提示
    /// </summary>
    public void ShowMessageBoxSigh(string _title, string _message, string _button)
    {
        MSGBoxSpecial.ShowMessageSigh(_title, _message, _button);
    }

    /// <summary>
    /// 提问
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_message"></param>
    /// <param name="_button_ok"></param>
    /// <param name="_button_cancel"></param>
    public void ShowMessageBoxQuestion(string _title, string _message, string _button_ok, string _button_cancel)
    {
        MSGBoxSpecial.ShowMessageQuestion(_title, _message, _button_ok, _button_cancel);
    }

    /// <summary>
    /// 祝贺
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageBoxCongratulation(string _message, string _button)
    {
        MSGBoxSpecial.ShowMessageCongratulation(_message,  _button);
    }

    /// <summary>
    /// 遗憾
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageBoxSad(string _message, string _button)
    {
        MSGBoxSpecial.ShowMessageSad(_message, _button);
    }

    /// <summary>
    /// 胜利
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageBoxWin(string _message, string _button, int _time, int _money, int _gem, int _zombie, string _mvp)
    {
        MSGBoxSpecial.ShowMessageWin(_message, _button, _time, _money, _gem, _zombie, _mvp);
    }

    /// <summary>
    /// 显示只有一个按钮的MessageBox
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="positiveButton">按钮文字</param>
    public void ShowMessageBox (string _title, string _message, string _positiveButton)
	{
#if UNITY_IPHONE
        PPPMiscFeature.showAlert(_title, _message, _positiveButton);
#elif UNITY_ANDROID
		PPPMiscFeature.showAlert(_title, _message, _positiveButton);
#else
		Debug.Log (_title + ": " + _message);
#endif
	}

    /// <summary>
    /// 显示有两个按钮的MessageBox
    /// </summary>
    /// <param name="title">标题</param>
    /// <param name="message">消息内容</param>
    /// <param name="positiveButton">第一个按钮的文字</param>
    /// <param name="negativeButton">第二个按钮的文字</param>
    public void ShowMessageBox(string _title, string _message, string _positiveButton, string _negativeButton)
    {
#if UNITY_IPHONE
        PPPMiscFeature.showAlert(_title, _message, _positiveButton, _negativeButton);
#elif UNITY_ANDROID
        PPPMiscFeature.showAlert(_title, _message, _positiveButton, _negativeButton);
#else
        Debug.Log(_title + ": " + _message);
#endif
    }

    private void alertButtonClicked(string _text)
    {
        if (alertButtonClickedEvent != null)
            alertButtonClickedEvent(_text);
    }

    /// <summary>
    /// 显示有两个按钮的MessageBox (GUI)
    /// </summary>
    /// <param name="_title">标题</param>
    /// <param name="_message">消息内容</param>
    /// <param name="_button1">第一个按钮文字</param>
    /// <param name="_button2">第二个按钮文字</param>
    public void ShowInGameMessageBox(string _title, string _message, string _button1, string _button2)
    {
        MSGBox.ShowMessageBox(_title, _message, _button1, _button2);
        mAllowEscapeButton = false;
    }

    /// <summary>
    /// 显示只有一个按钮的MessageBox (GUI)
    /// </summary>
    /// <param name="_title">标题</param>
    /// <param name="_message">消息内容</param>
    /// <param name="_button">按钮文字</param>
    public void ShowInGameMessageBox(string _title, string _message, string _button)
    {
        MSGBox.ShowMessageBox(_title, _message, _button);
        mAllowEscapeButton = false;
    }

    public void ShowInGameMessageBoxWithImage(string _title, string _message, string _button, string _resource)
    {
        MSGBox.ShowMessageBoxWithImage(_title, _message, _button, _resource);
        mAllowEscapeButton = false;
    }

    private void InGameMessageBoxClicked(string _text)
    {
        mAllowEscapeButton = true;

        if (InGameMessageBoxButtonClickedEvent != null)
        {
            InGameMessageBoxButtonClickedEvent(_text);
        }
    }

    /// <summary>
    /// 显示/隐藏系统等待窗
    /// </summary>
    private bool mEnableLoading = false;
    public bool EnableLoading
    {
        set
        {
            if (mEnableLoading == value)
            {
                return;
            }

            mEnableLoading = value;
            FlowerCollider.gameObject.active = mEnableLoading;
            FlowerCollider.enabled = mEnableLoading;

#if UNITY_IPHONE
            if (mEnableLoading)
            {
				PPPMiscFeature.showActivityInProgressNotification("","");
            }
            else
            {
                PPPMiscFeature.hideActivityInProgressNotification();
            }
#elif UNITY_ANDROID
            if (mEnableLoading)
            {
                PPPMiscFeature.showActivityInProgressNotification("System", "Please wait...");
            }
            else
            {
                PPPMiscFeature.hideActivityInProgressNotification();
            }
#else
			Debug.Log (mEnableLoading ? "Show Loading." : "Hide Loading.");
#endif
        }

        get
        {
            return mEnableLoading;
        }
    }

    #endregion

    #region Buy

    /// <summary>
    /// 购买AppStore的商品
    /// 结果会返回AddCrystalEvent事件
    /// </summary>
    /// <param name="_productId"></param>
    public void BuyAppStoreItem(AppStoreItemID _productId)
    {
        if (KOZNet.IsDebugVersion)
        {
            PurchaseSuccessful((int)_productId);
            return;
        }

#if UNITY_IPHONE
        //PurchaseSuccessful((int)_productId);
        //return;
        //如果在Loading，禁止再购买。
        if(EnableLoading)
            return;
        if (GameStore.CanMakePayments() && GameStore.PurchaseProduct((int)_productId))
        {
            EnableLoading = true;
        }
#elif UNITY_ANDROID
        if (GlobalStaticData.GetOptionValue("ShopType") == "1")
        {
#if MMPAY
			BuyAppStoreItemMM(_productId);
#endif
#if GFPAY
            BuyAppStoreItemGF(_productId);
#endif
        }

#endif
    }
	
	/// <summary>
	/// 购买机锋物品
	/// </summary>
	/// <param name='_productId'>
	/// _product identifier.
	/// </param>
    private void BuyAppStoreItemGF(AppStoreItemID _productId)
    {
#if GFPAY
        switch (_productId)
        {
            case AppStoreItemID.BuyMM30Crystal:
                PPPGFanBilling.pay("", "", GFBUY30);
                break;
            case AppStoreItemID.BuyMM55Crystal:
                PPPGFanBilling.pay("", "", GFBUY55);
                break;
            case AppStoreItemID.BuyMM115Crystal:
                PPPGFanBilling.pay("", "", GFBUY115);
                break;
            case AppStoreItemID.BuyMM360Crystal:
                PPPGFanBilling.pay("", "", GFBUY360);
                break;
        }	
#endif
    }

	/// <summary>
	/// 购买移动物品
	/// </summary>
	/// <param name='_productId'>
	/// _product identifier.
	/// </param>
    private void BuyAppStoreItemMM(AppStoreItemID _productId)
    {
#if MMPAY
       if (!mIsMMInitSuccess)
       {
			ShowInGameMessageBox("系统", "移动支付尚未初始化成功，请稍后再试！", "确定");
			return;
       }		
		
       switch (_productId)
       {
            case AppStoreItemID.BuyMM30Crystal:
                MM_CurrentBuy = MMBUY30;
                PPPMMBilling.order(MMBUY30);
                break;
            case AppStoreItemID.BuyMM55Crystal:
                MM_CurrentBuy = MMBUY55;
                 PPPMMBilling.order(MMBUY55);
                break;
            case AppStoreItemID.BuyMM115Crystal:
                MM_CurrentBuy = MMBUY115;
                PPPMMBilling.order(MMBUY115);
                break;
            case AppStoreItemID.BuyMM360Crystal:
                MM_CurrentBuy = MMBUY360;
                PPPMMBilling.order(MMBUY360);
                break;
       }
#endif
    }

    private void PurchaseSuccessful(int _productId)
    {
        EnableLoading = false;

        int count = 60;
        switch ((AppStoreItemID)_productId)
        {
            case AppStoreItemID.Buy60Crystal:
                count = 60;
                break;
            case AppStoreItemID.Buy320Crystal:
                count = 320;
                break;
            case AppStoreItemID.Buy680Crystal:
                count = 680;
                break;
            case AppStoreItemID.Buy1400Crystal:
                count = 1400;
                break;
            case AppStoreItemID.BuyMM30Crystal:
                count = 30;
                break;
            case AppStoreItemID.BuyMM55Crystal:
                count = 55;
                break;
            case AppStoreItemID.BuyMM115Crystal:
                count = 115;
                break;
            case AppStoreItemID.BuyMM360Crystal:
                count = 360;
                break;
        }

        if (AddCrystalEvent != null)
        {
            AddCrystalEvent(count);
        }

        string[] btn_str = string.Format(StringTable.GetString(EStringIndex.UIText_BuyGem), count).Split('_');
        if(btn_str.Length == 3)
        {
            ShowMessageBoxCongratulation(btn_str[1], btn_str[2]);
        }
    }

    private void PurchaseFailed()
    {
        EnableLoading = false;
    }

    private void PurchaseCancelled()
    {
        EnableLoading = false;
    }

    #endregion

    #region RatingMe

    /// <summary>
    /// 去应用商店为本游戏打分
    /// </summary>
    public void RatingMe()
    {
#if (UNITY_IPHONE || UNITY_ANDROID) && !MOBAGE_VERSION && !MOBAGE_TW_VERSION
        int ratingMe = PlayerPrefs.GetInt("RatingMe");
        if (!PlayerPrefs.HasKey("RatingMe") || ratingMe != -1)
        {
            alertButtonClickedEvent += RatingMeResult;
            if (ratingMe == 0)
            {
                PlayerPrefs.SetInt("RatingMe", ratingMe + 1);
                SaveSetting();
            }
            else if  (ratingMe == 1)
            {
                string[] btn_str = StringTable.GetString(EStringIndex.UIText_RateMe).Split('_');
                if(btn_str.Length == 4)
                {
                    ShowMessageBox(btn_str[0], btn_str[1], btn_str[2], btn_str[3]);
                }
            }
            else
            {
                string[] btn_str = StringTable.GetString(EStringIndex.UIText_RateMe).Split('_');
                if(btn_str.Length == 4)
                {
                    ShowMessageBox(btn_str[0], btn_str[1], btn_str[2], btn_str[3]);
                }
            }
        }
#endif
    }

    private void RatingMeResult(string _button)
    {
#if UNITY_IPHONE || (UNITY_ANDROID && !MOBAGE_VERSION && !MOBAGE_TW_VERSION)
        string[] btn_str = StringTable.GetString(EStringIndex.UIText_RateMe).Split('_');

        int ratingMe = PlayerPrefs.GetInt("RatingMe");
        alertButtonClickedEvent -= RatingMeResult;
        if (_button == btn_str[2])
        {
            PlayerPrefs.SetInt("RatingMe", -1);
            LinkToWWW(LinkToWWWEnum.ReleasePage);
        }
        else if (_button == btn_str[3])
        {
            PlayerPrefs.SetInt("RatingMe", -1);
        }
        else
        {
            PlayerPrefs.SetInt("RatingMe", ratingMe + 1);
        }
        SaveSetting();
#endif
    }

    #endregion

    #region Swarm

    public bool isSwarmEnable
    {
        get
        {
#if SWARM_ACTIVE
            return true;
#else
            return false;
#endif
        }
    }

#if SWARM_ACTIVE

    // Swarm ID
    private const int SWARMID = 2123;
    private const string SWARMAPPAUTH = "f0169ed8154349530333864c064d47cc";
#endif

    /// <summary>
    /// 登录到Swarm
    /// </summary>
    public void LoginSwarm()
    {
#if SWARM_ACTIVE
        if (!Swarm.isEnabled() || !Swarm.isInitialized() || !Swarm.isLoggedIn())
        {
            Swarm.init(SWARMID, SWARMAPPAUTH);
            //Swarm.disableNotificationPopups();
            SwarmLoginManager.addLoginListener(delegate(int status)
            {
                if (status == SwarmLoginManager.USER_LOGGED_IN)
                {
                    // The player has successfully logged in
                    PlayerPrefs.SetInt("LoginSwarm", 1);
                    PlayerPrefs.SetString("LoginName", SwarmActiveUser.getUsername());

                    SaveSetting();
                }
                else if (status == SwarmLoginManager.LOGIN_STARTED)
                {
                    // The player has started logging in
                    PlayerPrefs.SetInt("LoginSwarm", 0);
                    PlayerPrefs.DeleteKey("LoginName");

                    SaveSetting();
                }
                else if (status == SwarmLoginManager.LOGIN_CANCELED)
                {
                    // The player has cancelled the login
                    PlayerPrefs.SetInt("LoginSwarm", 0);
                    PlayerPrefs.DeleteKey("LoginName");

                    SaveSetting();
                }
                else if (status == SwarmLoginManager.USER_LOGGED_OUT)
                {
                    // The player has logged out
                    PlayerPrefs.SetInt("LoginSwarm", 0);
                    PlayerPrefs.DeleteKey("LoginName");

                    SaveSetting();
                }
            });
        }
#endif
    }

#if SWARM_ACTIVE

    /// <summary>
    /// 打开/关闭登录swarm的提示窗
    /// </summary>
    public bool ShowLoginSwarm
    {
        set
        {
            if (mShowLoginSwarm == value)
            {
                return;
            }

            mShowLoginSwarm = value;
            if (mShowLoginSwarm)
            {
                PlaySE(GlobalModule.Instance.LoadResource("Sound/MessageBox") as AudioClip, 0.5f);
                ShowSwarmLoginPanel.SetActiveRecursively(true);

                ShowSwarmLoginPanel.transform.localPosition = new Vector3(0, -207f, 1);
                TweenPosition.Begin(ShowSwarmLoginPanel, 0.25f, new Vector3(0, 300f, 1));

                Invoke("CloseLoginSwarm", 5f);

                string[] btn_text = StringTable.GetString(EStringIndex.UIText_LoginSwarm).Split('_');
                if (btn_text.Length == 4)
                {
                    ShowSwarmLoginBtnLabel.text = btn_text[0];
                    ShowSwarmStartBtnLabel.text = btn_text[1];
                }
            }
            else
            {
                ShowSwarmLoginPanel.transform.localPosition = new Vector3(0, 300f, 1);
                TweenPosition.Begin(ShowSwarmLoginPanel, 0.25f, new Vector3(0, -207f, 1));
                Invoke("HideAllLoginSwarm", 0.25f);
            }
        }
    }
    private bool mShowLoginSwarm = false;

    private void CloseLoginSwarm()
    {
        ShowLoginSwarm = false;
        CancelInvoke("CloseLoginSwarm");
    }

    private void HideAllLoginSwarm()
    {
        ShowSwarmLoginPanel.SetActiveRecursively(false);
    }

    public void ClickInitLoginSwarmBtn()
    {
        LoginSwarm();
        CloseLoginSwarm();
    }

#endif

    #endregion

    #region Welcome

    public void ShowWelcome(string _name)
    {
        if (string.IsNullOrEmpty(_name))
        {
            //WelcomePanelLabel.text = StringTable.GetString(EStringIndex.Welcome_Back);
			WelcomeSysLabel.Text = StringTable.GetString(EStringIndex.Welcome_Back);
        }
        else
        {
            //WelcomePanelLabel.text = StringTable.GetString(EStringIndex.Welcome_Back) + ", " + _name;
			WelcomeSysLabel.Text = StringTable.GetString(EStringIndex.Welcome_Back);
        }
        
        WelcomePanel.SetActiveRecursively(true);
        Invoke("HideWelcome", 3);
    }

    public void HideWelcome()
    {
        WelcomePanel.SetActiveRecursively(false);
    }

    #endregion
}