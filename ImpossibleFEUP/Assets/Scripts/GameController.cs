using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
    public static GameController instance;

	public GameObject soundManager;
    public Camera gameCamera;
    public LevelGenerator scenery;
    public PlayerControler player;

	private bool scoresShown;
	private Scores scores;
	public Text scoresText;
	public GameObject scoresPanel;

	public List<GameObject> UI;
	public GameObject menu;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SoundController.instance.playMusic(music.bossfight_commando_steve, true);
    }

	void Start () {
		scoresShown = false;
		for (int i = 0; i < UI.Count; i++) {
			UI [i].SetActive (false);
		}
	}

    // Update is called once per frame
    void Update () {

		if(player.getIsDead()) {
			if (!scoresShown)
				showScores ();
		}

		if (StaticLevelState.getState () == 0) {
			menu.SetActive (true);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (false);
			}

		} else {
			menu.SetActive (false);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (true);
			}
		}
			
	}

	public void showScores() {

		if (!System.IO.File.Exists(Application.persistentDataPath + "/scores")) {
			scores = new Scores ();
		} else {
			scores = FileManager.ReadFromBinaryFile<Scores> (Application.persistentDataPath + "/scores");
		}

		if (scores.getScores ().Count < 5) {
			scores.addScore ("Eduardo", player.getMeters());
			scores.getScores ().Sort (scores.SortByScore);
		} else if (player.getMeters() > scores.getScores () [4].Value) {
			Debug.Log ("é maior");
			scores.addScore ("Eduardo", player.getMeters());
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

	public void restartGame() {
		StaticLevelState.changeState (1);
		SceneManager.LoadScene ("ImpossibleFeupPrototype");
	}

	public void startGame() {
		StaticLevelState.changeState (1);
		SoundController.instance.playMusic(music.bossfight_commando_steve, true);
	}

	public void exitGame() {
		Application.Quit ();
	}
}
