using UnityEngine;
using System.Collections;

public class SEPlayer : MonoBehaviour {
    public AudioClip SoundClip;
    public float SoundVolume;
    public bool SoundLoop;

	// Use this for initialization
	void Start () {
        GlobalModule.Instance.PlaySE(SoundClip, SoundVolume, SoundLoop);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
