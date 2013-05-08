using UnityEngine;
using System.Collections;

public class GUIPanelHonor : MonoBehaviour {

    public UITexture Stage_Texture_1;
    public UITexture Stage_Texture_2;
    public UITexture Stage_Texture_3;
    bool mIsOpen = false;

    void OnEnable()
    {
        if(PlayerPrefs.GetInt("StageFull_1") != 0)
        {
            Stage_Texture_1.material = GlobalModule.Instance.LoadResource("GUI/Button/Medal/Mat_Medal_1") as Material;
            Stage_Texture_1.transform.localScale = new Vector3(Stage_Texture_1.mainTexture.width, Stage_Texture_1.mainTexture.height, 1);
        }

        if(PlayerPrefs.GetInt("StageFull_2") != 0)
        {
            Stage_Texture_2.material = GlobalModule.Instance.LoadResource("GUI/Button/Medal/Mat_Medal_2") as Material;
            Stage_Texture_2.transform.localScale = new Vector3(Stage_Texture_2.mainTexture.width, Stage_Texture_2.mainTexture.height, 1);
        }

        if (PlayerPrefs.GetInt("StageFull_3") != 0)
        {
            Stage_Texture_3.material = GlobalModule.Instance.LoadResource("GUI/Button/Medal/Mat_Medal_3") as Material;
            Stage_Texture_3.transform.localScale = new Vector3(Stage_Texture_3.mainTexture.width, Stage_Texture_3.mainTexture.height, 1);
        }
        Open();
    }

    void OnDisable()
    {

    }

	// Use this for initialization
	void Start () 
    {
	
	}

    void Open()
    {
        mIsOpen = true;
        OOTools.OOTweenScale(gameObject, new Vector3(0.01f, 0.01f, 1), Vector3.one);
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnClose()
    {
        mIsOpen = false;
        OOTools.OOTweenScale(gameObject, Vector3.one, new Vector3(0.01f, 0.01f, 1));
    }

    void OnCloseEnd()
    {
        if(!mIsOpen)
        {
            gameObject.SetActiveRecursively(false);
        }
    }
}
