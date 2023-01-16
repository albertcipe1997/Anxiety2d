using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;
using System.Diagnostics;

public class PartnerFireflyAI : MonoBehaviour
{
    /// <summary>
    /// The target should be the player
    /// </summary>
    private Vector3 target;

    /// <summary>
    /// Speed applied to the force added to the Rigidbody2D
    /// </summary>
    public float speed = 200f;
    /// <summary>
    /// The distance bettwen waypoints
    /// </summary>
    public float nextWaypointDistance = 3f;

    /// <summary>
    /// The distance where there will be no force applied
    /// </summary>
    float stopDistance = 2;

    Pathfinding.Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    /// <summary>
    /// If true the stop distance will be respected
    /// </summary>
    bool isPlayer = false;

    Rigidbody2D rb;

    Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {

        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        target = PlayerController.playerInstance.transform.position;

        if (seeker)
            InvokeRepeating("UpadatePath", 0f, .5f);

    }

    /// <summary>
    /// Check if the seeker is done drawing the path and in that case draw a new path again
    /// </summary>
    void UpadatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target, OnPathComplete);
        }
    }

    /// <summary>
    /// Sets the new path ans resets the currentWaypont
    /// </summary>
    /// <param name="p">New path that will be saved</param>
    void OnPathComplete(Pathfinding.Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            UnityEngine.Debug.Log("Campanilla se mueve");
            /*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, 0));
            float distance;
            xy.Raycast(ray, out distance);
            target = ray.GetPoint(distance);*/

            target = this.transform.position + (Vector3) new Vector2 (Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

            isPlayer = false;
        }
        else
        {
            if (PlayerController.playerInstance)
            {
                target = PlayerController.playerInstance.transform.position;
            }

            isPlayer = true;
        }
    }

    void FixedUpdate()
    {
        if (isPlayer)
            followTarget();
        else
        {
            //the target will be the place the firefly will hold
            Vector2 direction = ((Vector2)target - rb.position).normalized;
            float distance = Vector2.Distance(rb.position, (Vector2)target);

            
                this.transform.Translate(direction * speed / 30 * Time.deltaTime, Space.World);

                //Flip sprite depending on force
                if (direction.x >= 0.01f)
                    GetComponentInChildren<SpriteRenderer>().flipX = false;
                else if (direction.x <= -0.01f)
                    GetComponentInChildren<SpriteRenderer>().flipX = true;
            
        }


    }

    /// <summary>
    /// Simply aply force to reach the target
    /// </summary>
    void followTarget()
    {

        Vector2 direction = ((Vector2)target - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        float distance = Vector2.Distance(rb.position, (Vector2)target);

        if ((!isPlayer && distance > 0.25f) || (isPlayer && distance > stopDistance))
            rb.AddForce(force);


        //Flip sprite depending on force
        if (direction.x >= 0.01f)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        else if (direction.x <= -0.01f)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
    }
    /// <summary>
    /// Use path to apply forces that will get to the target
    /// </summary>
    void followTargetBySeeker()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint += 1;
        }

        //Flip sprite depending on velocity
        if (force.x >= 0.01f)
            GetComponentInChildren<SpriteRenderer>().flipX = false;
        else if (force.x <= -0.01f)
            GetComponentInChildren<SpriteRenderer>().flipX = true;
    }

}
