using UnityEngine;
using System.Collections;

public class OnlyForAndroid : MonoBehaviour {
    void Awake()
    {
        gameObject.SetActiveRecursively(Application.platform == RuntimePlatform.Android);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
