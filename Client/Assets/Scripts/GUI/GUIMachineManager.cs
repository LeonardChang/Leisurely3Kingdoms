using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum EMachineType
{
    Irrigation = 0,
    Machine,
    Altar
}

/// <summary>
/// 机器/灌溉/祭坛 Panel管理
/// </summary>
public class GUIMachineManager : MonoBehaviour 
{
    public EMachineType mMachineType = EMachineType.Irrigation;

    public Transform Sprite_Back;
    public Transform[] Buttons;

    public GameObject IBtn_ItemInfo;
    //UILabel IBtn_ItemInfo_Label;

    //public GameObject 

    Vector3[] ButtonPos;
    public GameObject Panel_Buttons;
    public UISprite Sprite_Level;
    public UILabel Label_Level;

    public UILabel Info_1;
    public UILabel Info_2;

    public UISprite SpriteIcon_1;
    public UISprite SpriteIcon_2;
    //public UILabel Text_1;
    //public UILabel Text_2;

    public GameObject Btn_Close;
    public UILabel Label_MachineValue;
    /// <summary>
    /// 不同等级初始化的按钮列表
    /// </summary>
    int[][][] IniBtns = new int[][][]{  new int[][]{  
                                                             new int[]{0,1,4, 5},
                                                             new int[]{0,1,2,4, 5},
                                                             new int[]{0,1,2,4, 5},
                                                             new int[]{0,1,2,3,4, 5},
                                                             new int[]{1,2,3,4, 5}
                                                                    },
                                                new int[][]{  
                                                             new int[]{0,3, 5, 6},
                                                             new int[]{0,1,3, 5, 6},
                                                             new int[]{0,1,3,4, 5, 6},
                                                             new int[]{0,1,3,4, 5, 6},
                                                             new int[]{1,2,3,4, 5, 6}
                                                     
                                                                    },
                                                new int[][]{  
                                                             new int[]{0, 5,6},
                                                             new int[]{0,1, 5,6},
                                                             new int[]{0,1,3, 5,6},
                                                             new int[]{0,1,2,3, 5,6},
                                                             new int[]{1,2,3,4, 5,6}
                                                                    },
    };

    bool isIni = false;


    public GameObject[] Sprite_Icons;
    TweenRotation Current_Sprite_Icon;
    Dictionary<ESceneItemDataType, GameObject> mIconGroup;
	// Use this for initialization
	void Start () 
    {

	    ButtonPos = new Vector3[Buttons.Length];
        for (int i = 0; i < ButtonPos.Length; i++)
        {
            ButtonPos[i] = Buttons[i].localPosition;
        }
        mIconGroup = new Dictionary<ESceneItemDataType, GameObject>();
        switch(mMachineType)
        {
            case EMachineType.Irrigation:
                mIconGroup[ESceneItemDataType.KeepRun] = Sprite_Icons[0];
                mIconGroup[ESceneItemDataType.KeepRun2] = Sprite_Icons[1];
                mIconGroup[ESceneItemDataType.KeepRun3] = Sprite_Icons[2];
                mIconGroup[ESceneItemDataType.KeepRun4] = Sprite_Icons[3];
                break;
            case EMachineType.Machine:
                mIconGroup[ESceneItemDataType.SpeedUp] = Sprite_Icons[0];
                mIconGroup[ESceneItemDataType.SpeedUp2] = Sprite_Icons[1];
                mIconGroup[ESceneItemDataType.SpeedUp3] = Sprite_Icons[2];

                break;
            case EMachineType.Altar:
                mIconGroup[ESceneItemDataType.Candle] = Sprite_Icons[0];
                mIconGroup[ESceneItemDataType.Candle2] = Sprite_Icons[1];
                mIconGroup[ESceneItemDataType.Flash] = Sprite_Icons[2];
                mIconGroup[ESceneItemDataType.Flash2] = Sprite_Icons[3];

                break;
        }

        foreach (GameObject obj in mIconGroup.Values)
        {
            TweenRotation rot = obj.AddComponent<TweenRotation>();
            rot.enabled = false;
            rot.from = new Vector3(0, 0, -20);
            rot.to = new Vector3(0, 0, 20);
            rot.duration = 0.5f;
            rot.method = UITweener.Method.EaseInOut;
            rot.style = UITweener.Style.PingPong;
        }
	}
	
    void OnEnable()
    {
        isIni = false;
    }
    
    void Awake()
    {
        mRareString = StringTable.GetString(EStringIndex.UIText_RareString).Split('_');
        mGrowString = StringTable.GetString(EStringIndex.UIText_SpeedString).Split('_');
        mPropertyString = StringTable.GetString(EStringIndex.UIText_PropertyString).Split('_');
    }

    string[] mRareString;
    float[] RARE_LEVEL = new float[] {0, 6.5f, 13, 19.5f, 26};
    string GetRareString(int _value)
    {
        int string_index = 0;
        for(int i = 0; i < RARE_LEVEL.Length; i++)
        {
            if(_value > RARE_LEVEL[i])
            {
                string_index = i;
            }
        }
        return mRareString[string_index];
    }

    string[] mGrowString;
    float[] GROW_LEVEL = new float[] {0, 8,16,24,32 };
    string GetGrowString(int _value)
    {
        int string_index = 0;
        for (int i = 0; i < GROW_LEVEL.Length; i++)
        {
            if (_value > GROW_LEVEL[i])
            {
                string_index = i;
            }
        }
        return mGrowString[string_index];
    }

    string[] mPropertyString;
    float[] PROPERTY_LEVEL = new float[]{0, 6.5f, 13, 19.5f, 26};
    string GetPropertyString(int _value)
    {
        int string_index = 0;
        for (int i = 0; i < PROPERTY_LEVEL.Length; i++)
        {
            if (_value > PROPERTY_LEVEL[i])
            {
                string_index = i;
            }
        }
        return mPropertyString[string_index];
    }


    /// <summary>
    /// 更新机器信息
    /// </summary>
    void UpdateShowInfo()
    {
        switch (mMachineType)
        {
            case EMachineType.Irrigation:
                Info_1.text = GetTimeString(GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.mLavaTime);
                Label_MachineValue.text = GetRareString(GameDataCenter.Instance.GetCurrentScene().RareValue);//string.Format(StringTable.GetString(EStringIndex.UIText_KeepUp), GameDataCenter.Instance.GetCurrentScene().RareValue);
                if (GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.mLavaTime > 0)
                {
                    if (!mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.Type].GetComponent<TweenRotation>().enabled = true;
                }
                else
                {
                    if (mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.Type].GetComponent<TweenRotation>().enabled = false;
                }
                
                break;
            case EMachineType.Machine:
                Info_1.text = GetTimeString(GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.mLavaTime);
                Info_2.text = GameDataCenter.Instance.GetCurrentScene().SceneMachine.HoleAmount.ToString() + "/30";
                Label_MachineValue.text = GetGrowString(GameDataCenter.Instance.GetCurrentScene().GrowSpeed);//string.Format(StringTable.GetString(EStringIndex.UIText_SpeedUp), GameDataCenter.Instance.GetCurrentScene().GrowSpeed);


                if (GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.mLavaTime > 0)
                {
                    if (!mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.Type].GetComponent<TweenRotation>().enabled = true;
                }
                else
                {
                    if (mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.Type].GetComponent<TweenRotation>().enabled = false;
                }              
                break;
            case EMachineType.Altar:
                Info_1.text = GetTimeString(GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.mLavaTime);
                Info_2.text = GetTimeString(GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.mLavaTime);
                Label_MachineValue.text = GetPropertyString(GameDataCenter.Instance.GetCurrentScene().Property);//string.Format(StringTable.GetString(EStringIndex.UIText_Candle), GameDataCenter.Instance.GetCurrentScene().Property);

                if (GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.mLavaTime > 0)
                {
                    if (!mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.Type].GetComponent<TweenRotation>().enabled = true;
                }
                else
                {
                    if (mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.Type].GetComponent<TweenRotation>().enabled = false;
                }


                if (GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.mLavaTime > 0)
                {
                    if (!mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.Type].GetComponent<TweenRotation>().enabled = true;
                }
                else
                {
                    if (mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.Type].GetComponent<TweenRotation>().enabled)
                        mIconGroup[GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.Type].GetComponent<TweenRotation>().enabled = false;
                } 





                if (GameDataCenter.Instance.GetCurrentScene().SceneAltar.Level >= 3)
                {
                    Info_2.alpha = 1;
                    SpriteIcon_2.alpha = 1;
                }
                else
                {
                    Info_2.alpha = 0;
                    SpriteIcon_2.alpha = 0;
                }
                break;
        }
    }


	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;
            ActiveBtn();
        }
        UpdateShowInfo();

	}


    string GetTimeString(int _time)
    {
        int time_second = _time;
            int time_minute = time_second / 60;
            time_second = time_second % 60;

            int time_hour = time_minute / 60;
            time_minute = time_minute % 60;

            string time_string = string.Format("{0:D2}:{1:D2}:{2:D2}", time_hour, time_minute, time_second);
            return time_string;
    }

    class CSceneItemSprite
    {
        public string mName = "";
        public float mWidth = 0;
        public float mHeight = 0;
        public CSceneItemSprite(string _name, float _width, float _height)
        {
            mName = _name;
            mWidth = _width;
            mHeight = _height;
        }
        public CSceneItemSprite()
        {
            
        }
    }

    /// <summary>
    /// 设置使用中的图标
    /// </summary>
    /// <param name="_sprite"></param>
    /// <param name="_type"></param>
    /// <returns></returns>
    CSceneItemSprite SetSprite(UISprite _sprite, ESceneItemDataType _type)
    {
        CSceneItemSprite ret = new CSceneItemSprite() ;

        switch(_type)
        {
            case ESceneItemDataType.KeepRun:
                ret = new CSceneItemSprite("MachineI_Icon_LvPing", 55, 70);
                break;
            case ESceneItemDataType.KeepRun2:
                ret = new CSceneItemSprite("MachineI_Icon_LanPing", 55, 70);
                break;
            case ESceneItemDataType.KeepRun3:
                ret = new CSceneItemSprite("MachineI_Icon_MeiHongPing", 55, 70);
                break;
            case ESceneItemDataType.KeepRun4:
                ret = new CSceneItemSprite("MachineI_Icon_ZiPing", 55, 70);
                break;
            case ESceneItemDataType.SpeedUp:
                ret = new CSceneItemSprite("MachineP_Icon_JiaSu", 59, 39);
                break;
            case ESceneItemDataType.SpeedUp2:
                ret = new CSceneItemSprite("MachineP_Icon_DaJiaSu", 95, 39);
                break;
            case ESceneItemDataType.SpeedUp3:
                ret = new CSceneItemSprite("MachineP_Icon_ChaoJiJiaSu", 95, 39);
                break;
            case ESceneItemDataType.Candle:
                ret = new CSceneItemSprite("MachineA_Icon_LaZu", 37, 63);
                break;
            case ESceneItemDataType.Candle2:
                ret = new CSceneItemSprite("MachineA_Icon_HaoHuaLaZu", 60, 66);
                break;
            case ESceneItemDataType.Flash:
                ret = new CSceneItemSprite("MachineA_Icon_ShanDian", 52, 34);
                break;
            case ESceneItemDataType.Flash2:
                ret = new CSceneItemSprite("MachineA_Icon_GaoJiShanDian", 61, 49);
                break;
        }

        _sprite.spriteName = ret.mName;
        _sprite.transform.localScale = new Vector3(ret.mWidth * 0.7f, ret.mHeight * 0.7f, 1);
        return ret;
    }


    /// <summary>
    /// 获取当前等级
    /// </summary>
    /// <returns></returns>
    public int GetLevel()
    {
        int level = 1;
        switch (mMachineType)
        {
            case EMachineType.Irrigation:
                level = GameDataCenter.Instance.GetCurrentScene().SceneIrrigation.Level;
                SetSprite(SpriteIcon_1, GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.Type);

                break;
            case EMachineType.Machine:
                level = GameDataCenter.Instance.GetCurrentScene().SceneMachine.Level;
                SetSprite(SpriteIcon_1, GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.Type);

                break;
            case EMachineType.Altar:
                level = GameDataCenter.Instance.GetCurrentScene().SceneAltar.Level;
                SetSprite(SpriteIcon_1, GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.Type);
                SetSprite(SpriteIcon_2, GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.Type);
                break;
        }
        return level;
    }

    /// <summary>
    /// 获取当前等级按钮个数
    /// </summary>
    /// <returns></returns>
    public int GetBtnCount()
    {
        int level = GetLevel();
        return IniBtns[(int)mMachineType][level - 1].Length;
    }

    /// <summary>
    /// 初始化按钮
    /// </summary>
    void ActiveBtn()
    {
        foreach (Transform pos in Panel_Buttons.transform)
        {
            pos.gameObject.SetActiveRecursively(false);
        }
        int level = GetLevel();

        for (int i = 0; i < IniBtns[(int)mMachineType][level - 1].Length ; i++)
        {
            Buttons[IniBtns[(int)mMachineType][level - 1][i]].gameObject.SetActiveRecursively(true);
            Buttons[IniBtns[(int)mMachineType][level - 1][i]].localPosition = ButtonPos[i];
        }
        Sprite_Back.transform.localScale = new Vector3(477, (IniBtns[(int)mMachineType][level - 1].Length) * 100 + 150, 1);

        //种植机器
        if (mMachineType == EMachineType.Machine)
        {
            Buttons[6].localPosition = ButtonPos[6];
            UILabel Label_Info = Buttons[6].Find("Label_Info").GetComponent<UILabel>();
            if (StringTable.mStringType != ELocalizationTyp.Chinese)
            {
                Label_Info.transform.localScale = new Vector3(23, 23, 1);
            }

            if (GameDataCenter.Instance.GetCurrentScene().SceneMachine.HoleAmount >= 30)
            {
                Buttons[5].localPosition = Buttons[4].localPosition;
                //Buttons[5].localPosition = Buttons[4].localPosition;

                Buttons[4].gameObject.SetActiveRecursively(false);
                Sprite_Back.transform.localScale += new Vector3(0, -100, 0);
            }
        }
        else if(mMachineType == EMachineType.Altar)//祭坛
        {
            Buttons[6].localPosition = ButtonPos[6];

        }



        //Sprite_Level.spriteName = "MachineP_" + level.ToString();
        //Sprite_Level.MakePixelPerfect();
        Label_Level.text = "Lv" + level.ToString();

        OnUpdatePrice();
        foreach (GameObject obj in mIconGroup.Values)
        {
            obj.GetComponent<TweenRotation>().enabled = false;
        }
    }


    /// <summary>
    /// 更新价格
    /// </summary>
    void OnUpdatePrice()
    {
        CPrice[] price_list = new CPrice[] { };
        int start_des = (int)EStringIndex.ItemIrrigationLvUp;

        switch (mMachineType)
        {
            case EMachineType.Irrigation:
                price_list = new CPrice[] 
                { 
                        GameDataCenter.Instance.GetCurrentIrrigationPrice(),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun2),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun3),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun4)
                    };
                start_des = (int)ESceneItemDataType.IrrigationLvUp;
                break;
            case EMachineType.Machine:
                price_list = new CPrice[] 
                { 
                        GameDataCenter.Instance.GetCurrentMachinePrice(),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp2),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp3),
                        GameDataCenter.Instance.GetCurrentHolePrice()
                    };
                start_des = (int)ESceneItemDataType.MachineLvUp;


                if (IsCanUse(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCatalyst)))
                {
                    Buttons[6].Find("Label_Money").GetComponent<UILabel>().color = Color.white;
                }
                else
                {
                    Buttons[6].Find("Label_Money").GetComponent<UILabel>().color = Color.red;
                }
                Buttons[6].Find("Label_Money").GetComponent<UILabel>().text = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCatalyst).Value.ToString();
                break;
            case EMachineType.Altar:
                price_list = new CPrice[] 
                { 
                        GameDataCenter.Instance.GetCurrentAltarPrice(),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCandle),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCandle2),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceFlash),
                        GlobalStaticData.GetPriceInfo(EPriceIndex.PriceFlash2)
                };
                if (IsCanUse(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZhanBu)))
                {
                    Buttons[6].Find("Label_Money").GetComponent<UILabel>().color = Color.white;
                }
                else
                {
                    Buttons[6].Find("Label_Money").GetComponent<UILabel>().color = Color.red;
                }
                Buttons[6].Find("Label_Money").GetComponent<UILabel>().text = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZhanBu).Value.ToString();
                start_des = (int)ESceneItemDataType.AltarLvUp;
                break;
        }
        

        for (int i = 0; i < price_list.Length; i++)
        {
            UILabel Label_Money = Buttons[i].Find("Label_Money").GetComponent<UILabel>();
            UISprite Sprite_Money =  Buttons[i].Find("Sprite_Money").GetComponent<UISprite>();
            UILabel Label_Info = Buttons[i].Find("Label_Info").GetComponent<UILabel>();

            Label_Info.text = GlobalStaticData.GetSceneItemInfo((ESceneItemDataType)(start_des + i)).Des;
            if(StringTable.mStringType != ELocalizationTyp.Chinese)
            {
                Label_Info.transform.localScale = new Vector3(23, 23, 1);
            }

            Label_Money.text = price_list[i].Value.ToString();

            if(Label_Money.text.Length == 3)
            {
                Label_Money.transform.localPosition = new Vector3(150, -6, -10);
            }
            else if(Label_Money.text.Length >= 4)
            {
                Label_Money.transform.localPosition = new Vector3(145, -10, -10);
            }
            else
            {
                Label_Money.transform.localPosition = new Vector3(160, -6, -10);
            }


            if (price_list[i].Type == EPriceType.Gem)
            {
               Sprite_Money.spriteName = "Main_Gem";
               Sprite_Money.transform.localScale = new Vector3(41f, 28.5f, 1f);

               OOTools.OOSetBtnSprite(Buttons[i].GetComponent<UIImageButton>(), "Main_Btn1_Nor", "Main_Btn1_Down", "Main_Btn1_Down");
               Buttons[i].transform.FindChild("Background").localScale = new Vector3(448, 120, 1);
            }
            else
            {
                Sprite_Money.spriteName = "Main_Coin";
                
                Sprite_Money.transform.localScale = new Vector3(36.5f, 31f, 1f);
                OOTools.OOSetBtnSprite(Buttons[i].GetComponent<UIImageButton>(), "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
                Buttons[i].transform.FindChild("Background").localScale = new Vector3(448, 100, 1);
            }

            if (IsCanUse(price_list[i]))
            {
                Label_Money.color = Color.white;
            }
            else
            {
                Label_Money.color = Color.red;
            }
        }

    }


    bool IsCanUse(CPrice _price)
    {
        if (_price.Type == EPriceType.Coin)
        {
            return GameDataCenter.Instance.GetCurrentMoney() >= _price.Value ? true : false;
        }
        else
        {
            return GameDataCenter.Instance.GetCurrentGem() >= _price.Value ? true : false;
        }
    }







    //升级灌溉系统
    void OnUpgradeIrrigation()
    {
        if(!GameDataCenter.Instance.DeductionPrice(GameDataCenter.Instance.GetCurrentIrrigationPrice(), ECostGem.IrrigationUp))
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
            return;
        }
        UseItem(ESceneItemDataType.IrrigationLvUp);
    }


    /// <summary>
    /// 确认使用
    /// </summary>
    /// <param name="_str"></param>
    void UseKeepRunOk(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= UseKeepRunOk;
        if (_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(mUsePrice, mCostGem))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            UseItem(mUseType);
        }

    }


    string GetItemName(ESceneItemDataType _type)
    {
        return string.Format(StringTable.GetString(EStringIndex.Tips_RePlaceItem),  "[FF0000]" +GlobalStaticData.GetSceneItemInfo(_type).Name + "[-]");
    }


    CPrice mUsePrice;
    ESceneItemDataType mUseType;
    ECostGem mCostGem = ECostGem.ItemCatalyst;
    /// <summary>
    /// 弹框确认使用
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_item"></param>
    void KeepRun(ESceneItemDataType _type, CSceneItem _item)
    {
        mUseType = _type;

        if (_item.GetPercent() > 0)
        {
            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetItemName(_item.Type),
                                                                                        StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += UseKeepRunOk;
            return;
        }
        UseKeepRunOk(StringTable.GetString(EStringIndex.Tips_OK));
    }


    //维持药剂
    void OnUseKeepRun()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun);
        mCostGem = ECostGem.ItemKeepUp;
        KeepRun(ESceneItemDataType.KeepRun, GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp);
    }
    //维持药剂2
    void OnUseKeepRun2()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun2);
        mCostGem = ECostGem.ItemKeepUp;
        KeepRun(ESceneItemDataType.KeepRun2, GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp);
    }
    //维持药剂3
    void OnUseKeepRun3()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun3);
        mCostGem = ECostGem.ItemKeepUp;
        KeepRun(ESceneItemDataType.KeepRun3, GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp);
    }
    //维持药剂4
    void OnUseKeepRun4()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceKeepRun4);
        mCostGem = ECostGem.ItemKeepUp;
        KeepRun(ESceneItemDataType.KeepRun4, GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp);
    }


    //升级祭坛
    void OnUpgradeAltar()
    {
        if (!GameDataCenter.Instance.DeductionPrice(GameDataCenter.Instance.GetCurrentAltarPrice(), ECostGem.AltarUp))
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
            return;
        }
        UseItem(ESceneItemDataType.AltarLvUp);
    }






    //蜡烛
    void OnUseCandle()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCandle);
        mCostGem = ECostGem.ItemCandle;
        KeepRun(ESceneItemDataType.Candle, GameDataCenter.Instance.GetCurrentScene().SceneItemCandle);
    }
    //豪华蜡烛
    void OnUseCandle2()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCandle2);
        mCostGem = ECostGem.ItemCandle;
        KeepRun(ESceneItemDataType.Candle2, GameDataCenter.Instance.GetCurrentScene().SceneItemCandle);
    }

    //升级机器
    void OnUpgradeMachine()
    {
        if (!GameDataCenter.Instance.DeductionPrice(GameDataCenter.Instance.GetCurrentMachinePrice(), ECostGem.MachineUp))
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
            return;
        }
        UseItem(ESceneItemDataType.MachineLvUp);
    }
    //加速
    void OnUseSpeedUp()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp);
        mCostGem = ECostGem.ItemSpeedUp;
        KeepRun(ESceneItemDataType.SpeedUp, GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp);
    }
    //小加速
    void OnUseSpeedUp2()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp2);
        mCostGem = ECostGem.ItemSpeedUp;
        KeepRun(ESceneItemDataType.SpeedUp2, GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp);
    }
    //大加速
    void OnUseSpeedUp3()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceSpeedUp3);
        mCostGem = ECostGem.ItemSpeedUp;
        KeepRun(ESceneItemDataType.SpeedUp3, GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp);
    }
    //增加坑
    void OnUseAddHole()
    {
        if (!GameDataCenter.Instance.DeductionPrice(GameDataCenter.Instance.GetCurrentHolePrice(), ECostGem.ItemAddHole))
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
            return;
        }

        UseItem(ESceneItemDataType.AddHole);
    }
    //闪电
    void OnUseFlash()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceFlash);
        mCostGem = ECostGem.ItemFlash;
        KeepRun(ESceneItemDataType.Flash, GameDataCenter.Instance.GetCurrentScene().SceneItemFlash);
    }
    //高级闪电
    void OnUseFlash2()
    {
        mUsePrice = GlobalStaticData.GetPriceInfo(EPriceIndex.PriceFlash2);
        mCostGem = ECostGem.ItemFlash;
        KeepRun(ESceneItemDataType.Flash2, GameDataCenter.Instance.GetCurrentScene().SceneItemFlash);
    }

    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="_type"></param>
    void UseItem(ESceneItemDataType _type)
    {
        GameDataCenter.Instance.GetCurrentScene().UseItem(_type);

        GameDataCenter.Instance.GuiManager.Panel_Manager.CloseAll();

        switch(_type)
        {
            case ESceneItemDataType.KeepRun2:
            case ESceneItemDataType.KeepRun3:
            case ESceneItemDataType.SpeedUp2:
            case ESceneItemDataType.Candle:
            case ESceneItemDataType.Flash:
                GameDataCenter.Instance.ActiveSmallHappy();
                break;
            case ESceneItemDataType.KeepRun4:
            case ESceneItemDataType.SpeedUp3:
            case ESceneItemDataType.Candle2:
            case ESceneItemDataType.Flash2:
            case ESceneItemDataType.IrrigationLvUp:
            case ESceneItemDataType.MachineLvUp:
            case ESceneItemDataType.AltarLvUp:
                GameDataCenter.Instance.ActiveBigHappy();
                break;
        }

        switch (_type)
        {
            case ESceneItemDataType.KeepRun:
                if(GameDataCenter.Instance.mIsFirstKeepRun30M)
                {
                    GameDataCenter.Instance.mIsFirstKeepRun30M = false;
                    GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_FirstKeepRun30M));
                }
                break;
            case ESceneItemDataType.KeepRun2:
                if (GameDataCenter.Instance.mIsFirstKeepRun1H)
                {
                    GameDataCenter.Instance.mIsFirstKeepRun1H = false;
                    GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_FirstKeepRun1H));
                }
                break;
            case ESceneItemDataType.KeepRun3:
                if (GameDataCenter.Instance.mIsFirstKeepRun4H)
                {
                    GameDataCenter.Instance.mIsFirstKeepRun4H = false;
                    GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_FirstKeepRun4H));
                }
                break;
            case ESceneItemDataType.KeepRun4:
                if (GameDataCenter.Instance.mIsFirstKeepRun24H)
                {
                    GameDataCenter.Instance.mIsFirstKeepRun24H = false;
                    GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_FirstKeepRun24H));
                }
                break;

        }
        GameDataCenter.Instance.ForceSave();
    }


    /// <summary>
    /// 催化剂
    /// </summary>
    void OnGrowFast()
    {

        if (GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceCatalyst), ECostGem.ItemCatalyst))
        {
            GameDataCenter.Instance.GuiManager.Panel_Manager.CloseAll();

            for (int i = 0; i < 30; i++)
            {
                GameDataCenter.Instance.GetCurrentScene().CreateRandomZombie();
            }
            foreach (CZombie zombie in GameDataCenter.Instance.GetCurrentScene().mZombies)
            {
                zombie.Nutrient = 60;
            }
        }
        else
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
        }
    }

    /// <summary>
    /// 占卜
    /// </summary>
    void OnZhanBu()
    {
        if (GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceZhanBu), ECostGem.ItemZhanbu))
        {
            GameDataCenter.Instance.GuiManager.Panel_Manager.CloseAll();
            //ZombieType type = GameDataCenter.Instance.GetCurrentRandomZombie();

            GameDataCenter.Instance.GuiManager.Zombie_Manager.CreateOneRandomZombie();
        }
        else
        {
            GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
        }
    }


}
