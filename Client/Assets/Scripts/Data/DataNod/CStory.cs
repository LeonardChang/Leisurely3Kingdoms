using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 剧情枚举
/// </summary>
public enum EStoryIndex
{
    Story_000 = 0,
    Story_001,
    Story_002,
    Story_003,
    Story_004,
    Story_005,
    Story_006,
    Story_007,
    Story_008,
    Story_009,
    Story_010,
    Story_011,
    Story_012,
    Story_013,
    Story_014,
    Story_015,
    Story_016,
    Story_017,
    Story_018,
    Story_019,
    Story_020,
    Story_021,
    Story_022,
    Story_023,
    Story_024,
    Story_025,
    Story_026,
    Story_027,
    Story_028,
    Story_029,
    Story_030,
    Story_031,
    Story_032,
    Story_033,
    Story_034,
    Story_035,
    Story_036,
    Story_037,
    Story_038,
    Story_039,
    Story_040,
    Story_041,
    Story_042,
    Story_043,
    Story_044,
    Story_045,
    Story_046,
    Story_047,
    Story_048,
    Story_049,
    Story_050,
    Story_051,
    Story_052,
    Story_053,
    Story_054,
    Story_055,
    Story_056,
    Story_057,
    Story_058,
    Story_059,
    Story_060,
    Story_061,
    Story_062,
    Story_063,
    Story_064,
    Story_065,
    Story_066,
    Max
}

/// <summary>
/// 剧情
/// </summary>
public class CStory 
{
    /// <summary>
    /// 枚举
    /// </summary>
    public EStoryIndex StoryIndex = EStoryIndex.Story_000;

    /// <summary>
    /// 名称
    /// </summary>
    public string Name = "";

    /// <summary>
    /// 描述
    /// </summary>
    public string Dsc = "";

    /// <summary>
    /// 最大血量
    /// </summary>
    public int MaxCondition = 0;

    /// <summary>
    /// 攻击背景
    /// </summary>
    public int AttackBG = 0;
    
    /// <summary>
    /// 攻击目标
    /// </summary>
    public int AttackTarget = 0;

    /// <summary>
    /// 当先血量
    /// </summary>
    public int Condition = 0;

    /// <summary>
    /// 奖励水晶个数
    /// </summary>
    public int AwardGem = 0;

    /// <summary>
    /// 兵种
    /// </summary>
    public int SoldierType = 0;
}
