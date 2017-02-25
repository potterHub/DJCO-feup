using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollScene : MonoBehaviour {
    private Rigidbody2D ground;
    
    // Use this for initialization
    void Start()
    {
        ground = GetComponent<Rigidbody2D>();// get the kinematic rigid body from the ground

        ground.velocity = new Vector2(GameControl.instance.scrollSpeed, 0);// scroll velocity
    }

    // Update is called once per frame
    void Update () {
		
	}
}
