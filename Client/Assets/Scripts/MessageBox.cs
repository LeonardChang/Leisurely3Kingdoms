using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBox : MonoBehaviour {
    public UILabel TitleLabel;
    public UILabel MessageLabel;
    public UILabel ButtonLeftLabel;
    public UILabel ButtonRightLabel;
    public UILabel ButtonCenterLabel;
    public UILabel ButtonShareLabel;
    public GameObject ButtonLeft;
    public GameObject ButtonRight;
    public GameObject ButtonCenter;
    public GameObject BasePanel;
    public UITexture ImageTex;
    public UILabel Message2Label;

    public static event System.Action<string> alertButtonClickedEvent;

    private class MessageData
    {
        public string mTitle = "";
        public string mMessage = "";
        public string mButton1 = "";
        public string mButton2 = "";
        public bool mOpenLeft = false;
        public string mImage = "";

        public MessageData(string _title, string _message, string _button1, string _button2, bool _openLeft, string _image)
        {
            mTitle = _title;
            mMessage = _message;
            mButton1 = _button1;
            mButton2 = _button2;
            mOpenLeft = _openLeft;
            mImage = _image;
        }
    }
    private List<MessageData> mMessageList = new List<MessageData>();

	// Use this for initialization
	void Start () {
        ButtonShareLabel.text = StringTable.GetString(EStringIndex.UIText_Share);//mStringType != ELocalizationTyp.Chinese ? "Share" : "分享";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ShowMessageBox(string _title, string _message, string _button1, string _button2)
    {
        mMessageList.Add(new MessageData(_title, _message, _button1, _button2, true, ""));
        PopMessage();
    }

    public void ShowMessageBox(string _title, string _message, string _button)
    {
        mMessageList.Add(new MessageData(_title, _message, _button, "", false, ""));
        PopMessage();
    }

    public void ShowMessageBoxWithImage(string _title, string _message, string _button, string _resource)
    {
        mMessageList.Add(new MessageData(_title, _message, _button, "", false, _resource));
        PopMessage();
    }

    private void PopMessage()
    {
        if (mMessageList.Count > 0)
        {
            MessageData msg = mMessageList[0];
            ShowMessageBox(msg.mTitle, msg.mMessage, msg.mButton1, msg.mButton2, msg.mOpenLeft, msg.mImage);
            mMessageList.RemoveAt(0);
        }
    }

    private void ShowMessageBox(string _title, string _message, string _button1, string _button2, bool _openLeft, string _image)
    {
        //alertButtonClickedEvent = null;

        BasePanel.SetActiveRecursively(true);

        string msg = _message;
        msg = msg.Replace("金币", "[ffffff]ф[-]");
        msg = msg.Replace("水晶", "[ffffff]ж[-]");

        TitleLabel.text = _title;
        if (string.IsNullOrEmpty(_image))
        {
            MessageLabel.text = msg;
            Message2Label.gameObject.active = false;
            ImageTex.gameObject.active = false;
            ImageTex.material.mainTexture = null;
        }
        else
        {
            Message2Label.text = msg;
            MessageLabel.gameObject.active = false;
            ImageTex.material.mainTexture = GlobalModule.Instance.LoadResource(_image) as Texture2D;
        }

        if (!_openLeft)
        {
            ButtonLeft.SetActiveRecursively(false);
            ButtonRight.SetActiveRecursively(false);
            ButtonCenter.SetActiveRecursively(true);

            ButtonCenterLabel.text = _button1;
        }
        else
        {
            ButtonLeft.SetActiveRecursively(true);
            ButtonRight.SetActiveRecursively(true);
            ButtonCenter.SetActiveRecursively(false);

            ButtonRightLabel.text = _button2;
            ButtonLeftLabel.text = _button1;
        }

        BasePanel.transform.localScale = new Vector3(0.1f, 0.1f, 1);
        BasePanel.transform.localEulerAngles = new Vector3(0, 0, 180);
        TweenScale.Begin(BasePanel, 0.15f, new Vector3(1, 1, 1));
        TweenRotation.Begin(BasePanel, 0.15f, Quaternion.Euler(Vector3.zero));

        if (GlobalModule.Instance.MSGBoxSpecial.Panel_MessageBoxSpecial.active)
        {
            GlobalModule.Instance.MSGBoxSpecial.OnCancelButton();
        }

        GlobalModule.Instance.PlaySE(GlobalModule.Instance.LoadResource("Sound/MessageBox") as AudioClip);
    }

    private string mClickText = "";
    public void ClickLeftButton()
    {
        BasePanel.transform.localScale = new Vector3(1, 1, 1);
        BasePanel.transform.localEulerAngles = Vector3.zero;
        TweenScale.Begin(BasePanel, 0.25f, new Vector3(0, 0, 1));
        TweenRotation.Begin(BasePanel, 0.25f, Quaternion.Euler(new Vector3(0, 0, -170)));

        Invoke("HideSelf", 0.25f);

        mClickText = ButtonLeftLabel.text;
    }

    public void ClickRightButton()
    {
        BasePanel.transform.localScale = new Vector3(1, 1, 1);
        BasePanel.transform.localEulerAngles = Vector3.zero;
        TweenScale.Begin(BasePanel, 0.25f, new Vector3(0, 0, 1));
        TweenRotation.Begin(BasePanel, 0.25f, Quaternion.Euler(new Vector3(0, 0, -170)));

        Invoke("HideSelf", 0.25f);

        mClickText = ButtonRightLabel.text;
    }

    public void ClickCenterButton()
    {
        BasePanel.transform.localScale = new Vector3(1, 1, 1);
        BasePanel.transform.localEulerAngles = Vector3.zero;
        TweenScale.Begin(BasePanel, 0.25f, new Vector3(0, 0, 1));
        TweenRotation.Begin(BasePanel, 0.25f, Quaternion.Euler(new Vector3(0, 0, -170)));

        Invoke("HideSelf", 0.25f);

        mClickText = ButtonCenterLabel.text;
    }

    public void ClickShareButton()
    {
        GlobalModule.Instance.LinkToWWW(LinkToWWWEnum.ShareMe2);
    }
    
    private void HideSelf()
    {
        BasePanel.SetActiveRecursively(false);
        if (alertButtonClickedEvent != null)
            alertButtonClickedEvent(mClickText);

        PopMessage();
    }
}
