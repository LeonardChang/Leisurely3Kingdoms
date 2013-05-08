using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GUIStoryManager : MonoBehaviour
{

    public GameObject Map_First_Name;
    public GameObject Map_Second_Name;

    public GameObject Sprite_Head;
    public Transform Grid;
    public AttackManager Panel_Attack;

    public TweenScale IBtn_StoryBack;
    public GameObject IBtn_StorySkip;

    public GameObject Story_InfoRect;
    public GameObject Story_InfoRectReview;

    //int mCurrentStory = 0;
    /// <summary>
    /// 剧情进度
    /// </summary>
    int mStoryState = -1;
    float mStoryDelay = 0;

    int mHeadCurrent = 0;
    /// <summary>
    /// 头像
    /// </summary>
    public GameObject Story_Left_Head;
    public GameObject Story_Right_Head;

    public UITexture Story_Left_Face;
    public UITexture Story_Left_Brow;
    public UITexture Story_Right_Face;
    public UITexture Story_Right_Brow;

    public GameObject Panel_Map;
    public Transform Panel_Map_Target;


    public float mDelay = 0;
    public Transform Panel_Talk;
    public GameObject TalkDialog;

    /// <summary>
    /// 特殊对话框
    /// </summary>
    public Material Story_duihuakuang2;
    public Material Story_duihuakuang3;


    /// <summary>
    /// 头像特殊位置
    /// </summary>
    Vector3 LF_In = new Vector3(-300f, 70f, -1000f);
    Vector3 LF_Out = new Vector3(-720f, 70f, -1000f);

    Vector3 RF_In = new Vector3(230f, 70f, -1000f);
    Vector3 RF_Out = new Vector3(730f, 70f, -1000f);


    //博士钱 博士伤心 博士笑 国王惊讶 骑士变绿 天子 公主
    Rect[] BrowRec = new Rect[]
    {
        new Rect(92, 0, 386, 412),
        new Rect(90, 0, 386, 410),
        new Rect(90, 0, 386, 411),
        new Rect(-3, 185, 264, 325),
        new Rect(-63, 18, 463, 426),
        new Rect(9, 155, 170, 134),
        new Rect(9, 180, 257, 193)
    };


    //博士 国王 骑士 死神 女巫 独角兽 女王 大熊
    Rect[] FaceRect = new Rect[]
    {
        new Rect(90, 0, 392, 408),

        new Rect(0, 0, 340, 511),
        new Rect(-35, 0, 529, 445),
        new Rect(-35, 0, 491, 492),
        new Rect(0, 0, 494, 517),

        new Rect(0, 0, 496, 467),
        new Rect(-35, 0, 496, 454),
        new Rect(0, 0, 550, 480),

        new Rect(-50, 0, 473, 498),
        new Rect(-50, 0, 473, 471),
        new Rect(-35, 0, 529, 406),
        new Rect(0, 0, 482, 505),
    };

    /// <summary>
    /// 剧本脚本列表
    /// </summary>
    string[] mScriptList;


    int mCurrent = 0;

    /// <summary>
    /// 地图是否被打开
    /// </summary>
    bool isOpen = false;
    public bool IsOpen
    {
        get { return isOpen; }
    }

    /// <summary>
    /// 当前剧本
    /// </summary>
    public int mStoryIndex = 1;
    public bool isStoryRun= false;


    public GameObject SpriteSceneRender_Map;

    public GameObject SpriteSceneRender_Obj;
    public GameObject SpriteSceneRender_GUI;
    public GameObject SpriteSceneRender_Panel;
    public GameObject SpriteSceneRender_Top;

    /// <summary>
    /// 面板打开类型  0-剧情  1-观看
    /// </summary>
    public int mOpenType = 0;


    void ActiveOtherRenderer()
    {
        SpriteSceneRender_Obj.SetActiveRecursively(true);
        SpriteSceneRender_GUI.SetActiveRecursively(true);
        SpriteSceneRender_Panel.SetActiveRecursively(true);
        SpriteSceneRender_Top.SetActiveRecursively(true);
    }

    void DeActiveOtherRenderer()
    {
        SpriteSceneRender_Obj.SetActiveRecursively(false);
        SpriteSceneRender_GUI.SetActiveRecursively(false);
        SpriteSceneRender_Panel.SetActiveRecursively(false);
        SpriteSceneRender_Top.SetActiveRecursively(false);
    }

    /// <summary>
    /// 初始地图位置
    /// </summary>
    void SetMapPosition()
    {
        Panel_Map.transform.localPosition = new Vector3(0, 60, 0);//mMapPos;
        Panel_Map.transform.localScale = new Vector3(1, 1, 1);//mMapScale;
        
    }

    bool isIni = false;

    // Use this for initialization
    void Start()
    {

    }

    void OnEnable()
    {
        isIni = false;
    }

    /// <summary>
    /// 执行脚本
    /// </summary>
    /// <param name="_script"></param>
    void RunScript(string _script)
    {
        if(!isOpen)
        {
            return;
        }
        if(_script.StartsWith("LStand"))
        {
            Story_Left_Head.transform.localPosition = LF_In;
            string set_string = _script.Replace("LStand", "LSet");
            Story_Left_Face.GetComponent<UITexture>().alpha = 1;
            RunScript(set_string);
        }
        else if(_script.StartsWith("RStand"))
        {
            Story_Right_Head.transform.localPosition = RF_In;
            string set_string = _script.Replace("RStand", "RSet");
            RunScript(set_string);
        }
        else if(_script.StartsWith("LSet"))
        {
            EventLeftSet(_script);
        }
        else if(_script.StartsWith("LIn"))
        {
            EventLeftIn();
        }
        else if(_script.StartsWith("LOut"))
        {
            EventLeftOut();
        }
        else if(_script.StartsWith("LChange"))
        {

        }
        else if(_script.StartsWith("RSet"))
        {
            EventRightSet(_script);
        }
        else if(_script.StartsWith("RIn"))
        {
            EventRightIn();
        }
        else if(_script.StartsWith("ROut"))
        {
            EventRightOut();
        }
        else if(_script.StartsWith("RChange"))
        {

        }
        else if(_script.StartsWith("Say"))
        {
            string[] arg = _script.Split(' ');
            if (arg.Length >= 5)
            {
                string name = arg[3];
                if(StringTable.mStringType == ELocalizationTyp.English)
                {
                    name = arg[3].Replace("_", " ");
                }
                
                string say = "";
                for(int i = 4; i < arg.Length; i ++)
                {
                    say = say + arg[i] + " ";
                }
                EventSay(name, say, int.Parse(arg[1]), int.Parse(arg[2]), 3f);
            }
        }
        else if(_script.StartsWith("LSay"))
        {

        }
        else if(_script.StartsWith("RSay"))
        {
            
        }
        else if(_script.StartsWith("LFace"))
        {
            EventLeftFace(_script);
        }
        else if (_script.StartsWith("RFace"))
        {
            EventRightFace(_script);
        }
        else if (_script.StartsWith("Wait"))
        {
            string[] arg = _script.Split(' ');
            if (arg.Length == 2)
            {
                mDelay = float.Parse(arg[1]) * 2;
            }
        }
        else if(_script.StartsWith("KillEffect"))
        {
            EventKillEffect();
        }
    }

    /// <summary>
    /// 杀死国王特效
    /// </summary>
    void EventKillEffect()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_KillKing);
        obj.transform.parent = GameDataCenter.Instance.GuiManager.Camera_2D.transform.parent;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(200, -200, -2273);

        ResourcePath.PlaySound(EResourceAudio.Audio_KillKing);
    }

    /// <summary>
    /// 移动头像
    /// </summary>
    void MoveHead()
    {

        TweenPosition tp = Sprite_Head.GetComponent<TweenPosition>();
        tp.enabled = true;
        if(start_nod.localPosition.x > end_nod.localPosition.x)
        {
            Sprite_Head.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            Sprite_Head.transform.localScale = new Vector3(1, 1, 1);
        }

        tp.from = start_nod.localPosition;
        Vector3 vc3 = end_nod.localPosition;
        vc3.z = tp.transform.localPosition.z;
        tp.to = vc3;
        tp.Reset();
        tp.Play(true);
    }

    /// <summary>
    /// 头像移动完毕
    /// </summary>
    void OnHeadMotionEnd()
    {
        isStoryRun = true;
    }

    /// <summary>
    /// 切换左边表情
    /// </summary>
    /// <param name="_script"></param>
    void EventLeftFace(string _script)
    {
        if (_script.StartsWith("LFace"))
        {
            string[] arg = _script.Split(' ');
            //int arg_count = arg.Length;
            int face_index = -1;

            if (arg.Length == 1)
            {
                Story_Left_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 0);
                Story_Left_Face.GetComponent<UITexture>().alpha = 1;

            }
            else if (arg.Length == 2)
            {
                face_index = int.Parse(arg[1]);

                Story_Left_Brow.GetComponent<UITexture>().material.mainTexture = ResourcePath.GetFacePic(face_index);
                Story_Left_Brow.transform.localPosition = new Vector3(BrowRec[face_index].x, BrowRec[face_index].y, -10);
                Story_Left_Brow.transform.localScale = new Vector3(BrowRec[face_index].width, BrowRec[face_index].height, 1);
                Story_Left_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 1);
                Story_Left_Face.GetComponent<UITexture>().alpha = 0;
            }
        }
    }

    /// <summary>
    /// 切换右边表情
    /// </summary>
    /// <param name="_script"></param>
    void EventRightFace(string _script)
    {
        if (_script.StartsWith("RFace"))
        {
            string[] arg = _script.Split(' ');
            //int arg_count = arg.Length;
            int face_index = -1;

            if (arg.Length == 1)
            {
                Story_Right_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 0);

            }
            else if (arg.Length == 2)
            {
                face_index = int.Parse(arg[1]);
                Story_Right_Brow.GetComponent<UITexture>().material.mainTexture = ResourcePath.GetFacePic(face_index);
                Story_Right_Brow.transform.localPosition = new Vector3(BrowRec[face_index].x, BrowRec[face_index].y, -10);
                Story_Right_Brow.transform.localScale = new Vector3(BrowRec[face_index].width, BrowRec[face_index].height, 1);
                Story_Right_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 1);
            }
        }
    }

    /// <summary>
    /// 设置左边头像
    /// </summary>
    void EventLeftSet(string _script)
    {
        string[] arg = _script.Split(' ');
        //int arg_count = arg.Length;
        int face_index = -1;

        if (arg.Length == 1)
        {
            Story_Left_Face.GetComponent<UITexture>().material.mainTexture = null;
        }
        else if (arg.Length == 2)
        {
            face_index = int.Parse(arg[1]);
            Story_Left_Face.GetComponent<UITexture>().material.mainTexture = ResourcePath.GetHeadPic(face_index);
        }
    }


    /// <summary>
    /// 设置右边头像
    /// </summary>
    void EventRightSet(string _script)
    {
        if (_script.StartsWith("RSet "))
        {
            string[] arg = _script.Split(' ');
            //int arg_count = arg.Length;
            int face_index = -1;

            if (arg.Length == 1)
            {
                Story_Right_Face.GetComponent<UITexture>().material.mainTexture = null;
            }
            else if (arg.Length == 2)
            {
                face_index = int.Parse(arg[1]);
                Story_Right_Face.GetComponent<UITexture>().material.mainTexture = ResourcePath.GetHeadPic(face_index);
                Story_Right_Face.transform.localScale = new Vector3(FaceRect[face_index].width, FaceRect[face_index].height, 1);
                Story_Right_Face.transform.localPosition = new Vector3(FaceRect[face_index].x, FaceRect[face_index].y, 0);
            }
        }
    }


    /// <summary>
    /// 左头像进入
    /// </summary>
    void EventLeftIn()
    {
        iTween.MoveTo(Story_Left_Head, iTween.Hash("position", LF_In,
                                                    "time", 1,
                                                    "islocal", true
                                                    ));
        mDelay = 1;
    }

    /// <summary>
    /// 左头像出去
    /// </summary>
    void EventLeftOut()
    {
        iTween.MoveTo(Story_Left_Head, iTween.Hash("position", LF_Out,
                                                    "time", 1,
                                                    "islocal", true
                                                    ));
        mDelay = 1;
    }


    /// <summary>
    /// 右头像进入
    /// </summary>
    void EventRightIn()
    {
        iTween.MoveTo(Story_Right_Head, iTween.Hash("position", RF_In,
                                            "time", 1,
                                            "islocal", true
                                            ));
    }

    /// <summary>
    /// 右头像出去
    /// </summary>
    void EventRightOut()
    {
        iTween.MoveTo(Story_Right_Head, iTween.Hash("position", RF_Out,
                                            "time", 1,
                                            "islocal", true
                                            ));
    }


    /// <summary>
    /// 说话
    /// </summary>
    void EventSay(string _name, string _say, int _direction, int _effect_type, float _delay)
    {
        GameObject obj_Talk = (GameObject)Instantiate(TalkDialog);
        obj_Talk.transform.parent = Panel_Talk.transform;
        obj_Talk.transform.localScale = Vector3.one;
        obj_Talk.GetComponent<GUIStoryTalk>().mDirection = _direction;
        obj_Talk.GetComponent<GUIStoryTalk>().mName = _name;
        obj_Talk.GetComponent<GUIStoryTalk>().mSay = _say;
        obj_Talk.GetComponent<GUIStoryTalk>().mEffectType = _effect_type;

        foreach (Transform trans in Panel_Talk.transform)
        {
            trans.SendMessage("OnNextStep");
        }

        mDelay = 150;
    }

    Transform start_nod;
    Transform end_nod;
    /// <summary>
    /// 获取剧情点位置
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    Transform GetNodTransform(int _index)
    {
        
        string str = string.Format("Story_{0:D3}", _index);
        return GameObject.Find(str).transform;
    }


    /// <summary>
    /// 设置显示名称
    /// </summary>
    /// <param name="_obj"></param>
    /// <param name="_text"></param>
    void SetShowName(GameObject _obj, string _text)
    {
        _obj.transform.FindChild("Label").GetComponent<UILabel>().text = _text;
        float len = _obj.transform.FindChild("Label").GetComponent<UILabel>().relativeSize.x;
        _obj.transform.FindChild("Background").localScale = new Vector3(Mathf.Max(135, len * 27.5f), 38, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isIni)
        {
            isIni = true;
        }

        //剧情顺序
        if (mOpenType == 0)
        {
            mStoryDelay -= Time.deltaTime;

            if ((GlobalStaticData.IsStageStart(mHeadCurrent) || GlobalStaticData.IsStageEnd(mHeadCurrent)) && mStoryState == 0)
            {
                    mStoryState = 4;
                    OnHeadMotionEnd(); 
            }

            switch (mStoryState)
            { 
                case 0:
                    if(mStoryDelay < 0)
                    {
                        mStoryState = 1;
                        mStoryDelay = 1;
                    }
                    break;
                case 1:
                    if (mStoryDelay <= 0)
                    {
                        //缩放
                        mStoryState = 2;
                        mStoryDelay = 2;

                        Vector3 mid_pos = Vector3.Lerp(start_nod.localPosition, end_nod.localPosition, 0.5f);
                        Vector3 len_pos = Panel_Map.transform.localPosition - mid_pos;
                        Panel_Map.GetComponent<GUIScaleMap>().StartScaleMap(new Vector3(0, 60, 0) - (new Vector3(23, 150, 0) - len_pos * 1.5f));

                    }
                    break;
                case 2:
                    if (mStoryDelay <= 0)
                    {
                        OOTools.OOTweenColor(Map_Second_Name.transform.FindChild("Background").gameObject, new Color(1, 1, 1, 0), new Color(1, 1, 1, 1));
                        OOTools.OOTweenColor(Map_Second_Name.transform.FindChild("Label").gameObject, new Color(47f/255f, 4f/255f, 4f/255f, 0), new Color(47f/255f, 4f/255f, 4f/255f, 1));
                        
                        mStoryState = 3;
                        mStoryDelay = 1;

                    }
                    break;
                case 3:
                    if (mStoryDelay <= 0)
                    {
                        //头动
                        mStoryState = 4;
                        MoveHead();
                    }
                    break;
            }
        }


        if(isStoryRun)
        {
            if (Input.GetMouseButtonDown(0))
            {
                mDelay = 0;
            }
            if (mDelay > 0)
            {
                mDelay -= Time.deltaTime;
                return;
            }

            if (mCurrent < mScriptList.Length && mCurrent >= 0)
            {
                RunScript(mScriptList[mCurrent]);
                mCurrent++;
            }
            else
            {
                EndStory();
            }
        }
    }


    /// <summary>
    /// 下一个
    /// </summary>
    void OnNextEx()
    {
        HideStoryInfoRect();
    }

    /// <summary>
    /// 下一个
    /// </summary>
    public void OnNext()
    {
        HideStoryInfoRect();

        if(mOpenType == 1)
        {
            OOTools.OOTweenScale(IBtn_StoryBack.gameObject, Vector3.one, Vector3.zero);
        }
        Close();
    }


    /// <summary>
    /// 打开
    /// </summary>
    /// <param name="_index"></param>
    public void Open(int _index)
    {
        if(isOpen)
        {
            OOTools.OOTweenScale(IBtn_StoryBack.gameObject, Vector3.one, Vector3.zero);
        }

        if(GameDataCenter.Instance.GuiManager.Weather)
        {
            GameDataCenter.Instance.GuiManager.Weather.SetActiveRecursively(false);
        }

        if (_index == 1)
            mIsCheckLogin = true;
        else
            mIsCheckLogin = false;


        GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.ForceKillAllZombie();

        isOpen = true;
        HideHead();
        GameDataCenter.Instance.GuiManager.Panel_MapName.SetActiveRecursively(true);
        GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.ClosePanel();
        Sprite_Head.GetComponent<TweenPosition>().enabled = false;

        mStoryType = 0;
        mOpenType = 0;
        mHeadCurrent = _index;
        mStoryState = 0;
        mStoryDelay = 1;


/*****************************/
        start_nod = GetNodTransform(mHeadCurrent);
        end_nod = GetNodTransform(mHeadCurrent);


        CStory story_start = GlobalStaticData.GetStory((EStoryIndex)(mHeadCurrent - 1));
        CStory story_end = GlobalStaticData.GetStory((EStoryIndex)(mHeadCurrent));

        if (GlobalStaticData.IsStageEnd(mHeadCurrent))
        {
            end_nod = GetNodTransform(mHeadCurrent - 1);
            story_end = GlobalStaticData.GetStory((EStoryIndex)(mHeadCurrent - 1));
        }
        else if (GlobalStaticData.IsStageStart(mHeadCurrent))
        {
            start_nod = GetNodTransform(mHeadCurrent);
            story_start = GlobalStaticData.GetStory((EStoryIndex)(mHeadCurrent));
        }
        else
        {
            start_nod = GetNodTransform(mHeadCurrent - 1);
            end_nod = GetNodTransform(mHeadCurrent);
        }


        Map_First_Name.transform.localPosition = start_nod.localPosition + new Vector3(0, 50, 0);
        Map_Second_Name.transform.localPosition = end_nod.localPosition + new Vector3(0, 50, 0);


        SetShowName(Map_First_Name, story_start.Name);
        SetShowName(Map_Second_Name, story_end.Name);

        Map_Second_Name.transform.FindChild("Background").GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
        Map_Second_Name.transform.FindChild("Label").GetComponent<UILabel>().color = new Color(1, 1, 1, 0);

        Sprite_Head.transform.localPosition = start_nod.localPosition;
/*****************************/
        StartTalking(_index);
        transform.localPosition = new Vector3(0, 0, -600f);
        SpriteSceneRender_Map.SetActiveRecursively(true);
        OOTools.OOTweenScale(IBtn_StorySkip.gameObject, Vector3.zero, Vector3.one);
        SetMapPosition();
        DeActiveOtherRenderer();
    }

    /// <summary>
    /// 用按钮打开该面板
    /// </summary>
    public void OpenByBtn()
    {
        isOpen = true;
        mOpenType = 1;
        Sprite_Head.GetComponent<TweenPosition>().enabled = false;

        if (GameDataCenter.Instance.GuiManager.Weather)
        {
            GameDataCenter.Instance.GuiManager.Weather.SetActiveRecursively(false);
        }

        GameDataCenter.Instance.GuiManager.Panel_MapName.SetActiveRecursively(true);
        //transform.localPosition = new Vector3(0, 0, -600f);

        Story_Left_Head.transform.localPosition = LF_Out;
        Story_Right_Head.transform.localPosition = RF_Out;

        start_nod = GetNodTransform(GameDataCenter.Instance.CurrentStory());
        Sprite_Head.transform.localPosition = start_nod.localPosition;

        CStory story_end = GameDataCenter.Instance.GetCurrentStory();
        //Map_Second_Name.transform.FindChild("Label").GetComponent<UILabel>().text = story_end.Name;
        SetShowName(Map_Second_Name, story_end.Name);
        Map_Second_Name.transform.localPosition = start_nod.localPosition + new Vector3(0, 50, 0);
        Map_Second_Name.transform.FindChild("Background").GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
        Map_Second_Name.transform.FindChild("Label").GetComponent<UILabel>().color =  new Color(47f / 255f, 4f / 255f, 4f / 255f, 1);

        OOTools.OOTweenScale(IBtn_StoryBack.gameObject, Vector3.zero, Vector3.one);
        OOTools.OOTweenPosition(gameObject, new Vector3(0, 1500, -600), new Vector3(0, 0, -600), 0.3f);
        GameDataCenter.Instance.GuiManager.PopTipsSeeStage();

        SetMapPosition();
        GameDataCenter.Instance.GuiManager.Panel_Manager.CloseAll();
    }


    bool mIsCheckLogin = false;
    /// <summary>
    /// 关闭
    /// </summary>
    public void Close()
    {
        Debug.Log("Close");
        if(!isOpen)
        {
            return;
        }
        ActiveOtherRenderer();
        GameDataCenter.Instance.GuiManager.DeleteTipsSeeStage();
        if (GameDataCenter.Instance.GuiManager.Weather)
        {
            GameDataCenter.Instance.GuiManager.Weather.SetActiveRecursively(true);
            //ResourcePath.PlaySound(EResourceAudio.Audio_Rain, true);
        }

        if (mIsCheckLogin)
            GameDataCenter.Instance.GuiManager.Panel_Manager.CheckLogin();
        else
            GameAward.CheckAward();

        HideStoryInfoRect();
        Map_First_Name.transform.localPosition = new Vector3(-1000, -1000, 0);
        Map_Second_Name.transform.localPosition = new Vector3(-1000, -1000, 0);

        isStoryRun = false;

        if (mOpenType == 0)
        {
            Panel_Attack.KillSoldier();
            Panel_Attack.OpenPanel();

        }

        OOTools.OOTweenPosition(gameObject, new Vector3(0, 0, -600), new Vector3(0, 1500, -600), 0.3f);
        GameDataCenter.Instance.GuiManager.Panel_MapName.GetComponent<GUIMapName>().ClosePanel();
        GameDataCenter.Instance.GuiManager.Panel_Manager.CloseAll();
        isOpen = false;


        
        HideHead();
        HideBtns();

        GameDataCenter.Instance.Save();


    }


    /// <summary>
    /// 开始剧情说话
    /// </summary>
    /// <param name="_storyIndex"></param>
    void StartTalking(int _storyIndex)
    {
        mDelay = 0;
        mScriptList = GlobalStaticData.GetStorySay(_storyIndex);
        mCurrent = 0;
        isStoryRun = false;
    }



    /// <summary>
    /// 地图完全关闭
    /// </summary>
    void OnCloseEnd()
    {
        if(!isOpen)
        {
            SpriteSceneRender_Map.SetActiveRecursively(false);
            gameObject.SetActiveRecursively(false);
        }
        else
        {
            DeActiveOtherRenderer();
        }
    }

    /// <summary>
    /// 结束剧情
    /// </summary>
    void EndStory()
    {

        isStoryRun = false;

        HideHead();

        if(mStoryType == 0)
        {
            if(GameDataCenter.Instance.CurrentStory() == 2)
            {
                GameDataCenter.Instance.GetCurrentScene().CreateOneZombie(ZombieType.Zombie03);
            }
            Close();  
        }
        else
        {
            OOTools.OOTweenScale(IBtn_StoryBack.gameObject, Vector3.zero, Vector3.one);
            OOTools.OOTweenScale(IBtn_StorySkip.gameObject, Vector3.one, Vector3.zero);
            //mStoryType = 0;
        }
        //mStoryType = 0;
        //Debug.Log("EndStory");
    }

    /// <summary>
    /// 是否自由模式
    /// </summary>
    public bool FreeState
    {
        get
        {
            bool free = true;
            if(isStoryRun)
            {
                free = false;
            }
            else
            {
                if(mOpenType == 0 && mStoryType == 0)
                {
                    free = false;
                }
            }
            return free;
        }
    }


    /// <summary>
    /// 跳过
    /// </summary>
    void OnSkipStory()
    {
        Panel_Map.GetComponent<GUIScaleMap>().EndScaleMap();
        TweenPosition tp = Sprite_Head.GetComponent<TweenPosition>();
        tp.enabled = false;
        mCurrent = mScriptList.Length;
        
        SetMapPosition();
        EndStory();
    }

    /// <summary>
    /// 0-正常 1-回顾
    /// </summary>
    public int mStoryType = 0;
    public EStoryIndex mReviewStory = EStoryIndex.Story_000;
    public void OnStartSelectStory()
    {
        mStoryType = 1;
        StartTalking((int)mReviewStory);
        mStoryState = 4;
        isStoryRun = true;

        HideStoryInfoRect();
        OOTools.OOTweenScale(IBtn_StoryBack.gameObject, Vector3.one, Vector3.zero);
        OOTools.OOTweenScale(IBtn_StorySkip.gameObject, Vector3.zero, Vector3.one);
    }


    /// <summary>
    /// 隐藏关卡详情
    /// </summary>
    void HideStoryInfoRect()
    {
        Story_InfoRect.transform.localPosition = new Vector3(-1000, -1000, 0);
    }

    /// <summary>
    /// 隐藏头像
    /// </summary>
    void HideHead()
    {
        iTween.Stop(Story_Left_Head);
        iTween.Stop(Story_Right_Head);

        Story_Left_Head.transform.localPosition = LF_Out;
        Story_Right_Head.transform.localPosition = RF_Out;
        Story_Right_Face.GetComponent<UITexture>().material.mainTexture = null;
        Story_Left_Face.GetComponent<UITexture>().material.mainTexture = null;

        Story_Left_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 0);
        Story_Right_Brow.GetComponent<UITexture>().color = new Color(1, 1, 1, 0);
        //RunScript("LFace");
        //RunScript("RFace");

        foreach (Transform trans in Panel_Talk.transform)
        {
            Destroy(trans.gameObject);
        }
    }





    /// <summary>
    /// 隐藏按钮
    /// </summary>
    void HideBtns()
    {
        IBtn_StorySkip.GetComponent<TweenScale>().enabled = false;
        IBtn_StoryBack.GetComponent<TweenScale>().enabled = false;

        IBtn_StorySkip.transform.localScale = Vector3.zero;
        IBtn_StoryBack.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// 返回//
    /// </summary>
    public void OnBack()
    {
        if(IBtn_StorySkip.gameObject.active)
        {
            OnSkipStory();
        }
        else if(IBtn_StoryBack.gameObject.active)
        {
            OnNext();
        }
    }
}
