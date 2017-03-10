using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum music {
    bossfight_commando_steve,
    player_death
}

public enum effect {
    jump
}

public class SoundController : MonoBehaviour {
    public AudioSource effectSource;
    public AudioSource musicSource;

    public AudioClip [] musicClips;
    public AudioClip [] effectClips;

    public static SoundController instance;

    public static float lowPitch = 0.9f;
    public static float highPitch = 1.1f;

    // Use this for initialization
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void playMusic(music m, bool loop) {
        musicSource.clip = musicClips[(int)m];
        effectSource.volume = 0.8f;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void stopMusic() {
        musicSource.Stop();
    }

    public void playEffect(effect eff) {
        effectSource.clip = effectClips[(int)eff];
        effectSource.volume = Random.Range(0.5f, 0.75f);
        effectSource.pitch = Random.Range(lowPitch, highPitch);
        effectSource.Play();
    }

}
