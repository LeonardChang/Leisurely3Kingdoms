using UnityEngine;
using System.Collections;
using EasyMotion2D;

/// <summary>
/// 陈列室僵尸卡片
/// </summary>
public class GUIZombieInfo : MonoBehaviour {

    /// <summary>
    /// 僵尸
    /// </summary>
    public CZombieData mZombieData;

    /// <summary>
    /// 僵尸编号Label
    /// </summary>
    public UILabel Label_Number;

    /// <summary>
    /// 僵尸名字Label
    /// </summary>
    public UILabel Label_Name;

    /// <summary>
    /// new
    /// </summary>
    public GameObject Sprite_new;

    /// <summary>
    /// 星星列表
    /// </summary>
    public UISprite[] Sprite_stars;

    /// <summary>
    /// 僵尸图片
    /// </summary>
    public UITexture Texture_Stand;

    public UISprite Background;

    private bool isIni = false;
    public UISprite Sprite_SuperTitle;
    //public ZombieMotionList mMotionList;
    private bool isOpen = false;
    public UISprite Sprite_UnOpen;

    /// <summary>
    /// 更新显示星星
    /// </summary>
    /// <param name="_count"></param>
    void SetStarCount(int _count)
    {
        

        for(int i = 0; i < _count; i++)
        {
            Sprite_stars[i].color = Color.white;
        }
        if(_count >= 3)
        {
            Sprite_SuperTitle.color = Color.white;
        }
    }

    /// <summary>
    /// 计算星星数量
    /// </summary>
    void UpDateStar()
    {
        int star = 0;
        int count = mZombieData.Count;
        if (count >= 50)
        {
            star++;
        }
        if (mZombieData.AttackLevel >= 4)
        {
            star++;
        }
        if (mZombieData.ValueLevel >= 4)
        {
            star++;
        }
        SetStarCount(star);
    }
    

	// Use this for initialization
	void Start () 
    {
        Sprite_UnOpen.GetComponent<UISprite>().atlas = ResourcePath.GetOtherFont();
	}
	


    public Sprite spr_sprites;
    /// <summary>
    ///初始化站立图像
    /// </summary>
    void IniStand()
    {
        ZombieType type = mZombieData.Type;
        //新主题相关
        if(type > ZombieType.Zombie45)
        {
            return;
        }
        Texture_Stand.material = ResourcePath.GetZombiePic(mZombieData.Type);

        if(mZombieData.IsOpen)
        {
            Texture_Stand.color = Color.white;
        }
        else
        {
             Texture_Stand.color = new Color(0, 0, 0, 1);
        }
        Texture_Stand.transform.localScale = new Vector3(Texture_Stand.material.mainTexture.width, Texture_Stand.material.mainTexture.height, 1);
    }


    /// <summary>
    /// 初始化
    /// </summary>
    void Ini()
    {
        if (!isIni)
        {
            IniStand();

            //新僵尸
            if (!mZombieData.IsNew)
            {
                Sprite_new.SetActiveRecursively(false);
            }
            else
            {
                Sprite_new.GetComponent<UISprite>().color = Color.white;
            }

            Label_Number.text = string.Format("{0:D3}", (int)mZombieData.Type);

            if (mZombieData.IsOpen)
            {
                Label_Name.text = mZombieData.ZombieInfo.Name;
                Background.spriteName = "Panel_Collection_ZombieBack_Open";
                Label_Number.color = Color.white;
            }
            else
            {
                Label_Name.text = "???";
                Label_Number.color = Color.black;
            }
            isIni = true;
            isOpen = mZombieData.IsOpen;
            //新主题相关
            if (mZombieData.Type > ZombieType.Zombie45)
            {
                Sprite_UnOpen.color = Color.white;
            }
        }

    }

	// Update is called once per frame
	void Update ()
    {
        //Sprite_Animation.GetComponent<SpriteRenderer>().Apply();
        UpDateStar();

        //bool is_open = mZombieData.IsOpen;
        mZombieData = GameDataCenter.Instance.ZombieCollection[(int)mZombieData.Type - 1];
        if (!isOpen && mZombieData.IsOpen)
        {
            isIni = false;
        }
        Ini();

        if(mZombieData.IsNew)
        {
            Sprite_new.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            Sprite_new.GetComponent<UISprite>().color = new Color(1, 1, 1, 0);
        }

	}

    void OnEnable()
    {
        isIni = false;
    }

    /// <summary>
    /// 点击卡片
    /// </summary>
    void OnClick()
    {
        if (mZombieData.Type > ZombieType.Zombie45)
        {
            return;
        }

        //mZombieData.IsNew = false;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PanelCollection");
        if(objs.Length > 0)
        {
            return;
        }

        //Sprite_new.SetActiveRecursively(false);
        
        GameObject.Find("GUIMask").transform.localPosition = new Vector3(150, 0, -520f);
        GameObject.Find("GameUIRoot").GetComponent<GUIManager>().CreateMoveLeftZombieInfo(mZombieData);

        GameObject.Find("IBtn_NextZombie").transform.localPosition = new Vector3(286, 0, -950);
        GameObject.Find("IBtn_PreZombie").transform.localPosition = new Vector3(-284.5f, 0, -950);

        OOTools.OOTweenPosition(GameObject.Find("IBtn_NextZombie").transform.FindChild("Background").gameObject, new Vector3(0, 0, 0), new Vector3(10, 0, 0));
        OOTools.OOTweenPosition(GameObject.Find("IBtn_PreZombie").transform.FindChild("Background").gameObject, new Vector3(0, 0, 0), new Vector3(-10, 0, 0));
    }

    public void OnIni()
    {
        isIni = false;
        Ini();
        for(int i = 0; i < 3; i++)
        {
            Sprite_stars[i].color = new Color(1, 1, 1, 0);
        }
        Sprite_SuperTitle.color = new Color(1, 1, 1, 0);
    }
}