using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICuffQuickAppMenu : MonoBehaviour {

    private bool menuState = false; 

    //toggle menu visibility
    public void toggleMenu()
    {
        menuState = !menuState;
        this.gameObject.SetActive(menuState);
        print(menuState);
    }


}
