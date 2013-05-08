using UnityEngine;
using System.Collections;
using EasyMotion2D;

/* 
 EasyMotion2D �����������ڡ�
 
 type 
 0-2����Զ�����
 1-���������������
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
