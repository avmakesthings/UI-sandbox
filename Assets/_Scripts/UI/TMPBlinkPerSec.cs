using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TMPBlinkPerSec : MonoBehaviour {

    private TextMeshProUGUI textToBlink;


    void Start()
    {
        textToBlink = this.GetComponent<TextMeshProUGUI>();
        StartCoroutine(Blink(textToBlink));
    }


    //a coroutine that blinks every second
    private IEnumerator Blink(TextMeshProUGUI text)
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
