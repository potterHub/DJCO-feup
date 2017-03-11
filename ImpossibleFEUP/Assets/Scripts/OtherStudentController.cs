using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherStudentController : MonoBehaviour {

    public float downSlowDownTime = 4.0f;
    public float topSlowDownTime = 6.0f;

    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")// to avoid the collision between the student and the other students
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            var slowDownTime = Random.Range(downSlowDownTime, topSlowDownTime);
            collision.gameObject.GetComponent<PlayerControler>().stepInOtherStudent(slowDownTime);

            SoundController.instance.playEffect(effect.students);
        }
    }
}
