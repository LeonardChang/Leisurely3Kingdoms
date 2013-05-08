using UnityEngine;
using System.Collections;

public class GUILaBaLight : MonoBehaviour 
{

    static string[] LightStage = new string[] { "Laba_Light_Off", "Laba_Light_On"};
	
    UISprite mSprite;

    public int mIndex;
    /// <summary>
    /// …¡Àı
    /// </summary>
    float mFlashCount_Start = 0;
    float mFlashCount_End = 0;
    // Use this for initialization
	void Start () 
    {
        mSprite = GetComponent<UISprite>();

        int i = mIndex % 6;
        mFlashCount_Start = i * 0.1f;
        mFlashCount_End = mFlashCount_Start + 0.1f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (GUIPanelRollReward.mLightType == LightType.Light_Flash)
        {
            if(GUIPanelRollReward.mRunLightCount > GUIPanelRollReward.max_count / 2)
            {
                mIsOn = true;
            }
            else
            {
                mIsOn = false;
            }
        }
        else if (GUIPanelRollReward.mLightType == LightType.Light_Run)
        {
            if(GUIPanelRollReward.mRunLightCount > mFlashCount_Start && GUIPanelRollReward.mRunLightCount < mFlashCount_End)
            {
                mIsOn = true;
            }
            else
            {
                mIsOn = false;
            }
        }


        if (!mIsOn || GUIPanelRollReward.mLightType == LightType.Light_Off)
        {
            mSprite.spriteName = LightStage[0];
            mSprite.MakePixelPerfect();
        }
        else
        {
            mSprite.spriteName = LightStage[1];
            mSprite.MakePixelPerfect();
        }

	}

    bool mIsOn = false;

    
}
