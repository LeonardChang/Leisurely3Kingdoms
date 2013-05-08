using UnityEngine;
using System.Collections;

public class MessageBoxSpecial : MonoBehaviour 
{
    public static event System.Action<string> alterButtonClickedEvent;

    public UILabel Label_Title;
    public UILabel Label_Message_1;
    public UILabel Label_Message_2;
    public UIImageButton IBtn_OK;
    public UIImageButton IBtn_Cancel;
    public UIImageButton IBtn_Share;
    public UILabel Label_OK;
    public UILabel Label_Cancel;
    public UITexture Texture_Icon;
    public UILabel Sprite_Title;
    public UISprite Sprite_Text_Back;
    public Transform Sprite_Message_Back;

    string mClickText = "";
    bool mIsOpen = false;
    //0-提示 1-提问 2-祝贺 3-胜利 4-遗憾
    int mMessageType = 0;
    public GameObject Panel_MessageBoxSpecial;
    

    void HideAll()
    {
        Label_Title.gameObject.SetActiveRecursively(false);
        Label_Message_2.gameObject.SetActiveRecursively(false);
        Sprite_Text_Back.gameObject.SetActiveRecursively(false);
        IBtn_Cancel.gameObject.SetActiveRecursively(false);
        IBtn_Share.gameObject.SetActiveRecursively(false);
        Sprite_Text_Back.gameObject.SetActiveRecursively(false);
        Sprite_Title.gameObject.SetActiveRecursively(false);
    }

	// Use this for initialization
	void Start ()
    {
	    
	}

    /// <summary>
    /// 对话框图标
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_pos"></param>
    void SetTextureIcon(string _path, Vector3 _pos)
    {
        Texture text = GlobalModule.Instance.LoadResource(_path) as Texture;
        Texture_Icon.material.mainTexture = text;
        Texture_Icon.transform.localScale = new Vector3(text.width, text.height, 0);
        Texture_Icon.transform.localPosition = _pos;
    }

    /// <summary>
    /// 设置文本
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_pos"></param>
    /// <param name="_line_width"></param>
    void SetLabel1(string _message, Vector3 _pos, int _line_width)
    {
        Label_Message_1.text = _message;
        Label_Message_1.transform.localPosition = _pos;
        Label_Message_1.lineWidth = _line_width;
    }

    void SetOKButton(string _button, Vector3 _pos)
    {

    }

    /// <summary>
    /// 提示
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageSigh(string _title, string _message, string _button)
    {
        Panel_MessageBoxSpecial.SetActiveRecursively(true);
        HideAll();

        Label_Title.gameObject.SetActiveRecursively(true);

        Sprite_Message_Back.localScale = new Vector3(448, 300, 0);
        SetTextureIcon("GUI/MessageBox/Special/SighIcon", new Vector3(-150, 80, -1));
        SetLabel1(_message, new Vector3(-85, 82, -5), 290);

        Label_Title.text = _title;
        Label_Title.transform.localPosition = new Vector3(0, 115, -5);
        IBtn_OK.transform.localPosition = new Vector3(0, -87, -1);
        Label_OK.text = _button;
        mMessageType = 0;
        ScaleToOne();
    }

    /// <summary>
    /// 提问
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_message"></param>
    /// <param name="_button_ok"></param>
    /// <param name="_button_cancel"></param>
    public void ShowMessageQuestion(string _title, string _message, string _button_ok, string _button_cancel)
    {
        string msg = _message;
        msg = msg.Replace("金币", "[ffffff]ф[-]");
        msg = msg.Replace("水晶", "[ffffff]ж[-]");

        Panel_MessageBoxSpecial.SetActiveRecursively(true);
        HideAll();

        IBtn_Cancel.gameObject.SetActiveRecursively(true);
        Label_Title.gameObject.SetActiveRecursively(true);

        Sprite_Message_Back.localScale = new Vector3(448, 300, 0);
        SetTextureIcon("GUI/MessageBox/Special/Question", new Vector3(-150, 80, -1));
        SetLabel1(msg, new Vector3(-85, 82, -5), 290);

        Label_Title.text = _title;
        Label_Title.transform.localPosition = new Vector3(0, 115, -5);
        IBtn_OK.transform.localPosition = new Vector3(-110, -87, -1);
        Label_OK.text = _button_ok;
        IBtn_Cancel.transform.localPosition = new Vector3(110, -87, -1);
        Label_Cancel.text = _button_cancel;

        mMessageType = 1;

        ScaleToOne();
    }

    /// <summary>
    /// 祝贺
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageCongratulation(string _message, string _button)
    {
        string msg = _message;
        msg = msg.Replace("金币", "[ffffff]ф[-]");
        msg = msg.Replace("水晶", "[ffffff]ж[-]");


        Panel_MessageBoxSpecial.SetActiveRecursively(true);
        HideAll();

        Sprite_Title.gameObject.SetActiveRecursively(true);

        Sprite_Title.text = StringTable.GetString(EStringIndex.UIText_Msg_Congratulation);
        Sprite_Title.transform.localPosition = new Vector3(88, 155, -15);

        Sprite_Message_Back.localScale = new Vector3(448, 440, 0);
        SetTextureIcon("GUI/MessageBox/Special/Congratulation", new Vector3(0, 116.5f, -1));
        SetLabel1(msg, new Vector3(-190, 10, -5), 390);

        IBtn_OK.transform.localPosition = new Vector3(0, -150, -1);
        Label_OK.text = _button;
        mMessageType = 2;
        ScaleToOne();

    }


    public void ShowMessageSad(string _message, string _button)
    {
        string msg = _message;
        msg = msg.Replace("金币", "[ffffff]ф[-]");
        msg = msg.Replace("水晶", "[ffffff]ж[-]");


        Panel_MessageBoxSpecial.SetActiveRecursively(true);
        HideAll();

        Sprite_Title.gameObject.SetActiveRecursively(true);

        Sprite_Title.text = StringTable.GetString(EStringIndex.UIText_Msg_Sad);
        Sprite_Title.transform.localPosition = new Vector3(88, 155, -15);

        Sprite_Message_Back.localScale = new Vector3(448, 440, 0);
        SetTextureIcon("GUI/MessageBox/Special/Sad", new Vector3(0, 116.5f, -1));
        SetLabel1(msg, new Vector3(-190, 10, -5), 390);

        IBtn_OK.transform.localPosition = new Vector3(0, -150, -1);
        Label_OK.text = _button;
        mMessageType = 2;
        ScaleToOne();
    }

    /// <summary>
    /// 胜利
    /// </summary>
    /// <param name="_message"></param>
    /// <param name="_button"></param>
    public void ShowMessageWin(string _message, string _button, int _time, int _money, int _gem, int _zombie, string _mvp)
    {
        mWinText = _message;
        Panel_MessageBoxSpecial.SetActiveRecursively(true);
        HideAll();

        Sprite_Title.gameObject.SetActiveRecursively(true);
        Sprite_Text_Back.gameObject.SetActiveRecursively(true);
        Label_Message_2.gameObject.SetActiveRecursively(true);
        IBtn_Share.gameObject.SetActiveRecursively(true);


        Sprite_Title.text = StringTable.GetString(EStringIndex.UIText_Msg_Win);
        Sprite_Title.transform.localPosition = new Vector3(0, 221, -15);

        Sprite_Message_Back.localScale = new Vector3(448, 533, 0);
        SetTextureIcon("GUI/MessageBox/Special/Win", new Vector3(1.5f, 113.5f, -1));
        SetLabel1(_message, new Vector3(-193.5f, -43,  -5), 390);
        Label_Message_2.text = GlobalStaticData.GetWinWord();

        IBtn_OK.transform.localPosition = new Vector3(-110, -206, 0);
        Label_OK.text = _button;

        mWinTime = 0;
        mWinMoney = 0;
        mWinGem = 0;
        mWinZombie = 0;

        mWinTimeFinal = _time;
        mWinMoneyFinal = _money;
        mWinGemFinal = _gem;
        mWinZombieFinal = _zombie;

        mWinMVP = _mvp;

        mMessageType = 3;
        mProgress = 0;
        ScaleToOne();
        ResourcePath.PlaySound("Msg_good");
    }




    void ScaleToOne()
    {
        Panel_MessageBoxSpecial.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        Panel_MessageBoxSpecial.transform.localEulerAngles = new Vector3(0, 0, 180);
        TweenScale.Begin(Panel_MessageBoxSpecial, 0.15f, Vector3.one);
        TweenRotation.Begin(Panel_MessageBoxSpecial, 0.15f, Quaternion.Euler(Vector3.zero));
        GlobalModule.Instance.PlaySE(GlobalModule.Instance.LoadResource("Sound/MessageBox") as AudioClip);

        if (GlobalModule.Instance.MSGBox.BasePanel.active)
        {
            GlobalModule.Instance.MSGBox.ClickRightButton();
        }
    }

    void ScaleToZero()
    {
        Panel_MessageBoxSpecial.transform.localScale = Vector3.one;
        Panel_MessageBoxSpecial.transform.localEulerAngles = Vector3.zero;
        TweenScale.Begin(Panel_MessageBoxSpecial, 0.15f, new Vector3(0.01f, 0.01f, 1));
        TweenRotation.Begin(Panel_MessageBoxSpecial, 0.15f, Quaternion.Euler(new Vector3(0, 0, 180)));
    }

    public void OnOKButton()
    {
        ScaleToZero();
        Invoke("HideSelf", 0.25f);
        mClickText = Label_OK.text;
    }

    public void OnCancelButton()
    {
        ScaleToZero();
        Invoke("HideSelf", 0.25f);
        mClickText = Label_Cancel.text;

        alterButtonClickedEvent = null;
    }

    public void OnShareButton()
    {
        GlobalModule.Instance.LinkToWWW(LinkToWWWEnum.ShareMe2);
    }

    void HideSelf()
    {
        Panel_MessageBoxSpecial.SetActiveRecursively(false);
        if (alterButtonClickedEvent != null)
            alterButtonClickedEvent(mClickText);
    }




	
	// Update is called once per frame
    int mWinTime = 0;
    int mWinMoney = 0;
    int mWinGem = 0;
    int mWinZombie = 0;

    int mWinTimeFinal = 0;
    int mWinMoneyFinal = 0;
    int mWinGemFinal = 0;
    int mWinZombieFinal = 0;


    string mWinMVP = "";
    string mWinText = "时间：{0:D}分钟    MVP：{1:S}\n共{2:D}只僵尸参战。\n收获：{3:D}[ffffff]ф[-]   {4:D}[ffffff]ж[-]";
    float mProgress = 0;
	void Update () 
    {
        if (mMessageType == 3)
        {
            if (mProgress < 1)
                mProgress += Time.deltaTime;

            int win_time = (int)Mathf.Lerp(mWinTime, mWinTimeFinal, mProgress);
            int win_money = (int)Mathf.Lerp(mWinMoney, mWinMoneyFinal, mProgress);
            int win_gem = (int)Mathf.Lerp(mWinGem, mWinGemFinal, mProgress);
            int win_zombie = (int)Mathf.Lerp(mWinZombie, mWinZombieFinal, mProgress);

            string show_text = string.Format(mWinText, win_time, mWinMVP, win_zombie, win_money, win_gem);
            Label_Message_1.text = show_text;
        }
	}
}
