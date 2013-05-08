using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewVersionChecking 
{


    public static void CheckVersion()
    {
        CheckDateSafe();
        CheckSpringFestivalVersion();
    }


    public static void CheckDateSafe()
    {
        if (GameDataCenter.Instance.StoryList[0] > 22)
        {
            GameDataCenter.Instance.SetSceneStory(0, 22);
        }
    }



    /// <summary>
    /// 检测是否用过春节版本。
    /// </summary>
	public static void CheckSpringFestivalVersion()
    {
        if (!PlayerPrefs.HasKey("Version_SpringFestival"))
        {
            PlayerPrefs.SetInt("Version_SpringFestival", 1);
            PlayerPrefs.Save();
        }
        else
        {
            return;
        }
        Debug.Log("Happy New Year");

        if (GameDataCenter.Instance.StoryList[2] < 43)
        {
            GameDataCenter.Instance.SetSceneStory(2, 43);
            GameDataCenter.Instance.SetSceneStoryState(2 ,GlobalStaticData.GetStory(EStoryIndex.Story_043).MaxCondition);
            Debug.Log(GameDataCenter.Instance.StoryList[2]);
        }


        GameDataCenter.Instance.InitSpringFestivalZombie();

        GameDataCenter.Instance.CheckOldVersionMoney();
    }
}
