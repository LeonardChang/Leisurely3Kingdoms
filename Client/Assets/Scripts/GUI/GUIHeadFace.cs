using UnityEngine;
using System.Collections;

/// <summary>
/// 博士头像
/// </summary>
public class GUIHeadFace : MonoBehaviour {

    enum EHeadState
    {
        Smile,
        Sad,
        Money,
        Normal
    }

    float mDelay = 0;
    UITexture mTexture;
    EHeadState mState = EHeadState.Normal;

    /// <summary>
    /// 笑
    /// </summary>
    public void SetSmile()
    {
        mDelay = 2;
        mTexture.material.mainTexture = ResourcePath.GetTexture(EResourceTexture.MainChar_smile);

        if(mState != EHeadState.Smile)
        {
            ResourcePath.PlaySound(EResourceAudio.Audio_Laugh);
            
        }

        mState = EHeadState.Smile;
    }

    /// <summary>
    /// 难过
    /// </summary>
    public void SetSad()
    {
        mDelay = 2;
        mTexture.material.mainTexture = ResourcePath.GetTexture(EResourceTexture.MainChar_sad);

        mState = EHeadState.Sad;
    }

    /// <summary>
    /// 收钱
    /// </summary>
    public void SetMoney()
    {
        mDelay = 2;
        mTexture.material.mainTexture = ResourcePath.GetTexture(EResourceTexture.MainChar_money);

        if (mState != EHeadState.Money)
        {
            if(Random.value < 0.2f)
                GameDataCenter.Instance.GuiManager.PopTipsNeedGem();

            iTween.ShakeScale(gameObject, new Vector3(0, 20, 0), 0.6f);
            ResourcePath.PlaySound(EResourceAudio.Audio_Laugh);
        }
        mState = EHeadState.Money;
    }

    /// <summary>
    /// 正常
    /// </summary>
    public void SetNormal()
    {
        mDelay = 2;
        mTexture.material.mainTexture = ResourcePath.GetTexture(EResourceTexture.MainChar_normal);

        mState = EHeadState.Normal;
    }

	// Use this for initialization
	void Start () 
    {
        mTexture = GetComponent<UITexture>();
        SetNormal();
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (mDelay > 0)
        { 
            mDelay -= Time.deltaTime;
            if(mDelay < 0)
            {
                SetNormal();
            }
        }
	}
}
