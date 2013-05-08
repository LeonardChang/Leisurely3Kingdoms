using UnityEngine;
using System.Collections;

//��̬����
public class CAchieveInfo
{
    /// <summary>
    /// ����
    /// </summary>
    public string mName = "";

    /// <summary>
    /// ����
    /// </summary>
    public string mDsc = "";

    /// <summary>
    /// ��Ҫ���ֵ
    /// </summary>
    public int mMaxFinish = 0;

    /// <summary>
    /// ͼ��
    /// </summary>
    public string mIcon = "";
}

//�ɾ�����
public class CAchieveData 
{
    /// <summary>
    /// �ɾ�ö��
    /// </summary>
    public AchievementEnum mAchieveType = AchievementEnum.ZombieArmy;

    /// <summary>
    /// ��ɶ�
    /// </summary>
    public int FinishProgress = 0;

    /// <summary>
    /// �Ƿ����
    /// </summary>
    public bool IsFinish = false;

    /// <summary>
    /// �ɾ���Ϣ
    /// </summary>
    public CAchieveInfo mAchieveInfo
    {
        get { return GlobalStaticData.GetAchieveInfo(mAchieveType); }
    }

    /// <summary>
    /// �ɾ���ɰٷֱ�
    /// </summary>
    public float FinishPercent
    {
        get { return (float)FinishProgress/(float)mAchieveInfo.mMaxFinish; }
    }
}



