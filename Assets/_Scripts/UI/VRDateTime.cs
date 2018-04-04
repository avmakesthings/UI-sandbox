using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VRDateTime : MonoBehaviour
{

    private Text dateTimeText;
    public bool year, month, day, time, dayString, monthString;


    void Start()
    {
        dateTimeText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

        System.DateTime dateTime = System.DateTime.Now;

        if (time)
        {

            dateTimeText.text = dateTime.ToString("hh:mm:tt");
        }
        if (day)
        {
            dateTimeText.text = dateTime.ToString("dd");
        }
        if (month)
        {
            dateTimeText.text = dateTime.ToString("MM");
        }
        if (year)
        {
            dateTimeText.text = dateTime.ToString("yyyy");
        }
        if (dayString)
        {
            dateTimeText.text = dateTime.ToString("dddd");
        }
        if (monthString)
        {
            dateTimeText.text = dateTime.ToString("MMMM");
        }


        //for every second
        //StartCoroutine("Blink");

    }



    //a coroutine that 
    //IEnumerator Blink (System.DateTime time)
    //{
    // print(clockText);
    //}

}
