using UnityEngine;
using System.Collections;

/// <summary>
/// ��λ
/// </summary>
public class CHole
{
    /// <summary>
    /// ��λX����
    /// </summary>
    private float mX;
    public float X
    {
        get { return mX; }
        set { mX = value; }
    }

    /// <summary>
    /// ��λY����
    /// </summary>
    private float mY;
    public float Y
    {
        get { return mY; }
        set { mY = value; }
    }

    /// <summary>
    /// ��λ���
    /// </summary>
    private int mDepth;
    public int Depth
    {
        get { return mDepth; }
        set { mDepth = value; }
    }

    /// <summary>
    /// ��λid
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


