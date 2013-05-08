using UnityEngine;
using System.Collections;

public class ArrayDataTest : MonoBehaviour {

    ArrayData data;
	// Use this for initialization
    public TextAsset mAsset;
	void Start () {
        /*
        data = GlobalStaticData.mStringTable;//;GetArrayData(mAsset);
        string show_string = "";
        for (int i = 0; i < data.mLineCount; i++)
        { 
            for(int j = 0; j < data.mRowCount; j ++)
            {
                show_string += (data.mData[j][i] + "  ");
            }
            show_string += "\n";
        }
        GetComponent<UILabel>().text = show_string;
         */
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
