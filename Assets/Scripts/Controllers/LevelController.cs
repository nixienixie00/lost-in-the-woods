using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Needed for UI elements
using UnityEngine.UI;
//Imported to sort the CSV using LINQ
using System.Linq;
//Imported to get the current date and time
using System.Globalization;

public class LevelController : MonoBehaviour
{
    [Header("UI Elements")]
    public Text[] Rank = new Text[5];

    [Header("Constants")]
    const int MAX_RANKS = 5;
    public const int MAX_LEVELS = 3;

    [Header("Controllers")]
    public TimeDatabase timeDatabase;

    //METHOD: Calls to Initialize Leaderboard in TimeDatabase.cs and Initializes the Leaderboard for all Levels
    public void setInitializedLeaderboard()
    {
        //For loop to iterate through all the levels. Number of levels stored in constant MAX_LEVELS which provides the upperbound for the loop.
        for (int i = 1; i <= MAX_LEVELS; i++)
        {
            //Initializes the Leaderboard for every Level i.
            timeDatabase.setInitializedLeaderboard(i);
        }
    }

    //METHOD: Refreshes to view the recorded times of a specified level and orders them to match the POS, only displays the top 5 times per level.
    public void setRefreshedLeaderboard(int level)
    {
        //Puts the Leaderboard of the specified level (see parameter)
        string leaderboard = timeDatabase.GetLeaderboard(level);

        //Initializes the string spaces which will later be multiplied by some integer to provide the gap between the date recorded in the time database and it's corresponding acheived time.
        string spaces = "";

        //Creates a conditional to check if the leaderboard is null. If it is not null, display as
        if (leaderboard != null)
        {
            //Creates a 2D array of strings called LevelLeaderboard by splitting the leaderboard via the "|" then again via the ","
            string[][] LevelLeaderboard = leaderboard.Split('|')
                                                    .Select(s => s.Split(','))
                                                    .ToArray();

            //For loop to display each item of index [1] and [2] of the array
            for (int i = 0; i < MAX_RANKS; i++)
            {
                string[] temp = LevelLeaderboard[i];
                //display temp[1] + spaces + temp[2] on text rank if it's not empty
                if (Rank[i] != null)
                {
                    //If the date is 01/01/9999, display N/A instead of the date and time
                    if (temp[1] == "01/01/9999")
                    {
                        //sets date to N/A for record i.
                        temp[1] = "N/A";

                        //sets time to N/A for record i.
                        temp[2] = "N/A";

                        //sets spaces to 130 spaces to provide a gap between the date and time.
                        spaces = "".PadRight(130, ' ');
                    }
                    else
                    {
                        //If the date is not 01/01/9999, a recorded time is stored there so display the date and time
                        temp[1] = temp[1].Substring(0, 10);

                        //sets spaces to 102 spaces to provide a gap between the date and time. As the gap is a little different from the gap between N/A for time and date.
                        spaces = "".PadRight(102, ' ');
                    }

                    //Display the rank of the record i in the leaderboard.
                    Rank[i].text = temp[1] + spaces + temp[2];
                }
            }
        }
        else
        {
            //Debug in case there are no records in the leaderboard at all (not even initializedLeaderboard)
            Debug.LogError("LEADERBOARD NOT RESPONDING: NULL");
        }
    }

    //METHOD: Calls to AddNewTime in time database to add a new time to the leaderboard of a specified level. This method is called to by gameController.levelComplete() once the Gem detect a collision with the Player.
    public void LogTime(int level, float newTime)
    {
        timeDatabase.AddNewTime(level, System.DateTime.Now, newTime);
    }
}
