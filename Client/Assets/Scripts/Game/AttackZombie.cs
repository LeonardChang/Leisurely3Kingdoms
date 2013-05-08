using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

public class AttackZombie : MonoBehaviour {

    /// <summary>
    /// 状态
    /// </summary>
    int state = 0;

    /// <summary>
    /// 是否停止移动
    /// </summary>
    bool isStop = false;

    float time = 0;

    /// <summary>
    /// 僵尸类型
    /// </summary>
    public ZombieType mZombieType = ZombieType.Normal;

    /// <summary>
    /// 移动速度
    /// </summary>
    float mSpeed = 200f;

    /// <summary>
    /// 攻击次数
    /// </summary>
    int mHits = 1;

    /// <summary>
    /// 动画
    /// </summary>
    SpriteAnimation mSAnimation;
    public List<string> mAnimationName;
    SpriteAnimationClip[] animationList;

    /// <summary>
    /// 僵尸数据信息
    /// </summary>
    CZombieData mZombieData;
    //public ZombieMotionList mMotionList;
	// Use this for initialization
	void Start () 
    {

        mAnimationName = new List<string>();
        mSAnimation = GetComponent<SpriteAnimation>();
        animationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);

        ZombieMotion motion = ResourcePath.GetZombie(mZombieType).GetComponent<ZombieMotion>();

        animationList[0] = motion.Move;
        animationList[1] = motion.Attack;

        SpriteAnimationUtility.SetAnimationClips(mSAnimation, animationList);
        foreach (SpriteAnimationClip ac in animationList)
        {
            mAnimationName.Add(ac.name);
        }
        mSAnimation.Play(mAnimationName[0], PlayMode.StopAll);


        mZombieData = GameDataCenter.Instance.GetOneZombieCollection(mZombieType);
        mSpeed = mZombieData.ZombieInfo.MoveSpeed;
	}
	
    void OnDestroy()
    {
    }

    /// <summary>
    /// 死掉
    /// </summary>
    void OnDie()
    {
        if (GameDataCenter.Instance.GuiManager.mIsLoading)
            return;

        CZombieData data = mZombieData;
        GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.CreateCoin(transform.position, data.Value);
        GameDataCenter.Instance.GuiManager.Attack(data.Attack);
        GameDataCenter.Instance.AddStoryWinZombie(mZombieType);
    }


    /// <summary>
    /// 强制杀死
    /// </summary>
    public void ForceKill()
    {
        CZombieData data = mZombieData;
        GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.CreateCoin(transform.position, data.Value);
        Destroy(gameObject);
    }

    private SpriteAnimation mSpriteAnimation = null;
    private SpriteAnimation CurrentSpriteAnimation
    {
        get
        {
            if (mSpriteAnimation == null)
            {
                mSpriteAnimation = gameObject.GetComponent<SpriteAnimation>();
            }
            return mSpriteAnimation;
        }
    }

	// Update is called once per frame
    void Update()
    {
        if (state == 0 && isStop == false)
        {
            transform.localPosition += new Vector3(Time.deltaTime * mSpeed, 0, 0);
            if (transform.localPosition.x > 120 - Random.Range(0, 30))
            {
                GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.AttackSoldier();
                isStop = true;
                CurrentSpriteAnimation.Play(mAnimationName[1], PlayMode.StopAll);
            }
        }

        if (isStop)
        {
            time += Time.deltaTime;

            float delay = 0.3f;
            if (time > animationList[1].length * mHits - delay)
            {
                mHits++;
                GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.MoveTarget();
                if(mHits == mZombieData.ZombieInfo.AttackTimes + 1)
                {
                    OnDie();
                    Destroy(gameObject, delay * 2);
                }
            }

        }
    }


    void CollectIt()
    {
        GameDataCenter.Instance.GuiManager.GameAddMoney(mZombieData.Value);
        Destroy(gameObject);
    }
}
