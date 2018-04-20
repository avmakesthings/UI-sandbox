using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TMPDateTime : MonoBehaviour {

    private TextMeshProUGUI timeText;
    public bool year, month, day, time, dayString, monthString, dateString;

    // Use this for initialization
    void Start () {
        timeText = this.GetComponent<TextMeshProUGUI>();
        DateTime dateTime = DateTime.Now;

        if (day)
        {
            timeText.text = dateTime.ToString("dd");
        }
        if (month)
        {
            timeText.text = dateTime.ToString("MM");
        }
        if (year)
        {
            timeText.text = dateTime.ToString("yyyy");
        }
        if (dayString)
        {
            timeText.text = dateTime.ToString("dddd");
        }
        if (monthString)
        {
            timeText.text = dateTime.ToString("MMMM");
        }
        if (dateString)
        {
            timeText.text = (dateTime.ToString("dddd") + " " + dateTime.ToString("dd") + " " + dateTime.ToString("MMMM"));
        }

    }
	
	// Update is called once per frame
	void Update () {
        
        DateTime dateTime = DateTime.Now;

        if (time)
        {

            timeText.text = dateTime.ToString("h:mm" + " " + "tt");
        }



    }
}
