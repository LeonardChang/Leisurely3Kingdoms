using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// ��ʬö��
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
    /// ��ʬ�Ƿ񿪷�
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
/// �����ڽ�ʬ״̬
/// </summary>
public enum EZombieState
{
    Born = 0,
    Stand,
    Die
}

/// <summary>
/// �����ڽ�ʬ
/// </summary>
[Serializable]
public class CZombie{
    /// <summary>
    /// ����
    /// </summary>
    private ZombieType mType = ZombieType.Normal;
    public ZombieType Type
    {
        get { return mType; }
        set { mType = value; }
    }
    /// <summary>
    /// ״̬
    /// </summary>
    private EZombieState mState = EZombieState.Born;
    public EZombieState State
    {
        get { return mState; }
        set { mState = value;}
    }

    /// <summary>
    /// �Ƿ������ֳ�����
    /// </summary>
    private bool isNew = true;
    public bool IsNew
    {
        get { return isNew; }
        set {isNew = value;}
    }

    /// <summary>
    /// ����ʱ��
    /// </summary>
    private DateTime mBornTime = DateTime.Now;
    public DateTime BornTime
    {
        get { return mBornTime; }
        set { mBornTime = value; }
    }


    //��������Ҫ������
    public float mStandNutrient = 0;
    //��ǰ����
    private float mNutrient = 0;
    /// <summary>
    /// ����
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
    /// ��λ
    /// </summary>
    private int mHoleId = 0;
    public int HoleId
    {
        get { return mHoleId; }
        set { mHoleId = value; }
    }

    private int mSceneId = 0;
    /// <summary>
    /// ����ID
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
/// ��ʬ����
/// </summary>
[Serializable]
public class CZombieData
{
    /// <summary>
    /// ����
    /// </summary>
    private ZombieType mType = ZombieType.Normal;
    public ZombieType Type
    {
        get { return mType; }
        set{mType = value;}
    }

    /// <summary>
    /// �ֳ�����
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
    /// �Ƿ��ֳ�
    /// </summary>
    private bool isOpen = false;
    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value;}
    }


    /// <summary>
    /// �۸�ȼ�
    /// </summary>
    private int mValueLevel = 1;
    public int ValueLevel
    {
        get{return mValueLevel;}
        set{mValueLevel = value;}
    }

    /// <summary>
    /// �۸񣨿�������
    /// </summary>
    private int mValue = 0;
    public int Value
    {
        get { return mValue;}
        set { mValue = value; }
    }



    /// <summary>
    /// �������ȼ�
    /// </summary>
    private int mAttackLevel = 1;
    public int AttackLevel
    {
        get { return mAttackLevel; }
        set { mAttackLevel = value;}
    }

    /// <summary>
    /// ����������������
    /// </summary>
    private float mAttack = 1;
    public float Attack
    {
        get { return mAttack; }
        set { mAttack = value; }
    }

    /// <summary>
    /// ��ʼ������
    /// </summary>
    /// <param name="_type"></param>
    public void IniData(ZombieType _type)
    {
        mType = _type;
        mValue = ZombieInfo.IniValue;
        mAttack = ZombieInfo.AttackValue;
    }

    /// <summary>
    /// �Ƿ����µ�
    /// </summary>
    private bool isNew = false;
    public bool IsNew
    {
        get { return isNew; }
        set { isNew = value;}
    }


    /// <summary>
    /// ��ʬ��Ϣ
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
    /// ��������
    /// </summary>
    public int PropertyType= 1;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public int PropertyValue= 0;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public int Experience = 1;

    /// <summary>
    /// ����
    /// </summary>
    public float Rare = 100;


    /// <summary>
    /// ��ʼ�۸񣨿�������
    /// </summary>
    public int IniValue;

    /// <summary>
    /// ����������������
    /// </summary>
    public float AttackValue= 1;

    /// <summary>
    /// ��������
    /// </summary>
    public int AttackTimes = 1;

    /// <summary>
    /// ����ʱ�ƶ��ٶ�
    /// </summary>
    public int MoveSpeed = 200;

    /// <summary>
    /// ����
    /// </summary>
    public string Name= "";

    /// <summary>
    /// ��ʬ����
    /// </summary>
    public string Dsc= "";

    /// <summary>
    /// ��������
    /// </summary>
    public string Ability = "";

    /// <summary>
    /// ϡ��ֵ
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