using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            print("the player has entered");
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("the player has left");
        }
    }
}
