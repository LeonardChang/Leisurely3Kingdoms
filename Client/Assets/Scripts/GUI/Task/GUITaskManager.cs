using UnityEngine;
using System.Collections;


public class GUITaskManager : MonoBehaviour 
{

    public UILabel Label_ScorePointPercent;

    public UILabel Label_YesterdayResult;
    public UILabel Label_Yesterday;

    public GameObject Prefab_TaskBtn;
    public Transform Task_Grid;

    int mCurrentPage = 1;
    bool isIni = false;

    public GameObject IBtn_PrePage;
    public GameObject IBtn_NextPage;
	// Use this for initialization
	void Start () 
    {
        Label_Yesterday.text = StringTable.GetString(EStringIndex.UIText_YesterdayValuation);
	}


    void UpdateUI()
    {
        Label_ScorePointPercent.text = GameDataCenter.Instance.mTodayTaskPoint.ToString() + "/100";
        Label_YesterdayResult.text = GameDataCenter.Instance.GetYestodayTaskResult();
    }

	// Update is called once per frame
	void Update () 
    {
        Init();
        UpdateUI();
	}

    void Init()
    {
        if(!isIni)
        {
            int today_point = 0;
            for (int i = 0; i < GameDataCenter.Instance.TaskData.Length; i++ )
            {
                GameObject obj = (GameObject)Instantiate(Prefab_TaskBtn);
                obj.transform.parent = Task_Grid;
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.GetComponent<GUITaskBtn>().mTaskId = i;
                if(GameDataCenter.Instance.TaskData[i].IsAccept)
                    today_point += GameDataCenter.Instance.TaskData[i].TaskInfo.AwardScorePoint;
            }
            Task_Grid.GetComponent<UIGrid>().Reposition();
            isIni = true;
        }
    }

    void OnEnable()
    {
        CheckPageBtn();
        GameDataCenter.Instance.SetTodayTaskPoint();
    }


    int MaxPage
    {
        get
        {
            return (GameDataCenter.Instance.TaskData.Length - 1) / 5 + 1;
        }
    }

    bool isDraging = false;
    /// <summary>
    /// 下一页//
    /// </summary>
    void OnNextPage()
    {
        if(!isDraging)
        {
            if(mCurrentPage < MaxPage)
            {
                mCurrentPage++;
            }
            else
            {
                mCurrentPage = 1;
            }
            ChangePage();
        }

    }

    /// <summary>
    /// 上一页//
    /// </summary>
    void OnPrePage()
    {
        if (!isDraging)
        {
            if (mCurrentPage > 1)
            {
                mCurrentPage--;
            }
            else
            {
                mCurrentPage = MaxPage;
            }

            ChangePage();
        }
    }



    /// <summary>
    /// 切换页码//
    /// </summary>
    void ChangePage()
    {
        TweenPosition.Begin(Task_Grid.gameObject, 0.3f, new Vector3(-600 * (mCurrentPage - 1), 210, 0));
        isDraging = true;
        Invoke("EnableDraging", 0.3f);
        CheckPageBtn();
    }




    void EnableDraging()
    {
        isDraging = false;
    }

    void CheckPageBtn()
    {
        if(mCurrentPage == 1)
        {
            IBtn_PrePage.gameObject.SetActiveRecursively(false);
        }
        else
        {
            IBtn_PrePage.gameObject.SetActiveRecursively(true);
        }

        if(mCurrentPage == MaxPage)
        {
            IBtn_NextPage.gameObject.SetActiveRecursively(false);
        }
        else
        {
            IBtn_NextPage.gameObject.SetActiveRecursively(true);
        }
    }
}
