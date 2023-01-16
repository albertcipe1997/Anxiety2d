using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// Target of the camera while there is no focus GameObject
    /// </summary>
    public PlayerController player;
    /// <summary>
    /// The wall at the top of the area
    /// </summary>
    public GameObject highWall;
    /// <summary>
    /// The wall at the bottom of the area
    /// </summary>
    public GameObject lowWall;
    /// <summary>
    /// The wall at the right of the area
    /// </summary>
    public GameObject rightWall;
    /// <summary>
    /// The wall at the left of the area
    /// </summary>
    public GameObject leftWall;

    private float yMax;
    private float yMin;
    private float xMax;
    private float xMin;

    /// <summary>
    /// Distance from the target that will have the camera
    /// </summary>
    public Vector3 offset = new Vector3(0, 0, -110f);
    /// <summary>
    /// Velocity in wich the camera will follow the target with Lerp
    /// </summary>
    public float smoothSpeed = 0.125f;

    private GameObject background;

    private float previousVerticalOffset;

    // Use this for initialization
    void Start()
    {
        player = PlayerController.playerInstance;//GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        background = GameObject.FindWithTag("Background");
        if (background)
            background.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 10);

        previousVerticalOffset = offset.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (!player)
        {
            player = PlayerController.playerInstance;//GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        if (!background)
        {
            background = GameObject.FindWithTag("Background");
        }

        if (player)
        {
            if (player.hasControl && !player.isDying && !player.isDead)
            {
                if ((Input.GetAxis("Vertical") > 0 && !highWall.GetComponent<Renderer>().isVisible))
                {
                    offset.y = previousVerticalOffset + Input.GetAxis("Vertical") * 2f;
                }
                else if ((Input.GetAxis("Vertical") < 0 && !lowWall.GetComponent<Renderer>().isVisible))
                {
                    offset.y = previousVerticalOffset + Input.GetAxis("Vertical") * 2f;
                }
                else
                {
                    offset.y = previousVerticalOffset;
                }
            }
        }
    }

    //LateUpdate will smooth the camera following the target position
    private void LateUpdate()
    {
        if (player)
        {
            if (!player.isDead && !player.isDying)
            {
                if (player.focusCamera)
                {
                    FocusCamera(player.focusCamera.transform);
                }
                else
                {
                    if (highWall && lowWall && leftWall && rightWall)
                        SnapEdges();
                }
            }
        }
        if (background)
        {
            background.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 10);
        }
    }

    /// <summary>
    /// When the player has the focusCamera variable not null the camera will focus this new target
    /// </summary>
    private void FocusCamera(Transform targetFocus)
    {
        Vector3 desiredPosition = new Vector2(targetFocus.position.x, targetFocus.position.y);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition + offset, smoothSpeed);
        transform.position = smoothedPosition;
    }
    /// <summary>
    /// It will follow the player smoothly and will stop on all edges
    /// </summary>
    private void SnapEdges()
    {
        //Saber si se ven los bordes para no avanzar en esa direccion
        bool arriba = highWall.GetComponent<Renderer>().isVisible;
        bool abajo = lowWall.GetComponent<Renderer>().isVisible;
        bool derecha = rightWall.GetComponent<Renderer>().isVisible;
        bool izquierda = leftWall.GetComponent<Renderer>().isVisible;

        Vector3 desiredPosition = Vector3.zero;

        float newX = player.transform.position.x;
        if ((izquierda && newX < transform.position.x) || (derecha && newX > transform.position.x))
            newX = transform.position.x;
        float newY = player.transform.position.y;
        if ((abajo && newY < transform.position.y) || (arriba && newY > transform.position.y))
        {
            if ((Input.GetAxis("Vertical") < 0 && abajo) || (Input.GetAxis("Vertical") > 0 && arriba) || (Input.GetAxis("Vertical") == 0))
            {
                newY = transform.position.y;
            }
        }

        desiredPosition = new Vector2(newX, newY);

        float newSmoothSpeed = smoothSpeed;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance >= 1)
            newSmoothSpeed *= distance;
        else if (distance >= 2)
            newSmoothSpeed *= distance * 50;
        /*
    if (Mathf.Abs(player.rb.velocity.x) >= player.dashSpeed)
        newSmoothSpeed = 1f;
        //newSmoothSpeed *= 4;
    else if (Mathf.Abs(player.rb.velocity.x) >= player.speed)
        newSmoothSpeed = Mathf.Abs(player.rb.velocity.x) / player.speed * 0.5f;
    //newSmoothSpeed *= 2;
    */

        //newSmoothSpeed = Mathf.Abs(player.rb.velocity.x) / player.speed;

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition + offset, newSmoothSpeed);
        transform.position = smoothedPosition;

    }

    [System.Obsolete("This is an obsolete method")]
    /// <summary>
    /// Scrolls defining limits positions
    /// </summary>
    private void SamePosition()
    {
        yMax = highWall.transform.position.y;
        yMin = lowWall.transform.position.y;
        xMax = rightWall.transform.position.x;
        xMin = leftWall.transform.position.x;

        //if within the bounds, camera locks onto player
        if (player.transform.position.y < yMax && player.transform.position.y > yMin)
        {
            transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -110.0f);
        }

        //if player is above/below the y axis binders, camera locks to player on xAxis and stays stationary 
        //on yAxis
        if (player.transform.position.y > yMax)
        {
            transform.position = new Vector3(player.transform.position.x, yMax, -110.0f);
        }
        else if (player.transform.position.y < yMin)
        {
            transform.position = new Vector3(player.transform.position.x, yMin, -110.0f);
        }

        //if player is right/left of the xAxis binders, camera locks to player on yAxis and stays stationary 
        //on xAxis
        if (player.transform.position.x > xMax)
        {
            transform.position = new Vector3(xMax, player.transform.position.y, -110.0f);
        }
        else if (player.transform.position.x < xMin)
        {
            transform.position = new Vector3(xMin, player.transform.position.y, -110.0f);
        }

        //if player is above the yAxis binder, and to the right of the xAxis, the camera stays stationary
        if (player.transform.position.y > yMax && player.transform.position.x > xMax)
        {
            transform.position = new Vector3(xMax, yMax, -110.0f);
        }
        //if player is above the yAxis binder, and to the left of the xAxis, the camera stays stationary
        if (player.transform.position.y > yMax && player.transform.position.x < xMin)
        {
            transform.position = new Vector3(xMin, yMax, -110.0f);
        }
        //if player is below the yAxis binder, and to the right of the xAxis, the camera stays stationary
        if (player.transform.position.y < yMin && player.transform.position.x > xMax)
        {
            transform.position = new Vector3(xMax, yMin, -110.0f);
        }
        //if player is below the yAxis binder, and to the left of the xAxis, the camera stays stationary
        if (player.transform.position.y < yMin && player.transform.position.x < xMin)
        {
            transform.position = new Vector3(xMin, yMin, -110.0f);
        }
    }
}
