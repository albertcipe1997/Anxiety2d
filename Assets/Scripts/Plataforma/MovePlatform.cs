using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    /// <summary>
    /// how fast is the platform
    /// </summary>
    public float speed = 3;
    /// <summary>
    /// The direction of the platform 1=right/up 2=left/down
    /// </summary>
    public int side = 1;
   /// <summary>
   /// The type 1=horizontal 2=vertical
   /// </summary>
    public int type = 2;
    /// <summary>
    /// the time to change the side
    /// </summary>
    public float ctime = 2.0f;
    protected Rigidbody2D myBody;
    protected Transform myTrans;

    Transform child;

    // Start is called before the first frame update
    void Start()
    {
        myBody = this.GetComponent<Rigidbody2D>();
        myTrans = this.transform;
        StartCoroutine(cSide(ctime));
        
        //myBody.MovePosition(positionRight);

    }

    /// <summary>
    /// fuction to change the side when the ctime finish and start again
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    private IEnumerator cSide(float time)
    {
        yield return new WaitForSeconds(time);
        side*= -1;
        StartCoroutine(cSide(ctime));


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //myBody.MovePosition(transform.position + transform.right * speed * side * Time.deltaTime);


        if (transform.childCount > 0)
        {
            //transform.GetChild(0).position = previousPosition;

            //previousPosition = transform.GetChild(0).position;
        }
        if (type == 1)
        {
            //myBody.velocity = new Vector2(speed * side, 0);//myBody.velocity.y
            transform.position = transform.position + transform.right * speed * side * Time.deltaTime;
        }
        else
        {
            //Vector2 myVel = myBody.velocity;
            //myVel.y = myTrans.up.y * speed*side;
            //myBody.velocity = myVel;
            transform.position = transform.position + transform.up * speed * side * Time.deltaTime;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DetectedCollision dc = collision.gameObject.GetComponent<DetectedCollision>();
        if (dc && dc.onGround)
        {
            collision.collider.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        collision.collider.transform.SetParent(null);
    }
    /*
    void ConnectTo(Rigidbody2D character)
    {
        SliderJoint2D joint = GetComponent<SliderJoint2D>();
        joint.connectedBody = character;
    }
    void OnCollisionEnter2D(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            ConnectTo(collision.collider.GetComponent<Rigidbody2D>());
        }
    }

    void Connect(Rigidbody2D character)
    {
    //...
    // listen to the character jump event
    character.GetComponent<PlayerController>().onJump += OnCharacterJump;
    }
    void OnCharacterJump(PlayerController character)
    {
        Disconnect(character);
    }
    void Disconnect(PlayerController character)
    {
        // do not listen to this anymore
        character.onJump -= OnCharacterJump;
        // disable the joint by unplugging the character
        joint.connectedBody = null;
    }
    */
}