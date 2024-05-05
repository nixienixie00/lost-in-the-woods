using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleMovement : MonoBehaviour
{
    [Header("Mole Movement")]
    Vector2 movement;
    public float speed;
    private float distance;

    [Header("Player Reference")]
    //Player Game Object is how we can find the mole's distance to the player by finding the player's position.
    public GameObject Player;

    [Header("Mole Animation")]
    public Animator MoleAnimator;

    [Header("Mole Health")]
    int MoleHealth = 3;

    [Header("Audio")]
    private bool hasPlayedAudio = false;

    //** START FUNCTION **//
    void Start()
    {
        //Initializes mass of the mole to 500 so that the mole can't be pushed around by the player.
        GetComponent<Rigidbody2D>().mass = 500;
    }

    //** UPDATE FUNCTION **//
    void Update()
    {

        //Assigns the distance between the player (player.transform.position) and the mole (transform.position) to the float "distance".
        distance = Vector3.Distance(Player.transform.position, transform.position);

        if ((distance < 5) && !hasPlayedAudio)
        {
            MoleSeesPlayerAudio();
            hasPlayedAudio = true;
        }
        else if (distance > 5)
        {
            hasPlayedAudio = false;
        }

        //Always test to see if the distance between the player and the mole is less than 5 units.
        if (distance < 5)
        {

            //Informs Animator that the mole is moving and so the Movement Blend Tree should be activated.
            MoleAnimator.SetBool("MoveCheck", true);

            //Changes the x and y values of the movement vector of the mole to the difference between the player's x and y values(Player.transform.position.x and Player.transform.position.y) and the mole's x and y values (transform.position.x and transform.position.y).
            movement.x = Player.transform.position.x - transform.position.x;
            movement.y = Player.transform.position.y - transform.position.y;

            // Normalize the movement vector so it's a unit vector
            movement.Normalize();

            // Move the mole towards the player
            transform.position += (Vector3)movement * speed * Time.deltaTime;

            // Update the Horizontal and Vertical Parameters in the Mole Animator so the appropriate animation is played depending on the direction the mole is moving in.
            MoleAnimator.SetFloat("Horizontal", movement.x);
            MoleAnimator.SetFloat("Vertical", movement.y);
        }

        //If the distance between the player and the mole is greater than 5 units, the mole stops moving.
        else
        {
            //Informs Animator that the mole is not moving and so the Movement Blend Tree should be deactivated. This sets Mole to it's default state which is Mole_Idle.
            MoleAnimator.SetBool("MoveCheck", false);
        }
    }


    //** DYING METHODS **//

    //METHOD: When the mole collides with an arrow, the mole's health decreases by 1. If the mole's health reaches 0, the MoleDeath coroutine is called.
    void OnCollisionEnter2D(Collision2D collision)
    {
        //Tests if the mole has collided with game objects with the tag "Arrow".
        if (collision.gameObject.tag == "Arrow")
        {
            //Decreases the mole's health by 1 if collided with an arrow.
            MoleHealth--;

            //When Mole is hit, tint red
            GetComponent<SpriteRenderer>().color = Color.red;

            //Make the mole white again after 0.1 seconds
            Invoke("ResetColor", 0.1f);

            //Tests if, as a result of this, MoleHealth is 0.
            if (MoleHealth == 0)
            {
                //Calls the MoleDeath coroutine.
                StartCoroutine(MoleDeath());
            }
        }
    }

    //METHOD: Resets the mole's color to white
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //METHOD: The coroutine that is called to in the event of the Mole Dying.
    private IEnumerator MoleDeath()
    {
        //stop mole sounds
        FindObjectOfType<AudioController>().Stop("MoleSound1");
        // Set tag to DeadMonster so the PLAYER isn't harmed by the DeadMonster Cloud.
        gameObject.tag = "DeadMonster";

        //The mole stops moving when it's dead
        movement.x = 0;
        movement.y = 0;

        //Informs Mole Animator that the mole is dead and so the standard Monster_Death animation should be played.
        MoleAnimator.SetBool("Death", true);

        FindObjectOfType<AudioController>().Play("MonsterDeath");

        //Let the Monster Death Animation Play before completely destroying the GameObject (Monster_Death animation lasts 0.6 seconds)
        yield return new WaitForSeconds(0.6f);

        //Destroys itself GameObject (the Mole Game object)
        Destroy(gameObject);
    }

    //METHOD: Plays the "PlayerSeen" sound effect when the mole sees the player.
    public void MoleSeesPlayerAudio()
    {
        List<string> MoleSounds = new List<string> { "MoleSound1", "MoleSound2", "MoleSound3", "MoleSound4", "MoleSound5" };

        int index = UnityEngine.Random.Range(0, MoleSounds.Count);

        FindObjectOfType<AudioController>().Play(MoleSounds[index]);
    }
}
