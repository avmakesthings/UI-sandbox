using UnityEngine;
using System.Collections;

public class CameraCutSequences : MonoBehaviour {
	

	public GameObject camGobject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		camGobject = Camera.main.gameObject;
		gameObject.transform.position = Camera.main.gameObject.transform.position; 
		gameObject.transform.rotation = Camera.main.gameObject.transform.rotation; 
	}
}
