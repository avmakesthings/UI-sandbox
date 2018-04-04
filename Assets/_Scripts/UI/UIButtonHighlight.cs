using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonHighlight : MonoBehaviour {

    
    public Color hoverColor;
    public Color selectedColor;

    private Image buttonImage;
    private bool active;
    private Color buttonColor;

    // Use this for initialization
    void Start () {
        buttonImage = GetComponent<Image>();
        buttonColor = buttonImage.color;
	}

    public void addButtonHighlight()
    {
        print("outside if"+ active);
        if (!active)
        {
            print("inside if"+ active);
            buttonImage.color = hoverColor;
            active = true;
        }
    }

    public void removeButtonHighlight()
    {
        if (active)
        {
            buttonImage.color = buttonColor;
            active = false;
        }
    }

}
