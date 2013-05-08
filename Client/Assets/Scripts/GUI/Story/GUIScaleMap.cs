using UnityEngine;
using System.Collections;

public class GUIScaleMap : MonoBehaviour {
    bool mIsScale = false;
    Vector3 mPosFrom = new Vector3(0, 60, 0);
    Vector3 mPosTo;
    Vector3 mScaleFrom = new Vector3(1, 1, 1);
    Vector3 mScaleTo = new Vector3(1.5f, 1.5f, 1f);

    float mTime = 0;
	// Use this for initialization
	void Start () 
    {
	    
	}
	
    void OnEnable()
    {
        transform.localPosition = new Vector3(0, 60, 0);
        transform.localScale = new Vector3(1, 1, 1);
        mIsScale = false;
    }

    void OnDisable()
    {
        mIsScale = false;
    }

	// Update is called once per frame
	void Update () 
    {
        if (mIsScale)
        {
            mTime += Time.deltaTime;
            if(mTime >= 1)
            {
                mTime = 1;
                mIsScale = false;
            }
            transform.localPosition = new Vector3(
                Mathf.Lerp(mPosFrom.x, mPosTo.x, mTime),
                Mathf.Lerp(mPosFrom.y, mPosTo.y, mTime),
                Mathf.Lerp(mPosFrom.z, mPosTo.z, mTime)
                );

            transform.localScale = new Vector3(
                Mathf.Lerp(mScaleFrom.x, mScaleTo.x, mTime),
                Mathf.Lerp(mScaleFrom.y, mScaleTo.y, mTime),
                Mathf.Lerp(mScaleFrom.z, mScaleTo.z, mTime)
                );

        }
	}

    public void StartScaleMap(Vector3 _pos_to)
    {
        mIsScale = true;
        mPosTo = _pos_to;
        mTime = 0;
    }

    public void EndScaleMap()
    {
        mIsScale = false;
    }
}
