using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

/// <summary>
/// 游戏场景僵尸管理
/// </summary>
public class ZombieManager : MonoBehaviour {

	// Use this for initialization
    public GameObject ZombiePrefab;

    public float TickTime = 0;

    public List<CHole> m_dustHoles;

    public UILabel irrigationLevel;
    public UILabel irrigationTime;
    public UILabel infos;

    public GameObject GraveStone;
    public GameObject Farmer;
    public bool isFirstCollect = true;
    public Transform Tombs;
    int mStarCount = 0;

	void Start () 
    {
        RunStart();
        InvokeRepeating("RunUpdate", 1, 1);

	}



    int[] Hole_Id ;
    void RunStart()
    {
        Hole_Id = GameDataCenter.Instance.GetCurrentScene().HoleList;
        m_dustHoles = new List<CHole>();
        int iCount = 0;
        for (int i = 0; i < 10; i++)
        {
            m_dustHoles.Add(new CHole(-290f + i * 60f, -60, 110, iCount));
            iCount++;
            m_dustHoles.Add(new CHole(-290f + i * 60f, -170, 90, iCount));
            iCount++;
            m_dustHoles.Add(new CHole(-250f + i * 60f, -114, 100, iCount));
            iCount++;
        }

        for (int i = 0; i < 30; i++ )
        {
            m_dustHoles[i].Id = Hole_Id[i];
        }

            print(GameDataCenter.DataFilePath);

        while (!IniSceneZombie())
        {
            IniSceneZombie();
        }
    }

    /// <summary>
    /// 切换场景更换僵尸
    /// </summary>
    void ChangeSceneZombie()
    {
        GameDataCenter.Instance.IniScene();

        CZombie[] zombies = GameDataCenter.Instance.GetCurrentSceneZombie();
        foreach (CZombie zombie in zombies)
        {
            CreateOneZombie(zombie);
        }
        GameDataCenter.Instance.SetZombieOld(GameDataCenter.Instance.mCurrentScene);

        CreateHoleGrave();

        IniRobber();     
    }

    /// <summary>
    /// 初始化场景僵尸
    /// </summary>
    /// <returns></returns>
    bool  IniSceneZombie()
    {

        GameDataCenter.Instance.IniScene();

        CZombie[] zombies = GameDataCenter.Instance.GetCurrentSceneZombie();
        foreach (CZombie zombie in zombies)
        {
            CreateOneZombie(zombie);
        }
        GameDataCenter.Instance.SetZombieOld(GameDataCenter.Instance.mCurrentScene);


        CreateHoleGrave();
        CreatePotZombie();

        IniRobber();
        return true;
    }

    /// <summary>
    /// 初始化盗贼
    /// </summary>
    void IniRobber()
    {
        if (GameDataCenter.Instance.GetCurrentScene().mHasRobber || (!GameDataCenter.Instance.GetCurrentScene().mHasRobber && Random.value < 0.5f))
        {
            if(GameDataCenter.Instance.GetCurrentScene().SceneItemFlash.GetPercent() <= 0)
            {
                System.TimeSpan span = System.DateTime.Now - GameDataCenter.Instance.GetCurrentScene().mLastRobber;
                if (span.TotalSeconds < 3600)
                {
                    return;
                }
                CreateOneRobber();
            }
        }
    }

    /// <summary>
    /// 创建一个盗贼
    /// </summary>
    void CreateOneRobber()
    {
        if(GameDataCenter.Instance.IsTeachMode)
        {
            return;
        }
        GameDataCenter.Instance.GetCurrentScene().mHasRobber = true;
        GameDataCenter.Instance.GetCurrentScene().mLastRobber = System.DateTime.Now;
        GameObject obj = (GameObject)Instantiate(Farmer);
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(0, -206, 0);
        obj.name = "Robber";
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    public void ShowSceneZombie()
    {
        foreach (Transform zombie in transform)
        {
            Destroy(zombie.gameObject);
        }
        ChangeSceneZombie();
    }

    /// <summary>
    /// 创建一个僵尸
    /// </summary>
    /// <param name="_zombie"></param>
    public void CreateOneZombie(CZombie _zombie)
    {
        //DeleteOneGrave(_zombie.HoleId);
        CHole hole = GetHole(_zombie.HoleId);
        GameObject obj = (GameObject)Instantiate(ZombiePrefab);
        obj.transform.parent = transform;
        obj.transform.localPosition = new Vector3(hole.X, hole.Y, hole.Depth);
        obj.GetComponent<SpriteRenderer>().depth = hole.Depth;
        obj.GetComponent<SpriteRenderer>().Apply();
        obj.GetComponent<Zombie>().mZombie = _zombie;
        obj.transform.localScale = new Vector3(1f,1f,1f);
    }


    /// <summary>
    /// 创建随机花盆僵尸
    /// </summary>
    public void CreateOneRandomZombie()
    {
        CreateOnePotZombie(GameDataCenter.Instance.GetCurrentRandomZombie());
    }


    public void CreateOnePotZombie(ZombieType _type)
    {
        CZombie _zombie = new CZombie();
        _zombie.Type = _type;

        CHole hole = GetHole(6);
        GameObject obj = (GameObject)Instantiate(ZombiePrefab);
        obj.transform.parent = transform;
        float rnd_x = Random.Range(-250, 250);
        int rnd_yt = Random.Range(0, 2);
        float rnd_y = 0;

        obj.transform.localPosition = new Vector3(rnd_x, hole.Y + 1000, hole.Depth);
        obj.GetComponent<SpriteRenderer>().depth = hole.Depth;
        obj.GetComponent<SpriteRenderer>().Apply();
        obj.GetComponent<Zombie>().mZombie = _zombie;
        obj.GetComponent<Zombie>().mIsSpecial = true;
        obj.GetComponent<Zombie>().mRndTargetType = rnd_yt;
        obj.transform.localScale = new Vector3(1f, 1f, 1f);

        if (rnd_yt == 1)
        {
            rnd_y = -200;
        }
        GameDataCenter.Instance.GetCurrentScene().AddPotZombie(new CPotZombie(System.Guid.NewGuid().ToString(), _zombie.Type, new Vector3(rnd_x, rnd_y, 0), rnd_yt));
    }

    public void CreatePotZombie()
    {
        foreach (CPotZombie zombie in GameDataCenter.Instance.GetCurrentScene().mPotZombie)
        {
            CZombie _zombie = new CZombie();
            _zombie.Type = zombie.mType;
            GameObject obj = (GameObject)Instantiate(ZombiePrefab);
            obj.transform.parent = transform;

            obj.transform.localPosition = zombie.mPos;
            obj.transform.localScale = Vector3.one;
            //obj.GetComponent<SpriteRenderer>().depth = hole.Depth;
            //obj.GetComponent<SpriteRenderer>().Apply();
            obj.GetComponent<Zombie>().mZombie = _zombie;
            obj.GetComponent<Zombie>().mIsSpecial = true;
            obj.GetComponent<Zombie>().mSpecialType = 1;
            obj.GetComponent<Zombie>().mRndTargetType = zombie.mTargetType;
            obj.GetComponent<Zombie>().mUID = zombie.mUID;
        }
    }


    /// <summary>
    /// 创建一个墓碑
    /// </summary>
    /// <param name="_hole"></param>
    void CreateOneGrave(int _hole)
    {
        CHole hole = GetHole(_hole);
        GameObject obj = (GameObject)Instantiate(GraveStone);
        obj.transform.parent = Tombs;
        obj.transform.localPosition = new Vector3(hole.X, hole.Y, hole.Depth);
        obj.GetComponent<SpriteRenderer>().depth = hole.Depth+1;
        obj.GetComponent<GraveStone>().HoleId = _hole;
        obj.name = "GraveStone";
        obj.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    /// <summary>
    /// 删除所有墓碑
    /// </summary>
    public void DeleteAllGrave()
    {
        foreach (Transform trans in Tombs)
        {
            Destroy(trans.gameObject);
        }
    }

    /// <summary>
    /// 创建墓碑
    /// </summary>
    void CreateHoleGrave()
    {
        int[] list = GameDataCenter.Instance.GetCurrentScene().GetUnOpenHoles();

        foreach (int i in list)
        {
            CreateOneGrave(i);
        }
    }

    /// <summary>
    /// 删除一个墓碑
    /// </summary>
    /// <param name="_hole"></param>
    public void DeleteOneGrave(int _hole)
    {
        foreach (Transform trans in Tombs)
        {
            if(trans.name == "GraveStone")
            {
                if(trans.GetComponent<GraveStone>().HoleId == _hole)
                {
                    GameDataCenter.Instance.GetCurrentScene().DeleteGrave(_hole);
                    Destroy(trans.gameObject);

                    CHole hole = GetHole(_hole);
                    GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Keng);
                    obj.transform.parent = transform;
                    obj.transform.localScale = new Vector3(480, 480, 480);
                    obj.transform.localPosition = new Vector3(hole.X, hole.Y, 0);

                }
            }
        }
    }

    
    private UIScrollBar mBarSpeed = null;
    private UIScrollBar BarSpeed
    {
        get
        {
            if (mBarSpeed == null)
            {
                mBarSpeed = GameObject.Find("Bar(Speed)").GetComponent<UIScrollBar>();
            }
            return mBarSpeed;
        }
    }


    int mStartDelay = 30;
    void RunUpdate()
    {
        mStarCount++;
        if(mStarCount >= 5)
        {
            GameDataCenter.Instance.GuiManager.EffectStar();
            mStarCount = 0;
        }

        mStartDelay--;
        if(mStartDelay <= 0)
        {
            if (!GameDataCenter.Instance.IsTeachMode && null == GameObject.Find("WinEffect"))
                ResourcePath.Instance(EResourceIndex.Prefab_Fly_Star);

            mStartDelay = Random.Range(20, 30);
        }

        GameDataCenter.Instance.GuiManager.EffectNew();
        GameDataCenter.Instance.GuiManager.EffectTaskDone();
        GameDataCenter.Instance.GuiManager.EffectNewIsland();
        GameDataCenter.Instance.UpdateScene();

        CZombie[] zombieList = GameDataCenter.Instance.mScenes[GameDataCenter.Instance.mCurrentScene].GetNewZombie();
        if (zombieList.Length > 0)
        {
            foreach (CZombie zombie in zombieList)
            {
                CreateOneZombie(zombie);
            }
            GameDataCenter.Instance.SetZombieOld(GameDataCenter.Instance.mCurrentScene);
            GameDataCenter.Instance.Save();
        }
        GameDataCenter.Instance.GuiManager.Panel_Manager.CheckPopLogin();
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouchZombie();




        if(GameDataCenter.Instance.mHappyTime >= 0)
        {
            GameDataCenter.Instance.mHappyTime -= Time.deltaTime;
            GameDataCenter.Instance.GetCurrentScene().TestSpeed = 5;
        }
        else
        {
            GameDataCenter.Instance.GetCurrentScene().TestSpeed = (int)(BarSpeed.scrollValue * 10);
        }
	}



    void CheckTouchZombie2()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            if(Input.touches.Length > 0)
            {
                Camera gameCamera = GameDataCenter.Instance.GuiManager.Camera_2D;
                RaycastHit hit;
                if (!mIsTouch)
                {
                    mLastMouse = Input.touches[0].position;
                }

                for (float i = 0; i < 1; i += 0.1f)
                {
                    Ray ray = gameCamera.camera.ScreenPointToRay(Vector3.Lerp(mLastMouse, Input.touches[0].position, i));//Input.mousePosition);
                    //if raycast any collider or character, then move to the target object
                    if (Physics.Raycast(ray, out hit, 10000f))
                    {
                        if (hit.collider.GetComponent<FlyCoin>() || hit.collider.GetComponent<FlyAward>()
                            || hit.collider.GetComponent<FlyExperience>() || hit.collider.GetComponent<FlyStar>() ||
                            hit.collider.GetComponent<Robber>() || hit.collider.GetComponent<FlyScorePoint>())
                        {
                            hit.collider.SendMessage("OnTouch");
                        }

                        if (hit.collider.GetComponent<Zombie>())
                        {
                            hit.collider.GetComponent<Zombie>().CheckTouch();
                        }
                    }
                }


                mIsTouch = true;
                mLastMouse = Input.mousePosition;
            }
            else
            {
                mIsTouch = false;
            }
        }
        else
        {
            CheckTouchZombie();
        }
    }

    Vector3 mLastMouse;
    bool mIsTouch = false;
    /// <summary>
    /// 检测触摸
    /// </summary>
    void CheckTouchZombie()
    {
       
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0))
        {
            
            Camera gameCamera = GameDataCenter.Instance.GuiManager.Camera_2D;
            RaycastHit hit;
            if(!mIsTouch)
            {
                mLastMouse = Input.mousePosition;
            }

            for(float i = 0; i < 1; i += 0.1f)
            {
                Ray ray = gameCamera.camera.ScreenPointToRay(Vector3.Lerp(mLastMouse, Input.mousePosition, i));//Input.mousePosition);
                //if raycast any collider or character, then move to the target object
                if (Physics.Raycast(ray, out hit, 10000f))
                {
                    if (hit.collider.GetComponent<FlyCoin>() || hit.collider.GetComponent<FlyAward>()
                        || hit.collider.GetComponent<FlyExperience>() || hit.collider.GetComponent<FlyStar>() ||
                        hit.collider.GetComponent<Robber>() || hit.collider.GetComponent<FlyScorePoint>())
                    {
                        hit.collider.SendMessage("OnTouch");
                    }

                    if(hit.collider.GetComponent<Zombie>())
                    {
                        hit.collider.GetComponent<Zombie>().CheckTouch();
                    }
                }
            }

            mIsTouch = true;
            mLastMouse = Input.mousePosition;
        }
        else
        {
            mIsTouch = false;
        }
    }




    /// <summary>
    /// 获取一个坑
    /// </summary>
    /// <param name="_id"></param>
    /// <returns></returns>
    CHole GetHole(int _id)
    {
        for (int i = 0; i < m_dustHoles.Count; i++)
        {
            if (m_dustHoles[i].Id == _id)
            {
                return m_dustHoles[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 退出
    /// </summary>
    public void OnApplicationQuit()
    {
        GameDataCenter.Instance.ForceSave();
        print("quit");
    }
}
