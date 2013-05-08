using UnityEngine;
using System.Collections;

/*‘⁄Œ⁄‘∆≤•∑≈…¡µÁ*/
public class RanDomFlash : MonoBehaviour {

    float Current = 0;
    float NextRun = 1;
    public GameObject Flash_Effect;

    float Max_X;
    float Min_X;
    bool isLeft = false;
	// Use this for initialization
	void Start () {
        Max_X = transform.localPosition.x + 50;
        Min_X = transform.localPosition.x - 50;
	}
	
	// Update is called once per frame
	void Update () {
        if(isLeft)
        {
            if(transform.localPosition.x < Max_X)
            {
                transform.localPosition += new Vector3(Time.deltaTime * 10,  0, 0);
            }
            else
            {
                isLeft = false;
            }
        }
        else
        {
            if (transform.localPosition.x > Min_X)
            {
                transform.localPosition += new Vector3(-Time.deltaTime * 10, 0, 0);
            }
            else
            {
                isLeft = true;
            }
        }



        Current += Time.deltaTime;

        if (Current > NextRun)
        {
            NextRun = Random.Range(0.5f, 1f);
            Current = 0;
            int times = Random.Range(3, 6);

            for(int i = 0; i < times; i++)
            {

                GameObject obj = (GameObject)GameObject.Instantiate(Flash_Effect);
                obj.transform.parent = transform;
                obj.transform.localScale = new Vector3(480, 480, 480);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localPosition += new Vector3(Random.Range(-38f, 58f), Random.Range(-25f, -20f), 0);
                Destroy(obj, Random.Range(0.3f, 0.5f));
            }

        }
	}
}
