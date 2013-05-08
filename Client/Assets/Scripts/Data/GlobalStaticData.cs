using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System;


public enum EPropertyType
{
    Corpse = 0,
    Ice,
    Fire,
    Sand,
    Dragon
}

[Serializable]
public class ArrayData
{
    
    public int mRowCount = 0;
    public int mLineCount = 0;

    public List<List<string>> mData = new List<List<string>>();

    /// <summary>
    /// 添加一列
    /// </summary>
    /// <param name="_index"></param>
    public void InsertRow(int _index)
    {
        List<string> tmp = new List<string>();
        for (int i = 0; i < mLineCount; i++)
        {
            tmp.Add("");
        }
        mData.Insert(_index, tmp);
        mRowCount++;
    }

    /// <summary>
    /// 删除一列
    /// </summary>
    public void DeleteRow(int _index)
    {

        mData.RemoveAt(_index);
        mRowCount--;
        if (mRowCount == 0)
        {
            mLineCount = 0;
        }
    }

    /// <summary>
    /// 添加一行
    /// </summary>
    /// <param name="_index"></param>
    public void InsertLine(int _index)
    {
        foreach (List<string> list in mData)
        {
            list.Insert(_index, "");
        }
        mLineCount++;
    }

    /// <summary>
    /// 删除一行
    /// </summary>
    /// <param name="_index"></param>
    public void DeleteLine(int _index)
    {
        foreach (List<string> list in mData)
        {
            list.RemoveAt(_index);
        }
        mLineCount--;
    }

    public string GetString(int _row, int _line)
    {
        return mData[_row][_line];
    }

    public int GetInt(int _row, int _line)
    {
        if(mData[_row][_line] == "")
        {
            return 0;
        }
        return int.Parse(mData[_row][_line]);
    }

    public float GetFloat(int _row, int _line)
    {
        return float.Parse(mData[_row][_line]);
    }
}

public class GlobalStaticData 
{
    //public static ArrayData mStringTable = GetArrayData();
    //public static List<CZombieData> mZombieData = new List<CZombieData>();

    public static Dictionary<EPriceIndex, CPrice> mPriceInfo;
    public static Dictionary<ESceneItemDataType, CSceneItemInfo> mSceneItemInfo;
    public static Dictionary<ZombieType, CZombieInfo> mZombieInfo;
    public static Dictionary<AchievementEnum, CAchieveInfo> mAchieveInfo;
    public static Dictionary<ETaskType, CTaskInfo> mTaskInfo;
    public static Dictionary<EStoryIndex, CStory> mStoryList;
    public static Dictionary<int, string> mStoryName;
    public static Dictionary<int, CSceneInfo> mStageInfo;

    public static Dictionary<string, string> mOptionValue;

    public static string[] mZombieTalkList = new string[]{};
    public static string[] mWinWord = new string[]{};


    /// <summary>
    /// 获取关卡信息
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public static CSceneInfo GetStageInfo(int index)
    {
        if(mStageInfo == null)
        {
            mStageInfo = new Dictionary<int, CSceneInfo>();
            ArrayData data = GetArrayData("Data/StageData");

            for(int i = 1; i < data.mLineCount; i++)
            {
                CSceneInfo stage = new CSceneInfo();

                stage.mSceneID = data.GetInt(0, i);
                stage.mOpenLevel = data.GetInt(2, i);
                mStageInfo[stage.mSceneID] = stage;
            }
        }

        if(mStageInfo.ContainsKey(index))
        {
            return mStageInfo[index];
        }
        else
        {
            return null;
        }
    }



    /// <summary>
    /// 获取僵尸说话
    /// </summary>
    /// <returns></returns>
    public static string GetZombieTalk()
    {
        if(mZombieTalkList.Length <= 0)
        {
            TextAsset asset = null;
            asset = GlobalModule.Instance.LoadResource("Data/" + StringTable.GetString(EStringIndex.Path_ZombieTalk)) as TextAsset;
            string talk = asset.text;
            mZombieTalkList = talk.Split('\n');
        }
        int index = UnityEngine.Random.Range(0, mZombieTalkList.Length);
        return mZombieTalkList[index];
    }


    /// <summary>
    /// 获取胜利文字
    /// </summary>
    /// <returns></returns>
    public static string GetWinWord()
    {
        if(mWinWord.Length <= 0)
        {
            TextAsset asset = null;
            asset = GlobalModule.Instance.LoadResource("Data/" + StringTable.GetString(EStringIndex.Path_WinText)) as TextAsset;
            string word = asset.text;
            mWinWord = word.Split('\n');
        }
        int index = UnityEngine.Random.Range(0, mWinWord.Length);
        return mWinWord[index];
    }


    public static string[] GetStorySay(int _index)
    {
        
        string filen_ame = GetStoryFileName(_index);
        TextAsset aset = GlobalModule.Instance.LoadResource("Data/Story/" + filen_ame) as TextAsset;
        return aset.text.Split('\n');
    }




    /// <summary>
    /// 获取剧情名
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetStoryName(int _index)
    {
        if(mStoryName == null)
        {
            mStoryName = new Dictionary<int, string>();
            ArrayData dataBase = GetArrayData("Data/Story/StoryIndex");
            for(int i = 1; i < dataBase.mLineCount; i++)
            {
                mStoryName[i-1] = dataBase.GetString(0, i);
            }
        }
        if(mStoryName.ContainsKey(_index))
        {
            return mStoryName[_index];
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 获取选项参数
    /// </summary>
    /// <param name="_str"></param>
    /// <returns></returns>
    public static string GetOptionValue(string _str)
    {
        if(mOptionValue == null)
        {
            mOptionValue = new Dictionary<string, string>();
            ArrayData option_value = GetArrayData("Data/OptionValue");
            for(int i = 1; i < option_value.mLineCount; i++)
            {
                mOptionValue[option_value.GetString(0, i)] = option_value.GetString(1, i);
            }
        }
        if(mOptionValue.ContainsKey(_str))
        {
            return mOptionValue[_str];
        }
        else 
        {
            return "";
        }
    }


    /// <summary>
    /// 获取关卡/剧情  信息
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static CStory GetStory(EStoryIndex _index)
    {
        if(mStoryList == null)
        {
            mStoryList = new Dictionary<EStoryIndex, CStory>();
            ArrayData story_data = GetArrayData("Data/Story/Story1");
            ArrayData story_info = GetArrayData("Data/Story/StoryInfo");
            for(int i = 1; i < story_data.mLineCount; i ++)
            {
                CStory story = new CStory();
                story.StoryIndex = (EStoryIndex)i - 1;

                story.Name = story_info.mData[int.Parse(StringTable.GetString(EStringIndex.StaticData_StoryName))][i];
                story.Dsc = story_info.mData[int.Parse(StringTable.GetString(EStringIndex.StaticData_StoryDsc))][i];

                story.Condition = story_data.GetInt(1, i);
                story.MaxCondition = story_data.GetInt(1, i);
                story.AttackBG = story_data.GetInt(3, i);
                story.AttackTarget = story_data.GetInt(4, i);
                story.AwardGem = story_data.GetInt(5, i);
                story.SoldierType = story_data.GetInt(6, i);
                mStoryList[(EStoryIndex)i - 1] = story;
            }
        }
        if(mStoryList.ContainsKey(_index))
        {
            return mStoryList[_index];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取僵尸静态数据数据
    /// </summary>
    /// <returns></returns>
    public static CZombieInfo GetZombieInfo(ZombieType _type)
    {

        if(mZombieInfo == null)
        {
            mZombieInfo = new Dictionary<ZombieType, CZombieInfo>();
            ArrayData dataBase = GetArrayData("Data/ZombieData");
            ArrayData dsc_data = GetArrayData("Data/ZombieDsc");
            for(int i = 1; i < dataBase.mLineCount; i ++)
            {
                CZombieInfo zombie = new CZombieInfo();

                zombie.Name = dsc_data.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_ZombieName)) ,i);
                zombie.Dsc = dsc_data.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_ZombieDsc)), i);
                zombie.Ability = dsc_data.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_Ability)), i);

                zombie.PropertyType = dataBase.GetInt(1, i);
                zombie.PropertyValue = dataBase.GetInt(2, i);
                zombie.Experience = dataBase.GetInt(3, i);
                zombie.IniValue = dataBase.GetInt(4, i);
                zombie.Rare = dataBase.GetFloat(5, i);
                zombie.AttackValue = dataBase.GetFloat(6, i);
                zombie.AttackTimes = dataBase.GetInt(7, i);
                zombie.MoveSpeed = dataBase.GetInt(8, i);
                zombie.XiYou = dataBase.GetInt(9, i);

                mZombieInfo[(ZombieType)i] = zombie;
            }
        }
        return mZombieInfo[_type];
    }


    /// <summary>
    /// 获取价格
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static CPrice GetPriceInfo(EPriceIndex _index)
    {
        if(mPriceInfo == null)
        {
            mPriceInfo = new Dictionary<EPriceIndex, CPrice>();
            ArrayData price_data = GetArrayData("Data/PriceTable");
            for(int i = 1; i < price_data.mLineCount; i++)
            {
                if (price_data.GetString(0, i) == "") continue;

                CPrice price_info = new CPrice();
                price_info.Type = (price_data.GetInt(1, i) == 1 ? EPriceType.Coin : EPriceType.Gem);
                price_info.Value = price_data.GetInt(2, i);
                mPriceInfo[(EPriceIndex)i] = price_info;
            }
        }
        return mPriceInfo[_index];
    }



    /// <summary>
    /// 场景道具数据元素
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static CSceneItemInfo GetSceneItemInfo(ESceneItemDataType _type)
    {
        if(mSceneItemInfo == null)
        {
            mSceneItemInfo = new Dictionary<ESceneItemDataType, CSceneItemInfo>();
            ArrayData dataBase = GetArrayData("Data/ItemData");
            for(int i = 1; i < dataBase.mLineCount; i++)
            {
                CSceneItemInfo sceneItem = new CSceneItemInfo();
                sceneItem.GrowSpeed = dataBase.GetInt(1, i);
                sceneItem.Rare = dataBase.GetInt(2, i);
                sceneItem.Property = dataBase.GetInt(3, i);
                sceneItem.LavaTime = dataBase.GetInt(4, i);
                sceneItem.Name = StringTable.GetString(dataBase.GetInt(5, i));
                sceneItem.Des = StringTable.GetString(dataBase.GetInt(6, i));
                sceneItem.Type = _type;
                mSceneItemInfo[(ESceneItemDataType)i] = sceneItem;
            }
        }
        return mSceneItemInfo[_type];
    }


    /// <summary>
    /// 成就数据元素
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static CAchieveInfo GetAchieveInfo(AchievementEnum _type)
    {
        if(mAchieveInfo == null)
        {
            mAchieveInfo = new Dictionary<AchievementEnum, CAchieveInfo>();

            ArrayData dataBase = GetArrayData("Data/AchieveData");
            for(int i = 1; i < dataBase.mLineCount; i ++)
            {
                CAchieveInfo achieve = new CAchieveInfo();
                achieve.mName = dataBase.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_AchieveName)), i);
                achieve.mDsc = dataBase.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_AchieveDsc)), i); 
                achieve.mMaxFinish = dataBase.GetInt(1, i);
                mAchieveInfo[(AchievementEnum)(i-1)] = achieve;
            }
        }
        return mAchieveInfo[_type];
    }



    /// <summary>
    /// 检测任务数据
    /// </summary>
    public static void CheckTaskInfo()
    {
        if(mTaskInfo == null)
        {
            mTaskInfo = new Dictionary<ETaskType, CTaskInfo>();

            ArrayData dataBase = GetArrayData("Data/TaskData");

            for(int i = 1; i < dataBase.mLineCount; i ++)
            {
                CTaskInfo task = new CTaskInfo();
                task.Name = "";//dataBase.GetString(1, i);
                task.Dsc = dataBase.GetString(int.Parse(StringTable.GetString(EStringIndex.StaticData_TaskDsc)), i);//dataBase.GetString(0, i);
                task.MaxValue = dataBase.GetInt(1, i);
                task.Type = dataBase.GetInt(2, i);
                //task.AwardType = dataBase.GetInt(5, i);
                task.AwardValue = dataBase.GetInt(4, i);
                task.AwardScorePoint = dataBase.GetInt(3, i);
                task.AwardExperience = dataBase.GetInt(5, i);

                mTaskInfo[(ETaskType)i] = task;
            }
        }
    }
    /// <summary>
    /// 任务数据元素
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static CTaskInfo GetTaskInfo(ETaskType _type)
    {
        CheckTaskInfo();
        return mTaskInfo[_type];
    }
    /// <summary>
    /// 任务数据列表
    /// </summary>
    /// <returns></returns>
    public static CTaskInfo[] GetTaskInfoList()
    {
        CheckTaskInfo();
        List<CTaskInfo> infos = new List<CTaskInfo>();
        foreach (CTaskInfo info in mTaskInfo.Values)
        {
            infos.Add(info);
        }
        return infos.ToArray();
    }

    /// <summary>
    /// 获取剧情脚本文件名
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static string GetStoryFileName(int _index)
    {
        int MAX_STORY = 65;

        if(_index <= MAX_STORY && _index > 0)
        {
            if(StringTable.mStringType == ELocalizationTyp.Chinese)
            {
                return "Story_" + string.Format("{0:D3}", _index);
            }
            else if(StringTable.mStringType == ELocalizationTyp.Japanese)
            {
                return "Story_" + string.Format("{0:D3}", _index) + "jp";
            }
            else if(StringTable.mStringType == ELocalizationTyp.ChineseTw)
            {
                return "Story_" + string.Format("{0:D3}", _index) + "tw";
            }
            else
            {
                return "Story_" + string.Format("{0:D3}", _index) + "en";
            }
        }
        else
        {
            return "";
        }
    }

    public static ArrayData GetArrayData(TextAsset _tAsset)
    {
        StreamReader reader = new StreamReader(new MemoryStream(_tAsset.bytes), System.Text.Encoding.UTF8);
        XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
        ArrayData mArrayData = xmls.Deserialize(reader) as ArrayData;
        reader.Close();
        return mArrayData;
    }

    public static ArrayData GetArrayData(string _filename)
    {
        TextAsset aset = GlobalModule.Instance.LoadResource(_filename) as TextAsset;
        StreamReader reader = new StreamReader(new MemoryStream(aset.bytes), System.Text.Encoding.UTF8);
        XmlSerializer xmls = new XmlSerializer(typeof(ArrayData));
        ArrayData mArrayData = xmls.Deserialize(reader) as ArrayData;
        reader.Close();
        return mArrayData;
    }


    /// <summary>
    /// 是否是起始剧情
    /// </summary>
    /// <param name="story_index"></param>
    /// <returns></returns>
    public static bool IsStageStart(int story_index)
    {
        if (story_index == 23 ||
            story_index == 43)
        {
            return true;
        }
        return false;
    }

    public static bool IsStageStart(EStoryIndex story_index)
    {
        if(story_index == EStoryIndex.Story_023 || 
            story_index == EStoryIndex.Story_043)
        {
            return true;
        }
        return false;
    }


    public static bool IsStageEnd(int story_index)
    {
        if (story_index == 22 ||
           story_index == 42 ||
           story_index == 65)
        {
            return true;
        }
        return false;       
    }

    public static bool IsStageEnd(EStoryIndex story_index)
    {
            if(story_index == EStoryIndex.Story_022 || 
                story_index== EStoryIndex.Story_042 || 
                story_index == EStoryIndex.Story_065)
            {
                return true;
            }
            return false;
    }
}
