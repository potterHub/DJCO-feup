using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {
    private Animator anim;
    public LayerMask firstFloor;
    private bool isGrounded;

    public HealthBarScript healthBar;
    public static float maxHealth = 100f;
    private float Health;

    public Text metersText;
	private float meters;

    private bool isDead = false;
    private int numCoffes = 0;

    private const float topTimeToIncreaseSpeed = 20f;
    private const float downTimeToIncreaseSpeed = 15f;
    private float timerLeftToIncrease;
    public float playerInitialSpeed = 6f;
    public float playerMaxSpeed = 20f;
    private float playerCurrentSpeed;

    private Rigidbody2D playerRgBody;
    private Collider2D playerColider;

    public float jumpSpeed;
    private bool doubleJump;
    private bool onSecondFloor;

	public GameObject boost;
	public GameObject cantJump;

    private float timerLeftSlowDown;

	private float timePlayed;

    private bool haveColideWithOtherStudent;
    // Use this for initialization
    void Start() {
		timePlayed = 0;
		meters = 0;
        isDead = false;
        isGrounded = false;
        doubleJump = false;
        numCoffes = 0;

        metersText.text = "0 m";
        anim = GetComponent<Animator>();
        playerRgBody = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<Collider2D>();

        Health = PlayerControler.maxHealth;
        healthBar.updateBar(Health / PlayerControler.maxHealth);

        timerLeftSlowDown = 0.0f;

        timerLeftToIncrease = Random.Range(downTimeToIncreaseSpeed,topTimeToIncreaseSpeed);
        playerCurrentSpeed = playerInitialSpeed;

        haveColideWithOtherStudent = false;

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
        SoundController.instance.playEffect(effect.coffe);
        return true;
    }

    public void stepInBeer(float heathToLose) {
        if (!isDead) {
            Health -= heathToLose;
            if (Health <= 0)
                killPlayer();
            healthBar.updateBar(Health / PlayerControler.maxHealth);
        }
    }

    public void stepInVendingMachine(float lifeGain) {
        if (!isDead) {
            Health += lifeGain;
            if (Health > PlayerControler.maxHealth)
                Health = PlayerControler.maxHealth;
            healthBar.updateBar(Health / PlayerControler.maxHealth);
        }
    }

    public void killPlayer() {
        Health = 0;
        isDead = true;
        playerCurrentSpeed = 0f;

        setPlayerHorizontalSpeed(0);

        anim.SetTrigger("dead");
        //Debug.Log("die");

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

    private void setPlayerHorizontalSpeed(float speedHor) {
        playerRgBody.velocity = new Vector2(speedHor, playerRgBody.velocity.y);

        // **********************************
        anim.SetFloat("speed", speedHor / 2.0f);
        // **********************************
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
			isGrounded = Physics2D.IsTouchingLayers (playerColider, firstFloor); //|| Physics2D.IsTouchingLayers(playerColider, otherObject);
			if (isGrounded)
				doubleJump = false;
			setPlayerHorizontalSpeed (!haveColideWithOtherStudent ? playerCurrentSpeed : (playerCurrentSpeed / 2.0f));
			if (StaticLevelState.getState() == 1) {
				if (Input.GetMouseButtonDown (0) && !haveColideWithOtherStudent) {
					if (isGrounded) {
						playerRgBody.velocity = new Vector2 (playerRgBody.velocity.x, jumpSpeed);
						SoundController.instance.playEffect (effect.jump);
					} else if (!doubleJump) {
						playerRgBody.velocity = new Vector2 (playerRgBody.velocity.x, jumpSpeed);
						SoundController.instance.playEffect (effect.jump);
						doubleJump = true;
					}
				}         

				// gui update
				timePlayed += Time.deltaTime;

				meters += (playerCurrentSpeed * Time.deltaTime) / 2f;
				metersText.text = meters.ToString ("0.00 ") + "m";

				// speed up level
				timerLeftToIncrease -= Time.deltaTime;
				if (timerLeftToIncrease <= 0f && playerCurrentSpeed < playerMaxSpeed) {
					timerLeftToIncrease = Random.Range (downTimeToIncreaseSpeed, topTimeToIncreaseSpeed);
					float speed = playerCurrentSpeed + Random.Range (1.0f, 2f);
					playerCurrentSpeed = speed < playerMaxSpeed ? speed : playerMaxSpeed;
					//Debug.Log("acelarate " + playerCurrentSpeed);
				}


				// jump animation 
				anim.SetFloat ("jump", playerRgBody.velocity.y);


				if (Health <= 0)
					killPlayer ();
			} 
		}

		if (haveColideWithOtherStudent)
			cantJump.SetActive (true);
		else
			cantJump.SetActive (false);

		if (isOnSecondFloor())
			boost.SetActive (true);
		else
			boost.SetActive(false);
	}

	public bool getIsDead() {
		return isDead;
	}

	public float getMeters() {
		return meters;
	}
}
