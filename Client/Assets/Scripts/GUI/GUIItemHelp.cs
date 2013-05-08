using UnityEngine;
using System.Collections;

/// <summary>
/// 提示条
/// </summary>
public class GUIItemHelp : MonoBehaviour {

    //public int index = 0;
    public ESceneItemDataType mType = ESceneItemDataType.IrrigationLvUp;
    public UILabel Label;

    bool isOpen = false;
    public float mDelay = 2;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (isOpen)
        {
            mDelay -= Time.deltaTime;
            if(mDelay <= 0)
            {
                UnLightTips();
                isOpen = false;
            }
        }
	}

    
    void LightTips()
    {
        OOTools.OOTweenColor(gameObject, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1));
        OOTools.OOTweenColor(Label.gameObject, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1));
    }

    void UnLightTips()
    {
        OOTools.OOTweenColor(gameObject, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0));
        OOTools.OOTweenColor(Label.gameObject, new Color(1, 1, 1, 1), new Color(1, 1, 1, 0));
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="_str">内容</param>
    public void OpenTips(string _str)
    {
        Label.text = _str;
        LightTips();
        isOpen = true;
        mDelay = 2;
    }

    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="_str">内容</param>
    /// <param name="_delay">延时</param>
    public void OpenTips(string _str, float _delay)
    {
        Label.text = _str;
        LightTips();
        isOpen = true;
        mDelay = _delay;
    }

}
