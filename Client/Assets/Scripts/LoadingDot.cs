using UnityEngine;
using System.Collections;

public class LoadingDot : MonoBehaviour {
    public float DTime = 0;
    public float DSize = 1;

    private GameObject DotObj = null;

	// Use this for initialization
	void Start () {
        ResetAll();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetAll()
    {
        DotObj = transform.GetChild(0).gameObject;
        DotObj.transform.localScale *= DSize;
        DotObj.active = false;

        CancelInvoke("StartAnim");
        Invoke("StartAnim", DTime);
    }

    private void StartAnim()
    {
        DotObj.active = true;

        TweenRotationEx tre = gameObject.GetComponent<TweenRotationEx>();
        tre.enabled = true;
        tre.Reset();
        tre.Play(true);
    }
}
