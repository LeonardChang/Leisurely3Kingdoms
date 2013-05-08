using UnityEngine;
using System.Collections;

/// <summary>
/// 坑位
/// </summary>
public class CHole
{
    /// <summary>
    /// 坑位X坐标
    /// </summary>
    private float mX;
    public float X
    {
        get { return mX; }
        set { mX = value; }
    }

    /// <summary>
    /// 坑位Y坐标
    /// </summary>
    private float mY;
    public float Y
    {
        get { return mY; }
        set { mY = value; }
    }

    /// <summary>
    /// 坑位深度
    /// </summary>
    private int mDepth;
    public int Depth
    {
        get { return mDepth; }
        set { mDepth = value; }
    }

    /// <summary>
    /// 坑位id
    /// </summary>
    private int mId;
    public int Id
    {
        get { return mId; }
        set { mId = value;}
    }

    public CHole(float _x, float _y, int _depth, int _id)
    {
        X = _x;
        Y = _y;
        Depth = _depth;
        Id = _id;
    }
}


