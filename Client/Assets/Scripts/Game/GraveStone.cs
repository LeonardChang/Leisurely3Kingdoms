using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

/// <summary>
/// Ä¹±®£¨Ëæ»ú²¥·Å¶¯»­£©
/// </summary>
public class GraveStone : MonoBehaviour {
    public int HoleId = -1;
    public float mAlpha = 0;

    bool isIni = false;

    public SpriteAnimation mSAnimation;
    public List<string> mAnimationName;
	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {

        if(!isIni)
        {
            isIni = true;

            mSAnimation = GetComponent<SpriteAnimation>();
            SpriteAnimationClip[] animationList = SpriteAnimationUtility.GetAnimationClips(mSAnimation);

            foreach (SpriteAnimationClip ac in animationList)
            {
                mAnimationName.Add(ac.name);
            }

            mSAnimation.Play(mAnimationName[Random.Range(0, mAnimationName.Count)]);
            transform.localScale = new Vector3(Random.value < 0.5? 1:-1, 1, 1);
        }

        if (mAlpha < 1)
        {
            mAlpha += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, mAlpha);
            //GetComponent<SpriteRenderer>().Apply();
        }
	}
}
