using UnityEngine;
using System.Collections;

public class GUIAchieveManager : MonoBehaviour {

    public GameObject AchieveBtnPrefab;
    public UIGrid Grid;

    bool isIni = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (!isIni)
        {
            isIni = true;

            foreach(CAchieveData data in  GameDataCenter.Instance.AchieveData)
            {
                GameObject obj = (GameObject)Instantiate(AchieveBtnPrefab);
                obj.transform.parent = Grid.transform;
                obj.transform.localScale = Vector3.one;

                obj.transform.FindChild("Label(Name)").GetComponent<UILabel>().text = data.mAchieveInfo.mName + "(" + data.FinishProgress.ToString() + "/" +
                                                                                    data.mAchieveInfo.mMaxFinish.ToString() + ")";

                obj.transform.FindChild("Label(Dsc)").GetComponent<UILabel>().text = data.mAchieveInfo.mDsc;
            }

            Grid.Reposition();
        }
	}
}
