using UnityEngine;
using System.Collections;

/// <summary>
/// 价目表枚举
/// </summary>
public enum EPriceIndex
{
    PriceIrrigationLv1 = 1,
    PriceIrrigationLv2 = 2,
    PriceIrrigationLv3 = 3,
    PriceIrrigationLv4 = 4,
    PriceIrrigationLv5 = 5,
    PriceMachineLv1 = 8,
    PriceMachineLv2 = 9,
    PriceMachineLv3 = 10,
    PriceMachineLv4 = 11,
    PriceMachineLv5 = 12,
    PriceAltarLv1 = 15,
    PriceAltarLv2 = 16,
    PriceAltarLv3 = 17,
    PriceAltarLv4 = 18,
    PriceAltarLv5 = 19,
    PriceKeepRun = 22,
    PriceKeepRun2 = 23,
    PriceKeepRun3 = 24,
    PriceKeepRun4 = 25,
    PriceSpeedUp = 27,
    PriceSpeedUp2 = 28,
    PriceSpeedUp3 = 29,
    PriceAddHole2 = 30,
    PriceCandle = 32,
    PriceCandle2 = 33,
    PriceFlash = 34,
    PriceFlash2 = 35,
    PriceOpenLogin2 = 37,
    PriceOpenLogin3 = 38,
    PriceOpenLogin4 = 39,
    PriceOpenLogin5 = 40,
    PriceIsland2 = 43,
    PriceIsland3 = 44,
    PriceIsland4 = 45,
    PriceIsland5 = 46,
    PriceZombieAttackLv1 = 48,
    PriceZombieAttackLv2 = 49,
    PriceZombieAttackLv3 = 50,
    PriceZombieValue1 = 51,
    PriceZombieValue2 = 52,
    PriceZombieValue3 = 53,
    PriceSkill25 = 55,
    PriceSkill50 = 56,
    PriceSkill75 = 57,
    PriceSkill99 = 58,

    PriceAddMorale = 60,
    PriceCatalyst = 61,
    PriceZhanBu = 62,
    Max
}


/// <summary>
/// 货币类型
/// </summary>
public enum EPriceType
{
    Coin = 1,
    Gem
}

public class CPrice
{
    /// <summary>
    /// 货币类型
    /// </summary>
    public EPriceType Type = EPriceType.Coin;

    /// <summary>
    /// 货币值
    /// </summary>
    public int Value = 0;


    public CPrice()
    {

    }

    public CPrice(EPriceType _type, int _value)
    {
        Type = _type;
        Value = _value;
    }
}



public enum ECostGem
{
    
    IrrigationUp = 101,              //灌溉升级
    MachineUp = 102,              //机器升级
    AltarUp = 103,                   //祭坛升级

    ItemKeepUp = 201,            //药剂
    ItemSpeedUp = 202,          //加速
    ItemCandle = 203,              //蜡烛
    ItemFlash = 204,                //闪电
    ItemCatalyst = 205,            //催化剂
    ItemAddHole = 206,           //增加坑位
    ItemZhanbu = 207,             //占卜
    ItemYaoJiang = 208,           //摇奖
        
    OpenLoginCard = 301,        //每日抽奖
    AddMorale = 302,               //增加士气
    ChangeCoin = 303,             //兑换金币

    ZombieUpAttack = 401,       //攻击升级
    ZombieUpValue = 402,        //价值升级

    SkillEx = 501,                    //经验技能
    SkillAttack = 502,              //攻击技能
    SkillGains = 503,               //收益技能
    SkillSpeed = 504,              //加速技能




    BuyGem = 601,                     //购买水晶
}