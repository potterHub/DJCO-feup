﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScene : MonoBehaviour {
    private Rigidbody2D ground;
    
    // Use this for initialization
    void Start()
    {
        ground = GetComponent<Rigidbody2D>();
        //setScrollSpeed(GameControl.instance.scrollSpeed);

        //GameControl.instance.setSceneScroll(this);
    }

    public void setScrollSpeed(float speed)
    {
        ground.velocity = new Vector2(speed, 0);// scroll velocity
    } 

    // Update is called once per frame
    void Update () {
		
	}
}
