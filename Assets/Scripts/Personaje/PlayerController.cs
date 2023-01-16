using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{ /// <summary>
  /// The side of collison detected
  /// </summary>
    private DetectedCollision coll;
    /// <summary>
    /// Rigidbody of the player's character
    /// </summary>

    /// <summary>
    /// Audio of the player's character
    /// </summary>
    public AudioSource audio_source;
    private AnimationController anim;
    private AnimationController anim2;
    private AnimationController anim3;
    private AnimationController anim4;

    public Rigidbody2D rb;

    public int stellaState=0;
    public int probability=5;
    public bool isNearEnemy = false;

    public GameObject normal;
    public GameObject ansiedad;
    public GameObject depresion;
    public GameObject ira;

    /// <summary>
    /// The main camera will focus this GameObject in case that it's not null
    /// </summary>
    public GameObject focusCamera;
    public GameObject canvas;

    /// <summary>
    /// Checks if it just started to aply some effects like in SituationChanger
    /// </summary>
    private bool started = true;
    [Space]
    [Header("Stats")]
    /// <summary>
    /// Horizontal velocity of the player
    /// </summary>
    public float speed = 7;
    /// <summary>
    /// The force applied to the player when jumped
    /// </summary>
    public float jumpForce = 15;
    /// <summary>
    /// Horizontal velocity while dashing
    /// </summary>
    public float dashSpeed = 30;
    /// <summary>
    /// Horizontal velocity while dashing
    /// </summary>
    public float dashTime = 0.15f;
    /// <summary>
    /// Falling speed while gliding
    /// </summary>
    public float glidingFallSpeed = -0.25f;

    private bool hasDashed;
    /// <summary>
    /// Side where the player is looking, it's -1 left and 1 right
    /// </summary>
    public int side = 1;

    [Space]
    [Header("Booleans")]
    /// <summary>
    /// Unlocks the user to take control of the player GameObject
    /// </summary>
    public bool hasControl;
    /// <summary>
    /// Unlocks the player movement
    /// </summary>
    public bool canMove;
    /// <summary>
    /// Unlocks the jumping mechanic
    /// </summary>
    public bool canJump;
    /// <summary>
    /// Unlocks the gliding mechanic
    /// </summary>
    public bool canGlide;
    /// <summary>
    /// Notify if the player is gliding
    /// </summary>
    public bool isGliding;
    /// <summary>
    /// Unlocks the dash mechanic
    /// </summary>
    public bool canDash;
    /// <summary>
    /// Notify if the player is dashing
    /// </summary>
    public bool isDashing;
    /// <summary>
    /// Notify if the player is against a wall
    /// </summary>
    public bool wallGrab;

    /// <summary>
    /// Notify if the player is is Invulnerable to know if it can receive damage or get killed
    /// </summary>
    public bool isInvulnerable;
    /// <summary>
    /// Notify if the player is dying
    /// </summary>
    public bool isDying;
    /// <summary>
    /// Notify if the player is dead
    /// </summary>
    public bool isDead;

    [Space]
    [Header("Trails")]
    /// <summary>
    /// Trail at the center of the player prepared to emmit only when the user dashes
    /// </summary>
    public TrailRenderer dashTrail;


    public static PlayerController playerInstance;


    private bool groundTouch;

    public ParticleSystem groundParticle;

    public ParticleSystem dashParticle;


    private void Awake()
    {
        if (playerInstance == null)
        {
            playerInstance = this;

            
            //DontDestroyOnLoad(playerObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        coll = GetComponent<DetectedCollision>();
        rb = GetComponent<Rigidbody2D>();
        anim = normal.GetComponent<AnimationController>();
        //audio_source = GetComponent<AudioSource>();
        isDashing = false; 
        canMove = true;
        canJump = true;
        dashTrail.emitting = false;
        isDead = false;
        Revive();

        //loadSaveVariables();
        //anim.Flip(side);

    }
   
    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown("l"))
            {
                GetComponent<SaveScript>().setEvent("palanca1");
                GetComponent<SaveScript>().setEvent("palanca2");
                GetComponent<SaveScript>().setEvent("palanca3");
                GetComponent<SaveScript>().setEvent("paltaforma1");
                GetComponent<SaveScript>().setEvent("stella");
                GetComponent<SaveScript>().setEvent("plane");
                GetComponent<SaveScript>().setEvent("dash");
                GetComponent<SaveScript>().LoadData();



            }

            if (Input.GetKeyDown("p"))
            {
                gameObject.transform.Find("Pause").gameObject.SetActive(true);
                canMove = false;
                canJump = false;
            }
            if (Input.GetKeyDown("r"))
            {
                SaveScript saveScript = GetComponent<SaveScript>();
                saveScript.SaveData("Depresion 1", 0);

                SceneManager.LoadScene("Depresion 1");
            }
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            float xRaw = Input.GetAxisRaw("Horizontal");
            float yRaw = Input.GetAxisRaw("Vertical");
            Vector2 dir = new Vector2(x, y);

            Walk(dir);
            if (x > 0)
            {
                side = 1;
                anim.Flip(side);
                ansiedad.GetComponent<SpriteRenderer>().flipX = false;
                depresion.GetComponent<SpriteRenderer>().flipX = false;
                ira.GetComponent<SpriteRenderer>().flipX = false;

                //Debug.Log(side);
            }
            if (x < 0)
            {
                side = -1;
                anim.Flip(side);
                ansiedad.GetComponent<SpriteRenderer>().flipX = true;
                depresion.GetComponent<SpriteRenderer>().flipX = true;
                ira.GetComponent<SpriteRenderer>().flipX = true;
                //Debug.Log(side);
            }
            anim.SetHorizontalMovement(x, y, rb.velocity.y);
            // Debug.Log("Hola "+x + " " + y + " " + rb.velocity.y);
            if (coll.onGround && !isDashing)
            {
                GetComponent<BetterJumping>().enabled = true;
            }


            //Jump
            if (Input.GetKeyDown(KeyCode.Space))
            {
                anim.SetTrigger("jump");

                if (coll.onGround)
                    Jump(Vector2.up);
                /*
                if (coll.onWall && !coll.onGround)
                    WallJump();
                    */
            }
            if (Input.GetKey(KeyCode.Space))
            {
                if (canGlide && !isDashing)
                {
                    if (!coll.onGround && rb.velocity.y < glidingFallSpeed)
                    {
                        rb.velocity = new Vector2(rb.velocity.x, glidingFallSpeed);
                        isGliding = true;
                    }
                    if (!coll.onGround && rb.velocity.y < 0 && rb.velocity.y == glidingFallSpeed)
                    {
                        isGliding = true;
                    }
                    if (coll.onGround)
                    {
                        isGliding = false;
                    }
                }
                else
                {
                    isGliding = false;
                }
            }
            else
            {
                isGliding = false;
            }
            if (isGliding)
            {
                if (dashParticle.isStopped)
                {
                    dashParticle.Play();
                    //print("paly glide");
                }
            }
            else
            {
                if (dashParticle.isPlaying)
                    dashParticle.Stop();
            }
            //Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) && !hasDashed)
            {
                /*
                if(xRaw != 0 || yRaw != 0)
                    Dash(xRaw, yRaw);
                    */
                Dash();// Dash(xRaw, yRaw);
            }
            //in case of dashing keep de vertial horizontal as 0
            if (isDashing)
            {
                rb.velocity = new Vector2(rb.velocity.x + 15*side, 0);
                //Debug.Log("Dashing "+rb.velocity);
            }
            if (coll.onGround && !groundTouch)
            {
                GroundTouch();
                groundTouch = true;
            }

            if (!coll.onGround && groundTouch)
            {
                groundTouch = false;
            }
            if (!hasControl)
                return;

            if (!canMove)
                return;



            if (started)
                started = false;



        }
        else
        {
            canvas.SetActive(true);
        }

    }

    public void Revive()
    {
        //isDead = false;
        rb.velocity = Vector2.zero;
        //StartCoroutine(invincibility());

        //Temporal
       // anim.sr.enabled = true;
        //anim.sr.color = Color.white;
        //print("position");
    }
    

    private void Walk(Vector2 dir)
    {
       
        /*
        if (coll.onMovingPlatform && Mathf.Abs(dir.x) < 0.1f)
            return;*/

        //print("se mueve");

        //The player wont stop if its feeling form is Anxiety
     
           rb.velocity = new Vector2(dir.x * speed, rb.velocity.y);

    }
    /// <summary>
     /// The player will jump in a specified direction
     /// </summary>
     /// <param name="dir"></param>
    private void Jump(Vector2 dir)
    {
      
       
        if (!canMove)
            return;
        if (!canJump)
            return;
        if (isDashing)
            return;

        //slideParticle.transform.parent.localScale = new Vector3(ParticleSide(), 1, 1);
        //ParticleSystem particle = wall ? wallJumpParticle : jumpParticle;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.velocity += dir * jumpForce;

        //AQUI SONIDO
        PlaySE("PlayerJump"); 
        groundParticle.Play();

    }
    private void Dash()
    {
        if (!canMove)
            return;
        if (!canDash)
            return;
        if (hasDashed)
            return;
        hasDashed = true;

        //anim.SetTrigger("dash");

        rb.velocity = Vector2.zero;
        //Vector2 dir = new Vector2(x, y);
        Vector2 dir = new Vector2(side, 0);
        UnityEngine.Debug.Log("Side "+side);
        rb.velocity += dir * dashSpeed;
        UnityEngine.Debug.Log("Side " + rb.velocity);
        StartCoroutine(DashWait());


        //AQUI SONIDO
        PlaySE("PlayerDash");

    }
    IEnumerator DashWait()
    {
        //Debug.Log("Entra aqui"); 
        dashParticle.Play();
        dashTrail.emitting = true;
        rb.gravityScale = 0;
        GetComponent<BetterJumping>().enabled = false;
        //wallJumped = true;
        isDashing = true;

        //rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        yield return new WaitForSeconds(dashTime);

        dashParticle.Stop();
        dashTrail.emitting = false;
        rb.gravityScale = 3;
        GetComponent<BetterJumping>().enabled = true;
        //wallJumped = false;
        isDashing = false;
        yield return new WaitForSeconds(dashTime * 5);
        if (coll.onGround)
            hasDashed = false;
    }

    void GroundTouch()
    {
        hasDashed = false;
        isDashing = false;

        side = anim.sr.flipX ? -1 : 1;

        if (rb.velocity.y < 0)
        {

            PlaySE("PlayerGround");
            groundParticle.Play();
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            //Debug.Log("Entra "+col.gameObject.tag);
            isNearEnemy = true;

            StopCoroutine(enemyCooldown());

            stellaState =1;
                switch (stellaState)
                {
                    case 0:
                        normal.SetActive(true);
                        ansiedad.SetActive(false);
                        depresion.SetActive(false);
                        ira.SetActive(false);
                      
                        break;

                    case 1:
                        normal.SetActive(false);
                        ansiedad.SetActive(true);
                        depresion.SetActive(false);
                        ira.SetActive(false);
                        break;

                    case 2:
                        normal.SetActive(false);
                        ansiedad.SetActive(false);
                        depresion.SetActive(true);
                        ira.SetActive(false);
                        break;
                    case 3:
                        normal.SetActive(false);
                        ansiedad.SetActive(false);
                        depresion.SetActive(false);
                        ira.SetActive(true);
                        break;
                }
           
               
        }
    }



    void OnTriggerExit2D(Collider2D col)
    {       
        if (col.gameObject.tag == "Enemy") 
        {
           
            if (isNearEnemy)
            {
                isNearEnemy=false;
                StartCoroutine(enemyCooldown());

            }

        }
    }
    IEnumerator enemyCooldown()
    {
        yield return new WaitForSeconds(4);
        if (!isNearEnemy)
        {
            int number = UnityEngine.Random.Range(1, 10);
            if (number < 4)
                StartCoroutine(otherState());
            else
            {
              //  Debug.Log("salgo");
                isNearEnemy = false;
                stellaState = 0;
                switch (stellaState)
                {
                    case 0:
                        normal.SetActive(true);
                        ansiedad.SetActive(false);
                        depresion.SetActive(false);
                        ira.SetActive(false);

                        break;

                    case 1:
                        normal.SetActive(false);
                        ansiedad.SetActive(true);
                        depresion.SetActive(false);
                        ira.SetActive(false);
                        break;

                    case 2:
                        normal.SetActive(false);
                        ansiedad.SetActive(false);
                        depresion.SetActive(true);
                        ira.SetActive(false);
                        break;
                    case 3:
                        normal.SetActive(false);
                        ansiedad.SetActive(false);
                        depresion.SetActive(false);
                        ira.SetActive(true);
                        break;
                }
            }
        }
        else
        {
            StartCoroutine(enemyCooldown());

        }
           
        
    }

    IEnumerator otherState()
    {
        int number = UnityEngine.Random.Range(1, 10);
        if (number < probability)
        {
            stellaState = 2;
            probability--;
        }
        else
        {
            stellaState = 3; 
            probability++;
        }
        switch (stellaState)
        {
            case 0:
                normal.SetActive(true);
                ansiedad.SetActive(false);
                depresion.SetActive(false);
                ira.SetActive(false);

                break;

            case 1:
                normal.SetActive(false);
                ansiedad.SetActive(true);
                depresion.SetActive(false);
                ira.SetActive(false);
                break;

            case 2:
                normal.SetActive(false);
                ansiedad.SetActive(false);
                depresion.SetActive(true);
                ira.SetActive(false);
                break;
            case 3:
                normal.SetActive(false);
                ansiedad.SetActive(false);
                depresion.SetActive(false);
                ira.SetActive(true);
                break;
        }

        yield return new WaitForSeconds(6);

        isNearEnemy = false;
        if(!isNearEnemy)
            stellaState = 0; 
        else
            stellaState = 1;
        switch (stellaState)
        {
            case 0:
                normal.SetActive(true);
                ansiedad.SetActive(false);
                depresion.SetActive(false);
                ira.SetActive(false);

                break;

            case 1:
                normal.SetActive(false);
                ansiedad.SetActive(true);
                depresion.SetActive(false);
                ira.SetActive(false);

                StartCoroutine(enemyCooldown());
                break;

            case 2:
                normal.SetActive(false);
                ansiedad.SetActive(false);
                depresion.SetActive(true);
                ira.SetActive(false);
                break;
            case 3:
                normal.SetActive(false);
                ansiedad.SetActive(false);
                depresion.SetActive(false);
                ira.SetActive(true);
                break;
        }

    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            PlaySE("yodadeath");
            //audio_source.PlayOneShot(Resources.Load<AudioClip>("Audio/SoundEffects/yodadeath"));
            //this.gameObject.GetComponent<DeathMenu>().deathUI();
            anim.sr.color = Color.red;
            isDead = true;
        }
            
        if (col.gameObject.tag == "Finish")
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("Enemy");
            //loop through the returned array of game objects and set each to active false
            foreach (GameObject go in gos)
                go.SetActive(false);
            
                StartCoroutine(finish());


        }
    }
    IEnumerator finish()
    {

        UnityEngine.Debug.Log("HOLA finish 1");
        GameObject g = gameObject.transform.Find("Final").gameObject;
        g.SetActive(true);

        UnityEngine.Debug.Log("HOLA finish 2");
        canMove = false;
        canJump = false;

        UnityEngine.Debug.Log("HOLA finish 3");
        for (int i = 0; i < 256; i++)
        {
            yield return new WaitForSeconds(0.025f);
            byte byt = (byte)i;
            UnityEngine.Debug.Log("HOLA " + byt);
            g.transform.Find("Panel").gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color32(0, 0, 0, byt);
        }

        UnityEngine.Debug.Log("HOLA finish 5");
        audio_source.Stop();
        yield return new WaitForSeconds(1.5f);

        AudioClip clip = Resources.Load<AudioClip>("Audio/laugh");
        audio_source.PlayOneShot(clip);

        yield return new WaitForSeconds(1f);
        g.transform.GetChild(1).gameObject.SetActive(true);

    }

    public void PlaySE(String soundName)
    {
        UnityEngine.Debug.Log(soundName);
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + soundName);

        UnityEngine.Debug.Log(clip);
        if (clip)
            audio_source.PlayOneShot(clip);
    }


}
