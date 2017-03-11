using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum music {
    bossfight_commando_steve,
    player_death
}

public enum effect {
    jump,
    coffe,
    beer,
    vendingMachine,
    students
}

public class SoundController : MonoBehaviour {
    public AudioSource effectSource;
    public AudioSource musicSource;

    public AudioClip [] musicClips;
    public AudioClip [] effectClips;

    public static SoundController instance;

    // Use this for initialization
    void Awake() {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void playMusic(music m, bool loop) {
        musicSource.clip = musicClips[(int)m];
        effectSource.volume = 0.5f;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void stopMusic() {
        musicSource.Stop();
    }

    public void playEffect(effect eff) {
        effectSource.clip = effectClips[(int)eff];
        if (eff == effect.jump) { 
            effectSource.volume = Random.Range(0.5f, 0.75f);
            effectSource.pitch = Random.Range(0.9f, 1.1f);
        }else{
            effectSource.volume = Random.Range(0.9f, 1f);
            effectSource.pitch = Random.Range(0.95f, 1.05f);
        }
        effectSource.Play();
    }
}
