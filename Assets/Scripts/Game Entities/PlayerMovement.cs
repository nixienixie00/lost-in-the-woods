using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Components")]
    public Rigidbody2D PlayerRB;

    [Header("Player Movement")]
    Vector2 movement;
    public float moveSpeed = 5f;

    [Header("Player Animation")]
    public Animator PlayerAnimator;

    [Header("Player Shooting")]
    public GameObject arrowPrefab;
    public float arrowSpeed = 40f;

    [Header("Audio")]
    private bool footstepsPlaying = false;

    [Header("Controllers")]
    public LivesController livesController;

    //** UPDATE FUNCTIONS **//
    void Update()
    {
        if ((movement.x != 0 || movement.y != 0) && !footstepsPlaying)
        {
            footstepsPlaying = true;
            FindObjectOfType<AudioController>().Play("PlayerFootsteps");
        }
        else if (movement.x == 0 && movement.y == 0 && footstepsPlaying)
        {
            footstepsPlaying = false;
            FindObjectOfType<AudioController>().Stop("PlayerFootsteps");
        }

        //Movement input - when the WASD keys are pressed, the x and y values of the movement vector are set to the input values (Default keys for GetAxisRaw are A and D for x and W and S for y).

        if (!((Input.GetKey(KeyCode.UpArrow)) || (Input.GetKey(KeyCode.DownArrow)) || (Input.GetKey(KeyCode.RightArrow)) || (Input.GetKey(KeyCode.LeftArrow))))
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }

        // Set the Animator Parameters to the the movement values, Horizontal parameters are dependant on movement.x, Vertical parameters are dependant on movement.y and Speed parameters are dependant on the magnitude of the movement vector.
        PlayerAnimator.SetFloat("Horizontal", movement.x);
        PlayerAnimator.SetFloat("Vertical", movement.y);
        PlayerAnimator.SetFloat("Speed", movement.sqrMagnitude);

        // Always tests whether the space key is pressed. If pressed, start the ShootTime coroutine.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Calls the ShootTime coroutine
            StartCoroutine(ShootTime());
        }
    }

    // METHOD: FixedUpdate Function used for player movement as it is physics based.
    void FixedUpdate()
    {
        // Move the player by adding the movement vector multiplied by the moveSpeed and Time.fixedDeltaTime to the player's position.
        PlayerRB.MovePosition(PlayerRB.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    //** PLAYER COLLISION METHODS **//

    // METHOD: OnCollisionEnter2D Function used to detect whether the player has collided with a game object tagged "Monster" and decreases health of the player by 1 with each collision.
    void OnCollisionEnter2D(Collision2D collision)
    {
        // If the player collides with a game object tagged "Monster", calls to the DecreaseLives method in the LivesController script with each collision.
        if (collision.gameObject.tag == "Monster")
        {
            //When Player is hit, tint red
            GetComponent<SpriteRenderer>().color = Color.red;

            Invoke("ResetColor", 0.1f);

            livesController.DecreaseLives();
        }
    }
    void ResetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    //** SHOOTING FUNCTION **//

    // METHOD: ShootTime Coroutine used to instantiate an arrow prefab and set the velocity of the arrow based on the direction the player is facing.
    IEnumerator ShootTime()
    {

        // Informs the Player Animator that the player is shooting by setting the Shoot Boolean Parameter to true.
        PlayerAnimator.SetBool("Shoot", true);

        //Initialise the arrow and arrowRB variables
        GameObject arrow;
        Rigidbody2D arrowRB;

        //PLAYER FACING RIGHT
        if (movement.x > 0)
        {
            // Creates an arrow using arrowPrefab to the right of the player game object.
            arrow = Instantiate(arrowPrefab, new Vector3(transform.position.x + 1, transform.position.y, 0), Quaternion.identity);

            //Initialise the arrowRB rigidbody2D component
            arrowRB = arrow.GetComponent<Rigidbody2D>();

            //Rotate the arrow to face right when the player is facing right.
            arrow.transform.Rotate(0, 0, 270);

            //Set the velocity of the arrow to move to the right (x = arrowSpeed, y = 0)
            arrowRB.velocity = new Vector2(arrowSpeed, 0);

            FindObjectOfType<AudioController>().Play("Arrow");
        }

        //PLAYER FACING LEFT
        else if (movement.x < 0)
        {
            // Creates an arrow using arrowPrefab to the left of the player game object.
            arrow = Instantiate(arrowPrefab, new Vector3(transform.position.x - 1, transform.position.y, 0), Quaternion.identity);

            //Initialise the arrowRB rigidbody2D component
            arrowRB = arrow.GetComponent<Rigidbody2D>();

            //Rotate the arrow to face left when the player is facing left.
            arrow.transform.Rotate(0, 0, 90);

            //Set the velocity of the arrow to move to the left (x = -arrowSpeed, y = 0)
            arrowRB.velocity = new Vector2(-arrowSpeed, 0);

            FindObjectOfType<AudioController>().Play("Arrow");
        }

        //PLAYER FACING UP
        else if (movement.y > 0)
        {
            // Creates an arrow using arrowPrefab above the player game object.
            arrow = Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y + 1, 0), Quaternion.identity);

            //Initialise the arrowRB rigidbody2D component
            arrowRB = arrow.GetComponent<Rigidbody2D>();

            //Rotate the arrow to point upwards when the player is facing up.
            arrow.transform.Rotate(0, 0, 0);

            //Set the velocity of the arrow to move upwards (x = 0, y = arrowSpeed)
            arrowRB.velocity = new Vector2(0, arrowSpeed);

            FindObjectOfType<AudioController>().Play("Arrow");
        }

        //PLAYER FACING DOWN
        else if (movement.y < 0)
        {
            // Creates an arrow using arrowPrefab below the player game object.
            arrow = Instantiate(arrowPrefab, new Vector3(transform.position.x, transform.position.y - 1, 0), Quaternion.identity);

            //Initialise the arrowRB rigidbody2D component
            arrowRB = arrow.GetComponent<Rigidbody2D>();

            //Rotate the arrow to point downwards when the player is facing down.
            arrow.transform.Rotate(0, 0, 180);

            //Set the velocity of the arrow to move downwards (x = 0, y = -arrowSpeed)
            arrowRB.velocity = new Vector2(0, -arrowSpeed);

            FindObjectOfType<AudioController>().Play("Arrow");
        }

        // Wait for 0.3 seconds so the player shoot animation can play (Player_Shooting animations all last 0.3 seconds)
        yield return new WaitForSeconds(0.3f);

        // Inform the Player Animator that the player is no longer shooting by setting the Shoot Boolean Parameter to false.
        PlayerAnimator.SetBool("Shoot", false);
    }

}
