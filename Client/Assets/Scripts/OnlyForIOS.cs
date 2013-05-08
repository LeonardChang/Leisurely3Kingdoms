using UnityEngine;
using System.Collections;

public class OnlyForIOS : MonoBehaviour {
    void Awake()
    {
        gameObject.SetActiveRecursively(Application.platform == RuntimePlatform.IPhonePlayer);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
