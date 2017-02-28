using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour {

    public float playerSpeed;
    public float jumpSpeed;

    private Rigidbody2D playerRgBody;

    public LayerMask firstFloor;
    public bool isGrounded;

    private Collider2D playerColider;

    private float startX = 0f;
    private float meters;
    public Text scoreText;
    public Text metersText;
    
    // Use this for initialization
    void Start () {
        metersText.text = "meters: 0m";
        scoreText.text = "score: 0";
        playerRgBody = GetComponent<Rigidbody2D>();
        playerColider = GetComponent<Collider2D>();
        startX = transform.position.x;
    }

    // Update is called once per 
	void Update () {
        isGrounded = Physics2D.IsTouchingLayers(playerColider, firstFloor);

        playerRgBody.velocity = new Vector2(playerSpeed, playerRgBody.velocity.y);
        if (isGrounded && Input.GetMouseButtonDown(0)) {
            playerRgBody.velocity = new Vector2(playerRgBody.velocity.x, jumpSpeed);
        }
        float meters = ((transform.position.x - startX) / 2f);
        metersText.text = "meters: " + meters.ToString("0.00") + "m"; 
	}
}
