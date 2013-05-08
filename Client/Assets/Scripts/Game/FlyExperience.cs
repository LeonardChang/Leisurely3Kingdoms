using UnityEngine;
using System.Collections;

/// <summary>
/// 掉落的经验以及小太阳
/// </summary>
public class FlyExperience : MonoBehaviour 
{

    public int mDepth = 0;
    public int mValue;

    /// <summary>
    /// 类型 0-经验  1-小太阳
    /// </summary>
    public int mType = 0;

    float mVSpeed = 500f;
    float mHspeed = -100f;
    float mGravity = -2000f;

    int mState = 0;
    float mMaxY;

    public int mSpecialType = 0;
    //public float hspeed = -180.0f;
    //public float vspeed = 500.0f;
   // public float grivaty = -2000f;

	// Use this for initialization
    void Start()
    {
    }

    void OnEnable()
    {
        isIni = false;
    }

    void OnDisable()
    {
        isIni = false;
    }

    private bool isIni = false;

	void Init() 
    {
        GetComponent<UISprite>().depth = 200;
        if(mType == 1)
        {
            GetComponent<UISprite>().depth -= 1;
            GetComponent<UISprite>().spriteName = "Main_Sun";
            GetComponent<UISprite>().MakePixelPerfect();
            mHspeed = Random.Range(50, 100);
        }
        else
        {
            mHspeed = Random.Range(-100, -50);
        }
        
        if(transform.localPosition.x > 200)
        {
            mHspeed = Random.Range(-200, -100);
        }
        if(transform.localPosition.x < -200)
        {
            mHspeed = Random.Range(100, 200);
        }

        mMaxY = transform.localPosition.y - 10;

        if(mSpecialType == 0)
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;

            mVSpeed = 500f;
            mHspeed = -100f;
            mGravity = -2000f;
            mState = 0;
            collider.enabled = true;

            CancelInvoke();
            Init();
        }

        if (mState == 0)
        {
            transform.localPosition = transform.localPosition + new Vector3(mHspeed * Time.deltaTime, mVSpeed * Time.deltaTime, 0);
            mVSpeed += (mGravity * Time.deltaTime);

            if(transform.localPosition.y <= mMaxY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, mMaxY, transform.localPosition.z);
                mState = 1;
                Invoke("FlyToExperience", 3);

                if(mType == 0)
                {
                    //ResourcePath.PlaySound("Money7");
                    ResourcePath.ReplaySound(EResourceAudio.Audio_Money7, 1, 0.05f);
                    //GameDataCenter.Instance.GuiManager.CreateFlyLabel(2, transform.parent.gameObject, transform.localPosition + new Vector3(0, 20, -500), mValue);
                }
                
            }
        }
	}

    void OnTouch()
    {
        collider.enabled = false;
        CancelInvoke("FlyToExperience");
        FlyToExperience();
        GlobalModule.Instance.Click();
    }

    void FlyToExperience()
    {
        AudioSource audio = ResourcePath.ReplaySound(EResourceAudio.Audio_TouchFlyItem, 2, 0.1f);
        if(audio != null && GlobalModule.Instance.SoundVolume > 0)
        {
            audio.volume = 0.1f;
        }

        Vector3 target = GameDataCenter.Instance.GuiManager.Bar_Exp.transform.position;
        target = new Vector3(target.x + GameDataCenter.Instance.GuiManager.Bar_Exp.sliderValue * 0.8f, target.y, transform.position.z);

        if(mType == 1)
        {
            target = GameDataCenter.Instance.GuiManager.Spr_MoraleBack.transform.position;
            target = new Vector3(target.x, target.y, transform.position.z);
        }
        iTween.MoveTo(gameObject, iTween.Hash("position", target,
                                            "time", 1,
                                            "easetype", iTween.EaseType.easeOutCubic,
                                            "oncomplete", "EndFly",
                                            "oncompletetarget", gameObject));
    }

    void EndFly()
    {
        if(mType == 0)
        {
            GameDataCenter.Instance.AddExperience(mValue);
        }
        
        //Destroy(gameObject);
        GlobalModule.Instance.Pool.DestoryObject(gameObject, "FlyExperience");
    }


    void OnForceAddMoney()
    {
        EndFly();
    }
}
