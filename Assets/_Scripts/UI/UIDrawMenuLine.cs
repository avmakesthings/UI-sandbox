using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDrawMenuLine : MonoBehaviour {

    public Vector3[] points;

    public string animationTrigger;
    public Animator animator;

    private LineRenderer lineRenderer;

	// Use this for initialization
	void Start () {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, points[0]);

	}

    // Update is called once per frame
    void Update()
    { }


    public void drawLine()
    {
        print("drawLine");
    }

    IEnumerator animateLine()
    {
        yield return null;
    }


}
