using UnityEngine;
using System.Collections;
using System;

public enum ESkillType
{
    LV25 = 1,
    LV50,
    LV75,
    LV99,
    MAX
}


[Serializable]
public class CSkillInfo
{
    /// <summary>
    /// 类型
    /// </summary>
    private ESkillType mType = ESkillType.LV25;
    public ESkillType Type
    {
        get { return mType; }
        set { mType = value;}
    }

    /// <summary>
    /// 开通等级
    /// </summary>
    public int NeedLevel
    {
        get
        {
            switch (mType)
            {
                case ESkillType.LV25:
                    return 25;
                case ESkillType.LV50:
                    return 50;
                case ESkillType.LV75:
                    return 75;
                case ESkillType.LV99:
                    return 99;
            }
            return 0;
        }
    }

    /// <summary>
    /// 价值
    /// </summary>
    public int Value
    {
        get
        {
            switch (mType)
            {
                case ESkillType.LV25:
                    return 5;
                case ESkillType.LV50:
                    return 20;
                case ESkillType.LV75:
                    return 35;
                case ESkillType.LV99:
                    return 80;
            }
            return 0;
        }
    }

    /// <summary>
    /// 技能时间
    /// </summary>
    public int MaxTime
    {
        get
        {
            switch (mType)
            {
                case ESkillType.LV25:
                    return 3600;
                case ESkillType.LV50:
                    return 3600 * 6;
                case ESkillType.LV75:
                    return 3600 * 12;
                case ESkillType.LV99:
                    return 3600 * 24;
            }
            return 0;
        }
    }

    /// <summary>
    /// 使用技能的时间
    /// </summary>
    private DateTime mUseTime = DateTime.MinValue;
    public DateTime UseTime
    {
        get { return mUseTime; }
        set { mUseTime = value;}
    }

    /// <summary>
    /// 是否使用
    /// </summary>
    public double RestTime
    {
        get 
        {
            TimeSpan time = DateTime.Now - mUseTime;
            return  MaxTime - time.TotalSeconds;
        }
    }

    public void SetData(ESkillType _type)
    {
        switch(_type)
        {
            case ESkillType.LV25:
                mType = _type;
                mUseTime = DateTime.MinValue;
                break;
            case ESkillType.LV50:
                mType = _type;
                mUseTime = DateTime.MinValue;
                break;
            case ESkillType.LV75:
                mType = _type;
                mUseTime = DateTime.MinValue;
                break;
            case ESkillType.LV99:
                mType = _type;
                mUseTime = DateTime.MinValue;
                break;
        }
        
    }
}

