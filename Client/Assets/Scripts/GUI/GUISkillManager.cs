using UnityEngine;
using System.Collections;
using System;

public class GUISkillManager : MonoBehaviour {

    public UIImageButton IBtn_JingYan;
    public UIImageButton IBtn_GongJi;
    public UIImageButton IBtn_ShouYi;
    public UIImageButton IBtn_SuDu;

    bool isIni = false;
    public UIImageButton[] IBtns;
	// Use this for initialization

	void Start () 
    {
        IBtns = new UIImageButton[] { IBtn_JingYan, IBtn_GongJi, IBtn_ShouYi, IBtn_SuDu};
	}
    void OnEnable()
    {
        isIni = false;
    }
	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;
            if (GameDataCenter.Instance.PlayerLevel >= 10)
            {
                SetBtnSprite(IBtn_JingYan, "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
                IBtn_JingYan.transform.FindChild("Text").GetComponent<UILabel>().color = Color.white;
                IBtn_JingYan.transform.FindChild("Sprite_Icon").GetComponent<UISprite>().color = Color.white;
            }
            if (GameDataCenter.Instance.PlayerLevel >= 20)
            {
                SetBtnSprite(IBtn_GongJi, "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
                IBtn_GongJi.transform.FindChild("Text").GetComponent<UILabel>().color = Color.white;
                IBtn_GongJi.transform.FindChild("Sprite_Icon").GetComponent<UISprite>().color = Color.white;
            }
            if (GameDataCenter.Instance.PlayerLevel >= 40)
            {
                SetBtnSprite(IBtn_ShouYi, "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
                IBtn_ShouYi.transform.FindChild("Text").GetComponent<UILabel>().color = Color.white;
                IBtn_ShouYi.transform.FindChild("Sprite_Icon").GetComponent<UISprite>().color = Color.white;
            }
            if (GameDataCenter.Instance.PlayerLevel >= 80)
            {
                SetBtnSprite(IBtn_SuDu, "Main_Btn0_Nor", "Main_Btn0_Down", "Main_Btn0_Down");
                IBtn_SuDu.transform.FindChild("Text").GetComponent<UILabel>().color = Color.white;
                IBtn_SuDu.transform.FindChild("Sprite_Icon").GetComponent<UISprite>().color = Color.white;
            }

        }

        for (int i = 1; i < (int)ESkillType.MAX; i++)
        {
            if (GameDataCenter.Instance.GetSkill((ESkillType)i).RestTime >= 0)
            {
                IBtns[i - 1].transform.FindChild("Label").GetComponent<UILabel>().color = new Color(1, 1, 1, 1);
                int second = (int)GameDataCenter.Instance.GetSkill((ESkillType)i).RestTime;
                IBtns[i - 1].transform.FindChild("Label").GetComponent<UILabel>().text = string.Format("{0:D2}:{1:D2}", second / 3600, (second % 3600) / 60);
            }
            else
            {
                IBtns[i - 1].transform.FindChild("Label").GetComponent<UILabel>().color = new Color(1, 1, 1, 0f);
            }           
        }
	}

    /// <summary>
    /// 设置按钮
    /// </summary>
    /// <param name="_iBtn"></param>
    /// <param name="_norSprite"></param>
    /// <param name="_hoverSprite"></param>
    /// <param name="_pressedSprite"></param>
    void SetBtnSprite(UIImageButton _iBtn, string _norSprite, string _hoverSprite, string _pressedSprite)
    {
        _iBtn.normalSprite = _norSprite;
        _iBtn.hoverSprite = _hoverSprite;
        _iBtn.pressedSprite = _pressedSprite;
        _iBtn.transform.FindChild("Background").GetComponent<UISprite>().spriteName = _norSprite;
    }




    EPriceIndex mPriceIndex = EPriceIndex.PriceSkill25;
    ECostGem mPriceType = ECostGem.SkillEx;
    ESkillType mSkillType = ESkillType.LV25;
    //int mEffectId = 0;

    /// <summary>
    /// 确认使用技能
    /// </summary>
    /// <param name="_str"></param>
    void UseSkillSure(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= UseSkillSure;

        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(mPriceIndex), mPriceType))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }
            GameDataCenter.Instance.UseSkill(mSkillType, DateTime.Now);
            //GameDataCenter.Instance.GuiManager.EffectUseSkill(mEffectId);
            ResourcePath.PlaySound(EResourceAudio.Audio_UseSkill);
            GameDataCenter.Instance.GuiManager.Panel_Manager.OnSkillBack();

        }
    }

    /// <summary>
    /// 经验技能按钮点击
    /// </summary>
    void OnIBtnJingYan()
    {
        if(GameDataCenter.Instance.PlayerLevel >= 10)
        {

            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_UseSkill_10),
                StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += UseSkillSure;

            mPriceIndex = EPriceIndex.PriceSkill25;
            mSkillType = ESkillType.LV25;
            mPriceType = ECostGem.SkillEx;
            //mEffectId = 0;

        }
    }


    /// <summary>
    /// 攻击技能按钮点击
    /// </summary>
    void OnIBtnGongJi()
    {
        if(GameDataCenter.Instance.PlayerLevel >= 20)
        {

            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_UseSkill_20),
                StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += UseSkillSure;

            mPriceIndex = EPriceIndex.PriceSkill50;
            mSkillType = ESkillType.LV50;
            mPriceType = ECostGem.SkillAttack;
            //mEffectId = 1;
        }
    }



    /// <summary>
    /// 收益技能按钮点击
    /// </summary>
    void OnIBtnShouYi()
    {
        if(GameDataCenter.Instance.PlayerLevel >= 40)
        {

            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_UseSkill_40),
                 StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += UseSkillSure;

            mPriceIndex = EPriceIndex.PriceSkill75;
            mSkillType = ESkillType.LV75;
            mPriceType = ECostGem.SkillGains;
            //mEffectId = 2;

        }
    }

    /// <summary>
    /// 速度技能按钮点击
    /// </summary>
    void OnIBtnSuDu()
    {
        if(GameDataCenter.Instance.PlayerLevel >= 80)
        {

            GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_UseSkill_80),
                 StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));
            MessageBoxSpecial.alterButtonClickedEvent += UseSkillSure;

            mPriceIndex = EPriceIndex.PriceSkill99;
            mSkillType = ESkillType.LV99;
            mPriceType = ECostGem.SkillSpeed;
            //mEffectId = 3;
        }
    }
}
