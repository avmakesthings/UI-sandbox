using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapObjects : MonoBehaviour {

    public GameObject[] objectsToSwap;
    private GameObject activeObject;

	// Use this for initialization
	void Start () {
        getActiveObject();
    }
	
    public void getActiveObject()
    {
        foreach(GameObject obj in objectsToSwap)
        {
            if (obj.activeSelf)
            {
                activeObject = obj;
            }
        }
    }

    public void setActiveObject(GameObject nextObject)
    {
        getActiveObject();
        print("current active obj" + activeObject);
        activeObject.SetActive(false);
        nextObject.SetActive(true);
        print("next active obj" + activeObject);
    }
}
