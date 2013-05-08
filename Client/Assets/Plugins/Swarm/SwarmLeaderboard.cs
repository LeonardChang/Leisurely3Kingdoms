using UnityEngine;
using System.Collections;
using System;

/**
 * 
 * @author SwarmConnect
 * The SwarmLeaderboard "Proxy" class masking the Android Java code.
 *
 */
public class SwarmLeaderboard : MonoBehaviour {
	
	#if UNITY_ANDROID	
	/**
	 * Reference the SwarmUnityInterface class
	 */
	public static IntPtr swarmUnityInterface = AndroidJNI.FindClass("com/swarmconnect/SwarmUnityInterface");
	
	/**
	 * Reference the necessary SwarmUnityInterface methods
	 */
	public static IntPtr submitScoreMethod = AndroidJNI.GetStaticMethodID(swarmUnityInterface, "submitScore", "(IF)V");
	public static IntPtr showLeaderboardMethod = AndroidJNI.GetStaticMethodID(swarmUnityInterface, "showLeaderboard", "(I)V");
	public static IntPtr submitScoreAndShowLeaderboardMethod = AndroidJNI.GetStaticMethodID(swarmUnityInterface, "submitScoreAndShowLeaderboard", "(IF)V");	
	#endif
	
	/**
	 * Submit a score to the leaderboard for ranking.
	 * 
	 * @param leadeboardId The Id number of the leaderboard you want to submit to.
	 * @param score The player's score to be submitted.
	 */		
	public static void submitScore (int leaderboardId, float score){
		
		#if UNITY_ANDROID		
		jvalue[] args = new jvalue[2];
		args[0].i = leaderboardId;
		args[1].f = score;
		
		AndroidJNI.CallStaticVoidMethod(swarmUnityInterface, submitScoreMethod, args);
		#endif
	}
	
	/**
	 * Display the leaderboard with the specified Id number.
	 * 
	 * @param leadeboardId The Id number of the leaderboard you want to display.
	 */	
	public static void showLeaderboard (int leaderboardId) {
		
		#if UNITY_ANDROID		
		jvalue[] args = new jvalue[1];
		args[0].i = leaderboardId;

		AndroidJNI.CallStaticVoidMethod(swarmUnityInterface, showLeaderboardMethod, args);
		#endif		
	}
	
	/**
	 * Submit a score to the leaderboard for ranking.
	 * 
	 * @param leadeboardId The Id number of the leaderboard you want to submit to.
	 * @param score The player's score to be submitted.
	 */		
	public static void submitScoreAndShowLeaderboard (int leaderboardId, float score){
		
		#if UNITY_ANDROID		
		jvalue[] args = new jvalue[2];
		args[0].i = leaderboardId;
		args[1].f = score;
		
		AndroidJNI.CallStaticVoidMethod(swarmUnityInterface, submitScoreAndShowLeaderboardMethod, args);
		#endif
	}	
}
