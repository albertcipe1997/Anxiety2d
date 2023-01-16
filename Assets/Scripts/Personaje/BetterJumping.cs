using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// When enabled will multiply the speed of falling
/// and also it will lower the speed of your jump when not holding jump
/// </summary>
public class BetterJumping : MonoBehaviour
{
    private Rigidbody2D rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float maxFallVelocity = -10f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            if (rb.velocity.y < maxFallVelocity)
            {
                rb.velocity = new Vector2(rb.velocity.x, maxFallVelocity);
            }
        }
        else if(rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
