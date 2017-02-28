using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public PlayerControler student;

    private Vector3 lastPlayerPosition;
    private float distanceToMove;

	// Use this for initialization
	void Start () {
        lastPlayerPosition = student.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x + (student.transform.position.x - lastPlayerPosition.x), 
                                            transform.position.y + ((student.transform.position.y - lastPlayerPosition.y) / 2.5f),
                                            transform.position.z);
        lastPlayerPosition = student.transform.position;
	}
}
