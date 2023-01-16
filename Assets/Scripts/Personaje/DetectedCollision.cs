using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Detect side collision on specific LayerMask
/// </summary>
public class DetectedCollision : MonoBehaviour
{

    [Header("Layers")]
    /// <summary>
    /// LayerMask of the ground where the collision will have effect
    /// </summary>
    public LayerMask groundLayer;

    [Space]
    
    /// <summary>
    /// The gameObject is colliding with the ground
    /// </summary>
    public bool onGround;
    /// <summary>
    /// The gameObject is colliding with a wall
    /// </summary>
    public bool onWall;
    /// <summary>
    /// The gameObject is colliding with a wall on the right side
    /// </summary>
    public bool onRightWall;
    /// <summary>
    /// The gameObject is colliding with a wall on the left side
    /// </summary>
    public bool onLeftWall;
    /// <summary>
    /// The side wall wich the gameObject is colliding with, -1 left, 1 right
    /// </summary>
    public int wallSide;
    /// <summary>
    /// The gameObject is colliding with the ground and it is a moving platform
    /// </summary>
    public bool onMovingPlatform;

    [Space]

    [Header("CollisionSize")]

    //public float collisionRadius = 0.25f;
    /// <summary>
    /// Size of the capsule collider in horizontal direction at the bottom of the GameObject
    /// </summary>
    public Vector2 bottomSize = new Vector2(1f, 2.3f);
    /// <summary>
    /// Size of the capsule collider in vertical direction at each side of the GameObject
    /// </summary>
    public Vector2 wallSize = new Vector2(0.1f, 0.1f);

    [Header("CollisionOffset")]

    /// <summary>
    /// Position from center of the GameObject where the Bottom Collider is
    /// </summary>
    public Vector2 bottomOffset;
    /// <summary>
    /// Position from center of the GameObject where the Right Collider is
    /// </summary>
    public Vector2 rightOffset;
    /// <summary>
    /// Position from center of the GameObject where the Left Collider is
    /// </summary>
    public Vector2 leftOffset;

    private Color debugCollisionColor = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(groundLayer);
        //Debug.Log(Physics2D.OverlapCapsule((Vector2)transform.position + bottomOffset, bottomSize, CapsuleDirection2D.Horizontal, 0, groundLayer));

        //onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, groundLayer);
        onGround = Physics2D.OverlapCapsule((Vector2)transform.position + bottomOffset, bottomSize, CapsuleDirection2D.Horizontal,0, groundLayer);
        
        //onWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer) 
        //    || Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onWall = Physics2D.OverlapCapsule((Vector2)transform.position + rightOffset, wallSize, CapsuleDirection2D.Vertical, 0, groundLayer)
                || Physics2D.OverlapCapsule((Vector2)transform.position + leftOffset, wallSize, CapsuleDirection2D.Vertical, 0, groundLayer);

        //onRightWall = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, groundLayer);
        //onLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, groundLayer);
        onRightWall = Physics2D.OverlapCapsule((Vector2)transform.position + rightOffset, wallSize, CapsuleDirection2D.Vertical, 0, groundLayer);
        onLeftWall = Physics2D.OverlapCapsule((Vector2)transform.position + leftOffset, wallSize, CapsuleDirection2D.Vertical, 0, groundLayer);

        wallSide = onRightWall ? -1 : 1;


    }

    /// <summary>
    /// A cross will be drawn on the scene to simbolize the space that is being used with Physics2D.OverlapCapsule
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        //Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        //Bottom
        Gizmos.DrawLine((Vector2)transform.position + bottomOffset - new Vector2(bottomSize.x / 2, 0), (Vector2)transform.position + bottomOffset + new Vector2(bottomSize.x / 2, 0));
        Gizmos.DrawLine((Vector2)transform.position + bottomOffset - new Vector2(0, bottomSize.y / 2), (Vector2)transform.position + bottomOffset + new Vector2(0, bottomSize.y / 2));
        //Right
        Gizmos.DrawLine((Vector2)transform.position + rightOffset - new Vector2(wallSize.x / 2, 0), (Vector2)transform.position + rightOffset + new Vector2(wallSize.x / 2, 0));
        Gizmos.DrawLine((Vector2)transform.position + rightOffset - new Vector2(0, wallSize.y / 2), (Vector2)transform.position + rightOffset + new Vector2(0, wallSize.y / 2));
        //Left
        Gizmos.DrawLine((Vector2)transform.position + leftOffset - new Vector2(wallSize.x / 2, 0), (Vector2)transform.position + leftOffset + new Vector2(wallSize.x / 2, 0));
        Gizmos.DrawLine((Vector2)transform.position + leftOffset - new Vector2(0, wallSize.y / 2), (Vector2)transform.position + leftOffset + new Vector2(0, wallSize.y / 2));
    }


}
