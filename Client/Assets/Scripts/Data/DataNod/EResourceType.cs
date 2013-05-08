using UnityEngine;
using System.Collections;
using EasyMotion2D;
using System.Collections.Generic;

/// <summary>
/// 预设枚举
/// </summary>
public enum EResourceIndex
{
    Prefab_Panel_CollectionOne = 0,
    
    Prefab_Effect_LvUP,
    Prefab_Effect_New,
    Prefab_Effect_Upgrade,
    Prefab_Effect_UseItem,

    Prefab_FlyMoney,
    Prefab_Scene1,

    Prefab_Attack_Bom,
    Prefab_Attack_Zombie,
    Prefab_Attack_Effect1,
    Prefab_Attack_Effect2,
    Prefab_Attack_FlyGem,
    Prefab_Attack_Arrow,
    Prefab_Fireeffect_1,
    
    Prefab_AttackBomb,
    Prefab_Art_UpGrade,

    Prefab_LoginMoney,
    Prefab_LoginGem,
    Prefab_FlyAward,

    Prefab_ZombieTalk,
    Prefab_TeachZombie,
    Prefab_Star,
    Prefab_Keng,
    Prefab_Win,
    Prefab_Win_EN,
    Prefab_Wini_TW,

    Prefab_Rain,
    Prefab_Snow,
    Prefab_PopTips,
    Prefab_Panel_StageInfo,

    Prefab_Effect_Skill_1,
    Prefab_Effect_Skill_2,
    Prefab_Effect_Skill_3,
    Prefab_Effect_Skill_4,

    Prefab_Attack_Soldier,
    Prefab_Fly_Star,
    Prefab_Fly_Label,
    Prefab_BomFlash,
    Prefab_DraggingHand,
    Prefab_FlyingBlood,
    Prefab_KillKing,
    Prefab_FlyExperience,
}

/// <summary>
/// 音效枚举
/// </summary>
public enum EResourceAudio
{
    Audio_ClickThief = 0,
    Audio_Hand,
    Audio_ItemFly,
    Audio_ItemHelp,
    Audio_LevelUp1,
    Audio_LevelUp2,
    Audio_LevelUp3,
    Audio_Page,
    Audio_StartWork,
    Audio_UseSkill,
    Audio_Explosion2,
    Audio_Attack,
    Audio_Money,
    Audio_se004,
    Audio_se005,

    Audio_Money2,
    Audio_Money3,
    Audio_Money4,
    Audio_Money5,
    Audio_Money6,
    Audio_Money7,
    Audio_Rain,
    Audio_Explosion3,
    Audio_Explosion4,
    Audio_TalkBox,
    Audio_Thunder,
    Audio_Clap,
    Audio_Win,
    Audio_Laugh,
    Audio_ZombieTalk,
    Audio_ThiefClick,
    Audio_EnemyDie,
    Audio_KillKing,
    Audio_TouchFlyItem,
}

/// <summary>
/// 博士头像
/// </summary>
public enum EResourceTexture
{
    MainChar_normal,
    MainChar_money,
    MainChar_sad,
    MainChar_smile,
}


public class ResourcePath:MonoBehaviour 
{
    /// <summary>
    /// 博士头像
    /// </summary>
    static string[] Pic_List = new string[]
    {
        "GUI/Main/Face/MainChar_normal",
        "GUI/Main/Face/MainChar_money",
        "GUI/Main/Face/MainChar_sad",
        "GUI/Main/Face/MainChar_smile",

    };

    /// <summary>
    /// 预设
    /// </summary>
    static string[] Path_List = new string[]{
        "Prefabs/GUI/Panel_CollectionOne",

        "Prefabs/Game/Effect/Effect_LvUp",
        "Prefabs/Game/Effect/Effect_New",
        "Prefabs/Game/Effect/Effect_Upgrade",
        "Prefabs/Game/Effect/Effect_UseItem",

        "Prefabs/Game/AttackEffect/FlyMoney",
        "Prefabs/Game/Scene/Scene1",

        "Prefabs/Game/AttackEffect/Attack_Bom",
        "Prefabs/Game/Game/ZombieAttack",
        "Prefabs/Game/AttackEffect/Attack_Effect1",
        "Prefabs/Game/AttackEffect/Attack_Effect2",
        "Prefabs/Game/AttackEffect/FlyGem",
        "Prefabs/Game/AttackEffect/Attack_Arrow",
        "Prefabs/Game/AttackEffect/FireEffect_1",

        "Prefabs/Game/AttackEffect/AttackBomb",
        "Prefabs/Game/Effect/UpGrade",

        "Prefabs/GUI/Login/LoginMoney",
        "Prefabs/GUI/Login/LoginGem",
        "Prefabs/Game/AttackEffect/FlyAward",

        "Prefabs/Game/Game/ZombieTalk",
        "Prefabs/Game/Game/TeachZombie",
        "Prefabs/Game/Effect/Star",
        "Prefabs/Game/Effect/Keng",
        "Prefabs/Game/Effect/Win",
        "Prefabs/Game/Effect/Win_EN",
        "Prefabs/Game/Effect/Win_TW",

        "Prefabs/Weather/Rain",
        "Prefabs/Weather/Snow",
        "Prefabs/Game/Effect/PopTips",
        "Prefabs/GUI/Panel_StageInfo",

        "Prefabs/Game/Effect/Effect_Skill_1",
        "Prefabs/Game/Effect/Effect_Skill_2",
        "Prefabs/Game/Effect/Effect_Skill_3",
        "Prefabs/Game/Effect/Effect_Skill_4",

        "Prefabs/Game/Game/Attack_Soldier",
        "Prefabs/Game/Game/FlyingStar",
        "Prefabs/Game/Effect/Fly_Label",
        "Prefabs/Game/Effect/BomFlash",
        "Prefabs/Game/Effect/Sprite_DragingHand",
        "Prefabs/Game/Game/Flyingblood",
        "Prefabs/Game/Effect/ALL_D_NEW",
        "Prefabs/Game/AttackEffect/FlyExperience"
    };

    /// <summary>
    /// 音效
    /// </summary>
    static string[] Sound_List = new string[]
    {
        "Sound/ClickThief",
        "Sound/Hand",
        "Sound/ItemFly",
        "Sound/ItemHelp",
        "Sound/LevelUp1",
        "Sound/LevelUp2",
        "Sound/LevelUp3",
        "Sound/Page",
        "Sound/StartWork",
        "Sound/UseSkill",
        "Sound/Explosion2",
        "Sound/Attack",
        "Sound/Money",
        "Sound/se004",
        "Sound/se005",

        "Sound/Money2",
        "Sound/Money3",
        "Sound/Money4",
        "Sound/Money5",
        "Sound/Money6",
        "Sound/Money7",
        "Sound/Rain",
        "Sound/Explosion3",
        "Sound/Explosion4",
        "Sound/TalkBox",
        "Sound/Thunder",
        "Sound/Clap",
        "Sound/Win",
        "Sound/Laugh",
        "Sound/ZombieTalk",
        "Sound/ThiefClick",
        "Sound/EnemyDie",
        "Sound/KillKing",
        "Sound/TouchFlyItem"
    };
   
    /// <summary>
    /// 剧情头像
    /// </summary>
    static string[] Head_List = new string[]
    {
        "GUI/Maps/Head/head0",
        "GUI/Maps/Head/head11",
        "GUI/Maps/Head/head12",
        "GUI/Maps/Head/head13",
        "GUI/Maps/Head/head14",
        "GUI/Maps/Head/head21",
        "GUI/Maps/Head/head22",
        "GUI/Maps/Head/head23",
        "GUI/Maps/Head/head31",
        "GUI/Maps/Head/head32",
        "GUI/Maps/Head/head33",
        "GUI/Maps/Head/head34"
    };

    /// <summary>
    /// 剧情表情
    /// </summary>
    static string[] Face_List = new string[]
    {
        "GUI/Maps/Face/Face_0",
        "GUI/Maps/Face/Face_1",
        "GUI/Maps/Face/Face_2",
        "GUI/Maps/Face/head11-1",
        "GUI/Maps/Face/head12-1",
        "GUI/Maps/Face/Face_5",
        "GUI/Maps/Face/Face_6"
    };

    /// <summary>
    /// 攻击背景
    /// </summary>
    static string[] AttackBG = new string[]
    {
        "GUI/AttackBG/Attack_Lv1_n1",//0
        "GUI/AttackBG/Attack_Lv1_n2",
        "GUI/AttackBG/Attack_Lv1_01",
        "GUI/AttackBG/Attack_Lv1_06",
        "GUI/AttackBG/Attack_Lv1_10",
        "GUI/AttackBG/Attack_Lv1_14",
        "GUI/AttackBG/Attack_Lv1_17",
        "GUI/AttackBG/Attack_Lv1_19",
        "GUI/AttackBG/Attack_Lv1_20",
        "GUI/AttackBG/Attack_Lv1_21",

        "GUI/AttackBG/Attack_Lv1_n1",//10

        "GUI/AttackBG/Attack_Lv2_n1",//11
        "GUI/AttackBG/Attack_Lv2_n2",
        "GUI/AttackBG/Attack_Lv2_01",
        "GUI/AttackBG/Attack_Lv2_02",
        "GUI/AttackBG/Attack_Lv2_03",
        "GUI/AttackBG/Attack_Lv2_04",
        "GUI/AttackBG/Attack_Lv2_05",
        "GUI/AttackBG/Attack_Lv2_06",
        "GUI/AttackBG/Attack_Lv2_07",
        "GUI/AttackBG/Attack_Lv2_08",

        "GUI/AttackBG/Attack_Lv2_n1",//21

        "GUI/AttackBG/Attack_Add_1",//22
        "GUI/AttackBG/Attack_Add_2",
        "GUI/AttackBG/Attack_Add_3",
        "GUI/AttackBG/Attack_Add_4",
        "GUI/AttackBG/Attack_Add_5",
        "GUI/AttackBG/Attack_Add_6",

        "GUI/AttackBG/Attack_28", //28
        "GUI/AttackBG/Attack_29",
        "GUI/AttackBG/Attack_30",
        "GUI/AttackBG/Attack_31",
        "GUI/AttackBG/Attack_32",
        "GUI/AttackBG/Attack_33",
        "GUI/AttackBG/Attack_34",
        "GUI/AttackBG/Attack_35",
        "GUI/AttackBG/Attack_36",
        "GUI/AttackBG/Attack_37",
        "GUI/AttackBG/Attack_38",
        "GUI/AttackBG/Attack_39",
        "GUI/AttackBG/Attack_40",
    };

    /// <summary>
    /// 攻击目标
    /// </summary>
    static string[] AttackTarget = new string[]
    {
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_01",//0
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_02",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_03",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_04",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_05",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_06",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_07",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_08",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_09",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_10",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_11",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_12",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_13",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_14",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_15",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_16",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_17",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv1_18",

        "Prefabs/GUI/AttackTarget/Attack_Target_Chest",//18

        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_01",//19
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_02",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_03",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_04",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_05",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_06",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_07",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_08",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_09",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_10",
        "Prefabs/GUI/AttackTarget/Attack_Target_Lv2_11",

        "Prefabs/GUI/AttackTarget/Attack_Target_Chest",//30
        "Prefabs/GUI/AttackTarget/Attack_Target_31",
        "Prefabs/GUI/AttackTarget/Attack_Target_32",
        "Prefabs/GUI/AttackTarget/Attack_Target_33",
        "Prefabs/GUI/AttackTarget/Attack_Target_34",
        "Prefabs/GUI/AttackTarget/Attack_Target_35",
        "Prefabs/GUI/AttackTarget/Attack_Target_36",
        "Prefabs/GUI/AttackTarget/Attack_Target_37",
        "Prefabs/GUI/AttackTarget/Attack_Target_38",
        "Prefabs/GUI/AttackTarget/Attack_Target_39",
        "Prefabs/GUI/AttackTarget/Attack_Target_40",
        "Prefabs/GUI/AttackTarget/Attack_Target_41",
        "Prefabs/GUI/AttackTarget/Attack_Target_42",
        "Prefabs/GUI/AttackTarget/Attack_Target_43",
        "Prefabs/GUI/AttackTarget/Attack_Target_44",
    };

    /// <summary>
    /// 兵种
    /// </summary>
    static string[] SoldierList = new string[]
    {
        "Prefabs/Game/AttackSoldier/Attack_Soldier_1",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_2",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_3",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_4",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_5",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_6",
        "Prefabs/Game/AttackSoldier/Attack_Soldier_7",
    };


    public static string GetPath(EResourceIndex _index)
    {
        return (Path_List[(int)_index]);
    }

    /// <summary>
    /// 实例化一个预设
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static GameObject Instance(EResourceIndex _index)
    {
        switch (_index)
        {
            case EResourceIndex.Prefab_Fly_Label:
                {
                    PerfabsPool pool = GlobalModule.Instance.Pool;
                    return pool.CreateObject(ResourcePath.GetPath(_index), "FlyLabel");
                }
                break;
            case EResourceIndex.Prefab_FlyExperience:
                {
                    PerfabsPool pool = GlobalModule.Instance.Pool;
                    return pool.CreateObject(ResourcePath.GetPath(_index), "FlyExperience");
                }
                break;
            case EResourceIndex.Prefab_FlyMoney:
                {
                    PerfabsPool pool = GlobalModule.Instance.Pool;
                    return pool.CreateObject(ResourcePath.GetPath(_index), "FlyMoney");
                }
                break;
        }

        GameObject obj = Instantiate(GlobalModule.Instance.LoadResource(ResourcePath.GetPath(_index))) as GameObject;
        return obj;
    }

    /// <summary>
    /// 实例化一个预设
    /// </summary>
    /// <param name="path">名字</param>
    /// <returns></returns>
    public static GameObject Instance(string path)
    {
        GameObject obj = (GameObject)Instantiate(GlobalModule.Instance.LoadResource("Prefabs/" + path));
        return obj;

        //PerfabsPool pool = GlobalModule.Instance.Pool;
        //return pool.CreateObject("Prefabs/" + path, "Prefabs/" + path);
    }

    /// <summary>
    /// 获取指定僵尸（用于获取动画）
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static GameObject GetZombie(ZombieType _type)
    {
        if (!ZombieDataManager.IsZombieOpen(_type))
        {
            _type = ZombieType.Normal;
        }

        string path = "Prefabs/Game/ZombieType/Zombie_" + string.Format("{0:D2}",  (int)_type);
        return GlobalModule.Instance.LoadResource(path) as GameObject;
    }

    /// <summary>
    /// 加载指定僵尸站立动画
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    private static SpriteAnimationClip LoadStandZombie(ZombieType _type)
    {
        if (!ZombieDataManager.IsZombieOpen(_type))
        {
            _type = ZombieType.Normal;
        }
        string path = "Prefabs/Game/ZombieTypeStand/Zombie_" + string.Format("{0:D2}", (int)_type);
        GameObject obj = GlobalModule.Instance.LoadResource(path) as GameObject;

        ZombieMotion motion = obj.GetComponent<ZombieMotion>();
        SpriteAnimationClip sac = (SpriteAnimationClip)Instantiate(motion.Stand1);
        return sac;
    }

    /// <summary>
    /// 获取指定僵尸站立动画（陈列室详情）
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static SpriteAnimationClip GetZombieStand(ZombieType _type)
    {
        if (!ZombieDataManager.IsZombieOpen(_type))
        {
            _type = ZombieType.Normal;
        }
        
        return LoadStandZombie(_type);
    }



    public static Material[] ZombiePic;
    public static bool isLoad = false;
    /// <summary>
    /// 获取僵尸静态图（陈列室）
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static Material GetZombiePic(ZombieType _type)
    {
        if(!isLoad)
        {
            isLoad = true;
            ZombiePic = new Material[] 
            { 
                GlobalModule.Instance.LoadResource("GUI/ZombieStand/Zombietexture/Zombie_1") as Material,
                GlobalModule.Instance.LoadResource("GUI/ZombieStand/Zombietexture/Zombie_2") as Material,
                GlobalModule.Instance.LoadResource("GUI/ZombieStand/Zombietexture/Zombie_3") as Material,
                GlobalModule.Instance.LoadResource("GUI/ZombieStand/Zombietexture/Zombie_4") as Material,
            };
        }
        if (!ZombieDataManager.IsZombieOpen(_type))
        {
            _type = ZombieType.Normal;
        }
        string path = "GUI/ZombieStand/ZombiePic/Zombie_" + string.Format("{0:D2}", (int)_type);
        Texture pic = GlobalModule.Instance.LoadResource(path) as Texture;

        int index = (((int)_type) - 1) % 4 ;
        ZombiePic[index].mainTexture = pic;
        return (Material)GameObject.Instantiate(ZombiePic[index]);
    }


    public static Material mMatLoginZombie;
    /// <summary>
    /// 获取僵尸静态图（登陆）
    /// </summary>
    /// <param name="_type"></param>
    /// <returns></returns>
    public static Material GetZombieP(ZombieType _type)
    {
        if(!mMatLoginZombie)
        {
            mMatLoginZombie = GlobalModule.Instance.LoadResource("GUI/Panel/Login/Zombie_Pic") as Material;
        }
        Material mat = (Material)Instantiate(mMatLoginZombie);

        string path = "GUI/ZombieStand/ZombiePic/Zombie_" + string.Format("{0:D2}", (int)_type);
        Texture pic = GlobalModule.Instance.LoadResource(path) as Texture;
        mat.mainTexture = pic;
        return mat;
    }

    /// <summary>
    /// 获取头像图
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static Texture GetHeadPic(int _index)
    {
        return GlobalModule.Instance.LoadResource(Head_List[_index]) as Texture;
    }

    /// <summary>
    /// 获取表情图
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static Texture GetFacePic(int _index)
    {
        return GlobalModule.Instance.LoadResource(Face_List[_index]) as Texture;
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_type">枚举</param>
    /// <returns></returns>
    public static AudioSource PlaySound(EResourceAudio _type)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource(Sound_List[(int)_type]) as AudioClip;
        return GlobalModule.Instance.PlaySE(clip);
    }

    /// <summary>
    /// 播放音效（限定播放）
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_maxCount"></param>
    /// <param name="_time"></param>
    /// <returns></returns>
    public static AudioSource ReplaySound(EResourceAudio _type, int _maxCount, float _time)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource(Sound_List[(int)_type]) as AudioClip;
        return GlobalModule.Instance.ReplaySE(clip, _maxCount, _time);
    }

    /// <summary>
    /// 播放音效（限定播放）
    /// </summary>
    /// <param name="_type"></param>
    /// <param name="_maxCount"></param>
    /// <param name="_time"></param>
    /// <returns></returns>
    public static AudioSource ReplaySound(string _path, int _maxCount, float _time)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource("sound/" + _path) as AudioClip;
        return GlobalModule.Instance.ReplaySE(clip, _maxCount, _time);
    }


    /// <summary>
    /// 播放resources/sound/下指定名称音效
    /// </summary>
    /// <param name="_str">音效名</param>
    /// <returns></returns>
    public static AudioSource PlaySound(string _str)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource("sound/" + _str) as AudioClip;
        return GlobalModule.Instance.PlaySE(clip);
    }

    /// <summary>
    /// 播放resources/sound/下指定名称音效
    /// </summary>
    /// <param name="_str">音效名</param>
    /// <returns></returns>
    public static AudioSource PlaySound(string _str, bool _isLoop)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource("sound/" + _str) as AudioClip;
        return GlobalModule.Instance.PlaySE(clip, 1, _isLoop);
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="_type">枚举</param>
    /// <param name="_volume">音量</param>
    /// <param name="_isLoop">是否循环</param>
    /// <returns></returns>
    public static AudioSource PlaySound(EResourceAudio _type, float _volume, bool _isLoop)
    {
        AudioClip clip = GlobalModule.Instance.LoadResource(Sound_List[(int)_type]) as AudioClip;
        return GlobalModule.Instance.PlaySE(clip, _volume, _isLoop);
    }

    /// <summary>
    /// 获取攻击背景
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static Texture GetAttackBG(int _index)
    {
        return GlobalModule.Instance.LoadResource(AttackBG[_index]) as Texture;
    }

    /// <summary>
    /// 获取攻击目标
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static SpriteAnimationClip GetAttackTarget(int _index)
    {
        GameObject obj = GlobalModule.Instance.LoadResource(AttackTarget[_index]) as GameObject;
        return obj.GetComponent<MotionIndex>().Clip;
    }

    /// <summary>
    /// 获取博士头像
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static Texture GetTexture(EResourceTexture _index)
    {
        return GlobalModule.Instance.LoadResource(Pic_List[(int)_index]) as Texture;
    }

    /// <summary>
    /// 获取stage 初始化信息
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static SceneData GetSceneData(int _index)
    {
        string[] str = new string[]
        {
            "Prefabs/Game/Scene/Scene1",
            "Prefabs/Game/Scene/Scene2",
            "Prefabs/Game/Scene/Scene3",
            "Prefabs/Game/Scene/Scene4",
        };

        return (GlobalModule.Instance.LoadResource(str[_index]) as GameObject).GetComponent<SceneData>();
    }

    /// <summary>
    /// 获取士兵（动画）
    /// </summary>
    /// <param name="_index"></param>
    /// <returns></returns>
    public static GameObject GetSoldier(int _index)
    {
        return GlobalModule.Instance.LoadResource(SoldierList[_index - 1]) as GameObject;
    }


    static UIAtlas mOtherFont;
    /// <summary>
    /// 获取其他语言
    /// </summary>
    /// <returns></returns>
    public static UIAtlas GetOtherFont()
    {
        string path = "";

        if(StringTable.mStringType == ELocalizationTyp.English)
        {
            path = "GUI/UIFont/UIFontEn";
        }
        else if(StringTable.mStringType == ELocalizationTyp.Japanese)
        {
            path = "GUI/UIFont/UIFontJp";
        }
        else if(StringTable.mStringType == ELocalizationTyp.ChineseTw)
        {
            path = "GUI/UIFont/UIFontTw";
        }
        else
        {
            path = "GUI/UIFont/UIFont";
        }
        
        if(!mOtherFont)
        {
            mOtherFont = (GlobalModule.Instance.LoadResource(path, typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
        }
        return mOtherFont;
    }


    static UIFont mOtherArtFont;
    /// <summary>
    /// 获取其他语言美术字
    /// </summary>
    /// <returns></returns>
    public static UIFont GetOtherArtFont()
    {
        string path = "";

        if (StringTable.mStringType == ELocalizationTyp.English)
        {
            path = "Atlas/UIFont_cn";
        }
        else if (StringTable.mStringType == ELocalizationTyp.Japanese)
        {
            path = "Atlas/UIFont_jp";
        }
        else if (StringTable.mStringType == ELocalizationTyp.ChineseTw)
        {
            path = "Atlas/UIFont_tw";
        }
        else
        {
            path = "Atlas/UIFont_cn";
        }

        if (!mOtherArtFont)
        {
            mOtherArtFont = (GlobalModule.Instance.LoadResource(path, typeof(GameObject)) as GameObject).GetComponent<UIFont>();
        }
        return mOtherArtFont;
    } 

}
