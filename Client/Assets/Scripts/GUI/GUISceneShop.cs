using UnityEngine;
using System.Collections;

public class GUISceneShop : MonoBehaviour {

    bool isIni = false;
    public Transform Spr_Shop_Using;
    public Transform Panel_Btns;



    public UIScrollBar ScrollBar;
    public UISlider SliderBar;


    public UILabel Label_Unlock_2;
    public UILabel Label_Unlock_1;


    
	// Use this for initialization
	void Start () 
    {

	}
	
    void OnEnable()
    {
        isIni = false;
        

        if(!GlobalStaticData.GetStageInfo(1).CanOpen &&  !GameDataCenter.Instance.mScenes[1].IsOpen)//未到等级
        {
            Label_Unlock_1.text = string.Format(StringTable.GetString(EStringIndex.UIText_StageUnlock), GlobalStaticData.GetStageInfo(1).mOpenLevel);
        }
        else
        {
            Label_Unlock_1.alpha = 0;
            Label_Unlock_1.transform.parent.FindChild("Sprite_Link").GetComponent<UISprite>().alpha = 0;
            Label_Unlock_1.transform.parent.FindChild("Sprite_LinkLock").GetComponent<UISprite>().alpha = 0;

        }

        if (!GlobalStaticData.GetStageInfo(2).CanOpen && !GameDataCenter.Instance.mScenes[2].IsOpen)//未到等级
        {
            Label_Unlock_2.text = string.Format(StringTable.GetString(EStringIndex.UIText_StageUnlock), GlobalStaticData.GetStageInfo(2).mOpenLevel);
        }
        else
        {
            Label_Unlock_2.alpha = 0;
            Label_Unlock_2.transform.parent.FindChild("Sprite_Link").GetComponent<UISprite>().alpha = 0;
            Label_Unlock_2.transform.parent.FindChild("Sprite_LinkLock").GetComponent<UISprite>().alpha = 0;
        }


        /*
        if( GameDataCenter.Instance.mScenes[1].IsOpen)
        {
            Label_Unlock_1.transform.parent.FindChild("Sprite_Link").GetComponent<UISprite>().alpha = 0;
            Label_Unlock_1.transform.parent.FindChild("Sprite_LinkLock").GetComponent<UISprite>().alpha = 0;
        }

        if( GameDataCenter.Instance.mScenes[2].IsOpen)
        {
            Label_Unlock_2.transform.parent.FindChild("Sprite_Link").GetComponent<UISprite>().alpha = 0;
            Label_Unlock_2.transform.parent.FindChild("Sprite_LinkLock").GetComponent<UISprite>().alpha = 0;
        }
         */ 

    }


    public void OpenStage2()
    {
            //Panel_Btns.FindChild("BtnScene2").FindChild("Label").GetComponent<UILabel>().text = "";
            //Panel_Btns.FindChild("BtnScene2").FindChild("Sprite_Money").GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
    }

    public void ChangeUsing()
    {
            Spr_Shop_Using.localPosition = Panel_Btns.FindChild("BtnScene" + (GameDataCenter.Instance.CurrentScene + 1).ToString()).localPosition + new Vector3(-120, 60, -10);
    }


	// Update is called once per frame
	void Update () 
    {
        SliderBar.sliderValue = ScrollBar.scrollValue;

        if (!isIni)
        {
            isIni = true;
            ChangeUsing();
            if(GameDataCenter.Instance.mScenes[1].IsOpen)
            {
                //OpenStage2();
            }

        }


        
	}
}
