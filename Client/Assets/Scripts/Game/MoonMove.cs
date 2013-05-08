using UnityEngine;
using System.Collections;
using EasyMotion2D;

/// <summary>
/// ≥°æ∞1‘¬¡¡
/// </summary>
public class MoonMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Vector3 pos = transform.localPosition;
        pos.x = Random.Range(250, 500);
        transform.localPosition = pos;
	}
	
	// Update is called once per frame
	void Update () {
        //-520 520
        transform.Translate(Vector3.left * 0.002f * Time.deltaTime);
        if (transform.localPosition.x < -520)
        { 
            Vector3 pos = transform.localPosition;
            pos.x = 520;
            transform.localPosition = pos;
        }
	}
}
