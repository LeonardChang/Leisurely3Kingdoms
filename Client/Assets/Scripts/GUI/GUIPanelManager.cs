using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 面板管理
/// </summary>
public class GUIPanelManager : MonoBehaviour {
    public TweenPosition Panel_Shop;
    public TweenPosition Panel_CollectionAll;
    public TweenPosition Panel_Stage;
    public TweenPosition Panel_MiniMap;
    public TweenPosition Panel_Skill;
    public TweenPosition Panel_Task;
    public TweenPosition Panel_LaBa;

    public TweenPosition Panel_Irrigation;
    public TweenPosition Panel_Machine;
    public TweenPosition Panel_Altar;

    public TweenScale Panel_ZombieUp;

    public GUILoginManager Panel_Login;


    bool isAchieveOpen = false;
    bool isCollectionOpen = false;
    public bool isShopOpen = false;
    bool isSkillOpen = false;
    bool isStageOpen = false;
    bool isTaskOpen = false;
    bool isLaBaOpen = false;

    bool isIrrigationOpen = false;
    bool isMachineOpen = false;
    bool isAltarOpen = false;


    public GameObject sceneMask;
    //bool isStatic = true;
    public GameObject mainBtnPanel;

    public ZombieManager zombieManager;
    public GameObject currentScene;

    public AttackManager Panel_Attack;


    bool isIni = false;


    public UIImageButton IBtn_Theme;
    public UIImageButton IBtn_Collection;
    public UIImageButton IBtn_Shop;
    public UIImageButton IBtn_MiniMap;

    public GameObject Panel_Story;
    public GameObject SpriteSceneRender_Map;


    public GameObject GUI_Collider;

   
    /// <summary>
    /// 打开遮挡碰撞盒
    /// </summary>
    void SetCollider()
    {
        GUI_Collider.transform.localPosition = new Vector3(0, 0, -350);
    }

    /// <summary>
    /// 取消遮挡碰撞盒
    /// </summary>
    void UnSetCollider()
    {
        GUI_Collider.transform.localPosition = new Vector3(1200, 0, -350);
    }

    /// <summary>
    /// 打开地图
    /// </summary>
    void OpenByBtn()
    {
        //iTween.ShakePosition(IBtn_Theme.gameObject, );
        iTween.ShakePosition(IBtn_Theme.transform.FindChild("Background").gameObject, new Vector3(0.02f, 0, 0), 0.2f);
        Panel_Story.SetActiveRecursively(true);
        SpriteSceneRender_Map.SetActiveRecursively(true);
        Panel_Story.GetComponent<GUIStoryManager>().OpenByBtn();
    }

	// Use this for initialization
	void Start () 
    {
        //CheckLogin();
	}

    /// <summary>
    /// 检测登陆奖励
    /// </summary>
    public void CheckLogin()
    {
        GameDataCenter.Instance.EnterGame();
        CheckPopLogin();
    }

    /// <summary>
    /// 检测弹出登陆奖励
    /// </summary>
    public void CheckPopLogin()
    {

        if (Panel_Login.mIsOpen) return;

        if (GameDataCenter.Instance.GuiManager.mapPanel.IsOpen) return;

        if (!HasPanelOpen()) return;


        if (GameDataCenter.Instance.LoginAward && !GameDataCenter.Instance.IsTeachMode)
        {
            Panel_Login.gameObject.SetActiveRecursively(true);
            Panel_Login.transform.localPosition = new Vector3(0, 0, -750);
            GameDataCenter.Instance.LoginAward = false;
            Panel_Login.IniCard();
        }
    }

    /// <summary>
    /// 检测是否有面板打开
    /// </summary>
    /// <returns></returns>
    bool HasPanelOpen()
    {
        if (isCollectionOpen) return false;

        if (isShopOpen) return false;

        if (isStageOpen) return false;

        GameObject[] obj = GameObject.FindGameObjectsWithTag("PanelCollection");
        if (obj.Length > 0) return false;

        if (isAltarOpen) return false;
        if (isIrrigationOpen) return false;
        if (isMachineOpen) return false;
        if (isSkillOpen) return false;

        if (mIsZombieUpOpen) return false;
        if (GameDataCenter.Instance.GuiManager.isStory) return false;
        if (isTaskOpen) return false;


        return true;
    }

    /// <summary>
    /// 手势关闭
    /// </summary>
    void CheckHandClose()
    {
        if(OnCheckHandUp())
        {
            if (!isShopOpen && !isStageOpen && GameObject.FindGameObjectsWithTag("PanelCollection").Length <= 0)
            {
                CloseAllByHand();
            }
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {
            isIni = true;
        }

        CheckHandClose();

	}

    /// <summary>
    /// 打开技能面板
    /// </summary>
    public void OnSkill()
    {

        if(!isSkillOpen)
        {
            if(GameDataCenter.Instance.PlayerLevel < 10)
            {
                return;
            }
            GameDataCenter.Instance.GuiManager.DeletePopTipsSkill();
            Panel_Skill.gameObject.SetActiveRecursively(true);
            OOTools.OOTweenPosition(Panel_Skill.gameObject, new Vector3(-1500, 0, -2500), new Vector3(0, 0, -2500));
            isSkillOpen = true;
            SetCollider();
        }
    }

    /// <summary>
    /// 关闭技能面板
    /// </summary>
    public void OnSkillBack()
    {
        if (isSkillOpen)
        {
            OOTools.OOTweenPosition(Panel_Skill.gameObject, new Vector3(0, 0, -2500), new Vector3(-1500, 0, -2500));
            isSkillOpen = false;
            UnSetCollider();
        }
    }

    /// <summary>
    /// 技能面板完全关闭
    /// </summary>
    void OnSkillClose()
    {
        if (!isSkillOpen)
        {
            Panel_Skill.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 关闭所有面板
    /// </summary>
    public void CloseAll()
    {
        if(isCollectionOpen)
        {
            OnCollectionBack();
        }

        if(isShopOpen)
        {
            OnShopBack();
        }

        if(isStageOpen)
        {
            OnCloseStage();
        }


        GameDataCenter.Instance.GuiManager.OnCloseCollection();

        if(isAltarOpen)
        {
            OnCloseAltar();
        }
        if(isIrrigationOpen)
        {
            OnCloseIrrigation();
        }
        if(isMachineOpen)
        {
            OnCloseMachine();
        }
        if(isSkillOpen)
        {
            OnSkillBack();
        }

        if (mIsZombieUpOpen)
        {
            OnCloseZombieUp();
        }

        if(isTaskOpen)
        {
            OnTaskBack();
        }

        if(isLaBaOpen)
        {
            OnLaBaBack();
        }

        UnSetCollider();
    }

    /// <summary>
    /// 手势关闭所有面板
    /// </summary>
    public void CloseAllByHand()
    {
        if (isCollectionOpen)
        {
            OnCollectionBack();
        }

        if (isShopOpen)
        {
            OnShopBack();
        }

        if (isStageOpen)
        {
            OnCloseStage();
        }


        GameDataCenter.Instance.GuiManager.OnCloseCollection();

        if (isAltarOpen)
        {
            OnCloseAltar();
        }
        if (isIrrigationOpen)
        {
            OnCloseIrrigation();
        }
        if (isMachineOpen)
        {
            OnCloseMachine();
        }
        if (isSkillOpen)
        {
            OnSkillBack();
        }

        if (mIsZombieUpOpen)
        {
            OnCloseZombieUp();
        }

        UnSetCollider();
    }


    public void ForceCloseAll()
    {
        CloseAll();
        GameDataCenter.Instance.GuiManager.mapPanel.Close();
        Panel_Login.OnClose();
        //GameObject obj = GameObject.Find("Panel_StageInfo(Clone)");
        if(Panel_StageInfo)
        {
            Panel_StageInfo.SendMessage("OnClose");
        }
    }

    /// <summary>
    /// 打开陈列室
    /// </summary>
    void OnCollectionBtn()
    {
        if(isCollectionOpen)
        {
            return;
        }
        iTween.ShakePosition(IBtn_Collection.gameObject, new Vector3(0.02f, 0, 0), 0.2f);
        Panel_CollectionAll.gameObject.SetActiveRecursively(true);
        OOTools.OOTweenPosition(Panel_CollectionAll.gameObject, new Vector3(0, 1000, -450), new Vector3(0, 0, -450));
        isCollectionOpen = true;
        GameObject obj = GameObject.Find("__EffectNew");
        if (obj)
        {
            Destroy(obj);
        }
        SetCollider();
    }

    /// <summary>
    /// 陈列室关闭
    /// </summary>
    void OnCollectionBack()
    {
        OOTools.OOTweenPosition(Panel_CollectionAll.gameObject, new Vector3(0, 0, -450), new Vector3(0, 1000, -450));
        isCollectionOpen = false;
        UnSetCollider();
    }

    /// <summary>
    /// 陈列室完全关闭
    /// </summary>
    void OnCollectionClose()
    {
        if (!isCollectionOpen)
        {
            Panel_CollectionAll.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 弹出拉霸，关闭任务
    /// </summary>
    void OnLaBaBtn()
    {
        if(isLaBaOpen)
        {
            return;
        }
        Panel_LaBa.gameObject.SetActiveRecursively(true);
        OOTools.OOTweenPosition(Panel_LaBa.gameObject, new Vector3(0, 1000, -650), new Vector3(0, 0, -650));
        isLaBaOpen = true;
        OnTaskBack();

        SetCollider();

    }

    /// <summary>
    /// 关闭拉霸
    /// </summary>
    void OnLaBaBack()
    {
        OOTools.OOTweenPosition(Panel_LaBa.gameObject, new Vector3(0, 0, -650), new Vector3(0, 1000, -650));
        isLaBaOpen = false;
        UnSetCollider();
    }

    /// <summary>
    /// 拉霸完全关闭
    /// </summary>
    void OnLaBaClose()
    {
        if(!isLaBaOpen)
        {
            Panel_LaBa.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 打开任务列表
    /// </summary>
    void OnTaskBtn(GameObject obj)
    {
        if(isTaskOpen)
        {
            return;
        }
        iTween.ShakePosition(obj, new Vector3(0.01f, 0, 0), 0.5f);
        Panel_Task.gameObject.SetActiveRecursively(true);
        OOTools.OOTweenPosition(Panel_Task.gameObject, new Vector3(0, 1000, -650), new Vector3(0, 0, -650));
        isTaskOpen = true;
        SetCollider();
    }

    /// <summary>
    /// 关闭任务列表
    /// </summary>
    void OnTaskBack()
    {
        OOTools.OOTweenPosition(Panel_Task.gameObject, new Vector3(0, 0, -650), new Vector3(0, 1000, -650));
        isTaskOpen = false;
        UnSetCollider();
    }


    /// <summary>
    /// 任务列表完全关闭
    /// </summary>
    void OnTaskClose()
    {
        if(!isTaskOpen)
        {
            Panel_Task.gameObject.SetActiveRecursively(false);
        }
    }












    /// <summary>
    /// 打开商店
    /// </summary>
    public void OnShopBtn()
    {
        if(isShopOpen)
        {
            return;
        }
        GameDataCenter.Instance.GuiManager.DeleteNeedGem();
        iTween.ShakePosition(IBtn_Shop.gameObject, new Vector3(0.02f, 0, 0), 0.2f);
        Panel_Shop.gameObject.SetActiveRecursively(true);
        OOTools.OOTweenPosition(Panel_Shop.gameObject, new Vector3(0, 1000, -600), new Vector3(0, 0, -600));
        isShopOpen = true;
        SetCollider();
    }

    /// <summary>
    /// 关闭商店
    /// </summary>
    void OnShopBack()
    {
        OOTools.OOTweenPosition(Panel_Shop.gameObject, new Vector3(0, 0, -600), new Vector3(0, 1000, -600));
        isShopOpen = false;
        UnSetCollider();
    }

    /// <summary>
    /// 完全关闭商店
    /// </summary>
    void OnShopClose()
    {
        if (!isShopOpen)
        {
            Panel_Shop.gameObject.SetActiveRecursively(false);
        }
    }


    /// <summary>
    /// 打开战场
    /// </summary>
    void OnMiniMap()
    {
        if(Panel_Attack.isOpen)
        {
            Panel_Attack.ClosePanel();
        }
        else
        {
            Panel_Attack.OpenPanel();
        }
        iTween.ShakePosition(IBtn_MiniMap.gameObject, new Vector3(0.02f, 0, 0), 0.2f);
    }

    /// <summary>
    /// 
    /// </summary>
    void OnAchieveClose()
    {
        if(!isAchieveOpen)
        {
            Panel_Shop.gameObject.SetActiveRecursively(false);
        }
    }


    int[] mMachineY = new int[] { -200, -100, -100, -100, -50, 0, 0 };
    int IrrigationY;
    int MachineY;
    int AltarY;
    /// <summary>
    /// 打开灌溉面板
    /// </summary>
    void OnOpenIrrigation()
    {

        iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_GuanGai.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);

        Panel_Irrigation.gameObject.SetActiveRecursively(true);
        IrrigationY = mMachineY[Panel_Irrigation.GetComponent<GUIMachineManager>().GetBtnCount()];
        OOTools.OOTweenPosition(Panel_Irrigation.gameObject, new Vector3(0, 1000, -800), new Vector3(0, IrrigationY, -800), 0.2f);
        isIrrigationOpen = true;

        OnCloseMachine();
        OnCloseAltar();
        SetCollider();
    }

    /// <summary>
    /// 关闭灌溉面板
    /// </summary>
    void OnCloseIrrigation()
    {
        OOTools.OOTweenPosition(Panel_Irrigation.gameObject, new Vector3(0, IrrigationY, -800), new Vector3(0, 1000, -800), 0.2f);
        isIrrigationOpen = false;
        UnSetCollider();
    }

    /// <summary>
    /// 完全关闭灌溉面板
    /// </summary>
    void OnEndIrrigation()
    {
        if(!isIrrigationOpen)
        {
            Panel_Irrigation.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 打开机器面板
    /// </summary>
    void OnOpenMachine()
    {

        iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_JiQi.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);

        Panel_Machine.gameObject.SetActiveRecursively(true);
        MachineY = mMachineY[Panel_Machine.GetComponent<GUIMachineManager>().GetBtnCount()];
        OOTools.OOTweenPosition(Panel_Machine.gameObject, new Vector3(0, 1000, -800), new Vector3(0, MachineY, -800), 0.2f);
        isMachineOpen = true;

        OnCloseIrrigation();
        OnCloseAltar();
        SetCollider();
    }
    /// <summary>
    /// 关闭机器面板
    /// </summary>
    void OnCloseMachine()
    {
        OOTools.OOTweenPosition(Panel_Machine.gameObject, new Vector3(0, MachineY, -800), new Vector3(0, 1000, -800), 0.2f);
        isMachineOpen = false;
        UnSetCollider();
    }
    /// <summary>
    /// 完全关闭机器面板
    /// </summary>
    void OnEndMachine()
    {
        if (!isMachineOpen)
        {
            Panel_Machine.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 打开祭坛面板
    /// </summary>
    void OnOpenAltar()
    {

        iTween.ShakePosition(GameDataCenter.Instance.GuiManager.SA_JiTan.gameObject, new Vector3(0.01f, 0.01f, 0), 0.5f);

        Panel_Altar.gameObject.SetActiveRecursively(true);
        AltarY = mMachineY[Panel_Altar.GetComponent<GUIMachineManager>().GetBtnCount()];
        OOTools.OOTweenPosition(Panel_Altar.gameObject, new Vector3(0, 1000, -800), new Vector3(0, AltarY, -800), 0.2f);
        isAltarOpen = true;

        OnCloseMachine();
        OnCloseIrrigation();
        SetCollider();
    }
    /// <summary>
    /// 关闭祭坛面板
    /// </summary>
    void OnCloseAltar()
    {
        OOTools.OOTweenPosition(Panel_Altar.gameObject, new Vector3(0, AltarY, -800), new Vector3(0, 1000, -800), 0.2f);
        isAltarOpen = false;
        UnSetCollider();
    }
    /// <summary>
    /// 完全关闭祭坛面板
    /// </summary>
    void OnEndAltar()
    {
        if (!isAltarOpen)
        {
            Panel_Altar.gameObject.SetActiveRecursively(false);
        }
    }



    /// <summary>
    /// 打开主题面板
    /// </summary>
    void OnOpenStage()
    {
        if(!isStageOpen)
        {
            GameDataCenter.Instance.GuiManager.DeleteTipsSeeStage();
            Panel_Stage.gameObject.SetActiveRecursively(true);
            OOTools.OOTweenPosition(Panel_Stage.gameObject, new Vector3(0, 1000, -1200), new Vector3(0, 0, -600));
            isStageOpen = true;

        }
    }

    /// <summary>
    /// 关闭主题面板
    /// </summary>
    void OnCloseStage()
    {
        isStageOpen = false;
        OOTools.OOTweenPosition(Panel_Stage.gameObject, new Vector3(0, 0, -1200), new Vector3(0, 1000, -600));
    }

    /// <summary>
    /// 完全关闭主题面板
    /// </summary>
    void OnEndStage()
    {
        if(!isStageOpen)
        {
            Panel_Stage.gameObject.SetActiveRecursively(false);
        }
    }

    /// <summary>
    /// 响应点击主题面板《死亡岛》按钮
    /// </summary>
    public void OnScene1()
    {
        if (GameDataCenter.Instance.mCurrentScene == 0)
        {
            return;
        }
        LoadStage(0);
    }


    string GetPriceString(CPrice _price)
    {
        string coin = StringTable.GetString(EStringIndex.Tips_Coin);
        string gem = StringTable.GetString(EStringIndex.Tips_Gem);
        string price = _price.Type == EPriceType.Coin ? "[0000FF]" + _price.Value.ToString() + "[-]" + coin : "[0000FF]" + _price.Value.ToString() + "[-]" + gem;
        return price;
    }

    /// <summary>
    /// 获取购买主题提示信息
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    string GetSceneTips(int _index)
    {
        CPrice[] scene_prices = new CPrice[]
        {
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland2),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland3),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland4),
            GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland5),
        };
        return string.Format(StringTable.GetString(EStringIndex.Tips_BuyStage), GetPriceString(scene_prices[_index]));
    }



    /// <summary>
    /// 确认购买第二主题
    /// </summary>
    /// <param name="_str"></param>
    void OnChangeScene2(string _str)
    {
        MessageBoxSpecial.alterButtonClickedEvent -= OnChangeScene2;
        if(_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if(!GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland2)))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }

            GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.ForceKillAllZombie();

            GameDataCenter.Instance.AchieveBuyIsland();
            GameDataCenter.Instance.mScenes[1].IsOpen = true;

            Panel_StageInfo.SendMessage("OnClose");
            LoadStage(1);
        }
    }

    /// <summary>
    /// 重新载入场景，进入主题
    /// </summary>
    /// <param name="_stage">要载入的主题</param>
    void LoadStage(int _stage)
    {
        
        if (GameDataCenter.Instance.GuiManager.isStory)
            return;
        
        GameDataCenter.Instance.GuiManager.mIsLoading = true;

        GameDataCenter.Instance.GuiManager.StopNextStory();

        
        GameDataCenter.Instance.ChangeTargetScene = _stage;
        GameDataCenter.Instance.Save();
        GlobalModule.Instance.LoadSceneN("GameMain");   
    }


    GameObject Panel_StageInfo;
    /// <summary>
    /// 响应点击主题面板《冰冻岛》按钮
    /// </summary>
    public void OnScene2()
    {
        mBuyingStage = 1;
        if(GameDataCenter.Instance.mCurrentScene == 1)
        {
            return;
        }

        if(GameDataCenter.Instance.mScenes[1].IsOpen)
        {
            LoadStage(1);
        }
        else
        {
            if(KOZNet.IsDebugVersion)
            {
                GameDataCenter.Instance.mScenes[1].IsOpen = true;
                LoadStage(1);
                return;
            }


            if (!GlobalStaticData.GetStageInfo(1).CanOpen)
            {
                GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_NeedLevel));
                return;
            }
            GameDataCenter.Instance.mScenes[1].IsOpen = true;
            LoadStage(1);

            /*
            if(!Panel_StageInfo)
            {
                Panel_StageInfo = ResourcePath.Instance(EResourceIndex.Prefab_Panel_StageInfo);
                Panel_StageInfo.transform.parent = transform;
            }

            Panel_StageInfo.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Panel_StageInfo.transform.localPosition = new Vector3(0, 0, -3000);
            OOTools.OOTweenScale(Panel_StageInfo, new Vector3(0.1f, 0.1f, 0.1f), Vector3.one);
             */
        }
             
    }






    /// <summary>
    /// 购买第2主题按钮响应
    /// </summary>
    public void OnScene2Change()
    {
        if (GameDataCenter.Instance.mCurrentScene == 1)
        {
            return;
        }

        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetSceneTips(0),
                    StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));

        MessageBoxSpecial.alterButtonClickedEvent += OnChangeScene2;
        
    }



    /// <summary>
    /// 确认购买第二主题
    /// </summary>
    /// <param name="_str"></param>
    void OnChangeScene3(string _str)
    {

        MessageBoxSpecial.alterButtonClickedEvent -= OnChangeScene3;
        if (_str == StringTable.GetString(EStringIndex.Tips_OK))
        {
            if (!GameDataCenter.Instance.DeductionPrice(GlobalStaticData.GetPriceInfo(EPriceIndex.PriceIsland3)))
            {
                GameDataCenter.Instance.GuiManager.MsgBoxNeedMoney();
                return;
            }

            GameDataCenter.Instance.GuiManager.Panel_Manager.Panel_Attack.ForceKillAllZombie();

            GameDataCenter.Instance.AchieveBuyIsland();
            GameDataCenter.Instance.mScenes[2].IsOpen = true;

            Panel_StageInfo.SendMessage("OnClose");
            LoadStage(2);
        }
    }


    /// <summary>
    /// 购买第3主题按钮响应
    /// </summary>
    public void OnScene3Change()
    {
        if (GameDataCenter.Instance.mCurrentScene == 2)
        {
            return;
        }

        GlobalModule.Instance.ShowMessageBoxQuestion(StringTable.GetString(EStringIndex.Tips_TitleTips), GetSceneTips(1),
                    StringTable.GetString(EStringIndex.Tips_OK), StringTable.GetString(EStringIndex.Tips_Cancel));

        MessageBoxSpecial.alterButtonClickedEvent += OnChangeScene3;

    }

    public int mBuyingStage = 0;
    /// <summary>
    /// 响应点击主题面板《神秘岛》按钮
    /// </summary>
    public void OnScene3()
    {
        Debug.Log("OnScene3");
        mBuyingStage = 2;
        if (GameDataCenter.Instance.mCurrentScene == 2)
        {
            return;
        }

        if (GameDataCenter.Instance.mScenes[2].IsOpen)
        {
            LoadStage(2);
        }
        else
        {
            if (KOZNet.IsDebugVersion)
            {
                GameDataCenter.Instance.mScenes[2].IsOpen = true;
                LoadStage(2);
                return;
            }


            if (!GlobalStaticData.GetStageInfo(2).CanOpen)
            {
                GameDataCenter.Instance.GuiManager.MsgBox(StringTable.GetString(EStringIndex.Tips_NeedLevel));
                return;
            }

            GameDataCenter.Instance.mScenes[2].IsOpen = true;
            LoadStage(2);

        }
             
    }



    public void OnScene4()
    {

    }

    public void OnScene5()
    {

    }

    /// <summary>
    /// 检测手势
    /// </summary>
    /// <returns></returns>
    bool OnCheckHandUp()
    {
        bool ret = false;
        if (Application.platform == RuntimePlatform.IPhonePlayer
                 || Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

                if(touchDeltaPosition.y > Screen.height / 12)
                {
                    ret = true;
                }
            }
        }
        else
        {
            if(Input.GetAxis("Mouse Y") >= 0.5f && Input.GetMouseButton(0))
            {
                ret = true;
            }
        }

        return ret;

    }


    bool mIsZombieUpOpen = false;
    void OnOpenZombieUp()
    {
        OOTools.OOTweenScale(Panel_ZombieUp.gameObject, new Vector3(0.1f, 0.1f, 1), new Vector3(1, 1, 1));
        GameObject.Find("GUIMask").transform.localPosition = new Vector3(150, 0, -990);
        mIsZombieUpOpen = true;
    }

    public void OnCloseZombieUp()
    {
        OOTools.OOTweenScale(Panel_ZombieUp.gameObject, new Vector3(1f, 1f, 1), new Vector3(0.01f, 0.1f, 1));
        if (GameObject.Find("GUIMask").transform.localPosition.x == 150)
            GameObject.Find("GUIMask").transform.localPosition = new Vector3(150, 0, -520);

        mIsZombieUpOpen = false;
    }

    void OnCloseZombieUpEnd()
    {
        if (!mIsZombieUpOpen)
            Panel_ZombieUp.gameObject.SetActiveRecursively(false);
    }

    public void OnOpenZombieUpProgress(ZombieType _type,  int _add_type)
    {
        Panel_ZombieUp.gameObject.SetActiveRecursively(true);
        OnOpenZombieUp();
        Panel_ZombieUp.GetComponent<GUIPanelZombieUp>().mZombieType = _type;
        Panel_ZombieUp.GetComponent<GUIPanelZombieUp>().mAddType = _add_type;
    }


    /// <summary>
    /// 打开成就
    /// </summary>
    public void OnAchieve(GameObject obj)
    {
        iTween.ShakePosition(obj, new Vector3(0.01f, 0, 0), 0.5f);

        TitleManager.ClickAchieve++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickAchievement, TitleManager.ClickAchieve.ToString());
        GlobalModule.Instance.OpenGameCenterAchievement();
    }

    /// <summary>
    /// 打开排行
    /// </summary>
    public void OnRank(GameObject obj)
    {
        iTween.ShakePosition(obj, new Vector3(0.01f, 0, 0), 0.5f);

        TitleManager.ClickRank++;
        GlobalModule.Instance.SendClientMessage(ClientMessageEnum.ClickLeaderboard, TitleManager.ClickRank.ToString());
        GlobalModule.Instance.OpenGameCenterLeaderboard();
    }
}
