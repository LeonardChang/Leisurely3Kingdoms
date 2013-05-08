using UnityEngine;
using System.Collections;

public class GUIDragBottomButton : MonoBehaviour {

    Vector3 mStartPos;
	// Use this for initialization
	void Start () 
    {
        mStartPos = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {
        if(gameObject.name == "IBtn_PaiHangBang" || gameObject.name == "IBtn_ChengJiu")
        {
            Publisher publisher = (Publisher)PubilshSettingData.Instance.SelectedPublisher;
            if (gameObject.active && (publisher == Publisher.Mobage_CN || publisher == Publisher.Mobage_TW))
            {
                gameObject.SetActiveRecursively(false);
            }
        }

	    
	}

    void OnDrag(Vector2 _delta)
    {
        //Debug.Log(_delta);
        if(_delta.x > Screen.width / 15)
        {
            if (!GlobalModule.Instance.IsCanClick)
                return;
            GameDataCenter.Instance.GuiManager.SlideBottomBtnRight();
        }
        if(_delta.x < -Screen.width / 15)
        {
            if (!GlobalModule.Instance.IsCanClick)
                return;
            GameDataCenter.Instance.GuiManager.SlideBottomBtnLeft();
        }
    }

    /// <summary>
    /// 复位
    /// </summary>
    void ResetPosition()
    {
        transform.localPosition = mStartPos;
    }
}
