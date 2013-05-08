using UnityEngine;
using System.Collections;

/// <summary>
/// 主题详细信息
/// </summary>
public class GUIStageInfo : MonoBehaviour {
    /// <summary>
    /// 主题名
    /// </summary>
    public UISprite Sprite_Name;

    /// <summary>
    /// 价格
    /// </summary>
    public UILabel Label_Money;
    public UISprite Sprite_Money;

    /// <summary>
    /// 描述Label
    /// </summary>
    public UILabel Label_LineInfo;

    /// <summary>
    /// 主题示意图
    /// </summary>
    public UITexture Texture_Pic;

    /// <summary>
    /// 当前展示场景
    /// </summary>
    public int mShowScene;


	// Use this for initialization
	void Start () 
    {
	    
	}


    float mOutTime = 0;
    bool isIni = false;

    void Ini()
    {
        isIni = true;
        GameDataCenter.Instance.GuiManager.GetComponent<GUIChange2En>().TransAll(transform);
        Label_LineInfo.text = StringTable.GetString(EStringIndex.UIText_StageInfo_2);       
    }


	// Update is called once per frame
	void Update () 
    {
        if(!isIni)
        {
            Ini();
        }

        if (mOutTime > 0)
        {
            mOutTime -= Time.deltaTime;
            if(mOutTime < 0)
            {
                transform.localPosition = new Vector3(-2000, -2000, 0);
            }
        }
	}


    void OnClose()
    {
        mOutTime = 0.5f;
        OOTools.OOTweenScale(gameObject, Vector3.one, new Vector3(0.00001f, 0.00001f, 1f));
    }

    /// <summary>
    /// 打开
    /// </summary>
    void Open()
    {
        
    }

    /// <summary>
    /// 购买
    /// </summary>
    void OnBuy()
    {
        if (GameDataCenter.Instance.GuiManager.Panel_Manager.mBuyingStage == 1)
        {
            GameDataCenter.Instance.GuiManager.Panel_Manager.OnScene2Change();
            Debug.Log("Stage2");
        }
        else if (GameDataCenter.Instance.GuiManager.Panel_Manager.mBuyingStage == 2)
        {
            GameDataCenter.Instance.GuiManager.Panel_Manager.OnScene3Change();
            Debug.Log("Stage3");
        }
    }

}
