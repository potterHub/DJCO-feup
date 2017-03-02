using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherStudentController : MonoBehaviour {

    public PlayerControler student;

    public float slowDownTime = 5.0f;
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
            if (timerLeftToDestroy < 0) {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!timerToDestroy && collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerControler>().stepInOtherStudent(slowDownTime);
            timerLeftToDestroy = slowDownTime;
            timerToDestroy = true;
        }
    }
}
