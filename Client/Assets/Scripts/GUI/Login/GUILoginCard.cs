using UnityEngine;
using System.Collections;
using EasyMotion2D;
//初始化   转背1  转背2  转正1  转正2

public enum LoginAwardType
{
    Baosi,
    Money,
    Zombie
}

public class GUILoginCard : MonoBehaviour
{
    public LoginAwardType mType = LoginAwardType.Baosi;
    //public SpriteRenderer Sprite_Zombie;
    public UITexture Zombie_Pic;
    public GameObject Card_Back;
    public UILabel Label;
    public UISprite Background;

    public GameObject LoginMoney;
    public GameObject LoginGem;

    public int mValue = 0;
    TweenScale mTS;
    //0-开始背->正  
    public int state = 0;
    bool isIni = false;
    GUILoginManager LoginManager;

    CPrice[] mPriceList; 
    // Use this for initialization
    void Start()
    {
        LoginManager = GameObject.Find("Panel_Login").GetComponent<GUILoginManager>();
        mTS = GetComponent<TweenScale>();
        mPriceList = new CPrice[6];
        for(int i = 2; i < 6; i ++)
        {
            mPriceList[i] = GlobalStaticData.GetPriceInfo((EPriceIndex)((int)EPriceIndex.PriceOpenLogin2 + i - 2));
        }
        

    }

    // Update is called once per frame
    void Update()
    {
        if(!isIni)
        {
            isIni = true;
            Card_Back.SetActiveRecursively(false);
            switch(mType)
            {
                case LoginAwardType.Baosi:
                    Label.text = mValue.ToString();
                    Background.spriteName = "Panel_Login_Gem";
                    break;
                case LoginAwardType.Money:
                    Label.text = mValue.ToString();
                    Background.spriteName = "Panel_Login_Coin";
                    break;
                case LoginAwardType.Zombie:
                    Label.text = "";
                    Background.spriteName = "Panel_Login_Zombie";
                    Zombie_Pic.color = Color.white;
                    Zombie_Pic.material = ResourcePath.GetZombieP((ZombieType)mValue);
                    Zombie_Pic.transform.localScale = new Vector3(Zombie_Pic.mainTexture.width, Zombie_Pic.mainTexture.height, 1);
                    //Sprite_Zombie.color = Color.white;
                    break;
            }
        }
    }

    
    void ScaleToZero()
    {
        mTS.enabled = true;
        mTS.from = new Vector3(1, 1, 1);
        mTS.to = new Vector3(0.1f, 1, 1);
        mTS.Reset();
        mTS.Play(true);
    }

    void ScaleToOne()
    {
        mTS.enabled = true;
        mTS.to = new Vector3(1, 1, 1);
        mTS.from = new Vector3(0.1f, 1, 1);
        mTS.Reset();
        mTS.Play(true);
    }

    /// <summary>
    /// 翻向背面
    /// </summary>
    void RollBack()
    {
        if (state == 1 || state == 0)
        {
            ScaleToZero();
            state = 2;
        }
        else if (state == 4)
        {
            ScaleToZero();
            state = 5;
        }
    }

    /// <summary>
    /// 缩放结束
    /// </summary>
    void OnScaleEnd()
    {
        switch (state)
        {
            case 0:
                state = 1;
                break;
            case 2:
                state = 3;
                ScaleToOne();
                Card_Back.SetActiveRecursively(true);
                DeLightZombie();
                GameObject.Find("Panel_Login").SendMessage("RollCard");
                break;
            case 5:
                state = 6;
                ScaleToOne();
                Card_Back.SetActiveRecursively(false);
                break;
        }
    }

    /// <summary>
    /// 开始选择
    /// </summary>
    void StartChoose()
    {
        state = 8;
        LightZombie();
    }


    void DeLightZombie()
    {
        Zombie_Pic.color = new Color(0, 0, 0, 0);
        //Sprite_Zombie.Apply();
    }


    void LightZombie()
    {
        if(LoginAwardType.Zombie == mType)
        {
            Zombie_Pic.color = new Color(1, 1, 1, 1);
            //Sprite_Zombie.Apply();
        }
    }

    void CreateMoney(int _value)
    {
        GameObject obj = (GameObject)Instantiate(LoginMoney);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<LoginMoney>().mValue = _value;
    }

    void CreateGem(int _value)
    {
        GameObject obj = (GameObject)Instantiate(LoginGem);
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<LoginMoney>().mValue = _value;
        obj.GetComponent<LoginMoney>().type = 1;
    }


    void OpenCard()
    {
        if (GameDataCenter.Instance.LoginAward == true)
        {
            GameDataCenter.Instance.LoginAward = false;
            GameDataCenter.Instance.Save();
        }
        ResourcePath.PlaySound("CardMove");
        LoginManager.mOpenCount++;
        state = 4;
        RollBack();
        switch (mType)
        {
            case LoginAwardType.Money:
                 CreateMoney(mValue);
                break;
            case LoginAwardType.Baosi:
                 CreateGem(mValue);
                break;
            case LoginAwardType.Zombie:
                 GameObject.Find("Panel_Login").GetComponent<GUILoginManager>().AddZombie(mValue);
                break;
        }
    }

    void MsgBoxOK(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= MsgBoxOK;
        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(mPriceList[LoginManager.mOpenCount], ECostGem.OpenLoginCard))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }

            OpenCard();
        }
    }

    string GetPriceString(CPrice _price)
    {
        string coin = StringTable.GetString(EStringIndex.Tips_Coin);
        string gem = StringTable.GetString(EStringIndex.Tips_Gem);
        string price = _price.Type == EPriceType.Coin ? "[0000FF]" + _price.Value.ToString() + "[-]" + coin: "[0000FF]" + _price.Value.ToString() + "[-]" + gem;
        string show_string = string.Format(StringTable.GetString(EStringIndex.Tips_LoginTips),  price);

        return show_string;

    }

    void OnClick()
    {
        if (state == 8)
        {
            if(LoginManager.mOpenCount == 0)
            {
                LoginManager.mOpenCount++;
                OpenCard();
                //GameDataCenter.Instance.GuiManager.ItemHelp.OpenTips(StringTable.GetString(EStringIndex.Info_LoginTips2), 600);
                GameObject.Find("Panel_Login").GetComponent<GUILoginManager>().SetTips(StringTable.GetString(EStringIndex.Info_LoginTips2));
            }
            else
            {
                GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetPriceString(mPriceList[LoginManager.mOpenCount]), 
                    StringTable.GetString(EStringIndex.Tips_OK),
                        StringTable.GetString(EStringIndex.Tips_Cancel));
                MessageBoxSpecial.alterButtonClickedEvent += MsgBoxOK;
            }
        }
    }
}
