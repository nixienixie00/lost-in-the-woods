using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    //METHOD: Destroys the Arrow when it collides with Game Objects Tagged: "Monster", "Wall" or "DeadMonster".
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Condition created to check if the Arrow has collided with a Monster, Wall or DeadMonster.
        if (collision.gameObject.tag == "Monster" || collision.gameObject.tag == "Wall" || collision.gameObject.tag == "DeadMonster")
        {
            //Destroys the game object the script is attached to which is the Arrow.
            Destroy(gameObject);
        }
    }
}
