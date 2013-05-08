
using UnityEngine;
using System.Collections;


public class TitleManager : MonoBehaviour {

	// Use this for initialization
    public bool isLoading = false;
    /// <summary>
    /// 点击成就次数
    /// </summary>
    public static int ClickAchieve
    {
        get
        {
            if (!PlayerPrefs.HasKey("ClickAchieve"))
            {
                PlayerPrefs.SetInt("ClickAchieve", 0);
                PlayerPrefs.Save();
            }
            return PlayerPrefs.GetInt("ClickAchieve");
        }
        set 
        {
            PlayerPrefs.SetInt("ClickAchieve", value);
            PlayerPrefs.Save();
        }
    }

    public static int ClickRank
    {
        get
        {
            if (!PlayerPrefs.HasKey("ClickRank"))
            {
                PlayerPrefs.SetInt("ClickRank", 0);
                PlayerPrefs.Save();
            }
            return PlayerPrefs.GetInt("ClickRank");
        }
        set
        {
            PlayerPrefs.SetInt("ClickRank", value);
            PlayerPrefs.Save();
        }
    }

    public int ClickShare
    {
        get
        {
            if (!PlayerPrefs.HasKey("ClickShare"))
            {
                PlayerPrefs.SetInt("ClickShare", 0);
                PlayerPrefs.Save();
            }
            return PlayerPrefs.GetInt("ClickShare");
        }
        set
        {
            PlayerPrefs.SetInt("ClickShare", value);
            PlayerPrefs.Save();
        }
    }

    public int ClickMoreGame
    {
        get
        {
            if (!PlayerPrefs.HasKey("ClickMoreGame"))
            {
                PlayerPrefs.SetInt("ClickMoreGame", 0);
                PlayerPrefs.Save();
            }
            return PlayerPrefs.GetInt("ClickMoreGame");
        }
        set
        {
            PlayerPrefs.SetInt("ClickMoreGame", value);
            PlayerPrefs.Save();
        }
    }

    /*
    public string MachineUID
    {
        get
        {
            if(!PlayerPrefs.HasKey("MachineUID"))
            {
                PlayerPrefs.SetString("MachineUID", System.Guid.NewGuid().ToString("N"));
                PlayerPrefs.Save();
            }

            return PlayerPrefs.GetString("MachineUID");
        }
    }
    */

    public GameObject IBtn_Swarm;

    public GameObject BtnMoreGame;
    public UILabel VersionLabel;
    public GameObject Background;

    void Awake()
    {
        UIRoot root = GetComponent<UIRoot>();
        if ((float)Screen.width / (float)Screen.height < 2.0f / 3.0f)
        {
            root.manualHeight = 640 * Screen.height / Screen.width;
        }
        else
        {
            root.manualHeight = (int)960;
        }
    }


	void Start () 
    {
        if(!PlayerPrefs.HasKey("StageFull_1"))
        {
            PlayerPrefs.SetInt("StageFull_1", 0);
            PlayerPrefs.Save();
        }
        if(!PlayerPrefs.HasKey("StageFull_2"))
        {
            PlayerPrefs.SetInt("StageFull_2", 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey("StageFull_3"))
        {
            PlayerPrefs.SetInt("StageFull_3", 0);
            PlayerPrefs.Save();
        }       

        ClickAchieve = PlayerPrefs.GetInt("ClickAchieve");
        ClickRank = PlayerPrefs.GetInt("ClickRank");
        ClickShare = PlayerPrefs.GetInt("ClickShare");
        ClickMoreGame = PlayerPrefs.GetInt("ClickMoreGame");


        //string m_id = MachineUID;
        //Screen.sleepTimeout = SleepTimeout.NeverSleep;

        isLoading = false;

#if UNITY_ANDROID

        if(PlayerPrefs.GetInt("LoginSwarm") == 1)
        {
            IBtn_Swarm.SetActiveRecursively(false);
        }
#else
        IBtn_Swarm.SetActiveRecursively(false);
#endif

        Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
        switch (publisherID)
        {
            case Publisher.Mobage_CN:
            case Publisher.Mobage_TW:
                IBtn_Swarm.SetActiveRecursively(false);
                BtnMoreGame.SetActiveRecursively(false);
                break;
            default:
                break;
        }

        if (PubilshSettingData.Instance.IsDebugVersion)
        {
            VersionLabel.text = "Debug Ver." + PubilshSettingData.Instance.PublishVersion.ToString() + " Device: " + GlobalModule.Instance.DeviceID;
        }
        else
        {
            VersionLabel.text = "Ver." + PubilshSettingData.Instance.PublishVersion.ToString();
        }

        Invoke("ShakeBackground", 2);

        if (StringTable.mStringType == ELocalizationTyp.ChineseTw)
        {
            GameObject.Find("SA_KOZ").GetComponent<EasyMotion2D.SpriteAnimation>().Play("Title_Fan");
        }
        else if (StringTable.mStringType == ELocalizationTyp.Japanese)
        {
            GameObject.Find("SA_KOZ").GetComponent<EasyMotion2D.SpriteAnimation>().Play("Title_JP");
        }
	}

    private void ShakeBackground()
    {
        iTween.ShakePosition(Background, new Vector3(0.02f, 0.02f, 0), 1.5f);
        GlobalModule.Instance.PlaySE(GlobalModule.Instance.LoadResource("Sound/EnemyDie") as AudioClip, 0.5f);
    }

    bool isQuit = false;
	// Update is called once per frame
	void Update () 
    {

        if (Input.GetKeyDown(KeyCode.Escape) && !isQuit)
        {
            string[] btn_str = StringTable.GetString(EStringIndex.UIText_QuitGame).Split('_');

            if(btn_str.Length == 4)
            {
                isQuit = true;
                GlobalModule.alertButtonClickedEvent += QuitButtonClick;
                GlobalModule.Instance.ShowMessageBox(btn_str[0], btn_str[1], btn_str[2], btn_str[3]);
            }
        }
	}
    /// <summary>
    /// 退出游戏
    /// </summary>
    /// <param name="_button"></param>
    private void QuitButtonClick(string _button)
    {
        GlobalModule.alertButtonClickedEvent -= QuitButtonClick;
        string[] btn_str = StringTable.GetString(EStringIndex.UIText_QuitGame).Split('_');

        if (_button == btn_str[2])
        {
            Application.Quit();
        }
        isQuit = false;
    }


    void TestEvent(string str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= TestEvent;
        Debug.Log(str);
    }

    /// <summary>
    /// 开始游戏
    /// </summary>
    void OnStartGame()
    {
        if(isLoading == false)
        {
            ResourcePath.PlaySound("TapToStart");
            GameDataCenter center = GameDataCenter.Instance;

            if (!center.HasSave || !center.Load())
            {
                GameDataCenter.BackupSave();
                center.NewGame();
                center.Save();
            }

            GlobalModule.Instance.LoadSceneN("GameMain");
            isLoading = true;
        }
    }

    /// <summary>
    /// 新游戏（debug）
    /// </summary>
    void OnNewGame()
    {
        if (isLoading == false)
        {
            GameDataCenter center = GameDataCenter.Instance;

            center.NewInstance();
            center.NewGame();
            center.Save();
            GlobalModule.Instance.LoadSceneN("GameMain");
            isLoading = true;
        }
    }

    /// <summary>
    /// 测试用
    /// </summary>
    public void OnSendAchieve()
    {
		GlobalModule.Instance.SendGameCenterAchievement(AchievementEnum.ZombieArmy,33.3f);
        print("SendAchieve");
    }

    /// <summary>
    /// 测试用
    /// </summary>
    public void OnSendRank()
    {
		GlobalModule.Instance.SendGameCenterLeaderboard(0,1988);
        print("SendRank");
    }



    /// <summary>
    /// 打开成就
    /// </summary>
    public void OnAchieve()
    {
        ClickAchieve++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickAchievement, ClickAchieve.ToString());
        GlobalModule.Instance.OpenGameCenterAchievement();
    }

    /// <summary>
    /// 打开排行
    /// </summary>
    public void OnRank()
    {
        ClickRank++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickLeaderboard, ClickRank.ToString());
        GlobalModule.Instance.OpenGameCenterLeaderboard();
    }

    /// <summary>
    /// 点击moregame
    /// </summary>
    public void OnMoreGame()
    {
        ClickMoreGame++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickMoregames, ClickMoreGame.ToString());
        GlobalModule.Instance.LinkToWWW(LinkToWWWEnum.ProductPage);
    }

    /// <summary>
    /// 点击分享
    /// </summary>
    public void OnShare()
    {
        ClickShare++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickShear, ClickShare.ToString());
        GlobalModule.Instance.LinkToWWW(LinkToWWWEnum.ShareMe1);
    }

    public GameObject Panel_Honor;
    /// <summary>
    /// 勋章
    /// </summary>
    public void OnHonor()
    {
        Panel_Honor.SetActiveRecursively(true);
    }

    /// <summary>
    /// swarm
    /// </summary>
    public void OnSwarm()
    {
        if (GlobalModule.Instance.isSwarmEnable)
        {
            GlobalModule.Instance.LoginSwarm();
        }
    }
}
