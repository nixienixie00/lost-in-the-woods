using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemController : MonoBehaviour
{
    [Header ("Game Objects")]
    public GameObject Player;
    public GameObject Gem;

    [Header ("Controllers")]
    public GameController gameController;

    //METHOD: Calls to the Level Complete Function in the Game Controller when the Player collides with it.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Tests to see if the player has collided with the game object which is the gem in this case.
        if (collision.gameObject == Player)
        {
            //calls to the Level Complete Method in the Game Controller
            gameController.LevelComplete();
        }
    }
}
