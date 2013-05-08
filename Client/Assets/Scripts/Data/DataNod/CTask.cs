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
/// ����
/// </summary>
[Serializable]
public class CTaskInfo
{
    /// <summary>
    /// ������
    /// </summary>
    public string Name;
    /// <summary>
    /// ����
    /// </summary>
    public string Dsc;
    /// <summary>
    /// Ŀ��ֵ
    /// </summary>
    public int MaxValue;
    /// <summary>
    /// ����
    /// </summary>
    public int Type;
    /// <summary>
    /// ��������
    /// </summary>
    public int AwardType;

    /// <summary>
    /// �������ֵ
    /// </summary>
    public int AwardValue;

    /// <summary>
    /// ��������ֵ
    /// </summary>
    public int AwardExperience;

    /// <summary>
    /// �����ɼ���
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
    /// ���ֵ
    /// </summary>
    private int mFinishValue = 0;
    public int FinishValue
    {
        get { return mFinishValue; }
        set { mFinishValue = value;}
    }

    /// <summary>
    /// �Ƿ����
    /// </summary>
    public bool IsFinish
    {
        get { return mFinishValue >= TaskInfo.MaxValue; }
    }

    /// <summary>
    /// �Ƿ�����˽���
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
