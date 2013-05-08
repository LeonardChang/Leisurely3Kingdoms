using UnityEngine;
using System.Collections;

/// <summary>
/// 僵尸升级锻造
/// </summary>
public class GUIPanelZombieUp : MonoBehaviour 
{
    
    public GameObject IBtn_OK;

    public UILabel Label_Process_Text;
    public UILabel Label_Up_Text;
    public GameObject Panel_Add;
    public UILabel Label_Add;
    public GUIZombieInfo IBtn_ZombieInfo;
    public UISlider PB_Process;



    public ZombieType mZombieType = ZombieType.Normal;
    public int mStartValue = 0;
    //0-攻击 1-价值
    public int mAddType = 0;
    public int mUptoValue = 0;
    public int mPBMAX = 1;
    public int mLevel = 0;


    bool isIni = false;
    string mAddText = "";
    string mProgressText = "";
    int mAdd = 0;
    float mCurrentValue = 0;


    /// <summary>
    /// 攻击力升级价格
    /// </summary>
    CPrice[] Attack_Price;
    /// <summary>
    /// 价值升级价格
    /// </summary>
    CPrice[] Value_Price;


    CZombieData mZombieData;
    void OnEnable()
    {
        isIni = false;
    }
	// Use this for initialization
	void Start () 
    {
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

    /// <summary>
    /// 获取提示文本。
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    string GetTips(int _index)
    {
        string ret;
        //0-攻击
        if (_index == 0)
        {
            if (mZombieData.AttackLevel > 3)
            {
                ret = StringTable.GetString(EStringIndex.UIText_UpZombieAttackFinish);
                IBtn_OK.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_Quit);
            }
            else
            {
                ret = string.Format(StringTable.GetString(EStringIndex.UIText_UpZombieNeedGem), Attack_Price[mZombieData.AttackLevel - 1].Value);
            }
        }
        else
        {
            if(mZombieData.ValueLevel > 3)
            {
                ret = StringTable.GetString(EStringIndex.UIText_UpZombieValueFinish);
                IBtn_OK.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_Quit);

            }
            else
            {
                ret = string.Format(StringTable.GetString(EStringIndex.UIText_UpZombieNeedGem), Value_Price[mZombieData.ValueLevel - 1].Value);
            }
        }
        return GameDataCenter.Instance.GetChangeText(ret);
    }



    float mProgress = 1;
    void Ini()
    {
        mZombieData = GameDataCenter.Instance.GetOneZombieCollection(mZombieType);
        //IBtn_OK.gameObject.SetActiveRecursively(tr);
        IBtn_ZombieInfo.mZombieData = GameDataCenter.Instance.GetOneZombieCollection(mZombieType);
        IBtn_ZombieInfo.Texture_Stand.color = Color.white;
        IBtn_ZombieInfo.OnIni();
        Panel_Add.transform.localScale = Vector3.zero;

        Label_Process_Text.text = GetTips(mAddType);
        mProgress = 1;
        PB_Process.sliderValue = 0;
        IBtn_OK.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_Start);

        
        if(mAddType == 0)
        {
            mAddText = StringTable.GetString(EStringIndex.UIText_ZombieAttack) + ": " ;
            mCurrentValue = mZombieData.Attack;
        }
        else if(mAddType == 1)
        {
            mAddText = StringTable.GetString(EStringIndex.UIText_ZombieValue) + ": " ;
            mCurrentValue = mZombieData.Value;
        }
    }

    /// <summary>
    /// 升级过程完毕
    /// </summary>
    void UpgradeFinish()
    {
        IBtn_OK.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.UIText_UpZombieContinue);
        Label_Process_Text.text = GetTips(mAddType);
        PB_Process.sliderValue = 0;
        
        IBtn_OK.gameObject.SetActiveRecursively(true);
        TweenScale.Begin(Panel_Add, 0.25f, Vector3.one);
        switch(mLevel)
        {
            case 2:
                ResourcePath.PlaySound("UpgradeLv1");
                break;
            case 3:
                ResourcePath.PlaySound("UpgradeLv2");
                break;
            case 4:
                ResourcePath.PlaySound("UpgradeLv3");
                break;
        }

        GameObject[] obj = GameObject.FindGameObjectsWithTag("PanelCollection");
        foreach (GameObject o in obj)
        {
            o.SendMessage("SetAttackBtn");
            o.SendMessage("SetValueBtn");
        }
        
    }

	// Update is called once per frame
	void Update () 
    {
        if(!isIni)
        {
            isIni = true;
            Ini();
        }


        if (mProgress < 1)
        {
            mProgress += Time.deltaTime;
            if (mProgress >= 1)
            {
                UpgradeFinish();
            }
            PB_Process.sliderValue = mProgress;
            mCurrentValue = Mathf.Lerp((float)mStartValue, (float)mUptoValue, mProgress);
        }
  

        Label_Up_Text.text = mAddText + ((int)mCurrentValue).ToString();
        Label_Next_Add.text = GetNextLevelAdd();
        
	}

    public UILabel Label_Next_Add;
    string GetNextLevelAdd()
    {
        string ret = " ";

        if (mProgress < 1)
            return ret;

        if (mAddType == 0)
        {
            switch (mZombieData.AttackLevel)
            {
                case 1:
                case 2:
                    ret = "[0000FF](Next +1)[-]";
                    break;
                case 3:
                    ret = "[0000FF](Next x2)[-]";
                    break;
            }
        }
        else
        {
            switch (mZombieData.ValueLevel)
            {
                case 1:
                case 2:
                    ret = "[0000FF](Next +1)[-]";
                    break;
                case 3:
                    ret = "[0000FF](Next x2)[-]";
                    break;
            }
        }

        return ret;
    }


    void OnClose()
    {
        GameDataCenter.Instance.GuiManager.Panel_Manager.OnCloseZombieUp();
    }

    /// <summary>
    /// 开始升级过程
    /// </summary>
    void StartCount()
    {
        mProgress = 0;
        Label_Process_Text.text = StringTable.GetString(EStringIndex.Tips_UpZombieAttack);

        if(mAddType == 0)
        {
            mStartValue = (int)mZombieData.Attack;
            Upgrade();
            mUptoValue = (int)mZombieData.Attack;
            Label_Add.text = "+" + (mUptoValue - mStartValue).ToString();
        }
        else
        {
            mStartValue = (int)mZombieData.Value;
            Upgrade();
            mUptoValue = (int)mZombieData.Value;
            Label_Add.text = "+" + (mUptoValue - mStartValue).ToString();
        }
        Panel_Add.transform.localScale = Vector3.zero;
        ResourcePath.PlaySound("Upgrading");
    }

    /// <summary>
    /// 升级
    /// </summary>
    void Upgrade()
    {
        if(mAddType == 0)
        {
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
            mLevel = mZombieData.AttackLevel;
        }
        else
        {
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
            mLevel = mZombieData.ValueLevel;
        }
    }

    /// <summary>
    /// 开始升级
    /// </summary>
    void OnStartUp()
    {
        if (mProgress < 1) return;
        if(mAddType == 0)
        {
            if (mZombieData.AttackLevel > 3)
            {
                OnClose();
                return;
            }
            if (!GameDataCenter.Instance.DeductionPrice(Attack_Price[mZombieData.AttackLevel - 1], ECostGem.ZombieUpAttack))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            StartCount();
        }
        else
        {
            if (mZombieData.ValueLevel > 3)
            {
                OnClose();
                return;
            }
            if (!GameDataCenter.Instance.DeductionPrice(Value_Price[mZombieData.ValueLevel - 1], ECostGem.ZombieUpValue))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            StartCount();
        }
    }
}
