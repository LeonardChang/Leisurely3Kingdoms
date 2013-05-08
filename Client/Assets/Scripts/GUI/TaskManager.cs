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
    /// 收集普通僵尸
    /// </summary>
    public void CollectNormalZombie()
    {
        mTaskList[0].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 使用药剂
    /// </summary>
    public void UseAgent()
    {
        mTaskList[1].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 使用加速
    /// </summary>
    public void UseSpeedUp()
    {
        mTaskList[2].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 使用
    /// </summary>
    public void UseAltar()
    {
        mTaskList[3].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 使用技能
    /// </summary>
    public void UseSkill()
    {
        mTaskList[5].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 收获变异僵尸
    /// </summary>
    public void CollectVariationZombie()
    {
        mTaskList[4].FinishValue++;
        CheckTaskFinish();
    }

    /// <summary>
    /// 掠夺金币
    /// </summary>
    /// <param name="earn_money"></param>
    public void EarnMoney(int earn_money)
    {
        mTaskList[6].FinishValue += earn_money;
        CheckTaskFinish();
    }

    /// <summary>
    /// 士气
    /// </summary>
    public void MoraleToCrazy()
    {
        if (mTaskList[7].FinishValue < 1)
            mTaskList[7].FinishValue += 1;
        CheckTaskFinish();
    }

    /// <summary>
    /// 使用水晶
    /// </summary>
    public void UseGem()
    {
        mTaskList[8].FinishValue += 1;
        CheckTaskFinish();
    }

    /// <summary>
    /// 兑换水晶
    /// </summary>
    public void ChangeGem()
    {
        mTaskList[9].FinishValue += 1;
        CheckTaskFinish();
    }

    
}
