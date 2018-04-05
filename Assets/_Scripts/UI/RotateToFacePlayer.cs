using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateToFacePlayer : MonoBehaviour {

    private Transform myTransform;
    public GameObject player;

    void Start()
    {
        myTransform = this.GetComponent<Transform>();


    }

    void Update()
    {
        //Vector3 v = player.transform.position - myTransform.position;
        //v.x = v.z = 0.0f;
        //transform.LookAt(cameraToLookAt.transform.position - v);
        //transform.Rotate(0, 180, 0);

        myTransform.LookAt(player.transform);
    }
}
