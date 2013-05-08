using UnityEngine;
using System.Collections;
using EasyMotion2D;

public class Robber : MonoBehaviour {

    float mStandTime = 20;
    bool isDie = false;
    float mAlpha = 1;
    //float
	// Use this for initialization
	void Start () 
    {
	    
	}

    /// <summary>
    /// 触摸到
    /// </summary>
    void OnTouch()
    {
        if(isDie)
        {
            return;
        }
        Kill();
        ResourcePath.PlaySound(EResourceAudio.Audio_ThiefClick);
    }

    /// <summary>
    /// 杀掉
    /// </summary>
    void Kill()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyingBlood);
        obj.transform.parent = GameObject.Find("Panel_Main").transform;
        obj.transform.localScale = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
        obj.transform.localPosition = transform.localPosition;
        Destroy(obj, 1f);


        CreateMoney(10);
        isDie = true;
        GameDataCenter.Instance.GetCurrentScene().mHasRobber = false;
    }

    /// <summary>
    /// 创建金币
    /// </summary>
    /// <param name="_value"></param>
    void CreateMoney(int _value)
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_FlyAward);
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Panel_Main.transform;
        obj.transform.position = transform.position;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<FlyAward>().AwardType = 0;
        obj.GetComponent<FlyAward>().mValue = 10;
        obj.GetComponent<FlyAward>().FlyType = 0;
    }


	// Update is called once per frame
	void Update () 
    {
        mStandTime -= Time.deltaTime;
        if (mStandTime <= 0 && !isDie)
        {
            GameDataCenter.Instance.StealOneZombie();
            isDie = true;
        }

        if(isDie)
        {
            mAlpha -= Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, mAlpha);
            if(mAlpha <= 0)
            {
                Destroy(gameObject);
            }
        }
	}
}
