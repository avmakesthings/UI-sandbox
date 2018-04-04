using UnityEngine;
using System.Collections;

public class StereoPanorama : MonoBehaviour {
	public GameObject mappingSphere ;
	public GameObject eyeCamera;

	// Use this for initialization
	void Start () {
		mappingSphere = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		mappingSphere.transform.position = eyeCamera.transform.position;
	}
}
