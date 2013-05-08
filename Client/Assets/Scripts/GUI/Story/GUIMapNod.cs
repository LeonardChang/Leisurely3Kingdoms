using UnityEngine;
using System.Collections;
using EasyMotion2D;




public enum EStoryInfoDirection
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}


public class GUIMapNod : MonoBehaviour 
{

    public int NodType = 0;
    public EStoryIndex Index;
    public EStoryInfoDirection Type;

    public GameObject Story_InfoRect;
    string mInfo;

    bool isIni = false;

    bool isOpen = false;
    GUIStoryManager Panel_Story;

    //GameObject.Find("Panel_Story").GetComponent<GUIStoryManager>()
	// Use this for initialization
	void Start () 
    {

	}

    void OnEnable()
    {
        isIni = false;

      
        //Panel_Story = GameObject.Find("Story_InfoRect").Get;

        //Story_InfoRect.transform.FindChild("isOpen")
    }

	// Update is called once per frame
	void Update () 
    {
        if (!isIni)
        {



            Panel_Story = GameObject.Find("Panel_Story").GetComponent<GUIStoryManager>();
            Story_InfoRect = Panel_Story.Story_InfoRect;  

            isIni = true;


            if (!transform.FindChild("Sprite Animation"))
                return;
            if(GameDataCenter.Instance.CurrentStory() >= (int)Index)
            {
                isOpen = true;
                //亮
                if (NodType == 0)
                {
                    transform.FindChild("Sprite Animation").GetComponent<SpriteAnimation>().Play("Map_NormalNod");
                }
                else
                {
                    transform.FindChild("Sprite Animation").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
                }
            }
            else
            {
                isOpen = false;
                //灭
                if(NodType != 0)
                {
                    transform.FindChild("Sprite Animation").GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
                }
                else
                {
                    transform.FindChild("Sprite Animation").GetComponent<SpriteAnimation>().Play("Map_NormalNodDark");
                }
            }  
        }
	}

    void ActiveInfo()
    {
        Vector3 pos = transform.position;
        Story_InfoRect.transform.position = new Vector3(pos.x, pos.y, Story_InfoRect.transform.position.z);
        CStory story = GlobalStaticData.GetStory(Index);
        Story_InfoRect.transform.FindChild("Label_Name").GetComponent<UILabel>().text = "<" + story.Name + ">";
        Story_InfoRect.transform.FindChild("Label_Info").GetComponent<UILabel>().text = story.Dsc;

        pos =  Story_InfoRect.transform.localPosition;

        if (transform.localPosition.x > 0 && transform.localPosition.y > 0)
            Type = EStoryInfoDirection.BottomLeft;
        if (transform.localPosition.x > 0 && transform.localPosition.y < 0)
            Type = EStoryInfoDirection.TopLeft;
        if (transform.localPosition.x < 0 && transform.localPosition.y > 0)
            Type = EStoryInfoDirection.BottomRight;
        if (transform.localPosition.x < 0 && transform.localPosition.y < 0)
            Type = EStoryInfoDirection.TopRight;


        switch(Type)
        {
            case EStoryInfoDirection.TopLeft:
                pos.x -= 160f;
                pos.y += 213f;
                pos.z = -500f;
                Story_InfoRect.transform.localPosition = pos;
                Story_InfoRect.transform.FindChild("Back").localScale = new Vector3(-1, 1, 1);
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").GetComponent<UISlicedSprite>().spriteName = "Main_Map_NodInfo2";
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").transform.localPosition = new Vector3(0, -23, 0);
                Story_InfoRect.transform.FindChild("IBtn_ReView").localPosition = new Vector3(6,-145,0);
                break;
            case EStoryInfoDirection.TopRight:
                pos.x += 160f;
                pos.y += 213f;
                pos.z = -500f;
                Story_InfoRect.transform.localPosition = pos;
                Story_InfoRect.transform.FindChild("Back").localScale = new Vector3(1, 1, 1);
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").GetComponent<UISlicedSprite>().spriteName = "Main_Map_NodInfo2";
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").transform.localPosition = new Vector3(12, -23, 0);
                Story_InfoRect.transform.FindChild("IBtn_ReView").localPosition = new Vector3(15,-145,0);
                break;
            case EStoryInfoDirection.BottomLeft:
                pos.x -= 160f;
                pos.y -= 200f;
                pos.z = -500f;
                Story_InfoRect.transform.localPosition = pos;
                Story_InfoRect.transform.FindChild("Back").localScale = new Vector3(-1, 1, 1);
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").GetComponent<UISlicedSprite>().spriteName = "Main_Map_NodInfo";
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").transform.localPosition = new Vector3(-10, 21, 0);
                Story_InfoRect.transform.FindChild("IBtn_ReView").localPosition = new Vector3(6, -143, 0);
                break;
            case EStoryInfoDirection.BottomRight:
                pos.x += 160f;
                pos.y -= 200f;
                pos.z = -500f;
                Story_InfoRect.transform.localPosition = pos;
                Story_InfoRect.transform.FindChild("Back").localScale = new Vector3(1, 1, 1);
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").GetComponent<UISlicedSprite>().spriteName = "Main_Map_NodInfo";
                Story_InfoRect.transform.FindChild("Back").FindChild("Background").transform.localPosition = new Vector3(-2, 21, 0);
                Story_InfoRect.transform.FindChild("IBtn_ReView").localPosition = new Vector3(10, -143, 0);
                break;
        }
    }


    int i = 0;
    void OnClick()
    {

        i++;
        if(i > 5)
        {
            //print("Outtttttttt");
            //GameDataCenter.Instance.SetStory((int)Index);
            //GameDataCenter.Instance.Save();
        }

        GameDataCenter.Instance.GuiManager.DeleteTipsSeeStage();
        ResourcePath.PlaySound("Click");
        if (!Panel_Story.FreeState)
        {
            return;
        }
        ActiveInfo();
        if(isOpen)
        {
            Panel_Story.Story_InfoRectReview.SetActiveRecursively(true);
        }
        else
        {
            
            Panel_Story.Story_InfoRectReview.SetActiveRecursively(false);
        }
        if(Index == EStoryIndex.Story_000)
        {
            Panel_Story.Story_InfoRectReview.SetActiveRecursively(false);
        }
        Panel_Story.mReviewStory = Index;
        Story_InfoRect.transform.localScale = Vector3.zero;
        TweenScale ts = Story_InfoRect.GetComponent<TweenScale>();
        ts.enabled = true;
        ts.Reset();
        ts.Play(true);
    }
}
