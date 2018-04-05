using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrigger : MonoBehaviour {

    public GameObject[] enable = { };
    public GameObject[] disable = { };
    public GameObject[] instantiate = { };

    private GameObject tempItem;



    private void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            foreach(GameObject obj in enable)
            {
                obj.SetActive(true);
            }

            foreach (GameObject obj in disable)
            {
                obj.SetActive(false);
            }

            //not working yet
            foreach (GameObject obj in instantiate)
            {
                GameObject tempItem = (GameObject)Instantiate(obj);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (GameObject obj in enable)
            {
                obj.SetActive(false);
            }

            foreach (GameObject obj in disable)
            {
                obj.SetActive(true);
            }

            //not working yet
            foreach (GameObject obj in instantiate)
            {

                Destroy(tempItem);
            }
        }
    }
}
