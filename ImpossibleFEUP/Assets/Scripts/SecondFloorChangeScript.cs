using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondFloorChangeScript : MonoBehaviour {
    public float dropTimer = 6.0f;
    private float timerLeftToDrop = 0.0f;
    private bool timerToDrop;

    // Use this for initialization
    void Start () {
        timerToDrop = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (timerToDrop)
        {
            timerLeftToDrop -= Time.deltaTime;
            if (timerLeftToDrop < 0)
            {
                var levelGen = GameController.instance.scenery.GetComponent<LevelGenerator>();
                levelGen.levelGenTriggerSecond();
                timerToDrop = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!timerToDrop && collision.gameObject.tag == "Player")
        {
            var playerControler = GameController.instance.player.GetComponent<PlayerControler>();
            if (playerControler.isOnSecondFloor()) {
                Debug.Log("To first Floor");
                playerControler.setOnSecondFloorTo(false);
            } else {
                var player = collision.gameObject.GetComponent<PlayerControler>();
                // Debug.Log(player.gameObject.transform.position.y + " - " + transform.position.y);
                if (player.transform.position.y > transform.position.y)
                {
                    Debug.Log("To second Floor");
                    GameController.instance.scenery.GetComponent<LevelGenerator>().levelGenSolidSecond();// change to solid
                    playerControler.setOnSecondFloorTo(true);
                    timerLeftToDrop = dropTimer;
                    timerToDrop = true;
                }
            }
        }
    }
}
