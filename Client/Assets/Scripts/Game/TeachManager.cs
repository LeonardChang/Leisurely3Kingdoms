using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;


public class TeachManager : MonoBehaviour {

    public int mTeachStep = -1;
    public GameObject Teach_Talk;
    public GameObject Teach_Talk_2;
    public GameObject Teach_Talk_3;
    public GameObject Story_Left_Head;

    public GameObject Sprite_Arrow;
    public GameObject Sprite_Hight;

    public GameObject Teach_Collider;
    public GameObject Teach_Collider_Mid;

    public GameObject Teach_PopMenu;

    public GameObject Sprite_Hand;

    public bool isMachineBom = false;

    Vector3 LF_In = new Vector3(-250f, 70f, -1000f);
    Vector3 LF_Out = new Vector3(-720f, 70f, -1000f);

    public GameObject Teach_Info;
    public int Teach_HP = 12;
    string[] motion = new string[] { "DH_5guanggai1", "DH_5zhongzhi1", "DH_5jitan1" };


    
    bool isIni = false;
    float delay = 0;


    List<CHole> m_dustHoles;
    List<CZombie> mZombieList;
    int[] HoleList;
	// Use this for initialization





	void Start () 
    {
        Story_Left_Head.transform.FindChild("Story_Left_Face").GetComponent<UITexture>().material.mainTexture = ResourcePath.GetHeadPic(0);
        if (GameDataCenter.Instance.IsTeachMode)
        {
            mTeachStep = 0;
        }
        else
        {
            Destroy(gameObject);
        }
      
        mTeachStep = 0;

        mZombieList = new List<CZombie>();




        m_dustHoles = new List<CHole>();
        int iCount = 0;
        int[] Hole_Id = new int[] {28,25,22,20,18,16,14,12,4,0,8,5,1,9,6,2,10,7,3,11,13,15,17,19,21,23,24,26,27,29 };

        for (int i = 0; i < 10; i++)
        {
            m_dustHoles.Add(new CHole(-290f + i * 60f, -60, 110, iCount));
            iCount++;
            m_dustHoles.Add(new CHole(-290f + i * 60f, -170, 90, iCount));
            iCount++;
            m_dustHoles.Add(new CHole(-250f + i * 60f, -114, 100, iCount));
            iCount++;
        }

        for (int i = 0; i < 30; i++ )
        {
            m_dustHoles[i].Id = Hole_Id[i];
        }


        HoleList = new int[]{6, 2, 3, 4, 5, 0, 7, 8, 9, 10, 11, 1};

        int tmp_int;
        int first;
        int second;
        for(int i = 0; i < 10; i ++)
        {
            first = Random.Range(1, 12);
            second = Random.Range(1, 12);

            tmp_int = HoleList[first];
            HoleList[first] = HoleList[second];
            HoleList[second] = tmp_int;
        }
	}


    /// <summary>
    /// 创建新机器
    /// </summary>
    /// <returns></returns>
    public IEnumerator CreateNewMachine()
    {
        GameDataCenter.Instance.GuiManager.SA_GuanGai.transform.localScale = Vector3.zero;
        GameDataCenter.Instance.GuiManager.SA_JiQi.transform.localScale = Vector3.zero;
        GameDataCenter.Instance.GuiManager.SA_JiTan.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1f);
        iTween.ScaleTo(GameDataCenter.Instance.GuiManager.SA_GuanGai.gameObject, iTween.Hash("scale", Vector3.one,
                                                                            "time", 0.5,
                                                                            "islocal", true));

        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(GameDataCenter.Instance.GuiManager.SA_JiQi.gameObject, iTween.Hash("scale", Vector3.one,
                                                                            "time", 0.5,
                                                                            "islocal", true));

        yield return new WaitForSeconds(0.5f);
        iTween.ScaleTo(GameDataCenter.Instance.GuiManager.SA_JiTan.gameObject, iTween.Hash("scale", Vector3.one,
                                                                            "time", 0.5,
                                                                            "islocal", true));
    }

    /// <summary>
    /// 创建6只僵尸
    /// </summary>
    /// <returns></returns>
    public IEnumerator Create6Zombie()
    {
        for (int i = 1; i < 5; i ++ )
        {
            CreateOneZombie(i, ZombieType.Normal);
            yield return new WaitForSeconds(0.5f);
        } 
    }

    /// <summary>
    /// 创建12只僵尸
    /// </summary>
    /// <returns></returns>
    public IEnumerator CreateAllZombie()
    {
        CreateOneZombie(5, ZombieType.Zombie02);
        yield return new WaitForSeconds(0.5f);
        CreateOneZombie(6, ZombieType.Zombie02);
        yield return new WaitForSeconds(0.5f);
        for (int i = 7; i < 12; i++)
        {
            CreateOneZombie(i, ZombieType.Normal);
            yield return new WaitForSeconds(0.5f);
        }
    }

    /// <summary>
    /// 爆炸效果
    /// </summary>
    public void CreateBom()
    {
        GameObject obj = ResourcePath.Instance(EResourceIndex.Prefab_Fireeffect_1);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(480, 480, 1);
        obj.transform.localPosition = new Vector3(0, 200, 0);
        Destroy(obj, 4f);

        obj = ResourcePath.Instance(EResourceIndex.Prefab_Fireeffect_1);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(480, 480, 1);
        obj.transform.localPosition = new Vector3(-150, 200, 0);
        Destroy(obj, 4f);

        obj = ResourcePath.Instance(EResourceIndex.Prefab_Fireeffect_1);
        obj.transform.parent = transform;
        obj.transform.localScale = new Vector3(480, 480, 1);
        obj.transform.localPosition = new Vector3(150, 200, 0);
        Destroy(obj, 4f);

        ResourcePath.PlaySound(EResourceAudio.Audio_Explosion4);
        GameDataCenter.Instance.GuiManager.ShakeScreen();
    }

    /// <summary>
    /// 爆炸掉机器
    /// </summary>
    /// <returns></returns>
    public IEnumerator BomMachine()
    {
        yield return new WaitForSeconds(0.5f);
        CreateBom();
        isMachineBom = true;
    }

    /// <summary>
    /// 创建第一只僵尸
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="_type"></param>
    void CreateOneZombie(int _index, ZombieType _type)
    {
        CZombie zombie = new CZombie();
        zombie.SetData(5, HoleList[_index], 100);
        zombie.Type = _type;

        GameDataCenter.Instance.GuiManager.Zombie_Manager.CreateOneZombie(zombie);
    }

    /// <summary>
    /// 给僵尸添加养分，让其长出来
    /// </summary>
    void ZombieComeOut()
    {
        foreach (Transform trans in GameDataCenter.Instance.GuiManager.Zombie_Manager.transform)
        {
            if (trans.GetComponent<Zombie>())
            {
                trans.GetComponent<Zombie>().mZombie.Nutrient += 20;
            }
        }
    }


	// Update is called once per frame
	void Update () 
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
        }
        
        if(Input.GetKeyDown(KeyCode.S))
        {
            mTeachStep = 31;
            isIni = false;
        }
        switch(mTeachStep)
        {
            case 0:
                if(!isIni)
                {
                    isIni = true;
                    HeadIn();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step1));
                }
                break;
            case 1:
                if(!isIni)
                {
                    isIni = true;
                    HideTeachTalk();
                    ShowTeachTalk2(StringTable.GetString(EStringIndex.Teach_Step2), 0);
                }
                break;
            case 2:

                if(!isIni)
                {
                    isIni = true;
                    HideTeachTalk2();
                    HeadOut();


                    SetCollider(new Vector3(-188, 126, 0), new Vector3(250, 300, 1));
                    ShowTeachAim(new Vector3(-147, 100, 0), 0);
                    ShowTeachRect(new Vector3(-188, 126, 0), new Vector3(250, 300, 1));
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step3));
                }
                break;
            case 3:
                if(!isIni)
                {
                    isIni = true;
                    HideCollider();
                    HideTeachRect();
                    HideText();

                    iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_GuanGai.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);

                    OpenMenu(0);
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step4));
                    //ShowTeachRect()
                }
                break;
            case 4:
                //灌溉道具使用播放效果
                if(!isIni)
                {
                    isIni = true;
                    CloseMenu();

                    HideTeachRect();
                    //iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_GuanGai.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);
                    GameDataCenter.Instance.GuiManager.EffectUseItem(0, 1, ESceneItemDataType.KeepRun);
                    HideText();

                    delay = 1.5f;
                    OnNext();
                }
                break;
            case 5:
                if(!isIni && delay < 0)
                {
                    isIni = true;
                    HeadIn();
                    ShowTeachTalk3(0, 10);
                }
                break;
            case 6:
                if(!isIni)
                {
                    isIni = true;

                    HideTeachTalk3();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step6));
                }
                break;
            case 7:
                //长出僵尸
                if(!isIni)
                {
                    isIni = true;
                    CreateOneZombie(0, ZombieType.Normal);
                    HideTeachTalk();
                    HeadOut();

                    delay = 5;
                    OnNext();
                    ShowTeachAim(new Vector3(50, -20, 0), 3);
                }

                break;
            case 8:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HideTeachRect();
                    HeadIn();
                    ShowTeachTalk2(StringTable.GetString(EStringIndex.Teach_Step8), 1);
                }
                ZombieComeOut();

                break;
            case 9:
                if(!isIni)
                {
                    isIni = true;

                    HeadOut();
                    HideTeachTalk2();


                    SetCollider(new Vector3(30, 160, 0), new Vector3(250, 300, 1));
                    ShowTeachAim(new Vector3(100, 115, 0), 0);
                    ShowTeachRect(new Vector3(30, 160, 0), new Vector3(250, 300, 1));
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step9));
                }
                break;
            case 10:
                if(!isIni)
                {
                    isIni = true;

                    HideCollider();
                    HideTeachRect();
                    HideText();

                    iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_JiQi.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);
                    
                    OpenMenu(1);
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step10));
                }
                break;
            case 11:
                if(!isIni)
                {
                    isIni = true;

                    CloseMenu();

                    GameDataCenter.Instance.GuiManager.EffectUseItem(1, 1, ESceneItemDataType.KeepRun);
                    HideText();
                    HideTeachRect();

                    delay = 1.5f;
                    OnNext();
                }
                break;
            case 12:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HeadIn();
                    ShowTeachTalk3(0, 100);
                }
                break;
            case 13:
                //6只僵尸
                if(!isIni)
                {
                    isIni = true;

                    HeadOut();
                    HideTeachTalk3();


                    StartCoroutine(Create6Zombie());

                    delay = 5;
                    OnNext();
                }
                break;
            case 14:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HeadIn();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step13));
                }
                ZombieComeOut();

                break;
            case 15:
                if(!isIni)
                {
                    isIni = true;

                    HideTeachTalk();

                    ShowTeachTalk2(StringTable.GetString(EStringIndex.Teach_Step14), 2);
                }
                break;
            case 16:
                if(!isIni)
                {
                    isIni = true;

                    HideTeachTalk2();


                    SetCollider(new Vector3(210, 138, 0), new Vector3(200, 300, 1));
                    ShowTeachAim(new Vector3(185, 99, 0), 1);
                    ShowTeachRect(new Vector3(210, 138, 0), new Vector3(200, 300, 1));
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step15));
                }
                break;
            case 17:
                if(!isIni)
                {
                    isIni = true;
                    HideCollider();
                    HideTeachRect();


                    iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_JiTan.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);
                    OpenMenu(2);
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step16));
                }
                break;
            case 18:
                if(!isIni)
                {
                    isIni = true;

                    CloseMenu();
                    HideText();
                    HideTeachRect();
                    GameDataCenter.Instance.GuiManager.EffectUseItem(2, 1, ESceneItemDataType.KeepRun);

                    delay = 1.5f;
                    OnNext();

                }
                break;
            case 19:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HeadIn();
                    ShowTeachTalk3(0, 100);
                }
                break;
            case 20:
                //长满僵尸
                if(!isIni)
                {
                    isIni = true;
                    HideTeachTalk3();


                    StartCoroutine(CreateAllZombie());
                    

                    delay = 8;
                    HeadOut();
                    OnNext();
                }
                break;
            case 21:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HeadIn();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step19));
                }
                ZombieComeOut();
                break;
            case 22:
                if(!isIni)
                {
                    isIni = true;
                    HeadOut();
                    HideTeachTalk();


                    Teach_Collider_Mid.transform.localPosition = new Vector3(-5000, -5000, 0);
                    ShowText(StringTable.GetString(EStringIndex.Teach_Step20), new Vector3(0, 230, 0));
                    //ShowTeachAim(Vector3.zero,2);
                    ShowHand();
                    ShowTeachRect(new Vector3(0,  -60, 0), new Vector3(450, 300, 0));
                }

                if (GameDataCenter.Instance.GuiManager.Zombie_Manager.transform.childCount <= 0)
                {
                    OnNext();
                    delay = 5;
                }
                break;
            case 23:
                if(!isIni && delay < 0)
                {
                    isIni = true;
                    HideHand();
                    HideText();
                    HideTeachRect();

                    HeadIn();
                    ShowTeachTalk3(1, 10);

                }
                break;
            case 24:
                if(!isIni)
                {
                    isIni = true;

                    GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.ClosePanel();

                    HideTeachTalk3();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step22));
                }
                break;
            case 25:
                if(!isIni)
                {
                    isIni = true;

                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step23));
                }
                break;
            case 26:
                //爆炸
                if(!isIni)
                {
                    isIni = true;

                    StartCoroutine(BomMachine());

                    HeadOut();
                    HideTeachTalk();
                    delay = 3;
                    OnNext();
                }
                break;
            case 27:
                if(!isIni && delay < 0)
                {
                    isIni = true;

                    HeadIn();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step25));
                }
                break;
            case 28:
                if(!isIni)
                {
                    isIni = true;

                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_StepAdd));
                }
                break;
            
            case 29://低级机器出现
                if(!isIni)
                {
                    isIni = true;
                    HideTeachTalk();
                    HeadOut();

                    StartCoroutine(CreateNewMachine());
                    delay = 3;
                    OnNext();
                }
                break;

            case 30:
                if(!isIni && delay < 0)
                {
                    isIni = true;
                    OnNext();
                }
                break;
            case 31:
                if(!isIni)
                {
                    isIni = true;
                    HeadIn();
                    ShowTeachTalk(StringTable.GetString(EStringIndex.Teach_Step26));
                }
                break;
            case 32:
                if(!isIni)
                {
                    isIni = true;
                    HeadOut();
                    HideTeachTalk();

                    ResourcePath.PlaySound("GoodJob");
                    GameDataCenter.Instance.IsTeachMode = false;
                    GameDataCenter.Instance.Save();

                    GameDataCenter.Instance.GuiManager.CheckStory();
                    Destroy(gameObject);
                }
                break;
        }
	}





    /// <summary>
    /// 显示提示
    /// </summary>
    /// <param name="_str"></param>
    void ShowText(string _str)
    {
        Teach_Info.transform.FindChild("Label").GetComponent<UILabel>().text =  _str;
        Teach_Info.transform.localPosition = new Vector3(0, -173, -50);
    }

    /// <summary>
    /// 在某位置显示提示
    /// </summary>
    /// <param name="_str"></param>
    /// <param name="_pos"></param>
    void ShowText(string _str, Vector3 _pos)
    {
        Teach_Info.transform.FindChild("Label").GetComponent<UILabel>().text =  _str;
        Teach_Info.transform.localPosition = _pos;
    }

    /// <summary>
    /// 隐藏提示
    /// </summary>
    void HideText()
    {
        Teach_Info.transform.localPosition = new Vector3(1000, 1000, 0);
    }


    int[] ItemInfo = {20, 25, 30 };
    public UILabel Pop_Label_Info;
    public UILabel Pop_Sprite_Name;
    public UISprite Pop_Sprite_Icon;
    public UILabel Pop_Sprite_Title;
    bool isMenuOpen = false;
    /// <summary>
    /// 打开菜单
    /// </summary>
    /// <param name="_index"></param>
    void OpenMenu(int _index)
    {
        string[] ItemName = { StringTableUI.GetString(3), StringTableUI.GetString(4), StringTableUI.GetString(4) };
        string[] ItemIcon = { "MachineI_Icon_LvPing", "MachineP_Icon_JiaSu", "MachineA_Icon_LaZu" };
        string[] MachineName = { StringTableUI.GetString(1), StringTableUI.GetString(8), StringTableUI.GetString(13) };

        ResourcePath.PlaySound("Click");
        Teach_PopMenu.SetActiveRecursively(true);
        Pop_Label_Info.text = StringTable.GetString(ItemInfo[_index]);

        Pop_Sprite_Name.text = ItemName[_index];

        Pop_Sprite_Icon.spriteName = ItemIcon[_index];
        Pop_Sprite_Icon.MakePixelPerfect();

        Pop_Sprite_Title.text = MachineName[_index];

        OOTools.OOTweenPosition(Teach_PopMenu, new Vector3(0, 1000, 0), new Vector3(0, -100, 0));
        isMenuOpen = true;
    }

    /// <summary>
    /// 菜单完全关掉
    /// </summary>
   void MenuMotionEnd()
   {
       if(isMenuOpen)
       {
           ShowTeachRect(new Vector3(0, 109, 0), new Vector3(460, 160, 0));
           ShowTeachAim(new Vector3(60, 60, 0), 0);
       }
   }

    

    /// <summary>
    /// 关掉菜单
    /// </summary>
    void CloseMenu()
    {
        OOTools.OOTweenPosition(Teach_PopMenu, new Vector3(0, -100, 0), new Vector3(0, 1000, 0));
        isMenuOpen = false;
    }

    /// <summary>
    /// 隐藏碰撞盒
    /// </summary>
    void HideCollider()
    {
        Teach_Collider.transform.localPosition = new Vector3(-1000, -1000, 0);
    }

    /// <summary>
    /// 设置碰撞盒
    /// </summary>
    /// <param name="_pos">位置</param>
    /// <param name="_size">尺寸</param>
    void SetCollider(Vector3 _pos, Vector3 _size)
    {
        Teach_Collider.transform.localScale = _size;
        Teach_Collider.transform.localPosition = _pos;
    }

    /// <summary>
    /// 显示瞄准框
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_size"></param>
    void ShowTeachRect(Vector3 _pos, Vector3 _size)
    {
        Sprite_Hight.transform.localScale = _size;
        Sprite_Hight.transform.localPosition = _pos;
        //OOTools.OOTweenScale(Sprite_Hight, _size, _size + new Vector3(20, 20, 0));
    }

    /// <summary>
    /// 显示手势
    /// </summary>
    void ShowHand()
    {
        Sprite_Hand.GetComponent<TweenPosition>().Play(true);
    }

    /// <summary>
    /// 隐藏手势
    /// </summary>
    void HideHand()
    {
        Sprite_Hand.transform.localPosition = new Vector3(-1500, -146, 0);
        Sprite_Hand.GetComponent<TweenPosition>().enabled = false;
    }

    /// <summary>
    /// 显示箭头
    /// </summary>
    /// <param name="_pos"></param>
    /// <param name="_type"></param>
    void ShowTeachAim(Vector3 _pos, int _type)
    {
        switch(_type)
        {
            case 0:
                Sprite_Arrow.transform.localEulerAngles = new Vector3(0, 0, 235);
                Sprite_Arrow.transform.localScale = new Vector3(61, 82, 1);
                OOTools.OOTweenPosition(Sprite_Arrow, _pos, _pos + new Vector3(20, -20, 0));
                break;
            case 1:
                Sprite_Arrow.transform.localEulerAngles = new Vector3(0, 0, 135);
                Sprite_Arrow.transform.localScale = new Vector3(61, 82, 1);
                OOTools.OOTweenPosition(Sprite_Arrow, _pos, _pos + new Vector3(-20, -20, 0));
                break;
            case 2:
                Sprite_Arrow.transform.localEulerAngles = new Vector3(0, 0, 90);
                Sprite_Arrow.transform.localScale = new Vector3(100, 300, 1);
                Sprite_Arrow.transform.localPosition = new Vector3(10, -55, 0);
                break;
            case 3:
                Sprite_Arrow.transform.localEulerAngles = new Vector3(0, 0, 315);
                Sprite_Arrow.transform.localScale = new Vector3(61, 82, 1);
                OOTools.OOTweenPosition(Sprite_Arrow, _pos, _pos + new Vector3(20, 20, 0));
                break;
        }
    }

    /// <summary>
    /// 隐藏瞄准框
    /// </summary>
    void HideTeachRect()
    {
        Sprite_Arrow.transform.localPosition = new Vector3(-1000, -1000, 0);
        Sprite_Arrow.GetComponent<TweenPosition>().enabled = false;
        Sprite_Hight.transform.localPosition = new Vector3(-1000, -1000, 0);
    }


    /// <summary>
    /// 显示提三种对话框（奖励）
    /// </summary>
    string[] AwardString = new string[] { "[ffffff]ф[-]", "[ffffff]ж[-]" };
    void ShowTeachTalk3(int _type, int _value)
    {
        Teach_Talk_3.SetActiveRecursively(true);
        Teach_Talk_3.transform.localPosition = new Vector3(0, 0, 0);
        Teach_Talk_3.transform.FindChild("Label").GetComponent<UILabel>().text = StringTable.GetString(EStringIndex.Teach_AwardText) + _value.ToString() + AwardString[_type];
        if(StringTable.mStringType != ELocalizationTyp.English)
        {
            StartCoroutine(GetAward(_type, _value, Teach_Talk_3.transform.position + new Vector3(-0.1f, 0.32f, -6f)));
        }
        else
        {
            StartCoroutine(GetAward(_type, _value, Teach_Talk_3.transform.position + new Vector3(0.27f, 0.32f, -6f)));
        }
        

    }

    IEnumerator GetAward(int _type, int _value, Vector3 _pos)
    {
        yield return new WaitForSeconds(0.01f);

        GameObject obj;
        if (_type == 1)
        {
            obj = ResourcePath.Instance(EResourceIndex.Prefab_LoginGem);
        }
        else
        {
            obj = ResourcePath.Instance(EResourceIndex.Prefab_LoginMoney);
        }
        obj.layer = 12;
        obj.transform.parent = transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.position = _pos;
        obj.GetComponent<LoginMoney>().mValue = _value;
        obj.GetComponent<LoginMoney>().type = _type;
        
    }

    /// <summary>
    /// 隐藏第三种对话框
    /// </summary>
    void HideTeachTalk3()
    {
        Teach_Talk_3.transform.localPosition = new Vector3(-800, 0, 0);
        
    }

    /// <summary>
    /// 显示第二种对话框（带机器）
    /// </summary>
    void HideTeachTalk2()
    {
        Teach_Talk_2.transform.localPosition = new Vector3(-800, 0, 0);
    }

    /// <summary>
    /// 隐藏第二种对话框
    /// </summary>
    /// <param name="_str"></param>
    /// <param name="_index"></param>
    void ShowTeachTalk2(string _str, int _index)
    {
        Teach_Talk_2.SetActiveRecursively(true);
        Teach_Talk_2.transform.FindChild("Label").GetComponent<UILabel>().text = _str;
        Teach_Talk_2.transform.FindChild("SA_Machine").GetComponent<SpriteAnimation>().Play(motion[_index]);
        Teach_Talk_2.transform.localPosition = new Vector3(0, 0, 0);
    }


    /// <summary>
    /// 显示第一种框（纯文字）
    /// </summary>
    /// <param name="_str"></param>
    void ShowTeachTalk(string _str)
    {
        Teach_Talk.SetActiveRecursively(true);
        Teach_Talk.transform.FindChild("Label").GetComponent<UILabel>().text =  _str;
        Teach_Talk.transform.localPosition = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// 隐藏第一种对话框
    /// </summary>
    void HideTeachTalk()
    {
        Teach_Talk.transform.localPosition = new Vector3(-800, 0, 0);
    }


    /// <summary>
    /// 博士头像进
    /// </summary>
    void HeadIn()
    {
        iTween.MoveTo(Story_Left_Head, iTween.Hash("position", LF_In,
                                                    "time", 1,
                                                    "islocal", true
                                                    ));
    }

    /// <summary>
    /// 博士头像出去
    /// </summary>
    void HeadOut()
    {
        iTween.MoveTo(Story_Left_Head, iTween.Hash("position", LF_Out,
                                                    "time", 1,
                                                    "islocal", true
                                                    ));
    }

    /// <summary>
    /// 下一步
    /// </summary>
    void OnNext()
    {
        mTeachStep++;
        isIni = false;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.FinishTeach, mTeachStep.ToString());
    }
}
