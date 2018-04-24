using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//this is a script that is attached to a button. when the button is clicked once, it triggers an animation
//when it is clicked again, it triggers a different animation 

public class UIToggleButtonAnim : MonoBehaviour {


    public string animTrigger, reverseAnimTrigger;
    public Animator myAnimator;
    private int counter;

	// Use this for initialization
	void Start () {
        counter = 0;
	}
	

    public void triggerAnimation()
    {

        if(counter == 0)
        {
            myAnimator.SetTrigger(animTrigger);
        }else if (counter == 1)
        {
            myAnimator.SetTrigger(reverseAnimTrigger);
        }

        counter = 1 - counter;
    }

}
