using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LivesController : MonoBehaviour
{
    [Header("Health")]
    public int playerHealth = 3;

    [Header("Health Visuals")]
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [Header("Controllers")]
    public GameController gameController;

    //METHOD: Decreases the health of the player by 1 when called to. If the health reaches 0, the game is over. Also displays the hearts accordingly (changing from a full heart to an empty heart when 1HP is lost).
    public void DecreaseLives()
    {
        //Decreases playerHealth by 1
        playerHealth--;

        //Creates a condition to test whether the playerHealth is 0 or not
        if (playerHealth == 0)
        {
            //Final heart is set to empty heart before game over is called.
            hearts[0].sprite = emptyHeart;

            //Calls the GameOver method from the GameController script
            gameController.GameOver();
        }
        //In the condition that playerHealth > 0...
        else
        {
            //Makes the heart at the same index as playerHealth and all hearts greater than that, empty.
            for (int i = playerHealth; i < 3; i++)
            {
                hearts[i].sprite = emptyHeart;

                FindObjectOfType<AudioController>().Play("PlayerHit");
            }
        }
    }
}
