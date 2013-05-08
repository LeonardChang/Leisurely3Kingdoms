using UnityEngine;
using System.Collections;

public class ShowFPS : MonoBehaviour {
    public UILabel Label;

    private float timeleft = 0;
    private float accum = 0;
    private int frames = 0;
    private float updateInterval = 0.2f;
    private string fps = "";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0f)
        {
            // display two fractional digits (f1 format)
            fps = "" + (accum / frames).ToString("f1");
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }

        if (accum / frames < 25f)
        {
            Label.text = "[FF0000]" + fps + " FPS\n";
        } 
        else
        {
            Label.text = fps + " FPS\n";
        }
	}
}
