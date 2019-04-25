using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    // Use this for initialization
    AudioSource backgroundMusic;
    GameObject player;
	void Start () {
        player = GameObject.Find("Player");
        backgroundMusic = GetComponent<AudioSource>();
        backgroundMusic.Play();

	}
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<PlayerScript>().IsDead == true)
        {
            backgroundMusic.Pause();
        }
	}
}
