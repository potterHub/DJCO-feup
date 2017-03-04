using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoffeUiController : MonoBehaviour {

    public Sprite[] coffeSprites;
    public Image coffeImage;
    private PlayerControler student;

	// Use this for initialization
	void Start () {
        student = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControler>();
	}
	
	// Update is called once per frame
	void Update () {
        int numCoffes = student.getNumCoffes();
        coffeImage.sprite = coffeSprites[numCoffes];
	}
}
