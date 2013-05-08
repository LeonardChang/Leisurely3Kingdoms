using UnityEngine;
using System.Collections;
using System;


public enum ETaskType
{
    Task1 = 1,
    Task2 = 2,
    Task3 = 3,
    Task4 = 4,
    Task5 = 5,
    Task6 = 6,
    Task7 = 7,
    Task8 = 8,
    Task9 = 9,
    Task10 = 10,

    Max
}


/// <summary>
/// 任务
/// </summary>
[Serializable]
public class CTaskInfo
{
    /// <summary>
    /// 任务名
    /// </summary>
    public string Name;
    /// <summary>
    /// 描述
    /// </summary>
    public string Dsc;
    /// <summary>
    /// 目标值
    /// </summary>
    public int MaxValue;
    /// <summary>
    /// 类型
    /// </summary>
    public int Type;
    /// <summary>
    /// 奖励类型
    /// </summary>
    public int AwardType;

    /// <summary>
    /// 奖励金币值
    /// </summary>
    public int AwardValue;

    /// <summary>
    /// 奖励经验值
    /// </summary>
    public int AwardExperience;

    /// <summary>
    /// 奖励成绩点
    /// </summary>
    public int AwardScorePoint;
}

[Serializable]
public class CTaskData
{
    private ETaskType mType = ETaskType.Task1;
    public ETaskType Type
    {
        get { return mType; }
        set { mType = value;}
    }


    public CTaskInfo TaskInfo
    {
        get { return GlobalStaticData.GetTaskInfo(mType); }
    }

    /// <summary>
    /// 完成值
    /// </summary>
    private int mFinishValue = 0;
    public int FinishValue
    {
        get { return mFinishValue; }
        set { mFinishValue = value;}
    }

    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsFinish
    {
        get { return mFinishValue >= TaskInfo.MaxValue; }
    }

    /// <summary>
    /// 是否接收了奖励
    /// </summary>
    private bool mIsAccept = false;
    public bool IsAccept
    {
        get { return mIsAccept; }
        set { mIsAccept = value;}
    }

    public void ClearTask()
    {
        IsAccept = false;
        mFinishValue = 0;
    }
}
