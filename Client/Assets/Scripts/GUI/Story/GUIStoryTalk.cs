using UnityEngine;
using System.Collections;

public class GUIStoryTalk : MonoBehaviour {
    //string _name, string _say, int _direction, int _effect_type, float _delay)
    public int mDirection = 0;
    public string mName;
    public string mSay;
    public int mEffectType;

    int mStep = 0;
	// Use this for initialization

    Vector3 mFrom = Vector3.zero;
    Vector3 mTo = Vector3.zero;

    public UILabel Label_Name;
    public UILabel Label_Say;
    public Transform Dialog_Back;

    /// <summary>
    /// Ãÿ ‚∂‘ª∞øÚ
    /// </summary>
    public Material Story_duihuakuang2;
    public Material Story_duihuakuang3;

	void Start () 
    {
        name = "__Talk";
        if (mDirection == 0)
        {
            mFrom = new Vector3(-480, 0, 10);
            mTo = new Vector3(100, 0, -10);
        }
        else if(mDirection == 1)
        {
            mFrom = new Vector3(750, 0, -10);
            mTo = new Vector3(200, 0, -10);
        }

        transform.localPosition = mFrom;

        switch (mEffectType)
        {
            case 1:
                Dialog_Back.GetComponent<UITexture>().material = Story_duihuakuang2;
                Dialog_Back.localPosition = new Vector3(-145, 53, 1);
                break;
            case 2:
                Dialog_Back.GetComponent<UITexture>().material = Story_duihuakuang3;
                Dialog_Back.localPosition = new Vector3(-163, 112, 1);
                Dialog_Back.localScale = new Vector3(450, 400, 1);
                if (mDirection == 0)
                {
                    Dialog_Back.localPosition = new Vector3(-136, 95, 1);
                }
                else
                {
                    Dialog_Back.localPosition = new Vector3(-151, 95, 1);
                }

                iTween.ShakeScale(gameObject, iTween.Hash("amount", new Vector3(0.1f, 0.1f, 0),
                                                            "time", 1f,
                                                            "islocal", true,
                                                            "delay", 0.3f));
                break;
        }
        if(mDirection == 1)
        {
            Vector3 ls = Dialog_Back.localScale;
            ls.x = -ls.x;
            Dialog_Back.localScale = ls;
        }


        Label_Name.text = "[" + mName + "]";
        Label_Say.text = mSay;

        iTween.MoveTo(gameObject, iTween.Hash("position", mTo,
                                                            "time", 1,
                                                            "islocal", true));
        ResourcePath.ReplaySound(EResourceAudio.Audio_TalkBox, 1, 0.3f);

        mStep++;
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    void OnNextStep()
    {
        
        float yoffset = (mStep - 1) * 300 +mTo.y;


        iTween.MoveTo(gameObject, iTween.Hash("position", new Vector3(mTo.x, yoffset, 0),
                                                                    "time", 0.5,
                                                                    "islocal", true));

        if (transform.localPosition.y > 890)
        {
            Destroy(gameObject);
        }
        mStep++;
    }
}