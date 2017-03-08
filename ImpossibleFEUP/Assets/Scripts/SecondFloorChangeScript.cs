using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecondFloorChangeScript : MonoBehaviour {
    public float dropTimer = 6.0f;
    private float timerLeftToDrop = 0.0f;
    private bool timerToDrop;

    public Text textTimer2ndFloor;

    // Use this for initialization
    void Start () {
        timerToDrop = false;
        textTimer2ndFloor.text = "";
    }
	
	// Update is called once per frame
	void Update () {
        if (timerToDrop) {
            timerLeftToDrop -= Time.deltaTime;
            textTimer2ndFloor.text = "Time To Drop: " + ((int)timerLeftToDrop) + "s";
            if (timerLeftToDrop < 0 || (Input.GetMouseButtonDown(0) && timerLeftToDrop <= dropTimer - 1.0f)) {
                var levelGen = GameController.instance.scenery.GetComponent<LevelGenerator>();
                levelGen.levelGenTriggerSecond();
                timerToDrop = false;
                textTimer2ndFloor.text = "";
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (!timerToDrop && collision.gameObject.tag == "Player") {
            var playerControler = GameController.instance.player.GetComponent<PlayerControler>();
            if (playerControler.isOnSecondFloor()) {// if the player is on the secound floor already
                Debug.Log("To first Floor");
                playerControler.fallingFromSecondFloor();
            } else {
                var player = collision.gameObject.GetComponent<PlayerControler>();
                
                // Debug.Log(player.gameObject.transform.position.y + " - " + transform.position.y);
                if (player.transform.position.y > transform.position.y && player.getNumCoffes() > 0 && player.hasCoffe()) {
                    Debug.Log("To second Floor");
                    player.useCoffe();
                    GameController.instance.scenery.GetComponent<LevelGenerator>().levelGenSolidSecond();// change to solid
                    playerControler.jumpingToSecondFloor();
                    timerLeftToDrop = dropTimer;
                    timerToDrop = true;                    
                }
            }
        }
    }
}
