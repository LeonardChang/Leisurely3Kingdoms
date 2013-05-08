using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUILoginManager : MonoBehaviour {

    /// <summary>
    /// ÊÇ·ñ´ò¿ª
    /// </summary>
    public bool mIsOpen = false;


    public GameObject IBtn_AwardCard;
    int mLoginDay = 10;
	// Use this for initialization

    public Transform[] Pos;
    public GameObject Login_Position;
    public Transform Login_Items;

    public bool mIsFinish = false;
    public bool mIsStart = false;

    public int[] PosIndex3 = new int[] { 1, 3, 6 };
    public int[] PosIndex4 = new int[] { 0, 2, 3, 6 };
    public int[] PosIndex5 = new int[] { 0, 1, 2, 4, 5 };

    private int mFinishCount = 0;
    public int mRollTimes = 0;

    private float mSpeed = 0.1f;
    private int mMaxRollTime = 10;

    public GameObject IBtn_Start;
    public GameObject IBtn_Back;

    int _mOpenCount = 0;
    public int mOpenCount// = 0;
    {
        get { return _mOpenCount; }
        set { _mOpenCount = value;
            if(_mOpenCount > mTotalCard)
            {
                Invoke("OnBack", 1.5f);
            }
        }
    }


    bool isIni = false;
    public int mChooseZombie = -1;
    public List<int> mChooseZombieList = new List<int>();

    public UILabel Label_Days;
    public UILabel Label_Tips;

    void CreateCard(LoginAwardType _type, int _value, int[] _pos, int _index)
    {
        GameObject obj = (GameObject)Instantiate(IBtn_AwardCard);
        obj.transform.parent = Login_Items;
        obj.transform.localScale = Vector3.one;
        obj.transform.position = Pos[_pos[_index]].position;
        obj.GetComponent<GUILoginCard>().mType = _type;
        obj.GetComponent<GUILoginCard>().mValue = _value;
    }



    void OnEnable()
    {
        IBtn_Back.SetActiveRecursively(false);
        mChooseZombie = -1;
        isIni = false;
        mIsOpen = true;
    }


    int GetZombie(bool _isHas)
    {
        int zombie_index = 1;
        if(_isHas)
        {
            //GameDataCenter.Instance.ZombieCollection

            CZombieData[] zombie_data = GameDataCenter.Instance.GetOpenZombieCollection(GameDataCenter.Instance.mCurrentScene);

            if(zombie_data.Length > 0)
            {
                int i = Random.Range(0, zombie_data.Length);
                zombie_index = (int)zombie_data[i].Type;
            }
            else
            {
                switch(GameDataCenter.Instance.mCurrentScene)
                {
                    case 1:
                        zombie_index = 16;
                        break;
                    case 2:
                        zombie_index = 31;
                        break;
                    default:
                        zombie_index = 1;
                        break;
                }
            }
        }
        else
        {
            CZombieData[] zombie_data = GameDataCenter.Instance.GetUnOpenZombieCollection(GameDataCenter.Instance.mCurrentScene);
            if(zombie_data.Length <= 0)
            {
                zombie_index = GetZombie(true);
            }
            else
            {
                zombie_index = (int)zombie_data[0].Type;
            }
        }

        return zombie_index;
    }

    void SetHardLevel(int _level)
    {
        switch(_level)
        {
            case 1:
                mSpeed = 0.4f;
                mMaxRollTime = 10;
                break;
            case 2:
                mSpeed = 0.35f;
                mMaxRollTime = 15;
                break;
            case 3:
                mSpeed = 0.25f;
                mMaxRollTime = 20;
                break;
        }
    }

    public int mTotalCard = 3;
    public void IniCard()
    {
        mRollTimes = 0;
        mOpenCount = 0;
        mChooseZombie = -1;
        mIsStart = false;
        mIsFinish = false;

        mLoginDay = GameDataCenter.Instance.ContinueDays;
        mChooseZombieList.Clear();
        Label_Tips.text = "";
       
        if(StringTable.mStringType != ELocalizationTyp.English)
        {
            Label_Days.text = string.Format(StringTable.GetString(EStringIndex.Info_loginDay), mLoginDay.ToString());
        }
        else
        {
            string[] ENTH = new string[] {"","st", "nd", "rd", "th"};
            string login_day = mLoginDay.ToString() + (mLoginDay < 4 ? ENTH[mLoginDay] : ENTH[4]);
            Label_Days.text = string.Format(StringTable.GetString(EStringIndex.Info_loginDay), login_day);
        }


        if (mLoginDay >= 10)
        {
            mLoginDay = 10;
        }

        foreach (Transform trans in Login_Items)
        {
            Destroy(trans.gameObject);
        }
        
        switch (mLoginDay)
        {
            case 1:
                CreateCard(LoginAwardType.Money, 1, PosIndex3, 0);
                CreateCard(LoginAwardType.Money, 10, PosIndex3, 1);
                CreateCard(LoginAwardType.Money, 50, PosIndex3, 2);
                mTotalCard = 3;
                SetHardLevel(1);
                break;
            case 2:
                CreateCard(LoginAwardType.Money, 1, PosIndex3, 0);
                CreateCard(LoginAwardType.Money, 50, PosIndex3, 1);
                CreateCard(LoginAwardType.Money, 150, PosIndex3, 2);
                mTotalCard = 3;
                SetHardLevel(1);
                break;
            case 3:
                CreateCard(LoginAwardType.Money, 1, PosIndex3, 0);
                CreateCard(LoginAwardType.Money, 500, PosIndex3, 1);
                CreateCard(LoginAwardType.Baosi, 2, PosIndex3, 2);
                mTotalCard = 3;
                SetHardLevel(2);
                break;
            case 4:
                CreateCard(LoginAwardType.Money, 500, PosIndex3, 0);
                CreateCard(LoginAwardType.Baosi, 1, PosIndex3, 1);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex3, 2);
                mTotalCard = 3;
                SetHardLevel(3);
                break;


            case 5:
                CreateCard(LoginAwardType.Money, 50, PosIndex4, 0);
                CreateCard(LoginAwardType.Money, 850, PosIndex4, 1);
                CreateCard(LoginAwardType.Baosi, 2, PosIndex4, 2);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex4, 3);
                mTotalCard = 4;
                SetHardLevel(2);
                break;

            case 6:
                CreateCard(LoginAwardType.Money, Random.Range(50, 500), PosIndex4, 0);
                CreateCard(LoginAwardType.Baosi, 1, PosIndex4, 1);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex4, 2);
                CreateCard(LoginAwardType.Zombie, GetZombie(false), PosIndex4, 3);
                mTotalCard = 4;
                SetHardLevel(2);
                break;
            case 7:
                CreateCard(LoginAwardType.Money, Random.Range(1, 500), PosIndex4, 0);
                CreateCard(LoginAwardType.Money, Random.Range(500, 1000), PosIndex4, 1);
                CreateCard(LoginAwardType.Baosi, 2, PosIndex4, 2);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex4, 3);
                mTotalCard = 4;
                SetHardLevel(2);
                break;
            case 8:
                CreateCard(LoginAwardType.Money, Random.Range(500, 1000), PosIndex4, 0);
                CreateCard(LoginAwardType.Baosi, 1, PosIndex4, 1);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex4, 2);
                CreateCard(LoginAwardType.Zombie, GetZombie(false), PosIndex4, 3);
                mTotalCard = 4;
                SetHardLevel(3);
                break;
            case 9:
                CreateCard(LoginAwardType.Money, Random.Range(1, 500), PosIndex4, 0);
                CreateCard(LoginAwardType.Money, Random.Range(500, 1000), PosIndex4, 1);
                CreateCard(LoginAwardType.Baosi, 2, PosIndex4, 2);
                CreateCard(LoginAwardType.Zombie, GetZombie(true), PosIndex4, 3);
                mTotalCard = 4;
                SetHardLevel(3);
                break;
            case 10:
                CreateCard(LoginAwardType.Money, Random.Range(1, 500), PosIndex5, 0);
                CreateCard(LoginAwardType.Money, Random.Range(500, 1000), PosIndex5, 1);
                CreateCard(LoginAwardType.Money, Random.Range(1000, 2000), PosIndex5, 2);
                CreateCard(LoginAwardType.Baosi, 2, PosIndex5, 3);
                CreateCard(LoginAwardType.Zombie, GetZombie(Random.value < 0.03 ? false : true), PosIndex5, 4);
                mTotalCard = 5;
                SetHardLevel(3);
                break;
        }
    }




	void Start () 
    {


        //GameDataCenter.Instance.LoginAward = false;



        //mLoginDay = 4;

	}


	// Update is called once per frame
	void Update () 
    {
        
        if (!isIni)
        {
            IniCard();
            isIni = true;
        }
        
        if (mOpenCount >= 1 && !IBtn_Back.gameObject.active)
        {
            IBtn_Back.SetActiveRecursively(true);
        }

        //if(mOpenCount > )
	}

    void OnStart()
    {


        foreach (Transform trans in Login_Items)
        {
            trans.SendMessage("RollBack");
        }

        IBtn_Start.SetActiveRecursively(false);
        //IBtn_Back.SetActiveRecursively(true);
    }

    void RollCard()
    {
        if(!mIsStart)
        {
            mIsStart = true;
            NextRoll();
        }
    }

    public void FinishRoll()
    {
        mFinishCount++;
        if(mFinishCount >= 2)
        {
            mFinishCount = 0;
            NextRoll();
        }
    }

    public void SetTips(string _tips)
    {
        Label_Tips.text = _tips;//StringTable.GetString(EStringIndex.Info_LoginTips1);
    }


    void NextRoll()
    {
        mRollTimes++;
        if (mRollTimes >= mMaxRollTime)
        {
            foreach (Transform trans in Login_Items)
            {
                trans.SendMessage("StartChoose");
                SetTips(StringTable.GetString(EStringIndex.Info_LoginTips1));
                //GameDataCenter.Instance.GuiManager.ItemHelp.OpenTips(StringTable.GetString(EStringIndex.Info_LoginTips1), 600);
            }
            return;
        }
        ResourcePath.PlaySound("CardMove");
        List<Transform> btns = new List<Transform>();
        foreach(Transform trans in Login_Items)
        {
            btns.Add(trans);
        }

        int count = btns.Count;
        int tmp = Random.Range(0, count);
        Transform first = btns[tmp];
        btns.RemoveAt(tmp);

        count = btns.Count;
        tmp = Random.Range(0, count);
        Transform second = btns[tmp];

        TweenPosition tp = first.GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.duration = mSpeed;
        tp.from = first.transform.localPosition;
        tp.to = second.transform.localPosition;
        tp.eventReceiver = gameObject;
        tp.callWhenFinished = "FinishRoll";
        tp.Reset();
        tp.Play(true);

        tp = second.GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.duration =  mSpeed;
        tp.from = second.transform.localPosition;
        tp.to = first.transform.localPosition;
        tp.eventReceiver = gameObject;
        tp.callWhenFinished = "FinishRoll";
        tp.Reset();
        tp.Play(true);
    }





    void OnBack()
    {
        foreach (Transform trans in Login_Items)
        {
            trans.SendMessage("DeLightZombie");
        }
        GameDataCenter.Instance.GuiManager.ItemHelp.mDelay = 0.1f;


        TweenPosition tp = GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.to = new Vector3(0, 1000, -500);
        tp.from = new Vector3(0, 0, -500);
        tp.Reset();
        tp.Play(true);
        tp.eventReceiver = gameObject;
        tp.callWhenFinished = "OnClose";
    }

    public void AddZombie(int _index)
    {
        mChooseZombieList.Add(_index);
    }

    public void OnClose()
    {
        if(mChooseZombieList.Count > 0)
        {
            foreach (int i in mChooseZombieList)
            {
                GameDataCenter.Instance.GuiManager.Zombie_Manager.CreateOnePotZombie((ZombieType)i);
            }
        }

        GameAward.CheckAward();
        gameObject.SetActiveRecursively(false);
        mIsOpen = false;
    }
}
