﻿//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Sends a message to the remote object when something happens.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Message")]
public class UIButtonMessage : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick,
        OnHover,
	}

	public GameObject target;
	public string functionName;
	public Trigger trigger = Trigger.OnClick;
	public bool includeChildren = false;

	float mLastClick = 0f;
	bool mStarted = false;
	bool mHighlighted = false;

	void Start () { mStarted = true; }

	void OnEnable () { if (mStarted && mHighlighted) OnHover(UICamera.IsHighlighted(gameObject)); }

	void OnHover (bool isOver)
	{
		if (enabled)
		{
			if (((isOver && trigger == Trigger.OnMouseOver) ||
				(!isOver && trigger == Trigger.OnMouseOut))) Send();
			mHighlighted = isOver;

            

		}
	}

    void OnDrag(Vector2 drag)
    {
        if(enabled)
        {
            if ( drag.magnitude > 5 &&   trigger == Trigger.OnHover) Send();
        }
        
    }

	void OnPress (bool isPressed)
	{
		if (enabled)
		{
			if (((isPressed && trigger == Trigger.OnPress) ||
				(!isPressed && trigger == Trigger.OnRelease))) Send();
		}
	}

	void OnClick ()
	{
		if (enabled)
		{
			float time = Time.realtimeSinceStartup;

			if (mLastClick + 0.2f > time)
			{
				if (trigger == Trigger.OnDoubleClick) Send();
			}
			else if (trigger == Trigger.OnClick) Send();

			mLastClick = time;
		}
	}

	void Send ()
	{
        if(!GlobalModule.Instance.IsCanClick)
        {
            return;
        }
        GlobalModule.Instance.Click();

		if (!gameObject.active || string.IsNullOrEmpty(functionName)) return;
		if (target == null) target = gameObject;

		if (includeChildren)
		{
			Transform[] transforms = target.GetComponentsInChildren<Transform>();

			for (int i = 0, imax = transforms.Length; i < imax; ++i)
			{
				Transform t = transforms[i];
				t.gameObject.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
			}
		}
		else
		{
			target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
		}
	}
}