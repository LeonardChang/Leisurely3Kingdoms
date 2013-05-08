using UnityEngine;
using System.Collections;

public class GUIUIArtText : MonoBehaviour {
    public int mTextID = -1;


    public bool IsResizeJapanese = false;
    public Rect RectJapanese; 




	// Use this for initialization
	void Start ()
    {
        if (GetComponent<UILabel>().font.name != "DFPHAIBAOW32")
            GetComponent<UILabel>().font = ResourcePath.GetOtherArtFont();

        if(IsResizeJapanese && StringTable.mStringType == ELocalizationTyp.Japanese)
        {
            transform.localScale = new Vector3(RectJapanese.width, RectJapanese.height, 1);
            if(RectJapanese.x != 0)
            {
                Vector3 pos = transform.localPosition;
                pos.x = RectJapanese.x;
                pos.y = RectJapanese.y;
                transform.localPosition = pos;
            }
        }

        GetComponent<UILabel>().text = StringTableUI.GetString(mTextID);
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
