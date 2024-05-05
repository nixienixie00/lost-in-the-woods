using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Globalization;


public class TimeDatabase : MonoBehaviour
{
    [Header("Constant")]
    // Declare a constant LEADERBOARD_KEY to make it easier to detect if any errors relevant to leaderboard is made.
    private const string LEADERBOARD_KEY = "Leaderboard";
    //creating constant for max leaderboard entries
    const int MAX_ENTRIES = 50;

    [Header("Initialization")]
    //created an initialLeaderboard string to store 5 ranks as there will be 5 slots for "N/A" to display in under Times and Date.
    public string initialLeaderboard = "X,01/01/9999,999|X,01/01/9999,999|X,01/01/9999,999|X,01/01/9999,999|X,01/01/9999,999";
    
    //METHOD: Calls to PlayerPrefs and whatever integer is entered into the parameter will be the level Leaderboard csv that will be initialized.
    public void setInitializedLeaderboard(int level)
    {
        //Writes to "LEADERBOARD_KEY + level"/Leaderboard1 or Leaderboard2 or Leaderboard3 by setting the whole string to initialLeaderboard.
        PlayerPrefs.SetString(LEADERBOARD_KEY + level, initialLeaderboard);
    }

    //METHOD: When called to, adds a new time to LeaderboardX where X is the integer parameter "level"
    public void AddNewTime(int level, DateTime date, float newTime)
    {
        //sets the string variable timeToLog to the standard form for that record, using the parameters
        string timeToLog = level + "," + date.ToString() + "," + newTime.ToString();

        //Here I break up items in the csv to sort it via the Times item in index [2] of each sublist in the csv (sublists separated by the "|")

        //Creates a 2D array where the main array holds the records and the records inside it are separate instances of recorded times with the date they were acheived and the level they were acheived in.
        string[][] LevelLeaderboard = (PlayerPrefs.GetString("Leaderboard" + level) + "|" + timeToLog)
                                                                                            .Split('|')
                                                                                            .Select(s => s.Split(','))
                                                                                            .ToArray();


        // Sort the array based on the third item (time) in each sub-array
        LevelLeaderboard = LevelLeaderboard.OrderBy(arr => float.Parse(arr[2], CultureInfo.InvariantCulture.NumberFormat)).ToArray();

        // If the array is longer than MAX_ENTRIES, remove the last element
        if (LevelLeaderboard.Length > MAX_ENTRIES)
        {
            Array.Resize(ref LevelLeaderboard, MAX_ENTRIES);
        }

        // set playprefs leaderboard
        PlayerPrefs.SetString(LEADERBOARD_KEY + level, string.Join("|", LevelLeaderboard.Select(arr => string.Join(",", arr))));
    }

    //METHOD: Get's the leaderboard for a specified level
    public string GetLeaderboard(int level)
    {
        Debug.Log(level + LEADERBOARD_KEY);
        return PlayerPrefs.GetString(LEADERBOARD_KEY + level);
    }
}
