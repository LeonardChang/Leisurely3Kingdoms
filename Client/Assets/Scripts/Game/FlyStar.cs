using UnityEngine;
using System.Collections;


/// <summary>
/// 随机出现的小恶魔
/// </summary>
public class FlyStar : MonoBehaviour {

    int mStartSide;

    bool isFloatUp = true;
    float mMoveSpeed;
    float mFloatSpeed;

    float MAX_Y;
    float MIN_Y;

    float Start_X;
    float End_X;

    float Start_Y;
    //float End_Y;

    bool isDie = false;

    int mFlyType = 1;

    
	// Use this for initialization
	void Start () 
    {
        name = "FlyEmo";
        mFlyType = Random.Range(0, 3);
        switch(mFlyType)
        {
            case 1:
                Ini1();
                break;
            case 2:
                Ini2();
                break;
            default:
                Ini0();
                break;
        }
        transform.localPosition += new Vector3(0, 0, -480);
	}
	
	// Update is called once per frame
	void Update () 
    {
        switch(mFlyType)
        {
            case 1:
                Update1();
                break;
            case 2:
                Update2();
                break;
            default:
                Update0();
                break;
        }
        /*
        if (GameDataCenter.Instance.GuiManager.mapPanel.IsOpen)
        {
            Destroy(gameObject);
        }
         */ 
	}

    /// <summary>
    /// 触摸到
    /// </summary>
    void OnTouch()
    {
        if (isDie) return;

        GlobalModule.Instance.Click();
        collider.enabled = false;
        isDie = true;
        ResourcePath.PlaySound("KillFly");
        OOTools.OOTweenColor(gameObject, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0));
        Destroy(gameObject, 0.3f);

        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyingBlood);
        obj.transform.parent = transform.parent;
        obj.transform.localScale = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        obj.transform.localPosition = transform.localPosition;
        Destroy(obj, 1f);


        obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Panel_Main.transform;
        obj.transform.position = transform.position + new Vector3(0, 0, -5f);
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mValue = 1;
        obj.GetComponent<FlyAward>().FlyType = 0;
    }



/******************轨迹类型1********************/


    void Ini0()
    {
        mMoveSpeed = Random.Range(50, 100);
        mFloatSpeed = Random.Range(50, 100);
        mStartSide = Random.Range(0, 2);

        transform.parent = GameObject.Find("Panel_MachineBtns").transform;
        if (mStartSide == 0)
        {
            Start_X = -500;
            End_X = 500;
            transform.localScale = new Vector3(-61, 55, 1);
        }
        else
        {
            Start_X = 500;
            End_X = -500;
            transform.localScale = new Vector3(61, 55, 1);
        }

        transform.localPosition = new Vector3(Start_X, Random.Range(-100, 100), 0);
        MAX_Y = transform.localPosition.y + Random.Range(200, 300);
        MIN_Y = transform.localPosition.y - Random.Range(200, 300);
    }

    void Update0()
    {
        if (isDie)
        {
            return;
        }
        if (isFloatUp)
        {
            transform.localPosition += new Vector3(0, Time.deltaTime * mFloatSpeed, 0);

            if (transform.localPosition.y > MAX_Y)
            {
                isFloatUp = false;
            }
        }
        else
        {
            transform.localPosition -= new Vector3(0, Time.deltaTime * mFloatSpeed, 0);
            if (transform.localPosition.y < MIN_Y)
            {
                isFloatUp = true;
            }
        }

        if (Start_X > End_X)
        {
            transform.localPosition -= new Vector3(mMoveSpeed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x < End_X)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.localPosition += new Vector3(mMoveSpeed * Time.deltaTime, 0, 0);
            if (transform.localPosition.x > End_X)
            {
                Destroy(gameObject);
            }
        }
    }


    /***********轨迹类型2*************/
    int mSideX;
    int mSideY;
    void Ini1()
    {
        mSideX = Random.value < 0.5 ? 1 : -1;
        mSideY = Random.value < 0.5 ? 1 : -1;


        Start_X = mSideX == 1 ? -500 : 500;
        Start_Y = mSideY == 1 ? -500 : 500;

        End_X = -Start_X;
        //End_Y = -Start_Y;
        transform.parent = GameObject.Find("Panel_MachineBtns").transform;
        if(mSideX == 1)
        {
            transform.localScale = new Vector3(-61, 55, 1);
        }
        else
        {
            transform.localScale = new Vector3(61, 55, 1);
        }

        transform.localPosition = new Vector3(Start_X, Start_Y, 0);
    }

    void Update1()
    {


        transform.localPosition += new Vector3(Time.deltaTime * mSideX * 150, Time.deltaTime * mSideY * 150, 0);

        if(Mathf.Abs(transform.localPosition.x) > 510 )
        {
            Destroy(gameObject);
        }
    }





/**********轨迹类型3*************/
    float mAddX = 0;
    float mAddY = 0;
    float mSpeedX = 0;
    float mSpeedY = 0;
    void Ini2()
    {
        //mStartSide = Random.Range(0, 4);
        transform.parent = GameObject.Find("Panel_MachineBtns").transform;
        int mStartPoint = Random.Range(0, 4);



        switch (mStartPoint)
        {
            case 0:
                Start_X = -400;
                Start_Y = -600;
                mSpeedX = 100;
                mSpeedY = 150;
                break;
            case 1:
                Start_X = -400;
                Start_Y = 600;
                mSpeedX = 100;
                mSpeedY = -150;
                break;
            case 2:
                Start_X = 400;
                Start_Y = -600;
                mSpeedX = -100;
                mSpeedY = 150;
                break;
            case 3:
                Start_X = 400;
                Start_Y = 600;
                mSpeedX = -100;
                mSpeedY = -150;
                break;
        }
        
        if(Random.value < 0.5f)
        {
            mAddX = Mathf.Sign(Start_X) * 50;
            mSpeedX *= 2;
        }
        else
        {
            mAddY = Mathf.Sign(Start_Y) * 75;
            mSpeedY *= 2;
        }

        transform.localPosition = new Vector3(Start_X, Start_Y, 0);
        transform.localScale = new Vector3(61, 55, 1);
    }

    void Update2()
    {
        transform.localPosition += new Vector3(mSpeedX * Time.deltaTime, mSpeedY * Time.deltaTime, 0);


        mSpeedX +=  (Time.deltaTime * mAddX);
        mSpeedY += (Time.deltaTime * mAddY);

        if(mSpeedX > 0)
        {
            transform.localScale = new Vector3(-61, 55, 1);
        }
        else
        {
            transform.localScale = new Vector3(61, 55, 1);
        }

        if (Mathf.Abs(transform.localPosition.x) > 510)
        {
            Destroy(gameObject);
        }
    }
}
