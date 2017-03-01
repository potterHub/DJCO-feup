using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    // move to the game control and acess trought game control
    public PlayerControler player;

    // first floor
    public GameObject firstFloorGround;
    private float firstFloorgroundHorizontalLenght;
    private Vector3 lastPositionFirstFloor;

    private Queue<GameObject> firstFloorGroundList;

    // second floor
    public GameObject secondFloorGround;
    private float secondFloorgroundHorizontalLenght;
    private Vector3 lastPositionSecondFloor;

    private Queue<GameObject> secondFloorGroundList;
        
    // Use this for initialization
    void Start () {
        firstFloorGroundList = new Queue<GameObject>();
        firstFloorgroundHorizontalLenght = firstFloorGround.GetComponent<BoxCollider2D>().size.x;

        lastPositionFirstFloor = firstFloorGround.transform.position;
        lastPositionFirstFloor.x -= firstFloorgroundHorizontalLenght * 2;
        while (lastPositionFirstFloor.x < player.transform.position.x + firstFloorgroundHorizontalLenght * 4)
            addNewFirstFloorToScene();

        GameObject.Find("1stFloor").SetActive(false);// desactivate the  1st plataform (so that does not appear in the scene)
        

        secondFloorGroundList = new Queue<GameObject>();
        secondFloorgroundHorizontalLenght = secondFloorGround.GetComponent<BoxCollider2D>().size.x;

        lastPositionSecondFloor = secondFloorGround.transform.position;
        lastPositionSecondFloor.x -= secondFloorgroundHorizontalLenght * 2;
        while (lastPositionSecondFloor.x < player.transform.position.x + secondFloorgroundHorizontalLenght * 4)
            addNewSecondFloorToScene();

        GameObject.Find("2ndFloor").SetActive(false);// desactivate the  2nd plataform (so that does not appear in the scene)
    }

    private void addNewFirstFloorToScene() {
        lastPositionFirstFloor.x += firstFloorgroundHorizontalLenght;
        var newObj = Instantiate(firstFloorGround, lastPositionFirstFloor, transform.rotation);
        newObj.transform.parent = GameObject.Find("Scenery").transform;
        firstFloorGroundList.Enqueue(newObj);
    }

    private void addNewSecondFloorToScene()
    {
        lastPositionSecondFloor.x += secondFloorgroundHorizontalLenght;
        var newObj = Instantiate(secondFloorGround, lastPositionSecondFloor, transform.rotation);
        newObj.transform.parent = GameObject.Find("Scenery").transform;
        secondFloorGroundList.Enqueue(newObj);
    }

    private void rotateFirstQueuePlataform() {
        var obj = firstFloorGroundList.Dequeue();
        lastPositionFirstFloor.x += firstFloorgroundHorizontalLenght;
        obj.transform.position = lastPositionFirstFloor;
        firstFloorGroundList.Enqueue(obj);
    }

    private void rotateSecondQueuePlataform() {
        var obj = secondFloorGroundList.Dequeue();
        lastPositionSecondFloor.x += secondFloorgroundHorizontalLenght;
        obj.transform.position = lastPositionSecondFloor;
        secondFloorGroundList.Enqueue(obj);
    }

    public void levelGenSolidSecond() {
        Queue<GameObject> temp = new Queue<GameObject>(secondFloorGroundList);
        do{            
            Debug.Log("ola - " + temp.Count);
            temp.Dequeue().GetComponent<BoxCollider2D>().isTrigger = false;
        }while (temp.Count > 0);
    }
    public void levelGenTriggerSecond(){
        Queue<GameObject> temp = new Queue<GameObject>(secondFloorGroundList);
        do { 
            Debug.Log("adeus - " + temp.Count);
            temp.Dequeue().GetComponent<BoxCollider2D>().isTrigger = true;
        } while (temp.Count > 0);
    }

    // Update is called once per frame
    void Update() {
        if (lastPositionFirstFloor.x < player.transform.position.x + firstFloorgroundHorizontalLenght * 3)
            rotateFirstQueuePlataform();

        if (lastPositionSecondFloor.x < player.transform.position.x + secondFloorgroundHorizontalLenght * 3)
            rotateSecondQueuePlataform();
    }
}
