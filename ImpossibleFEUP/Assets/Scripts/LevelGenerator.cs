using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    // move to the game control and acess trought game control
    private const float numPlataformsInAdvance = 4.0f;

	// background
	public GameObject background;
	private float backgroundHorizontalLength;
	private Vector3 lastPositionBackground;

	private Queue<GameObject> backgroundList;

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
    public float minTimeToSpawn = 0.5f;
    public float smallerTimeToSpawn = 2f;//2.0f;
    public float biggestTimeToSpawn = 3f;//2.0f;
    private float timerTimeToInc = 0.0f;

    public float downTimeToIncreaseSpawn = 20f;
    public float topTimeToIncreaseSpawn = 30f;
    private float timerLeftToSpwan = 0.0f;

    // beer
    public GameObject beerObject;
    // other student
    public GameObject otherStudentObject;

    // coffe Object
    public GameObject coffeObject;

    // other Random Object
    public GameObject otherObject;


    private const float circRadius = 1.15f;
    // temp sphere
    //public GameObject tempSphere;

    // Use this for initialization
    void Start()
    {
        Random.InitState(unchecked((int)System.DateTime.Now.Ticks));

		// background
		backgroundList = new Queue<GameObject>();
		backgroundHorizontalLength = background.GetComponent<SpriteRenderer>().sprite.bounds.max.x - background.GetComponent<SpriteRenderer>().sprite.bounds.min.x;

		lastPositionBackground = background.transform.position;
		lastPositionBackground.x -= backgroundHorizontalLength;
		while (lastPositionBackground.x < GameController.instance.player.transform.position.x + backgroundHorizontalLength) {
			addNewBackgroundToScene ();
		}

		GameObject.Find("feup").SetActive(false);

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
        timerTimeToInc = Random.Range(downTimeToIncreaseSpawn, topTimeToIncreaseSpawn);
        timerLeftToSpwan = Random.Range(smallerTimeToSpawn, biggestTimeToSpawn);
        
        // beer object
        beerObject.SetActive(false);

        // other student
        otherStudentObject.SetActive(false);

        // coffe object
        coffeObject.SetActive(false);

        // other Random Object
        otherObject.SetActive(false);
    }

	private void addNewBackgroundToScene() {
		lastPositionBackground.x += backgroundHorizontalLength;
		var newObj = Instantiate(background, lastPositionBackground, transform.rotation);
		newObj.transform.parent = GameObject.Find("Scenery").transform;
		backgroundList.Enqueue(newObj);
	}

    private void addNewFirstFloorToScene()
    {
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

	private void rotateBackgroundQueuePlataform() {
		var obj = backgroundList.Dequeue();
		lastPositionBackground.x += backgroundHorizontalLength;
		obj.transform.position = lastPositionBackground;
		backgroundList.Enqueue(obj);
	}

    private void rotateFirstQueuePlataform()
    {
        var obj = firstFloorGroundList.Dequeue();
        lastPositionFirstFloor.x += firstFloorgroundHorizontalLenght;
        obj.transform.position = lastPositionFirstFloor;
        firstFloorGroundList.Enqueue(obj);
    }

    private void rotateSecondQueuePlataform()
    {
        var obj = secondFloorGroundList.Dequeue();
        lastPositionSecondFloor.x += secondFloorgroundHorizontalLenght;
        obj.transform.position = lastPositionSecondFloor;
        secondFloorGroundList.Enqueue(obj);
    }

    public void levelGenSolidSecond()
    {
        Queue<GameObject> temp = new Queue<GameObject>(secondFloorGroundList);
        do
        {
            temp.Dequeue().GetComponent<BoxCollider2D>().isTrigger = false;
        } while (temp.Count > 0);
    }
    public void levelGenTriggerSecond()
    {
        Queue<GameObject> temp = new Queue<GameObject>(secondFloorGroundList);
        do
        {
            temp.Dequeue().GetComponent<BoxCollider2D>().isTrigger = true;
        } while (temp.Count > 0);
    }

    // Update is called once per frame
    void Update()
    {        

		if (lastPositionBackground.x < GameController.instance.player.transform.position.x + backgroundHorizontalLength/2 - 10)
			rotateBackgroundQueuePlataform();
		
        if (lastPositionFirstFloor.x < GameController.instance.player.transform.position.x + firstFloorgroundHorizontalLenght * numPlataformsInAdvance)
            rotateFirstQueuePlataform();

        if (lastPositionSecondFloor.x < GameController.instance.player.transform.position.x + secondFloorgroundHorizontalLenght * numPlataformsInAdvance)
            rotateSecondQueuePlataform();

        destroyObjects();

        if (!GameController.instance.player.isPlayerDead()) {
            timerLeftToSpwan -= Time.deltaTime;
            timerTimeToInc -= Time.deltaTime;
            if (timerLeftToSpwan <= 0f)
                creatObjects();
            if (timerTimeToInc <= 0f && smallerTimeToSpawn > minTimeToSpawn)
            {
                timerTimeToInc = Random.Range(downTimeToIncreaseSpawn, topTimeToIncreaseSpawn);
                float smallerTime = smallerTimeToSpawn - Random.Range(0.2f, 0.6f);
                smallerTimeToSpawn = smallerTime > minTimeToSpawn ? smallerTime : minTimeToSpawn;
                biggestTimeToSpawn = smallerTimeToSpawn + 0.5f;

                //Debug.Log("decrease time to spawn " + smallerTimeToSpawn + " -> " + biggestTimeToSpawn);
            }
        }
}

    private void destroyObjects()
    {
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

    private void creatObjects()
    {
        // getting camera dimensions to spawn the object outside of the screen before the player passes by it
        var vertExtent = GameController.instance.gameCamera.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;
        float endOfScreen = GameController.instance.gameCamera.transform.position.x + horzExtent;

        // inc camera size so the objects appear after the camera
        GameObject newObj;
        int type = Random.Range(0, 20);
        if (type < 10)
            newObj = generateNewObject(ref beerObject, endOfScreen);
        else if (type < 16)
            newObj = generateNewObject(ref otherStudentObject, endOfScreen);
        else if (type < 18)
            newObj = generateNewObject(ref otherObject, endOfScreen);
        else
            newObj = generateNewObject(ref coffeObject, endOfScreen);

        // setting object active and adding it to the scene
        if (newObj != null) {
            newObj.SetActive(true);
            objectsInGame.Add(newObj);
        }
        // after creating reset timer to spawn
        timerLeftToSpwan = Random.Range(smallerTimeToSpawn, biggestTimeToSpawn);
        //Debug.Log("generating " + timerLeftToSpwan);
    }


    // generates objects by generating a random position and check if there is an object there already (if there is an object is not added)
    // i think this is the better way because if there is to much objects it less probable to spawn in empty position wich make it harder to completely fill the map
    private GameObject generateNewObject(ref GameObject obj, float endOfScreen) {
            Vector3 newPos = new Vector3(Random.Range(endOfScreen, lastPositionFirstFloor.x), obj.transform.position.y, 0);

            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(new Vector2(newPos.x, newPos.y), circRadius);
            if (hitColliders.Length == 0) {// checks if there is an object in the random object place
                GameObject newObj = Instantiate(obj, newPos, transform.rotation);
                newObj.transform.parent = GameObject.Find("Scenery").transform;
                return newObj;
            }
        return null;
    }
}
    
