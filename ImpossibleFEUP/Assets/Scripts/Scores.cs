using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Scores {

	private List<KeyValuePair<string, float>> scores = new List<KeyValuePair<string, float>>();

	public void addScore(string name, float score) {
		KeyValuePair<string,float> temp = new KeyValuePair<string,float> (name, score);
		scores.Add (temp);
	}

	public List<KeyValuePair<string, float>> getScores() {
		return scores;
	}

	public int SortByScore(KeyValuePair<string, float> k1, KeyValuePair<string, float> k2) {
		return k2.Value.CompareTo (k1.Value);
	}
}
