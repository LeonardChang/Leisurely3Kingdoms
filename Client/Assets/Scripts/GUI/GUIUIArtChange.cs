using UnityEngine;
using System.Collections;

public class GUIUIArtChange : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
        if (GetComponent<UILabel>().font.name != "DFPHAIBAOW32")
            GetComponent<UILabel>().font = ResourcePath.GetOtherArtFont();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
