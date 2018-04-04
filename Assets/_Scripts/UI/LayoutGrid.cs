using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//a class for creating a visual layout grid to check dimensions in VR
public class LayoutGrid : MonoBehaviour {

    public Plane orientationPlane;
    public int cellSize;
    public int numX, numY;

    private Transform location;
    private Vector3[] vertices;



    // Use this for initialization
    void Awake () {
        location = this.GetComponent<Transform>();
        generateGridPts();


    }
	
    private void generateGridPts()
    {
        vertices = new Vector3[(numX + cellSize) * (numY + cellSize)];
        for (int i = 0, y = 0; y <= numY; y++)
        {
            for (int x = 0; x <= numX; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null)
        {
            return;
        }
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }

    void drawLines()
    {

    }

    void addLabels()
    {

    }
}
