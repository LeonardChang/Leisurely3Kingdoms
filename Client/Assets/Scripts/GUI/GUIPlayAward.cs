using UnityEngine;
using System.Collections;

public class GUIPlayAward : MonoBehaviour {
    //static string
    static int[] AWARD_TIME = new int[] { 180, 360, 840, 1800};
    //static int[] AWARD_TIME = new int[] { 10, 10, 10, 10 };
    public UILabel Label;

    Vector3 mStartPosition;
    TweenScale mTS;
	// Use this for initialization
	void Start () 
    {
        mTS = GetComponent<TweenScale>();
        mStartPosition = transform.localPosition;
        if(GameDataCenter.Instance.IsTeachMode)
        {
            transform.localPosition = new Vector3(-9999, -9999, 0);
        }
        if (GameDataCenter.Instance.mPlayAwardLevel >= 5)
        {
            Destroy(transform.gameObject);
            return;
        }
        if (GameDataCenter.Instance.mPlayAwardTime == 0 && GameDataCenter.Instance.mPlayAwardLevel == 0)
        {
            GameDataCenter.Instance.mPlayAwardTime = AWARD_TIME[0];
            GameDataCenter.Instance.mPlayAwardLevel++;
        }

        Label.text = string.Format("{0:D2}", GameDataCenter.Instance.mPlayAwardTime / 60) + ":" + string.Format("{0:D2}", GameDataCenter.Instance.mPlayAwardTime % 60);
        InvokeRepeating("RunUpdate", 1, 1);
	}

    int mAutoSaveTime = 0;
    void RunUpdate()
    {
        if (GameDataCenter.Instance.IsTeachMode)
        {
            return;
        }

        transform.localPosition = mStartPosition;
        mAutoSaveTime += 1;
        if(mAutoSaveTime >= 10)
        {
            GameDataCenter.Instance.Save();
            mAutoSaveTime = 0;
        }

        if (GameDataCenter.Instance.mPlayAwardLevel >= 5)
        {
            Destroy(transform.gameObject);
            return;
        }

        if (GameDataCenter.Instance.mPlayAwardTime > 0)
        {
            GameDataCenter.Instance.mPlayAwardTime -= 1;
        }

        if (GameDataCenter.Instance.mPlayAwardTime <= 0)
        {
            if(!mTS.enabled)
            {
                mTS.enabled = true;
            }
        }
        else
        {
            if (mTS.enabled)
            {
                mTS.enabled = false;
                transform.localScale = new Vector3(1f, 1f, 1);
            }
        }


        Label.text = string.Format("{0:D2}", GameDataCenter.Instance.mPlayAwardTime / 60) + ":" + string.Format("{0:D2}", GameDataCenter.Instance.mPlayAwardTime % 60);
    }


	// Update is called once per frame
	void Update () 
    {
	    
	}

    /// <summary>
    /// 创建奖励
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_value"></param>
    void CreatePlayAward(int _type , int _value)
    {

            GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
            obj.transform.parent = transform.parent;
            obj.transform.position = transform.position;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<FlyAward>().AwardType = _type;
            obj.GetComponent<FlyAward>().mValue = _value;
            obj.GetComponent<FlyAward>().FlyType = 2;
    }

    /// <summary>
    /// 生长僵尸
    /// </summary>
    void GrowAllZombie()
    {
        for (int i = 0; i < 30; i++)
        {
            GameDataCenter.Instance.GetCurrentScene().CreateRandomZombie();
        }
        foreach (CZombie zombie in GameDataCenter.Instance.GetCurrentScene().mZombies)
        {
            zombie.Nutrient = 60;
        }
    }

    /// <summary>
    /// 显示奖励消息框
    /// </summary>
    /// <param name="_coin"></param>
    /// <param name="_gem"></param>
    void ShowGetAwardMessage(int _coin, int _gem)
    {

        string show = string.Format(StringTable.GetString(EStringIndex.Tips_GetGiftAward), _coin, _gem);

        GameDataCenter.Instance.GuiManager.MsgBoxCongratulation(show);
    }

    /// <summary>
    /// 获取奖励
    /// </summary>
    void GetAward()
    {
        print(GameDataCenter.Instance.mPlayAwardLevel);

        switch(GameDataCenter.Instance.mPlayAwardLevel)
        {
            case 1:
                CreatePlayAward(0, 100);
                GrowAllZombie();
                ShowGetAwardMessage(100, 0);
                break;
            case 2:
                CreatePlayAward(0, 150);
                CreatePlayAward(1, 1);
                GrowAllZombie();
                ShowGetAwardMessage(150, 1);
                break;
            case 3:
                CreatePlayAward(0, 200);
                CreatePlayAward(1, 2);
                GrowAllZombie();
                ShowGetAwardMessage(200, 2);
                break;
            case 4:
                CreatePlayAward(0, 250);
                CreatePlayAward(1, 4);
                GrowAllZombie();
                ShowGetAwardMessage(250, 4);
                break;
        }
        GameDataCenter.Instance.mPlayAwardLevel++;
        if (GameDataCenter.Instance.mPlayAwardLevel >= 5)
        {
            Destroy(transform.gameObject);
            return;
        }
        else
        {
            GameDataCenter.Instance.mPlayAwardTime = AWARD_TIME[GameDataCenter.Instance.mPlayAwardLevel - 1];
            
        }
    }


    void OnClick()
    {
        if(GameDataCenter.Instance.mPlayAwardTime <= 0)
        {
            ResourcePath.PlaySound("NewZombie");
            GetAward();
        }
        else
        {
            GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Info_GetGift));
        }
    }
}
