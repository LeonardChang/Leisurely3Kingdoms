using UnityEngine;
using System.Collections;


/********UI∑≠“Î≥…”¢Œƒ********/
public class GUIChange2En : MonoBehaviour 
{

    //public UIAtlas UIFontEn;
    UIAtlas UIFontCn;
    //public UIAtlas UIFontJp;

    UIAtlas UIFont_Local;
	// Use this for initialization
	void Start () 
    {
        UIFontCn = (GlobalModule.Instance.LoadResource("GUI/UIFont/UIFont") as GameObject).GetComponent<UIAtlas>();
        UIFont_Local = ResourcePath.GetOtherFont();

        

        TransAll(transform);
	    //IBtn_Collection.
	}


    public void TransAll(Transform _transform)
    {
        if (StringTable.mStringType == ELocalizationTyp.Chinese)
        {
            return;
        }
        if(_transform.GetComponent<UISprite>())
        {
            if (_transform.GetComponent<UISprite>().atlas == UIFontCn)
            {
                _transform.GetComponent<UISprite>().atlas = UIFont_Local;
                _transform.GetComponent<UISprite>().MakePixelPerfect();
            }
        }

        foreach (Transform trans in _transform)
        {
            TransAll(trans);
        }
    }


    void SetFontEn(UISprite _font)
    {
        _font.atlas = UIFont_Local;
        _font.MakePixelPerfect();
    }

	// Update is called once per frame
	void Update () {
	
	}
}
