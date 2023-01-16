using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;
    private PlayerController move;
    private DetectedCollision coll;
    [HideInInspector]
    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coll = GetComponentInParent<DetectedCollision>();
        move = GetComponentInParent<PlayerController>();
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        setBoolExisting("onGround", coll.onGround);
        setBoolExisting("onWall", coll.onWall);
        setBoolExisting("onRightWall", coll.onRightWall);
        //In case of being a graphic from the PlayerController
        if (move)
        {
            setBoolExisting("wallGrab", move.wallGrab);
            //anim.SetBool("wallSlide", move.wallSlide);
            setBoolExisting("canMove", move.canMove);
            setBoolExisting("isDashing", move.isDashing);
        }
    }
    public void SetHorizontalMovement(float x, float y, float yVel)
    { 
        setFloatExisting("HorizontalAxis", x);
        setFloatExisting("VerticalAxis", y);
        setFloatExisting("VerticalVelocity", yVel);
       

    }

    /// <summary>
    /// Check if param exists in animator at the same time that applies the changes the bool for no errors
    /// </summary>
    /// <param name="_ParamName">The name of the parameter inside the animator</param>
    /// <param name="flag">The boolean that will be applied in the specified parameter of the animator</param>
    void setBoolExisting(string _ParamName, bool flag)
    {
        anim = GetComponent<Animator>();
        bool hasParameter = false;

        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == _ParamName) hasParameter = true;
        }
        if (hasParameter)
        {
            anim.SetBool(_ParamName, flag);
        }
    }
    /// <summary>
    /// Check if param exists in animator at the same time that applies the changes the float for no errors
    /// </summary>
    /// <param name="_ParamName">The name of the parameter inside the animator</param>
    /// <param name="flag">The float that will be applied in the specified parameter of the animator</param>
    void setFloatExisting(string _ParamName, float number)
    {
        anim = GetComponent<Animator>();
        bool hasParameter = false;
        
        foreach (AnimatorControllerParameter param in anim.parameters)
        {
            if (param.name == _ParamName)
            {
                
                hasParameter = true;
            }
        }
        if (hasParameter)
        {
            anim.SetFloat(_ParamName, number);
        }
    }
    /// <summary>
    /// Active animation defined by the trigger of the Animator from other script
    /// </summary>
    /// <param name="trigger">Name of the trigger</param>
    public void SetTrigger(string trigger)
    {
        anim.SetTrigger(trigger);
    }

    /// <summary>
    /// Change direction of the sprite renderer, flipX
    /// </summary>
    /// <param name="side">-1 Left 1 Right</param>
    public void Flip(int side)
    {
        /*
        if (move.wallGrab || move.wallSlide)
        {
            if (side == -1 && sr.flipX)
                return;

            if (side == 1 && !sr.flipX)
            {
                return;
            }
        }*/

        bool state = (side == 1) ? false : true;
        sr.flipX = state;
    }
}
