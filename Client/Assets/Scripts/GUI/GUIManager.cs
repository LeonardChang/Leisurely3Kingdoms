using UnityEngine;
using System.Collections;
using EasyMotion2D;


public class GUIManager : MonoBehaviour 
{
    //public 
    /// <summary>
    /// 药剂剩余时间
    /// </summary>
    public UISlider Bar_LavaTime;

    /// <summary>
    /// 攻击目标血条
    /// </summary>
    public UISlider storyProcess;

    public GUIStoryManager mapPanel;

    public TweenScale Panel_Option;
    public bool isOptionOpen = false;

    public GameObject Panel_MapName;

    //各机器动画
    public SpriteAnimation SA_GuanGai;
    public SpriteAnimation SA_JiQi;
    public SpriteAnimation SA_JiTan;

    //声音按钮
    public UIImageButton IBtn_Sound;
    public UIImageButton IBtn_Music;

    public Transform Info_list;

    public UILabel Label_Level;
    public UISlider Bar_Exp;

    public UISprite Icon_Money;
    public UILabel Label_Money;
    public UILabel Label_Gem;

    public GameObject Flash_Effect;

    public float mHouseBomTime = 0;

    public GameObject Panel_Main;
    public GUIPanelManager Panel_Manager;
    public Camera Camera_2D;
    public Camera Camera_UI;
    public GameObject SceneMask;

    public ZombieManager Zombie_Manager;
    public bool isStory = false;
    //public GameObject Rain;
    public GUIHeadFace HeadFace;
    public GUIItemHelp ItemHelp;

    public GameObject Weather;
    public GameObject GameMain;
    public TeachManager Teach_Manager;

    TweenColor Bar_LavaFore_White;
    Transform Bar_LavaFore;

    public bool mIsLoading = true;
     //Use this for initialization
	void Start () 
    {
        GameDataCenter.Instance.mCurrentScene = GameDataCenter.Instance.ChangeTargetScene;
        GameAward.AwardOldVersion();
        NewVersionChecking.CheckVersion();


        UIRoot root = GetComponent<UIRoot>();
        if ((float)Screen.width / (float)Screen.height < 2.0f / 3.0f)
        {
            root.manualHeight = 640 * Screen.height / Screen.width;
        }
        else
        {
            root.manualHeight = (int)960;
        }

        Camera_UI = GameObject.Find("Camera").camera;

        //SetWeather();
        IniScene(GameDataCenter.Instance.CurrentScene);

        if(!GameDataCenter.Instance.IsTeachMode)
        {
            GlobalModule.Instance.RatingMe();
        }
        CheckSaveFile();
        Bar_LavaFore_White = Bar_LavaTime.transform.FindChild("Foreground_White").GetComponent<TweenColor>();
        Bar_LavaFore = Bar_LavaTime.transform.FindChild("Foreground");
        mFromMoney = GameDataCenter.Instance.GetCurrentMoney();
        mFromGem = GameDataCenter.Instance.GetCurrentGem();
        
        ResetBottomArrowBtn();

        mIsLoading = false;
        //Debug.Log(GameDataCenter.Instance.mTodayTaskPoint);
	}


    void CheckSaveFile()
    {
        if (string.IsNullOrEmpty(GameDataCenter.Instance.SavedUniqueID))
        {
            GameDataCenter.Instance.SavedUniqueID = GameDataCenter.Instance.UniqueID;
            GameDataCenter.Instance.ForceSave();

            print("The save data file has not saved unique ID, create one.");
        }
        else
        {
            string uniqueID = GameDataCenter.Instance.UniqueID;
            if (GameDataCenter.Instance.SavedUniqueID != uniqueID)
            {
                GameDataCenter.Instance.SavedUniqueID = uniqueID;
                GameDataCenter.Instance.ClearMoney();
                GameDataCenter.Instance.ForceSave();

                Debug.LogError("The save data file does not belong to the device!");
            }
            else
            {
                print("Check save data file's unique ID successful.");
            }
        }        
    }
    
    /// <summary>
    /// 设置天气音效
    /// </summary>
    void SetWeatherSound()
    {
        if (!Weather) return;

        if(Weather.GetComponent<AudioSource>())
        {
            Weather.GetComponent<AudioSource>().volume = GlobalModule.Instance.SoundVolume * 0.5f;
        }
    }


    public float mStopWeatherTime = 0;
    public int mWeatherType = 0;
    public void SetWeather()
    {
        SetWeather(0.5f);
    }

    /// <summary>
    /// 刷新天气
    /// </summary>
    /// <param name="_value">概率</param>
    public void SetWeather(float _value)
    {
        if (Weather)
        {
            Destroy(Weather);
        }
        if (Random.value < _value)
        {
            //Rain.SetActiveRecursively(true);
            if (GameDataCenter.Instance.mCurrentScene == 0 || GameDataCenter.Instance.mCurrentScene == 2)
            {
                Weather = ResourcePath.Instance(EResourceIndex.Prefab_Rain);
                Weather.transform.parent = GameMain.transform;
                Weather.transform.localScale = new Vector3(480, 480, 480);
                Weather.transform.localPosition = new Vector3(123, 712, -626);
                SetWeatherSound();
                mWeatherType = 1;
            }
            else if (GameDataCenter.Instance.mCurrentScene == 1)
            {
                Weather = ResourcePath.Instance(EResourceIndex.Prefab_Snow);
                Weather.transform.parent = GameMain.transform;
                Weather.transform.localScale = new Vector3(480, 480, 480);
                Weather.transform.localPosition = new Vector3(0, 574, 0);
                mWeatherType = 2;
            }
            mStopWeatherTime = 60;
        }
        else
        {
            mWeatherType = 0;
        }
    }

    /// <summary>
    /// 缺钱确认回调
    /// </summary>
    /// <param name="_btn"></param>
    void MsgBoxNeedMoneyEvent(string _btn)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= MsgBoxNeedMoneyEvent;

        if(_btn == StringTable.GetString(EStringIndex.Tips_OK) && !Panel_Manager.Panel_Shop.gameObject.active)
        {
            Panel_Manager.ForceCloseAll();
            Panel_Manager.OnShopBtn();
        }
    }

    /// <summary>
    /// 缺钱对话框
    /// </summary>
    public void MsgBoxNeedMoney()
    {
        //if (Panel_Manager.isShopOpen) return;

        if (GameDataCenter.Instance.mIsDeviceLocked)
        {
            MsgBox(string.Format(StringTable.GetString(EStringIndex.Tips_FreezeDevice), PlayerPrefs.GetString("Devive")));
            return;
        }


        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_Warn), StringTable.GetString(EStringIndex.Tips_NeedMoney),
                StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));

        MessageBoxSpecial.alterButtonClickedEvent += MsgBoxNeedMoneyEvent;

    }

    /// <summary>
    /// 显示提示对话框（一个OK按钮）
    /// </summary>
    /// <param name="_str">显示内容</param>
    public void MsgBox(string _str)
    {
        GlobalModule.Instance.ShowMessageBoxSigh(StringTable.GetString(EStringIndex.Tips_TitleTips), _str,
                StringTable.GetString(EStringIndex.Tips_OK));
    }

    /// <summary>
    /// 显示祝贺对话框(一个OK按钮)
    /// </summary>
    /// <param name="_str"></param>
    public void MsgBoxCongratulation(string _str)
    {
        GlobalModule.Instance.ShowMessageBoxCongratulation(_str, StringTable.GetString(EStringIndex.Tips_OK));
    }

    void Awake()
    {
        UIRoot root = GetComponent<UIRoot>();
        if ((float)Screen.width / (float)Screen.height < 2.0f/3.0f)
        {
            root.manualHeight = 640 * Screen.height / Screen.width;
        }
        else
        {
            root.manualHeight = (int)960;
        }

        mMoraleString  = StringTable.GetString(EStringIndex.UIText_MoraleLevel).Split('_');
    }

    bool isIni = false;
    public SpriteRenderer DustLine1;
    public SpriteRenderer DustLine2;
    public SpriteRenderer DustLine3;


    /****************Effect start****************/
    /// <summary>
    /// 使用技能特效
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public GameObject EffectUseSkill(int _index)
    {
        EResourceIndex[] SkillGroup = new EResourceIndex[] 
        { 
            EResourceIndex.Prefab_Effect_Skill_1,
            EResourceIndex.Prefab_Effect_Skill_2,
            EResourceIndex.Prefab_Effect_Skill_3,
            EResourceIndex.Prefab_Effect_Skill_4
        };

        GameObject obj = ResourcePath.Instance(SkillGroup[_index]);
        obj.transform.parent = Panel_Main.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(-300, 430, 800);
        return obj;
    }

    /// <summary>
    /// 金币和宝石随机的闪烁效果
    /// </summary>
    public void EffectStar()
    {

        Vector3 pos = Random.value < 0.5 ? new Vector3(110, 442, -178): new Vector3(-132, 442, -178);
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Star);
        obj.transform.parent = Panel_Main.transform;
        obj.transform.localPosition = pos;
        obj.transform.localScale = new Vector3(480, 480, 480);
    }

    /// <summary>
    /// 博士升级特效
    /// </summary>
    public void EffectLevelUp()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Effect_LvUP);
        obj.transform.parent = Panel_Main.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(-250, 373, 800);


        obj = ResourcePath.Instance(EResourceIndex.Prefab_Art_UpGrade);
        obj.transform.parent = Panel_Main.transform;
        obj.transform.localPosition = new Vector3(-305, 440, -46);
        obj.transform.localScale = new Vector3(480, 480, 1);//Vector3.one;
        Destroy(obj, 2);
        HeadFace.SetSmile();
    }

    /// <summary>
    /// 创建跳出来的数字
    /// </summary>
    /// <param name="_type">类型</param>
    /// <param name="_parent">父节点</param>
    /// <param name="_pos">位置</param>
    /// <param name="_value">值</param>
    /// <returns></returns>
    public GameObject CreateFlyLabel(int _type, GameObject _parent, Vector3 _pos, int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Fly_Label);
        obj.transform.parent = _parent.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.transform.localPosition = _pos;
        obj.GetComponent<FlyLabel>().mType = _type;
        obj.GetComponent<FlyLabel>().mValue = _value;
        obj.GetComponent<FlyLabel>().mHasCritical = false;
        return obj;
    }

    /// <summary>
    /// 创建泡泡提示
    /// </summary>
    /// <param name="_type">类型</param>
    /// <param name="_poptip">提示内容</param>
    /// <param name="_parent">父对象</param>
    /// <param name="_pos">坐标</param>
    /// <returns></returns>
    public GameObject CreatePopTips(int _type, string _poptip, GameObject _parent, Vector3 _pos)
    {
        if(GameDataCenter.Instance.IsTeachMode)
        {
            return null;
        }

        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_PopTips);
        obj.transform.parent = _parent.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = _pos;

        obj.GetComponent<GUIPopTips>().mType = _type;
        obj.GetComponent<GUIPopTips>().mTipsString = _poptip;

        return obj;
    }

    GameObject PopTips_Marola;
    /// <summary>
    /// 士气泡泡
    /// </summary>
    /// <param name="_index"></param>
    public void PopTipsMarola(int _index)
    {
        EStringIndex[] tips_str = new EStringIndex[] { EStringIndex.Tips_HasAddMorale, EStringIndex.Tips_HasSubMorle};

        if(!PopTips_Marola)
        {
            PopTips_Marola = CreatePopTips(0, StringTable.GetString(tips_str[_index]), GameObject.Find("Panel_MainButton"), new Vector3(100, -350, -100));
        }
    }

    /// <summary>
    /// 需要药剂泡泡
    /// </summary>
    /// <returns></returns>
    public GameObject PopTipsNeedAgent()
    {
        if(GameDataCenter.Instance.IsTeachMode)
        {
            return null;
        }

        GameObject obj = ResourcePath.Instance("Tips_NeedAgent");
        obj.transform.parent = GameObject.Find("Anchor_GameMainPanel").transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(-225, 140, -100);

        return obj;
    }


    GameObject PopTips_SeeStage;
    /// <summary>
    /// 弹出查看主题泡泡
    /// </summary>
    public void PopTipsSeeStage()
    {
        if(GameDataCenter.Instance.mTipSeeStage >= 1)
        {
            return;
        }
        if(!PopTips_SeeStage)
        {
            GameDataCenter.Instance.mTipSeeStage++;
        }
    }

    /// <summary>
    /// 删除查看主题泡泡
    /// </summary>
    public void DeleteTipsSeeStage()
    {
        if(PopTips_SeeStage)
        {
            Destroy(PopTips_SeeStage);
        }
    }

    GameObject PopTips_Skill;
    /// <summary>
    /// 弹出技能泡泡
    /// </summary>
    public void PopTipsSkill()
    {
        Debug.Log("pop");
        PopTips_Skill = CreatePopTips(1, StringTable.GetString(EStringIndex.Tips_ClickToUseSkill), GameObject.Find("Panel_Main_Pop"), new Vector3(-293, 340, -100));
    }

    /// <summary>
    /// 删除技能泡泡
    /// </summary>
    public void DeletePopTipsSkill()
    {
        if(PopTips_Skill)
        {
            Destroy(PopTips_Skill);
        }
    }

    /// <summary>
    /// 需要宝石泡泡
    /// </summary>
    GameObject PopTips_NeedGem;
    public void PopTipsNeedGem()
    {
        if(GameDataCenter.Instance.mTipGemTimes > 5)
        {
            return;
        }
        if(!PopTips_NeedGem)
        {
            if(Random.value < 0.3)
            {
                PopTips_NeedGem = CreatePopTips(0, StringTable.GetString(EStringIndex.Tips_NeedMoreGem), GameObject.Find("IBtn_Shop"), new Vector3(-160, 150, -100));
                GameDataCenter.Instance.mTipGemTimes ++;
            }
        }
    }

    /// <summary>
    /// 删除需要宝石泡泡
    /// </summary>
    public void DeleteNeedGem()
    {
        if(PopTips_NeedGem)
        {
            Destroy(PopTips_NeedGem);
        }
    }



    /// <summary>
    /// 新僵尸提示泡泡
    /// </summary>
    GameObject PopTips_NewZombie;
    public void PopTipsNewZombie()
    {
        if(!PopTips_NewZombie)
        {
            ResourcePath.PlaySound("NewZombie");
            PopTips_NewZombie = CreatePopTips(0, StringTable.GetString(EStringIndex.Tips_NewZombie), GameObject.Find("IBtn_Collection"), new Vector3(-160, 150, -100));
        }
        else
        {
            PopTips_NewZombie.GetComponent<GUIPopTips>().mLifeTime = 5;
        }
    }


    /// <summary>
    /// 删除新僵尸提示泡泡
    /// </summary>
    public void DeleteTipsNewZombie()
    {
        if(PopTips_NewZombie)
        {
            Destroy(PopTips_NewZombie);
        }
    }


    /// <summary>
    /// 新僵尸提示
    /// </summary>
    GameObject Effect_New;
    public void EffectNew()
    {
        if(GameDataCenter.Instance.GetIsNewZombie())
        {
            if (Effect_New)
            {
                return;
            }
            Effect_New = ResourcePath.Instance("Sprite_Exclamation");
            Effect_New.transform.parent = GameObject.Find("IBtn_Collection").transform;
            Effect_New.transform.localScale = Vector3.one;
            Effect_New.transform.localPosition = new Vector3(-22f, 58f, -5);
            //Effect_New = CreatePopTips(0, StringTable.GetString(EStringIndex.Tips_NewZombie), GameObject.Find("Panel_MainButton"), new Vector3(-200, -225, -100));
        }
        else
        {
            Destroy(Effect_New);  
        }
    }

    /// <summary>
    /// 任务完成
    /// </summary>
    GameObject Effect_TaskDone;
    public void EffectTaskDone()
    {
        if(GameDataCenter.Instance.GetIsTaskDone())
        {
            if(Effect_TaskDone)
            {
                return;
            }
            Effect_TaskDone = ResourcePath.Instance("Sprite_Exclamation");
            Effect_TaskDone.transform.parent = GameObject.Find("IBtn_RiChang").transform;
            Effect_TaskDone.transform.localScale = Vector3.one;
            Effect_TaskDone.transform.localPosition = new Vector3(-22f, 58f, -5);
        }
        else
        {
            Destroy(Effect_TaskDone);
        }
    }

    /// <summary>
    /// 有新的岛屿开放
    /// </summary>
    GameObject Effect_NewIsland;
    public void EffectNewIsland()
    {
        if (GameDataCenter.Instance.GetIsNewIslandOpen())
        {
            if(Effect_NewIsland)
            {
                return;
            }
            Effect_NewIsland = ResourcePath.Instance("Sprite_Exclamation");
            Effect_NewIsland.transform.parent = GameObject.Find("IBtn_DaoYu").transform;
            Effect_NewIsland.transform.localScale = Vector3.one;
            Effect_NewIsland.transform.localPosition = new Vector3(-22f, 58f, -5);
        }
        else
        {
            Destroy(Effect_NewIsland);
        }
    }


    /// <summary>
    /// 掉落经验
    /// </summary>
    /// <param name="_pos">位置</param>
    void CreateExperience(Vector3 _pos)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyExperience);
        obj.transform.parent = GameObject.Find("Panel_Main_FlyItemParent").transform;
        obj.transform.position = _pos;
        obj.transform.localScale = new Vector3(50, 48, 1);
        obj.GetComponent<FlyExperience>().mValue = 1;


        obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyExperience);
        obj.transform.parent = GameObject.Find("Panel_Main_FlyItemParent").transform;
        obj.transform.position = _pos;
        obj.transform.localScale = new Vector3(50, 48, 1);
        obj.GetComponent<FlyExperience>().mType = 1;
    }

    /// <summary>
    /// 使用道具产生特效
    /// </summary>
    /// <param name="_index">第几个机器</param>
    /// <param name="type">特效类型 0-升级 1-其他道具</param>
    /// <param name="_item_type">道具类型</param>
    public void EffectUseItem(int _index, int type, ESceneItemDataType _item_type)
    {
        Vector3[] pos_list = new Vector3[] {
        new Vector3(-187, 240, 0),
        new Vector3(24, 250, 0),
        new Vector3(250, 230, 0)
        };

        Vector3[] pos_list2 = new Vector3[] { 
        SA_GuanGai.transform.position,
        SA_JiQi.transform.position,
        SA_JiTan.transform.position
        };
        GameObject obj;
        if(type == 0)
        {
            obj = ResourcePath.Instance(EResourceIndex.Prefab_Effect_Upgrade);
            ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp1);
            HeadFace.SetSmile();

        }
        else
        {
            obj = ResourcePath.Instance(EResourceIndex.Prefab_Effect_UseItem);
            ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
        }

        obj.transform.parent = GameObject.Find("Scene1").transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = pos_list[_index];

        if(_item_type != ESceneItemDataType.KeepRun)
        {
            CreateExperience(pos_list2[_index] + new Vector3(0, 0, -1));
        }
    }

    /// <summary>
    /// 震动屏幕
    /// </summary>
    public void ShakeScreen()
    {
        iTween.ShakePosition(Camera_2D.transform.parent.gameObject, new Vector3(0.03f, 0.03f, 0), 1f);
    }

    /**********Effect end*********************************/


    public UITexture Main_bg1;
    //public SpriteAnimation SA_GuanGai
	// Update is called once per frame

    /// <summary>
    /// 初始化场景主题
    /// </summary>
    /// <param name="_index">主题id</param>
    public void IniScene(int _index)
    {
        SceneData sd = ResourcePath.GetSceneData(_index);

        Main_bg1.material.mainTexture = sd.DustLine4;

        Main_bg1.transform.localScale = new Vector3(780,   780f * sd.DustLine4.height / (float)sd.DustLine4.width );

        DustLine1.Clear();
        DustLine1.AttachSprite(sd.DustLine1);
        DustLine1.Apply();

        DustLine2.Clear();
        DustLine2.AttachSprite(sd.DustLine2);
        DustLine2.Apply();

        DustLine3.Clear();
        DustLine3.AttachSprite(sd.DustLine3);
        DustLine3.Apply();

        SpriteAnimationUtility.SetAnimationClips(SA_GuanGai, sd.IrrigationMotion);
        SpriteAnimationUtility.SetAnimationClips(SA_JiQi, sd.MachineMotion);
        SpriteAnimationUtility.SetAnimationClips(SA_JiTan, sd.AltarMotion);

        GameObject obj = (GameObject)Instantiate(sd.Scene);
        obj.transform.parent = Panel_Manager.Panel_Story.GetComponent<GUIStoryManager>().Grid;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, 0, 0) ;
        obj.SetActiveRecursively(false);

        SA_GuanGai.GetComponent<MachineManager>().UpdateMotion();
        SA_JiQi.GetComponent<MachineManager>().UpdateMotion();
        SA_JiTan.GetComponent<MachineManager>().UpdateMotion();

        //Icon_Money.spriteName = sd.MoneySprite;
        //Icon_Money.MakePixelPerfect();
        UpdateMorale();
    }

    /// <summary>
    /// 确认升级士气
    /// </summary>
    /// <param name="_str"></param>
    void ClickMoraleSure(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= ClickMoraleSure;
        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if(!GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceAddMorale), ECostGem.AddMorale))
            {
                MsgBoxNeedMoney();
                return;
            }

            GameDataCenter.Instance.mMoraleLevel = 5;
            UpdateMorale();
        }

    }

    /// <summary>
    /// 点击士气
    /// </summary>
    void OnClickMorale()
    {
        //GlobalModule.Instance.ShowInGameMessageBox();
        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_AddMorale),
        StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
        MessageBoxSpecial.alterButtonClickedEvent += ClickMoraleSure;
    }


    /// <summary>
    /// 更新士气图标
    /// </summary>
    public UISprite Spr_MoraleBack;
    public UILabel Label_Morale;

    static Color GetColor(int _r, int _g, int _b)
    {
        return new Color(_r / 255.0f, _g/255.0f, _b/255.0f);
    }
    static Color[] MoraleColors = new Color[] {
        GetColor(127, 178, 236),
        GetColor(192, 129, 232),
        GetColor(221, 199, 77),
        GetColor(228, 162, 92),
        GetColor(215, 129, 129)
        };
 
    static string[] mMoraleString;
    /// <summary>
    /// 更新士气UI
    /// </summary>
    public void UpdateMorale()
    {
        Spr_MoraleBack.spriteName = "Main_ShiQi_" + GameDataCenter.Instance.mMoraleLevel.ToString();
        Label_Morale.color = MoraleColors[GameDataCenter.Instance.mMoraleLevel - 1];
        Label_Morale.text = mMoraleString[GameDataCenter.Instance.mMoraleLevel - 1];
    }


    GameObject Skill_Effect_1;
    GameObject Skill_Effect_2;
    GameObject Skill_Effect_3;
    GameObject Skill_Effect_4;
    /// <summary>
    /// 更新技能特效
    /// </summary>
    public void UpDateSkillEffect()
    {
        if(GameDataCenter.Instance.GetSkill(ESkillType.LV25).RestTime >= 0)
        {
            if(!Skill_Effect_1)
            {
                Skill_Effect_1 = EffectUseSkill(0);
            }
        }
        else
        {
            if(Skill_Effect_1)
            {
                Destroy(Skill_Effect_1);
            }
        }

        if (GameDataCenter.Instance.GetSkill(ESkillType.LV50).RestTime >= 0)
        {
            if (!Skill_Effect_2)
            {
                Skill_Effect_2 = EffectUseSkill(1);
            }
        }
        else
        {
            if (Skill_Effect_2)
            {
                Destroy(Skill_Effect_2);
            }
        }

        if (GameDataCenter.Instance.GetSkill(ESkillType.LV75).RestTime >= 0)
        {
            if (!Skill_Effect_3)
            {
                Skill_Effect_3 = EffectUseSkill(2);
            }
        }
        else
        {
            if (Skill_Effect_3)
            {
                Destroy(Skill_Effect_3);
            }
        }

        if (GameDataCenter.Instance.GetSkill(ESkillType.LV99).RestTime >= 0)
        {
            if (!Skill_Effect_4)
            {
                Skill_Effect_4 = EffectUseSkill(3);
            }
        }
        else
        {
            if (Skill_Effect_4)
            {
                Destroy(Skill_Effect_4);
            }
        }

    }

    /// <summary>
    /// Debug功能
    /// </summary>
    void DebugUpdate()
    {
        //////////////////////////////////debug//////////////////////////////////////////////////////
        if (KOZNet.IsDebugVersion)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {

                EffectLevelUp();
                GameDataCenter.Instance.ZombieCollection[15].Count = 0;
                GameDataCenter.Instance.ZombieCollection[15].IsNew = false;
                GameDataCenter.Instance.ZombieCollection[15].IsOpen = false;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                Attack(9999);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                //GameDataCenter.Instance.Money = 88888;
                GameDataCenter.Instance.SetCurrentMoney(88888);
                GameDataCenter.Instance.GemString = "888";
            }
            if (Input.GetKeyDown(KeyCode.J))
            {
                GameDataCenter.Instance.PlayerLevel += 1;
                GameDataCenter.Instance.CheckSkillPop();
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                GameDataCenter.Instance.Money += 1;
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                GameDataCenter.Instance.Gem += 1;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                foreach (CZombieData zombie in GameDataCenter.Instance.ZombieCollection)
                {
                    if (zombie.Type >= ZombieType.Normal && zombie.Type <= ZombieType.Zombie45)
                    {
                        zombie.Count += 1;
                    }

                }
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameDataCenter.Instance.AddExperience(1);
            }
        }
        //////////////////////////////////////////////////////////////////////////
    }


    float mOptionOpenTime = 0;
	void Update () 
    {
        ////////////////Debug/////////////////////
        DebugUpdate();


        if(mStopWeatherTime > 0)
        {
            mStopWeatherTime -= Time.deltaTime;
            if(mStopWeatherTime <= 0)
            {
                SetWeather(0);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && GlobalModule.Instance.AllowEscapeButton)
        {
            OnBackToTitle();
        }

        if(mOptionOpenTime > 0)
        {
            mOptionOpenTime -= Time.deltaTime;
            if(mOptionOpenTime < 0)
            {
                CloseOptionPanel();
            }
        }


        if(!isIni)
        {
            isIni = true;
            //Sprite spr = new Sprite();
            if(!GameDataCenter.Instance.IsDateTimeOk())
            {
                string str = string.Format(StringTable.GetString(EStringIndex.Tips_DateTimeError), GameDataCenter.Instance.LastSaveTime.ToString());
                MsgBox(str);
            }
            UnSetBgmPich();
            SetWeather();
            if (!GameDataCenter.Instance.IsTeachMode)
            {
                
                if ((GlobalStaticData.IsStageStart(GameDataCenter.Instance.GetCurrentStory().StoryIndex))
                    && GameDataCenter.Instance.GetCurrentScene().IsNewOpen)
                {
                    GameDataCenter.Instance.GetCurrentScene().IsNewOpen = false;
                    GameDataCenter.Instance.StartStory();

                    OpenStory(GameDataCenter.Instance.CurrentStory());
                }
                else if(GameDataCenter.Instance.CurrentStoryHP() <= 0)
                {
                    GameDataCenter.Instance.GoNextStory();
                    CStory _story = GameDataCenter.Instance.GetCurrentStory();
                    GameDataCenter.Instance.SetStoryHP(_story.MaxCondition);
                }
                 

            }
        }

        UpdateLabels();

        if (!Flash_Effect.active && GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.GetPercent() > 0)
        {
            Flash_Effect.SetActiveRecursively(true);

        }
        if (Flash_Effect.active && GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.GetPercent() <= 0)
        {
            Flash_Effect.SetActiveRecursively(false);
        }

        //经验
        Label_Level.text = GameDataCenter.Instance.PlayerLevel.ToString();
        Bar_Exp.sliderValue = GameDataCenter.Instance.Experience / (float)(GameDataCenter.Instance.PlayerLevel * 20);


        
        if (Teach_Manager.mTeachStep >= 4 && GameDataCenter.Instance.IsTeachMode && Teach_Manager.mTeachStep < 26)
        {
            Bar_LavaTime.sliderValue = 1;
        }
        else
        {
            Bar_LavaTime.sliderValue = GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.GetPercent();
        }


        Bar_LavaFore_White.transform.localScale = Bar_LavaFore.localScale;
        if ( Mathf.Abs(Bar_LavaFore_White.duration - Bar_LavaTime.sliderValue * 2.0f)  > 0.1f)
        {
            Bar_LavaFore_White.duration = Bar_LavaTime.sliderValue * 2.0f;
        }

        CStory story = GameDataCenter.Instance.GetCurrentStory();
        if(story.MaxCondition == 0)
        {
            story.MaxCondition = 1;
        }
        //storyProcess.sliderValue = GameDataCenter.Instance.CurrentStoryHP() / (float)story.MaxCondition;

        if (GameDataCenter.Instance.IsTeachMode)
        {
            storyProcess.sliderValue = (float)Teach_Manager.Teach_HP / 12.0f;
        }
        else
        {
            storyProcess.sliderValue = GameDataCenter.Instance.CurrentStoryHP() / (float)story.MaxCondition;
        }


        if(Input.GetMouseButtonUp(0))
        {
            HideDraggingHand();
        }
        
	}


    /// <summary>
    /// 更新金币以及宝石Label
    /// </summary>
    void UpdateLabels()
    {


        if (mMoneyLerp < 1)
        {
            mMoneyLerp += (Time.deltaTime * 0.5f);
            if (mMoneyLerp > 1)
            {
                mMoneyLerp = 1;
            }
        }

        if (mGemLerp < 1)
        {
            mGemLerp += (Time.deltaTime * 0.5f);
            if (mGemLerp > 1)
            {
                mGemLerp = 1;
            }
        }

        float from_money = mFromMoney;
        if (mFromMoney < GameDataCenter.Instance.GetCurrentMoney())
            mFromMoney = Mathf.CeilToInt(Mathf.Lerp(mFromMoney, GameDataCenter.Instance.GetCurrentMoney(), mMoneyLerp));
        else
            mFromMoney = Mathf.FloorToInt(Mathf.Lerp(mFromMoney, GameDataCenter.Instance.GetCurrentMoney(), mMoneyLerp));
        if ((int)from_money != (int)mFromMoney) MotionScaleMoney();
        float ui_money = mFromMoney > 99999999 ? 99999999 : mFromMoney;
        Label_Money.text = ((int)ui_money).ToString();


        float from_gem = mFromGem;
        if(mFromGem < GameDataCenter.Instance.GetCurrentGem())
            mFromGem = Mathf.CeilToInt(Mathf.Lerp(mFromGem, GameDataCenter.Instance.GetCurrentGem(), mGemLerp));
        else
            mFromGem = Mathf.FloorToInt(Mathf.Lerp(mFromGem, GameDataCenter.Instance.GetCurrentGem(), mGemLerp));
        if ((int)from_gem != (int)mFromGem) MotionScaleGem();
        float ui_gem = mFromGem > 9999 ? 9999 : mFromGem;
        Label_Gem.text = ((int)ui_gem).ToString();

    }



    /// <summary>
    /// 攻击目标
    /// </summary>
    /// <param name="_value"></param>
    public void Attack(float _value)
    {
        if(GameDataCenter.Instance.IsTeachMode)
        {

            return;
        }
        //CreateFlyLabel(0, Panel_Manager.Panel_Attack.gameObject, Panel_Manager.Panel_Attack.Sprite_AttackTarget.transform.localPosition + new Vector3(Random.Range(-10, 10), 20, -500), _value);
        GameDataCenter.Instance.AddStoryHP(-_value);
        CheckStory();
    }

    /// <summary>
    /// 创建打击数字
    /// </summary>
    /// <param name="_value">值</param>
    /// <param name="_type">类型 0-正常 1-暴击</param>
    public void CreateHitPoint(int _value, int _type)
    {
        if(_type == 0)
        {
            CreateFlyLabel(0, Panel_Manager.Panel_Attack.gameObject, Panel_Manager.Panel_Attack.Sprite_AttackTarget.transform.localPosition + new Vector3(Random.Range(-25, 25), 20, -500), _value);
        }
        else if(_type == 1)
        {
            GameObject obj = CreateFlyLabel(0, Panel_Manager.Panel_Attack.gameObject, Panel_Manager.Panel_Attack.Sprite_AttackTarget.transform.localPosition + new Vector3(Random.Range(-25, 25), 20, -500), _value);
            obj.GetComponent<FlyLabel>().mHasCritical = true;
        }
    }


    void CreateAttackGem(int count)
    {
                 for (int i = 0; i < count; i++)
                {
                    Panel_Manager.Panel_Attack.CreateGem(1);
                }
    }



    /// <summary>
    /// 下个剧情
    /// </summary>
    /// <returns></returns>
    IEnumerator NextStory()
    {
        if (GameDataCenter.Instance.CurrentStory() != 0)//非教程后的第一个关卡
        {
            if (GlobalStaticData.IsStageEnd(GameDataCenter.Instance.CurrentStory()))//如果是宝箱关卡
            {

                Panel_Manager.Panel_Attack.CreateBom();
                CreateAttackGem(GameDataCenter.Instance.GetCurrentStory().AwardGem);

                yield return new WaitForSeconds(2);
                GameDataCenter.Instance.GetCurrentScene().ChestLevel++;
                GameDataCenter.Instance.SetEndTarget();

                CStory story = GameDataCenter.Instance.GetCurrentStory();
                GameDataCenter.Instance.SetStoryHP(story.MaxCondition);
                Panel_Manager.Panel_Attack.NextChest();
                isStory = false;
            }
            else//其他普通关卡
            {
                Panel_Manager.Panel_Attack.CreateBom();
                int count = Random.Range(3, 9);
                for (int i = 0; i < count; i++)
                {
                    Panel_Manager.Panel_Attack.CreateCoin(10);
                }

                for(int i = 0; i < GameDataCenter.Instance.GetCurrentStory().AwardGem; i++)
                {
                    Panel_Manager.Panel_Attack.CreateGem(1);
                }

                yield return new WaitForSeconds(1);
                WinEffect();
                if(mapPanel.IsOpen)
                {
                    mapPanel.OnNext();
                }
                
                yield return new WaitForSeconds(4);
                ShowWinDialog();
            }
        }
        else//教程后的第一个关卡
        {
            yield return new WaitForSeconds(0.3f);
            GameDataCenter.Instance.GoNextStory();
            CStory story = GameDataCenter.Instance.GetCurrentStory();
            GameDataCenter.Instance.SetStoryHP(story.MaxCondition);
            OpenStory(GameDataCenter.Instance.CurrentStory());
            Panel_Manager.Panel_Attack.ClosePanel();
            isStory = false;
        }
    }

    /// <summary>
    /// 确认开始下个剧情
    /// </summary>
    /// <param name="_str"></param>
    void NextStoryOk(string _str)
    {
        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            MessageBoxSpecial.alterButtonClickedEvent -= NextStoryOk;
            GameDataCenter.Instance.GoNextStory();
            CStory story = GameDataCenter.Instance.GetCurrentStory();

            GameDataCenter.Instance.SetStoryHP(story.MaxCondition);
            OpenStory(GameDataCenter.Instance.CurrentStory());
            isStory = false;
        }
    }

    /// <summary>
    /// 显示胜利对话框
    /// </summary>
    void ShowWinDialog()
    {

        CScene scene = GameDataCenter.Instance.GetCurrentScene();
        int use_minute = (int)((System.DateTime.Now - scene.mWinStartTime).TotalMinutes);
        string mvp = GameDataCenter.Instance.GetOneZombieCollection(scene.mWinMVP).ZombieInfo.Name;

        GlobalModule.Instance.ShowMessageBoxWin(StringTable.GetString(EStringIndex.Tips_WinTotal), StringTable.GetString(EStringIndex.Tips_OK), use_minute, scene.mWinMoney,
            GameDataCenter.Instance.GetCurrentStory().AwardGem, scene.mWinJoinZombie, mvp);

        MessageBoxSpecial.alterButtonClickedEvent += NextStoryOk;

    }

    /// <summary>
    /// 胜利特效
    /// </summary>
    void WinEffect()
    {
        Panel_Manager.ForceCloseAll();
        if(GlobalModule.Instance.MSGBox.BasePanel.active)
        {
            GlobalModule.Instance.MSGBox.ClickRightButton();
        }

        if (GlobalModule.Instance.MSGBoxSpecial.Panel_MessageBoxSpecial.active)
        {
            GlobalModule.Instance.MSGBoxSpecial.OnCancelButton();
        }

        GameObject obj_emo = GameObject.Find("FlyEmo");
        if(obj_emo)
        {
            Destroy(obj_emo);
        }


        EResourceIndex win_prefab = EResourceIndex.Prefab_Win_EN;
        switch(StringTable.mStringType)
        {
            case ELocalizationTyp.Chinese:
                win_prefab = EResourceIndex.Prefab_Win;
                break;
            case ELocalizationTyp.ChineseTw:
                win_prefab = EResourceIndex.Prefab_Wini_TW;
                break;
        }

        GameObject obj = ResourcePath.Instance(win_prefab);
        


        obj.name = "WinEffect";
        obj.transform.parent = Camera_UI.transform;
        obj.transform.localScale = new Vector3(1, 1, 1);

        ResourcePath.PlaySound(EResourceAudio.Audio_Clap);
        ResourcePath.PlaySound(EResourceAudio.Audio_Win);
    }


    public void StopNextStory()
    {
        StopCoroutine("NextStory");
    }



    /// <summary>
    /// 检测当前目标是否被击破
    /// </summary>
    public void CheckStory()
    {
        if (GameDataCenter.Instance.CurrentStoryHP() <= 0 && !isStory)
        {
            isStory = true;
            StartCoroutine(NextStory());
        }

        if ((GlobalStaticData.IsStageStart(GameDataCenter.Instance.GetCurrentStory().StoryIndex))
            && GameDataCenter.Instance.GetCurrentScene().IsNewOpen)
        {
            GameDataCenter.Instance.GetCurrentScene().IsNewOpen = false;
            GameDataCenter.Instance.StartStory();

            OpenStory(GameDataCenter.Instance.CurrentStory());
        }
    }

    public bool IsStoryMode
    {
        get { return true; }
    }


/****************Zombie Collection***********************/
    /// <summary>
    /// 从左边创建僵尸详情面板
    /// </summary>
    /// <param name="zombie">僵尸数据</param>
    public void CreateMoveLeftZombieInfo(CZombieData zombie)
    {
        ResourcePath.PlaySound(EResourceAudio.Audio_Page);
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Panel_CollectionOne);

        obj.transform.parent = Camera_UI.transform;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<GUICollectionManager>().mZombieData = zombie;

        OOTools.OOTweenPosition(obj, new Vector3(-800, 0, -550), new Vector3(0, 0, -550));

    }

    /// <summary>
    /// 从右边创建僵尸详情面板
    /// </summary>
    /// <param name="zombie">僵尸数据</param>
    public void CreateMoveRightZombieInfo(CZombieData zombie)
    {
        ResourcePath.PlaySound(EResourceAudio.Audio_Page);
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Panel_CollectionOne);

        obj.transform.parent = Camera_UI.transform;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<GUICollectionManager>().mZombieData = zombie;

        OOTools.OOTweenPosition(obj, new Vector3(800, 0, -550), new Vector3(0, 0, -550));
    }

    /// <summary>
    /// 关闭僵尸详情面板
    /// </summary>
   public void OnCloseCollection()
   {
       GameObject.Find("GUIMask").transform.localPosition = new Vector3(1200f, 0, 75f);

       GameObject.Find("IBtn_NextZombie").transform.localPosition = new Vector3(-2000, 0, -950);
       GameObject.Find("IBtn_PreZombie").transform.localPosition = new Vector3(-2000, 0, -950);


       GameObject[] obj = GameObject.FindGameObjectsWithTag("PanelCollection");
       foreach(GameObject o in obj)
       {
           o.SendMessage("GetOut");
       }
   }

    /// <summary>
    /// 往左移动所有僵尸详情面板
    /// </summary>
    void OnLeftCollection()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("PanelCollection");

        foreach (GameObject o in obj)
        {
            GUICollectionManager collection = o.GetComponent<GUICollectionManager>();

            if(collection.isMoving == false)
            {
                collection.OnLeft();
            }
        }
    }

    /// <summary>
    /// 往右移动所有僵尸详情面板
    /// </summary>
    void OnRightCollection()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("PanelCollection");

        foreach (GameObject o in obj)
        {
            GUICollectionManager collection = o.GetComponent<GUICollectionManager>();
            if (collection.isMoving == false)
            {
                collection.OnRight();
            }
        }
    }

    /*****************ZombieCollection*******************************/

    /// <summary>
    /// 打开地图开始剧情
    /// </summary>
    /// <param name="_index"></param>
    public void OpenStory(int _index)
    {
        GameDataCenter.Instance.ForceSave();
        mapPanel.gameObject.SetActiveRecursively(true);
        mapPanel.Open(_index);
    }


    GameObject Sprite_DragingHand;
    bool mIsShowDraging = true;
    /// <summary>
    /// 显示摇晃手势
    /// </summary>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public GameObject SetDraggingHand(Vector3 _pos)
    {
        if (!mIsShowDraging || GameDataCenter.Instance.IsTeachMode) return null;

        //Anchor_GameMainPanel
        if (!Sprite_DragingHand)
        {
            mIsShowDraging = false;
            Sprite_DragingHand = ResourcePath.Instance(EResourceIndex.Prefab_DraggingHand);
            Sprite_DragingHand.transform.parent = GameObject.Find("Anchor_GameMainPanel").transform;
            Sprite_DragingHand.transform.localScale = Vector3.one;
            Sprite_DragingHand.transform.localPosition = new Vector3(0, 0, 0);
            Destroy(Sprite_DragingHand, 3);
        }

        //Sprite_DragingHand.transform.position = _pos + new Vector3(0.1f, 0.5f, 0);
        return Sprite_DragingHand;
    }

    /// <summary>
    /// 隐藏摇晃手势
    /// </summary>
    public void HideDraggingHand()
    {
        if(Sprite_DragingHand)
        {
            Destroy(Sprite_DragingHand);
        }
    }



    /****************Option  start**************/
    /// <summary>
    /// 设置按钮
    /// </summary>
    void OnOption()
    {
        if(!isOptionOpen)
        {
            OpenOptionPanel();
        }
        else
        {
            mOptionOpenTime = 0;
            CloseOptionPanel();
        }
    }


    bool isBackToTitle = false;
    /// <summary>
    /// 返回标题
    /// </summary>
    void OnBackToTitle()
    {
        if (isBackToTitle) return;
        if (isStory) return;



        if (mapPanel.IsOpen)
        {
            mapPanel.OnBack();
            return;
        }
        
        if (GlobalModule.Instance.MSGBox.BasePanel.active)
        {
            GlobalModule.Instance.MSGBox.ClickRightButton();
        }
        if (GlobalModule.Instance.MSGBoxSpecial.Panel_MessageBoxSpecial.active)
        {
            GlobalModule.Instance.MSGBoxSpecial.OnCancelButton();
        }

        StopCoroutine("NextStory");
        isBackToTitle = true;
        GameDataCenter.Instance.ForceSave();
        

        UnSetBgmPich();
        ForceAddFlyMoney();
        CloseOptionPanel();
        GlobalModule.Instance.LoadSceneN("Title");
    }

   

    /// <summary>
    /// 打开设置面板
    /// </summary>
    void OpenOptionPanel()
    {
        OOTools.OOTweenScale(Panel_Option.gameObject, Vector3.zero, Vector3.one);

        mOptionOpenTime = 3;
        isOptionOpen = true;

        if(GlobalModule.Instance.SoundVolume <= 0)
        {
            SetBtnSprite(IBtn_Sound, "Option_Sound_Close_Nor", "Option_Sound_Close_Nor", "Option_Sound_Close_Down");
        }
        if(GlobalModule.Instance.MusicVolume <= 0)
        {
            SetBtnSprite(IBtn_Music, "Option_Music_Close_Nor", "Option_Music_Close_Nor", "Option_Music_Close_Down");
        }
    }

    /// <summary>
    /// 关闭设置面板
    /// </summary>
    void CloseOptionPanel()
    {
        if(!isOptionOpen)
        {
            return;
        }

        isOptionOpen = false;
        OOTools.OOTweenScale(Panel_Option.gameObject, Vector3.one, Vector3.zero);
    }


    /// <summary>
    /// 设置按钮图片
    /// </summary>
    /// <param name="_iBtn">按钮</param>
    /// <param name="_norSprite">正常状态</param>
    /// <param name="_hoverSprite">划过状态</param>
    /// <param name="_pressedSprite">按下状态</param>
    void SetBtnSprite(UIImageButton _iBtn, string _norSprite, string _hoverSprite, string _pressedSprite)
    {
        _iBtn.normalSprite = _norSprite;
        _iBtn.hoverSprite = _hoverSprite;
        _iBtn.pressedSprite = _pressedSprite;
        _iBtn.transform.FindChild("Background").GetComponent<UISprite>().spriteName = _norSprite;
    }

    /// <summary>
    /// 点击音效
    /// </summary>
    void OnSound()
    {
        mOptionOpenTime = 3;
        if(GlobalModule.Instance.SoundVolume >= 1)
        {
            GlobalModule.Instance.SoundVolume = 0;
            SetBtnSprite(IBtn_Sound, "Option_Sound_Close_Nor", "Option_Sound_Close_Nor", "Option_Sound_Close_Down");
        }
        else
        {
            GlobalModule.Instance.SoundVolume = 1;
            SetBtnSprite(IBtn_Sound, "Option_Sound_Nor", "Option_Sound_Nor", "Option_Sound_Down");
        }
        SetWeatherSound();
    }

    /// <summary>
    /// 点击背景音乐
    /// </summary>
    void OnMusic()
    {
        mOptionOpenTime = 3;
        if (GlobalModule.Instance.MusicVolume >= 1)
        {
            GlobalModule.Instance.MusicVolume = 0;
            SetBtnSprite(IBtn_Music, "Option_Music_Close_Nor", "Option_Music_Close_Nor", "Option_Music_Close_Down");
        }
        else
        {
            GlobalModule.Instance.MusicVolume = 1;
            SetBtnSprite(IBtn_Music, "Option_Music_Nor", "Option_Music_Nor", "Option_Music_Down");
        }
    }
    /***************************************Option  end***************************************/




    float mMoneyLerp = 1;
    float mFromMoney = 0;
    public void SetMoneyMotion()
    {
        mMoneyLerp = 0;
    }

    float mGemLerp = 1;
    float mFromGem = 0;
    public void SetGemMotion()
    {
        mGemLerp = 0;
    }
    
    /// <summary>
    /// 加金币（附带数字缩放效果）
    /// </summary>
    /// <param name="_value"></param>
    public void GameAddMoney(int _value)
    {
 
        GameDataCenter.Instance.AddMoney(_value);
        ResourcePath.ReplaySound(EResourceAudio.Audio_Money, 1, 0.2f);
        HeadFace.SetMoney();
        SetMoneyMotion();
    }

    public void MotionScaleMoney()
    {
        TweenScaleEx ts = Label_Money.GetComponent<TweenScaleEx>();
        ts.enabled = true;
        ts.Reset();
        ts.Play(true);
    }


    /// <summary>
    /// 加宝石（附带数字缩放效果）
    /// </summary>
    /// <param name="_value"></param>
    public void GameAddGem(int _value)
    {

        GameDataCenter.Instance.AddGem(_value);
        ResourcePath.PlaySound(EResourceAudio.Audio_Money2);
        HeadFace.SetMoney();
        SetGemMotion();
    }

    public void MotionScaleGem()
    {
        TweenScaleEx ts = Label_Gem.GetComponent<TweenScaleEx>();
        ts.enabled = true;
        ts.Reset();
        ts.Play(true);
    }


    /// <summary>
    /// 设置背景音乐pich
    /// </summary>
    public void SetBgmPich()
    {
        GlobalModule.Instance.BGMPlayer.pitch = 2.0f;
        Invoke("UnSetBgmPich", 30);
    }

    /// <summary>
    /// 关闭背景音乐pich
    /// </summary>
    public void UnSetBgmPich()
    {
        GlobalModule.Instance.BGMPlayer.pitch = 1.0f;
    }




    void OnEnable()
    {
        GlobalModule.AddCrystalEvent += ShopAddGem;
        KOZNet.FreezeDeviceEvent += FreezeDevice;
    }
    void OnDisable()
    {
        GlobalModule.AddCrystalEvent -= ShopAddGem;
        KOZNet.FreezeDeviceEvent -= FreezeDevice;
    }

    /// <summary>
    /// 购买金币回调
    /// </summary>
    /// <param name="_value"></param>
    void ShopAddGem(int _value)
    {
        GameDataCenter.Instance.GuiManager.GameAddGem(_value);
        GameDataCenter.Instance.ForceSave();
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.UseGem, ((int)ECostGem.BuyGem).ToString() + "_" + _value.ToString());
        GameDataCenter.Instance.AddAchieve(AchievementEnum.VIP);
        GameDataCenter.Instance.taskManager.ChangeGem();
    }

    /// <summary>
    /// 冻结账户回调
    /// </summary>
    /// <param name="_locked"></param>
    void FreezeDevice(bool _locked)
    {
        if (_locked)
        {
            GameDataCenter.Instance.mIsDeviceLocked = true;
        }
        else
        {
            GameDataCenter.Instance.mIsDeviceLocked = false;
        }
    }




    static int MAX_BOTTOMBTNPAGE = 2;
    int mCurrentBottomBtnPage = 1;
    bool mIsCurrentBottomBtnSliding = false;
    public GameObject Panel_MenuButton;
    public GameObject IBtn_NextBtnPage;
    public GameObject IBtn_PreBtnPage;
    /// <summary>
    /// 相左滑动下方墓碑按钮
    /// </summary>
    public void SlideBottomBtnLeft()
    {

        if (mCurrentBottomBtnPage < MAX_BOTTOMBTNPAGE && mIsCurrentBottomBtnSliding == false)
        {
            mIsCurrentBottomBtnSliding = true;
            mCurrentBottomBtnPage++;
            TweenPosition.Begin(Panel_MenuButton, 0.3f, new Vector3((mCurrentBottomBtnPage - 1) * (-800), 0, 10));
            ResetBottomArrowBtn();
            Invoke("ResetBottomBtnSlidState", 0.3f);
            GlobalModule.Instance.Click();
        }
    }

    /// <summary>
    /// 向右滑动下方墓碑按钮
    /// </summary>
    public void SlideBottomBtnRight()
    {
        if (mCurrentBottomBtnPage > 1 && mIsCurrentBottomBtnSliding == false)
        {

            mIsCurrentBottomBtnSliding = true;
            mCurrentBottomBtnPage--;
            TweenPosition.Begin(Panel_MenuButton, 0.3f, new Vector3((mCurrentBottomBtnPage - 1) * (-800), 0, 10));
            ResetBottomArrowBtn();
            Invoke("ResetBottomBtnSlidState", 0.3f);
            GlobalModule.Instance.Click();
        }
    }

    /// <summary>
    /// 重置下方墓碑按钮滑动状态
    /// </summary>
    void ResetBottomBtnSlidState()
    {
        mIsCurrentBottomBtnSliding = false;
        foreach (Transform trans in Panel_MenuButton.transform)
        {
            if(trans.gameObject.active)
                trans.SendMessage("ResetPosition");
        }
    }

    /// <summary>
    /// 底部墓碑方向箭头状态
    /// </summary>
    void ResetBottomArrowBtn()
    {
        if(mCurrentBottomBtnPage == 1)
        {
            IBtn_PreBtnPage.SetActiveRecursively(false);
        }
        else
        {
            IBtn_PreBtnPage.SetActiveRecursively(true);
        }

        if (mCurrentBottomBtnPage == MAX_BOTTOMBTNPAGE)
        {
            IBtn_NextBtnPage.SetActiveRecursively(false);
        }
        else
        {
            IBtn_NextBtnPage.SetActiveRecursively(true);
        }
    }


    /// <summary>
    /// 将场景残余的钱加起来
    /// </summary>
    void ForceAddFlyMoney()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("FlyAward");
        foreach (GameObject obj in objs)
        {
            if(obj.active)
                obj.SendMessage("OnForceAddMoney");
        }

        foreach(Transform trans in Zombie_Manager.transform)
        {
            if(trans.gameObject.active)
                trans.SendMessage("CollectIt");
        }

        foreach(Transform trans in Panel_Manager.Panel_Attack.zombieList)
        {
            if(trans.gameObject.active)
                trans.SendMessage("CollectIt");
        }

    }


}
