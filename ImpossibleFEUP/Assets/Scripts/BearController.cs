using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearController : MonoBehaviour {
        
    public ParticleSystem partEffect;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") {
            collision.gameObject.GetComponent<PlayerControler>().stepInBeer(25);

            //partEffect.transform.position = this.transform.position;
            //partEffect.gameObject.SetActive(true);
            //partEffect.Play();

            gameObject.SetActive(false);

            SoundController.instance.playEffect(effect.beer);
        }
    }
}
