using UnityEngine;
using System.Collections;

public class GUITaskBtn : MonoBehaviour 
{
    public UILabel Label_Dsc;
    public UILabel Label_Finish;
    //public UISprite Sprite_GetReward;
    public UISlicedSprite Sprite_Background;
    public UILabel Label_Btn;
    public UISlicedSprite Sprite_Back;


    public int mTaskId = 0;
    CTaskData mTaskData;

    public GameObject Panel_Tips;

    

	// Use this for initialization
	void Start () 
    {
        mTaskData = GameDataCenter.Instance.taskManager.GetTaskData(mTaskId);
        Panel_Tips.transform.FindChild("Label").GetComponent<UILabel>().text = mTaskData.TaskInfo.AwardValue + "[ffffff]ф[-]";
        Label_Btn.text = StringTable.GetString(EStringIndex.UITExt_GetTaskAward);
	}
	
	// Update is called once per frame
	void Update () 
    {
        UpdateUI();
	}

    void UpdateUI()
    {

        Label_Dsc.text = mTaskData.TaskInfo.Dsc + "(" + mTaskData.TaskInfo.AwardScorePoint + "[FFFFFF]и[-])";
        Label_Finish.text = mTaskData.FinishValue.ToString() + "/" +mTaskData.TaskInfo.MaxValue.ToString();


        if (mTaskData.IsAccept)//已经领奖
        {
            Label_Finish.alpha = 1;
            Label_Finish.text = StringTable.GetString(EStringIndex.UIText_TaskFinish);

            Sprite_Back.alpha = 1;
            Sprite_Background.alpha = 0;
            Label_Btn.alpha = 0;
            if(Panel_Tips.active)
                Panel_Tips.SetActiveRecursively(false);
        }
        else if (mTaskData.IsFinish)//完成-未领奖
        {
            Label_Finish.alpha = 0;
            Sprite_Back.alpha = 1;
            Sprite_Background.alpha = 1;
            Label_Btn.alpha = 1;
            if (!Panel_Tips.active)
                Panel_Tips.SetActiveRecursively(true);
        }
        else//进行中
        {
            Label_Finish.alpha = 1;
            Sprite_Back.alpha = 0;
            Sprite_Background.alpha = 0;
            Label_Btn.alpha = 0;
            if (Panel_Tips.active)
                Panel_Tips.SetActiveRecursively(false);
        }
    }

    //领奖
    void OnClick()
    {
        if (mTaskData.IsFinish &&  !mTaskData.IsAccept)
        {
            mTaskData.IsAccept = true;
            CreateMoney();
            CreateStar();
            CreateExperience();
            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.FinishDayMission, ((int)mTaskData.Type).ToString());
            //GameDataCenter.Instance.AddMoney(mTaskData.TaskInfo.AwardValue);
            //GameDataCenter.Instance.mTodayTaskPoint += mTaskData.TaskInfo.AwardScorePoint;
            //GameDataCenter.Instance.Experience += mTaskData.TaskInfo.AwardExperience;
        }
    }


    void CreateMoney()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameObject.Find("Panel_FlyItemList").transform;
        obj.transform.position = transform.position + new Vector3(0, 0, -1f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mValue = mTaskData.TaskInfo.AwardValue;
        obj.GetComponent<FlyAward>().FlyType = 3;
    }


    void CreateStar()
    {
        GameObject obj = ResourcePath.Instance("ScorePointStar");
        obj.transform.parent = GameObject.Find("Panel_FlyItemList").transform;
        obj.transform.position = transform.position + new Vector3(0, 0, -1f);
        obj.transform.localScale = new Vector3(32, 32, 1);//Vector3.one;
        //obj.GetComponent<FlyScorePoint>().AwardType = 0;
        obj.GetComponent<FlyScorePoint>().mValue = mTaskData.TaskInfo.AwardScorePoint;
    }

    void CreateExperience()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyExperience);
        obj.transform.parent = GameObject.Find("Panel_FlyItemList").transform;
        obj.transform.position = transform.position + new Vector3(0, 0, -1f);
        obj.transform.localScale = new Vector3(50, 48, 1);
        obj.GetComponent<FlyExperience>().mValue = mTaskData.TaskInfo.AwardExperience;
        obj.GetComponent<FlyExperience>().mSpecialType = 1;
    }
}
