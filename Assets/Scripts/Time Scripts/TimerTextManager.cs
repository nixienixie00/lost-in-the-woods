using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TimerTextManager : MonoBehaviour
{
    [Header("Level Number")]
    public int level;

    [Header("Manipulated Text")]
    public Text timerText;

    [Header("Current Time")]
    public static float currentTime;

    //** START FUNCTION **//
    void Start()
    {
        //sets the current time to 0 at the start of each level.
        currentTime = 0;
    }

    
    //** UPDATE FUNCTION **//
    void Update()
    {
        //Increments the current time by the time since the last frame.
        currentTime += Time.deltaTime;

        //Sets the text of the timerText to the current time in seconds.
        timerText.text = currentTime.ToString("#");
    }

    
    //** TIME RECORDING FUNCTIONS **//

    // METHOD: Returns the value of current time when called as an integer.
    public int GetCurrentTime()
    {
        //Returns the current time as an integer, +0.5 to round up all recorded times.
        return ((int)(currentTime + 0.5));
    }
}