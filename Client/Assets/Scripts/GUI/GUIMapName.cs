using UnityEngine;
using System.Collections;

public class GUIMapName : MonoBehaviour {

    bool isOpen = false;

    bool isIni = false;

    float delay = 0;

    string[] Str_Name;
    string[] Spr_Name;

    public UILabel Label_MapName;
    public UILabel Label_MapName_Top;

	// Use this for initialization
	void Start () 
    {
        Str_Name = new string[] { StringTable.GetEnglish(EStringIndex.StageName_1), StringTable.GetEnglish(EStringIndex.StageName_2), StringTable.GetEnglish(EStringIndex.StageName_3) };
        Spr_Name = new string[] { StringTableUI.GetString(19), StringTableUI.GetString(20), StringTableUI.GetString(21), };	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    if(!isIni)
        {
            OpenPanel();
            //Debug.Log(GameDataCenter.Instance.CurrentScene);
            //transform.FindChild("Sprite_MapName").GetComponent<UI>().spriteName = Spr_Name[GameDataCenter.Instance.CurrentScene];
            //transform.FindChild("Sprite_MapName").GetComponent<UISprite>().spriteName = Spr_Name[GameDataCenter.Instance.CurrentScene];
            //transform.FindChild("Sprite_MapName").GetComponent<UISprite>().MakePixelPerfect();
            Label_MapName_Top.text = Spr_Name[GameDataCenter.Instance.CurrentScene];
            
            string str = Str_Name[GameDataCenter.Instance.CurrentScene];
            if(StringTable.mStringType == ELocalizationTyp.English)
            {
                str = "";
            }

            
            Label_MapName.text = str;
            isIni = true;
        }

        if (isOpen && delay > 0)
        {
            delay -= Time.deltaTime;
            if(delay < 0)
            {
                ClosePanel();
            }
        }
	}


    void OnEnable()
    {
        isIni = false;
    }

    public void OpenPanel()
    {
        if(!isOpen)
        {
            OOTools.OOTweenPosition(gameObject, new Vector3(550f, -155f, -2000f), new Vector3(250, -155, -2000f));
            isOpen = true;
            delay = 2;
        }
    }

    public void ClosePanel()
    {
        if(isOpen)
        {
            OOTools.OOTweenPosition(gameObject, new Vector3(250f, -155f, -2000f), new Vector3(550, -155, -2000f));
            isOpen = false;
        }
    }

    public void EndMotion()
    {
        if(!isOpen)
        {
            gameObject.SetActiveRecursively(false);
        }
    }
}
