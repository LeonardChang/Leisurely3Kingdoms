using UnityEngine;
using System.Collections;

public class FlyScorePoint : MonoBehaviour 
{


    public int mValue = 0;
    float hspeed = 0;
    float vspeed = 0;
    float grivaty = 0;
    float floor = 0;
    int state = 0;
    float delay = 0;

    Vector3 start_pos;
	// Use this for initialization
	void Start () 
    {
        start_pos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () 
    {
        Init();
        UpdateMove();
	}

    bool mIsInit = false;
    void Init()
    {
        if(!mIsInit)
        {
            hspeed = Random.Range(-100f, 100);
            vspeed = 600;
            state = 0;
            grivaty = -2000f;
            floor = start_pos.y;
            mIsInit = true;
        }
    }


    void UpdateMove()
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
            Invoke("MoveToLabel", 1f);
            //MoveToLabel();
        }
    }

     /// <summary>
    /// ±»´¥Ãþµ½
    /// </summary>
    void OnTouch()
    {
        CancelInvoke();
        MoveToLabel();
    }


    void MoveToLabel()
    {
        GameObject obj = GameObject.Find("Label_Icon_ScorePoint");
        if (obj == null)
        {
            OnDie();
            return;
        }
        Vector3 target = obj.transform.position;
            
        iTween.MoveTo(gameObject, iTween.Hash("position", target,
                                                                    "time", 0.5,
                                                                    "oncomplete", "OnDie",
                                                                    "oncompletetarget", gameObject));
    }

    void OnDie()
    {
        GameDataCenter.Instance.mTodayTaskPoint += mValue;
        GameDataCenter.Instance.GuiManager.CreateFlyLabel(2, transform.parent.gameObject, transform.localPosition, mValue);
        Destroy(gameObject);
    }
}
