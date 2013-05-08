using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;


/// <summary>
/// 机器动画管理
/// </summary>
public class MachineManager : MonoBehaviour {

    /// <summary>
    /// 机器类型
    /// </summary>
    public EMachineType Type = EMachineType.Irrigation;

    /// <summary>
    /// 机器动画
    /// </summary>
    public SpriteAnimation mSAnimation;
    public SpriteRenderer mSRender;
    public List<string> mAnimationName = new List<string>();

    /// <summary>
    /// 需要药剂提示
    /// </summary>
    public UISprite Sprite_NeedItem;

    /// <summary>
    /// 蜡烛列表
    /// </summary>
    public GameObject[] CandleList;
    //int mCandleState = 0;

    /// <summary>
    /// 教程
    /// </summary>
    public TeachManager Panel_Teach;

    /// <summary>
    /// 运行粒子
    /// </summary>
    public GameObject FireFly;
    public GameObject FireFly2;

    GameObject FireFlyEffect;

    /// <summary>
    /// 需要药剂提示泡泡
    /// </summary>
    GameObject NeedPopTips;
    //int mCurrentLevel = -1;

    bool isIni = false;

	// Use this for initialization
	void Start () 
    {

        
        Panel_Teach = GameObject.Find("Panel_Teach").GetComponent<TeachManager>();
        mSAnimation = GetComponent<SpriteAnimation>();
        mSRender = GetComponent<SpriteRenderer>();

        if (!isIni)
        {
            isIni = true;
            UpdateMotion();

            if(GameDataCenter.Instance.mCurrentScene == 0)
            {
                FireFlyEffect = FireFly;
                Destroy(FireFly2);
            }
            else
            {
                FireFlyEffect = FireFly2;
                Destroy(FireFly);
            }
        }
	}
	

    public void UpdateMotion()
    {
        SpriteAnimationClip[] animationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);
        mAnimationName.Clear();
        foreach (SpriteAnimationClip ac in animationList)
        {
            mAnimationName.Add(ac.name);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        switch (Type)
        { 
            case EMachineType.Irrigation:
                Irrigation();
                break;
            case EMachineType.Machine:
                Machine();
                break;
            case EMachineType.Altar:
                Altar();
                break;
        }
	}

    void Irrigation()
    {
        int index = 0;
        if(GameDataCenter.Instance.GetCurrentScene().SceneItemKeepUp.GetPercent() > 0)
        {
            index = GameDataCenter.Instance.GetCurrentScene().SceneIrrigation.Level * 2 - 1;
            //Sprite_NeedItem.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
            Sprite_NeedItem.transform.localPosition = new Vector3(9999, 999, -20);
            if (!FireFlyEffect.active)
            {
                FireFlyEffect.SetActiveRecursively(true);
            }
            if(NeedPopTips)
            {
                Destroy(NeedPopTips);
                
            }
        }
        else
        {
            index = GameDataCenter.Instance.GetCurrentScene().SceneIrrigation.Level * 2 - 2;

            
            if(GameDataCenter.Instance.IsTeachMode)
            {
                if (Panel_Teach.mTeachStep >= 4 && Panel_Teach.mTeachStep < 26)
                {
                    Sprite_NeedItem.transform.localPosition = new Vector3(9999, 999, -20);
                }
                else
                {
                    Sprite_NeedItem.transform.localPosition = new Vector3(-112, -429, -20);
                }
            }
            else
            {
                Sprite_NeedItem.transform.localPosition = new Vector3(-112, -429, -20);
            }

            if (FireFlyEffect.active)
            {
                FireFlyEffect.SetActiveRecursively(false);
            }
            if(!NeedPopTips)
            {
                NeedPopTips = GameDataCenter.Instance.GuiManager.PopTipsNeedAgent();
            }
        }

        if(GameDataCenter.Instance.IsTeachMode)
        {
            if (Panel_Teach.mTeachStep >= 4)
            {
                index = 9;
            }
            else
            {
                index = 8;
            }
            if (Panel_Teach.isMachineBom)
            {
                mSRender.color = Color.black;
                index = 8;
            }
            if(Panel_Teach.mTeachStep >= 29)
            {
                index = 0;
                mSRender.color = Color.white;
            }
        }
        else
        {
            if (mSRender.color == Color.black)
            {
                mSRender.color = Color.white;
            }
        }
        if(!mSAnimation.IsPlaying(mAnimationName[index]))
        {
            mSAnimation.Play(mAnimationName[index]);
        }
    }

    void Machine()
    {
        int index = 0;
        if (GameDataCenter.Instance.GetCurrentScene().SceneItemSpeedUp.GetPercent() > 0)
        {
            index = GameDataCenter.Instance.GetCurrentScene().SceneMachine.Level * 2 - 1;
        }
        else
        {
            index = GameDataCenter.Instance.GetCurrentScene().SceneMachine.Level * 2 - 2;
        }
        if (GameDataCenter.Instance.IsTeachMode)
        {
            if (Panel_Teach.mTeachStep >= 11)
            {
                index = 9;
            }
            else
            {
                index = 8;
            }
            if (Panel_Teach.isMachineBom)
            {
                mSRender.color = Color.black;
                index = 8;
            }
            if (Panel_Teach.mTeachStep >= 29)
            {
                index = 0;
                mSRender.color = Color.white;
            }
        }
        else
        {
            if (mSRender.color == Color.black)
            {
                mSRender.color = Color.white;
                
            }
        }
        if (!mSAnimation.IsPlaying(mAnimationName[index]))
        {
            mSAnimation.Play(mAnimationName[index]);

        }
    }


    int[] CandleCounts = new int[] { 0, 1, 0, 1, 0, 2, 0, 2, 0, 3};
    int[] CandleCounts_2 = new int[] { 0, 0, 0, 4, 0, 0, 0, 5, 0, 6 };
    void Altar()
    {
        int index = 0;
        
        int altar_level = GameDataCenter.Instance.GetCurrentScene().SceneAltar.Level;
        if (GameDataCenter.Instance.GetCurrentScene().SceneItemCandle.GetPercent() > 0)
        {
            index = altar_level * 2 - 1;
        }
        else
        {
            index = altar_level * 2 - 2;
        }

        if (GameDataCenter.Instance.IsTeachMode)
        {
            if (Panel_Teach.mTeachStep >= 18)
            {
                index = 9;
            }
            else
            {
                index = 8;
            }
            if (Panel_Teach.isMachineBom)
            {
                mSRender.color = Color.black;
                index = 8;
            }
            if (Panel_Teach.mTeachStep >= 29)
            {
                index = 0;
                mSRender.color = Color.white;
            }
        }
        else
        {
            if (mSRender.color == Color.black)
            {
                mSRender.color = Color.white;
            }
        }


        if (!mSAnimation.IsPlaying(mAnimationName[index]))
        {
            mSAnimation.Play(mAnimationName[index]);
            foreach (GameObject obj in CandleList)
            {
                obj.SetActiveRecursively(false);
            }

            if (GameDataCenter.Instance.CurrentScene != 2)
            {
                if (CandleCounts[index] != 0)
                {
                    CandleList[CandleCounts[index] - 1].SetActiveRecursively(true);
                }
            }
            else
            {
                if (CandleCounts_2[index] != 0)
                {
                    CandleList[CandleCounts_2[index] - 1].SetActiveRecursively(true);
                }
            }
        }


    }




}
