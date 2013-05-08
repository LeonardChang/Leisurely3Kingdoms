using UnityEngine;
using System.Collections;
using EasyMotion2D;


/// <summary>
/// 战斗面板
/// </summary>
public class AttackManager : MonoBehaviour {

    /// <summary>
    /// 僵尸父节点
    /// </summary>
    public Transform zombieList;
    /// <summary>
    /// 士兵父节点
    /// </summary>
    public Transform Soldier_List;

    /// <summary>
    /// 是否已经打开
    /// </summary>
    public bool isOpen = false;
    TweenPosition tp;

    public float standTime = -1;

    /// <summary>
    /// 攻击特效父节点
    /// </summary>
    public GameObject Attack_Effect;
    public UIImageButton IBtn_MiniMap;

    /// <summary>
    /// 攻击目标
    /// </summary>
    public GameObject Sprite_AttackTarget;
    public SpriteRenderer Sprite_AttackTarget_SR;

    /// <summary>
    /// 攻击目标名
    /// </summary>
    public UILabel Label_AttackTarget;
    /// <summary>
    /// 攻击背景
    /// </summary>
    public UITexture Sprite_AttackBG;



    /// <summary>
    /// 箭
    /// </summary>
    public GameObject Attack_Arrow;


    /// <summary>
    /// 初始化士兵
    /// </summary>
    void IniSoldier()
    {
        foreach (Transform trans in Soldier_List)
        {
            Destroy(trans);
        }
        
    }


    void OnDelayClose()
    {
        standTime = 0;
    }

    /// <summary>
    /// 创建一个士兵
    /// </summary>
    void CreateSoldier()
    {
        if(Sprite_AttackTarget_SR.color == Color.black)
        {
            return;
        }
        if(Soldier_List.childCount >= 3)
        {
            return;
        }
        CStory story = GameDataCenter.Instance.GetCurrentStory();
        if (story.SoldierType == 0) return;

        int pos_y = Random.Range(0, 5);
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Attack_Soldier);
        obj.transform.parent = Soldier_List;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<AttackSoldier>().mSoldierType = story.SoldierType;
        obj.transform.localPosition = new Vector3(170 + Random.value * 50, -55 - pos_y * 7, 0);
        obj.GetComponent<SpriteRenderer>().depth = 5 - pos_y;
    }

    /// <summary>
    /// 杀死所有士兵
    /// </summary>
    public void KillSoldier()
    {
        foreach (Transform trans in Soldier_List)
        {
            trans.SendMessage("Kill");
        }
    }

    /// <summary>
    /// 士兵攻击
    /// </summary>
    public void AttackSoldier()
    {
        foreach (Transform trans in Soldier_List)
        {
            trans.SendMessage("Attack");
        }
    }

    /// <summary>
    /// 击打士兵
    /// </summary>
    public void HitSoldier()
    {
        foreach (Transform trans in Soldier_List)
        {
            trans.SendMessage("Hit");
        }
    }


    //攻击目标初始位置（itween会导致位置偏差，因此需要初始化）
    Vector3 mTargetPos;

	void Start () 
    {
        tp = GetComponent<TweenPosition>();
        mTargetPos = Sprite_AttackTarget.transform.localPosition;
        Sprite_AttackTarget_SR = Sprite_AttackTarget.GetComponent<SpriteRenderer>();
	}

    /// <summary>
    /// 获取战场僵尸数量
    /// </summary>
    /// <returns></returns>
    int GetZombieCount()
    {
        return zombieList.childCount;
    }


    float mTickCount = 0;
	// Update is called once per frame
	void Update () 
    {

        //每两秒一只新士兵
        if(mTickCount < 2)
        {
            mTickCount += Time.deltaTime;
            if(mTickCount >= 2)
            {
                mTickCount = 0;
                CreateSoldier();
            }
        }

        //战场打开后18秒自动Close
        if(standTime >= 0)
        {
            standTime += Time.deltaTime;
            if (standTime > 18)
            {
                ClosePanel();
                standTime = -1;
            }
        }

        //根据面板打开状态设定按钮状态
        if (isOpen)
        {
            IBtn_MiniMap.normalSprite = "Main_Btn_ZhanChang_Down";
            IBtn_MiniMap.hoverSprite = "Main_Btn_ZhanChang_Down";
            IBtn_MiniMap.transform.FindChild("Background").GetComponent<UISprite>().spriteName = "Main_Btn_ZhanChang_Down";
        }
        else
        {
            Sprite_AttackTarget.transform.localPosition = mTargetPos;
            IBtn_MiniMap.normalSprite = "Main_Btn_ZhanChang_Nor";
            IBtn_MiniMap.hoverSprite = "Main_Btn_ZhanChang_Nor";

        }

        if (tp.enabled)
        {
            IBtn_MiniMap.transform.FindChild("Background").GetComponent<UISprite>().spriteName = "Main_Btn_ZhanChang_Nor";
            iTween.Stop(Sprite_AttackTarget);           
        }
	}

    /// <summary>
    /// 创建爆炸
    /// </summary>
    public void CreateBom()
    {
        //特效
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Fireeffect_1);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(480, 480, 1);
        Vector3 v3 = Sprite_AttackTarget.transform.localPosition;
        v3.z = -400;
        obj.transform.localPosition = v3;
        Destroy(obj, 4f);

        //闪光
        obj = ResourcePath.Instance(EResourceIndex.Prefab_BomFlash);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(2000, 3000, 1);
        obj.transform.localPosition = new Vector3(0, 0, -3500);

        //目标变黑
        Sprite_AttackTarget_SR.color = Color.black;

        
        ResourcePath.PlaySound(EResourceAudio.Audio_Explosion3);
        GameDataCenter.Instance.GuiManager.ShakeScreen();
        KillSoldier();
    }

    /// <summary>
    /// 下个宝箱
    /// </summary>
    public void NextChest()
    {

        SetTarget(GameDataCenter.Instance.GetCurrentStory());

        Sprite_AttackTarget_SR.color = new Color(1, 1, 1, 1);
    }


    /// <summary>
    /// 每下攻击特效
    /// </summary>
    public void CreateSmoke()
    {
        if(Attack_Effect.transform.childCount <= 6)
        {
            int type = Random.Range(1, 100) ;
            GameObject obj;
            //烟雾
            if(type < 50)
            {
                obj = ResourcePath.Instance(EResourceIndex.Prefab_Attack_Effect2);
                obj.transform.parent = Attack_Effect.transform;
                obj.transform.localPosition = new Vector3(Random.Range(150f, 160f), Random.Range(40f, 50f) * (-1f), 0);
                obj.transform.localScale = Vector3.one;
                OOTools.OOTweenPosition(obj, obj.transform.localPosition, obj.transform.localPosition + new Vector3(Random.Range(-30f, 30f), Random.Range(-30f, 30f), 0));
                Destroy(obj, 1);
            }

            //抓击
            obj = ResourcePath.Instance(EResourceIndex.Prefab_AttackBomb);
            obj.transform.parent = Attack_Effect.transform;
            obj.transform.localPosition = new Vector3(Random.Range(150f, 160f), Random.Range(40f, 50f) * (-1f), -500);
            obj.transform.localScale = new Vector3(480, 480, 1);
        }
    }

    /// <summary>
    /// 掉落金币
    /// </summary>
    /// <param name="_pos">位置坐标</param>
    /// <param name="_value">价值</param>
    public void CreateCoin(Vector3 _pos, int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyMoney);
        obj.transform.position = _pos;
        obj.transform.parent = transform;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyCoin>().mValue = _value;

        float rare = Random.value;
        if(rare < 0.01f)
        {
            obj.GetComponent<FlyCoin>().mLevel = 2;
            obj.GetComponent<FlyCoin>().mValue = Mathf.Max(_value * 2, 50);
        }
        else if(rare < 0.1f)
        {
            obj.GetComponent<FlyCoin>().mLevel = 1;
            obj.GetComponent<FlyCoin>().mValue = Mathf.Max((int)(_value * 1.5f), 10);
        }
        else
        {
            obj.GetComponent<FlyCoin>().mLevel = 0;
        }
    }

    /// <summary>
    /// 掉落金币（过关的时候掉落）
    /// </summary>
    /// <param name="_value">价值</param>
    public void CreateCoin(int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyMoney);
        obj.transform.parent = transform;
        obj.transform.position = Sprite_AttackTarget.transform.position;
        obj.transform.localPosition += new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), 0);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyCoin>().mValue = _value;
    }

    /// <summary>
    /// 掉落宝石（过关的时候掉落）
    /// </summary>
    /// <param name="_value"></param>
    public void CreateGem(int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Attack_FlyGem);
        obj.transform.position = Sprite_AttackTarget.transform.position;
        obj.transform.parent = transform;
        obj.transform.localPosition += new Vector3(Random.Range(-20, 20), Random.Range(-20, 20), 0);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyCoin>().mValue = _value;
        obj.GetComponent<FlyCoin>().type = 1;
    }

    /// <summary>
    /// 播放箭动画
    /// </summary>
    public void CreateArrow()
    {
        string[] arrow_str = new string[] { "DH_jian", "DH_jian2", "DH_jian" };
        if (Sprite_AttackTarget_SR.color == Color.white)
        {
            SpriteAnimation arrow_animation = Attack_Arrow.GetComponent<SpriteAnimation>();
            if(!arrow_animation.isPlaying)
            {
                arrow_animation.Play(arrow_str[GameDataCenter.Instance.mCurrentScene]);
            }
        }
    }

    /// <summary>
    /// 目标变白
    /// </summary>
    public void DeRedTarget()
    {
        if (Sprite_AttackTarget.GetComponent<SpriteRenderer>().color == Color.red)
        {
            Sprite_AttackTarget.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    /// <summary>
    /// 目标变红
    /// </summary>
    public void RedTarget()
    {
        if(Sprite_AttackTarget.GetComponent<SpriteRenderer>().color == Color.white)
        {
            Sprite_AttackTarget.GetComponent<SpriteRenderer>().color = Color.red;
            Invoke("DeRedTarget", 0.1f);
        }
    }

    /// <summary>
    /// 震动目标（视为打击攻击目标）
    /// </summary>
    public void MoveTarget()
    {
        HitSoldier();
        iTween.ShakePosition(Sprite_AttackTarget, new Vector3(0.01f, 0, 0), 0.2f);

        ResourcePath.ReplaySound(EResourceAudio.Audio_Attack, 1, 0.1f);
        CreateSmoke();
        CreateArrow();
        RedTarget();
        if(GameDataCenter.Instance.IsTeachMode)
        {
            GameDataCenter.Instance.GuiManager.CreateHitPoint(1, 0);
            GameDataCenter.Instance.GuiManager.Teach_Manager.Teach_HP -= 1;
            if(GameDataCenter.Instance.GuiManager.Teach_Manager.Teach_HP <= 0)
            {
                CreateBom();

            }
        }
    }

    /// <summary>
    /// 派出一只僵尸
    /// </summary>
    /// <param name="_type"></param>
    public void CreateOneZombie(ZombieType _type)
    {
        OpenPanel();
        standTime = 0;

        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Attack_Zombie);

        obj.GetComponent<AttackZombie>().mZombieType = _type;
        obj.transform.parent = zombieList;
        obj.transform.localScale = Vector3.one;
        int pos_y = Random.Range(0, 5);
        obj.transform.localPosition = new Vector3(-450 - Random.Range(0, 20), -60 - pos_y * 5, 0);

        obj.GetComponent<EasyMotion2D.SpriteRenderer>().depth = 5 - pos_y;
        //obj.GetComponent<EasyMotion2D.SpriteRenderer>().Apply();
    }

    /// <summary>
    /// 打开面板
    /// </summary>
    public void OpenPanel()
    {
        if (isOpen) return;
        Sprite_AttackTarget.transform.localPosition = mTargetPos;
        CStory story = GameDataCenter.Instance.GetCurrentStory();

        SetTarget(story);


        if (Sprite_AttackTarget_SR.color == Color.black)
        {
            IniSoldier();
        }

        Sprite_AttackTarget_SR.color = Color.white;

        standTime = 0;
        if(GetZombieCount() <= 0)
        {
            standTime = 5;
        }
        OOTools.OOTweenPosition(gameObject, transform.localPosition, new Vector3(0, -100f, 10f), (transform.localPosition.y + 100) / 300f * 0.25f);
        isOpen = true;
    }

    /// <summary>
    /// 设定攻击目标动画
    /// </summary>
    /// <param name="story"></param>
    void SetTarget(CStory story)
    {
        SpriteAnimation mSAnimation = Sprite_AttackTarget.GetComponent<SpriteAnimation>();
        SpriteAnimationClip[] animationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);
        animationList[0] = ResourcePath.GetAttackTarget(story.AttackTarget);
        string mAnimationName = "";
        SpriteAnimationUtility.SetAnimationClips(mSAnimation, animationList);
        foreach (SpriteAnimationClip ac in animationList)
        {
            mAnimationName = ac.name;
        }
        mSAnimation.Play(mAnimationName);

        Label_AttackTarget.text = story.Name;
        Sprite_AttackBG.material.SetTexture("_MainTex", ResourcePath.GetAttackBG(story.AttackBG));
    }






    /// <summary>
    /// 关闭面板
    /// </summary>
    public void ClosePanel()
    {
        OOTools.OOTweenPosition(gameObject, transform.localPosition, new Vector3(0, 200f, 10f), (150 - transform.localPosition.y) / 300f * 0.25f);
        isOpen = false;
    }


    public void ForceKillAllZombie()
    {
        foreach(Transform trans in zombieList)
        {
            trans.SendMessage("ForceKill");
        }
        
    }
}
