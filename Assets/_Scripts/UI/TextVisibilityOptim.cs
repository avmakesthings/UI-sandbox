using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextVisibilityOptim : MonoBehaviour {


    //private Text textLabel;
    private Transform parentTransform;
    private Transform player;

    void Start()
    {
        //textLabel = this.GetComponent<Text>();
        parentTransform = this.GetComponentInParent<Transform>();
        player = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        parentTransform.LookAt(player);

    }

    //void resizeLableByViewDistance()
    //{
        //float labelScale = 0.3f + Vector3.Distance(transform.position, player.transform.position) * 0.3f;
        //textLabel.transform.localScale = new Vector3(labelScale, labelScale, labelScale);
    //}



}
