using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//a class that plays an animation on a button click, than reverses it when the buton is clicked again 
public class UIToggleAnimation : MonoBehaviour {

    public Animator anim;
    public string animationName;
    public string triggerName;
    private bool reverse;


    public void toggleState()
    {

        if (anim.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            print("STILL PLAYING");                                                    
        }

        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && !reverse)
        {

            anim.SetFloat("Direction", 1f);
            
            anim.SetTrigger(triggerName);

        }else if (!anim.GetCurrentAnimatorStateInfo(0).IsName(animationName) && reverse)
        {
            anim.SetFloat("Direction", -2f);
            //anim.ResetTrigger(triggerName);
            anim.SetTrigger(triggerName);
        }
        reverse = !reverse;

    }


}
