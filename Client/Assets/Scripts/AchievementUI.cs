using UnityEngine;
using System.Collections;

public class AchievementUI : MonoBehaviour {
    public UILabel TitleLabel;
    public UILabel NameLabel;

    private bool mOpen = false;
    private float mBuffTime = 0;
    private float mBuffTimeMax = 3;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (mOpen && mBuffTime < mBuffTimeMax)
        {
            mBuffTime += Time.deltaTime;
            if (mBuffTime >= mBuffTimeMax)
            {
                ClosePanel();
            }
        }
	}

    private void OpenPanel()
    {
        if (mOpen)
        {
            return;
        }

        GlobalModule.Instance.PlaySE(GlobalModule.Instance.LoadResource("Sound/ItemFly") as AudioClip);
        GlobalModule.Instance.PlaySE(GlobalModule.Instance.LoadResource("Sound/Clap") as AudioClip, 0.65f);

        gameObject.SetActiveRecursively(true);
        mOpen = true;
        transform.localScale = new Vector3(1, 0.1f, 1);
        TweenScale.Begin(gameObject, 0.15f, new Vector3(1, 1.5f, 1));

        mBuffTime = 0;
    }

    private void ClosePanel()
    {
        if (!mOpen)
        {
            return;
        }

        gameObject.SetActiveRecursively(true);
        mOpen = false;
        transform.localScale = new Vector3(1, 1.5f, 1);
        TweenScale.Begin(gameObject, 0.15f, new Vector3(1, 0, 1));
        Invoke("HideAll", 0.15f);
    }

    private void HideAll()
    {
        gameObject.SetActiveRecursively(false);
    }

    public void ShowAchievement(string _name)
    {
        TitleLabel.text = StringTable.GetString(EStringIndex.UIText_AchieveUnLock);//StringTable.mStringType != ELocalizationTyp.Chinese ? "< Achievement unlocked >" : "< 新成就解锁 >";
        NameLabel.text = _name;

        OpenPanel();
    }

    public void ClickClose()
    {
        ClosePanel();
    }
}
