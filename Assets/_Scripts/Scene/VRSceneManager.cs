using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRSceneManager : MonoBehaviour {

	public void LoadScene(string levelName){
		Application.LoadLevel(levelName);
	}

	public void quit() {
		Application.Quit();
	}
}
