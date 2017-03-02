using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {
    // move to the game control and acess trought game control
    private const float numPlataformsInAdvance = 4.0f;

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

    // objects in map
    private List<GameObject> objectsInGame;
    public float smallerTimeToSpawn = 2.0f;
    public float biggestTimeToSpawn = 2.0f;
    private float timerLeftToSpwan = 0.0f;

    // beer
    public GameObject beerObject;
    // other student
    public GameObject otherStudentObject;

    // other Random Object
    public GameObject otherObject;

    // Use this for initialization
    void Start () {
        Random.InitState(unchecked((int)System.DateTime.Now.Ticks));

        // first floor
        firstFloorGroundList = new Queue<GameObject>();
        firstFloorgroundHorizontalLenght = firstFloorGround.GetComponent<BoxCollider2D>().size.x;

        lastPositionFirstFloor = firstFloorGround.transform.position;
        lastPositionFirstFloor.x -= firstFloorgroundHorizontalLenght;
        while (lastPositionFirstFloor.x < GameController.instance.player.transform.position.x + firstFloorgroundHorizontalLenght * (numPlataformsInAdvance + 1))
            addNewFirstFloorToScene();

        GameObject.Find("1stFloor").SetActive(false);// desactivate the  1st plataform (so that does not appear in the scene)

        // second floor
        secondFloorGroundList = new Queue<GameObject>();
        secondFloorgroundHorizontalLenght = secondFloorGround.GetComponent<BoxCollider2D>().size.x;

        lastPositionSecondFloor = secondFloorGround.transform.position;
        lastPositionSecondFloor.x -= secondFloorgroundHorizontalLenght;
        while (lastPositionSecondFloor.x < GameController.instance.player.transform.position.x + secondFloorgroundHorizontalLenght * (numPlataformsInAdvance + 1))
            addNewSecondFloorToScene();

        GameObject.Find("2ndFloor").SetActive(false);// desactivate the  2nd plataform (so that does not appear in the scene)

        // objects in game
        objectsInGame = new List<GameObject>();
        timerLeftToSpwan = Random.Range(smallerTimeToSpawn, biggestTimeToSpawn);

        // beer
        beerObject.SetActive(false);

        // other student
        otherStudentObject.SetActive(false);

        // other Random Object
        otherObject.SetActive(false);
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
        if (lastPositionFirstFloor.x < GameController.instance.player.transform.position.x + firstFloorgroundHorizontalLenght * numPlataformsInAdvance)
            rotateFirstQueuePlataform();

        if (lastPositionSecondFloor.x < GameController.instance.player.transform.position.x + secondFloorgroundHorizontalLenght * numPlataformsInAdvance)
            rotateSecondQueuePlataform();

        destroyObjects();

        timerLeftToSpwan -= Time.deltaTime;
        if (timerLeftToSpwan <= 0)
            creatObjects();
    }

    private void destroyObjects() {
        float limit = firstFloorGroundList.Peek().transform.position.x;
        for (int i = objectsInGame.Count - 1; i >= 0; i--)
        {
            if (objectsInGame[i].transform.position.x <= limit)
            {
                Destroy(objectsInGame[i]);
                objectsInGame.RemoveAt(i);
            }
        }
    }

    private void creatObjects() {
        // getting camera dimensions to spawn the object outside of the screen before the player passes by it
        var vertExtent = GameController.instance.gameCamera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        float endOfScreen = GameController.instance.gameCamera.transform.position.x + horzExtent;
        
        // inc camera size so the objects appear after the camera
        GameObject newObj;
        int type = Random.Range(0, 3);
        switch (type) {
            case 0:
                newObj = Instantiate(beerObject, new Vector3(Random.Range(endOfScreen, lastPositionFirstFloor.x), beerObject.transform.position.y, 0), transform.rotation);
                break;
            case 1:
                newObj = Instantiate(otherStudentObject, new Vector3(Random.Range(endOfScreen, lastPositionFirstFloor.x), beerObject.transform.position.y, 0), transform.rotation);
                break;
            default:
                newObj = Instantiate(otherObject, new Vector3(Random.Range(endOfScreen, lastPositionFirstFloor.x), beerObject.transform.position.y, 0), transform.rotation);
                break;
        }
        // after generating the object check if the object can be placed be listing all the others and verifing its positions (if )


        // setting object active and adding it to the scene
        newObj.SetActive(true);
        objectsInGame.Add(newObj);
        // after creating reset timer to spawn
        timerLeftToSpwan = Random.Range(smallerTimeToSpawn, biggestTimeToSpawn);
    }
}
