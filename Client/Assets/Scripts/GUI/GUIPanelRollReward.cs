using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using EasyMotion2D;

/// <summary>
/// 摇奖机器
/// </summary>
public class RollMachine
{


    public float mLevel = 0;
    public float mExperience = 0;

    /// <summary>
    /// 必须出现777
    /// </summary>
    public bool mMustGet777
    {
        get 
        {
            int day_count = GameDataCenter.Instance.PlayDates.Length;
            if(mCanGet777 && (day_count == 3 || day_count == 14 || day_count == 28))
                return true;
            else
                return false;
        }
    }
    
    /// <summary>
    /// 是否可以获得777
    /// </summary>
    public bool mCanGet777
    {
        get
        {
            if (mCount777 < 1)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 是否可以获得Z77
    /// </summary>
    public bool mCanGetZ77
    {
        get
        {
            if (mCountZ77 < 2)
                return true;
            else
                return false;
        }
    }

    /// <summary>
    /// 是否可以获得B77
    /// </summary>
    public bool mCanGetB77
    {
        get
        {
            if(mCountB77 < 1)
                return true;
            else
                return false;
        }
    }

    public int mCount777 = 0;
    public int mCountB77 = 0;
    public int mCountZ77 = 0;

    public int mWeekGet777 = 0;
    public int mWeekGetB77 = 0;
    public int mWeekGetZ77 = 0;
    
    /// <summary>
    /// 初始化当前摇奖
    /// </summary>
    public void InitTodayReword()
    {
        int week = DateTime.Today.DayOfYear / 7;

        if(mWeekGet777 != week )
        {
            mCount777 = 0;
            mWeekGet777 = week;
        }
        if(mWeekGetB77 != week)
        {
            mCountB77 = 0;
            mWeekGetB77 = week;
        }
        if(mWeekGetZ77 != week)
        {
            mCountZ77 = 0;
            mWeekGetZ77 = week;
        }
        mFreeTimes = 1;
    }


    public int mFreeTimes = 1;

    /// <summary>
    /// 升级
    /// </summary>
    public bool LevelUp()
    {
        if (mExperience >= 100)
        {
            if (GameDataCenter.Instance.mRollMachine.mLevel < 10)
            {
                GameDataCenter.Instance.mRollMachine.mLevel++;
                GameDataCenter.Instance.mRollMachine.mExperience = 0;
                return true;
            }
        }
        return false;
    }

    public float mLevelMultiple
    {
        get
        {
            return 1;
        }
    }
}


 /// <summary>
/// 闪灯类型
/// </summary>
public  enum LightType
{
    Light_Off,
    Light_Run,
    Light_Flash,
}

/// <summary>
/// 摇奖UI
/// </summary>
public class GUIPanelRollReward : MonoBehaviour 
{
 
    public SpriteAnimation AnimationRoll;

    public static float mRunLightCount = 0;
    public static bool mIsLightRunning = false;
    public static LightType mLightType = LightType.Light_Off;
    public static  float max_count = 0.5f;

    public UITexture Texture_Roll_1;
    public UITexture Texture_Roll_2;
    public UITexture Texture_Roll_3;

    public UILabel Label_Reword;
    public UISlider PB_Experience;
    public UILabel Label_Level;
    public UILabel Label_YesterdayResult;

    public UIImageButton IBtn_UpgradeRoll;

    /// <summary>
    /// 摇奖Label
    /// </summary>
    public UILabel IBtn_Roll_Label;
    public UILabel Label_Odds;
    public UILabel Label_Yesterday;
    public UILabel Label_HighReward;

    /// <summary>
    /// 
    /// </summary>
    public Transform Panel_Light;
    public UILabel Label_Tips;

	// Use this for initialization
	void Start () 
    {
        InitRoller();
        InitLight();
	}
	
    float rool = 0;
    bool mIsRolling = false;


    float mTarget1 = 0;
    float mTarget2 = 0;
    float mTarget3 = 0;


    float mCurrent1 = 0;
    float mCurrent2 = 0;
    float mCurrent3 = 0;

    int mTarget_1 = 0;
    int mTarget_2 = 0;
    int mTarget_3 = 0;

    bool mHasStop_1 = true;
    bool mHasStop_2 = true;
    bool mHasStop_3 = true;


    void OnEnable()
    {
        UpdateBtn();
        if(mIsRolling)
        {
            StartRunSound();
        }
        OnIniRollBar();
    }

    void OnDisable()
    {
        StopRunSound();
    }

	// Update is called once per frame
	void Update () 
    {

        RunLightUpdate();

        if (!AnimationRoll.IsPlaying("DH_YaoJiangJi"))
        {
            OnIniRollBar();
        }

        if(mIsRolling)
        {
            if(mCurrent1 < mTarget1)
            {
                mCurrent1 += (Time.deltaTime * (mTarget1 - mCurrent1 < 1 ? 0.5f:2));
                if(mCurrent1 > mTarget1)
                {
                    mCurrent1 = mTarget1;

                    if (!mHasStop_1)
                    {
                        mHasStop_1 = true;
                        ResourcePath.PlaySound("777RollOK");
                    }
                    
                }
            }

            if (mCurrent2 < mTarget2)
            {
                mCurrent2 += (Time.deltaTime * (mTarget2 - mCurrent2 < 1 ? 0.5f : 2));
                if (mCurrent2 > mTarget2)
                {
                    mCurrent2 = mTarget2;

                    if (!mHasStop_2)
                    {
                        mHasStop_2 = true;
                        ResourcePath.PlaySound("777RollOK");
                    }
                }
            }

            if (mCurrent3 < mTarget3)
            {
                mCurrent3 += (Time.deltaTime * (mTarget3 - mCurrent3 < 1 ? 0.5f : 2));
                if (mCurrent3 > mTarget3)
                {
                    mCurrent3 = mTarget3;
                    mIsRolling = false;
                    CheckResult();

                    if (!mHasStop_3)
                    {
                        mHasStop_1 = true;
                        ResourcePath.PlaySound("777RollOK");
                    }
                }
            }
        }

        Texture_Roll_1.material.mainTextureOffset = new Vector2(0, mCurrent1);
        Texture_Roll_2.material.mainTextureOffset = new Vector2(0, mCurrent2);
        Texture_Roll_3.material.mainTextureOffset = new Vector2(0, mCurrent3);

        UpdateUI();
	}

    void UpdateText()
    {

        Label_Odds.text = string.Format(StringTable.GetString(EStringIndex.UIText_RewardOdds), GameDataCenter.Instance.mYesterdayTaskMultiple);
        int high = (int)(GetReword(999) * GameDataCenter.Instance.mYesterdayTaskMultiple);
        Label_HighReward.text = GameDataCenter.Instance.GetChangeText(string.Format(StringTable.GetString(EStringIndex.UIText_RollHighReward), high));

    }

    /// <summary>
    /// 
    /// </summary>
    void UpdateUI()
    {
        PB_Experience.sliderValue = GameDataCenter.Instance.mRollMachine.mExperience / 100f;

        if (GameDataCenter.Instance.mRollMachine.mLevel > 0)
            Label_Level.text = "Lv" + GameDataCenter.Instance.mRollMachine.mLevel.ToString();
        else
            Label_Level.text = "";

        Label_YesterdayResult.text = GameDataCenter.Instance.GetYestodayTaskResult();
        if(GameDataCenter.Instance.mRollMachine.mFreeTimes > 0)
        {
            IBtn_Roll_Label.text = StringTable.GetString(EStringIndex.UIText_RollRewardFree);
        }
        else
        {
            int need_gem = (int)(GameDataCenter.Instance.mYesterdayTaskMultiple * 2);
            IBtn_Roll_Label.text =  "[ffffff]ж[-]×" + need_gem;
        }
        UpdateText();
    }

    void InitRoller()
    {
        mCurrent1 = mPicIndex[0];
        mCurrent2 = mPicIndex[0];
        mCurrent3 = mPicIndex[0];



        Debug.Log(mPicIndex[0]);
        Label_Yesterday.text = StringTable.GetString(EStringIndex.UIText_YesterdayValuation);
        UpdateText();
        
        UpdateBtn();
        Label_Tips.text = StringTable.GetString(EStringIndex.Tips_RollMachineTips);
    }

    int GetPic(float target)
    {
        return (int)((target * 10) % 10);
    }


    /// <summary>
    /// 获取奖励
    /// </summary>
    /// <param name="reword"></param>
    /// <returns></returns>
    int GetReword(int reword)
    {
        return (int)(reword * Mathf.Pow(1.1f, (GameDataCenter.Instance.mRollMachine.mLevel)));
    }

    /// <summary>
    /// 升级
    /// </summary>
    void OnLevelUp(GameObject sender)
    {
        if (mIsRolling)
            return;
        if(GameDataCenter.Instance.mRollMachine.LevelUp())
        {
            GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Art_UpGrade);
            obj.transform.parent = sender.transform;
            obj.transform.localPosition = new Vector3(0, 0, -100);
            obj.transform.localScale = new Vector3(480, 480, 1);//Vector3.one;
            ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp2);
            Destroy(obj, 2);
            iTween.ShakePosition(gameObject, new Vector3(0.02f, 0.02f, 0), 0.5f);
        }
        UpdateBtn();

    }


    /// <summary>
    /// 结算
    /// </summary>
    void CheckResult()
    {
        int[] list_target = new int[3] {mTarget_1, mTarget_2, mTarget_3};
        int reward_base = 50;
         if(GetPicCount(list_target, 0) == 3)//777
         {
             GameDataCenter.Instance.mRollMachine.mCount777++;
             GameDataCenter.Instance.mRollMachine.mExperience += 100;
             reward_base = 999;
             On777Effect();
         }
        else if(GetPicCount(list_target, 0) == 2)
        {
            if(GetPicCount(list_target, 1) == 1)//B77
            {
                GameDataCenter.Instance.mRollMachine.mCountB77++;
                GameDataCenter.Instance.mRollMachine.mExperience += 50;
                reward_base = 700;
            }
            else//Z77
            {
                GameDataCenter.Instance.mRollMachine.mCountZ77++;
                GameDataCenter.Instance.mRollMachine.mExperience += 45;
                reward_base = 550;
            }
        }
         else if(GetPicCount(list_target, 1) == 3)//3个博士头
         {
             GameDataCenter.Instance.mRollMachine.mExperience += 40;
             reward_base = 400;
         }
         else if(GetPicCount(list_target, 1) == 2)//2博士头
         {
             if(GetPicCount(list_target, 0) == 1)//1个7
             {
                 GameDataCenter.Instance.mRollMachine.mExperience += 35;
                 reward_base = 300;                  
             }
             else//1个僵尸头
             {
                 GameDataCenter.Instance.mRollMachine.mExperience += 30;
                 reward_base = 250;     
             }
         }
         else if(GetPicCount(list_target, 2) == 3 || GetPicCount(list_target, 3) == 3 || GetPicCount(list_target, 4) == 3)//三个僵尸头
         {
             GameDataCenter.Instance.mRollMachine.mExperience += 25;
             reward_base = 200;    
         }
         else if (GetPicCount(list_target, 2) == 2 || GetPicCount(list_target, 3) == 2 || GetPicCount(list_target, 4) == 2)//两个僵尸头
         {
             if(GetPicCount(list_target, 0) == 1 || GetPicCount(list_target, 1) == 1)//博士或7
             {
                 GameDataCenter.Instance.mRollMachine.mExperience += 20;
                 reward_base = 150;   
             }
             else//僵尸
             {
                 GameDataCenter.Instance.mRollMachine.mExperience += 16;
                 reward_base = 100;   
             }
         }
         else
         {
             GameDataCenter.Instance.mRollMachine.mExperience += 11;
             reward_base = 50;   
         }

         int total_reword = GetReword(reward_base);


         Label_Reword.text = total_reword.ToString() + " × " + GameDataCenter.Instance.mYesterdayTaskMultiple.ToString() + " = [FF0000]" +
             ((int)(total_reword * GameDataCenter.Instance.mYesterdayTaskMultiple)).ToString() + "[-]";

         //GameDataCenter.Instance.AddMoney((int)(total_reword * GameDataCenter.Instance.mYesterdayTaskMultiple));
        int total_money = (int)(total_reword * GameDataCenter.Instance.mYesterdayTaskMultiple);

         StartCoroutine(CreateMoneys(total_money));
         GameDataCenter.Instance.mRollMachine.mFreeTimes--;

         int need_gem = (int)(GameDataCenter.Instance.mYesterdayTaskMultiple * 2);
         GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ERNIE, need_gem.ToString() + "_" + total_money.ToString());

         StopRunSound();

         UpdateBtn();
         UpdateText();
         StartFlashLight();
         Invoke("UnLight", 3);
         
    }


    /// <summary>
    /// 摇奖按钮点击
    /// </summary>
    void OnStartRoll()
    {
        if (mIsRolling)
            return;

        if (GameDataCenter.Instance.mRollMachine.mFreeTimes > 0)
        {
            StartRolling();
        }
        else
        {
            int need_gem = (int)(GameDataCenter.Instance.mYesterdayTaskMultiple * 2);
            if (!GameDataCenter.Instance.DeductionPrice(new CPrice(EPriceType.Gem, need_gem), ECostGem.ItemYaoJiang))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            StartRolling();
        }
    }


    AudioSource m777Roll;
    /// <summary>
    /// 开始滚动
    /// </summary>
    void StartRolling()
    {
        OnPlayRollBar();

        SetRollTarget();
        mCurrent1 = mPicIndex[0];
        mCurrent2 = mPicIndex[0];
        mCurrent3 = mPicIndex[0];
        mIsRolling = true;
        Label_Reword.text = "";

        mHasStop_1 = false;
        mHasStop_2 = false;
        mHasStop_3 = false;

        StartRunLight();
        CancelInvoke();
        StartRunSound();
    }




    //0-7   1-博士  2-僵尸1  3-僵尸2   4-僵尸3
     static float[] mPicIndex = new float[] {0.75f,  0.5f, 0.25f, 0f, 0f};
     void SetRollTarget()
     {
         int tar_1 = 3;
         int tar_2 = 3;
         int tar_3 = 3;


        int rnd = UnityEngine.Random.Range(0, 100);
        if ((rnd < 1 && GameDataCenter.Instance.mRollMachine.mCanGet777) || GameDataCenter.Instance.mRollMachine.mMustGet777)
        {
            tar_1 = 0;
            tar_2 = 0;
            tar_3 = 0;
        }
        else if (rnd < 6 && GameDataCenter.Instance.mRollMachine.mCanGetB77)
        {
            int[] b77_list = GetRandomList(new int[] {0, 0, 1 });
            tar_1 = b77_list[0];
            tar_2 = b77_list[1];
            tar_3 = b77_list[2];
        }
        else if (rnd < 26 && GameDataCenter.Instance.mRollMachine.mCanGetZ77)
        {
            int[] Z77_list = GetRandomList(new int[] { 0, 0, UnityEngine.Random.Range(2, 4)});
            tar_1 = Z77_list[0];
            tar_2 = Z77_list[1];
            tar_3 = Z77_list[2];
        }
        else
        {
            int[] roll_list = new int[3]{0, 0, 0};
            do
            {
                roll_list[0] = UnityEngine.Random.Range(0, 4);
                roll_list[1] = UnityEngine.Random.Range(0, 4);
                roll_list[2] = UnityEngine.Random.Range(0, 4);
            } while (SeventCount(roll_list) >= 2);
            tar_1 = roll_list[0];
            tar_2 = roll_list[1];
            tar_3 = roll_list[2];
        }

        mTarget_1 = tar_1;
        mTarget_2 = tar_2;
        mTarget_3 = tar_3;

        mTarget1 = mPicIndex[mTarget_1] + 4;
        mTarget2 = mPicIndex[mTarget_2] + 6; 
        mTarget3 = mPicIndex[mTarget_3] + 8;
    


     }

    /// <summary>
    /// 图标7的个数
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    int SeventCount(int[] list)
    {
        int count = 0;
        foreach (int i in list)
        {
            if (i == 0)
                count++;
        }
        return count;
    }

    /// <summary>
    /// 获取数组里某个值的个数
    /// </summary>
    /// <param name="list"></param>
    /// <param name="pic"></param>
    /// <returns></returns>
    int GetPicCount(int[] list, int pic)
    {
        int count = 0;
        foreach (int i in list)
        {
            if (i == pic)
                count++;
        }
        return count;
    }


    /// <summary>
    /// 打乱数组顺序
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    int[] GetRandomList(int[] list)
    {
        for (int i = 0; i < 10; i++ )
        {
            int first = UnityEngine.Random.Range(0, list.Length);
            int second = UnityEngine.Random.Range(0, list.Length);

            int tmp = list[first];
            list[first] = list[second];
            list[second] = tmp;
        }
        return list;
    }


    //GameObject[] mLights = new GameObject[] { };
    List<GameObject> mLights = new List<GameObject>();
    void AddLight(Vector3 pos)
    {
        GameObject obj = ResourcePath.Instance("Sprite_Light");
        obj.transform.parent = Panel_Light;
        obj.transform.localScale = new Vector3(16, 16, 1);
        obj.transform.localPosition = pos;
        obj.GetComponent<GUILaBaLight>().mIndex = mLights.Count;
        mLights.Add(obj);
    }

    /// <summary>
    /// 初始化创建灯
    /// </summary>
    void InitLight()
    {
        for(int i = 0; i < 9; i++)
        {
            AddLight(new Vector3(0 + 50 * i, 0, 0));
        }
        AddLight(new Vector3(405, -40, 0));
        AddLight(new Vector3(405, -80, 0));
        AddLight(new Vector3(405, -120, 0));
        for (int i = 8; i >= 0; i--)
        {
            AddLight(new Vector3(0 + 50 * i, -162, 0));
        }
        AddLight(new Vector3(-5, -120, 0));
        AddLight(new Vector3(-5, -80, 0));
        AddLight(new Vector3(-5, -40, 0));
    }





    IEnumerator CreateMoneys(int _money)
    {
        int per_value = _money / 10;
        for(int i = 0; i<10; i++)
        {
            CreateMoney(per_value);
            yield return new WaitForSeconds(0.1f);
        }
        if(_money % 10 != 0)
            CreateMoney(_money % 10);

    }

    void CreateMoney(int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameObject.Find("Panel_FlyItemList").transform;
        obj.transform.position = transform.position + new Vector3(0, 0, -1f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mValue = _value;
        obj.GetComponent<FlyAward>().FlyType = 3;
    }

    void UpdateBtn()
    {
        if(GameDataCenter.Instance.mRollMachine.mExperience >= 100 && GameDataCenter.Instance.mRollMachine.mLevel < 10)
        {
            OOTools.OOSetBtnSprite(IBtn_UpgradeRoll, "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
        }
        else
        {
            OOTools.OOSetBtnSprite(IBtn_UpgradeRoll, "Main_Btn0_No", "Main_Btn0_No", "Main_Btn0_No");
        }
        if(GameDataCenter.Instance.mRollMachine.mLevel == 10)
        {
            IBtn_UpgradeRoll.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.Tips_RollFullLevel);
            PB_Experience.gameObject.SetActiveRecursively(false);
        }
        else
        {
            IBtn_UpgradeRoll.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_UpgraRoll);
        }

    }

    void StopRunSound()
    {
        if (m777Roll)
            m777Roll.Stop();
    }

    /// <summary>
    /// 开始跑灯
    /// </summary>
    void StartRunSound()
    {

        ResourcePath.PlaySound("777RollStart");
        m777Roll = ResourcePath.PlaySound("777Roll", true);
    }



    /// <summary>
    /// 跑马灯逻辑
    /// </summary>
    void RunLightUpdate()
    {
        if(mIsLightRunning)
        {
            mRunLightCount += Time.deltaTime;
            if(mRunLightCount > max_count)
            {
                mRunLightCount = 0;
            }
        }
       
    }

    /// <summary>
    /// 开始跑
    /// </summary>
    void StartRunLight()
    {
        mIsLightRunning = true;
        mLightType = LightType.Light_Run;
        mRunLightCount = 0;
        max_count = 0.6f;
    }

    /// <summary>
    /// 开始闪
    /// </summary>
    void StartFlashLight()
    {
        mIsLightRunning = true;
        mLightType = LightType.Light_Flash;
        mRunLightCount = 0;
        max_count = 1f;
    }

    /// <summary>
    /// 停止
    /// </summary>
    void UnLight()
    {
        mIsLightRunning = false;
        mLightType = LightType.Light_Off;
        mRunLightCount = 0;
    }

    void OnIniRollBar()
    {
        AnimationRoll.Play("DH_YaoJiangJi");
        AnimationRoll.Pause("DH_YaoJiangJi");
    }

    void OnPlayRollBar()
    {
        AnimationRoll.StopAll();
        AnimationRoll.Play("DH_YaoJiangJi");
    }

    void On777Effect()
    {
        GameObject obj = ResourcePath.Instance("LaBa");
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Camera_2D.transform.parent;
        obj.transform.localScale = new Vector3(480, 480, 480);
        obj.transform.localPosition = new Vector3(-0.6f, 131f, -866f);
        Destroy(obj, 3f);
    }

    void TTT()
    {
        Texture2D t;
        //t 
    }
     
}
