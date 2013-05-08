using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;


public class AttackSoldier : MonoBehaviour {


    //0-≥ˆœ÷  1-’æ¡¢ 2-π•ª˜ 3-À¿Õˆ
    int state = 0;
    float mMoveSpeed = 100;
    int hp;// = Random.Range();

    public int mSoldierType = 0;

    /// <summary>
    /// π•ª˜
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackOther()
    {
        yield return new WaitForSeconds(Random.Range(0.2f, 1f));
        if (state == 1)
        {
            state = 2;
            mSAnimation.Play(mAnimationName[1]);
        }
    }

    /// <summary>
    ///  ‹…À∫¶
    /// </summary>
    /// <returns></returns>
    IEnumerator HitSelf()
    {
        yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        hp -= 1;
        if(hp <= 0)
        {
            Kill();
        }
    }

    /// <summary>
    ///  ‹…À∫¶
    /// </summary>
    public void Hit()
    {
        StartCoroutine(HitSelf());
        StartCoroutine(AttackOther());
    }

    /// <summary>
    /// π•ª˜
    /// </summary>
    public void Attack()
    {
        StartCoroutine(AttackOther());
    }
   
    /// <summary>
    /// À¿µÙ
    /// </summary>
    public void Kill()
    {
        state = 3;
        mSAnimation.Play(mAnimationName[0]);
        ResourcePath.ReplaySound(EResourceAudio.Audio_EnemyDie, 1, 0.3f);
    }

    List<string> mAnimationName;
    SpriteAnimation mSAnimation;
    SpriteAnimationClip[] animationList;

	// Use this for initialization
	void Start () 
    {
        hp = Random.Range(2, 5);
        mAnimationName = new List<string>();
        mSAnimation = GetComponent<SpriteAnimation>();
        animationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);
        AttackSoldierMotion motion = ResourcePath.GetSoldier(mSoldierType).GetComponent<AttackSoldierMotion>();

        animationList[0] = motion.Die;
        animationList[1] = motion.Attack;
        animationList[2] = motion.Move;
        animationList[3] = motion.Stand;


        SpriteAnimationUtility.SetAnimationClips(mSAnimation, animationList);
         
 
        foreach (SpriteAnimationClip ac in animationList)
        {
            mAnimationName.Add(ac.name);
        }

        mSAnimation.Play(mAnimationName[2]);
        /*
        mZombieData = GameDataCenter.Instance.GetOneZombieCollection(mZombieType);
        */
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (state == 0)
        {
            transform.localPosition += new Vector3(-mMoveSpeed * Time.deltaTime, 0, 0);
            if(transform.localPosition.x < 100 + Random.Range(0, 100))
            {
                state = 1;
                mSAnimation.Play(mAnimationName[3]);
            }
        }
        if (state == 2)
        { 
            if(!mSAnimation.isPlaying)
            {
                state = 1;
                mSAnimation.Play(mAnimationName[3]);
            }
        }
        if (state == 3)
        { 
            if(!mSAnimation.isPlaying)
            {
                
                Destroy(gameObject);
            }
        }
	}
}
