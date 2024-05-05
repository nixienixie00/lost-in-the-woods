using System.Collections;
using UnityEngine;

public class TreantMovement : MonoBehaviour
{
    [Header("Treant Movement")]
    Vector2 movement;
    public float speed = 5f;

    [Header("Player Reference")]
    //Player Game Object is how we can find the mole's distance to the player by finding the player's position.
    private float distance;
    public GameObject Player;

    [Header("Treant Animation")]
    public Animator TreantAnimator;
    //X_Movement is a boolean that determines whether the treant moves on the x or y axis (determines wheteher the treant is a TreantY or TreantX)
    public bool X_Movement;
    
    [Header("Treant Health")]
    public int TreantHealth = 3;

    [Header("Treant Components")]
    public Rigidbody2D TreantRB;

    [Header("Audio")]
    private bool hasPlayedAudio = false;


    //** START FUNCTION **//
    void Start()
    {

        //If X_Movement is true, the treant only moves horizontally.
        if (X_Movement)
        {
            //Informs the Animator that the treant is moving horizontally by setting the animator's boolean parameter "X_Movement" to True, causing the animator to use the Treant_Walk_Side state.
            TreantAnimator.SetBool("X_Movement", true);

            //Sets the x value of the movement vector to 0.1f, TreantX will always try to move right first.
            movement.x = 0.1f;

            //Informs the Animator of which direction the Treant is moving in to determine whether it should make the Treant face right or left (right in this case).
            TreantAnimator.SetFloat("Direction", movement.x);
        }

        //If X_Movement is false, the treant only moves vertically.
        else
        {
            //Informs the Animator that the treant is moving vertically by setting the animator's boolean parameter "X_Movement" to False, causing the animator to use the Treant_Walk_Up and Treant_Walk_Down states.
            TreantAnimator.SetBool("X_Movement", false);

            //Sets the y value of the movement vector to 0.1f, TreantY will always try to move up first.
            movement.y = 0.1f;

            //Informs the Animator of which direction the Treant is moving in to determine whether it should make the Treant face up or down (up in this case).
            TreantAnimator.SetFloat("Direction", movement.y);
        }
    }

    void Update()
    {
        distance = Vector3.Distance(Player.transform.position, transform.position);

        if ((distance < 5) && !hasPlayedAudio)
        {
            if (X_Movement == false)
            {
                FindObjectOfType<AudioController>().Play("TreantSound1");
            }
            else
            {
                FindObjectOfType<AudioController>().Play("TreantSound2");
            }
            hasPlayedAudio = true;
        }
        else if (distance > 5)
        {
            hasPlayedAudio = false;
        }
    }

    //** FIXED UPDATE FUNCTION **//
    // METHOD: FixedUpdate Function used for treant movement as it is physics based.
    void FixedUpdate()
    {
        // Move the treant by adding the movement vector multiplied by the speed and Time.fixedDeltaTime to the treant's position.
        TreantRB.MovePosition(TreantRB.position + movement * speed * Time.fixedDeltaTime);
    }
    
    //** COLLISION METHOD **//
    // METHOD: OnCollisionEnter2D Function used to detect whether the treant has collided with a game object tagged "Wall" changing direction of the treant with each collision in the axis it's walking on (e.g. positive x to negative x or positive y to negative y).
    public void OnCollisionEnter2D(Collision2D collision)
    {
        //* WALL COLLISION *//
        //Tests to see if the treant has collided with a game object tagged "Wall".
        if (collision.gameObject.tag == "Wall")
        {
            //If movement.y is not equal to 0, the treant is a TreantY and so the treant will change direction on the y axis.
            if (movement.y != 0.0f)
            {   
                //Inverts the y value of the movement vector so the treant moves in the opposite direction.
                movement.y = -movement.y;

                //Informs the Animator of which direction the Treant is now moving in to determine whether it should make the Treant face up or down.
                TreantAnimator.SetFloat("Direction", movement.y);
            }
            
            //If movement.x is equal to 0, the treant is a TreantX and so the treant will change direction on the x axis.
            else
            {
                //Inverts the x value of the movement vector so the treant moves in the opposite direction.
                movement.x = -movement.x;

                //Informs the Animator of which direction the Treant is now moving in to determine whether it should make the Treant face right or left.
                TreantAnimator.SetFloat("Direction", movement.x);

                //Inverts the x scale of the treant so it faces the direction it is moving in (to flip the treant sprite animation on the x axis because there's only one side animation not left and right).
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
            }
        }
        
        //* ARROW COLLISION *//
        //Tests if the treant has collided with game objects with the tag "Arrow".
        if (collision.gameObject.tag == "Arrow")
        {
            //Decreases the treant's health by 1 if collided with an arrow.
            TreantHealth--;

            //When Treant is hit, tint red
            GetComponent<SpriteRenderer>().color = Color.red;

            Invoke("ResetColor", 0.1f);

            //Tests if, as a result of this, TreantHealth is 0.
            if (TreantHealth == 0)
            {
                //Calls the TreantDeath coroutine.
                StartCoroutine(TreantDeath());
            }
        }
    }


    //METHOD: Resets the Treant's color to white
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //** DYING METHOD **//
    // METHOD: TreantDeath Coroutine used to inform the animator that the treant is dead and then destroy the treant game object.
    private IEnumerator TreantDeath()
    {
        // Set tag to DeadMonster so the PLAYER isn't harmed by the DeadMonster Cloud.
        gameObject.tag = "DeadMonster";

        //Stops the Treant from moving when it's dead
        movement.x = 0;
        movement.y = 0;

        //Informs the Animator that the treant is dead by setting the Death boolean parameter to true, causing the animator to use the Monster_Death state.
        TreantAnimator.SetBool("Death", true);

        FindObjectOfType<AudioController>().Play("MonsterDeath");

        //Waits for 0.6 seconds before destroying the treant game object so there's time for the Monster_Death animation to play.
        yield return new WaitForSeconds(0.6f);

        //Destroys the treant game object once death animation has played.
        Destroy(gameObject);
    }
}
