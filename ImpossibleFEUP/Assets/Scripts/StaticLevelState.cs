using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticLevelState {

	private static int state = 0; //0 - Menu; 1 - Playing

	public static void changeState(int stateTemp) {
		state = stateTemp;
	}

	public static int getState() {
		return state;
	}
}
