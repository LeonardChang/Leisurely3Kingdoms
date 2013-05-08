using UnityEngine;
using System.Collections;
using EasyMotion2D;

/// <summary>
/// 战斗中飞出来的金币/宝箱/钱袋/宝石
/// </summary>
public class FlyCoin : MonoBehaviour {

    /// <summary>
    /// 类型 0-金币  1-宝石
    /// </summary>
    public int type = 0;

    private int mState = 0;

    /// <summary>
    /// 价值
    /// </summary>
    public int mValue = 1;

    TweenPosition TP;

    private float hspeed = -180.0f;
    private float vspeed = 500.0f;
    private float grivaty = -2000f;

    float mTime = 0;

    /// <summary>
    /// 金钱类型 0-金币 1-钱袋 2-宝箱
    /// </summary>
    public int mLevel = 0;

    string[] ANIMATION_NAME = new string[] {"DH_JinBin", "DH_Qiandai", "DH_Baoxiang" };
    //public AudioClip acMoney;
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
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -2700);
        hspeed = Random.Range(-600f, -50f);
        GetComponent<SpriteAnimation>().Play(ANIMATION_NAME[mLevel]);

        if (type == 0)
            GameDataCenter.Instance.AddStoryWinMoney(mValue);

        if (GameDataCenter.Instance.GetSkill(ESkillType.LV75).RestTime >= 0 && type == 0)
        {
            mValue *= 2;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;

            collider.enabled = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;

            mTime = 0;
            mState = 0;
            hspeed = -180.0f;
            vspeed = 500.0f;
            grivaty = -2000f;

            Init();
        }

        if (mState == 0)
        {
            mTime += Time.deltaTime;
            transform.localPosition += new Vector3(hspeed*Time.deltaTime, vspeed*Time.deltaTime, 0);
            vspeed += (grivaty * Time.deltaTime);
            if(transform.localPosition.y < -90f)
            {
                Vector3 v3 = transform.localPosition;
                v3.y = -90f;
                transform.localPosition = v3;
                mState = 1;
                mTime = 0;
                if(mLevel == 1)
                {
                    ResourcePath.PlaySound(EResourceAudio.Audio_Money4);
                }
                else if(mLevel == 2)
                {
                    ResourcePath.PlaySound(EResourceAudio.Audio_Money5);
                }
                else
                {
                    ResourcePath.ReplaySound(EResourceAudio.Audio_Money6, 1, 0.1f);
                }
                
            }
        }
        else if (mState == 1)
        {
            mTime += Time.deltaTime;
            if(mTime >=  3)
            {
                mState = 2;
                if(type == 0)
                {
                    //OOTools.OOTweenPosition(gameObject, transform.localPosition, new Vector3(-52f, 46f, -200));
                    iTween.MoveTo(gameObject, iTween.Hash("position", GameDataCenter.Instance.GuiManager.Label_Money.transform.position,
                                                            "time", 0.5,
                                                            "oncomplete", "EndMotion",
                                                            "oncompletetarget", gameObject));

                }
                else
                {

                    //OOTools.OOTweenPosition(gameObject, transform.localPosition, new Vector3(180f, 46f, -200));
                    iTween.MoveTo(gameObject, iTween.Hash("position", GameDataCenter.Instance.GuiManager.Label_Gem.transform.position,
                                        "time", 0.5,
                                        "oncomplete", "EndMotion",
                                        "oncompletetarget", gameObject));

                }

                GameDataCenter.Instance.GuiManager.CreateFlyLabel(1, transform.parent.gameObject, transform.localPosition + new Vector3(0, 20, 2400), mValue);                

                AudioSource audio = ResourcePath.ReplaySound(EResourceAudio.Audio_TouchFlyItem, 2, 0.1f);
                if (audio != null && GlobalModule.Instance.SoundVolume > 0)
                {
                    audio.volume = 0.1f;
                }
            }
        }
	}

    /// <summary>
    /// 被触摸到
    /// </summary>
    void OnTouch()
    {
        if(mState == 1)
        {
            collider.enabled = false;
            mTime = 3;
        }
    }

    /// <summary>
    /// 飞到目的地
    /// </summary>
    void EndMotion()
    {
        if(mState == 2)
        {
            if(type == 0)
            {
                GameDataCenter.Instance.taskManager.EarnMoney(mValue);
                GameDataCenter.Instance.GuiManager.GameAddMoney(mValue);
                //Destroy(gameObject);
            }
            else
            {
                GameDataCenter.Instance.GuiManager.GameAddGem(mValue);
                //Destroy(gameObject);
            }
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<SpriteRenderer>().Apply();
            GlobalModule.Instance.Pool.DestoryObject(gameObject, "FlyMoney");
        }
    }

    void OnForceAddMoney()
    {
        if (type == 0)
        {
            GameDataCenter.Instance.GuiManager.GameAddMoney(mValue);
            //Destroy(gameObject);
        }
        else
        {
            GameDataCenter.Instance.GuiManager.GameAddGem(mValue);
            //Destroy(gameObject);
        }
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().Apply();
        GlobalModule.Instance.Pool.DestoryObject(gameObject, "FlyMoney");
    }
}
