using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 陈列室面板管理
/// 打开的时候找到有new的一页，否则打开当前主题的第一只僵尸所在的那一页。
/// 
/// </summary>
public class GUICollectionAllManager : MonoBehaviour {
    static int SHOW_COUNT = 45;


    CZombieData[] mZombieDataList;// = new List<CZombieData>();

    /// <summary>
    /// 卡片预设
    /// </summary>
    public GameObject IBtn_ZombieInfo;

    /// <summary>
    /// 卡片列表
    /// </summary>
    public Transform Info_list;

    /// <summary>
    /// 上一页
    /// </summary>
    public TweenPosition Btn_PrePage;

    /// <summary>
    /// 下一页
    /// </summary>
    public TweenPosition Btn_NextPage;


    public TweenPosition tweenPosition;

    /// <summary>
    /// 是否打开
    /// </summary>
    public bool isOpen = false;


    public bool isDie = false;

    bool isIni = false;

    /// <summary>
    /// 僵尸卡片位置
    /// </summary>
    Vector3[] mZombiePos = new Vector3[] {new Vector3(-100f, 195f, 0),
                                                                new Vector3(120, 195, 0),
                                                                new Vector3(-100, -45, 0),
                                                                new Vector3(120, -45, 0)};

    /// <summary>
    /// 当前页码
    /// </summary>
    public int mCurrentPage = 1;

    /// <summary>
    /// 最大页码
    /// </summary>
    public int mMaxPage = 1;

    /// <summary>
    /// 页码Label
    /// </summary>
    public UILabel Label_Page;

    /// <summary>
    /// 收集度Label
    /// </summary>
    public UILabel Label_CollectionP;
	// Use this for initialization

	void Start () 
    {

	}

    /// <summary>
    /// 英文版坐标偏移
    /// </summary>
    void IniEnglish()
    {
        if(StringTable.mStringType == ELocalizationTyp.English)
        {
            Label_CollectionP.transform.localPosition = new Vector3(42, -238, -10);
        }
    }

    /// <summary>
    /// 创建僵尸卡片
    /// </summary>
    /// <param name="_zombie"></param>
    /// <param name="_pos"></param>
    void CreateZombieBtn(CZombieData _zombie, int _pos)
    {
        GameObject obj = (GameObject)Instantiate(IBtn_ZombieInfo);
        obj.GetComponent<GUIZombieInfo>().mZombieData = _zombie;
        obj.transform.parent = Info_list;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
        obj.transform.localPosition = mZombiePos[_pos];
    }

    void OnEnable()
    {
        isIni = false;
    }

    void OnDisable()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    int[] SCENE_START = new int[]{0, 15, 30};
	// Update is called once per frame
	void Update () 
    {
        Label_CollectionP.text = GameDataCenter.Instance.GetCollectPercent().ToString() + "%";


        if (!isIni)
        {
            isIni = true;
            IniEnglish();
            Btn_NextPage.Reset();
            Btn_NextPage.Play(true);
            Btn_PrePage.Reset();
            Btn_PrePage.Play(true);

            foreach (Transform trans in Info_list)
            {
                Destroy(trans.gameObject);
            }
            
            mZombieDataList = GameDataCenter.Instance.ZombieCollection;
            Debug.Log("-------------------------------" + mZombieDataList.Length.ToString());
            mMaxPage = Mathf.CeilToInt(SHOW_COUNT / 4.0f);// + 1;
            

            int start_zombie = SCENE_START[GameDataCenter.Instance.CurrentScene];
            for (int i = 0; i < SHOW_COUNT; i++)
            {
                if(mZombieDataList[i].IsNew)
                {
                    start_zombie = i;
                    break;
                }
            }

            mCurrentPage = start_zombie / 4 + 1;
            CreatePageZombie();
            tweenPosition = GetComponent<TweenPosition>();
        }
        CheckHand();
	}

    /// <summary>
    /// 创建当前页的僵尸
    /// </summary>
    void CreatePageZombie()
    {
        Label_Page.text = mCurrentPage.ToString() + "/" + mMaxPage.ToString();

        int start = (mCurrentPage - 1) * 4;
        int end = mZombieDataList.Length < mCurrentPage * 4 ? mZombieDataList.Length : mCurrentPage * 4;

        for (int i = start; i < end; i++)
        {
            CreateZombieBtn(mZombieDataList[i], i - start);
        }
    }

    /// <summary>
    /// 上一页
    /// </summary>
    void OnBtnLeft()
    {

        foreach (Transform trans in Info_list)
        {
            Destroy(trans.gameObject);
        }
        
        mCurrentPage -= 1;
        if(mCurrentPage <= 0)
        {
            mCurrentPage = mMaxPage;
        }
        CreatePageZombie();
    }

    /// <summary>
    /// 下一页
    /// </summary>
    void OnBtnRight()
    {
        foreach (Transform trans in Info_list)
        {
            Destroy(trans.gameObject);
        }
        
        mCurrentPage += 1;
        if (mCurrentPage > mMaxPage)
        {
            mCurrentPage = 1;
        }
        CreatePageZombie();
    }



    void OnAnimationEnd()
    {
        if(!isOpen)
        {
            tweenPosition.gameObject.SetActiveRecursively(false);
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    int OnCheckHandDir()
    {
        //ret 0-静止 1-向左  2-向右
        int ret = 0;
        if (Application.platform == RuntimePlatform.IPhonePlayer
                 || Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            { 
                if(Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                    if (touchDeltaPosition.x > Screen.width / 12)
                    {
                        ret = 1;
                    }
                    else if (touchDeltaPosition.x < -Screen.height / 12)
                    {
                        ret = 2;
                    }
                }
                else if(Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    SetCanPage();
                }

            }
        }
        else
        {
            //Debug.Log(Input.GetAxis("Mouse Y"));
            if (Input.GetMouseButton(0))
            {
                if (Input.GetAxis("Mouse X") >= 0.5f)
                {
                    ret = 1;
                }
                else if (Input.GetAxis("Mouse X") <= -0.5f)
                {
                    ret = 2;
                }
            }
            if(Input.GetMouseButtonDown(0))
            {
                SetCanPage();
            }
        }
        return ret;
    }


    void SetCanPage()
    {
        isCanPage = true;
    }

    bool isCanPage = true;
    void CheckHand()
    {
        if (GameObject.FindGameObjectsWithTag("PanelCollection").Length > 0)
            return;

        int direction = OnCheckHandDir();
        if (!isCanPage) return;
        switch (direction)
        {
            case 1:
                isCanPage = false;
                OnBtnLeft();
                break;
            case 2:
                isCanPage = false;
                OnBtnRight();
                
                break;
        }
    }
}
