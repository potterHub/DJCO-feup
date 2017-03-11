using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachineController : MonoBehaviour {

    public int downLife = 5;
    public int topLife = 15;

    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update() { }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")// to avoid the collision between the student and the other students
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            PlayerControler playerCont = collision.gameObject.GetComponent<PlayerControler>();
            if (playerCont.isOnSecondFloor()) {
                int life = Random.Range(downLife, topLife);
                collision.gameObject.GetComponent<PlayerControler>().stepInVendingMachine((float)life);
                SoundController.instance.playEffect(effect.vendingMachine);
            }
        }
    }
}
