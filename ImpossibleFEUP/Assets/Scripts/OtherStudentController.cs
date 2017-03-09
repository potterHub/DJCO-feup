﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherStudentController : MonoBehaviour {

    public PlayerControler student;

    public float downSlowDownTime = 4.0f;
    public float topSlowDownTime = 6.0f;
    private float timerLeftToDestroy = 0.0f;
    private bool timerToDestroy;

    // Use this for initialization
    void Start () {
        timerToDestroy = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (timerToDestroy) {
            timerLeftToDestroy -= Time.deltaTime;
            if (timerLeftToDestroy < 0)
                gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")// to avoid the collision between the student and the other students
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!timerToDestroy && collision.gameObject.tag == "Player") {
            var slowDownTime = Random.Range(downSlowDownTime, topSlowDownTime);
            collision.gameObject.GetComponent<PlayerControler>().stepInOtherStudent(slowDownTime);
            timerLeftToDestroy = slowDownTime;
            timerToDestroy = true;
        }
    }
}
