using UnityEngine;
using System.Collections;

public class DebugGo : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

    void OnGUI()
    { 
        if(GUI.Button(new Rect(0 , 0 , 300 , 50) , "Go"))
        {
            PostParam pParam = new PostParam();
            pParam.AddPair("sys", "new");
            NetworkCtrl.Post(pParam, Hello);
        }
    }

    public void Hello(Response resp)
    {
        Debug.Log("Hello");
    }
}
