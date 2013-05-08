using UnityEngine;
using System.Collections;

public class Logo : MonoBehaviour {
    public UISprite[] LogoSprite;
    public GameObject MainPanel;
    
    public UITexture ColorVersionSprite;

    private bool mShowing = false;
    private float mShowingTime = 0;
    private float mShowingTimeMax = 2.1f;
	
	public string NextLoadLevel = "Initialize";

    void Awake()
    {
        Market marketID = (Market)PubilshSettingData.Instance.SelectedMarket;
        Publisher publisherID = (Publisher)PubilshSettingData.Instance.SelectedPublisher;

        print("Market: " + marketID.ToString());
        print("Publisher: " + publisherID.ToString());

#if UNITY_IPHONE
        switch (publisherID)
        {
            case Publisher.PlayPlusPlus:
                // 暗红色
                ColorVersionSprite.color = new Color(0.49f, 0.11f, 0.12f);
                break;
            case Publisher.Mobage_CN:
                // 橙色
                ColorVersionSprite.color = new Color(1, 0.32f, 0.02f);
                break;
            case Publisher.Mobage_TW:
                // 柠檬黄
                ColorVersionSprite.color = new Color(1.0f, 1.0f, 0);
                break;
            case Publisher.BBG_JP:
                // 浅粉色
                ColorVersionSprite.color = new Color(0.98f, 0.78f, 1);
                break;
            default:
                // 棕色
                ColorVersionSprite.color = new Color(0.49f, 0.28f, 0.13f);
                break;
        }
#elif UNITY_ANDROID
        switch (publisherID)
        {
            // 绿色系
            case Publisher.Manloo_MM:
                // 墨绿色
                ColorVersionSprite.color = new Color(0.03f, 0.32f, 0.18f);
                break;
            case Publisher.Manloo_G:
                // 绿色
                ColorVersionSprite.color = new Color(0.06f, 0.74f, 0.42f);
                break;
            case Publisher.Manloo_GLOBAL:
                // 浅绿色
                ColorVersionSprite.color = new Color(0.51f, 0.96f, 0.75f);
                break;

            // 红色系
            case Publisher.PlayPlusPlus:
                // 暗红色
                ColorVersionSprite.color = new Color(0.49f, 0.11f, 0.12f);
                break;

            // 黄色系
            case Publisher.Mobage_CN:
                // 橙色
                ColorVersionSprite.color = new Color(1, 0.32f, 0.02f);
                break;
            case Publisher.Mobage_TW:
                // 柠檬黄
                ColorVersionSprite.color = new Color(1.0f, 1.0f, 0);
                break;

            // 粉色系
            case Publisher.BBG_JP:
                // 浅粉色
                ColorVersionSprite.color = new Color(0.98f, 0.78f, 1);
                break;
            case Publisher.BBG_JP_Yahoo:
                // 粉色
                ColorVersionSprite.color = new Color(0.94f, 0.4f, 1);
                break;
            case Publisher.BBG_JP_Samsung:
                // 深粉色
                ColorVersionSprite.color = new Color(0.75f, 0, 0.84f);
                break;
            case Publisher.BBG_JP_Amazon:
                // 浅粉红
                ColorVersionSprite.color = new Color(1, 0.44f, 0.65f);
                break;
            case Publisher.BBG_JP_Au:
                // 深粉红
                ColorVersionSprite.color = new Color(1, 0.18f, 0.48f);
                break;

            default:
                // 棕色
                ColorVersionSprite.color = new Color(0.49f, 0.28f, 0.13f);
                break;
        }
#else
        // 灰色
        ColorVersionSprite.color = new Color(0.5f, 0.5f, 0.5f);
#endif
    }

	// Use this for initialization
	void Start () {
        //if (!UseMarketLogo)
        //{
            GoSpriteInit();
        //}
        //else
        //{
        //    MarketFadein();
        //    Invoke("MarketFadeout", 1.5f);
        //    Invoke("GoSpriteInit", 3.0f);
        //}
	}
	
	// Update is called once per frame
	void Update () {
        if (mShowing)
        {
            mShowingTime += Time.deltaTime;
            if (mShowingTime >= mShowingTimeMax)
            {
                mShowingTime = 0;
                mShowing = false;

                TweenColor tc = null;
                foreach (UISprite sp in LogoSprite)
                {
                    tc = TweenColor.Begin(sp.gameObject, 1.0f, new Color(1, 1, 1, 0));
                }
                tc.eventReceiver = gameObject;
                tc.callWhenFinished = "KillSelf";
            }
        }
	}

    private AudioSource Audio
    {
        get { return gameObject.GetComponent<AudioSource>(); }
    }

    private void PlayAudio(string _clip, float _volume, float _pitch)
    {
        Audio.clip = Resources.Load(_clip) as AudioClip;
        Audio.pitch = _pitch;
        Audio.volume = _volume;
        Audio.Play();
    }

    //public void MarketFadein()
    //{
    //    TweenColor.Begin(MarketLogo.gameObject, 0.5f, new Color(1, 1, 1, 1));
    //}

    //public void MarketFadeout()
    //{
    //    TweenColor.Begin(MarketLogo.gameObject, 1.0f, new Color(1, 1, 1, 0));
    //}

    public void GoSpriteInit()
    {
        TweenColor tc = TweenColor.Begin(LogoSprite[0].gameObject, 0.15f, new Color(1, 1, 1, 1));
        tc.eventReceiver = gameObject;
        tc.callWhenFinished = "GoSprite0";
    }

    public void GoSprite0()
    {
        TweenScale ts = TweenScale.Begin(LogoSprite[0].gameObject, 0.25f, new Vector3(65, 71, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoSprite1";

        TweenColor.Begin(LogoSprite[1].gameObject, 0.15f, new Color(1, 1, 1, 1));
    }

    public void GoSprite1()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.025f, 0.025f, 0), 0.1f);
        PlayAudio("Sound/Down", 1, 1);

        TweenScale ts = TweenScale.Begin(LogoSprite[1].gameObject, 0.25f, new Vector3(55, 71, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoSprite2";

        TweenColor.Begin(LogoSprite[2].gameObject, 0.15f, new Color(1, 1, 1, 1));
    }

    public void GoSprite2()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.025f, 0.025f, 0), 0.1f);
        PlayAudio("Sound/Down", 0.9f, 1.25f);

        TweenScale ts = TweenScale.Begin(LogoSprite[2].gameObject, 0.25f, new Vector3(66, 71, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoSprite3";

        TweenColor.Begin(LogoSprite[3].gameObject, 0.15f, new Color(1, 1, 1, 1));
    }

    public void GoSprite3()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.025f, 0.025f, 0), 0.1f);
        PlayAudio("Sound/Down", 0.8f, 1.5f);

        TweenScale ts = TweenScale.Begin(LogoSprite[3].gameObject, 0.25f, new Vector3(65, 71, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoSprite5";

        TweenColor.Begin(LogoSprite[5].gameObject, 0.15f, new Color(1, 1, 1, 1));
    }

    public void GoSprite4()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.025f, 0.025f, 0), 0.1f);
        PlayAudio("Sound/Down", 0.7f, 1.75f);

        TweenScaleEx ts = TweenScaleEx.Begin(LogoSprite[4].gameObject, 0.25f, new Vector3(78, 77, 1) * 2.5f, new Vector3(78, 77, 1), 0.75f);
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoShake";

        TweenColor.Begin(LogoSprite[7].gameObject, 0.15f, new Color(1, 1, 1, 1));
    }

    public void GoSprite5()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.025f, 0.025f, 0), 0.1f);
        PlayAudio("Sound/Down", 1, 2.0f);

        TweenScale ts = TweenScale.Begin(LogoSprite[5].gameObject, 0.25f, new Vector3(69, 70, 1));
        ts.eventReceiver = gameObject;
        ts.callWhenFinished = "GoSprite4";
    }

    public void GoShake()
    {
        iTween.ShakePosition(MainPanel, new Vector3(0.05f, 0.05f, 0), 0.5f);
        PlayAudio("Sound/BBB", 1, 1);

        LogoSprite[6].color = new Color(1, 1, 1, 1);

        mShowing = true;
        mShowingTime = 0;

//#if UNITY_IPHONE || UNITY_ANDROID
//        Handheld.Vibrate();
//#endif
    }

    public void KillSelf()
    {
        Application.LoadLevel(NextLoadLevel);
        Destroy(gameObject);
    }
}
