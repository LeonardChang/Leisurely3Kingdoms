using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GtGamecenter : MonoBehaviour {
	
	static bool loadLeaderBoard = false;
	static bool loadAchievement = false;
	static bool connectGameCenter = false;
	float timeCheck = 0;
	
	private static List<GameCenterLeaderboard> leaderboards;
	private static List<GameCenterAchievementMetadata> achievementMetadata;

    public delegate void LoginOK();
    public static event LoginOK LoginEvent;
	
	// Use this for initialization
	void Start () {
		GameCenterManager.categoriesLoaded += categoriesLoaded;
		GameCenterManager.playerAuthenticated += playerAuthenticated;
		GameCenterManager.playerFailedToAuthenticate += playerFailedToAuthenticate;
		GameCenterBinding.authenticateLocalPlayer();
		//GameCenterBinding.playerAlias();
		
		GameCenterManager.achievementsLoaded += achievementsLoaded;
		GameCenterManager.achievementMetadataLoaded += achievementMetadataLoaded;
		//GameCenterBinding.getAchievements();
		
	}
	
	void OnDisable()
	{
		GameCenterManager.achievementsLoaded -= achievementsLoaded;
		GameCenterManager.categoriesLoaded -= categoriesLoaded;	
		GameCenterManager.achievementMetadataLoaded -= achievementMetadataLoaded;
	}
	
	// Update is called once per frame
	void Update () {
		if((loadLeaderBoard && loadAchievement) || !connectGameCenter)
		{
			return;
		}
		
		timeCheck += Time.deltaTime;
		if(timeCheck < 1)
		{
			return;
		}
		
		timeCheck = 0;
		
		if(!loadLeaderBoard)
		{
			GameCenterBinding.loadLeaderboardTitles();
		}
		if(!loadAchievement)
		{
			GameCenterBinding.getAchievements();
		}
	}
	
	void categoriesLoaded( List<GameCenterLeaderboard> _leaderboards )
	{
		Debug.Log( "categoriesLoaded" );
		Debug.Log("list count" + _leaderboards.Count);
		leaderboards = _leaderboards;
		loadLeaderBoard = true;
		
		if (LoginEvent != null)
		{
			LoginEvent();
		}
	}
	
	public void OpenLeaderBoard()
	{
		GameCenterBinding.showLeaderboardWithTimeScope(GameCenterLeaderboardTimeScope.Today);
	}
	
	public string GetName()
	{
		return GameCenterBinding.playerAlias();
	}
	
	public void PostLeaderBoadrd(int index,int store)
	{
		if(leaderboards != null)
		{
			Debug.Log(leaderboards.Count);
		}
		else
		{
			Debug.Log("lead is null");
		}
		if(leaderboards != null && leaderboards.Count >index)
		{
			GameCenterBinding.reportScore(store,leaderboards[index].leaderboardId);
			Debug.Log("really post point");
		}		
	}
	
	void achievementMetadataLoaded( List<GameCenterAchievementMetadata> _achievementMetadata )
	{
		Debug.Log( "achievementMetadatLoaded" );
		foreach( GameCenterAchievementMetadata s in _achievementMetadata )
			Debug.Log( s );	
		achievementMetadata = _achievementMetadata;
	}
	
	public void OpenAchievement()
	{
		GameCenterBinding.showAchievements();
	}
	
	public void PostAchievement(int index,float percent)
	{		
		if(achievementMetadata != null && achievementMetadata.Count > index)
		{
			GameCenterBinding.reportAchievement( achievementMetadata[index].identifier, percent);
			Debug.Log("post achievement percent" + percent);
		}
		else
		{
			Debug.Log("index out of range");
		}
	}
	
	void achievementsLoaded( List<GameCenterAchievement> achievements )
	{
		Debug.Log( "achievementsLoaded" );
		foreach( GameCenterAchievement s in achievements )
			Debug.Log( s );
		if(achievements.Count >=0)
		{
			GameCenterBinding.retrieveAchievementMetadata();
		}
		loadAchievement = true;
	}
	
	void playerAuthenticated()
	{
		connectGameCenter = true;
		Debug.Log( "playerAuthenticated" );
	}
	
	
	void playerFailedToAuthenticate( string error )
	{
		Debug.Log( "playerFailedToAuthenticate: " + error );
	}
	
	
	
	
	
}
