
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public int hp = 2;
    public float spd = 2;
    public int dmg = 1;
    public GameObject[] wayPoints;
    int nextWaypoint = 1;
    float distToPoint;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        move();
    }
    public void move()
    {
        //The slime always moves towards the next point
        distToPoint = Vector2.Distance(transform.position, wayPoints[nextWaypoint].transform.position);
        transform.position = Vector2.MoveTowards(transform.position, wayPoints[nextWaypoint].transform.position, spd * Time.deltaTime);
        //When the slime is near the next point rotates and change the point
        if(distToPoint < 0.1f)
        {
            TakeTurn();
        }
    }
    void TakeTurn()
    {
        //The slime changes its rotation with the rotation of the point
        if (wayPoints.Length > 2)
        {
            Vector3 currRot = transform.eulerAngles;
            currRot.z += wayPoints[nextWaypoint].transform.eulerAngles.z;
            currRot.x += wayPoints[nextWaypoint].transform.eulerAngles.x;
            currRot.y += wayPoints[nextWaypoint].transform.eulerAngles.y;
            //Debug.Log("Hola "+ currRot);
            transform.eulerAngles = currRot;
            //And we change the next point
            ChooseNextWaypoint();
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            ChooseNextWaypoint();
        }
    }
    void ChooseNextWaypoint()
    {
        //We choose the next point
        nextWaypoint++;
        //But if the point is the last one, we change to the first in the array
        if (nextWaypoint == wayPoints.Length)
        {
            nextWaypoint = 0;
        }
    }

}
