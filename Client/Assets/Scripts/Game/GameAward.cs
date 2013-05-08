using UnityEngine;
using System.Collections;

public class GameAward : MonoBehaviour {

	// Use this for initialization
	void Start () 
    {
	    
	}
	
	// Update is called once per frame
	void Update () 
    {
	    
	}

    /// <summary>
    /// 检测奖励
    /// </summary>
    public static void CheckAward()
    {
        if(!GameDataCenter.Instance.mIsAwardTeach)
        {
            AwardTeach();
        }

         
        if(!GameDataCenter.Instance.mIsAwardSiWangPass && GameDataCenter.Instance.CurrentStory() == 5)
        {
            AwardSiWangPass();
        }

        if (!GameDataCenter.Instance.mIsAwardShiTouPass && GameDataCenter.Instance.CurrentStory() == 12)
        {
            AwardShitouPass();
        }

        if (!GameDataCenter.Instance.mIsAwardFuRenPass && GameDataCenter.Instance.CurrentStory() == 17)
        {
            AwardFuRenPass();
        }

        if (!GameDataCenter.Instance.mIsAwardDeathIslandPass && GameDataCenter.Instance.CurrentStory() == 22)
        {
            AwardDeathIslandPass();
        }

        if(GameDataCenter.Instance.mAwardLoginBackDays >= 3 && GameDataCenter.Instance.mAwardLoginBackDays < 30)
        {
            AwardLoginBack(GameDataCenter.Instance.mAwardLoginBackDays);
            GameDataCenter.Instance.mAwardLoginBackDays = 0;
        }
    }



    static void AddMoney(string money)
    {

    }


    /// <summary>
    /// 教程后开始游戏
    /// </summary>
    static void AwardTeach()
    {
        if (GameDataCenter.Instance.mIsAwardTeach) return;
        GameDataCenter.Instance.mIsAwardTeach = true;
        GlobalModule.Instance.ShowMessageBoxCongratulation(StringTable.GetString(EStringIndex.TipsAward_FinishTeach), StringTable.GetString(EStringIndex.Tips_OK));
        MessageBoxSpecial.alterButtonClickedEvent += AddMoney;
        GameDataCenter.Instance.GuiManager.GameAddMoney(200);
    }

    /// <summary>
    /// 财政赤字
    /// </summary>
    public static void AwardLackMoney()
    {
        if (GameDataCenter.Instance.mIsAwardLackMoney) return;
        GameDataCenter.Instance.mIsAwardLackMoney = true;
        GlobalModule.Instance.ShowInGameMessageBox(StringTable.GetString(EStringIndex.Tips_TitleTips),StringTable.GetString(EStringIndex.TipsAward_LackMoney),StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(200);

    }

    /// <summary>
    /// 死亡沼泽
    /// </summary>
    public static void AwardSiWangPass()
    {
        if (GameDataCenter.Instance.mIsAwardSiWangPass) return;
        GameDataCenter.Instance.mIsAwardSiWangPass = true;
        GlobalModule.Instance.ShowMessageBoxCongratulation(StringTable.GetString(EStringIndex.TipsAward_SiWangPass), StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(700);
    }

    /// <summary>
    /// 石头人领地
    /// </summary>
    public static void AwardShitouPass()
    {
        if (GameDataCenter.Instance.mIsAwardShiTouPass) return;
        GameDataCenter.Instance.mIsAwardShiTouPass = true;
        GlobalModule.Instance.ShowMessageBoxCongratulation(StringTable.GetString(EStringIndex.TipsAward_ShiTouPass), StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(2000);
    }

    /// <summary>
    /// 富人城
    /// </summary>
    public static void AwardFuRenPass()
    {
        if (GameDataCenter.Instance.mIsAwardFuRenPass) return;
        GameDataCenter.Instance.mIsAwardFuRenPass = true;
        GlobalModule.Instance.ShowMessageBoxCongratulation(StringTable.GetString(EStringIndex.TipsAward_FuRenPass), StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(4000); 
    }

    /// <summary>
    /// 新僵尸奖励
    /// </summary>
    public static void AwardNewZombie(CZombieData zombie_data)
    {

        if ((zombie_data.Type >= ZombieType.Zombie04 && zombie_data.Type <= ZombieType.Zombie15) ||
            (zombie_data.Type >= ZombieType.Zombie19 && zombie_data.Type <= ZombieType.Zombie30) ||
            (zombie_data.Type >= ZombieType.Zombie34 && zombie_data.Type <= ZombieType.Zombie45))
        {
            GlobalModule.Instance.ShowMessageBoxCongratulation(string.Format(StringTable.GetString(EStringIndex.TipsAward_NewZombie), zombie_data.ZombieInfo.Name),
            StringTable.GetString(EStringIndex.Tips_OK));


            GameDataCenter.Instance.GuiManager.GameAddGem(5);
        }
    }


    /// <summary>
    /// 死亡岛通关
    /// </summary>
    public static void AwardDeathIslandPass()
    {
        if (GameDataCenter.Instance.mIsAwardDeathIslandPass) return;
        GameDataCenter.Instance.mIsAwardDeathIslandPass = true;
        GlobalModule.Instance.ShowMessageBoxCongratulation(StringTable.GetString(EStringIndex.TipsAward_DeathIslandPass), StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(5000); 
    }


    /// <summary>
    /// 回归奖励
    /// </summary>
    public static void AwardLoginBack(int days)
    {
        GlobalModule.Instance.ShowMessageBoxCongratulation(string.Format(StringTable.GetString(EStringIndex.TipsAward_LoginBack), days * 150), StringTable.GetString(EStringIndex.Tips_OK));
        GameDataCenter.Instance.GuiManager.GameAddMoney(days * 150); 
    }


    /// <summary>
    /// 旧版本奖励补偿
    /// </summary>
    public static void AwardOldVersion()
    {
        int money = 0;
        int gem = 0;
        if (GameDataCenter.Instance.IsTeachMode)
        {
            PlayerPrefs.SetInt("Version_11", 1);
            PlayerPrefs.Save();
            return;
        }
            
        if(!PlayerPrefs.HasKey("Version_11"))
        {
            PlayerPrefs.SetInt("Version_11", 1);
            PlayerPrefs.Save();
            //僵尸补偿
            if (!GameDataCenter.Instance.mIsAwardTeach)
            {
                money += 200;
                GameDataCenter.Instance.mIsAwardTeach = true;
            }

            //死亡沼泽补偿
            if (!GameDataCenter.Instance.mIsAwardSiWangPass && GameDataCenter.Instance.CurrentStory() >= 5)
            {
                money += 700;
                GameDataCenter.Instance.mIsAwardSiWangPass = true;
            }

            //石头人领地补偿
            if (!GameDataCenter.Instance.mIsAwardShiTouPass && GameDataCenter.Instance.CurrentStory() >= 12)
            {
                money += 2000;
                GameDataCenter.Instance.mIsAwardShiTouPass = true;
            }

            //富人城领地补偿
            if (!GameDataCenter.Instance.mIsAwardFuRenPass && GameDataCenter.Instance.CurrentStory() >= 17)
            {
                money += 4000;
                GameDataCenter.Instance.mIsAwardFuRenPass = true;
            }

            //死亡地带关卡
            if (!GameDataCenter.Instance.mIsAwardDeathIslandPass && GameDataCenter.Instance.CurrentStory() >= 22)
            {
                money += 500;
                GameDataCenter.Instance.mIsAwardDeathIslandPass = true;
            }
            //新主题相关
            foreach(CZombieData zombie in GameDataCenter.Instance.ZombieCollection)
            {
                if (zombie.ZombieInfo.Rare < 20 && zombie.Count > 0 && ZombieDataManager.IsZombieOpen(zombie.Type))
                    gem += 5;
            }
            GameDataCenter.Instance.GuiManager.GameAddMoney(money);
            GameDataCenter.Instance.GuiManager.GameAddGem(gem);

            GlobalModule.Instance.ShowMessageBoxCongratulation(string.Format(StringTable.GetString(EStringIndex.TipsAward_OldVersion), money, gem), StringTable.GetString(EStringIndex.Tips_OK));
        }
    }
}
