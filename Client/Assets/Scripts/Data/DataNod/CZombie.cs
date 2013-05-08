using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 僵尸枚举
/// </summary>
public enum ZombieType:int
{
    Normal = 1,
    Zombie02,
    Zombie03,
    Zombie04,
    Zombie05,
    Zombie06,
    Zombie07,
    Zombie08,
    Zombie09,
    Zombie10,
    Zombie11,
    Zombie12,
    Zombie13,
    Zombie14,
    Zombie15,

    Zombie16,
    Zombie17,
    Zombie18,
    Zombie19,
    Zombie20,
    Zombie21,
    Zombie22,
    Zombie23,
    Zombie24,
    Zombie25,
    Zombie26,
    Zombie27,
    Zombie28,
    Zombie29,
    Zombie30,

    Zombie31,
    Zombie32,
    Zombie33,
    Zombie34,
    Zombie35,
    Zombie36,
    Zombie37,
    Zombie38,
    Zombie39,
    Zombie40,
    Zombie41,
    Zombie42,
    Zombie43,
    Zombie44,
    Zombie45,

    
    Zombie46,
    Zombie47,
    Zombie48,
    Zombie49,
    Zombie50,
    Zombie51,
    Zombie52,
    Zombie53,
    Zombie54,
    Zombie55,
    Zombie56,
    Zombie57,
    Zombie58,
    Zombie59,
    Zombie60,
     
    Max
}

public class ZombieDataManager
{
    public static Dictionary<ZombieType, bool> mOpenZombie;

    /// <summary>
    /// 僵尸是否开放
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsZombieOpen(ZombieType type)
    {
        if(mOpenZombie == null)
        {
            mOpenZombie = new Dictionary<ZombieType, bool>();
            
            for(int i = 1; i < (int)ZombieType.Max; i++)
            {
                mOpenZombie[(ZombieType)i] = true;
            }
        }

        if (mOpenZombie.ContainsKey(type))
            return true;
        else
            return false;
    }
}





/// <summary>
/// 场景内僵尸状态
/// </summary>
public enum EZombieState
{
    Born = 0,
    Stand,
    Die
}

/// <summary>
/// 场景内僵尸
/// </summary>
[Serializable]
public class CZombie{
    /// <summary>
    /// 类型
    /// </summary>
    private ZombieType mType = ZombieType.Normal;
    public ZombieType Type
    {
        get { return mType; }
        set { mType = value; }
    }
    /// <summary>
    /// 状态
    /// </summary>
    private EZombieState mState = EZombieState.Born;
    public EZombieState State
    {
        get { return mState; }
        set { mState = value;}
    }

    /// <summary>
    /// 是否是新种出来的
    /// </summary>
    private bool isNew = true;
    public bool IsNew
    {
        get { return isNew; }
        set {isNew = value;}
    }

    /// <summary>
    /// 出生时间
    /// </summary>
    private DateTime mBornTime = DateTime.Now;
    public DateTime BornTime
    {
        get { return mBornTime; }
        set { mBornTime = value; }
    }


    //爬起来需要的养分
    public float mStandNutrient = 0;
    //当前养分
    private float mNutrient = 0;
    /// <summary>
    /// 养分
    /// </summary>
    public float Nutrient
    {
        get { return mNutrient; }
        set 
        { 
            mNutrient = value; 
            if(mNutrient >= mStandNutrient)
            {
                if(mState != EZombieState.Die)
                {
                    mState = EZombieState.Stand;
                }
            }
        }
    }


    /// <summary>
    /// 坑位
    /// </summary>
    private int mHoleId = 0;
    public int HoleId
    {
        get { return mHoleId; }
        set { mHoleId = value; }
    }

    private int mSceneId = 0;
    /// <summary>
    /// 场景ID
    /// </summary>
    public int SceneId
    {
        get { return mSceneId; }
        set { mSceneId = value; }
    }
    
    public void SetData(int _sceneId, int _holeId, float _standNutrient)
    {
        mSceneId = _sceneId;
        mHoleId = _holeId;
        mStandNutrient = _standNutrient;
    }
}


/// <summary>
/// 僵尸数据
/// </summary>
[Serializable]
public class CZombieData
{
    /// <summary>
    /// 类型
    /// </summary>
    private ZombieType mType = ZombieType.Normal;
    public ZombieType Type
    {
        get { return mType; }
        set{mType = value;}
    }

    /// <summary>
    /// 种出个数
    /// </summary>
    private int mCount = 0;
    public int Count
    {
        get { return mCount; }
        set { mCount = value;
            if(!isOpen && mCount > 0)
            {
                isOpen = true;
            }
        }
    }

    /// <summary>
    /// 是否种出
    /// </summary>
    private bool isOpen = false;
    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value;}
    }


    /// <summary>
    /// 价格等级
    /// </summary>
    private int mValueLevel = 1;
    public int ValueLevel
    {
        get{return mValueLevel;}
        set{mValueLevel = value;}
    }

    /// <summary>
    /// 价格（可升级）
    /// </summary>
    private int mValue = 0;
    public int Value
    {
        get { return mValue;}
        set { mValue = value; }
    }



    /// <summary>
    /// 攻击力等级
    /// </summary>
    private int mAttackLevel = 1;
    public int AttackLevel
    {
        get { return mAttackLevel; }
        set { mAttackLevel = value;}
    }

    /// <summary>
    /// 攻击力（可升级）
    /// </summary>
    private float mAttack = 1;
    public float Attack
    {
        get { return mAttack; }
        set { mAttack = value; }
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    /// <param name="_type"></param>
    public void IniData(ZombieType _type)
    {
        mType = _type;
        mValue = ZombieInfo.IniValue;
        mAttack = ZombieInfo.AttackValue;
    }

    /// <summary>
    /// 是否是新的
    /// </summary>
    private bool isNew = false;
    public bool IsNew
    {
        get { return isNew; }
        set { isNew = value;}
    }


    /// <summary>
    /// 僵尸信息
    /// </summary>
    public CZombieInfo ZombieInfo
    {
        get 
        {
            return GlobalStaticData.GetZombieInfo(mType);
        }
    }

}


public class CZombieInfo
{
    /// <summary>
    /// 属性类型
    /// </summary>
    public int PropertyType= 1;

    /// <summary>
    /// 属性值
    /// </summary>
    public int PropertyValue= 0;

    /// <summary>
    /// 经验值
    /// </summary>
    public int Experience = 1;

    /// <summary>
    /// 概率
    /// </summary>
    public float Rare = 100;


    /// <summary>
    /// 初始价格（可升级）
    /// </summary>
    public int IniValue;

    /// <summary>
    /// 攻击力（可升级）
    /// </summary>
    public float AttackValue= 1;

    /// <summary>
    /// 攻击次数
    /// </summary>
    public int AttackTimes = 1;

    /// <summary>
    /// 攻击时移动速度
    /// </summary>
    public int MoveSpeed = 200;

    /// <summary>
    /// 名字
    /// </summary>
    public string Name= "";

    /// <summary>
    /// 僵尸描述
    /// </summary>
    public string Dsc= "";

    /// <summary>
    /// 能力描述
    /// </summary>
    public string Ability = "";

    /// <summary>
    /// 稀有值
    /// </summary>
    public int XiYou = 0;
}

[Serializable]
public class CPotZombie
{
    public string mUID = "";

    public ZombieType mType = ZombieType.Normal;

    public Vector3 mPos = Vector3.zero;

    public int mTargetType = 0;
    public CPotZombie()
    {

    }

    public CPotZombie(string uid, ZombieType type, Vector3 pos, int target_type)
    {
        mUID = uid;
        mType = type;
        mPos = pos;
        mTargetType = target_type;
    }
}