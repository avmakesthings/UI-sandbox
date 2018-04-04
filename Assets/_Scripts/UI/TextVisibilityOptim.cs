using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextVisibilityOptim : MonoBehaviour {


    private Text textLabel;
    private Transform player;

    void Start()
    {
        textLabel = this.GetComponent<Text>();
        print(GameObject.FindWithTag("MainCamera"));
        //player = GameObject.FindWithTag("MainCamera").transform;
    }

    void Update()
    {
        textLabel.transform.LookAt(player);
    }

    void resizeLableByViewDistance()
    {
        float labelScale = 0.3f + Vector3.Distance(transform.position, player.transform.position) * 0.3f;
        textLabel.transform.localScale = new Vector3(labelScale, labelScale, labelScale);
    }



}
