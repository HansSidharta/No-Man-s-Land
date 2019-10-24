using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public Text timerText;
    float timer = 300f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   //set up the timer
        timer -= Time.deltaTime;
        int seconds = (int)(timer % 60);
        int minutes = (int)(timer / 60) % 60;
        //formats to minutes and seconds
        string timerString = string.Format("{0:0} : {1:00}", minutes, seconds);
        //if timer reaches 0, timer becomes 0 so it doesn't go negative
        if(timer < 0)
        {
            timer = 0;
        }
        //output to the screen
        timerText.text = timerString;

    }
}
