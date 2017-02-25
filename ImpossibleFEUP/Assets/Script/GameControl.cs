using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {
    public static GameControl instance;

    public float scrollSpeed = -2.5f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)// if there is another game control around (this old object is destroid)
            Destroy(gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
