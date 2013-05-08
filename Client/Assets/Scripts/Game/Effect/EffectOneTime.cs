using UnityEngine;
using System.Collections;
using EasyMotion2D;

/* 
 EasyMotion2D 动画生命周期。
 
 type 
 0-2秒后自动销毁
 1-动画播放完后销毁
 */

public class EffectOneTime : MonoBehaviour {

    public int type = 0;
	// Use this for initialization
	void Start () {
        if(type == 1)
        {
            Destroy(gameObject, 2);
        }
        
	}
	
	// Update is called once per frame
	void Update () 
    {
        if(type == 0)
        {
            if (!GetComponent<SpriteAnimation>().isPlaying)
            {
                Destroy(gameObject);
            }
        }

	}
}
