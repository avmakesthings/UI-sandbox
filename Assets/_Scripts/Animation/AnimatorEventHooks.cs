using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEventHooks : MonoBehaviour {

    public void onAppMenuActivate(string s)
    {
        print(s);
    }

    public void onAppMenuDeactivate(string t)
    {
        print(t);
    }

    public void onNavMenuActivate(string u)
    {
        print(u);
    }
	
    public void onNavMenuDeactivate(string f)
    {
        print(f);
    }


}
