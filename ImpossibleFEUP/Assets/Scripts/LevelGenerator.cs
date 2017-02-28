using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    public PlayerControler player;
    public GameObject firstFloorGround;
    private float firstFloorgroundHorizontalLenght;

    private Vector3 lastPosition;

    private Queue<GameObject> firstFloorGroundList;

    // Use this for initialization
    void Start () {
        firstFloorGroundList = new Queue<GameObject>();
        firstFloorgroundHorizontalLenght = firstFloorGround.GetComponent<BoxCollider2D>().size.x;

        lastPosition = firstFloorGround.transform.position;
        lastPosition.x -= firstFloorgroundHorizontalLenght * 2;
        while (lastPosition.x < player.transform.position.x + firstFloorgroundHorizontalLenght * 4)
            addNewFloorToScene();

        GameObject.Find("1stFloor").SetActive(false);// desactivate the  1st plataform (so that does not appear in the scene)
    }

    private void addNewFloorToScene() {
        lastPosition.x += firstFloorgroundHorizontalLenght;
        var newObj = Instantiate(firstFloorGround, lastPosition, transform.rotation);
        newObj.transform.parent = GameObject.Find("Scenery").transform;
        firstFloorGroundList.Enqueue(newObj);
    }

    private void rotatePlataform() {
        var obj = firstFloorGroundList.Dequeue();
        lastPosition.x += firstFloorgroundHorizontalLenght;
        obj.transform.position = lastPosition;
        firstFloorGroundList.Enqueue(obj);
    }

	// Update is called once per frame
	void Update () {
        if (lastPosition.x < player.transform.position.x + firstFloorgroundHorizontalLenght * 3) {
            rotatePlataform();
        }
    }
}
