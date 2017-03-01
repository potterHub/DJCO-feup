using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour {

    public PlayerControler student;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerControler>().stepInBeer();
            Destroy(gameObject);
        }
    }
}
