using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is the base for all enemies
/// </summary>
public abstract class EnemyController : MonoBehaviour
{
    [Header("Layers")]
    /// <summary>
    /// LayerMask of the ground where the collision will have effect
    /// </summary>
    public LayerMask groundLayer;

    [Space]
    [Header("Basic")]
    /// <summary>
    /// Number of hitpoints to kill the enemy
    /// </summary>
    public int hp = 2;
    /// <summary>
    /// The number of hitpoints the enemy deals
    /// </summary>
    public int dmg = 1;
    /// <summary>
    /// The side wich the sprite is lookin, 1 right -1 left
    /// </summary>
    public int side = 1;
    /// <summary>
    /// The speed of the enemy, how fast the enemy can move
    /// </summary>
    public float speed;

    [Space]

    /// <summary>
    /// Detected collisions
    /// </summary>
    protected DetectedCollision coll;
    /// <summary>
    /// Rigidbody of the enemy
    /// </summary>
    protected Rigidbody2D myBody;
    /// <summary>
    /// Transform of the enemy
    /// </summary>
    protected Transform myTrans;
    /// <summary>
    /// Width from the sprite of the enemy
    /// </summary>
    protected float myWidth;
    /// <summary>
    /// Height from the sprite of the enemy
    /// </summary>
    protected float myHeight;
    /// <summary>
    /// Return depending on the side where the comprobation should be
    /// </summary>
    protected Vector2 lineCastPos;
    /// <summary>
    /// Return a second vector depending on the side where the comprobation should be and also the speed
    /// </summary>
    protected Vector2 lineCastPos2;

    /// <summary>
    /// Return true if the enemy wont fall from a lot of distance
    /// </summary>
    protected bool willBeGrounded;

    // Start is called before the first frame update
    protected void Start()
    {
        coll = GetComponent<DetectedCollision>();
        myBody = this.GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        myWidth = this.GetComponentInChildren<SpriteRenderer>().bounds.extents.x;
        myHeight = this.GetComponentInChildren<SpriteRenderer>().bounds.extents.y;

        if(side==-1)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        
    }

    // Update is called once per frame
    protected void Update()
    {
        GetComponentInChildren<SpriteRenderer>().flipX = (side == 1) ? false : true;

        if (coll)
        {
            //lineCastPos = (((Vector2)myTrans.position) + coll.rightOffset/*new Vector2(coll.rightOffset.x, coll.bottomOffset.y)*/) * side; //the left offset will be the same on negative
            

            if (side == 1)
            {
                lineCastPos = ((Vector2)myTrans.position) + coll.rightOffset;
                lineCastPos2 = lineCastPos + coll.rightOffset * 0.25f * speed;
            }
            else
            {
                lineCastPos = ((Vector2)myTrans.position) + coll.leftOffset;
                lineCastPos2 = lineCastPos + coll.leftOffset * 0.25f * speed;
            }

            //AI checker for the all enemies
            willBeGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + coll.bottomOffset + Vector2.down * 4f, groundLayer);
            willBeGrounded |= Physics2D.Linecast(lineCastPos2, lineCastPos2 + coll.bottomOffset + Vector2.down * 4f, groundLayer);
            //willBeGrounded = Physics2D.CapsuleCast(lineCastPos, lineCastPos + coll.bottomOffset + Vector2.down * 4f, groundLayer);
            Debug.DrawLine(lineCastPos, lineCastPos + coll.bottomOffset + Vector2.down * 4, Color.red);
            Debug.DrawLine(lineCastPos2, lineCastPos2 + coll.bottomOffset + Vector2.down * 4, Color.red);
            //Vector2 size = new Vector2(0.1f, 1f);
            //Vector2 offset = new Vector2(0, -0.5f);
            //bool willBeGrounded = Physics2D.OverlapCapsule(lineCastPos + offset, size, CapsuleDirection2D.Vertical, 0, groundLayer);
            //Vector2 frente = myTrans.right * side;
            //Debug.DrawLine(lineCastPos, lineCastPos + frente * 0.02f);
            //bool willBeBlocked = Physics2D.Linecast(lineCastPos, lineCastPos + frente * 0.02f, groundLayer);
            //print(this.name + ": willBeGrounded(" + willBeGrounded + "), willBeBlocked(" + willBeBlocked + ")");
        }
        else
        {
            print("El enemigo " + this.name + " no tiene el script DetectedCollision");
        }
    }

    /// <summary>
    /// The Enemy will turn using the collisions offsets from DetectedCollision to check if there is ground in front of it
    /// if it sees an empty path to walk on it will automatically turn only while onGround is active
    /// </summary>
    public void TurnOnBorder()
    {
        if (!willBeGrounded && coll.onGround)
        {
            //Girar
            Turn();
        }
    }
    /// <summary>
    /// The Enemy will turn using the spriteRenderer bounds to check if there is ground in front of it
    /// if it sees an empty path to walk on it will automatically turn only while onGround is active
    /// </summary>
    public void TurnOnBorderByRendererSize()
    {
        lineCastPos = myTrans.position + myTrans.right * myWidth * side;

        Debug.DrawLine(lineCastPos, lineCastPos + Vector2.down);
        //bool isGrounded = Physics2D.Linecast(lineCastPos, lineCastPos + Vector2.down, groundLayer);
        Vector2 size = new Vector2(0.1f, 1f);
        Vector2 offset = new Vector2(0, -0.5f);
        bool willBeGrounded = Physics2D.OverlapCapsule(lineCastPos + offset, size, CapsuleDirection2D.Vertical, 0, groundLayer);
        //Vector2 frente = myTrans.right * side;
        //Debug.DrawLine(lineCastPos, lineCastPos + frente * 0.02f);
        //bool willBeBlocked = Physics2D.Linecast(lineCastPos, lineCastPos + frente * 0.02f, groundLayer);
        if (!willBeGrounded && coll.onGround)
        {
            //Girar
            Turn();
        }

    }

    /// <summary>
    /// The Enemy will change the side and will flip its SpriteRenderer
    /// </summary>
    public void Turn()
    {
        /*
        if (coll.onGround)
        {
        }*/
        //Girar
        GetComponentInChildren<SpriteRenderer>().flipX = !GetComponentInChildren<SpriteRenderer>().flipX;
        side = -side;
    }

    /// <summary>
    /// The enemy will move forward with their own speed
    /// It will turn on wall collision
    /// It wont move if trapped between two walls
    /// </summary>
    /// <param name="turnOnCollision">The enemy will turn on direct collision</param>
    public void MoveBySide(bool turnOnCollision)
    {
        if ((side == 1 && coll.onRightWall) && (side == -1 && coll.onLeftWall))
            return;
        if (turnOnCollision)
        {
            if (side == 1 && coll.onRightWall)
                Turn();
            if (side == -1 && coll.onLeftWall)
                Turn();
        }
        else
        {
            if (side == 1 && coll.onRightWall)
                return;
            if (side == -1 && coll.onLeftWall)
                return;
        }

        myBody.velocity = new Vector2(speed * side, myBody.velocity.y);
    }


}
