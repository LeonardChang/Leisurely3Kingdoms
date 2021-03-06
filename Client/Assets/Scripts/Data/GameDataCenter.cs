using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Security.Cryptography; 

[Serializable]
public class GameDataCenter 
{
    //存档版本
    private int SaveVersion = 46;

    private static volatile GameDataCenter instance;
    private static object syncRoot = new System.Object();

    public static GameDataCenter Instance
    {
        get
        {
            if(instance == null)
            {
                lock(syncRoot)
                {
                    if(instance == null)
                    {
                        instance = new GameDataCenter();
                    }
                }
            }
            return instance;
        }
    }

    public GameDataCenter()
    {
        SaveVersion = int.Parse(PubilshSettingData.Instance.SaveFileVersion);
    }

    public void NewInstance()
    {
        instance = new GameDataCenter();
    }

    public static bool IsRobber = UnityEngine.Random.value < 0.5f ? true : false;
    /// <summary>
    /// 存档文件名
    /// </summary>
    public static string DataFilePath
    {
        get
        {
            string path = "";
#if UNITY_IPHONE
            string fileNameBase = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
            path = fileNameBase.Substring(0, fileNameBase.LastIndexOf('/')) + "/Documents";
#else
            path = Application.persistentDataPath;
#endif
            //UnityEngine.Debug.Log(path);
            //GlobalModule.Instance.AppendDebugInfo(path);
            return path;
        }
    }

    public static string SaveFileName
    {
        get { return "ZombieGameData.gdata"; }
    }

    public static string SaveBackFileName
    {
        get { return "ZombieGameDataBack.gdata"; }
    }

    public static string SaveTempFileName
    {
        get { return "ZombieGameDataTemp.gdata"; }
    }

    public static string BackupSaveFileName
    {
        get
        {
            string str = "ZombieGameData_" + System.DateTime.Now.ToUniversalTime().ToString() + ".gdata.bk";
            str = str.Replace('/', '_');
            str = str.Replace(' ', '_');
            str = str.Replace(':', '_');
            return str;
        }
    }
    
    private const string STATIC_KEY_64 = "aT#$86g3";
    private const string STATIC_IV_64 = "k':8[]2D";

    private string KEY_64 = STATIC_KEY_64;
    private string IV_64 = STATIC_IV_64;

    public string Encode(string data, string _key, string _iv)
    {
        byte[] byKey = System.Text.Encoding.Default.GetBytes(_key);
        byte[] byIV = System.Text.Encoding.Default.GetBytes(_iv);

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream();
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

        StreamWriter sw = new StreamWriter(cst);
        sw.Write(data);
        sw.Flush();
        cst.FlushFinalBlock();
        sw.Flush();
        return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
    }

    public string Decode(string data, string _key, string _iv)
    {
        byte[] byKey = System.Text.Encoding.Default.GetBytes(_key);
        byte[] byIV = System.Text.Encoding.Default.GetBytes(_iv);

        byte[] byEnc;
        try
        {
            byEnc = Convert.FromBase64String(data);
        }
        catch
        {
            return "";
        }

        DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
        MemoryStream ms = new MemoryStream(byEnc);
        CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
        StreamReader sr = new StreamReader(cst);
        return sr.ReadToEnd();
    }

    public string UniqueID
    {
        get
        {
#if UNITY_ANDROID
            return GetMD5("KOZ" + SystemInfo.deviceModel + SystemInfo.processorType + SystemInfo.graphicsDeviceName + SystemInfo.graphicsMemorySize.ToString() + SystemInfo.systemMemorySize.ToString());
#else
            return GetMD5("KOZUNIQUEID");
#endif
        }
    }

    private static string GetMD5(string sDataIn)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] bytValue, bytHash;
        bytValue = System.Text.Encoding.UTF8.GetBytes(sDataIn);
        bytHash = md5.ComputeHash(bytValue);
        md5.Clear();
        string sTemp = "";
        for (int i = 0; i < bytHash.Length; i++)
        {
            sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
        }
        return sTemp.ToLower();
    }

    //public string mUID = "";
    private string mSavedUniqueID = "";
    public string SavedUniqueID
    {
        set { mSavedUniqueID = value; }
        get { return mSavedUniqueID; }
    }

    /// <summary>
    /// 新游戏
    /// </summary>
    public void NewGame()
    {
        LastSaveTime = DateTime.MinValue;
        CurrentScene = 0;
        PlayerLevel = 1; 
        Experience = 0;
        Money = 0;
        Gem = 0;

        GemString = "0";

        //第一主题僵尸收集状态
        PlayerPrefs.SetInt("StageFull_1", 0);
        //第二主题僵尸收集状态
        PlayerPrefs.SetInt("StageFull_2", 0);
        //第三主题僵尸收集状态
        PlayerPrefs.SetInt("StageFull_3", 0);

        //初始化岛屿
        mScenes.Clear();
        for (int i = 0; i < mSceneCount; i++)
        {
            
            CScene scene = new CScene();
            scene.RandomHoleList();
            scene.mSceneId = i;
            scene.DigTime = scene.GetRndDigTime();
            mScenes.Add(scene);
        }
        mScenes[0].IsOpen = true;

        //初始化僵尸收藏数据
        mZombieCollection.Clear();
        for (int i = 1; i <(int)ZombieType.Max; i++)
        {
            CZombieData zombie = new CZombieData();
            zombie.IniData((ZombieType)i);
            mZombieCollection[zombie.Type] = zombie;
        }
        //初始化技能
        mSkill.Clear();
        for (int i = 1; i < (int)ESkillType.MAX; i++ )
        {
            CSkillInfo skill = new CSkillInfo();
            skill.SetData((ESkillType)i);
            mSkill.Add(skill);
        }
        //初始化成就
        mAchieveData.Clear();
        for (int i = 0; i < (int)AchievementEnum.Max; i++)
            {
                CAchieveData data = new CAchieveData();
                data.mAchieveType = (AchievementEnum)i;
                mAchieveData.Add(data);
            }

        //初始化任务
        mTaskList.Clear();
        for(int i = 1; i <(int)ETaskType.Max; i ++)
        {
            CTaskData data = new CTaskData();
            data.Type = (ETaskType)i;
            mTaskList[data.Type] = data;
        }

        //初始化故事
        int[] story_index = new int[] {0, 23, 43, 0, 0 };
        mStoryList.Clear();
        for(int i = 0; i < 5; i ++)
        {
            mStoryList.Add(story_index[i]);
        }

        int[] story_hp = new int[] { 0, GlobalStaticData.GetStory(EStoryIndex.Story_023).MaxCondition, GlobalStaticData.GetStory(EStoryIndex.Story_043).MaxCondition, 0, 0, 0 };
        mStoryState.Clear();
        for (int i = 0; i < 5; i++)
        {
            mStoryState.Add(story_hp[i]);
        }

        SavedUniqueID = UniqueID;
    }

    /// <summary>
    /// 当前时间是否正常。
    /// </summary>
    /// <returns></returns>
    public bool IsDateTimeOk()
    {
        TimeSpan span = DateTime.Now - LastSaveTime;
        if (span.TotalSeconds < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public DateTime LastSaveTime = DateTime.MinValue;
    /// <summary>
    /// 保存
    /// </summary>
    public void Save()
    {
        TimeSpan span = DateTime.Now - LastSaveTime;
        if(span.TotalSeconds < 10 || IsTeachMode)
        {
            return;
        }
        
        //UnityEngine.Debug.Log("Save game data.");

        ForceSave();
        LastSaveTime = DateTime.Now;
    }

    /// <summary>
    /// 强制保存
    /// </summary>
    public void ForceSave()
    {
        TimeSpan span = DateTime.Now - LastSaveTime;
        if (span.TotalSeconds < 0)
        {
            return;
        }
        if(IsTeachMode)
        {
            return;
        }

        try
        {
            using (StreamWriter writer = new StreamWriter(DataFilePath + "/" + SaveTempFileName, false, System.Text.Encoding.Default))
            {
                writer.WriteLine(SaveVersion);

                XmlSerializer xmls = new XmlSerializer(typeof(GameDataCenter));
                MemoryStream ms = new MemoryStream();
                xmls.Serialize(ms, this);
                ms.Flush();

                writer.Write(Encode(System.Text.Encoding.Default.GetString(ms.ToArray()), KEY_64, IV_64));
                writer.Write(0);

                ms.Close();
                writer.Close();
            }
            
            // 如已存在旧存档，将其转为备份存档
            if (File.Exists(DataFilePath + "/" + SaveFileName))
            {
                if (File.Exists(DataFilePath + "/" + SaveBackFileName))
                {
                    File.Delete(DataFilePath + "/" + SaveBackFileName);
                }
                File.Move(DataFilePath + "/" + SaveFileName, DataFilePath + "/" + SaveBackFileName);
            }
            // 将临时存档转为新存档
            File.Move(DataFilePath + "/" + SaveTempFileName, DataFilePath + "/" + SaveFileName);

            // 保存新存档的MD5
            using (FileStream get_file = File.OpenRead(DataFilePath + "/" + SaveFileName))
            {
                System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] hash_byte = get_md5.ComputeHash(get_file);
                PlayerPrefs.SetString("SavedMD5", System.BitConverter.ToString(hash_byte));
                get_file.Close();

                PlayerPrefs.SetString("SavedGemString", GemString);
                PlayerPrefs.Save();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e.Message);
        }
        UnityEngine.Debug.Log("Save game data.");
    }

    /// <summary>
    /// 是否有存档
    /// </summary>
    public bool HasSave
    {
        get
        {
            return File.Exists(DataFilePath + "/" + SaveFileName);
        }
    }
    /// <summary>
    /// 读档
    /// </summary>
    /// <returns></returns>
    public bool Load()
    {
        UnityEngine.Debug.Log("Load game data.");

        try
        {
            if (!HasSave)
            {
                UnityEngine.Debug.Log("Con't find save file.");
                return false;
            }

            StreamReader reader = new StreamReader(DataFilePath + "/" + SaveFileName, System.Text.Encoding.Default);
            int ver = int.Parse(reader.ReadLine());
            if (ver != SaveVersion)
            {
                UnityEngine.Debug.Log("Save file verison is too old.");
                return false;
            }
            string buff = reader.ReadToEnd();
            if (!buff.EndsWith("0"))
            {
                // 存档不完整，尝试读取备份存档
                UnityEngine.Debug.Log("Save file is incompleted, try to load back save file.");
                if (File.Exists(DataFilePath + "/" + SaveBackFileName))
                {
                    reader = new StreamReader(DataFilePath + "/" + SaveBackFileName, System.Text.Encoding.Default);
                    ver = int.Parse(reader.ReadLine());
                    if (ver != SaveVersion)
                    {
                        UnityEngine.Debug.Log("Back save file verison is too old.");
                        return false;
                    }
                    buff = reader.ReadToEnd();
                    if (!buff.EndsWith("0"))
                    {
                        UnityEngine.Debug.Log("Back save file is incompleted.");
                        return false;
                    }
                }
                else
                {
                    UnityEngine.Debug.Log("Can't find back save file.");
                    return false;
                }
            }
            reader.Close();

            string decode = Decode(buff.Substring(0, buff.Length - 1), KEY_64, IV_64);

            MemoryStream ms = new MemoryStream(System.Text.Encoding.Default.GetBytes(decode));
            ms.Flush();

            Initialize();
            
            XmlSerializer xmls = new XmlSerializer(typeof(GameDataCenter));
            GameDataCenter obj = xmls.Deserialize(ms) as GameDataCenter;
            ms.Close();
            instance = obj;

            UnityEngine.Debug.Log("Load game data successful, the data verison is " + ver);

            // 读档后进行MD5校验，查看存档是否被修改过
            if (PlayerPrefs.HasKey("SavedMD5") && PlayerPrefs.HasKey("SavedGemString"))
            {
                string savedMD5 = PlayerPrefs.GetString("SavedMD5");
                string savedGem =  PlayerPrefs.GetString("SavedGemString");

                using (FileStream get_file = File.OpenRead(DataFilePath + "/" + SaveFileName))
                {
                    System.Security.Cryptography.MD5CryptoServiceProvider get_md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    byte[] hash_byte = get_md5.ComputeHash(get_file);
                    string newMD5 = System.BitConverter.ToString(hash_byte);
                    get_file.Close();

                    if (newMD5 != savedMD5)
                    {
                        UnityEngine.Debug.LogError("The archive MD5 checksum is not correct, and may have been modified");

                        int oldGem = int.Parse(savedGem);
                        int newGem = int.Parse(GemString);

                        if (newGem >= oldGem + 50)
                        {
                            GemString = savedGem;
                            ForceSave();
                            UnityEngine.Debug.LogError("Crystal incorrect number, may be modified, restore the Crystal Quantity: " + savedGem);
                        }
                        else
                        {
                            PlayerPrefs.SetString("SavedMD5", newMD5);
                            PlayerPrefs.Save();
                            UnityEngine.Debug.LogError("Crystal number correct, update archive MD5");
                        }
                    }
                    else
                    {
                        UnityEngine.Debug.Log("The archive MD5 Checksum correct");
                    }
                }
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError(e.Message);
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// 备份当前存档
    /// </summary>
    /// <returns></returns>
    public static bool BackupSave()
    {
        string saveFile = DataFilePath + "/" + SaveFileName;
        if (File.Exists(saveFile))
        {
            File.Copy(saveFile, DataFilePath + "/" + BackupSaveFileName);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 删除当前存档
    /// 删除前若已有存档，则备份
    /// </summary>
    public static void DeleteSave()
    {
        BackupSave();
        File.Delete(DataFilePath + "/" + SaveFileName);
    }

    /// <summary>
    /// 获取所有备份存档表
    /// </summary>
    /// <returns></returns>
    public IEnumerable<string> GetBackupSaves()
    {
        foreach (string str in Directory.GetFiles(DataFilePath, "*.bk"))
        {
            yield return str;
        }
    }

    /// <summary>
    /// 从某个备份还原存档
    /// </summary>
    /// <param name="_file"></param>
    public void RecoverBackupSave(string _file)
    {
        BackupSave();

        if (File.Exists(DataFilePath + "/" + SaveFileName))
        {
            File.Delete(DataFilePath + "/" + SaveFileName);
        }
        if (File.Exists(DataFilePath + "/" + SaveBackFileName))
        {
            File.Delete(DataFilePath + "/" + SaveBackFileName);
        }

        File.Copy(DataFilePath + "/" + BackupSaveFileName, SaveFileName);
        File.Copy(DataFilePath + "/" + BackupSaveFileName, SaveBackFileName);

        Load();
    }
    
    /// <summary>
    /// 初始化
    /// </summary>
    public void Initialize()
    {
        mScenes.Clear();
        mZombieCollection.Clear();
    }

    /// <summary>
    /// 上次被偷提示时间
    /// </summary>
    public DateTime mLastRobberTips = DateTime.MinValue;
    public bool IsRobberTips
    {
        get 
        {
            TimeSpan span = DateTime.Now - mLastRobberTips;
            if(span.TotalSeconds < 20)  
                return false; 
            else
                return true;
        }
    }

    /// <summary>
    /// 收获死僵尸
    /// </summary>
    public void CollectDieZombie()
    {
        mLastRobberTips = DateTime.Now;
    }

    //场景个数
    public static int mSceneCount = 5;

    public bool IsSoundOpen = true;
    public bool IsMusicOpen = true;

    //水晶提示次数
    public int mTipGemTimes = 0;
    //主题按钮提示次数
    public int mTipSeeStage = 0;

    /// <summary>
    /// 当前场景
    /// </summary>
    public int mCurrentScene = 0;
    public int CurrentScene
    {
        set { mCurrentScene = value; }
        get { return mCurrentScene; }
    }

    public int ChangeTargetScene = 0;

    /// <summary>
    /// 玩家等级
    /// </summary>
    private int mPlayerLevel = 1;
    public int PlayerLevel
    {
        get { return mPlayerLevel; }
        set { mPlayerLevel = value;}
    }

    /// <summary>
    /// 玩家经验
    /// </summary>
    private int mExperience = 0;
    public int Experience
    {
        get { return mExperience; }
        set { mExperience = value;}
    }

    /// <summary>
    /// 金钱（新版本中仅作旧版本的兼容）
    /// </summary>
    private float mMoney = 0;
    public float Money
    {
        get { return mMoney; }
        set { mMoney = value; }
    }

    /// <summary>
    /// 获取当前钱数
    /// </summary>
    /// <returns></returns>
    public float GetCurrentMoney()
    {
        //*钱不通用*//
        //float money = float.Parse(GetCurrentScene().MoneyString);

        //*钱通用*//
        float money = float.Parse(mScenes[0].MoneyString);
        return money;
    }

    /// <summary>
    /// 设置当前钱数
    /// </summary>
    /// <returns></returns>
    public void  SetCurrentMoney(float money)
    {
        //*钱不通用*//
        //GetCurrentScene().MoneyString = money.ToString();

        //*钱通用*//
        mScenes[0].MoneyString = money.ToString();
    }

    /// <summary>
    /// 水晶（新版本中仅作旧版本的兼容）
    /// </summary>
    private float mGem = 0;
    public float Gem
    {
        get { return mGem; }
        set { mGem = value;}
    }

    /// <summary>
    /// 宝石字串
    /// </summary>
    public string GemString = "0";
    public float GetCurrentGem()
    {

        return float.Parse(GemString);
    }

    void SetCurrentGem(string gem)
    {
        GemString = gem;
    }

    /// <summary>
    /// 清除金币与水晶。
    /// </summary>
    public void ClearMoney()
    {
        foreach (CScene scene in mScenes)
        {
            scene.MoneyString = "0";
        }
        GemString = "0";
    }

    /// <summary>
    /// 新版本中钱的转换。
    /// </summary>
    public void CheckOldVersionMoney()
    {
        if(Gem != 0)
        {
            SetCurrentGem(mGem.ToString());
            Gem = 0;
        }
        if(Money != 0)
        {
            mScenes[0].MoneyString = mMoney.ToString();
            Money = 0;
        }
        
    }

    /// <summary>
    /// 是否是教程
    /// </summary>
    private bool mIsTeachMode = true;
    public bool IsTeachMode
    {
        get { return mIsTeachMode; }
        set { mIsTeachMode = value;}
    }

    /// <summary>
    /// 当前故事
    /// </summary>
    public int CurrentStory()
    {
        return mStoryList[CurrentScene];
    }

    /// <summary>
    /// 开始关卡（初始化关卡统计数据）
    /// </summary>
    public void StartStory()
    {
        CScene scene = GetCurrentScene();
        scene.mWinStartTime = DateTime.Now;
        scene.mWinMVP = ZombieType.Normal;
        scene.mWinJoinZombie = 0;
        scene.mWinMoney = 0;
    }

    /// <summary>
    /// 下一剧情(关卡)
    /// </summary>
    public void GoNextStory()
    {
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.FinishStoryPoint, mCurrentScene.ToString() + "_" + mStoryList[mCurrentScene].ToString() + "_" + GetCurrentMoney().ToString() + "_" + GetCurrentGem().ToString());
        mStoryList[mCurrentScene]++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.BeginStoryPoint, mCurrentScene.ToString() + "_" + mStoryList[mCurrentScene].ToString() + "_" + GetCurrentMoney().ToString() + "_" + GetCurrentGem().ToString());
        StartStory();
        ForceSave();
        if(mCurrentScene == 0)
        {
            SetAchieve(AchievementEnum.DeathLeader,  mStoryList[0]);
        }
    }

    public void SetStory(int _index)
    {
        mStoryList[mCurrentScene] = _index;
    }

    /// <summary>
    /// 设置某个主题的关卡
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_index"></param>
    public void SetSceneStory(int _scene, int _index)
    {
        mStoryList[_scene] = _index;
    }

    /// <summary>
    /// 当前故事HP
    /// </summary>
    public int CurrentStoryHP()
    {
        return mStoryState[mCurrentScene];
    }

    /// <summary>
    /// 攻击目标扣血
    /// </summary>
    static float[] CRITICAL_PERCENT = new float[]{0, 0.2f, 0.4f, 0.6f, 1f};
    public int AddStoryHP(float _hp)
    {
        float hit = _hp;
        if (GetSkill(ESkillType.LV50).RestTime >= 0)
        {
            hit *= 2.0f;
        }

        int i_hit = (int)hit;
        if(UnityEngine.Random.value < CRITICAL_PERCENT[mMoraleLevel - 1])
        {
            hit *= 1.2f;
            i_hit = Mathf.FloorToInt(hit);
            GuiManager.CreateHitPoint(i_hit, 1);
        }
        else
        {
            GuiManager.CreateHitPoint(i_hit, 0);
        }

        mStoryState[mCurrentScene] += i_hit;
        return mStoryState[mCurrentScene];
    }

    /// <summary>
    /// 设置当前主题剧情HP
    /// </summary>
    /// <param name="_hp"></param>
    /// <returns></returns>
    public int SetStoryHP(int _hp)
    {
        mStoryState[mCurrentScene] = _hp;
        return mStoryState[mCurrentScene];
    }

    /// <summary>
    /// 每个场景当前故事列表
    /// </summary>
    private List<int> mStoryList = new List<int>();
    public int[] StoryList
    {
        set
        {
            mStoryList.Clear();
            for(int i = 0; i < value.Length; i ++)
            {
                mStoryList.Add(value[i]);
            }
        }
        get { return mStoryList.ToArray(); }
    }

    /// <summary>
    /// 每个场景当前故事的状态列表
    /// </summary>
    private List<int> mStoryState = new List<int>();
    public int[] StoryState
    {
        set
        {
            mStoryState.Clear();
            for(int i = 0; i < value.Length; i++)
            {
                mStoryState.Add((value[i]));
            }
        }
        get { return mStoryState.ToArray(); }
    }

    public void SetSceneStoryState(int _scene, int _state)
    {
        mStoryState[_scene] = _state;
    }

    /// <summary>
    /// 获取当前故事
    /// </summary>
    /// <returns></returns>
    public CStory GetCurrentStory()
    {
        CStory story = GlobalStaticData.GetStory((EStoryIndex)CurrentStory());

        if (GlobalStaticData.IsStageEnd(story.StoryIndex))
        {
            story.MaxCondition = mScenes[mCurrentScene].ChestLevel * 1000 + 10000;
            if(mScenes[mCurrentScene].ChestLevel >= 1)
            {
                CStory _story = GlobalStaticData.GetStory((EStoryIndex)GetCurrentScene().EndTarget);

                story.AttackBG = _story.AttackBG;
                story.AttackTarget = _story.AttackTarget;
                story.Name = StringTable.GetString(EStringIndex.UIText_AttackBack) + _story.Name;
                story.SoldierType = _story.SoldierType;
            }
        }
        return story;
    }

    /// <summary>
    /// 宝箱关卡设置成随机boss
    /// </summary>
    public void SetEndTarget()
    {
        if(CurrentScene == 0)
        {
            int rnd = UnityEngine.Random.Range(1, 21);

            mScenes[mCurrentScene].EndTarget = rnd;
        }
        else if(CurrentScene == 1)
        {
            int rnd = UnityEngine.Random.Range(23, 41);
            mScenes[mCurrentScene].EndTarget = rnd;
        }else if(CurrentScene == 2)
        {
            int rnd = UnityEngine.Random.Range(43, 65);

            mScenes[mCurrentScene].EndTarget = rnd;
        }
    }

    /// <summary>
    /// 游戏日期列表
    /// </summary>
    private List<DateTime> mPlayDates = new List<DateTime>();
    public DateTime[] PlayDates
    {
        set
        {
            mPlayDates.Clear();
            for(int i = 0; i < value.Length; i ++)
            {
                mPlayDates.Add(value[i]);
            }
        }
        get { return mPlayDates.ToArray(); }
    }

    /// <summary>
    /// 连续登陆天数
    /// </summary>
    private int mContinueDays = 1;
    public int ContinueDays
    {
        set
        {
            mContinueDays = value;
        }
        get
        {
            return mContinueDays;
        }
    }

    /// <summary>
    /// 是否有每日奖励
    /// </summary>
    private bool mLoginAward = true;
    public bool LoginAward
    {
        set{mLoginAward = value;}
        get { return mLoginAward; }
    }

    /// <summary>
    /// 上一次登陆时间
    /// </summary>
    private DateTime mLastLogin = DateTime.MinValue;
    public DateTime LastLogin
    {
        set{mLastLogin = value;}
        get { return mLastLogin;}
    }

    /// <summary>
    /// 摇晃金币个数
    /// </summary>
    private int mTodayShakeMoney = 0;
    public int TodayShakeMoney
    {
        set { mTodayShakeMoney = value;}
        get{return mTodayShakeMoney;}
    }

    public int mTodayEarnMoney = 0;

    /// <summary>
    /// 计算登陆天数
    /// </summary>
    public void EnterGame()
    {
        DateTime today = DateTime.Now;
        if(mLastLogin.Year == today.Year && mLastLogin.DayOfYear == today.DayOfYear)
        {
            //LoginAward = false;
        }
        else
        {
            DateTime tmp = mLastLogin;
            if(tmp.AddDays(1).Year == today.Year && tmp.AddDays(1).DayOfYear == today.DayOfYear)
            {
                mContinueDays++;
                mYestodayTaskPoint = mTodayTaskPoint;
            }
            else
            {
                mContinueDays = 1;
                mYestodayTaskPoint = 0;

                mAwardLoginBackDays = Mathf.Min((int)(today - mLastLogin).TotalDays, 30);
                if (LastLogin == DateTime.MinValue) mAwardLoginBackDays = 0;
            }
            mPlayDates.Add(today);
            SetAchieve(AchievementEnum.Fans,  mPlayDates.Count);
            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.YesterdayEvaluation ,GetYestodayTaskResult());
            LoginAward = true;
            mTodayTaskPoint = 0;
            mRollMachine.InitTodayReword();
        }
        LastLogin = today;
        
        MonoBehaviour.print(LastLogin.ToString());
    }

    /// <summary>
    /// 技能
    /// </summary>
    private List<CSkillInfo> mSkill = new List<CSkillInfo>();
    public CSkillInfo[] SkillInfo
    {
        get
        {
            return mSkill.ToArray();
        }

        set
        {
            mSkill.Clear();
            foreach (CSkillInfo skill in value)
            {
                mSkill.Add(skill);
            }
        }
    }

    public CSkillInfo GetSkill(ESkillType _type)
    {
        foreach (CSkillInfo skill in mSkill)
        {
            if(skill.Type == _type)
            {
                return skill;
            }
        }
        return null;
    }

    /// <summary>
    /// 使用技能
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_time"></param>
    public void UseSkill(ESkillType _type, DateTime _time)
    {
        foreach (CSkillInfo skill in mSkill)
        {
            if(skill.Type == _type)
            {
                skill.UseTime = DateTime.Now;
                taskManager.UseSkill();
            }
        }
    }

    //僵尸解锁情况表
    //private List<CZombieData> mZombieCollection = new List<CZombieData>();
    private Dictionary<ZombieType, CZombieData> mZombieCollection = new Dictionary<ZombieType, CZombieData>();
    public CZombieData[] ZombieCollection
    {
        get
        {
            List<CZombieData> dataList = new List<CZombieData>();
            foreach (CZombieData zombie in mZombieCollection.Values)
            {
                dataList.Add(zombie);
            }
            return dataList.ToArray();
        }
        set
        {
            mZombieCollection.Clear();
            foreach (CZombieData zombie in value)
            {
                if(zombie.Type >= ZombieType.Max)
                    continue;
                mZombieCollection[zombie.Type] = zombie;
            }

            for (int i = 1; i < (int)ZombieType.Max; i++)
            {
                if(!mZombieCollection.ContainsKey((ZombieType)i))
                {
                    CZombieData zombie = new CZombieData();
                    zombie.IniData((ZombieType)i);
                    mZombieCollection[zombie.Type] = zombie;
                }
            }
        }
    }

    /// <summary>
    /// 更新春节主题僵尸数值（由于旧版本含有31-45号僵尸的数据，需重置）
    /// </summary>
    public void InitSpringFestivalZombie()
    {
        for(int i = 31; i <= 45; i++)
        {
            CZombieData zombie = new CZombieData();
            zombie.IniData((ZombieType)i);
            mZombieCollection[zombie.Type] = zombie;
        }
    }

    /// <summary>
    /// 检测指定主题僵尸是否收满
    /// </summary>
    /// <param name="_stage"></param>
    /// <returns></returns>
    public bool IsStageZombieFull(int _stage)
    {
        int[] STAGES = new int[] {0, 15, 30, 45};
        CZombieData[] zombie_collection = ZombieCollection;
        for (int i = STAGES[_stage - 1]; i < STAGES[_stage]; i++)
        {
            if(!zombie_collection[i].IsOpen)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 检测僵尸是否收满
    /// </summary>
    /// <param name="_type">新收集的僵尸</param>
    public void CheckStageZombieFull(ZombieType _type)
    {
        if(_type <= ZombieType.Zombie15)
        {
            if(IsStageZombieFull(1))
            {
                PlayerPrefs.SetInt("StageFull_1", 1);
                GlobalModule.Instance.ShowInGameMessageBoxWithImage(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_GetMedal), StringTable.GetString(EStringIndex.Tips_OK), "GUI/Button/Medal/Stage_Medal_1");
            }
        }
        else if(_type <= ZombieType.Zombie30)
        {
            if(IsStageZombieFull(2))
            {
                PlayerPrefs.SetInt("StageFull_2", 1);
                GlobalModule.Instance.ShowInGameMessageBoxWithImage(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_GetMedal), StringTable.GetString(EStringIndex.Tips_OK), "GUI/Button/Medal/Stage_Medal_2");
            }
        }
        else if (_type <= ZombieType.Zombie45)
        {
            if (IsStageZombieFull(3))
            {
                PlayerPrefs.SetInt("StageFull_3", 1);
                GlobalModule.Instance.ShowInGameMessageBoxWithImage(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_GetMedal), StringTable.GetString(EStringIndex.Tips_OK), "GUI/Button/Medal/Stage_Medal_3");
            }
        }
    }

    /// <summary>
    /// 已经收集的僵尸列表
    /// </summary>
    public CZombieData[] OpenZombieCollection
    {
        get 
        {
            List<CZombieData> dataList = new List<CZombieData>();
            foreach (CZombieData zombie in mZombieCollection.Values)
            {
                if(zombie.IsOpen)
                {
                    dataList.Add(zombie);
                }
            }
            return dataList.ToArray();          
        }
    }

    /// <summary>
    /// 获取指定主题已经收集的僵尸
    /// </summary>
    /// <param name="_stage">主题id</param>
    /// <returns></returns>
    public CZombieData[] GetOpenZombieCollection(int _stage)
    {
        //主题相关
        int start_id = 1;
        int end_id = 15;
        if (_stage == 1)
        {
            start_id = 16;
            end_id = 30;
        }
        else if(_stage == 2)
        {
            start_id = 31;
            end_id = 45;
        }
        List<CZombieData> dataList = new List<CZombieData>();
        foreach (CZombieData zombie in mZombieCollection.Values)
        {
            if (zombie.IsOpen && (int)zombie.Type >= start_id && (int)zombie.Type <= end_id)
            {
                dataList.Add(zombie);
            }
        }
        return dataList.ToArray();   
    }

    /// <summary>
    /// 获取指定主题未收集到的僵尸
    /// </summary>
    /// <param name="_stage">主题id</param>
    /// <returns></returns>
    public CZombieData[] GetUnOpenZombieCollection(int _stage)
    {
        //主题相关
        int start_id = 1;
        int end_id = 15;
        if (_stage == 1)
        {
            start_id = 16;
            end_id = 30;
        }
        else if(_stage == 2)
        {
            start_id = 31;
            end_id = 45;
        }
        List<CZombieData> dataList = new List<CZombieData>();
        foreach (CZombieData zombie in mZombieCollection.Values)
        {
            if (!zombie.IsOpen && (int)zombie.Type >= start_id && (int)zombie.Type <= end_id)
            {
                dataList.Add(zombie);
            }
        }
        return dataList.ToArray();
    }

    /// <summary>
    /// 获取当前主题任意一只僵尸类型
    /// </summary>
    /// <returns></returns>
    public ZombieType GetCurrentRandomZombie()
    {
        ZombieType type = ZombieType.Normal;
        switch(CurrentScene)
        {
            case 1:
                type = ZombieType.Zombie16;
                break;
            case 2:
                type = ZombieType.Zombie31;
                break; ;
        }

        int rnd = UnityEngine.Random.Range(0, 15);

        return type + rnd;
    }

    /// <summary>
    /// 获取一只指定僵尸的数据信息
    /// </summary>
    /// <param name="_type">类型</param>
    /// <returns></returns>
    public CZombieData GetOneZombieCollection(ZombieType _type)
    {
        return mZombieCollection[_type];
    }

    /// <summary>
    /// 成就列表
    /// </summary>
    private List<CAchieveData> mAchieveData = new List<CAchieveData>();
    public List<CAchieveData> AchieveData
    {
        get { return mAchieveData; }
    }

    /// <summary>
    /// 所有任务列表
    /// </summary>
    private Dictionary<ETaskType, CTaskData> mTaskList = new Dictionary<ETaskType, CTaskData>();
    public CTaskData[] TaskData
    {
        get
        {
            List<CTaskData> task_data = new List<CTaskData>();
            foreach(CTaskData task in mTaskList.Values)
            {
                task_data.Add(task);
            }
            return task_data.ToArray();
        }
        set
        {
            mTaskList.Clear();
            foreach (CTaskData task in value)
            {
                mTaskList[task.Type] = task;
            }

            if(mTaskList.Count < (int)ETaskType.Max - 1)
            {
                //初始化任务
                mTaskList.Clear();
                for (int i = 1; i < (int)ETaskType.Max; i++)
                {
                    CTaskData data = new CTaskData();
                    data.Type = (ETaskType)i;
                    mTaskList[data.Type] = data;
                }
            }
        }
    }

    /// <summary>
    /// 当天成绩点
    /// </summary>
    public int mTodayTaskPoint = 0;

    public void SetTodayTaskPoint()
    {
        mTodayTaskPoint = 0;
        foreach (CTaskData task in mTaskList.Values)
        {
            if(task.IsAccept)
                mTodayTaskPoint += task.TaskInfo.AwardScorePoint;
        }
    }

    /// <summary>
    /// 昨天成绩点
    /// </summary>
    public int mYestodayTaskPoint = 0;

    public string GetYestodayTaskResult()
    {
        int point = mYestodayTaskPoint;
        string ret = "";
        if (point <= 39)
        {
            ret = "C";
        }
        else if (point <= 59)
        {
            ret = "B";
        }
        else if (point <= 79)
        {
            ret = "A";
        }
        else if (point <= 89)
        {
            ret = "S";
        }
        else if (point <= 99)
        {
            ret = "SS";
        }
        else if (point <= 100)
        {
            ret = "SSS";
        }
        return ret;
    }

    public float mYesterdayTaskMultiple
    {
        get
        {
            int point = mYestodayTaskPoint;
            float ret = 1;
            if (point <= 39)
            {
                ret = 1f;
            }
            else if (point <= 59)
            {
                ret = 1.5f;
            }
            else if (point <= 79)
            {
                ret = 2f;
            }
            else if (point <= 89)
            {
                ret = 2.5f;
            }
            else if (point <= 99)
            {
                ret = 3f;
            }
            else if (point <= 100)
            {
                ret = 5f;
            }

            return ret;
        }
    }

    /// <summary>
    /// 清空任务
    /// </summary>
    public void ClearTask()
    {
        //初始化任务
        foreach (CTaskData task in mTaskList.Values)
        {
            task.ClearTask();
        }
    }

    /// <summary>
    /// 场景列表
    /// </summary>
    public List<CScene> mScenes = new List<CScene>();
    public CScene GetCurrentScene()
    {
        return mScenes[mCurrentScene];
    }

    /// <summary>
    /// 购买岛屿成就
    /// </summary>
    public void AchieveBuyIsland()
    {
        int count = 0;
        foreach (CScene scene in mScenes)
        {
            if(scene.IsOpen)
            {
                count++;
            }
        }
        CAchieveData data;
        foreach (CAchieveData achieve in AchieveData)
        {
            if (achieve.mAchieveType == AchievementEnum.King)
            {
                data = achieve;
                GlobalModule.Instance.SendGameCenterAchievement(AchievementEnum.King, data.FinishPercent);

                if (achieve.FinishPercent >= 1 && !achieve.IsFinish)
                {
                    achieve.IsFinish = true;
                    GlobalModule.Instance.ShowAchievbementUnlocked(achieve.mAchieveInfo.mName);
                    GlobalModule.Instance.SendClientMessage(ClientMessageEnum.GetAchievement, ((int)achieve.mAchieveType).ToString());
                }
            }
        }
    }
    
    /// <summary>
    /// 成就递增
    /// </summary>
    /// <param name="_type"></param>
    public void AddAchieve(AchievementEnum _type)
    {
        foreach (CAchieveData achieve in AchieveData)
        {
            if (achieve.mAchieveType == _type)
            {
                achieve.FinishProgress++;
                GlobalModule.Instance.SendGameCenterAchievement(_type, achieve.FinishPercent);

                if (achieve.FinishPercent >= 1 && !achieve.IsFinish)
                {
                    achieve.IsFinish = true;
                    GlobalModule.Instance.ShowAchievbementUnlocked(achieve.mAchieveInfo.mName);
                    GlobalModule.Instance.SendClientMessage(ClientMessageEnum.GetAchievement, ((int)achieve.mAchieveType).ToString());
                }
            }
        }
    }

    /// <summary>
    /// 设置成就
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_value"></param>
    public void SetAchieve(AchievementEnum _type, int _value)
    {
        foreach (CAchieveData achieve in AchieveData)
        {
            if (achieve.mAchieveType == _type)
            {
                achieve.FinishProgress = _value;
                GlobalModule.Instance.SendGameCenterAchievement(_type, achieve.FinishPercent);

                if (achieve.FinishPercent >= 1 && !achieve.IsFinish)
                {
                    achieve.IsFinish = true;
                    GlobalModule.Instance.ShowAchievbementUnlocked(achieve.mAchieveInfo.mName);
                    GlobalModule.Instance.SendClientMessage(ClientMessageEnum.GetAchievement, ((int)achieve.mAchieveType).ToString());
                }
            }
        }
    }

    /// <summary>
    /// 是否有新僵尸没有被查看
    /// </summary>
    /// <returns></returns>
    public bool GetIsNewZombie()
    {
        foreach (CZombieData zombie in ZombieCollection)
        {
            if(zombie.IsNew)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 是否有任务完成了没有领取奖励
    /// </summary>
    /// <returns></returns>
    public bool GetIsTaskDone()
    {
        foreach (CTaskData task in mTaskList.Values)
        {
            if (task.IsFinish && !task.IsAccept)
                return true;
        }
        return false;
    }

    /// <summary>
    /// 检测是否有新岛屿开放
    /// </summary>
    /// <returns></returns>
    public bool GetIsNewIslandOpen()
    {
        for(int i = 1; i < 3; i ++)
        {
            if(!mScenes[i].IsOpen && PlayerLevel >= GlobalStaticData.GetStageInfo(i).mOpenLevel)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    ///收集一个僵尸
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_hole"></param>
    public void CollectOneZombie(int _scene, int _hole)
    {
        CZombieData zombie = mZombieCollection[mScenes[_scene].GetOneZombie(_hole).Type];
        CollectOneZombie(zombie.Type);
        mScenes[_scene].RemoveOneZombie(_hole);
    }

    public void CollectOneZombie(ZombieType _type)
    {
        taskManager.CollectNormalZombie();
        if (mZombieCollection[_type].ZombieInfo.Rare == 100)
        {
            AddAchieve(AchievementEnum.ZombieArmy);
            
        }
        else if (mZombieCollection[_type].ZombieInfo.Rare != 100)
        {
            AddAchieve(AchievementEnum.GreatFarmer);
            taskManager.CollectVariationZombie();
        }
        mZombieCollection[_type].Count++;
        if(mZombieCollection[_type].Count == 1)//新僵尸
        {
            GuiManager.PopTipsNewZombie();
            SetAchieve(AchievementEnum.Collector, GetCollectPercent());
            mZombieCollection[_type].IsNew = true;

             GameAward.AwardNewZombie(mZombieCollection[_type]);

            CheckStageZombieFull(_type);
        }
        
        if (mZombieCollection[_type].Count % 10 == 0 || mZombieCollection[_type].ZombieInfo.Rare < 20)
        {
            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.GetZombie, ((int)_type).ToString() + "_" + mZombieCollection[_type].Count.ToString());
        }
    }

    /// <summary>
    /// 删除主题农田里的一只僵尸
    /// </summary>
    /// <param name="_scene">主题id</param>
    /// <param name="_hole">坑位id</param>
    public void DeleteOneZombie(int _scene, int _hole)
    {
        mScenes[_scene].RemoveOneZombie(_hole);
    }
    /// <summary>
    /// 获取场景僵尸列表
    /// </summary>
    /// <param name="_scene">主题id</param>
    /// <returns></returns>
    public CZombie[] GetSceneZombie(int _scene)
    {
        return mScenes[_scene].mZombies.ToArray();
    }

    /// <summary>
    /// 获取当前场景僵尸列表
    /// </summary>
    /// <returns></returns>
    public CZombie[] GetCurrentSceneZombie()
    {
        return mScenes[mCurrentScene].mZombies.ToArray();
    }

    /// <summary>
    /// 获取特定一个僵尸
    /// </summary>
    /// <param name="_scene"></param>
    /// <param name="_hole"></param>
    /// <returns></returns>
    public CZombie GetOneZombie(int _scene, int _hole)
    {
        return mScenes[_scene].GetOneZombie(_hole);
    }

    /// <summary>
    /// 初始化场景
    /// </summary>
    public void IniScene()
    {
        foreach (CScene scene in mScenes)
        {
            scene.Ini();
        }
    }

    /// <summary>
    /// 士气等级
    /// </summary>
    public int mMoraleLevel = 1;
    public DateTime mLastMoraleTime = DateTime.MinValue;

    /// <summary>
    /// 今天是否兑换金币
    /// </summary>
    public bool IsTodayChange = false;

    public int mTodayChangeTimes = 0;

    /// <summary>
    /// 计算士气
    /// </summary>
    /// <returns></returns>
    public void UpdateMorale()
    {
        DateTime now = DateTime.Now;
        DateTime last = mLastMoraleTime;
        TimeSpan pass_time = now - last;


        if (mMoraleLevel >= 5)
        {
            taskManager.MoraleToCrazy();
        }

        if(pass_time.TotalHours < 1)
        {
            GuiManager.UpdateMorale();
            return;
        }



        //达到升级士气条件
        if (pass_time.TotalHours >= 1 && pass_time.TotalHours < 2)
        {
            if(mMoraleLevel < 5)
            {
                GuiManager.PopTipsMarola(0);
            }
            
            mMoraleLevel++;
            if(mMoraleLevel > 5)
            {
                mMoraleLevel = 5;
            }


        }

        //扣掉士气
        if(pass_time.TotalHours > 3)
        {
            int sub = (int)(pass_time.TotalHours-1) / 2;
            GuiManager.PopTipsMarola(1);
            mMoraleLevel -= sub;
            if(mMoraleLevel <= 0)
            {
                mMoraleLevel = 1;
            }
        }
        mLastMoraleTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0, 0);
        GuiManager.UpdateMorale();
    }

    /// <summary>
    /// 上次更新时间
    /// </summary>
    public DateTime mLastRefreshTime = DateTime.MinValue;

    /// <summary>
    /// 新的一天
    /// </summary>
    void EnterNewDay()
    {
        TodayShakeMoney = 0;
        mTodayEarnMoney = 0;
        mTodayChangeTimes = 0;
        mSmallHappyTimes = 0;

        if (GlobalModule.Instance.isSwarmEnable)
        {
            GlobalModule.Instance.ForceClearTodayGainCoin();
        }
        
        ClearTask();
    }

    /// <summary>
    /// 更新场景
    /// </summary>
    public void UpdateScene()
    {
        TimeSpan passTime = DateTime.Now - mLastRefreshTime;
        double passSecond = passTime.TotalSeconds;
        if (passSecond < 0) return;

        if (passSecond > 5)
        {
            GuiManager.UnSetBgmPich();
            GuiManager.SetWeather();

            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.StartPlay, "1");
            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.LogError, PlayerPrefs.GetInt("PostErrorTimes").ToString());
        }

        UpdateMorale();

        //上次更新跟今天不是同一天
        if (!(DateTime.Now.Year == mLastRefreshTime.Year && DateTime.Now.DayOfYear == mLastRefreshTime.DayOfYear))
        {
            GuiManager.Panel_Manager.CheckLogin();
            EnterNewDay();
        }

        GuiManager.UpDateSkillEffect();

        foreach(CScene scene in mScenes)
        {
            scene.RefreshScene();
        }
        mLastRefreshTime = DateTime.Now;
    }

    /// <summary>
    /// 将僵尸状态设置成旧的
    /// </summary>
    /// <param name="_sceneId"></param>
    public void SetZombieOld(int _sceneId)
    {
        mScenes[_sceneId].SetZombieOld();
    }

    /// <summary>
    /// 加钻石
    /// </summary>
    /// <param name="_value"></param>
    public void AddGem(int _value)
    {

        float gem = float.Parse(GemString) + _value;
        GemString = gem.ToString();
        ForceSave();

    }

    /// <summary>
    /// 加钱
    /// </summary>
    /// <param name="_value"></param>
    public void AddMoney(int _value)
    {

        float money = GetCurrentMoney();
        money += _value;
        SetCurrentMoney(money);

        SetAchieve(AchievementEnum.Richer, (int)money);

        if(_value >  0)
        {
            mTodayEarnMoney += _value;
        }
        
        GlobalModule.Instance.SendGameCenterLeaderboard(0, mTodayEarnMoney);
        Save();
    }

    /// <summary>
    /// 第一次使用药剂
    /// </summary>
    public float mHappyTime = 0;
    public bool isFirstUse = true;
    public void FirstUse()
    {
        if(IsTeachMode)
        {
            return;
        }
        if(isFirstUse)
        {
            isFirstUse = false;
            for (int i = 0; i < 3; i++)
            {
                GetCurrentScene().CreateRandomZombie();
            }
            mHappyTime = 80;
        }
    }

    //小 大 狂欢
    public int mSmallHappyTimes = 0;
    public int mBigHappyTimes = 0;

    /// <summary>
    /// 激活小狂欢
    /// </summary>
    public void ActiveSmallHappy()
    {
        if (mSmallHappyTimes >= 10)
            return;
        mSmallHappyTimes ++;

        foreach (CZombie zombie in GetCurrentScene().mZombies)
        {
            zombie.Nutrient = 60;
        }

        int count = 8 - GetCurrentScene().mZombies.Count;

        for(int i = 0; i < count; i ++)
        {
            GetCurrentScene().CreateRandomZombie();
        }
    }

    /// <summary>
    /// 激活大狂欢
    /// </summary>
    public void ActiveBigHappy()
    {
        //mBigHappyTime = 30;

        foreach (CZombie zombie in GetCurrentScene().mZombies)
        {
            zombie.Nutrient = 60;
        }

        int count = 30 - GetCurrentScene().mZombies.Count;

        for (int i = 0; i < count; i++)
        {
            GetCurrentScene().CreateRandomZombie();
        }


        GuiManager.SetWeather(1);
        GuiManager.SetBgmPich();

    }

    /// <summary>
    /// 增加经验
    /// </summary>
    /// <param name="_value">值</param>
    public void AddExperience(int _value)
    {
        int multiple = 1;
        if(GetSkill(ESkillType.LV25).RestTime >= 0)
        {
            multiple = 2;
        }
        mExperience += (_value * multiple);

        if(mExperience > mPlayerLevel * 20)
        {

            if (mPlayerLevel >= 99)
                return;

            GuiManager.EffectLevelUp();
            ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp2);
            mPlayerLevel++;
            mExperience = 0;

            CheckSkillPop();
        }
    }

    /// <summary>
    /// 冒出提示框
    /// </summary>
    public void CheckSkillPop()
    {
        switch(mPlayerLevel)
        {
            case 8:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_SKill_8));
                break;
            case 10:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_10));
                GuiManager.PopTipsSkill();
                break;
            case 18:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_18));
                //GuiManager.MsgBoxCongratulation(string.Format(StringTable.GetString(EStringIndex.Tips_UnLockIsland), 18, StringTable.GetString(EStringIndex.StageName_3)));
                break;
            case 20:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_20));
                break;
            case 34:
                //GuiManager.MsgBoxCongratulation(string.Format(StringTable.GetString(EStringIndex.Tips_UnLockIsland), 34, StringTable.GetString(EStringIndex.StageName_2)));
                break;
            case 35:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_35));
                break;
            case 40:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_40));
                break;
            case 70:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_70));
                break;
            case 80:
                GuiManager.MsgBoxCongratulation(StringTable.GetString(EStringIndex.Tips_Skill_80));
                break;
        }
    }

    /// <summary>
    /// 游戏运转速度
    /// </summary>
    /// <returns></returns>
    public int GameRunSpeed()
    {
        int multiple = 1;
        if (GetSkill(ESkillType.LV99).RestTime >= 0)
        {
            multiple = 2;
        }
        return multiple;
    }

    /// <summary>
    /// 检测是否够钱
    /// </summary>
    /// <param name="_price"></param>
    /// <returns></returns>
    public bool CheckPrice(CPrice _price)
    {
        switch(_price.Type)
        {
            case EPriceType.Coin:
                return GetCurrentMoney() >= _price.Value? true : false;
            case EPriceType.Gem:
                return GetCurrentGem() >= _price.Value ? true : false;
        }
        return false;
    }

    /// <summary>
    /// 扣钱
    /// </summary>
    /// <param name="_price"></param>
    /// <returns></returns>
    public bool DeductionPrice(CPrice _price)
    {
        bool success = false;

        switch (_price.Type)
        {
            case EPriceType.Coin:
                success = GetCurrentMoney() >= _price.Value ? true : false;
                if(success)
                {
                    AddMoney(-_price.Value);

                    if (!GameDataCenter.Instance.mIsAwardLackMoney && GameDataCenter.Instance.GetCurrentMoney() < 50)
                    {
                        GameAward.AwardLackMoney();
                    }
                    GuiManager.SetMoneyMotion();
                }
                break;
            case EPriceType.Gem:
                if (mIsDeviceLocked)
                {
                    return false;
                }
                success = GetCurrentGem() >= _price.Value ? true : false;
                if (success)
                {
                    AddGem(-_price.Value);
                    GuiManager.SetGemMotion();
                    taskManager.UseGem();
                }
                break;
        }
        if(success)
        {
            //Save();
        }
        return success;
    }

    /// <summary>
    /// 扣钱
    /// </summary>
    /// <param name="_price">钱数</param>
    /// <param name="_type">消费的钻石类型</param>
    /// <returns></returns>
    public bool DeductionPrice(CPrice _price, ECostGem _type)
    {
        bool success = DeductionPrice(_price);
        if(success && _price.Type == EPriceType.Gem)
        {
            GlobalModule.Instance.SendClientMessage(ClientMessageEnum.UseGem, ((int)_type).ToString() + "_" + _price.Value.ToString());
        }
        return success;
    }

    /// <summary>
    /// 当前主题当前灌溉价格
    /// </summary>
    /// <returns></returns>
    public CPrice GetCurrentIrrigationPrice()
    {
        EPriceIndex[] index = new EPriceIndex[]{ 
        EPriceIndex.PriceIrrigationLv1,
        EPriceIndex.PriceIrrigationLv2,
        EPriceIndex.PriceIrrigationLv3,
        EPriceIndex.PriceIrrigationLv4,
        EPriceIndex.PriceIrrigationLv5
        };

        CPrice price = GlobalStaticData.GetPriceInfo(index[GetCurrentScene().SceneIrrigation.Level - 1]);
        return price;
    }

    /// <summary>
    /// 当前主题增加坑位价格
    /// </summary>
    /// <returns></returns>
    public CPrice GetCurrentHolePrice()
    {
        CPrice price_hole;

        int hole_count = GetCurrentScene().SceneMachine.HoleAmount;

        if (hole_count < 20)
        {
            price_hole = new CPrice();
            price_hole.Type = EPriceType.Coin;
            price_hole.Value = hole_count * 30 - 300;
        }
        else if(hole_count < 25)
        {
            price_hole = new CPrice();
            price_hole.Type = EPriceType.Gem;
            price_hole.Value = 2;
        }
        else
        {
            price_hole = new CPrice();
            price_hole.Type = EPriceType.Gem;
            price_hole.Value = 10;
        }
        return price_hole;
    }

    /// <summary>
    /// 当前主题当前机器升级价格
    /// </summary>
    /// <returns></returns>
    public CPrice GetCurrentMachinePrice()
    {
        EPriceIndex[] index = new EPriceIndex[]{ 
        EPriceIndex.PriceMachineLv1,
        EPriceIndex.PriceMachineLv2,
        EPriceIndex.PriceMachineLv3,
        EPriceIndex.PriceMachineLv4,
        EPriceIndex.PriceMachineLv5
        };

        CPrice price = GlobalStaticData.GetPriceInfo(index[GetCurrentScene().SceneMachine.Level - 1]);
        return price;
    }

    /// <summary>
    /// 当前主题当前祭坛升级价格
    /// </summary>
    /// <returns></returns>
    public CPrice GetCurrentAltarPrice()
    {
        EPriceIndex[] index = new EPriceIndex[]{ 
        EPriceIndex.PriceAltarLv1,
        EPriceIndex.PriceAltarLv2,
        EPriceIndex.PriceAltarLv3,
        EPriceIndex.PriceAltarLv4,
        EPriceIndex.PriceAltarLv5
        };

        CPrice price = GlobalStaticData.GetPriceInfo(index[GetCurrentScene().SceneAltar.Level - 1]);
        return price;
    }

    /// <summary>
    /// 收集度
    /// </summary>
    /// <returns></returns>
    public int GetCollectPercent()
    {
        int count = 0;
        foreach (CZombieData zombie in ZombieCollection)
        {
            if(zombie.IsOpen)
            {
                count++;
            }
        }
        return count*100 / 50;
    }

    /// <summary>
    /// GUIManager
    /// </summary>
    GUIManager mGuiManager;
    public GUIManager GuiManager
    {
        get
        {
            if(!mGuiManager)
            {
                mGuiManager = GameObject.Find("GameUIRoot").GetComponent<GUIManager>();
            }
            return mGuiManager;
        }
    }

    /// <summary>
    /// 任务
    /// </summary>
    TaskManager mTaskManager;
    public TaskManager taskManager
    {
        get
        {
            if(!mTaskManager)
            {
                mTaskManager = GameObject.Find("GameUIRoot").GetComponent<TaskManager>();
            }
            return mTaskManager;
        }
    }

    /// <summary>
    /// 偷掉一只僵尸
    /// </summary>
    public void StealOneZombie()
    {
        GetCurrentScene().StealOneZombie();
        
    }

    /// <summary>
    /// 第一次使用30分钟药剂
    /// </summary>
    public bool mIsFirstKeepRun30M = true;

    /// <summary>
    /// 第一次使用1小时药剂
    /// </summary>
    public bool mIsFirstKeepRun1H = true;

    /// <summary>
    /// 第一次使用4小时药剂
    /// </summary>
    public bool mIsFirstKeepRun4H = true;

    /// <summary>
    /// 第一次使用24小时药剂
    /// </summary>
    public bool mIsFirstKeepRun24H = true;

    /// <summary>
    /// 礼物奖励等级
    /// </summary>
    public int mPlayAwardLevel = 0;
    /// <summary>
    /// 礼物奖励剩余时间
    /// </summary>
    public int mPlayAwardTime = 0;

    /// <summary>
    /// 骷髅提示次数
    /// </summary>
    public int mSkillTipsTimes = 0;

    /// <summary>
    /// 统计关卡收获钱数
    /// </summary>
    /// <param name="_money"></param>
    public void AddStoryWinMoney(int _money)
    {
        GetCurrentScene().mWinMoney += _money;
    }

    /// <summary>
    /// 统计关卡参战僵尸
    /// </summary>
    /// <param name="_zombie"></param>
    public void AddStoryWinZombie(ZombieType _zombie)
    {
        GetCurrentScene().mWinJoinZombie++;
        UpdateStoryWinMVP(_zombie);
    }

    /// <summary>
    /// 更新MVP
    /// </summary>
    /// <param name="_zombie"></param>
    public void UpdateStoryWinMVP(ZombieType _zombie)
    {
        if (GetCurrentScene().mWinMVP < _zombie)
            GetCurrentScene().mWinMVP = _zombie;
    }

    /// <summary>
    /// 开始死亡岛奖励
    /// </summary>
    public bool mIsAwardTeach = false;

    /// <summary>
    /// 财政赤字奖励
    /// </summary>
    public bool mIsAwardLackMoney = false;

    /// <summary>
    /// 死亡沼泽奖励
    /// </summary>
    public bool mIsAwardSiWangPass = false;

    /// <summary>
    /// 石头人领地奖励
    /// </summary>
    public bool mIsAwardShiTouPass = false;

    /// <summary>
    /// 富人城领地奖励
    /// </summary>
    public bool mIsAwardFuRenPass = false;

    /// <summary>
    /// 新僵尸奖励
    /// </summary>
    public bool mIsAwardNewZombie = false;

    /// <summary>
    /// 死亡岛通关奖励
    /// </summary>
    public bool mIsAwardDeathIslandPass = false;

    /// <summary>
    /// 3天后回归奖励
    /// </summary>
    public bool mIsAwardLoginBack = false;
    public int mAwardLoginBackDays = 0;

    /// <summary>
    /// 旧版本奖励
    /// </summary>
    public bool mIsOldVersion = false;

    /// <summary>
    /// 摇奖机器
    /// </summary>
    public RollMachine mRollMachine = new RollMachine();

    /// <summary>
    /// 获取金币水晶转换字符串
    /// </summary>
    /// <param name="_message"></param>
    /// <returns></returns>
    public string GetChangeText(string _message)
    {
        string msg = _message;
        msg = msg.Replace("金币", "[ffffff]ф[-]");
        msg = msg.Replace("水晶", "[ffffff]ж[-]");
        return msg;
    }

    /// <summary>
    /// 是否被冻结
    /// </summary>
    public bool mIsDeviceLocked = false;
}
