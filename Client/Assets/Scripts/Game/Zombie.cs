using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

public class Zombie : MonoBehaviour {
    //僵尸状态枚举
    public enum ZombieState
    {
        Born = 0,
        Born2,
        Stand,
        SayHello,
        ComeOut,
        Back,
        RunAway,

        Draging,
        DragComeout,
        Falling,
        Die,
    };

    //僵尸数据
    public CZombie mZombie;
    //僵尸状态
    ZombieState state = ZombieState.Born;

    //public AudioClip runawayAudio;

    public SpriteAnimation mSAnimation;
    public List<string> mAnimationName = new List<string>();
    bool isIni = false;

    /// <summary>
    /// 待机动画
    /// </summary>
    public int mStandPos;

    /// <summary>
    /// 父节点
    /// </summary>
    public ZombieManager mParent;
    //bool isDraging = false;


    SpriteAnimationClip[] mAnimationList;

    Vector3 mLastPos;
    float mDragingTime = 2f;
    bool hasMoney = true;

    GameObject ZombieTalk;
    Transform ZombieTalkList;

    public bool mIsSpecial = false;
    public int mSpecialType = 0;
    //动画ID
 /*0伸手 1.手待机 2.摆手 3.手消失 4.种出 5.待机1  6.待机2 7.待机3  8.拉起  9.走开 10.拖拽*/
	void Start () 
    {
        ZombieTalkList = GameObject.Find("ZombieTalkList").transform;
        mParent = transform.parent.GetComponent<ZombieManager>();
        mSAnimation = GetComponent<SpriteAnimation>();
        SpriteAnimationClip[] mAnimationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);


        ZombieMotion mMotion = ResourcePath.GetZombie(mZombie.Type).GetComponent<ZombieMotion>();
        mAnimationList[4] = mMotion.ComeOut;
        mAnimationList[5] = mMotion.Stand1;
        mAnimationList[6] = mMotion.Stand2;
        mAnimationList[8] = mMotion.PickUp;
        mAnimationList[9] = mMotion.GoAway;
        mAnimationList[10] = mMotion.Draging;

        SpriteAnimationUtility.SetAnimationClips(mSAnimation, mAnimationList);
        foreach (SpriteAnimationClip ac in mAnimationList)
        {
            mAnimationName.Add(ac.name);
        }
        mAnimationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);

        //随机方向
        if (Random.value < 0.5)
        {
            Vector3 v3 = transform.localScale;
            v3.x = -v3.x;
            transform.localScale = v3;
        }

        float value = Random.value;
        mStandPos = value > 0.5f ? 5 : 6;
        //GetComponent<SpriteRenderer>().Apply();

        mLastPos = transform.localPosition;


	}

    /// <summary>
    /// 掉落摇晃的金币
    /// </summary>
    /// <param name="_value"></param>
    void CreateMoney(int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Panel_Main.transform;
        obj.transform.position = transform.position + new Vector3(0, 0.125f, -2);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mValue = _value;
        obj.GetComponent<FlyAward>().FlyType = 1;
    }

    /// <summary>
    /// 掉落经验
    /// </summary>
    void CreateExperience()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyExperience);
        obj.transform.parent = GameObject.Find("Panel_Main_FlyItemParent").transform;
        obj.transform.position = transform.position;
        obj.transform.localScale = new Vector3(50, 48, 1);
        obj.GetComponent<FlyExperience>().mValue = GameDataCenter.Instance.GetOneZombieCollection(mZombie.Type).ZombieInfo.Experience;
        

        obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyExperience);
        obj.transform.parent = GameObject.Find("Panel_Main_FlyItemParent").transform;
        obj.transform.position = transform.position;
        obj.transform.localScale = new Vector3(50, 48, 1);
        obj.GetComponent<FlyExperience>().mValue = GameDataCenter.Instance.GetOneZombieCollection(mZombie.Type).ZombieInfo.Experience;
        obj.GetComponent<FlyExperience>().mType = 1;
    }

    /// <summary>
    /// 说话（待机）
    /// </summary>
    void CreateTalk()
    {
        if(ZombieTalkList.childCount < 1 && !ZombieTalk)
        {
            ZombieTalk = ResourcePath.Instance(EResourceIndex.Prefab_ZombieTalk);
            ZombieTalk.transform.parent = ZombieTalkList;
            if(transform.localPosition.x < 0)
            {
                ZombieTalk.transform.localPosition = transform.localPosition + new Vector3(60, 80, -90);
            }
            else
            {
                ZombieTalk.transform.localPosition = transform.localPosition + new Vector3(-40, 80, -90);
                Vector3 scale = ZombieTalk.transform.FindChild("Background").transform.localScale;
                scale.x = -scale.x;
                ZombieTalk.transform.FindChild("Background").transform.localScale = scale;
                ZombieTalk.transform.FindChild("Background").transform.localPosition += new Vector3(-15, 0, 0);
            }
            ZombieTalk.transform.FindChild("Label").GetComponent<UILabel>().text = GlobalStaticData.GetZombieTalk();
            ZombieTalk.transform.localScale = Vector3.one;
            ZombieTalk.name = "ZombieTalk";
            Destroy(ZombieTalk, 3);
            ResourcePath.PlaySound(EResourceAudio.Audio_ZombieTalk);
        }
    }

    /// <summary>
    /// 说话（手）
    /// </summary>
    void CreateTalk2()
    {
        if (ZombieTalk)
        {
            return;
        }
        foreach (Transform trans in ZombieTalkList)
        {
            Destroy(trans.gameObject);
        }
            ZombieTalk = ResourcePath.Instance(EResourceIndex.Prefab_ZombieTalk);
            ZombieTalk.transform.parent = ZombieTalkList;
            if (transform.localPosition.x < 0)
            {
                ZombieTalk.transform.localPosition = transform.localPosition + new Vector3(60, 80, -90);
            }
            else
            {
                ZombieTalk.transform.localPosition = transform.localPosition + new Vector3(-40, 80, -90);
                Vector3 scale = ZombieTalk.transform.FindChild("Background").transform.localScale;
                scale.x = -scale.x;
                ZombieTalk.transform.FindChild("Background").transform.localScale = scale;
                ZombieTalk.transform.FindChild("Background").transform.localPosition += new Vector3(-15, 0, 0);
            }
            ZombieTalk.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.mStringType == ELocalizationTyp.Chinese ? "你好啊～" : "Hello~";
            ZombieTalk.transform.localScale = Vector3.one;
            ZombieTalk.name = "ZombieTalk";
            Destroy(ZombieTalk, 3);
            ResourcePath.PlaySound(EResourceAudio.Audio_ZombieTalk);
        
    }

    private BoxCollider mBoxCollider = null;
    private BoxCollider CurrentCollider
    {
        get
        {
            if (mBoxCollider == null)
            {
                mBoxCollider = gameObject.GetComponent<BoxCollider>();
            }
            return mBoxCollider;
        }
    }

    private SpriteRenderer mSpriteRenderer = null;
    private SpriteRenderer CurrentSpriteRenderer
    {
        get
        {
            if (mSpriteRenderer == null)
            {
                mSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            }
            return mSpriteRenderer;
        }
    }

	// Update is called once per frame
    void Update()
    {
        if (!isIni)
        {
            if (mZombie.State == EZombieState.Stand)
            {
                mSAnimation.Play(mAnimationName[4]);
                state = ZombieState.ComeOut;
                
            }
            else if (mZombie.State == EZombieState.Die)
            {
                mSAnimation.Play(mAnimationName[12]);
                state = ZombieState.Die;
                CurrentCollider.size = new Vector3(40, 50, 1);
            }
            else
            {
                mSAnimation.Play(mAnimationName[0]);
                CurrentCollider.size = new Vector3(40, 50, 1);
                state = ZombieState.Born;
            }
            isIni = true;
            if (mIsSpecial)
            {
                if(mSpecialType == 0)
                {
                    SetGetByFall();
                }
                else if (mSpecialType == 1)
                {
                    SetStandPot();
                }
            }

                
                //SetHasDragOut();
        }


        if(mIsGetFalling && mIsGetByFall)
        {
            transform.localPosition -= Time.deltaTime * new Vector3(0, 600, 0);

            if(transform.localPosition.y < mFallTargety)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, mFallTargety, transform.localPosition.z);
                mIsGetFalling = false;
            }
        }
        else
        {
            NormalUpdate();
        }
        
	}

    void NormalUpdate()
    {

        CZombie zombie = mZombie;

        switch (state)
        {
            case ZombieState.Back:
                if (!mSAnimation.isPlaying)
                {
                    mSAnimation.Play(mAnimationName[4]);
                    state = ZombieState.ComeOut;
                    CurrentCollider.size = new Vector3(40, 200, 1);
                }
                break;
            case ZombieState.ComeOut:
                if (!mSAnimation.isPlaying)
                {
                    mSAnimation.Play(mAnimationName[mStandPos]);
                    state = ZombieState.Stand;
                }
                break;
            case ZombieState.Born:
                if (!mSAnimation.isPlaying)
                {
                    mSAnimation.Play(mAnimationName[1]);
                    state = ZombieState.Born2;
                }
                break;
            case ZombieState.SayHello:
                if (!mSAnimation.isPlaying)
                {
                    state = ZombieState.Born2;
                    mSAnimation.Play(mAnimationName[1]);
                }
                break;
            case ZombieState.Born2:
                if (zombie.State == EZombieState.Die)
                {
                    mSAnimation.Play(mAnimationName[12]);
                    state = ZombieState.Die;
                    CurrentCollider.size = new Vector3(40, 50, 1);
                }
                if (zombie.State == EZombieState.Stand)
                {
                    mSAnimation.Play(mAnimationName[3]);
                    state = ZombieState.Back;
                }
                break;
            case ZombieState.RunAway:
                if (!mSAnimation.isPlaying)
                {
                    GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.GetComponent<AttackManager>().CreateOneZombie(mZombie.Type);
                    Destroy(gameObject);
                }
                break;
            case ZombieState.Stand:
                mSAnimation.Play(mAnimationName[mStandPos]);

                if (zombie.State == EZombieState.Die)
                {
                    mSAnimation.Play(mAnimationName[12]);
                    state = ZombieState.Die;
                    CurrentCollider.size = new Vector3(40, 50, 1);
                }
                else
                {
                    if (Random.value < 0.0001f)
                    {
                        CreateTalk();
                    }
                }
                break;
            case ZombieState.DragComeout:
                if (!mSAnimation.isPlaying)
                {
                    if (mIsGetByFall || mSpecialType == 1)
                        DeletePot();

                    //collider.enabled = false;
                    if (mParent.isFirstCollect)
                    {
                        mParent.isFirstCollect = false;
                        mSAnimation.Play(mAnimationName[10]);
                        state = ZombieState.Draging;
                        //isDraging = true;
                        CurrentSpriteRenderer.depth = 80;
                    }
                    else
                    {
                        RunAway();
                    }
                }
                break;
            case ZombieState.Draging:
                if (!Input.GetMouseButton(0))
                {
                    state = ZombieState.Falling;
                    mParent.isFirstCollect = true;
                }
                else
                {
                    mDragingTime -= Time.deltaTime;
                    Vector3 tmp = GameDataCenter.Instance.GuiManager.Camera_2D.ScreenToWorldPoint(Input.mousePosition);
                    tmp.z = transform.position.z;
                    tmp += new Vector3(0, -0.55f, 0);
                    transform.position = Vector3.Lerp(transform.position, tmp, 0.2f);

                    if (mDragingTime < -1)
                    {
                        GameDataCenter.Instance.GuiManager.SetDraggingHand(tmp);
                    }

                    if (Mathf.Pow(transform.localPosition.x - mLastPos.x, 2) + Mathf.Pow(transform.localPosition.y - mLastPos.y, 2) + Mathf.Pow(transform.localPosition.z - mLastPos.z, 2) > 400 && mDragingTime <= 0 && hasMoney)
                    {

                        float rnd_value = 0.2f;
                        if (GameDataCenter.Instance.TodayShakeMoney > 10)
                        {
                            rnd_value = 0.2f - GameDataCenter.Instance.TodayShakeMoney * 0.004f;
                        }
                        if (Random.value < rnd_value)
                        {
                            CreateMoney(1);
                            GameDataCenter.Instance.TodayShakeMoney += 1;
                            hasMoney = false;
                            GameDataCenter.Instance.GuiManager.HideDraggingHand();
                        }
                    }

                    mLastPos = transform.localPosition;
                }

                break;
            case ZombieState.Falling:

                transform.localPosition -= Time.deltaTime * new Vector3(0, 600, 0);
                if (transform.localPosition.y < 0f)
                {
                    AudioSource audios = ResourcePath.ReplaySound(EResourceAudio.Audio_se005, 1, 0.05f);
                    if (audios != null)
                    {
                        audios.pitch = 0.75f;
                    }
                    RunAway();
                    if (transform.localPosition.y > -50)
                    {
                        CurrentSpriteRenderer.depth = 115;
                    }
                    else if (transform.localPosition.y > -100)
                    {
                        CurrentSpriteRenderer.depth = 105;
                    }
                    else if (transform.localPosition.y > -150)
                    {
                        CurrentSpriteRenderer.depth = 95;
                    }
                }
                break;
        }
    }


    void LateUpdate()
    {
        if(isTouch)
        {
            mTouchCount += Time.deltaTime;
            if (mTouchCount > 0.1f)
            {
                isTouch = false;
                mTouchCount = 0;
            }
        }
    }

    /// <summary>
    /// 跑掉
    /// </summary>
    void RunAway()
    {
        state = ZombieState.RunAway;
        mSAnimation.Play(mAnimationName[9]);
        CurrentSpriteRenderer.depth -= 1;
    }



    bool isTouch = false;
    float mTouchCount = 0;
    int mClickTimes = 3;
    /// <summary>
    /// 被触摸到
    /// </summary>
    public void CheckTouch()
    {
        if (state == ZombieState.Stand)
        {
            if (GameDataCenter.Instance.IsTeachMode || mIsSpecial)
            {
                GameDataCenter.Instance.CollectOneZombie(mZombie.Type);
                if(mIsSpecial)
                {
                    GameDataCenter.Instance.GetCurrentScene().RemovePotZombie(mUID);
                }
            }
            else
            {
                GameDataCenter.Instance.CollectOneZombie(mZombie.SceneId, mZombie.HoleId);
            }


            Invoke("CreateExperience", 1f);
            StartCoroutine(DeleteCollider());
            AudioSource audios = ResourcePath.ReplaySound(EResourceAudio.Audio_se004, 2, 0.1f);
            if (audios != null)
            {
                audios.pitch = 0.75f;
            }
            state = ZombieState.DragComeout;
            mSAnimation.Play(mAnimationName[8]);
            Destroy(ZombieTalk);
        }
        else if (state == ZombieState.Born2 || (state == ZombieState.SayHello && !isTouch))
        {
            ResourcePath.ReplaySound(EResourceAudio.Audio_Hand, 1, 0.1f);
            state = ZombieState.SayHello;
            mSAnimation.Play(mAnimationName[2]);
            if (mClickTimes > 0)
            {
                mClickTimes -= 1;
                if (mClickTimes <= 0)
                {
                    CreateTalk2();
                    mClickTimes = 3;
                }
            }

        }
        else if(state == ZombieState.Die)
        {
            collider.enabled = false;
            GameDataCenter.Instance.DeleteOneZombie(mZombie.SceneId, mZombie.HoleId);
            iTween.ShakePosition(gameObject, new Vector3(0.02f, 0, 0), 0.3f);
            StartCoroutine(DestroySelf());
            //Destroy(gameObject);
        }
        isTouch = true;
        mTouchCount = 0;
    }

    /// <summary>
    /// 删除碰撞盒
    /// </summary>
    /// <returns></returns>
    IEnumerator DeleteCollider()
    {
        yield return new WaitForSeconds(0.1f);
        collider.enabled = false;
    }

    /// <summary>
    /// 被偷掉
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroySelf()
    {
        if (GameDataCenter.Instance.IsRobberTips && GameDataCenter.Instance.mSkillTipsTimes < 3)
        {
            GlobalModule.Instance.ShowInGameMessageBox(StringTable.GetString(EStringIndex.Tips_TitleTips), StringTable.GetString(EStringIndex.Tips_StealZombie),
                    StringTable.GetString(EStringIndex.Tips_OK));
            GameDataCenter.Instance.mSkillTipsTimes++;
        }
        yield return new WaitForSeconds(0.5f);
        GameDataCenter.Instance.CollectDieZombie();
        Destroy(gameObject);
    }


    public void SetHasDragOut()
    {
        mSAnimation.Play(mAnimationName[10]);
        state = ZombieState.Draging;
        //isDraging = true;
        CurrentSpriteRenderer.depth = 80;
    }


    bool mIsGetByFall = false;
    bool mIsGetFalling = false;
    int mFallTargety = 0;
    public void SetGetByFall()
    {
        mSAnimation.Play(mAnimationName[mStandPos]);
        state = ZombieState.Stand;
        mIsGetByFall = true;
        mIsGetFalling = true;
        CurrentCollider.size = new Vector3(40, 200, 1);


        RandomFallTargetY();
        AddPot();
    }

    public string mUID = "";
    void SetStandPot()
    {
        mSAnimation.Play(mAnimationName[mStandPos]);
        state = ZombieState.Stand;
        CurrentCollider.size = new Vector3(40, 200, 1);
        RandomFallTargetY();
        AddPot();
    }


    public int mRndTargetType = 0;
    void RandomFallTargetY()
    {
        switch (mRndTargetType)
        {
            case 0:
                mFallTargety = 0;
                CurrentSpriteRenderer.depth = 115;
                break;
            case 1:
                mFallTargety = -200;
                CurrentSpriteRenderer.depth = 85;
                break;
        }
    }

    /// <summary>
    /// 添加花盆
    /// </summary>
    void AddPot()
    {
        GameObject obj = ResourcePath.Instance("Sprite_HuaPen");
        obj.name = "HuaPen";
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.FindChild("Sprite_Pen_Front").GetComponent<SpriteRenderer>().depth = CurrentSpriteRenderer.depth - 1;
        obj.transform.FindChild("Sprite_Pen_Back").GetComponent<SpriteRenderer>().depth = CurrentSpriteRenderer.depth + 1;
    }

    /// <summary>
    /// 删除花盆
    /// </summary>
    void DeletePot()
    {
        GameObject obj = transform.FindChild("HuaPen").gameObject;
        if(obj)
        {
            Destroy(obj);
        }
    }


    void CollectIt()
    {
        if(state == ZombieState.Draging || state == ZombieState.RunAway)
        {
            GameDataCenter.Instance.GuiManager.GameAddMoney(GameDataCenter.Instance.GetOneZombieCollection(mZombie.Type).Value);
            Destroy(gameObject);
        }
    }
}
