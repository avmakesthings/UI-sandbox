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

        myTransform.LookAt(player.transform);
    }
}
