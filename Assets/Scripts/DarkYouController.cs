using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkYouController : MonoBehaviour
{
    [Header("DarkYou")]
    public SpriteRenderer sprite;
    public float targetDistance = 2.5f;
    public bool canDash;
    private bool hasDashed;
    public float dashSpeed = 10;
    public ParticleSystem dashParticle;
    public TrailRenderer dashTrail;
    private int side=-1;
    //public bool findEnemy;

    /// <summary>
    /// Notify if this enemy is dashing
    /// </summary>
    public bool isDashing;
    /// <summary>
    /// The force of the jump when the enemy jumps
    /// </summary>
    public float jumpForce = 12;
    /// <summary>
    /// If is true, the spider can jump else cannot jump
    /// </summary>
    private bool canJump = true;
    /// <summary>
    /// If is true, the spider just jumped
    /// </summary>
    private bool groundTouch = true;

    private Rigidbody2D myBody;
    private DetectedCollision coll;
    private Transform myTrans;
    private Vector2 lineCastPos;
    public LayerMask groundLayer;
    // Start is called before the first frame update
    protected new void Start()
    {

        sprite = GetComponentInChildren<SpriteRenderer>();
        //findEnemy = false;
        //dashh = true;
        //cancooldown = true;
        myBody = GetComponent<Rigidbody2D>();
        coll= GetComponent<DetectedCollision>();
        myTrans = GetComponent<Transform>();
        lineCastPos = new Vector2(0.0f, 0.0f);

    }

    // Update is called once per frame
    protected new void Update()
    {

        //TurnOnBorder();

        //SEEK MECHANIC
        if (!isDashing && !hasDashed)
        {
            if (PlayerController.playerInstance.transform.position.x < transform.position.x)
            {
                side = -1;
            }
            else
            {
                side = 1;
            }
        }

       
        if (isDashing)
        {
            
            myBody.velocity = new Vector2(Mathf.Abs(myBody.velocity.x) * side, 0);
        }
        //in case of dashing keep de vertial horizontal as 0

        GetComponentInChildren<AnimationController>().SetHorizontalMovement(myBody.velocity.x, 0, myBody.velocity.y);

        //DASH MECHANIC
        //What to do if it sees the player in front of her
        //if (Physics2D.Linecast(lineCastPos, lineCastPos + frente * 3f, LayerMask.GetMask("Player")))
        //En vez de una linea recta un area en frente del objetov
        Vector2 altura = new Vector2(0, coll.wallSize.y * 1.5f);
        Vector2 frente = myTrans.right * side;
        //Debug.DrawLine(lineCastPos + Vector2.down, lineCastPos + frente * 2.5f + Vector2.up);
        //if (Physics2D.OverlapArea(lineCastPos + Vector2.down, lineCastPos + frente * 2.5f + Vector2.up, LayerMask.GetMask("Player")))
        Debug.DrawLine(lineCastPos - altura / 2, lineCastPos + altura / 2 + frente * targetDistance);
        if (Physics2D.OverlapArea(lineCastPos - altura / 2, lineCastPos + altura / 2 + frente * targetDistance, LayerMask.GetMask("Player")))
        {
            Dash();
        }

        //JUMP MECHANIC
        Vector2 jump_distance = lineCastPos + Vector2.right * 0.75f * side;
        bool jump_willBeGrounded = Physics2D.Linecast(jump_distance, jump_distance + coll.bottomOffset + Vector2.down * 4, groundLayer);
        Debug.DrawLine(jump_distance, jump_distance + coll.bottomOffset + Vector2.down * 4, Color.green);

        Vector2 block_distance = lineCastPos + Vector2.right * 0.75f * side;
        bool next_willBeBlocked = Physics2D.Linecast(block_distance - coll.wallSize * Vector2.up * 0.5f, block_distance + coll.wallSize * Vector2.up * 0.5f, groundLayer);
        Debug.DrawLine(block_distance - coll.wallSize * Vector2.up * 0.5f, block_distance + coll.wallSize * Vector2.up * 0.5f, Color.blue);
        bool jump_willBeBlocked = Physics2D.Linecast(block_distance + coll.wallSize * Vector2.up * 1.5f, block_distance + coll.wallSize * Vector2.up * 1.5f + Vector2.right * side, groundLayer);
        Debug.DrawLine(block_distance + coll.wallSize * Vector2.up * 1.5f, block_distance + coll.wallSize * Vector2.up * 1.5f + Vector2.right * side, Color.blue);

        if (coll.onGround)
        {
           if (next_willBeBlocked && !jump_willBeBlocked)
            {
                jump();
            }
            
        }

        if (coll.onGround && !groundTouch)
        {
            //GroundTouch();
            groundTouch = true;
        }

        if (!coll.onGround && groundTouch)
        {
            groundTouch = false;
        }
    }

    /*
    private IEnumerator cooldown()
    {
        
        cancooldown = false;
        yield return new WaitForSeconds(1.5f);
        dashh = true;
        cancooldown = true;
    }*/

    private void Dash()
    {
        if (!canDash)
            return;
        if (hasDashed)
            return;
        if (isDashing)
            return;

        hasDashed = true;

        //anim.SetTrigger("dash");

        myBody.velocity = Vector2.zero;
        //Vector2 dir = new Vector2(x, y);
        //Debug.Log(dir.normalized+" "+ dashSpeed);
        StartCoroutine(DashWait());
    }

    IEnumerator DashWait()
    {

        Vector2 startingPos = sprite.transform.position;
        //float amount = 0.1f;

        //Preparing before dashing
        Color color = new Color();
        color = sprite.color;
        color.a = 1;
        //for (int i = 0; i < 40; i++)
        while (color.a > 0)
        {
            color.a -= 0.01f;
            sprite.color = color;
            yield return new WaitForSeconds(.01f);
        }
        sprite.color = color;
        //print("finalizado");
        //sprite.transform.position = startingPos;

        //Dash while returning to normal color
        isDashing = true;
        Vector2 dir = new Vector2(side, 0);

        myBody.velocity += dir.normalized * dashSpeed;

        dashParticle.Play();
        dashTrail.emitting = true;
        myBody.gravityScale = 0;

        color.a = 0;
        //for (int i = 0; i < 40; i++)
        while (color.a < 1)
        {
            color.a += 0.05f;
            sprite.color = color;
            yield return new WaitForSeconds(.01f);
        }
        color.a = 1;
        sprite.color = color;
        //GetComponent<BetterJumping>().enabled = false;
        //wallJumped = true;

        //rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(.4f);

        //rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        dashParticle.Stop();
        dashTrail.emitting = false;
        myBody.gravityScale = 3;
        //GetComponent<BetterJumping>().enabled = true;
        //wallJumped = false;
        isDashing = false;
        yield return new WaitForSeconds(1f);
        if (coll.onGround)
            hasDashed = false;
    }
    /// <summary>
    /// the jump action of the spider
    /// </summary>
    private void jump()
    {
        myBody.velocity = new Vector2(myBody.velocity.x, 0);
        myBody.velocity += Vector2.up * jumpForce;
        canJump = false;
    }
}
