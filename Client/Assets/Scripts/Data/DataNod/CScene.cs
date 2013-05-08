using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 制造机器
/// </summary>
public class CSceneMachine
{
    /// <summary>
    /// 坑位最大数量
    /// </summary>
    public static int maxHoleAmount = 30;


    /// <summary>
    /// 等级
    /// </summary>
    private int mLevel = 1;
    public int Level 
    {
        get { return mLevel; }
        set { if (value <= 5)  mLevel = value; }
    }


    /// <summary>
    /// 坑数
    /// </summary>
    private int mHoleAmount = 12;
    public int HoleAmount
    {
        get { return mHoleAmount; }
        set 
        {
                mHoleAmount = value;
        }
    }

    /// <summary>
    /// 生长速度
    /// ****根据等级计算
    /// </summary>
    private int mGrowSpeed = 5;
    public int GrowSpeed
    {
        get
        {
            mGrowSpeed = 5 * Level;
            return mGrowSpeed; 
        }
        set { mGrowSpeed = value; }
    }
}

/// <summary>
/// 灌溉系统
/// </summary>
public class CSceneIrrigation
{
    /// <summary>
    /// 等级
    /// </summary>
    private int mLevel = 1;
    public int Level
    {
        get { return mLevel; }
        set { if (value <= 5) mLevel = value; }
    }

    /// <summary>
    /// 稀有概率
    /// ******根据等级计算
    /// </summary>
    private int mRareProbability = 5;
    public int RareProbability
    {
        get {
            mRareProbability = 5 * Level;
            return mRareProbability; }
        set { mRareProbability = value;}
    }

    //最大运转时间
    public float mMaxLaveTime = 1;
    /// <summary>
    /// 剩余运转时间（秒）
    /// </summary>
    public float mLaveTime = 0;
    public float LaveTime
    {
        get { return mLaveTime; }
        set { mLaveTime = value; }
    }

    /// <summary>
    /// 设置时间
    /// </summary>
    /// <param name="_time"></param>
    public void SetLaveTime(float _time)
    {
        mLaveTime = _time;
        mMaxLaveTime = _time;
    }

    /// <summary>
    /// 当前剩余时间百分比
    /// </summary>
    /// <returns></returns>
    public float GetPercent()
    {
        return mLaveTime / mMaxLaveTime;
    }

    /// <summary>
    /// 运行一秒
    /// </summary>
    /// <returns></returns>
    public bool RunOneTime()
    {
        mLaveTime -= GameDataCenter.Instance.GameRunSpeed();
        if(mLaveTime > 0)
        {
            return true;
        }
        else
        {
            mLaveTime = 0;
            return false;
        }
    }

    public bool RunTime(float _times)
    {
        bool ret = false;
        mLaveTime -= (GameDataCenter.Instance.GameRunSpeed() * _times);
        if(mLaveTime <= 0)
        {
            mLaveTime = 0;
        }

        return ret;
    }
}

/// <summary>
/// 祭坛
/// </summary>
public class CSceneAltar
{
    /// <summary>
    /// 等级
    /// </summary>
    private int mLevel = 1;
    public int Level
    {
        get { return mLevel; }
        set {if(value <= 5) mLevel = value; }
    }

    /// <summary>
    /// 基础属性
    /// ****根据等级计算
    /// </summary>
    private int mProperty = 5;
    public int Property
    {
        get {
            mProperty = 5 * Level;
            return mProperty; }
        set { mProperty = value;}
    }
}

/// <summary>
/// Stage/岛屿
/// </summary>
[Serializable]
public class CScene
{
    /// <summary>
    /// 钱数
    /// </summary>
    public string MoneyString = "0";

    /// <summary>
    /// 最后一关随机关卡
    /// </summary>
    public int EndTarget = 1;

    /// <summary>
    /// 坑位列表
    /// </summary>
    public int[] HoleList = new int[]{};

    /// <summary>
    /// 是否开放
    /// </summary>
    public bool IsOpen = false;

    /// <summary>
    /// 是否新购买
    /// </summary>
    public bool IsNewOpen = true;

    /// <summary>
    /// 上次盗贼出现时间
    /// </summary>
    public DateTime mLastRobber = DateTime.MinValue;
    /// <summary>
    /// 是否有盗贼
    /// </summary>
    public bool mHasRobber = false;

    /// <summary>
    /// 是否提示盗贼
    /// </summary>
    public bool mIsRobberTips = false;

    /// <summary>
    /// 宝箱等级
    /// </summary>
    private int mChestLevel = 0;
    public int ChestLevel
    {
        set { mChestLevel = value; }
        get { return mChestLevel; }
    }

    /// <summary>
    /// 上次刷新时间
    /// </summary>
    public DateTime mLastUpdate = DateTime.Now;

    /// <summary>
    /// 场景ID
    /// </summary>
    public int mSceneId = 0;

    /// <summary>
    /// 僵尸列表
    /// </summary>
    public List<CZombie> mZombies = new List<CZombie>();
    /// <summary>
    /// 墓碑列表
    /// </summary>
    public List<int> mGraves = new List<int>();

    /// <summary>
    /// 花盆僵尸列表
    /// </summary>
    public List<CPotZombie> mPotZombie = new List<CPotZombie>();

    /// <summary>
    /// 参战僵尸
    /// </summary>
    public int mWinJoinZombie = 0;
    /// <summary>
    /// 关卡开始时间
    /// </summary>
    public DateTime mWinStartTime = DateTime.Now;
    /// <summary>
    /// 关卡收获总金币
    /// </summary>
    public int mWinMoney = 0;
    /// <summary>
    /// MVP
    /// </summary>
    public ZombieType mWinMVP = ZombieType.Normal;

    //种植机器
    private CSceneMachine mSceneMachine = new CSceneMachine();
    public CSceneMachine SceneMachine
    {
        get { return mSceneMachine; }
        set { mSceneMachine = value;}
    }
    //灌溉系统
    private CSceneIrrigation mSceneIrrigation = new CSceneIrrigation();
    public CSceneIrrigation SceneIrrigation
    {
        get { return mSceneIrrigation; }
        set { mSceneIrrigation = value;}
    }
    //祭坛
    private CSceneAltar mSceneAltar = new CSceneAltar();
    public CSceneAltar SceneAltar
    {
        get { return mSceneAltar; }
        set { mSceneAltar = value;}
    }

    //加速道具
    private CSceneItem mSceneItemSpeedUp = new CSceneItem(ESceneItemDataType.SpeedUp);
    public CSceneItem SceneItemSpeedUp
    {
        get { return mSceneItemSpeedUp; }
        set { mSceneItemSpeedUp = value;}
    }
    //维持药剂
    private CSceneItem mSceneItemKeepRun = new CSceneItem(ESceneItemDataType.KeepRun);
    public CSceneItem SceneItemKeepUp
    {
        get { return mSceneItemKeepRun; }
        set { mSceneItemKeepRun = value;}
    }
    //蜡烛
    private CSceneItem mSceneItemCandle = new CSceneItem(ESceneItemDataType.Candle);
    public CSceneItem SceneItemCandle
    {
        get { return mSceneItemCandle;}
        set { mSceneItemCandle = value;}
    }
    //闪电
    private CSceneItem mSceneItemFlash = new CSceneItem(ESceneItemDataType.Flash);
    public CSceneItem SceneItemFlash
    {
        get { return mSceneItemFlash; }
        set { mSceneItemFlash = value; }
    }




    public void AddPotZombie(CPotZombie zombie)
    {
        mPotZombie.Add(zombie);
    }

    public void RemovePotZombie(string id)
    {
        foreach (CPotZombie zombie in mPotZombie)
        {
            if(zombie.mUID == id)
            {
                mPotZombie.Remove(zombie);
                return;
            }
        }
    }

    


    /// <summary>
    /// 使用道具
    /// </summary>
    /// <param name="_type"></param>
    public void UseItem(ESceneItemDataType _type)
    {
        switch(_type)
        {
            case ESceneItemDataType.KeepRun: case ESceneItemDataType.KeepRun2: case ESceneItemDataType.KeepRun3:  case ESceneItemDataType.KeepRun4:
                mSceneItemKeepRun = CSceneItemData.GetItem(_type);
                mSceneIrrigation.SetLaveTime(mSceneItemKeepRun.mLavaTime);
                GameDataCenter.Instance.GuiManager.EffectUseItem(0, 1, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
                GameDataCenter.Instance.FirstUse();
                GameDataCenter.Instance.taskManager.UseAgent();
            break;
            case ESceneItemDataType.Candle: case ESceneItemDataType.Candle2:
                mSceneItemCandle = CSceneItemData.GetItem(_type);
                GameDataCenter.Instance.GuiManager.EffectUseItem(2, 1, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
                GameDataCenter.Instance.taskManager.UseAltar();
            break;
            case ESceneItemDataType.SpeedUp:case ESceneItemDataType.SpeedUp2:case ESceneItemDataType.SpeedUp3:
                mSceneItemSpeedUp = CSceneItemData.GetItem(_type);
                GameDataCenter.Instance.GuiManager.EffectUseItem(1, 1, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
                GameDataCenter.Instance.taskManager.UseSpeedUp();
            break;
            case ESceneItemDataType.IrrigationLvUp:
                UpgradeIrrigation();
                GameDataCenter.Instance.GuiManager.EffectUseItem(0, 0, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp1);
            break;
            case ESceneItemDataType.MachineLvUp:
                UpgradeMachine();
                GameDataCenter.Instance.GuiManager.EffectUseItem(1, 0, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp1);
            break;
            case ESceneItemDataType.AltarLvUp:
                UpgradeAltar();
                GameDataCenter.Instance.GuiManager.EffectUseItem(2, 0, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_LevelUp1);
            break;
            case ESceneItemDataType.AddHole:
                AddOneHole();
                GameDataCenter.Instance.GuiManager.EffectUseItem(1, 1, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
            break;
            case ESceneItemDataType.Flash:case ESceneItemDataType.Flash2:
                mSceneItemFlash = CSceneItemData.GetItem(_type);
                GameDataCenter.Instance.GuiManager.EffectUseItem(2, 1, _type);
                ResourcePath.PlaySound(EResourceAudio.Audio_StartWork);
            break;
        }
    }

    /// <summary>
    /// 升级灌溉（道具）
    /// </summary>
    public void UpgradeIrrigation()
    {
        mSceneIrrigation.Level += 1;
    }

    /// <summary>
    /// 升级祭坛（道具）
    /// </summary>
    public void UpgradeAltar()
    {
        mSceneAltar.Level += 1;
    }

    /// <summary>
    /// 升级机器（道具）
    /// </summary>
    public void UpgradeMachine()
    {
        mSceneMachine.Level += 1;
    }

    /// <summary>
    /// 增加一个坑(道具)
    /// </summary>
    public bool AddOneHole()
    {

        mSceneMachine.HoleAmount += 1;
        if (mSceneMachine.HoleAmount > CSceneMachine.maxHoleAmount)
        {
            mSceneMachine.HoleAmount = CSceneMachine.maxHoleAmount;
            return false;
        }
        else
        {
            AddHole(mSceneMachine.HoleAmount - 1);
            //删除一个墓碑
            GameDataCenter.Instance.GuiManager.Zombie_Manager.DeleteOneGrave(mSceneMachine.HoleAmount - 1);
            return true;
        }
    }

    /// <summary>
    /// 获取道具总成长值
    /// </summary>
    /// <returns></returns>
    public int GetItemSpeed()
    {
        return mSceneItemSpeedUp.mGrowSpeed + mSceneItemKeepRun.mGrowSpeed;
    }

    /// <summary>
    /// 获取道具总概率值
    /// </summary>
    /// <returns></returns>
    public int GetItemRare()
    {
        return mSceneItemSpeedUp.mRareProbability + mSceneItemKeepRun.mRareProbability + mSceneItemCandle.mRareProbability;
    }

    /// <summary>
    /// 获取道具总属性值
    /// </summary>
    /// <returns></returns>
    public int GetItemProperty()
    {
        return mSceneItemCandle.mProperty;
    }

    //成长值
    private int mGrowSpeed;
    public int GrowSpeed
    {
        get
        {
            return  mSceneMachine.GrowSpeed + GetItemSpeed();
        }
    }

    //稀有值
    public int RareValue
    {
        get
        {
            if(mSceneItemKeepRun.Type == ESceneItemDataType.KeepRun)
            {
                return 0;
            }
            return  mSceneIrrigation.RareProbability + GetItemRare();
        }
    }

    //属性
    public int Property
    {
        get
        {
            int zombie_pt = 0;
            foreach (CZombieData data in GameDataCenter.Instance.ZombieCollection)
            {
                if(data.ZombieInfo.PropertyType == mSceneId + 1 && data.Count > 0)
                {
                    zombie_pt += data.ZombieInfo.PropertyValue;
                }
            }
            return mSceneAltar.Property + GetItemProperty() + zombie_pt - 5;
        }
    }


    //播种剩余时间
    private float mDigTime = 0;
    public float DigTime
    {
        get { return mDigTime; }
        set { mDigTime = value;}
    }



    private List<int> emptyHoles = new List<int>();
    private List<int> zombieHoles = new List<int>();
    /// <summary>
    /// 获取一个随机坑位
    /// </summary>
    /// <returns></returns>
    public int GetRandomHole()
    {
        int amount = emptyHoles.Count;
        if (amount <= 0)
        {
            return -1;
        }
        int tmp_rnd = UnityEngine.Random.Range(0, amount);

        return emptyHoles[tmp_rnd];
    }

    /// <summary>
    /// 初始化坑位
    /// </summary>
    public void IniHoles()
    {
        emptyHoles.Clear();
        for (int i = 0; i < CSceneMachine.maxHoleAmount; i++)
        {
            emptyHoles.Add(i);
        }

        for (int i = mSceneMachine.HoleAmount; i < CSceneMachine.maxHoleAmount;  i++)
        {
            RemoveHole(i);
        }

        foreach (CZombie zombie in mZombies)
        {
            RemoveHole(zombie.HoleId);
        }
    }


    /// <summary>
    /// 添加坑位
    /// </summary>
    /// <param name="_id"></param>
    public void AddHole(int _id)
    {
        for (int i = 0; i < zombieHoles.Count; i++)
        {
            if (zombieHoles[i] == _id)
            {
                emptyHoles.Add(zombieHoles[i]);
                zombieHoles.RemoveAt(i);
                return;
            }
        }
    }


    /// <summary>
    /// 删除坑位
    /// </summary>
    /// <param name="_id"></param>
    public void RemoveHole(int _id)
    {
        for (int i = 0; i < emptyHoles.Count; i++)
        {
            if (emptyHoles[i] == _id)
            {
                zombieHoles.Add(emptyHoles[i]);
                emptyHoles.RemoveAt(i);
                return;
            }
        }
    }


    /// <summary>
    /// 设置机器维持时间
    /// </summary>
    /// <param name="_nutrient"></param>
    public void SetNutrient(float _nutrient)
    {
        mSceneIrrigation.SetLaveTime(_nutrient);
    }


    /// <summary>
    /// 僵尸养分增加
    /// </summary>
    public void AddNutrient()
    {
        /*
            foreach (CZombie zombie in mZombies)
            {
                //-----------------------------------------------------------
                zombie.Nutrient += TestSpeed * GameDataCenter.Instance.GameRunSpeed();
                //-------------------------------------------------------------
            }
         */
        AddNutrient(1);
    }

    public void AddNutrient(float _add_count)
    {
        foreach (CZombie zombie in mZombies)
        {
            //-----------------------------------------------------------
            zombie.Nutrient += (TestSpeed * GameDataCenter.Instance.GameRunSpeed() * _add_count);
            //-------------------------------------------------------------
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void SetZombieOld()
    {
        foreach(CZombie zombie in mZombies)
        {
            zombie.IsNew = false;
        }
    }

    /// <summary>
    /// 获取所有新添加的僵尸
    /// </summary>
    /// <returns></returns>
    public CZombie[] GetNewZombie()
    {
        List<CZombie> zombieList = new List<CZombie>();

        foreach(CZombie zombie in mZombies)
        {
            if(zombie.IsNew)
            {
                zombieList.Add(zombie);
            }
        }
        return zombieList.ToArray();
    }

    public int TestSpeed = 1;
    /// <summary>
    /// 更新场景(1秒)
    /// </summary>
    public void UpdateScene()
    {
        
            //-----------------------------------------------------------
            DigTime -= TestSpeed * GameDataCenter.Instance.GameRunSpeed();
            //-----------------------------------------------------------
            if(DigTime < 0)
            {
                CreateRandomZombie();
                DigTime = GetRndDigTime();
            }
            AddNutrient();
    }

    /// <summary>
    /// 更新场景
    /// </summary>
    /// <param name="_times">时间</param>
    public void UpdateScene(float _times)
    {


        int grow_speed = 70 - (GrowSpeed / 2);

        if (_times > grow_speed)
        {
            int count = (int)(_times / grow_speed);
            count = Mathf.Min(30, count);
            for(int i = 0; i < count; i ++)
            {
                CreateRandomZombie();
                DigTime = GetRndDigTime();
            }
            AddNutrient(_times);
            mSceneIrrigation.RunTime(_times);
        }
        else
        {
            DigTime -= (TestSpeed * GameDataCenter.Instance.GameRunSpeed() * _times);
            if (DigTime < 0)
            {
                CreateRandomZombie();
                DigTime = GetRndDigTime();
            }
            AddNutrient(_times);
            mSceneIrrigation.RunTime(_times);
        }

        
    }


    /// <summary>
    /// 创建随机僵尸
    /// </summary>
    /// <returns></returns>
    public bool CreateRandomZombie()
    {
        if (!GameDataCenter.Instance.IsDateTimeOk())
            return false;
        if (GameDataCenter.Instance.IsTeachMode || emptyHoles.Count <= 0) return false;

        return CreateOneZombie(GetZombieType());
    }

    /// <summary>
    /// 创建一个特定僵尸
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public bool CreateOneZombie(ZombieType _type)
    {
        int rnd_hole = GetRandomHole();
        if (rnd_hole == -1)
        {
            return false;
        }
        CZombie zombie = new CZombie();
        zombie.SetData(mSceneId, rnd_hole, GetRndGrowTime());
        zombie.Type = _type;
        RemoveHole(rnd_hole);
        DeleteGrave(rnd_hole);
        AddOneZombie(zombie);
        return true;
    }


    /// <summary>
    /// 偷掉一个僵尸
    /// </summary>
    public void StealOneZombie()
    {
        foreach (CZombie zombie in mZombies)
        {
            if(zombie.State != EZombieState.Die)
            {
                zombie.State = EZombieState.Die;
                //博士伤心
                GameDataCenter.Instance.GuiManager.HeadFace.SetSad();
                return;
            }
        }
        
    }


    /// <summary>
    /// 创建墓碑
    /// </summary>
    /// <returns></returns>
    public int CreateGraveStone()
    {
        if(mGraves.Count >= 3)
        {
            return -1;
        }

        int rnd_hole = GetRandomHole();
        foreach(int i in mGraves)
        {
            if(rnd_hole == i)
            {
                return -1;
            }
        }
        if(rnd_hole == -1)
        {
            return -1;
        }
        mGraves.Add(rnd_hole);
        return rnd_hole;
    }


    /// <summary>
    /// 删除墓碑
    /// </summary>
    /// <param name="hole"></param>
    /// <returns></returns>
    public bool DeleteGrave(int hole)
    {
        for (int i = 0; i < mGraves.Count; i ++ )
        {
            if (hole == mGraves[i])
            {
                mGraves.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获取未开放坑位
    /// </summary>
    /// <returns></returns>
    public int[] GetUnOpenHoles()
    {
        List<int> hole = new List<int>() ;

        for(int i = SceneMachine.HoleAmount; i < 30; i ++)
        {
            hole.Add(i);
        }
        return hole.ToArray();
    }




    /// <summary>
    /// 获取符合条件的僵尸列表，主题1
    /// </summary>
    /// <returns></returns>
    public ZombieType[] GetAllowCreateZombie1()
    {
        List<ZombieType> allow_zombie = new List<ZombieType>();
        allow_zombie.Clear();

        CZombieData[] zombie_list = GameDataCenter.Instance.ZombieCollection;
        DateTime now = DateTime.Now;

        for(int i = 1; i <= 14; i++)
        {
            switch (14 - i)
            {
                case 0:
                    if (Property >= 33 && zombie_list[13].Count > 0 && mSceneAltar.Level >= 5 && now.Hour >= 20 && now.Hour <= 23)
                    {
                        allow_zombie.Add(ZombieType.Zombie15);
                    }
                    break;
                case 1:
                    if (Property >= 28 && GameDataCenter.Instance.GetCurrentMoney() >= 1000 && mSceneMachine.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie14);
                    }
                    break;
                case 2:
                    if (Property >= 26 && mSceneIrrigation.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie13);
                    }
                    break;
                case 3:
                    if (Property >= 23 && zombie_list[10].Count > 0 && mSceneAltar.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie12);
                    }
                    break;
                case 4:
                    if (Property >= 21 && GameDataCenter.Instance.PlayDates.Length >= 3 && mSceneMachine.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie11);
                    }
                    break;
                case 5:
                    if (Property >= 18 && mSceneIrrigation.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie10);
                    }
                    break;
                case 6:
                    if (Property >= 15 && mSceneAltar.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie09);
                    }
                    break;
                case 7:
                    if (Property >= 13 && mSceneMachine.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie08);
                    }
                    break;
                case 8:
                    if (Property >= 11 && GameDataCenter.Instance.PlayerLevel >= 5 && mSceneIrrigation.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie07);
                    }
                    break;
                case 9:
                    if (Property >= 9 && mSceneAltar.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie06);
                    }
                    break;
                case 10:
                    if (Property >= 8 && mSceneMachine.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie05);
                    }
                    break;
                case 11:
                    if (Property >= 4 && GameDataCenter.Instance.PlayDates.Length >= 2 && mSceneIrrigation.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie04);
                    }
                    break;

                case 12:
                    if (Property >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie03);
                    }
                    break;
                case 13:
                    if (Property >= 1)
                    {
                        allow_zombie.Add(ZombieType.Zombie02);
                    }
                    break;
            }
        }
        //Debug.Log("------------------------------------------------------------");
        //Debug.Log("个数：" + allow_zombie.Count.ToString());
        //Debug.Log("Property:" + Property.ToString());
        return allow_zombie.ToArray();
    }

    /// <summary>
    /// 获取符合条件的僵尸列表,主题2
    /// </summary>
    /// <returns></returns>
    public ZombieType[] GetAllowCreateZombie2()
    {
        List<ZombieType> allow_zombie = new List<ZombieType>();
        allow_zombie.Clear();

        CZombieData[] zombie_list = GameDataCenter.Instance.ZombieCollection;
        DateTime now = DateTime.Now;

        for (int i = 1; i <= 14; i++)
        {
            switch (14 - i)
            {
                case 0:
                    Debug.Log(zombie_list[28].ZombieInfo.Name);
                    if (Property >= 33 && zombie_list[28].Count > 0 && (now.Month == 12 || now.DayOfWeek == DayOfWeek.Sunday || now.DayOfWeek == DayOfWeek.Saturday) && mSceneAltar.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie30);
                    }
                    break;
                case 1:
                    if (Property >= 28 && mSceneMachine.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie29);
                    }
                    break;
                case 2:
                    if (Property >= 26 && zombie_list[21].Count > 0 && GameDataCenter.Instance.PlayDates.Length >= 7 && mSceneIrrigation.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie28);
                    }
                    break;
                case 3:
                    if (Property >= 23 && mSceneAltar.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie27);
                    }
                    break;
                case 4:
                    if (Property >= 21 && mSceneMachine.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie26);
                    }
                    break;
                case 5:
                    if (Property >= 18 && zombie_list[19].Count > 0 && mSceneIrrigation.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie25);
                    }
                    break;
                case 6:
                    if (Property >= 15 && mSceneAltar.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie24);
                    }
                    break;
                case 7:
                    if (Property >= 13 && zombie_list[20].Count > 0 && mSceneMachine.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie23);
                    }
                    break; ;
                case 8:
                    if (Property >= 11 && mSceneIrrigation.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie22);
                    }
                    break;
                case 9:
                    if (Property >= 9 && mSceneAltar.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie21);
                    }
                    break;
                case 10:
                    if (Property >= 8 && GameDataCenter.Instance.PlayerLevel >= 7 && mSceneMachine.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie20);
                    }
                    break;
                case 11:
                    if (Property >= 4 && mSceneIrrigation.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie19);
                    }
                    break;
                case 12:
                    if (Property >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie18);
                    }
                    break;
                case 13:
                    if (Property >= 1)
                    {
                        allow_zombie.Add(ZombieType.Zombie17);
                    }
                    break;
                
            }
        }
        //Debug.Log("------------------------------------------------------------");
        //Debug.Log("个数：" + allow_zombie.Count.ToString());
        //Debug.Log("Property:" + Property.ToString());
        return allow_zombie.ToArray();
    }


    /// <summary>
    /// 获取符合条件的僵尸列表主题3
    /// </summary>
    /// <returns></returns>
    public ZombieType[] GetAllowCreateZombie3()
    {
        List<ZombieType> allow_zombie = new List<ZombieType>();
        allow_zombie.Clear();

        CZombieData[] zombie_list = GameDataCenter.Instance.ZombieCollection;
        DateTime now = DateTime.Now;

        for (int i = 1; i <= 14; i++)
        {
            switch (14 - i)
            {
                case 0:
                    if (Property >= 33 && zombie_list[43].Count > 0 && GameDataCenter.Instance.PlayDates.Length >= 10 && mSceneAltar.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie45);
                    }
                    break;
                case 1:
                    if (Property >= 28 && mSceneMachine.Level >= 5 && zombie_list[37].Count > 0)
                    {
                        allow_zombie.Add(ZombieType.Zombie44);
                    }
                    break;
                case 2:
                    if (Property >= 26 && SceneMachine.HoleAmount >= 30 && mSceneIrrigation.Level >= 5)
                    {
                        allow_zombie.Add(ZombieType.Zombie43);
                    }
                    break;
                case 3:
                    if (Property >= 23 && zombie_list[38].Count > 0 && mSceneAltar.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie42);
                    }
                    break;
                case 4:
                    if (Property >= 21 && mSceneMachine.Level >= 4)
                    {
                        allow_zombie.Add(ZombieType.Zombie41);
                    }
                    break;
                case 5:
                    if (Property >= 18 && zombie_list[35].Count > 0 && mSceneIrrigation.Level >= 4 && (now.Hour >= 20 || now.Hour <= 5))
                    {
                        allow_zombie.Add(ZombieType.Zombie40);
                    }
                    break;
                case 6:
                    if (Property >= 15 && mSceneAltar.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie39);
                    }
                    break;
                case 7:
                    if (Property >= 13 && (now.Hour >= 20 || now.Hour <= 5) && mSceneMachine.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie38);
                    }
                    break; ;
                case 8:
                    if (Property >= 11 && mSceneIrrigation.Level >= 3)
                    {
                        allow_zombie.Add(ZombieType.Zombie37);
                    }
                    break;
                case 9:
                    if (Property >= 9 && mSceneAltar.Level >= 2 && (now.Hour >= 20 || now.Hour <= 5))
                    {
                        allow_zombie.Add(ZombieType.Zombie36);
                    }
                    break;
                case 10:
                    if (Property >= 8 && mSceneMachine.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie35);
                    }
                    break;
                case 11:
                    if (Property >= 4 && mSceneIrrigation.Level >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie34);
                    }
                    break;
                case 12:
                    if (Property >= 2)
                    {
                        allow_zombie.Add(ZombieType.Zombie33);
                    }
                    break;
                case 13:
                    if (Property >= 1 && (now.Hour >= 20 || now.Hour <= 5))
                    {
                        allow_zombie.Add(ZombieType.Zombie32);
                    }
                    break;

            }
        }
        //Debug.Log("------------------------------------------------------------");
        //Debug.Log("个数：" + allow_zombie.Count.ToString());
        //Debug.Log("Property:" + Property.ToString());
        return allow_zombie.ToArray();
    }




    /// <summary>
    /// 僵尸类型
    /// </summary>
    /// <returns></returns>
    public ZombieType GetSceneZombieType(ZombieType _normal)
    {
        //return (ZombieType)UnityEngine.Random.Range(1, 45);

        ZombieType type = _normal;

        if(GetRarePercent())
        {
            CZombieData[] zombie_list = GameDataCenter.Instance.ZombieCollection;
            ZombieType[] allow_type = new ZombieType[]{};
            if(mSceneId == 0)
            {
                allow_type = GetAllowCreateZombie1();
            }
            else if(mSceneId == 1)
            {
                allow_type = GetAllowCreateZombie2();
            }
            else if(mSceneId == 2)
            {
                allow_type = GetAllowCreateZombie3();
            }

            if(allow_type.Length > 0)
            {
                int[] zombie_rare = new int[allow_type.Length];
                int amount = 0;
                string tmp = "";
                for (int i = 0; i < allow_type.Length; i++)
                {
                    int zombie_index = (int)allow_type[i] - 1;
                    amount += (int)(zombie_list[zombie_index].ZombieInfo.Rare * 10f);
                    zombie_rare[i] = amount;
                    tmp = tmp + amount.ToString() + "    ";
                }
                //Debug.Log("RARE:" + tmp);
                int value = (int)(UnityEngine.Random.value * zombie_rare[allow_type.Length - 1]);

                //Debug.Log("Value:" + value.ToString() + "   Max:" + zombie_rare[allow_type.Length - 1].ToString());
                for (int i = 0; i < allow_type.Length; i++)
                {
                    if (value < zombie_rare[i])
                    {
                        type = allow_type[i];
                        break;
                    }
                }
                //Debug.Log("类型：" + type.ToString());
                //Debug.Log("____________________________");                
            }

        }

        if (mSceneItemKeepRun.Type == ESceneItemDataType.KeepRun)
        {
            type = _normal;
        }
        return type;
    }


    /// <summary>
    /// 随机僵尸类型
    /// </summary>
    /// <returns></returns>
    public ZombieType GetZombieType()
    {
        ZombieType type = ZombieType.Normal;
        if (mSceneId == 0)
        { 
            type = GetSceneZombieType(ZombieType.Normal);
        }
        else if (mSceneId == 1)
        {
            type = GetSceneZombieType(ZombieType.Zombie16);
        }
        else if(mSceneId == 2)
        {
            type = GetSceneZombieType(ZombieType.Zombie31);
        }
        return type;
    }
    

    public void AddOneZombie(CZombie _zombie)
    {
        mZombies.Add(_zombie);
    }

    /// <summary>
    /// 根据坑位获取一个僵尸
    /// </summary>
    /// <param name="_hole"></param>
    /// <returns></returns>
    public CZombie GetOneZombie(int _hole)
    {
        foreach(CZombie zombie in mZombies)
        {
            if(zombie.HoleId == _hole)
            {
                return zombie;
            }
        }
        return null;
    }

    /// <summary>
    /// 删除一个僵尸
    /// </summary>
    /// <param name="_hole"></param>
    public void  RemoveOneZombie(int _hole)
    {
        for (int i = 0; i < mZombies.Count; i ++ )
        {
            if (mZombies[i].HoleId == _hole)
            {
                AddHole(_hole);
                mZombies.RemoveAt(i);
                return;
            }
        }
    }

    public CScene()
    {

    }

    /// <summary>
    /// 随机分布坑位
    /// </summary>
    public void RandomHoleList()
    {
        HoleList = new int[30];
        for (int i = 0; i < 30; i++)
        {
            HoleList[i] = i;
        }

        for (int i = 0; i < 100; i++)
        {
            int start = UnityEngine.Random.Range(0, 30);
            int end = UnityEngine.Random.Range(0, 30);
            int tmp = HoleList[start];
            HoleList[start] = HoleList[end];
            HoleList[end] = tmp;
        }

        for(int i = 0; i < 30; i ++)
        {
            if(HoleList[i] == 6)
            {
                int tmp;
                tmp = HoleList[14];
                HoleList[14] = HoleList[i];
                HoleList[i] = tmp;
            }
        }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Ini()
    {
        IniHoles();
        RefreshScene();
    }

    /// <summary>
    /// 更新场景
    /// </summary>
    public void RefreshScene()
    {
        if (!GameDataCenter.Instance.IsDateTimeOk())
            return;
 
        TimeSpan passTime = DateTime.Now - mLastUpdate;
        double passSecond = passTime.TotalSeconds;

        if (passSecond < 0) return;

        int flash_count = mSceneItemFlash.mLavaTime / 3600;


        int count = 0;
        if (passSecond >= 3600)
        {
            count = (int)passSecond / 3600 - flash_count;
        }

        UpdateSceneTime(passSecond);

        mSceneItemSpeedUp.UpDate(Mathf.RoundToInt((float)passSecond));
        mSceneItemKeepRun.UpDate(Mathf.RoundToInt((float)passSecond));
        mSceneItemCandle.UpDate(Mathf.RoundToInt((float)passSecond));
        mSceneItemFlash.UpDate(Mathf.RoundToInt((float)passSecond));


        for (int i = 0; i < count; i++)
        {
            if (UnityEngine.Random.value < 0.5f)
            {
                StealOneZombie();
            }
        }        
        //UpdateSceneTime(passSecond);
        mLastUpdate = DateTime.Now;
    }


    void UpdateSceneTime(double passSecond)
    {
        if(passSecond >= 1)
        {
            if ((float)passSecond > SceneItemKeepUp.mLavaTime)
            {
                UpdateScene(SceneItemKeepUp.mLavaTime);
            }
            else
            {
                UpdateScene((float)passSecond);
            }
        }
    }


    // 出芽时间
    public float GetRndDigTime()
    {
        float d_time = (44f - GrowSpeed) * 0.3f + 20f;
        return UnityEngine.Random.Range(d_time - d_time * 0.15f, d_time + d_time * 0.15f);
    }

    //成熟时间
    public float GetRndGrowTime()
    {
        float g_time =  (44f - GrowSpeed) * 0.7f + 20f;
        return UnityEngine.Random.Range(g_time - g_time * 0.05f, g_time + g_time * 0.05f);
    }

    //稀有概率
    public bool GetRarePercent()
    {
        float persent = (RareValue + 3f) * 1.1f / 100f;
        float rnd =  UnityEngine.Random.value;
        return rnd < persent;
    }
}



/// <summary>
/// 主题信息
/// </summary>
public class CSceneInfo
{
    /// <summary>
    /// 场景ID
    /// </summary>
    public int mSceneID = 0;

    /// <summary>
    /// 场景开放等级
    /// </summary>
    public int mOpenLevel = 0;


    public bool CanOpen
    {
        get 
        {
            if (GameDataCenter.Instance.PlayerLevel >= mOpenLevel)
                return true;
            else
                return false;
        }
    }
    //public int 

}
