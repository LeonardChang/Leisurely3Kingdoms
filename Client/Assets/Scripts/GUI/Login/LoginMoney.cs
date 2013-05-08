using UnityEngine;
using System.Collections;

public class LoginMoney : MonoBehaviour 
{

    public int type = 0;


    Transform Label_Money;
    Transform Label_Gem;


    public int mValue = 0;
    public AudioClip acMoney;
	// Use this for initialization
	void Start () 
    {
        Label_Money = GameObject.Find("Label_Money").transform;
        Label_Gem = GameObject.Find("Label_Gem").transform;

        if(type == 0)
        {
            Vector3 target = Label_Money.position;
            target.z = transform.position.z;
            iTween.MoveTo(gameObject, iTween.Hash("position", target,
                                                                "time", 0.5,
                                                                "delay", 0.5f));
            Invoke("PlayMoveSound", 0.5f);
            GameDataCenter.Instance.GuiManager.CreateFlyLabel(1, transform.parent.gameObject, transform.localPosition + new Vector3(0, 20, -1000), mValue);
        }
        else
        {
            Vector3 target = Label_Gem.position;
            target.z = transform.position.z;
            iTween.MoveTo(gameObject, iTween.Hash("position", target,
                                                                "time", 0.5f,
                                                                "delay", 0.5f));
            Invoke("PlayMoveSound", 0.5f);
            GameDataCenter.Instance.GuiManager.CreateFlyLabel(1, transform.parent.gameObject, transform.localPosition + new Vector3(0, 20, -1000), mValue);
        }
        Destroy(gameObject, 0.9f);
	}

    void PlayMoveSound()
    {
        AudioSource audio = ResourcePath.ReplaySound(EResourceAudio.Audio_TouchFlyItem, 2, 0.1f);
    }

    void OnDestroy()
    {
        //GameDataCenter.Instance.AddMoney(mValue);
        //GUIManager Manager = GameObject.Find("GameUIRoot").GetComponent<GUIManager>();
        if(type == 0)
        {
            GameDataCenter.Instance.GuiManager.GameAddMoney(mValue);
            
        }
        else
        {
            GameDataCenter.Instance.GuiManager.GameAddGem(mValue);
        }
        GlobalModule.Instance.PlaySE(acMoney);
    }

	// Update is called once per frame
	void Update () 
    {
	    
	}
}
