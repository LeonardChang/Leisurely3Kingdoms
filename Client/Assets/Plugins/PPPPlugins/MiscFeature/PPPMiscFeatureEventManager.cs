using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public class PPPMiscFeatureEventManager : MonoBehaviour, IPPPMiscFeatureEventListener
{
	// Fired when the user cancels selection of an image from the photo album
	public static event Action albumChooserCancelledEvent;
	
	// Fired when a user chooses an image.  A ready to use Texture2D is returned.
	public static event Action<string, Texture2D> albumChooserSucceededEvent;
	
	// Fired when a user cancels the camera app without taking a picture
	public static event Action photoChooserCancelledEvent;
	
	// Fired when a photo is taking.  A ready to use Texture2D is returned.
	public static event Action<string, Texture2D> photoChooserSucceededEvent;
	
	// Fired when the user cancels the video recording operation
	public static event Action videoRecordingCancelledEvent;

	// Fired when a video is successfully taken and returns the full path to the video
	public static event Action<string> videoRecordingSucceededEvent;
	
	public static event Action<string> alertButtonClickedEvent;
	
	public static event Action alertCancelledEvent;
	
	// Loads up a Texture2D with the image at the given path
	public static Texture2D textureFromFileAtPath( string filePath )
	{
		PPPDebug.Log("Start textureFromFileAtPath:"+Time.realtimeSinceStartup.ToString());
		var bytes = System.IO.File.ReadAllBytes( filePath );
		PPPDebug.Log("All bytes read ok:"+Time.realtimeSinceStartup.ToString());
		var tex = new Texture2D( 1, 1 );
		tex.LoadImage( bytes );
		PPPDebug.Log("Image load to texture ok:"+Time.realtimeSinceStartup.ToString());
		tex.Apply();
		
		PPPDebug.Log( "texture size: " + tex.width + "x" + tex.height );
		PPPDebug.Log("End textureFromFileAtPath:"+Time.realtimeSinceStartup.ToString());
		return tex;
	}
	
	public IEnumerator textureFromWWW( string url, Action<string, Texture2D> action )
	{
		PPPDebug.Log("Start textureFromWWW:"+Time.realtimeSinceStartup.ToString());
		
		String urlWithProtocol = "file://"+url;
		PPPDebug.Log("Target URL:"+urlWithProtocol);
		
		Texture2D tex2d = new Texture2D( 1, 1 );//, TextureFormat.DXT5, false );
		WWW wwwObj = new WWW(urlWithProtocol);
		PPPDebug.Log("WWW Loading Progress = "+(wwwObj.progress*100).ToString()+"%:"+Time.realtimeSinceStartup.ToString());
		yield return wwwObj;
		PPPDebug.Log("WWW Loading Progress = "+(wwwObj.progress*100).ToString()+"%:"+Time.realtimeSinceStartup.ToString());
		PPPDebug.Log("Start WWW.LoadImageIntoTexture:"+Time.realtimeSinceStartup.ToString());
		wwwObj.LoadImageIntoTexture(tex2d);
		PPPDebug.Log("End WWW.LoadImageIntoTexture:"+Time.realtimeSinceStartup.ToString());

		// Use the generated texture2D to trigger UserLevel event
		PPPDebug.Log( "WWW texture size: " + tex2d.width + "x" + tex2d.height );
		action(url, tex2d);
	}

	#region IPPPMiscFeatureEventListener
	
	public void albumChooserCancelled( string empty )
	{
		if( albumChooserCancelledEvent != null )
			albumChooserCancelledEvent();
	}


	public void albumChooserSucceeded( string path )
	{
		if( albumChooserSucceededEvent != null )
		{
			// make sur the file exists before proceeding to load it
			if( System.IO.File.Exists( path ) )
			{
				// Usual way:Call textureFromFileAtPath,this method is slow because of its loading from byte array.
				//albumChooserSucceededEvent( path, textureFromFileAtPath( path ) );
				
				// Faster way:Using WWW.LoadImageIntoTexture
				StartCoroutine(textureFromWWW( path, albumChooserSucceededEvent ));
			}
			else if( albumChooserCancelledEvent != null )
				albumChooserCancelledEvent();
		}
	}

	public void photoChooserCancelled( string empty )
	{
		if( photoChooserCancelledEvent != null )
			photoChooserCancelledEvent();
	}


	public void photoChooserSucceeded( string path )
	{
		if( photoChooserSucceededEvent != null )
		{
			// make sure the file exists before proceeding to load it
			if( System.IO.File.Exists( path ) )
			{
				// Usual way:Call textureFromFileAtPath,this method is slow because of its loading from byte array.
				//photoChooserSucceededEvent( path, textureFromFileAtPath( path ) );
				
				// Faster way:Using WWW.LoadImageIntoTexture
				StartCoroutine(textureFromWWW( path, photoChooserSucceededEvent ));
			}
			else if( photoChooserCancelledEvent != null )
				photoChooserCancelledEvent();
		}
	}
	
	
	public void videoRecordingSucceeded( string path )
	{
		if( videoRecordingSucceededEvent != null )
			videoRecordingSucceededEvent( path );
	}
	
	
	public void videoRecordingCancelled( string empty )
	{
		if( videoRecordingCancelledEvent != null )
			videoRecordingCancelledEvent();
	}
	
	#endregion
	
	#region ShowAlert related event
	public void alertButtonClicked( string clickedButtonText )
	{
		if( alertButtonClickedEvent != null )
			alertButtonClickedEvent(clickedButtonText);
	}
	
	public void alertCancelled()
	{
		if( alertCancelledEvent != null )
			alertCancelledEvent();
	}
	#endregion
}
