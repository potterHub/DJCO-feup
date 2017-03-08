using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
	private bool gameStarted;
    public HealthBarScript healthBar;
    private bool isDead = false;
    private int numCoffes = 0;
	public GameObject scoresPanel;
	private Scores scores;
	public Text scoresText;
	private bool scoresShown;

    public float playerSpeed;
    private float playerCurrentSpeed;
    public float jumpSpeed;

    private Rigidbody2D playerRgBody;

    public LayerMask firstFloor;
    public LayerMask secondFloor;
	public LayerMask otherObject;
    
    private bool doubleJump;
    private bool onSecondFloor;
    private bool isGrounded;

    private Collider2D playerColider;

    private float startX = 0f;
    private float meters;
   
    public Text metersText;

    public static float maxHealth = 100;
    public float Health;

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
        startX = transform.position.x;

        healthBar.updateBar(Health / PlayerControler.maxHealth);

        timerLeftSlowDown = 0.0f;

        playerCurrentSpeed = playerSpeed;

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
            Health -= 10;
            if (Health < 0) {
                Health = 0;
                isDead = true;
                playerCurrentSpeed = 0f;
                // play dead animation
            }
            healthBar.updateBar(Health / PlayerControler.maxHealth);
        }
    }

    public void stepInOtherStudent(float slowDownTime) {
        if (!isDead) {
            timerLeftSlowDown = slowDownTime;
            playerCurrentSpeed = playerSpeed / 3.0f;
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
				if (timerLeftSlowDown < 0) {
					haveColideWithOtherStudent = false;
					playerCurrentSpeed = playerSpeed;
				}
			}

            // secondFloor colider is importante to make the double jump to the secound floor only
            bool _1st_grounded = Physics2D.IsTouchingLayers(playerColider, firstFloor) || Physics2D.IsTouchingLayers(playerColider, otherObject);
            bool _2nd_touching = Physics2D.IsTouchingLayers(playerColider, secondFloor);

            if (_1st_grounded)
                doubleJump = false;
            //if (_2nd_touching)
            //    doubleJump = false;

            isGrounded = _1st_grounded; //|| _2nd_touching;
            playerRgBody.velocity = new Vector2(playerCurrentSpeed, playerRgBody.velocity.y);
            if (Input.GetMouseButtonDown(0)) {
                if (_1st_grounded) {
                    playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
                } else if (!doubleJump) {
                    playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
                    doubleJump = true;
                }
            }         

			// gui update
			timePlayed += Time.deltaTime;
			if (timePlayed < 0.2)
				meters = 0;
			else
				meters += (timePlayed * playerCurrentSpeed)/3000f;
			metersText.text = "Meters: " + meters.ToString ("0.00") + "m";
			if (Health <= 0)
				isDead = true;
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
			scores.addScore ("Eduardo", meters);
			scores.getScores ().Sort (scores.SortByScore);
			scores.getScores ().RemoveAt (4);
		}

		for (int i = 0; i < scores.getScores().Count; i++) {
			scoresText.text += scores.getScores() [i].Key + "         " + scores.getScores()[i].Value.ToString("0.00") + "m" + "\r\n";
		}

		scoresPanel.SetActive (true);

		scoresShown = true;

		FileManager.WriteToBinaryFile (Application.persistentDataPath + "/scores", scores);
	}
}
