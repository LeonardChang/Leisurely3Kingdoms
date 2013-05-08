using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 字串枚举
/// </summary>
public enum EStringIndex
{
    StringChinese = 1,
    ItemIrrigationLvUp_Name = 3,
    ItemKeepRun30M_Name = 4,
    ItemKeepRun1H_Name = 5,
    ItemKeepRun4H_Name = 6,
    ItemKeepRun24H_Name = 7,
    ItemMachineLvUp_Name = 8,
    ItemSpeedUp1H_Name = 9,
    ItemSpeedUp12H_Name = 10,
    ItemSpeedUp24H_Name = 11,
    ItemAddHole_Name = 12,
    ItemAltarLvUp_Name = 13,
    ItemCandle_Name = 14,
    ItemSuperCandle_Name = 15,
    ItemFlash_Name = 16,
    ItemSuperFlash_Name = 17,
    ItemIrrigationLvUp = 19,
    ItemKeepRun30M = 20,
    ItemKeepRun1H = 21,
    ItemKeepRun4H = 22,
    ItemKeepRun24H = 23,
    ItemMachineLvUp = 24,
    ItemSpeedUp1H = 25,
    ItemSpeedUp12H = 26,
    ItemSpeedUp24H = 27,
    ItemAddHole = 28,
    ItemAltarLvUp = 29,
    ItemCandle = 30,
    ItemSuperCandel = 31,
    ItemFlash = 32,
    ItemSuperFlash = 33,
    ItemFastGrow = 34,
    ItemZhaBu = 35,
    Teach_Step1 = 37,
    Teach_Step2 = 38,
    Teach_Step3 = 39,
    Teach_Step4 = 40,
    Teach_Step6 = 41,
    Teach_Step8 = 42,
    Teach_Step9 = 43,
    Teach_Step10 = 44,
    Teach_Step13 = 45,
    Teach_Step14 = 46,
    Teach_Step15 = 47,
    Teach_Step16 = 48,
    Teach_Step19 = 49,
    Teach_Step20 = 50,
    Teach_Step22 = 51,
    Teach_Step23 = 52,
    Teach_Step25 = 53,
    Teach_Step26 = 54,
    Teach_AwardText = 55,
    Teach_StepAdd = 56,
    Tips_TitleTips = 58,
    Tips_Warn = 59,
    Tips_OK = 60,
    Tips_Cancel = 61,
    Tips_UpAttack = 62,
    Tips_UpValue = 63,
    Tips_NeedMoney = 64,
    Tips_Spend = 65,
    Tips_Coin = 66,
    Tips_Gem = 67,
    Tips_StealZombie = 68,
    Tips_UseItem = 69,
    Tips_Congratulation = 70,
    Tips_LoginTips = 71,
    Tips_RePlaceItem = 72,
    Tips_BuyStage = 73,
    Tips_NewZombie = 74,
    Tips_NeedMoreGem = 75,
    Tips_SeeStage = 76,
    Tips_ClickToUseSkill = 77,
    Tips_UpToLightStar = 78,
    Tips_SKill_8 = 79,
    Tips_Skill_10 = 80,
    Tips_Skill_18 = 81,
    Tips_Skill_20 = 82,
    Tips_Skill_35 = 83,
    Tips_Skill_40 = 84,
    Tips_Skill_70 = 85,
    Tips_Skill_80 = 86,
    Tips_ChangeGem = 87,
    Tips_HasChangeGem = 88,
    Tips_AddMorale = 89,
    Tips_HasAddMorale = 90,
    Tips_HasSubMorle = 91,
    Tips_UseSkill_10 = 92,
    Tips_UseSkill_20 = 93,
    Tips_UseSkill_40 = 94,
    Tips_UseSkill_80 = 95,
    Tips_FirstKeepRun30M = 96,
    Tips_FirstKeepRun1H = 97,
    Tips_FirstKeepRun4H = 98,
    Tips_FirstKeepRun24H = 99,
    Tips_NeedAgent = 100,
    Tips_DateTimeError = 101,
    Tips_GetMedal = 102,
    Info_LoginTips1 = 103,
    Info_LoginTips2 = 104,
    Info_loginDay = 105,
    Info_GetGift = 106,
    StageName_1 = 107,
    StageName_2 = 108,
    StageName_3 = 109,
    UITExt_GetTaskAward = 111,
    UIText_ZombieTimes = 112,
    UIText_KeepUp = 113,
    UIText_SpeedUp = 114,
    UIText_CurrentHoles = 115,
    UIText_Candle = 116,
    UIText_Flash = 117,
    UIText_ZombieRare = 118,
    UIText_ZombieValue = 119,
    UIText_ZombieAttack = 120,
    UIText_StageInfo_2 = 121,
    UIText_10off = 122,
    UIText_15off = 123,
    UIText_25off = 124,
    UIText_Review = 125,
    UIText_ShopMoney = 126,
    UIText_MoraleLevel = 127,
    UIText_RareString = 128,
    UIText_SpeedString = 129,
    UIText_PropertyString = 130,
    UIText_AchieveUnLock = 131,
    UIText_LoginSwarm = 132,
    UIText_BuyGem = 133,
    UIText_RateMe = 134,
    UIText_Share = 135,
    UIText_QuitGame = 136,
    Path_ZombieTalk = 138,
    Path_WinText = 139,
    Path_LoadingText = 140,
    StaticData_StoryName = 143,
    StaticData_StoryDsc = 144,
    StaticData_ZombieName = 145,
    StaticData_ZombieDsc = 146,
    StaticData_Ability = 147,
    StaticData_AchieveName = 148,
    StaticData_AchieveDsc = 149,
    StaticData_TaskDsc = 150,
    Tips_GetGiftAward = 151,
    Welcome_Back = 152,
    Tips_NeedGem = 153,
    Tips_UpZombieAttack = 154,
    Tips_UpZombieValue = 155,
    Tips_WinTotal = 156,
    Tips_UpFinish = 157,
    UIText_UpZombieNeedGem = 158,
    UIText_Start = 159,
    UIText_UpZombieAttackFinish = 160,
    UIText_UpZombieValueFinish = 161,
    UIText_UpZombieContinue = 162,
    UIText_Quit = 163,
    UIText_UpgraRoll = 164,
    UIText_YesterdayValuation = 165,
    UIText_RewardOdds = 166,
    UIText_RollReward = 167,
    UIText_RollRewardFree = 168,
    UIText_Msg_Win = 169,
    UIText_Msg_Congratulation = 170,
    UIText_Msg_Sad = 171,
    UIText_RollHighReward = 172,
    UIText_AttackBack = 173,
    UIText_TaskFinish = 174,
    UIText_StageUnlock = 175,
    Tips_NeedLevel = 176,
    Tips_FreezeDevice = 177,
    Tips_RollFullLevel = 178,
    Tips_RollMachineTips = 179,
    TipsAward_FinishTeach = 181,
    TipsAward_LackMoney = 182,
    TipsAward_SiWangPass = 183,
    TipsAward_ShiTouPass = 184,
    TipsAward_FuRenPass = 185,
    TipsAward_NewZombie = 186,
    TipsAward_DeathIslandPass = 187,
    TipsAward_LoginBack = 188,
    TipsAward_OldVersion = 189,
    Tips_UnLockIsland = 190,
    Max
}

/// <summary>
/// 语言类型
/// </summary>
public enum ELocalizationTyp
{ 
    English,
    Chinese,
    Japanese,
    ChineseTw,
}

/// <summary>
/// 字串表数据项
/// </summary>
public class StringTableData
{
    private string mEnglish = "";
    public string English
    {
        get { return mEnglish; }
        set { mEnglish = value; }
    }

    private string mChinese = "";
    public string Chinese
    {
        get { return mChinese; }
        set { mChinese = value; }
    }

    private string mJapanese = "";
    public string Japanese
    {
        get { return mJapanese; }
        set { mJapanese = value;}
    }

    private string mChineseTw = "";
    public string ChineseTw
    {
        get { return mChineseTw; }
        set { mChineseTw = value; }       
    }
}



public class StringTableUI
{
    public static Dictionary<int, StringTableData> mStringTable;

    /// <summary>
    /// 检测表格
    /// </summary>
    /// <returns></returns>
    public static bool CheckTable()
    {
        if(mStringTable == null)
        {
            mStringTable = new Dictionary<int, StringTableData>();
            ArrayData data = GlobalStaticData.GetArrayData("Data/UIText");
            for(int i = 1; i < data.mLineCount; i++)
            {
                StringTableData s_data = new StringTableData();
                s_data.Chinese = data.GetString(1, i);
                s_data.English = data.GetString(2, i);
                s_data.Japanese = data.GetString(3, i);
                s_data.ChineseTw = data.GetString(4, i);
                int id = data.GetInt(0, i);
                mStringTable[id] = s_data;
            }
        }
        return true;
    }

    /// <summary>
    /// 获取字符串
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetString(int id)
    {
        CheckTable();
        string ret = "";
        if (mStringTable.ContainsKey(id))
        {
            if (StringTable.mStringType == ELocalizationTyp.Chinese)
            {
                ret = mStringTable[id].Chinese;
            }
            else if (StringTable.mStringType == ELocalizationTyp.Japanese)
            {
                ret = mStringTable[id].Japanese;
            }
            else if (StringTable.mStringType == ELocalizationTyp.ChineseTw)
            {
                ret = mStringTable[id].ChineseTw;
            }
            else
            {
                ret = mStringTable[id].English;
            }
        }
        return ret;
    }


}



/// <summary>
/// 字串类
/// </summary>
public class StringTable
{
    /// <summary>
    /// 字串列表
    /// </summary>
    public static List<StringTableData> mStringList;

    /// <summary>
    /// 语言类型
    /// </summary>
    public static ELocalizationTyp mStringType = ELocalizationTyp.Chinese;

    /// <summary>
    /// 检测字串列表是否初始化
    /// </summary>
    /// <returns></returns>
    public static bool CheckTable()
    {
        if(mStringList == null)
        {
            mStringList = new List<StringTableData>();
            ArrayData data = GlobalStaticData.GetArrayData("Data/StringTable");
            for (int i = 0; i < data.mLineCount; i++)
            {
                StringTableData s_data = new StringTableData();
                s_data.Chinese = data.GetString(1, i);
                s_data.English = data.GetString(2, i);
                s_data.Japanese = data.GetString(3, i);
                s_data.ChineseTw = data.GetString(4, i);
                mStringList.Add(s_data);
            }
        }
        return true;
    }

    /// <summary>
    /// 根据语言类型获取字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetString(EStringIndex _index)
    {
        CheckTable();
        if(mStringType == ELocalizationTyp.Chinese)
        {
            return GetChinese((int)_index);
        }
        else if(mStringType == ELocalizationTyp.Japanese)
        {
            return GetJapanese((int)_index);
        }
        else if (mStringType == ELocalizationTyp.ChineseTw)
        {
            return GetChineseTw((int)_index);
        }     
        return GetEnglish((int)_index);
    }

    /// <summary>
    /// 根据语言类型获取字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetString(int _index)
    {
        CheckTable();

        if(mStringType == ELocalizationTyp.Chinese)
        {
            return GetChinese(_index);
        }
        else if(mStringType == ELocalizationTyp.Japanese)
        {
            return GetJapanese(_index);
        }
        else if (mStringType == ELocalizationTyp.ChineseTw)
        {
            return GetChineseTw(_index);
        }
        return GetEnglish(_index);
    }


    /// <summary>
    /// 获取中文字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetChinese(EStringIndex _index)
    {
        CheckTable();
        return mStringList[(int)_index].Chinese;
    }

    public static string GetChinese(int _index)
    {
        CheckTable();
        return mStringList[_index].Chinese;
    }

    /// <summary>
    /// 获取英文字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetEnglish(EStringIndex _index)
    {
        CheckTable();
        return mStringList[(int)_index].English;
    }

    public static string GetEnglish(int _index)
    {
        CheckTable();
        return mStringList[_index].English;
    }

    /// <summary>
    /// 获取日文字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetJapanese(EStringIndex _index)
    {
        CheckTable();
        return mStringList[(int)_index].Japanese;
    }
    public static string GetJapanese(int _index)
    {
        CheckTable();
        return mStringList[_index].Japanese;
    }


    /// <summary>
    /// 获取中文繁体字串
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetChineseTw(EStringIndex _index)
    {
        CheckTable();
        return mStringList[(int)_index].ChineseTw;
    }

    public static string GetChineseTw(int _index)
    {
        CheckTable();
        return mStringList[_index].ChineseTw;
    }
}
