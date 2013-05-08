using UnityEngine;
using System.ComponentModel;

public enum ClientMessageEnum : int
{
    FirstOpenGame = 1000,       // 第一次启动游戏
    OpenGame = 1001,            // 启动游戏
    BeginStoryPoint = 1002,     // 开始剧情点
    FinishStoryPoint = 1003,    // 结束剧情点
    GetZombie = 1005,           // 收获僵尸
    GetAchievement = 2001,      // 达成成就
    ClickAchievement = 2002,    // 查看成就
    ClickLeaderboard = 2003,    // 查看排行榜
    ClickShear = 2004,          // 分享游戏
    ClickMoregames = 2005,      // MoreGames
    UseGem = 2006,              // 使用钻石
    FinishTeach = 2007,         // 完成教程
    SystemInfomation = 2008,    // 系统信息
    StartPlay = 2009,           // 开始游戏
    LogError = 2010,            // 提交log的出错总次数
    MMInitError = 2011,         // 移动插件初始化失败
    ERNIE = 2012,               // 777摇奖
    FinishDayMission = 2013,    // 完成每日任务
    YesterdayEvaluation = 2014, // 昨日评价
}

public enum LinkToWWWEnum
{
    ProductPage,                // 产品列表
    ReleasePage,                // 发布页
    ShareMe1,                   // 分享页（标题页）
    ShareMe2,                   // 分享页（内页）
}

public enum AchievementEnum : int
{
    ZombieArmy = 0,             // 僵尸军团
    GreatFarmer,                // 高级农夫
    Richer,                     // 大富豪
    DeathLeader,                // 死亡领主
    King,                       // 国王
    Fans,                       // 爱好者
    VIP,                        // VIP
    Collector,                  // 收藏家
    Mummy,                      // 木乃伊
    FireBird,                   // 朱雀之谜
    Max,
}

public enum AppStoreItemID : int 
{
    // for IOS
    Buy60Crystal = 0,           // 购买60水晶
    Buy320Crystal,              // 购买320水晶
    Buy680Crystal,              // 购买680水晶
    Buy1400Crystal,             // 购买1400水晶

    // for 10086
    BuyMM30Crystal,             // 移动MM市场，购买30水晶
    BuyMM55Crystal,             // 移动MM市场，购买55水晶
    BuyMM115Crystal,            // 移动MM市场，购买115水晶
    BuyMM360Crystal,            // 移动MM市场，购买360水晶
}

public enum ShopType : int
{
    Normal = 0,                 // 普通
    MM,                         // 中国移动
    GFan,                       // 机锋
    Alipay,                     // 支付宝
    GooglePlay,                 // Google play
}

/// <summary>
/// 发布商
/// </summary>
public enum Publisher : int
{
    /// <summary>
    /// 巨唐官方渠道
    /// </summary>
    [Description("PlayPlusPlus")]
    PlayPlusPlus = 0,
    /// <summary>
    /// 厦门漫乐 for 移动MM市场
    /// </summary>
    [Description("厦门漫乐 【MM市场】")]
    Manloo_MM,
    /// <summary>
    /// 梦宝谷 for 大陆
    /// </summary>
    [Description("梦宝谷 【简体】")]
    Mobage_CN,
    /// <summary>
    /// 厦门漫乐 for 游戏基地
    /// </summary>
    [Description("厦门漫乐 【游戏基地】")]
    Manloo_G,
    /// <summary>
    /// 梦宝谷 for 港澳台
    /// </summary>
    [Description("梦宝谷 【港澳台】")]
    Mobage_TW,
    /// <summary>
    /// 厦门漫乐 for 通用市场
    /// </summary>
    [Description("厦门漫乐 【通用市场】")]
    Manloo_GLOBAL,

    /// <summary>
    /// BBG-Entertainment for 通用市场（App Store和Google Play）
    /// </summary>
    [Description("BBG 【日版 通用市场】")]
    BBG_JP,
    /// <summary>
    /// BBG-Entertainment for Yahoo Market
    /// </summary>
    [Description("BBG 【日版 Yahoo】")]
    BBG_JP_Yahoo,
    /// <summary>
    /// BBG-Entertainment for Samsung Apps
    /// </summary>
    [Description("BBG 【日版 三星市场】")]
    BBG_JP_Samsung,
    /// <summary>
    /// BBG-Entertainment for Amazon
    /// </summary>
    [Description("BBG 【日版 亚马逊】")]
    BBG_JP_Amazon,
    /// <summary>
    /// BBG-Entertainment for auMarket
    /// </summary>
    [Description("BBG 【日版 auMarket】")]
    BBG_JP_Au,

    Max,
}

/// <summary>
/// 投放市场
/// </summary>
public enum Market : int
{
    AppStore = 0,
    GooglePlay,
    //MobileMarket_移动市场,
    //Mobage_梦宝谷,
    //GFan_机锋,
    //HiAPK_安卓市场,
    //Mobile91_91助手,
    //AnZhi_安智市场,
    //LeDang_乐当网,
    //AppChina_应用会,
    //MuZhiWan_拇指玩,
    //MuMaYi_木蚂蚁,
    //AliYun_阿里云,
    //Baidu_百度应用,
    //Mobile163_网易应用,
    //TencentApp_腾讯应用,
    //SohuDownload_搜狐应用,
    //LenovoMM_联想乐商店,
    Max,
}