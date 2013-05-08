using UnityEngine;
using System.Collections;

public class CSceneItem
{
    /// <summary>
    /// ����ö��
    /// </summary>
    public ESceneItemDataType Type = ESceneItemDataType.Max; 

    /// <summary>
    /// �����ٶȼӳ�
    /// </summary>
    public int mGrowSpeed = 0;

    /// <summary>
    /// ���߸��ʼӳ�
    /// </summary>
    public int mRareProbability = 0;

    /// <summary>
    /// �������Լӳ�
    /// </summary>
    public int mProperty = 0;

    /// <summary>
    /// ���ʣ��ʱ��
    /// </summary>
    public int mMaxLavaTime = 1;

    /// <summary>
    /// ����ʵ��ʣ��ʱ��
    /// </summary>
    public int mLavaTime = 0;


    public CSceneItem(ESceneItemDataType _type)
    {
        Type = _type;
    }

    public CSceneItem()
    {

    }

    /// <summary>
    /// ����ʣ��ʱ��
    /// </summary>
    public int LavaTime
    {
        set { mLavaTime = value; mMaxLavaTime = value; }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void UpDate()
    {
        if (mLavaTime > 1)
        {
            mLavaTime -= GameDataCenter.Instance.GameRunSpeed();
        }
        else
        {
            mLavaTime = 0;
            mGrowSpeed = 0;
            mRareProbability = 0;
            mProperty = 0;
        }
    }

    /// <summary>
    /// ���¶�����
    /// </summary>
    /// <param name="_passtime"></param>
    public void UpDate(int _passtime)
    {
        int total_pass = _passtime * GameDataCenter.Instance.GameRunSpeed();

        if(mLavaTime > total_pass)
        {
            mLavaTime -= total_pass;
            if (mLavaTime < 0)
                mLavaTime = 0;
        }
        else
        {
            mLavaTime = 0;
            mGrowSpeed = 0;
            mRareProbability = 0;
            mProperty = 0;
        }
    }

    public void SetData(int _growSpeed, int _rareProbability, int _Property, int _lavaTime, ESceneItemDataType _type)
    {
        mGrowSpeed = _growSpeed;
        mRareProbability = _rareProbability;
        mProperty = _Property;
        LavaTime = _lavaTime;
        Type = _type;
    }

    /// <summary>
    /// ����ʹ��ʱ��ʣ����ٰٷֱ�
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        if(mLavaTime <= 0)
        {
            return 0;
        }

        return (float)mLavaTime / (float)mMaxLavaTime;
    }
}

/// <summary>
/// ����ö��
/// </summary>
public enum ESceneItemDataType
{
    MachineLvUp = 1,
    SpeedUp = 2,
    SpeedUp2 = 3,
    SpeedUp3 = 4,
    AddHole = 5,
    IrrigationLvUp = 6,
    KeepRun = 7,
    KeepRun2 = 8,
    KeepRun3 = 9,
    KeepRun4 = 10,
    AltarLvUp = 11,
    Candle = 12,
    Candle2 = 13,
    Flash = 14,
    Flash2 = 15,
    Max
}


/// <summary>
/// ������Ϣ
/// </summary>
public class CSceneItemInfo
{
    /// <summary>
    /// ��������
    /// </summary>
    public ESceneItemDataType Type = ESceneItemDataType.Max;

    /// <summary>
    /// �����ٶȼӳ�
    /// </summary>
    public int GrowSpeed = 0;
    /// <summary>
    /// ���߸��ʼӳ�
    /// </summary>
    public int Rare = 0;
    /// <summary>
    /// �������Լӳ�
    /// </summary>
    public int Property = 0;
    /// <summary>
    /// ����ά��ʱ��
    /// </summary>
    public int LavaTime = 0;
    /// <summary>
    /// ��������
    /// </summary>
    public string Name = "";
    /// <summary>
    /// ��������
    /// </summary>
    public string Des = "";
}

public class CSceneItemData
{
    /// <summary>
    /// ����һ������
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static CSceneItem GetItem(ESceneItemDataType _type)
    {
        CSceneItem item = new CSceneItem();
        CSceneItemInfo itemInfo = GlobalStaticData.GetSceneItemInfo(_type);
        item.SetData(itemInfo.GrowSpeed, itemInfo.Rare, itemInfo.Property, itemInfo.LavaTime, _type);
        return item;
    }
}