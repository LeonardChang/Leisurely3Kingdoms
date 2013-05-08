using UnityEngine;
using System.Collections;

/// <summary>
/// ��Ŀ��ö��
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
/// ��������
/// </summary>
public enum EPriceType
{
    Coin = 1,
    Gem
}

public class CPrice
{
    /// <summary>
    /// ��������
    /// </summary>
    public EPriceType Type = EPriceType.Coin;

    /// <summary>
    /// ����ֵ
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
    
    IrrigationUp = 101,              //�������
    MachineUp = 102,              //��������
    AltarUp = 103,                   //��̳����

    ItemKeepUp = 201,            //ҩ��
    ItemSpeedUp = 202,          //����
    ItemCandle = 203,              //����
    ItemFlash = 204,                //����
    ItemCatalyst = 205,            //�߻���
    ItemAddHole = 206,           //���ӿ�λ
    ItemZhanbu = 207,             //ռ��
    ItemYaoJiang = 208,           //ҡ��
        
    OpenLoginCard = 301,        //ÿ�ճ齱
    AddMorale = 302,               //����ʿ��
    ChangeCoin = 303,             //�һ����

    ZombieUpAttack = 401,       //��������
    ZombieUpValue = 402,        //��ֵ����

    SkillEx = 501,                    //���鼼��
    SkillAttack = 502,              //��������
    SkillGains = 503,               //���漼��
    SkillSpeed = 504,              //���ټ���




    BuyGem = 601,                     //����ˮ��
}