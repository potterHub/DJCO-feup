using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
	private bool gameStarted;

    public LayerMask firstFloor;
    public LayerMask secondFloor;
    public LayerMask otherObject;

    public HealthBarScript healthBar;
    public static float maxHealth = 100f;
    public float Health;

    public Text metersText;
    private float meters;

    public Text scoresText;
    private bool scoresShown;

    private bool isDead = false;
    private int numCoffes = 0;

	public GameObject scoresPanel;
	private Scores scores;

    private const float topTimeToIncreaseSpeed = 30f;
    private const float downTimeToIncreaseSpeed = 20f;
    private float timerLeftToIncrease;
    public float playerInitialSpeed = 8f;
    public float playerMaxSpeed = 20f;
    private float playerCurrentSpeed;

    private Rigidbody2D playerRgBody;
    private Collider2D playerColider;

    public float jumpSpeed;
    private bool doubleJump;
    private bool onSecondFloor;


    private float timerLeftSlowDown;

	private float timePlayed;

    private bool haveColideWithOtherStudent;
    // Use this for initialization
    void Start() {
		timePlayed = 0;
		gameStarted = false;
		meters = 0;
        isDead = false;
        doubleJump = false;
        numCoffes = 0;

        metersText.text = "Meters: 0m";
        playerRgBody = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<Collider2D>();

        Health = PlayerControler.maxHealth;
        healthBar.updateBar(Health / PlayerControler.maxHealth);

        timerLeftSlowDown = 0.0f;

        timerLeftToIncrease = Random.Range(downTimeToIncreaseSpeed,topTimeToIncreaseSpeed);
        playerCurrentSpeed = playerInitialSpeed;

        haveColideWithOtherStudent = false;
		scoresShown = false;
    }
    public bool isPlayerDead() {
        return isDead;
    }

    public int getNumCoffes() {
        return numCoffes;
    } 
    public void coffePicked()
    {
        if (!isDead && numCoffes < 3)
            numCoffes++;
    }

    public bool useCoffe() {
        if (isDead || numCoffes <= 0)
            return false;
        numCoffes--;
        return true;
    }

    public void stepInBeer() {
        if (!isDead) {
            Health -= 25;
            if (Health <= 0)
                killPlayer();
            healthBar.updateBar(Health / PlayerControler.maxHealth);
        }
    }

    public void killPlayer() {
        Health = 0;
        isDead = true;
        playerCurrentSpeed = 0f;

        playerRgBody.velocity = new Vector2(0, playerRgBody.velocity.y);

        SoundController.instance.stopMusic();
        SoundController.instance.playMusic(music.player_death, false);
        // play dead animation

    }

    public void stepInOtherStudent(float slowDownTime) {
        if (!isDead) {
            timerLeftSlowDown = slowDownTime;
            haveColideWithOtherStudent = true;
        }
    }

    public void jumpingToSecondFloor() {
        if (!isDead) {
            this.onSecondFloor = doubleJump = true;
        }
    }

    public void fallingFromSecondFloor() {
        if (!isDead)
            this.onSecondFloor = doubleJump = false;
    }

    public bool isOnSecondFloor() {
        return onSecondFloor;
    }

    public bool hasCoffe() {
        return numCoffes > 0;
    }

    // Update is called once per 
    void Update () {
		if (!isDead) {
			if (haveColideWithOtherStudent) {
				timerLeftSlowDown -= Time.deltaTime;
				if (timerLeftSlowDown < 0)
					haveColideWithOtherStudent = false;
			}

            // secondFloor colider is importante to make the double jump to the secound floor only
            bool _1st_grounded = Physics2D.IsTouchingLayers(playerColider, firstFloor) || Physics2D.IsTouchingLayers(playerColider, otherObject);
            //bool _2nd_touching = Physics2D.IsTouchingLayers(playerColider, secondFloor);

            if (_1st_grounded)
                doubleJump = false;
            //if (_2nd_touching)
            //    doubleJump = false;

            //isGrounded = _1st_grounded; //|| _2nd_touching;

            playerRgBody.velocity = new Vector2(!haveColideWithOtherStudent ? playerCurrentSpeed : (playerCurrentSpeed / 2.0f), playerRgBody.velocity.y);
            if (Input.GetMouseButtonDown(0)) {
                if (_1st_grounded) {
                    playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
                    SoundController.instance.playEffect(effect.jump);
                } else if (!doubleJump) {
                    playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
                    SoundController.instance.playEffect(effect.jump);
                    doubleJump = true;
                }
            }         

			// gui update
			timePlayed += Time.deltaTime;

            meters += (playerCurrentSpeed * Time.deltaTime) / 2f;
			metersText.text = "Meters: " + meters.ToString ("0.00") + "m";

            // speed up level
            timerLeftToIncrease -= Time.deltaTime;
            if (timerLeftToIncrease <= 0f && playerCurrentSpeed < playerMaxSpeed) {
                timerLeftToIncrease = Random.Range(downTimeToIncreaseSpeed, topTimeToIncreaseSpeed);
                float speed = playerCurrentSpeed + Random.Range(1.0f,1.5f);
                playerCurrentSpeed += speed < playerMaxSpeed ? speed : playerMaxSpeed;
                Debug.Log("acelarate " + playerCurrentSpeed);
            }

            if (playerCurrentSpeed >= playerMaxSpeed)
                Debug.Log("max Speed reached");

            if (Health <= 0)
                killPlayer();
        } else {
			if (!scoresShown)
				showScores ();
		}
	}

	private void showScores() {
		
		if (!System.IO.File.Exists(Application.persistentDataPath + "/scores")) {
			scores = new Scores ();
		} else {
			scores = FileManager.ReadFromBinaryFile<Scores> (Application.persistentDataPath + "/scores");
		}

		if (scores.getScores ().Count < 5) {
			scores.addScore ("Eduardo", meters);
			scores.getScores ().Sort (scores.SortByScore);
		} else if (meters > scores.getScores () [4].Value) {
			Debug.Log ("é maior");
			scores.addScore ("Eduardo", meters);
			scores.getScores ().Sort (scores.SortByScore);
			scores.getScores ().RemoveAt (5);
		}

		for (int i = 0; i < scores.getScores().Count; i++) {
			scoresText.text += scores.getScores() [i].Key + "         " + scores.getScores()[i].Value.ToString("0.00") + "m" + "\r\n";
		}

		scoresPanel.SetActive (true);

		scoresShown = true;

		FileManager.WriteToBinaryFile (Application.persistentDataPath + "/scores", scores);
	}
}
