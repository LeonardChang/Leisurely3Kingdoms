using UnityEngine;
using System.Collections;

public class OOTools : MonoBehaviour {

	public static void OOTweenPosition(GameObject _obj, Vector3 _from, Vector3 _to)
    {
        TweenPosition tp = _obj.GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.from = _from;
        tp.to = _to;
        tp.Reset();
        tp.Play(true);
    }

    public static void OOTweenPosition(GameObject _obj, Vector3 _from, Vector3 _to, float _duration)
    {
        TweenPosition tp = _obj.GetComponent<TweenPosition>();
        tp.enabled = true;
        tp.from = _from;
        tp.to = _to;
        tp.duration = _duration;
        tp.Reset();
        tp.Play(true);
    }



    public static void OOTweenScale(GameObject _obj, Vector3 _from, Vector3 _to)
    {
        TweenScale tp = _obj.GetComponent<TweenScale>();
        tp.enabled = true;
        tp.from = _from;
        tp.to = _to;
        tp.Reset();
        tp.Play(true);
    }

    public static void OOTweenScaleEx(GameObject _obj, Vector3 _from, Vector3 _mid, Vector3 _to)
    {
        TweenScaleEx tp = _obj.GetComponent<TweenScaleEx>();
        tp.enabled = true;
        tp.from = _from;
        tp.mid = _mid;
        tp.to = _to;
        tp.Reset();
        tp.Play(true);
    }    


    public static void OOTweenColor(GameObject _obj, Color _from, Color _to)
    {
        TweenColor tp = _obj.GetComponent<TweenColor>();
        tp.enabled = true;
        tp.from = _from;
        tp.to = _to;
        tp.Reset();
        tp.Play(true);
    }

    public static void OOTweenColorEx(GameObject _obj, Color _from, Color _mid, Color _to)
    {
        TweenColorEx tp = _obj.GetComponent<TweenColorEx>();
        tp.enabled = true;
        tp.from = _from;
        tp.to = _to;
        tp.mid = _mid;
        tp.Reset();
        tp.Play(true);
    }

    public static void OOSetBtnSprite(UIImageButton _iBtn, string _norSprite, string _hoverSprite, string _pressedSprite)
    {
        _iBtn.normalSprite = _norSprite;
        _iBtn.hoverSprite = _hoverSprite;
        _iBtn.pressedSprite = _pressedSprite;
        _iBtn.transform.FindChild("Background").GetComponent<UISprite>().spriteName = _norSprite;
        _iBtn.transform.FindChild("Background").GetComponent<UISprite>().MakePixelPerfect();
    }
}
