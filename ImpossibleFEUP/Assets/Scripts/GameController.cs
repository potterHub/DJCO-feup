using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
    public static GameController instance;

    public Camera gameCamera;
    public LevelGenerator scenery;
    public PlayerControler player;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SoundController.instance.playMusic(music.bossfight_commando_steve);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
