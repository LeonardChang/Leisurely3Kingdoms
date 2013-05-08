using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using PPP.Unity3D.Editor.Toolkit;

public class PPPEclipseProjectHelperUnityIntegration : MonoBehaviour {

	[MenuItem("P++ Tools/Eclipse Project Helper")]
	public static void CleanForEclipseImport()
	{
		PPPEclipseProjectHelper.CleanUnity3DStagingAreaForEclipseImport();
	}
	
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
		if (target == BuildTarget.Android)
		{
			CleanForEclipseImport();
		}
	}
}
