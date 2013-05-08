using UnityEngine;
using System.Collections;

/// <summary>
/// 跳动数字
/// </summary>
public class FlyLabel : MonoBehaviour {

    /// <summary>
    /// 类型 0-扣血   1-金币   2-任务成绩点
    /// </summary>
    public int mType = 0;

    /// <summary>
    /// 值
    /// </summary>
    public int mValue = 0;

    /// <summary>
    /// 是否是暴击
    /// </summary>
    public bool mHasCritical = false;

    /// <summary>
    /// 值Label
    /// </summary>
    public UILabel Label;
    /// <summary>
    /// 暴击背景Label
    /// </summary>
    public UILabel BackLabel;

	// Use this for initialization
    void Start()
    {
    }

    void OnEnable() 
    {
        isIni = false;
    }

    void OnDisable()
    {
        isIni = false;
        CancelInvoke("DestroySelf");
    }

    private bool isIni = false;

	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;
            Init();
        }
	}

    private void Init()
    {
        switch (mType)
        {
            case 0:
                Label.color = Color.red;
                OOTools.OOTweenColorEx(Label.gameObject, Color.red, Color.red, new Color(1, 0, 0, 0));
                Label.text = "-" + (Mathf.Abs(-mValue)).ToString();
                break;
            case 1:
                Label.color = Color.yellow;
                OOTools.OOTweenColorEx(Label.gameObject, Color.yellow, Color.yellow, new Color(1, 1, 0, 0));
                Label.text = "+" + mValue.ToString();
                break;
            case 2:
                Label.color = Color.green;
                OOTools.OOTweenColorEx(Label.gameObject, Color.green, Color.green, new Color(0, 1, 0, 0));
                Label.text = "+" + mValue.ToString();
                break;
        }

        int scale;
        if (mValue >= 30)
        {
            scale = 34;
        }
        else if (mValue > 20)
        {
            scale = 32;
        }
        else if (mValue >= 10)
        {
            scale = 30;
        }
        else
        {
            scale = 25;
        }

        if (mHasCritical)
        {
            OOTools.OOTweenColorEx(BackLabel.gameObject, Color.white, Color.white, new Color(1, 1, 1, 0));
            BackLabel.enabled = true;
        }
        else
        {
            BackLabel.enabled = false;
        }

        Label.transform.localScale = new Vector3(scale, scale, 1);

        OOTools.OOTweenScaleEx(gameObject, new Vector3(0.1f, 0.1f, 1), new Vector3(2.75f, 2.75f, 1), new Vector3(1.5f, 1.5f, 1));

        float height = Random.Range(0, 10);
        OOTools.OOTweenPosition(gameObject, transform.localPosition + new Vector3(0, height, 0), transform.localPosition + new Vector3(0, 60 + height, 0));

        Invoke("DestroySelf", 1.5f);
    }

    private void DestroySelf()
    {
        GlobalModule.Instance.Pool.DestoryObject(gameObject, "FlyLabel");
    }
}
