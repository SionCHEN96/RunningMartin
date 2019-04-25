using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    // Use this for initialization
    public AudioClip[] audio;
    AudioSource music;
    GameObject player;

	void Start () {
        player = GameObject.Find("Player");
        GetComponent<AudioSource>().clip = audio[0];
        GetComponent<AudioSource>().Play();

    }
	
	// Update is called once per frame
	void Update () {
        if (player.GetComponent<MartinController>().IsDead == true)
        {
            GetComponent<AudioSource>().clip = audio[1];
            GetComponent<AudioSource>().volume += 0.4f;
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();

        }

    }
}
