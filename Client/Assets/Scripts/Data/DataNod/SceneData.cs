using UnityEngine;
using System.Collections;
using EasyMotion2D;

public class SceneData : MonoBehaviour {

    /// <summary>
    /// 第一条地面
    /// </summary>
    public Sprite DustLine1;
    /// <summary>
    /// 第二条地面
    /// </summary>
    public Sprite DustLine2;
    /// <summary>
    /// 第三条地面
    /// </summary>
    public Sprite DustLine3;

    /// <summary>
    /// 上部背景
    /// </summary>
    public Texture DustLine4;


    /// <summary>
    /// 灌溉系统动画列表
    /// </summary>
    public SpriteAnimationClip[] IrrigationMotion;
    /// <summary>
    /// 机器动画列表
    /// </summary>
    public SpriteAnimationClip[] MachineMotion;
    /// <summary>
    /// 祭坛动画列表
    /// </summary>
    public SpriteAnimationClip[] AltarMotion;
    /// <summary>
    /// 地图
    /// </summary>
    public GameObject Scene;


    public string MoneySprite;
}
