using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

public class SpriteAnimationControl : MonoBehaviour 
{
    //SpriteAnimation 
    [HideInInspector]
    public SpriteAnimation mSpriteAnimation;
    [HideInInspector]
    public SpriteRenderer mSpriteRenderer;
    [HideInInspector]
    public bool isPlaying
    {
        get { return mSpriteAnimation.isPlaying; }
    }
    [HideInInspector]
    public bool isPause = false;


    public SpriteAnimationClip[] mAnimations;
    public string[] mAnimationNames;

    List<string> mAnimationNameList;


    string mCurrentClip;
    
	// Use this for initialization
	void Start () 
    {
        mSpriteAnimation = GetComponent<SpriteAnimation>();
        mSpriteRenderer = GetComponent<SpriteRenderer>();

        mSpriteRenderer.updateMode = SpriteRendererUpdateMode.Update;
        mSpriteAnimation.updateMode = SpriteRendererUpdateMode.Update;

        

        IniClips();
	}
	
    /// <summary>
    /// ��ʼ��
    /// </summary>
    void IniClips()
    {
        mSpriteAnimation.Clear();
        mAnimationNameList = new List<string>();
        for(int i = 0; i < mAnimations.Length; i++)
        {
            if(mAnimations.Length <= mAnimationNames.Length)
            {
                mSpriteAnimation.AddClip(mAnimations[i], mAnimationNames[i]);
                mAnimationNameList.Add(mAnimationNames[i]);
            }
            else
            {
                mSpriteAnimation.AddClip(mAnimations[i], mAnimations[i].name);
                mAnimationNameList.Add(mAnimations[i].name);
            }
        }

        if(mSpriteAnimation.playAutomatically && mAnimationNameList.Count > 0)
        {
            mCurrentClip = mAnimationNameList[0];
        }

        Play();
    }


    /// <summary>
    /// ���ò���(Avater)
    /// </summary>
    public void SetComponent(string _component,  Sprite _sprite)
    {
        SpriteTransform st = mSpriteRenderer.GetSpriteTransform(_component);
        if(st != null)
        {
            st.overrideSprite = _sprite;
        }
    }

    /// <summary>
    /// ��ȡ����SpriteAnimation�б�
    /// </summary>
    /// <returns></returns>
    public SpriteAnimationClip[] GetAnimationClips()
    {
        return SpriteAnimationUtility.GetAnimationClips(mSpriteAnimation);
    }

    /// <summary>
    /// ��ȡ�����б�
    /// </summary>
    /// <returns></returns>
    public SpriteAnimationClip[] GetClips()
    {
        return mAnimations;//SpriteAnimationUtility.GetAnimationClips(mSpriteAnimation);
    }

    /// <summary>
    /// ���Ŷ���
    /// </summary>
    public void Play()
    {
        if(isPause)
        {
            mSpriteAnimation.Resume(mCurrentClip);
            isPause = false;
        }
        else
        {
            mSpriteAnimation.Play(mCurrentClip);
        }
    }

    /// <summary>
    /// ��ͣ
    /// </summary>
    public void Pause()
    {
        mSpriteAnimation.Pause(mCurrentClip);
        isPause = true;
    }

    /// <summary>
    /// ֹͣ
    /// </summary>
    public void Stop()
    {
        mSpriteAnimation.Stop();
    }

    /// <summary>
    /// ��Ӷ���
    /// </summary>
    /// <param name="_clip">����</param>
    /// <param name="_name">����</param>
    public void AddClip(SpriteAnimationClip _clip, string _name)
    {
        mSpriteAnimation.AddClip(_clip, _name);
    }

	// Update is called once per frame
	void Update () 
    {
	    
	}
}
