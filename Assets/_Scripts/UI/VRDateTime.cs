using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;


//a class that gets system time and converts it to formatted strings
public class VRDateTime : MonoBehaviour
{

    private Text dateTimeText;
    public bool year, month, day, time, dayString, monthString, dateString;


    void Start()
    {
        dateTimeText = GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {

        DateTime dateTime = DateTime.Now;

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
        if (dateString)
        {
            dateTimeText.text = (dateTime.ToString("dddd") + " " + dateTime.ToString("dd") +  " " + dateTime.ToString("MMMM"));
        }

    }





}
