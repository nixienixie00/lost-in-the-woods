using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [Header("Game Panels")]
    public GameObject PauseMenu;
    public GameObject LevelCompleteScreen;
    public GameObject GameOverScreen;

    [Header("Game Navigation Variables")]
    public int level = 1;

    [Header("Controllers")]
    public TimerTextManager timerTextManager;
    public LevelController levelController;


    //** GAME RUN FUNCTIONS **//

    //METHOD: Shows the Pause Menu when called to (called to by the Pause Button)
    public void ShowPauseMenu()
    {
        // Sets the Pause Menu Game Object to Active so it appears on the screen.
        PauseMenu.SetActive(true);

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();

        // Calls to the PauseGame function to stop the game from running while the player is on the Pause Menu.
        PauseGame();
    }

    //METHOD: Hides the Pause Menu when called to (called to by the Resume Button)
    public void HidePauseMenu()
    {
        //Sets the Pause Menu Game Object to Inactive so it disappears from the screen.
        PauseMenu.SetActive(false);

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();

        // Calls to the Resume function to start the game running again after the player has left the Pause Menu.
        Resume();
    }

    //METHOD: Pauses all Game Element when called (called to by the ShowPauseMenu function)
    public void PauseGame()
    {
        //Sets TimeScale to 0 to stop all game elements from running.
        Time.timeScale = 0f;
    }

    //METHOD: Resumes all Game Elements when called (called to by the HidePauseMenu function)
    public void Resume()
    {
        //Sets TimeScale to 1 so all game elements can run again.
        Time.timeScale = 1f;
    }

    //METHOD: Summons the Level Complete Screen when called to (called to by the EndLevel function)
    public void LevelComplete()
    {
        //Calls to setInitializedBackgroundAudio to stop any music from playing once the level is complete. (Level Background Music)
        setInitializedBackgroundAudio();

        //Plays the "Player Win" audio using the "Play" function in the Audio Controller.
        FindObjectOfType<AudioController>().Play("PlayerWin");

        //Calls to the PauseGame function to stop the game from running while the player is on the Level Complete Screen. Also stops timer from running so time can be recorded.
        PauseGame();

        //Sets the Level Complete Screen Game Object to Active so it appears on the screen.
        LevelCompleteScreen.SetActive(true);

        //Returns the Build Index of the current scene/active scene
        level = SceneManager.GetActiveScene().buildIndex;

        //Calls to the LogTime function in the LevelController script to log the time the player took to complete the level.
        levelController.LogTime(level, timerTextManager.GetCurrentTime());
    }

    //MEHTOD: Ends the current level and loads the next level when called to (called to by the Next Level Button)
    public void GameOver()
    {
        //Calls to setInitializedBackgroundAudio to stop any music from playing once the level is complete. (Level Background Music)
        setInitializedBackgroundAudio();

        //Plays the "Player Defeat" audio using the "Play" function in the Audio Controller.
        FindObjectOfType<AudioController>().Play("PlayerDefeat");

        //Calls to the Pause Game function to stop the game from running while the player is on the Game Over Screen.
        PauseGame();

        //Set the "GameOverScreen" game object to active so it can be seen by the player.
        GameOverScreen.SetActive(true);
    }

    //METHOD: Restarts the current level when called to (called to by the Restart Button)
    public void RestartLevel()
    {
        //Calls to the Resume function to start the game running again once the player decides to restart the level.
        Resume();

        //Sets the Level Complete Screen Game Object to Inactive so it disappears from the screen so the player can play again.
        GameOverScreen.SetActive(false);

        //Returns the Build Index of the current scene/active scene
        level = SceneManager.GetActiveScene().buildIndex;

        //Reloads the current level so the player can try again.
        SceneManager.LoadScene(level);

        //creates a string for the scene name to refer to the dedicated background music for that particular level. Each level theme is labelled as "LevelXTheme" where X is an integer between 1-3 (total number of levels)
        string sceneName = "Level" + level.ToString() + "Theme";

        //Plays the sceneName audio using the "Play" function in the Audio Controller.
        FindObjectOfType<AudioController>().Play(sceneName);

    }


    //** NAVIGATING FUNCTIONS **//

    //METHOD: Loads the Main Menu when called to (called to by the Home Button on the Level and Tutorial Page and the Menu Button on the Pause Menu)
    public void LoadHome()
    {
        //Calls to setInitializedBackgroundAudio to stop any music from playing once the level is complete. (Level Background Music)
        setInitializedBackgroundAudio();

        //Loads the Scene Main Menu using the SceneManager.
        SceneManager.LoadScene("MainMenu");

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();

        //Plays the "MainTheme" audio using the "Play" function in the Audio Controller.
        FindObjectOfType<AudioController>().Play("MainTheme");
    }

    //METHOD: Loads the Times Page when called to (called to by the Times Button on the Main Menu)
    public void LoadTimePage()
    {
        //Loads the Scene "Time Page" using the SceneManager.
        SceneManager.LoadScene("Times Scene");

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();
    }

    //METHOD: Loads the Tutorial Page when called to (called to by the Tutorial Button on the Main Menu)
    public void LoadTutorial()
    {
        //Loads the Scene "Tutorial Scene" using the SceneManager.
        SceneManager.LoadScene("Tutorial Scene");

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();
    }

    //METHOD: Loads the Next Level to the current Level (called to by the Next Button on the Level Complete Screen)
    public void LoadNextLevel()
    {
        //Calls to the Resume function to start the game running again once the player decides to move to the next level.
        Resume();

        //Uses SceneManager to load the next level in the build index.
        int nextLevel = level + 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextLevel);

        setInitializedBackgroundAudio();

        //creates a string for the scene name to refer to the dedicated background music for that particular level. Each level theme is labelled as "LevelXTheme" where X is an integer between 1-3 (total number of levels)
        string nextLevelTheme = "Level" + nextLevel.ToString() + "Theme";

        //Plays the nextLevelTheme audio using the "Play" function in the Audio Controller.
        FindObjectOfType<AudioController>().Play(nextLevelTheme);

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();
    }

    //METHOD: Opens the Level with the corresponding levelId when called to (called to by the Level Buttons on the Level Page)
    public void OpenLevel(int levelId)
    {
        //Calls to the Resume function in case the game is paused when the player decides to open a level.
        Resume();

        //Finds "ButtonSelect" Audio using AudioController and plays it
        FindObjectOfType<AudioController>().Play("ButtonSelect");

        //Depending on the levelId parameter, the levelName will be set to the corresponding level (e.g. levelId = 1, levelName = "Level 1")
        string levelName = "Level " + levelId;

        //Loads the Scene called levelName using the SceneManager.
        SceneManager.LoadScene(levelName);

        //Calls to Button Select Audio to play the "ButtonSelect" sound effect
        getButtonSelectAudio();

        //Stops the "MainTheme" audio using the "Stop" function in the Audio Controller.
        FindObjectOfType<AudioController>().Stop("MainTheme");

        //Creates a conditional statement that looks at which level is being loaded via "levelId" and plays the appropriate theme for that level.
        if (levelId == 1)
        {
            FindObjectOfType<AudioController>().Play("Level1Theme");
        }
        else if (levelId == 2)
        {
            FindObjectOfType<AudioController>().Play("Level2Theme");
        }
        else if (levelId == 3)
        {
            FindObjectOfType<AudioController>().Play("Level3Theme");
        }
    }


    //** AUDIO MANAGING FUNCTIONS **//

    //METHOD: Fetches and plays Button Select Audio from the Audio Controller for the Play Button
    public void getButtonSelectAudio()
    {
        //Finds "ButtonSelect" Audio using AudioController and plays it
        FindObjectOfType<AudioController>().Play("ButtonSelect");
    }
    
    //METHOD: Fetches and plays Small Button Select Audio from the Audio Controller for the Play Button
    public void getSmallButtonAudio()
    {
        //Finds "ButtonSelect" Audio using AudioController and plays it
        FindObjectOfType<AudioController>().Play("SmallButtonSelect");
    }
    
    //METHOD: Fetches and plays Button Denied Audio from the Audio Controller for the Play Button
    public void getButtonDeniedAudio()
    {
        //Finds "ButtonDenied" Audio using AudioController and plays it
        FindObjectOfType<AudioController>().Play("ButtonDenied");
    }
    
    //METHOD: Stops any background music from playing (apart from "MainMenuTheme" as it wasn't requried at any point in the game).
    public void setInitializedBackgroundAudio()
    {
        //Finds "Level1Theme", "Level2Theme", "Level3Theme" Audios using AudioController and stops all of them from playing (at any one time, it's likely only one would be playing)
        FindObjectOfType<AudioController>().Stop("Level1Theme");
        FindObjectOfType<AudioController>().Stop("Level2Theme");
        FindObjectOfType<AudioController>().Stop("Level3Theme");
    }


    //** DATA RELATED FUNCTIONS **//

    //METHOD: The method called to via the clear button to reset the Time Database
    public void setClearLeaderboard()
    {

        //Calls to InitializeLeaderboard in levelController to reset all of the playerLeaderboard data
        levelController.setInitializedLeaderboard();

        //Reloads Times Scene to refresh the whole scene and see the newly re-initialized Time Database.
        SceneManager.LoadScene("Times Scene");
    }
}