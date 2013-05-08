using UnityEngine;
using System.Collections;
using EasyMotion2D;

public class SceneData : MonoBehaviour {

    /// <summary>
    /// ��һ������
    /// </summary>
    public Sprite DustLine1;
    /// <summary>
    /// �ڶ�������
    /// </summary>
    public Sprite DustLine2;
    /// <summary>
    /// ����������
    /// </summary>
    public Sprite DustLine3;

    /// <summary>
    /// �ϲ�����
    /// </summary>
    public Texture DustLine4;


    /// <summary>
    /// ���ϵͳ�����б�
    /// </summary>
    public SpriteAnimationClip[] IrrigationMotion;
    /// <summary>
    /// ���������б�
    /// </summary>
    public SpriteAnimationClip[] MachineMotion;
    /// <summary>
    /// ��̳�����б�
    /// </summary>
    public SpriteAnimationClip[] AltarMotion;
    /// <summary>
    /// ��ͼ
    /// </summary>
    public GameObject Scene;


    public string MoneySprite;
}
