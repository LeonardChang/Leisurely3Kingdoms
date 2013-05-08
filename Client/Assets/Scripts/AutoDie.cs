using UnityEngine;
using System.Collections;

public class AutoDie : MonoBehaviour {
    public float LifeTime = 1.0f;

    void Awake()
    {
        Destroy(gameObject, LifeTime);
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
