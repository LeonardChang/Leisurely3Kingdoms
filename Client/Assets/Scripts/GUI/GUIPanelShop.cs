using UnityEngine;
using System.Collections;

public class GUIPanelShop : MonoBehaviour {

    string[] PRICE_ENGLISH = new string[] { "$0.99", "$4.99", "$9.99", "$19.99"};
    string[] PRICE_CHINESE = new string[] { "￥6", "￥25", "￥68", "￥128" };

    string[] mPriceText;
    //9  8.5  7.5
    public Transform IBtn_Change;
    public GameObject[] Group_1;
    public GameObject[] Group_2;

    public UIScrollBar ScrollBar;
    public UISlider SliderBar;

    public UISprite Spr_Change;
    public UILabel Label_ChangeNeed;

    void OnDisable()
    {
        //GlobalModule.AddCrystalEvent -= AddGem;
    }

    void BuyMM30()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.BuyMM30Crystal);
    }

    void BuyMM55()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.BuyMM55Crystal);
    }

    void BuyMM115()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.BuyMM115Crystal);
    }

    void BuyMM360()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.BuyMM360Crystal);
    }



    void Buy60()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.Buy60Crystal);
    }

    void Buy320()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.Buy320Crystal);
    }

    void Buy680()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.Buy680Crystal);
    }


    void Buy1400()
    {
        GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.Buy1400Crystal);
    }

    void Buy172()
    {
        //GlobalModule.Instance.BuyAppStoreItem(AppStoreItemID.Buy)
    }




    //0 正常 1 中移动
    int group = 1;
	void Start () 
    {
        string txt = GlobalStaticData.GetOptionValue("ShopType");
        group = int.Parse(txt);

        mPriceText = StringTable.GetString(EStringIndex.UIText_ShopMoney).Split('_');
	}

    void OnEnable()
    {        
        //GlobalModule.AddCrystalEvent += AddGem;
        isIni = false;
    }

    bool isIni = false;
    void Ini()
    { 
        SetTodayGemLabel();
        if(!isIni)
        {

            int index = 0;
            foreach (GameObject obj in Group_2)
            {
                obj.transform.FindChild("Label").GetComponent<UILabel>().text = mPriceText[index];

                index++;
            }

            isIni = true;
            if (group == 0)
            {
                foreach (GameObject obj in Group_1)
                {         

                        obj.SetActiveRecursively(false); 
                  
                }
            }
            else if (group == 1)
            {
                foreach (GameObject obj in Group_2)
                {
                        obj.SetActiveRecursively(false);
                }
            }
        }
    }
	// Update is called once per frame
	void Update () 
    {
        Ini();
        SliderBar.sliderValue = ScrollBar.scrollValue;
	}

    /// <summary>
    /// 创建宝箱
    /// </summary>
    void CreateMoney()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Panel_Main.transform;
        obj.transform.position = IBtn_Change.position + new Vector3(0, 0.125f, -0.5f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mSpecial = 1;
        obj.GetComponent<FlyAward>().mValue = 1000;
        obj.GetComponent<FlyAward>().FlyType = 1;
    }

    /// <summary>
    /// 获取下次兑换所需水晶
    /// </summary>
    /// <returns></returns>
    int GetTodayChangeGem()
    {
        int ret = 10;

        if (GameDataCenter.Instance.mTodayChangeTimes < 2)
        {
            ret = 10;
        }
        else if (GameDataCenter.Instance.mTodayChangeTimes < 4)
        {
            ret = 15;
        }
        else if (GameDataCenter.Instance.mTodayChangeTimes >= 4)
        {
            ret = 20;
        }
        return ret;
    }


    /// <summary>
    /// 更新兑换所需文本
    /// </summary>
    void SetTodayGemLabel()
    {
        Label_ChangeNeed.text = GetTodayChangeGem().ToString();
    }

    /// <summary>
    /// 确认兑换
    /// </summary>
    /// <param name="_str"></param>
    void OnChangeMessage(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= OnChangeMessage;

        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(new CPrice(EPriceType.Gem, GetTodayChangeGem()), ECostGem.ChangeCoin)) 
            {
                //GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_NeedGem));
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            GameDataCenter.Instance.mTodayChangeTimes++;
            
            SetTodayGemLabel();
            CreateMoney();
        }
    }


    /// <summary>
    /// 兑换金币
    /// </summary>
    void OnChangeBtn()
    {
            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips),  string.Format(StringTable.GetString(EStringIndex.Tips_ChangeGem), GetTodayChangeGem()),
            StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += OnChangeMessage;
    }

}
