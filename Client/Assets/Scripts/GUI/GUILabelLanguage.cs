using UnityEngine;
using System.Collections;

/// <summary>
/// 多语言
/// </summary>
public class GUILabelLanguage : MonoBehaviour {
    public int index; 

    void OnEnable()
    {
         GetComponent<UILabel>().text = StringTable.GetString(index);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
