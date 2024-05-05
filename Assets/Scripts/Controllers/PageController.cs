using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PageController : MonoBehaviour
{
    [Header ("Buttons")]
    public Button NextButton;
    public Button BackButton;

    [Header ("Level Title")]
    public Text LevelTitle;

    [Header ("Controllers")]
    public GameController gameController;
    public LevelController levelController;


    [Header ("levels")]
    public int level = 1;

    void Start()
    {
        //Change the level title depending on the level Leaderboard being displayed which is initially level 1
        LevelTitle.text = "Level " + level;
        //Set the refreshed leaderboard to the current level
        levelController.setRefreshedLeaderboard(level);
    }

    public void BackPage()
    {
        //If the level is greater than 1, go back a level
        if (level > 1)
        {
            //Play the small button audio
            gameController.getSmallButtonAudio();

            //Decrement the level and set the refreshed leaderboard to the new level
            level--;

            //Change the level title depending on the level Leaderboard being displayed
            LevelTitle.text = "Level " + level;

            //  Set the refreshed leaderboard to the current level
            levelController.setRefreshedLeaderboard(level);
        }
        //If the level is 1, play the button denied audio
        else
        {
            //Play the button denied audio
            gameController.getButtonDeniedAudio();
        }
    }
    public void NextPage()
    {
        //If the level is less than 3, go to the next level
        if (level < 3)
        {
            //Play the small button audio
            gameController.getSmallButtonAudio();

            //Increment the level and set the refreshed leaderboard to the new level
            level++;

            //Change the level title depending on the level Leaderboard being displayed
            LevelTitle.text = "Level " + level;

            //Set the refreshed leaderboard to the current level
            levelController.setRefreshedLeaderboard(level);
        }
        //If the level is 3, play the button denied audio
        else
        {
            //Play the button denied audio
            gameController.getButtonDeniedAudio();
        }
    }

}