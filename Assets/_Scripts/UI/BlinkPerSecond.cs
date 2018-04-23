using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkPerSecond : MonoBehaviour {

    private Text textToBlink;


    void Start()
    {
        textToBlink = this.GetComponent<Text>();
        StartCoroutine(Blink(textToBlink));
    }


    //a coroutine that blinks every second
    private IEnumerator Blink(Text text)
    {
        while (true)
        {
            text.enabled = true;
            yield return new WaitForSeconds(1f);
            text.enabled = false;
            yield return new WaitForSeconds(1f);
        }

    }
}
