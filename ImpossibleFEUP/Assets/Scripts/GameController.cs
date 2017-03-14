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

	private Scores scores;
	public Text scoresText;
	public Text namesText;
	public GameObject scoresPanel;

	public List<GameObject> UI;
	public GameObject menu;
	public GameObject gameOverMenu;
	public InputField name;

    // Use this for initialization
    void Awake () {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        SoundController.instance.playMusic(music.bossfight_commando_steve, true);
    }

	void Start () {
		for (int i = 0; i < UI.Count; i++) {
			UI [i].SetActive (false);
		}
	}

    // Update is called once per frame
    void Update () {

		if (StaticLevelState.getState () == 0) {
			menu.SetActive (true);
			gameOverMenu.SetActive (false);
			scoresPanel.SetActive (false);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (false);
			}

		} else if (StaticLevelState.getState () == 1) {
			menu.SetActive (false);
			gameOverMenu.SetActive (false);
			scoresPanel.SetActive (false);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (true);
			}
		} else if (StaticLevelState.getState () == 2) {
			menu.SetActive (false);
			gameOverMenu.SetActive (true);
			scoresPanel.SetActive (false);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (true);
			}
		} else {
			menu.SetActive (false);
			gameOverMenu.SetActive (false);
			for (int i = 0; i < UI.Count; i++) {
				UI [i].SetActive (false);
			}
		}
			
	}

	public void showScores() {

		StaticLevelState.changeState (3);

		if (!System.IO.File.Exists(Application.persistentDataPath + "/scores")) {
			scores = new Scores ();
		} else {
			scores = FileManager.ReadFromBinaryFile<Scores> (Application.persistentDataPath + "/scores");
		}

		scoresPanel.SetActive (true);

		for (int i = 0; i < scores.getScores().Count; i++) {
			namesText.text += scores.getScores() [i].Key + "\r\n";
		}

		for (int i = 0; i < scores.getScores().Count; i++) {
			scoresText.text += scores.getScores()[i].Value.ToString("0.00") + "m" + "\r\n";
		}

		FileManager.WriteToBinaryFile (Application.persistentDataPath + "/scores", scores);
	}

	public void addScore() {

		string nameString;

		if (name.text != "")
			nameString = name.text;
		else
			nameString = "Vasco";

		if (!System.IO.File.Exists(Application.persistentDataPath + "/scores")) {
			scores = new Scores ();
		} else {
			scores = FileManager.ReadFromBinaryFile<Scores> (Application.persistentDataPath + "/scores");
		}
		if (scores.getScores ().Count < 5) {
			scores.addScore (nameString, player.getMeters());
			scores.getScores ().Sort (scores.SortByScore);
		} else if (player.getMeters() > scores.getScores () [4].Value) {
			scores.addScore (nameString, player.getMeters());
			scores.getScores ().Sort (scores.SortByScore);
			scores.getScores ().RemoveAt (5);
		}

		name.interactable = false;

		FileManager.WriteToBinaryFile (Application.persistentDataPath + "/scores", scores);

		showScores ();
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

	public void showMenu() {
		StaticLevelState.changeState (0);
		SceneManager.LoadScene ("ImpossibleFeupPrototype");
	}

	public void showMenuWithoutRestart() {
		StaticLevelState.changeState (0);
	}
}
