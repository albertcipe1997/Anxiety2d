using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Spider : EnemyController
{
    [Header("Spider")]
    /// <summary>
    /// The force of the jump when the enemy jumps
    /// </summary>
    public float jumpForce;
    /// <summary>
    /// The enemy chases this target
    /// </summary>
    private Transform target = null;
    /// <summary>
    /// If is true, the spider can jump else cannot jump
    /// </summary>
    public bool canJump = true;
    /// <summary>
    /// If is true, the spider just jumped
    /// </summary>
    private bool groundTouch = true;
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();

        if(PlayerController.playerInstance)
            target = PlayerController.playerInstance.transform;//GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();

        //eyes.transform.localScale = new Vector2(side, 0);
        
      

        if (!target && PlayerController.playerInstance)
            target = PlayerController.playerInstance.transform;

        //
       

        //New AI checker for the spider
        Vector2 jump_distance = lineCastPos + Vector2.right * 5 * side;
        Vector2 jump_distance2 = lineCastPos2 + Vector2.right * 10 * side;
        bool jump_willBeGrounded = Physics2D.Linecast(jump_distance, jump_distance2 + coll.bottomOffset + Vector2.down * 20, groundLayer);
        UnityEngine.Debug.DrawLine(jump_distance, jump_distance2 + coll.bottomOffset + Vector2.down * 20, Color.green);
        UnityEngine.Debug.Log(lineCastPos.x + " " + lineCastPos.y);
        Vector2 v1 =  new Vector2(lineCastPos.x, lineCastPos.y-3f)+Vector2.right* side;
        Vector2 v2 = new Vector2(lineCastPos2.x , lineCastPos2.y-3f) + Vector2.right * side;
        Vector2 block_distance = lineCastPos + Vector2.right*1* side;
        bool next_willBeBlocked = Physics2D.Linecast(block_distance - coll.wallSize*40 * Vector2.up * 0.2f, block_distance + coll.wallSize * Vector2.up * 0.5f, groundLayer);
      // UnityEngine.Debug.DrawLine(block_distance - coll.wallSize*40 * Vector2.up , (block_distance + coll.wallSize * Vector2.up) , Color.blue);
        UnityEngine.Debug.DrawLine(v1,v2 , Color.blue);
        bool fall = Physics2D.Linecast(v1, v2, groundLayer);
        bool jump_willBeBlocked = Physics2D.Linecast(block_distance + coll.wallSize * Vector2.up *1.5f, block_distance + coll.wallSize * Vector2.up*1.5f + Vector2.right*side, groundLayer);
        //UnityEngine.Debug.DrawLine(block_distance + coll.wallSize * Vector2.up * 1f, block_distance + coll.wallSize * Vector2.up * 1.5f + Vector2.right * side, Color.blue);

        //UnityEngine.Debug.Log(next_willBeBlocked+" "+jump_willBeBlocked+" "+willBeGrounded+" "+ jump_willBeGrounded);
       // UnityEngine.Debug.Log(lineCastPos + Vector2.right * 2 * side);
        if (coll.onGround)
        {
            if ((next_willBeBlocked || !fall) && jump_willBeGrounded)
            {
                print("salta");
                jump();
                groundTouch = false;
            }
            else if (next_willBeBlocked && !jump_willBeBlocked)
            {
                print("salta");
                jump();
                groundTouch = false;
            } else
            {
                if(groundTouch)
                    TurnOnBorder();
            }
        }
        
        if (coll.onGround && myBody.velocity.y<jumpForce)
            MoveBySide(true);

        if (coll.onGround && !groundTouch)
        {
            //GroundTouch();
            groundTouch = true;
        }
        /*
        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }*/
    }
    /// <summary>
    /// The movement of the spider, the spider looks if it need to go left or right to be near the player
    /// if the player jumps to other platform, the spider looks if it can jump, if can jump, it will try, but if not, the spider will stay in the place
    /// </summary>
    private void move()
    {
        Vector2 lineCastPos = myTrans.position - myTrans.right * (myWidth*3) - myTrans.up * (myHeight*2);
        Vector2 lineCastPos2 = myTrans.position + myTrans.right * (myWidth*3) - myTrans.up * myHeight;
        
        bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, groundLayer);
        bool isGrounded2 = Physics2D.Linecast(lineCastPos2, lineCastPos2 + Vector2.down, groundLayer);
        bool canGrounded;

        UnityEngine.Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down, Color.green);
        // print("target " + target.transform.position.x+" "+ myTrans.position.x);
        if (myTrans.position.x > target.transform.position.x)
        {
            canGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + (Vector2.left *5), groundLayer);

            //Debug.Log("1 "+ canGrounded+" "+ isGrounded+" "+canJump);
            if (isGrounded)
            {
                Vector2 myVel = myBody.velocity;
                myVel.x = -myTrans.right.x * speed;
                myBody.velocity = myVel;
            }
        }
        else
        {
            canGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + (Vector2.right * 6), groundLayer);

            //Debug.Log("2 " + canGrounded + " " + isGrounded + " "+canJump);
            if (isGrounded2)
            {
                Vector2 myVel = myBody.velocity;
                myVel.x = -myTrans.right.x * -speed;
                myBody.velocity = myVel;

            }
        }
    }
    /// <summary>
    /// the jump action of the spider
    /// </summary>
    private void jump()
    {
        UnityEngine.Debug.Log("JUMP ");
        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        myBody.velocity += Vector2.up * jumpForce;
        canJump = false;
    }
    //If the spider collides with ground, can jump again
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
       /* if (collision.gameObject.tag == "Ground")
        {
            canJump = true;
        }*/
    }
    //If the spider stops colliding with ground, can't jump
    private void OnCollisionExit2D(Collision2D collision)
    {
        
       /* if(collision.gameObject.tag == "Ground")
        {
            canJump = false;
        }*/
    }
}
