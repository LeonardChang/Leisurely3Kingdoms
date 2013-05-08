using UnityEngine;
using System.Collections;

public class PPPDebug {
	
	private const bool PPPDebugEnabled = true;
	
	public static void Log(object message)
	{
		if (PPPDebugEnabled)
			Debug.Log(message);
	}

	public static void Log(object message, Object context)
	{
		if (PPPDebugEnabled)
			Debug.Log(message, context);
	}
	
}
