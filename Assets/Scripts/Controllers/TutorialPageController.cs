using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Needed for UI elements
using UnityEngine.UI;

public class TutorialPageController : MonoBehaviour
{
    [Header("Tutorial Scene Pages")]
    public GameObject[] pages;
    public int currentPage = 0;

    [Header("Tutorial Scene Buttons")]
    public Button nextButton;
    public Button backButton;

    [Header("Game Controller")]
    public GameController gameController;


    //** BUTTON METHODS **//

    //METHOD: NextPage is called to when the Next Button is clicked. Increments the currentPage to activate the Next Page.
    public void NextPage()
    {
        //Condition to check that the currentPage is less than the number of pages there are, otherwise there are no more pages to show.
        if (currentPage < pages.Length - 1)
        {
            //Deactivate the current page and activate the next page.
            pages[currentPage].SetActive(false);

            //Increment the currentPage variable.
            currentPage++;

            //Activate the next page using the newly incremented currentPage variable and setting that page in the GameObject array to active.
            pages[currentPage].SetActive(true);

            //Call to gameController.getSmallButtonAudio to Play the small button audio.
            gameController.getSmallButtonAudio();
        }
        else
        {
            //Call to gameController.getButtonDeniedAudio to Play the button denied audio to indicate that there are no more pages to show.
            gameController.getButtonDeniedAudio();
        }
    }

    //METHOD: BackPage is called to when the Back Button is clicked. Decrements the currentPage to activate the Previous Page.
    public void BackPage()
    {
        //Condition to check that the currentPage is greater than 0, otherwise there are no more pages to show.
        if (currentPage > 0)
        {
            //Deactivate the current page and activate the previous page.
            pages[currentPage].SetActive(false);

            //Decrement the currentPage variable.
            currentPage--;

            //Activate the previous page using the newly decremented currentPage variable and setting that page in the GameObject array to active.
            pages[currentPage].SetActive(true);

            //Call to gameController.getSmallButtonAudio to Play the small button audio.
            gameController.getSmallButtonAudio();
        }
        else
        {
            //Call to gameController.getButtonDeniedAudio to Play the button denied audio to indicate that there are no more pages to show.
            gameController.getButtonDeniedAudio();
        }
    }
}
