using UnityEngine;
using System.Collections;

//静态数据
public class CAchieveInfo
{
    /// <summary>
    /// 名称
    /// </summary>
    public string mName = "";

    /// <summary>
    /// 描述
    /// </summary>
    public string mDsc = "";

    /// <summary>
    /// 需要达成值
    /// </summary>
    public int mMaxFinish = 0;

    /// <summary>
    /// 图标
    /// </summary>
    public string mIcon = "";
}

//成就数据
public class CAchieveData 
{
    /// <summary>
    /// 成就枚举
    /// </summary>
    public AchievementEnum mAchieveType = AchievementEnum.ZombieArmy;

    /// <summary>
    /// 完成度
    /// </summary>
    public int FinishProgress = 0;

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsFinish = false;

    /// <summary>
    /// 成就信息
    /// </summary>
    public CAchieveInfo mAchieveInfo
    {
        get { return GlobalStaticData.GetAchieveInfo(mAchieveType); }
    }

    /// <summary>
    /// 成就完成百分比
    /// </summary>
    public float FinishPercent
    {
        get { return (float)FinishProgress/(float)mAchieveInfo.mMaxFinish; }
    }
}



