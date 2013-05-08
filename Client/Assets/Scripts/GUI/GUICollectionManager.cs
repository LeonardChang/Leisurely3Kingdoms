using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using EasyMotion2D;

/// <summary>
/// 陈列室详情
/// 每只僵尸的详情面板。
/// </summary>
public class GUICollectionManager : MonoBehaviour 
{
    static int SHOW_COUNT = 45;


    public static float Max_XiYou = 100f;
    public static float Max_Attack = 2004f;
    public static float Max_Value = 644f;

    /// <summary>
    /// 超级星星（积满3颗星星出现）
    /// </summary>
    public UISprite Sprite_SuperTitle;

    /// <summary>
    /// 僵尸数据信息
    /// </summary>
    public CZombieData mZombieData;

    /// <summary>
    /// 面板是否在滑动
    /// </summary>
    public bool isMoving = false;


    CZombieData[] collectionList = GameDataCenter.Instance.ZombieCollection;

    /// <summary>
    /// 编号Label
    /// </summary>
    public UILabel Label_Number;

    /// <summary>
    /// 僵尸Label
    /// </summary>
    public UILabel Label_Name;

    /// <summary>
    /// 收集数量
    /// </summary>
    public UILabel Label_Collect_times;

    /// <summary>
    /// 星星列表
    /// </summary>
    public UISprite[] Sprite_Stars;

    /// <summary>
    /// 稀有条
    /// </summary>
    public UISlider PB_Xiyou;

    /// <summary>
    /// 攻击条
    /// </summary>
    public UISlider PB_Attack;

    /// <summary>
    /// 价值条
    /// </summary>
    public UISlider PB_Value;

    /// <summary>
    /// 僵尸动画
    /// </summary>
    public SpriteAnimation Sprite_Animation;

    /// <summary>
    /// 僵尸背景
    /// </summary>
    public UISprite Background;

    /// <summary>
    /// 关闭按钮
    /// </summary>
    public UIButtonMessage IBtn_Back;

    private bool isIni = false;
    //public ZombieMotionList mMotionList;

    /// <summary>
    /// 能力Label
    /// </summary>
    public UILabel Label_Able;
    /// <summary>
    /// 描述Label
    /// </summary>
    public UILabel Label_Description;

    /// <summary>
    /// 升级攻击力
    /// </summary>
    public UIImageButton IBtn_UpAttack;
    /// <summary>
    /// 升级价值
    /// </summary>
    public UIImageButton IBtn_UpValue;

    /// <summary>
    /// UIRoot
    /// </summary>
    public GUIManager GUIRoot;

    /// <summary>
    /// 攻击力升级价格
    /// </summary>
    CPrice[] Attack_Price;
    /// <summary>
    /// 价值升级价格
    /// </summary>
    CPrice[] Value_Price;

    /// <summary>
    /// 设置升级按钮上的星星以及按钮状态
    /// </summary>
    /// <param name="_iBtn"></param>
    /// <param name="_level"></param>
    void SetBtn(UIImageButton _iBtn, int _level)
    {
        UISprite Star_1 = _iBtn.transform.FindChild("Spr_star_1").GetComponent<UISprite>();
        UISprite Star_2 = _iBtn.transform.FindChild("Spr_star_2").GetComponent<UISprite>();
        UISprite Star_3 = _iBtn.transform.FindChild("Spr_star_3").GetComponent<UISprite>();

        if(_level > 1)
        {
            Star_1.spriteName = "Panel_Collection_Star";
        }
        if(_level > 2)
        {
            Star_2.spriteName = "Panel_Collection_Star";
        }
        if(_level > 3)
        {
            Star_3.spriteName = "Panel_Collection_Star";

            OOTools.OOSetBtnSprite(_iBtn, "Main_Btn1_Nor", "Main_Btn1_Nor", "Main_Btn1_Nor");
        }

    }

    /// <summary>
    /// 点击星星
    /// </summary>
    void OnClickStar()
    {
        if(mZombieData.IsOpen)
        {
            GlobalModule.Instance.ShowMessageBoxSigh(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_UpToLightStar),
                    StringTable.GetString(EStringIndex.Tips_OK));
        }

    }

    void SetAttackBtn()
    {
        SetBtn(IBtn_UpAttack, mZombieData.AttackLevel);
    }



    /// <summary>
    /// 确认升级攻击
    /// </summary>
    /// <param name="_str"></param>
    void UGAttack(string _str)
    {
        int start = 0;
        int end = 0;
        MessageBoxSpecial.alterButtonClickedEvent -= UGAttack;
        if (_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(Attack_Price[mZombieData.AttackLevel - 1], ECostGem.ZombieUpAttack))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            start = (int)mZombieData.Attack;
            switch (mZombieData.AttackLevel)
            {
                case 1:
                case 2:
                    mZombieData.AttackLevel++;
                    mZombieData.Attack++;
                    break;
                case 3:
                    mZombieData.AttackLevel++;
                    mZombieData.Attack *= 2;
                    break;
            }
            end = (int)mZombieData.Attack;

            PB_Attack.sliderValue = mZombieData.Attack / Max_Attack;
            Invoke("SetAttackBtn", 1);
            ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp3);
            GameDataCenter.Instance.ForceSave();
            //GameDataCenter.Instance.GuiManager.Panel_Manager.OnOpenZombieUpProgress(mZombieData.Type, start, end, 0, (int)Max_Attack, mZombieData.AttackLevel);
        }
    }



    void SetValueBtn()
    {
        SetBtn(IBtn_UpValue, mZombieData.ValueLevel);
    }
    /// <summary>
    /// 确认升级价值
    /// </summary>
    /// <param name="_str"></param>
    void UGValue(string _str)
    {
        int start;
        int end;
        MessageBoxSpecial.alterButtonClickedEvent -= UGValue;
        if (_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(Value_Price[mZombieData.ValueLevel - 1], ECostGem.ZombieUpValue))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            start = mZombieData.Value;
            switch (mZombieData.ValueLevel)
            {
                case 1:
                case 2:
                    mZombieData.ValueLevel++;
                    mZombieData.Value++;
                    break;
                case 3:
                    mZombieData.ValueLevel++;
                    mZombieData.Value *= 2;
                    break;
            }
            end = mZombieData.Value;
            PB_Value.sliderValue = mZombieData.Value / Max_Value;
            Invoke("SetValueBtn", 1);
            ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp3);
            GameDataCenter.Instance.ForceSave();
            //GameDataCenter.Instance.GuiManager.Panel_Manager.OnOpenZombieUpProgress(mZombieData.Type, start, end, 1, (int)Max_Value, mZombieData.ValueLevel);
        }
    }

    /// <summary>
    /// 获取价格文本
    /// </summary>
    /// <param name="_price"></param>
    /// <returns></returns>
    string GetPriceString(CPrice _price)
    {
        string coin = StringTable.GetString(EStringIndex.Tips_Coin);
        string gem = StringTable.GetString(EStringIndex.Tips_Gem);
        string price = _price.Type == EPriceType.Coin ? "[0000FF]" + _price.Value.ToString() + "[-]" + coin : "[0000FF]" + _price.Value.ToString() + "[-]" + gem;
        return price;
    }

    /// <summary>
    /// 获取提示文本。
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    string GetTips(int _index)
    {
        string ret;
        //0-攻击
        if(_index == 0)
        {
            ret = string.Format(StringTable.GetString(EStringIndex.Tips_UpAttack), GetPriceString(Attack_Price[mZombieData.AttackLevel - 1]));
        }
        else
        {
            ret = string.Format(StringTable.GetString(EStringIndex.Tips_UpValue), GetPriceString(Attack_Price[mZombieData.ValueLevel - 1]));
        }
        return ret;
    }

    /// <summary>
    /// 升级攻击点击
    /// </summary>
    void UpAttack()
    {
        if(mZombieData.AttackLevel > 3)
        {
            return;
        }

        GameDataCenter.Instance.GuiManager.Panel_Manager.OnOpenZombieUpProgress(mZombieData.Type, 0);
        /*
        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetTips(0),
                StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
        MessageBoxSpecial.alterButtonClickedEvent += UGAttack;
         */ 
    }

    /// <summary>
    /// 升级价值点击
    /// </summary>
    void UpValue()
    {
        if (mZombieData.ValueLevel > 3)
        {
            return;
        }

        GameDataCenter.Instance.GuiManager.Panel_Manager.OnOpenZombieUpProgress(mZombieData.Type, 1);
        /*
        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetTips(1),
                StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
        MessageBoxSpecial.alterButtonClickedEvent += UGValue;
         */ 
    }


    float mAlpha = 0;
	// Use this for initialization
	void Start () 
    {
        GUIRoot = GameDataCenter.Instance.GuiManager;

        Attack_Price = new CPrice[]
        {
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieAttackLv1),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieAttackLv2),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieAttackLv3)
        };
        Value_Price = new CPrice[]
        {
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieValue1),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieValue2),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZombieValue3)
        };


	}
	


	// Update is called once per frame
	void Update () 
    {

	    if(!isIni)
        {
            mZombieData.IsNew = false;
            UpdateText();
            isIni = true;
        }

        if (GetComponent<TweenPosition>().enabled)
        {
            mAlpha = 0;
        }
        else
        {
            mAlpha = 1;
        }

        Color color = Sprite_Animation.GetComponent<SpriteRenderer>().color;
        Sprite_Animation.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, mAlpha);
        
        if(mZombieData.Count > 0)
            UpdateSlider();

        UpdateStar();
	}

    void UpdateSlider()
    {

        PB_Xiyou.sliderValue = mZombieData.ZombieInfo.XiYou / Max_XiYou;
        PB_Attack.sliderValue = mZombieData.Attack / Max_Attack;
        PB_Value.sliderValue = mZombieData.Value / Max_Value;
    }

    


    /// <summary>
    /// 更新星星
    /// </summary>
    void UpdateStar()
    { 
        int star = 0;
        if(mZombieData.Count >= 50)
        {
            star++;
        }
        if(mZombieData.ValueLevel >= 4)
        {
            star++;
        }
        if(mZombieData.AttackLevel >= 4)
        {
            star++;
        }

        for(int i = 0; i < star; i ++)
        {
            Sprite_Stars[i].color = Color.white;
        }
        if (star >= 3)
        {
            Sprite_SuperTitle.color = Color.white;
        }
    }
     
    /// <summary>
    /// 僵尸动画初始化
    /// </summary>
    void IniMotion()
    {

        SpriteAnimationClip[] animationList = SpriteAnimationUtility.GetAnimationClips(Sprite_Animation);

        animationList[0] = ResourcePath.GetZombieStand(mZombieData.Type);
        SpriteAnimationUtility.SetAnimationClips(Sprite_Animation, animationList);
        foreach (SpriteAnimationComponent a in animationList[0].subComponents)
        {
            if (a.name == "前土" || a.name == "后土")
            {
                a.visible = false;
            }
        }

        Sprite_Animation.Play(animationList[0].name);

        if(!mZombieData.IsOpen)
        {
            Sprite_Animation.Pause(animationList[0].name);
        }
    }


    /// <summary>
    /// 初始化界面信息
    /// </summary>
    void UpdateText()
    {

        Sprite_Animation.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, mAlpha);

        IBtn_UpAttack.gameObject.SetActiveRecursively(false);
        IBtn_UpValue.gameObject.SetActiveRecursively(false);

        //StartCoroutine(IniMotion());
        IniMotion();
        Label_Collect_times.text = string.Format(StringTable.GetString(EStringIndex.UIText_ZombieTimes), mZombieData.Count);
        Label_Number.text = string.Format("{0:D3}", (int)mZombieData.Type);
        PB_Attack.transform.FindChild("Label_Attack").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_ZombieAttack);
        PB_Value.transform.FindChild("Label_Value").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_ZombieValue);
        PB_Xiyou.transform.FindChild("Label_Xiyou").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_ZombieRare);


        if(mZombieData.IsOpen)
        {
            Label_Able.text = mZombieData.ZombieInfo.Ability;
            Label_Description.text = mZombieData.ZombieInfo.Dsc;

            IBtn_UpAttack.gameObject.SetActiveRecursively(true);
            IBtn_UpValue.gameObject.SetActiveRecursively(true);

            SetBtn(IBtn_UpAttack, mZombieData.AttackLevel);
            SetBtn(IBtn_UpValue, mZombieData.ValueLevel);

            Label_Name.text = mZombieData.ZombieInfo.Name; 
            PB_Xiyou.sliderValue = mZombieData.ZombieInfo.XiYou / Max_XiYou;
            PB_Attack.sliderValue = mZombieData.Attack / Max_Attack;
            PB_Value.sliderValue = mZombieData.Value / Max_Value;




            Sprite_Animation.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, mAlpha);
            Background.spriteName = "Panel_Collection_ZombieBack_Open";

        }
        else
        {
            //Sprite_Animation.Pause(animationList[0].name);
            Label_Name.text = "???";
            PB_Xiyou.sliderValue = 0;
            PB_Attack.sliderValue = 0;
            PB_Value.sliderValue = 0;
            Label_Number.color = Color.black;
        }
        IBtn_Back.target = GUIRoot.gameObject;

        if(StringTable.mStringType != ELocalizationTyp.Chinese)
        {
            GameDataCenter.Instance.GuiManager.GetComponent<GUIChange2En>().TransAll(transform);
        }

    }

    /// <summary>
    /// 右移
    /// </summary>
    void OnMoveLeft()
    {
        OOTools.OOTweenPosition(gameObject, new Vector3(0, 0, -550), new Vector3(800, 0, -550));
        isMoving = true;
    }

    /// <summary>
    /// 
    /// </summary>
    void OnCreateLeft()
    {
        int index = (int)mZombieData.Type - 2;
        if (index < 0)
        {
            index = (int)SHOW_COUNT - 1;
        }
        GUIRoot.CreateMoveLeftZombieInfo(collectionList[index]);
    }


    public void OnLeft()
    {
        OnMoveLeft();
        OnCreateLeft();
    }



    void OnMoveRight()
    {
        OOTools.OOTweenPosition(gameObject, new Vector3(0, 0, -550), new Vector3(-800, 0, -550));
        isMoving = true;
    }

    void OnCreateRight()
    {
        int index = (int)mZombieData.Type;
        if (index >= SHOW_COUNT)
        {
            index = 0;
        }
        GUIRoot.CreateMoveRightZombieInfo(collectionList[index]);
    }

    public void OnRight()
    {
        OnMoveRight();
        OnCreateRight();
    }

    void OnDragEnd()
    {
        if(isMoving)
        {
            Destroy(gameObject);
        }
    }

    void OnDrag(Vector2 drag)
    {
        if(isMoving)
        {
            return;
        }
        int width = Screen.width;
        

        if(drag.x  < -20f * width / 640f)
        {
            OnRight();
        }
        else if(drag.x > 20f * width / 640f)
        {
            OnLeft();
        }
    }

    public void GetOut()
    {
        OnMoveLeft();
    }



}
