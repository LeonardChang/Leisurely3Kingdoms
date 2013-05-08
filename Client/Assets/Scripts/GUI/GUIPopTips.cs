using UnityEngine;
using System.Collections;

public class GUIPopTips : MonoBehaviour 
{

    //string[] TIPS_BACK = new string[] { "Main_PopTips_Up",  "Main_PopTips_Down"};

    Rect[] TIPS_BACK_RECT = new Rect[] 
    {
        new Rect(90, -12, 218, 100),
        new Rect(92 ,13 ,-218, -100),
        new Rect(0, 0, 200, 100),
        new Rect(0, 0, -200, 100),
    };

    public string mTipsString;
    public int mType;

    UILabel Label_Tips;
    UISprite Back;

    public float mLifeTime = 5;
    void DeleteSelf()
    {
        Destroy(gameObject);
    }



	// Use this for initialization
	void Start () 
    {


        Label_Tips = transform.FindChild("Label_Tips").GetComponent<UILabel>();
        Back = transform.FindChild("Back").GetComponent<UISprite>();
        

        Label_Tips.text = mTipsString;
        //Back.spriteName = TIPS_BACK[mType];
        Back.transform.localPosition = new Vector3(TIPS_BACK_RECT[mType].x, TIPS_BACK_RECT[mType].y, 0);
        Back.transform.localScale = new Vector3(TIPS_BACK_RECT[mType].width, TIPS_BACK_RECT[mType].height, 0);
	}
	



	// Update is called once per frame
	void Update () 
    {
        if (mLifeTime > 0)
        {
            mLifeTime -= Time.deltaTime;
            if(mLifeTime < 0)
            {
                DeleteSelf();
            }
        }
	}
}
