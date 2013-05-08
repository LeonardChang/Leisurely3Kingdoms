using UnityEngine;
using System.Collections;

public class TaskManager : MonoBehaviour
{

    GameDataCenter center;
    CTaskData[] mTaskList;

	// Use this for initialization
	void Start () 
    {
        center = GameDataCenter.Instance;
        mTaskList = center.TaskData;

        //Debug.Log(center.TaskData.Length);
	}
	

    public CTaskData GetTaskData(int index)
    {
        return mTaskList[index];
    }


	// Update is called once per frame
	void Update () 
    {
	    
	}

    void CheckTaskFinish()
    {

    }


    /// <summary>
    /// �ռ���ͨ��ʬ
    /// </summary>
    public void CollectNormalZombie()
    {
        mTaskList[0].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʹ��ҩ��
    /// </summary>
    public void UseAgent()
    {
        mTaskList[1].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʹ�ü���
    /// </summary>
    public void UseSpeedUp()
    {
        mTaskList[2].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʹ��
    /// </summary>
    public void UseAltar()
    {
        mTaskList[3].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʹ�ü���
    /// </summary>
    public void UseSkill()
    {
        mTaskList[5].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// �ջ���콩ʬ
    /// </summary>
    public void CollectVariationZombie()
    {
        mTaskList[4].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// �Ӷ���
    /// </summary>
    /// <param name="earn_money"></param>
    public void EarnMoney(int earn_money)
    {
        mTaskList[6].FinishValue += earn_money;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʿ��
    /// </summary>
    public void MoraleToCrazy()
    {
        if (mTaskList[7].FinishValue < 1)
            mTaskList[7].FinishValue += 1;
        CheckTaskFinish();
    }

    /// <summary>
    /// ʹ��ˮ��
    /// </summary>
    public void UseGem()
    {
        mTaskList[8].FinishValue += 1;
        CheckTaskFinish();
    }

    /// <summary>
    /// �һ�ˮ��
    /// </summary>
    public void ChangeGem()
    {
        mTaskList[9].FinishValue += 1;
        CheckTaskFinish();
    }

    
}
