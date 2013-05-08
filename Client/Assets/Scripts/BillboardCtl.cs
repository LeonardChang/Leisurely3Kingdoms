using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BillboardCtl : MonoBehaviour {
    public KOZNet NetComponent;
    public UITexture ImageUI;
    public float UpdateTime = 10;

    private float mTime = 0;
    private Dictionary<int, BillboardData> mTexList = new Dictionary<int, BillboardData>();

    void OnEnable()
    {
        KOZNet.BillboardEvent += ReceiveNewImage;
    }

    void OnDisable()
    {
        KOZNet.BillboardEvent -= ReceiveNewImage;
    }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        mTime += Time.deltaTime;
        if (mTime >= UpdateTime)
        {
            mTime = 0;
            RefreshImage();
        }
	}

    private void ReceiveNewImage(int id)
    {
        mTexList[id] = NetComponent.GetBillboardData(id);
    }

    private int mSelectIndex = -1;
    private void RefreshImage()
    {
        if (Random.Range(0, 4) == 0)
        {
            GlobalModule.Instance.EnableAd = true;
        }
        else
        {
            if (mTexList.Count > 0)
            {
                List<int> datas = new List<int>();
                foreach (BillboardData data in mTexList.Values)
                {
                    datas.Add(data.ID);
                }

                int index = datas[Random.Range(0, datas.Count)];
                ImageUI.material.mainTexture = mTexList[index].Texture;
                mSelectIndex = index;

                FadeInBillboard();
                Invoke("FadeOutBillboard", 8);

                GlobalModule.Instance.EnableAd = false;
            }
            else
            {
                GlobalModule.Instance.EnableAd = true;
            }
        }
    }

    private void FadeInBillboard()
    {
        ImageUI.enabled = true;

        ImageUI.gameObject.transform.localPosition = new Vector3(-800, 50, -2);
        TweenPosition.Begin(ImageUI.gameObject, 0.5f, new Vector3(0, 50, -2));
    }

    private void FadeOutBillboard()
    {
        ImageUI.gameObject.transform.localPosition = new Vector3(0, 50, -2);
        TweenPosition.Begin(ImageUI.gameObject, 0.5f, new Vector3(800, 50, -2));

        Invoke("CloseBillboard", 0.6f);
    }

    private void CloseBillboard()
    {
        ImageUI.enabled = false;
    }

    private void OnClick()
    {
        if (mSelectIndex != -1 && mTexList.ContainsKey(mSelectIndex))
        {
            string url = mTexList[mSelectIndex].URL;
            if (!string.IsNullOrEmpty(url))
            {
                GlobalModule.Instance.LinkToURL(url);
            }
        }
    }
}
