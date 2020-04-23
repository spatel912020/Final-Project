using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public float speed = 10;                    //speed of left and right movement
    public float jumpPower = 30;                //how much can act against gravity

    private float moveInput;                    //used to get the left and right arrows as -1 or 1
    private bool right = true;                  //if the player is moving right
                                                //used to flip sprite

    private Rigidbody2D playerRb;               //used to change the players velocity
    private CapsuleCollider2D boxCollider;      //used to check if player is touching the ground

    //used to change fall speed, can hold space or tap space
    public float fallPower = 4.5f;
    public float lowJumpPower = 4f;
    
    public Animator animator;                   //get the animator for the player

    public LayerMask tile;                      //used to detect if tile is below player
                                                //if so player can jump

    // Start is called before the first frame update
    void Start()
    {
        //get the players rigidbody and box collider
        playerRb = GetComponent<Rigidbody2D>();
        boxCollider = transform.GetComponent<CapsuleCollider2D>();
    }

    //runs depending on physics frames
    //becasue velocity is being changed here fixed update is used
    void FixedUpdate()
    {
        //left and right keys
        moveInput = Input.GetAxis("Horizontal");
        //if the speed is above 0.01 in animator the walking anmiation plays
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        //set velocity of the player
        playerRb.velocity = new Vector2(moveInput * speed, playerRb.velocity.y);
        //if the player is moveing left face the sprite left
        if(!right && moveInput > 0)
        {
            FlipSprite();
        }
        //if the player is moving right face the sprite right
        else if(right && moveInput < 0)
        {
            FlipSprite();
        }  

    }

    //checks if the player is touching a tile
    private bool IsGrounded()
    {
        //get a raycast below the character and check if the raycast hits the layer mask tile               raycast length, LayerMask)
        RaycastHit2D raycast = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, .1f, tile);
        //returns true if the player is grounded
        return raycast.collider != null;
    }

    //flips the sprite
    void FlipSprite()
    {
        //bool variable swaped and the x scale of the player is swaped from + to - or vise vesa
        right = !right;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    

    // Update is called once per frame
    void Update()
    {
        //jump if the space is pressed and the player is on the ground
        //this gets the player of the ground
        if (Input.GetKeyDown("space") && IsGrounded())
        {
            playerRb.velocity = Vector2.up * jumpPower;
        }
        //changes the gravity with fallPower to give jump arch
        if (playerRb.velocity.y < 0)
        {
            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (fallPower - 1) * Time.deltaTime;
        }
        //changes the gravity with lowJumpPower to simulate small jump
        //short press of space
        //getbutton checks if button is still being pressed
        else if(playerRb.velocity.y > 0 && !Input.GetButton("Jump"))
        {

            playerRb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpPower - 1) * Time.deltaTime;
        }
    }

 
}
