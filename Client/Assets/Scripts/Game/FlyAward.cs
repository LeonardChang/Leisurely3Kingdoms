using UnityEngine;
using System.Collections;
using EasyMotion2D;


public class FlyAward : MonoBehaviour {

    //0-金币  1-宝石
    public int AwardType = 0;
    public int mValue;

    //0-
    public int FlyType = 0;


    Vector3 start_pos;

    bool isIni = false;

    string DH_JinBi = "DH_JinBi";
    string DH_BaoShi = "DH_Baoshi";
    string DH_BaoXiang = "DH_Baoxiang";
    SpriteAnimation mSA;

    GameObject Label_Money;
    GameObject Label_Gem;

    /// <summary>
    /// 特殊类型
    /// </summary>
    public int mSpecial = 0;
    float hspeed = 0;
    float vspeed = 0;
    float grivaty = 0;
    float floor = 0;
    int state = 0;
    float delay = 0;

	// Use this for initialization
	void Start () 
    {
        start_pos = transform.localPosition;
        mSA = GetComponent<SpriteAnimation>();
        Label_Money = GameDataCenter.Instance.GuiManager.Label_Money.gameObject;
        Label_Gem = GameDataCenter.Instance.GuiManager.Label_Gem.gameObject;
        switch (FlyType)
        { 
            case 0:
                IniType0();
                break;
            case 1:
                IniType1();
                break;
            case 2:
                IniType2();
                break;
            case 3:
                IniType3();
                break;
        }

	}

	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;

            if(AwardType == 0)
            {
                mSA.Play(DH_JinBi);
            }
            else if(AwardType == 1)
            {
                mSA.Play(DH_BaoShi);
            }

            if(mSpecial == 1)
            {
                mSA.Play(DH_BaoXiang);
            }
        }

        switch(FlyType)
        {
            case 0:
                UpdateType0();
                break;
            case 1:
                UpdateType1();
                break;
            case 2:
                UpdateType2();
                break;
            case 3:
                UpdateType3();
                break;
        }
	}

    void IniType1()
    {
        vspeed = 0;
        grivaty = -2000f;
        floor = start_pos.y - 150;

    }

    void UpdateType1()
    {
        if (state == 0 || state == 1)
        {
            transform.localPosition += new Vector3(hspeed * Time.deltaTime, vspeed * Time.deltaTime, 0);
            vspeed += (grivaty * Time.deltaTime);

            if (transform.localPosition.y < floor)
            {
                if(state == 0)
                {
                    state = 1;
                    if(mSpecial == 1)
                    {
                        ResourcePath.PlaySound(EResourceAudio.Audio_Money5);
                    }
                    else
                    {
                        ResourcePath.PlaySound(EResourceAudio.Audio_Money3);
                    }
                    
                }
                else
                {
                    state = 2;
                    delay = 3;
                    grivaty = 0;
                    vspeed = 0;
                }
                Vector3 v3 = transform.localPosition;
                v3.y = floor;
                transform.localPosition = v3;
                vspeed = - vspeed/1.2f;
            }
        }
        else if(state == 2)
        {
            delay -= Time.deltaTime;
            if(delay < 0)
            {
                state = 3;
                MoveToLabel();
            }
        }
    }


    void IniType0()
    {
        vspeed = 500;
        grivaty = -2000f;

        floor = start_pos.y - 20;
    }

    /// <summary>
    /// 加钱
    /// </summary>
    void OnDie()
    {
            if (AwardType == 0)
            {
                GameDataCenter.Instance.GuiManager.GameAddMoney(mValue);
                Destroy(gameObject);
            }
            else
            {
                GameDataCenter.Instance.GuiManager.GameAddGem(mValue);
                Destroy(gameObject);
            }  
    }


    void MoveToLabel()
    {
        collider.enabled = false;
        AudioSource audio = ResourcePath.ReplaySound(EResourceAudio.Audio_TouchFlyItem, 2, 0.1f);
        if (audio != null && GlobalModule.Instance.SoundVolume > 0)
        {
            audio.volume = 0.1f;
        }
        Vector3 target = AwardType == 0 ? Label_Money.transform.position : Label_Gem.transform.position;

        iTween.MoveTo(gameObject, iTween.Hash("position", target,
                                                                    "time", 0.5,
                                                                    "oncomplete", "OnDie",
                                                                    "oncompletetarget", gameObject));


        GameDataCenter.Instance.GuiManager.CreateFlyLabel(1, transform.parent.gameObject, transform.localPosition + new Vector3(0, 20, 0), mValue);

    }


    void UpdateType0()
    {
            if(state == 0)
            {
                transform.localPosition += new Vector3(hspeed * Time.deltaTime, vspeed * Time.deltaTime, 0);
                vspeed += (grivaty * Time.deltaTime);

                if(transform.localPosition.y < floor)
                {
                    state = 1;
                    Vector3 v3 = transform.localPosition;
                    v3.y = floor;
                    transform.localPosition = v3;
                }
            }
            else if(state == 1)
            {
                state = 2;
                Invoke("MoveToLabel", 3f);
                //MoveToLabel();
            }
    }

    void IniType2()
    {
        hspeed = Random.Range(300f, 500f);
        vspeed = 400;
        state = 0;
        grivaty = -2000f;
        floor = start_pos.y - 50;
    }

    void UpdateType2()
    {
        if (state == 0)
        {
            transform.localPosition += new Vector3(hspeed * Time.deltaTime, vspeed * Time.deltaTime, 0);
            vspeed += (grivaty * Time.deltaTime);

            if (transform.localPosition.y < floor)
            {
                state = 1;
                Vector3 v3 = transform.localPosition;
                v3.y = floor;
                transform.localPosition = v3;
            }
        }
        else if (state == 1)
        {
            state = 2;
            Invoke("MoveToLabel", 3f);
            //MoveToLabel();
        }
    }

    void IniType3()
    {
        hspeed = Random.Range(-150, 150);
        vspeed = 1000;
        state = 0;
        grivaty = -2000f;
        floor = start_pos.y;
    }

    void UpdateType3()
    {
        if (state == 0)
        {
            transform.localPosition += new Vector3(hspeed * Time.deltaTime, vspeed * Time.deltaTime, 0);
            vspeed += (grivaty * Time.deltaTime);

            if (transform.localPosition.y < floor)
            {
                state = 1;
                Vector3 v3 = transform.localPosition;
                v3.y = floor + Random.Range(-10, 10);
                transform.localPosition = v3;
                ResourcePath.ReplaySound(EResourceAudio.Audio_Money6, 1, 0.1f);
            }
        }
        else if (state == 1)
        {
            state = 2;
            Invoke("MoveToLabel", 3f);
            //MoveToLabel();
        }
    }


    /// <summary>
    /// 被触摸到
    /// </summary>
    void OnTouch()
    {
        

        if (FlyType == 0)
        {
            if(state == 2)
            {
                CancelInvoke();
                MoveToLabel();
                GlobalModule.Instance.Click();
            }
            
        }
        else if(FlyType == 1)
        {
            if(state == 2)
            {
                delay = 0;
                GlobalModule.Instance.Click();
            }
        }
        else if(FlyType == 2 || FlyType == 3)
        {
            if (state == 2)
            {
                CancelInvoke();
                MoveToLabel();
                GlobalModule.Instance.Click();
            }
        }
    }

    /// <summary>
    /// 强制加钱//
    /// </summary>
    void OnForceAddMoney()
    {
        OnDie();
    }
}
