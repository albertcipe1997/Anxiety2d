using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visible : MonoBehaviour
{
    GameObject obj;
    Collider2D objCollider;

    Camera cam;
    Plane[] planes;

    /// <summary>
    /// Defines if it's SpriteRenderer is in the visual field of the camera
    /// </summary>
    public bool neverSeen = true;
    /// <summary>
    /// Defines if it's SpriteRenderer is in the visual field of the camera
    /// </summary>
    public bool isSeen;

    /// <summary>
    /// Frames the camera has being seeing the rendere of this GameObject
    /// </summary>
    public long framesSeen = 0;
    /// <summary>
    /// Frames the camera has stop seeing the rendere of this GameObject
    /// </summary>
    public long framesNotSeen = 0;

    void Start()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (!neverSeen)
        {
            if (isSeen)
            {
                framesSeen++;
            }
            else
            {
                framesNotSeen++;
            }
        }
        //Obsolete
        /*
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            //Debug.Log(obj.name + " has been detected!");
            isSeen = true;
        }
        else
        {
            //Debug.Log("Nothing has been detected");
            isSeen = false;
        }*/
    }

    //Detects that it's SpriteRender it's in the visual field of the camera
    private void OnBecameVisible()
    {
        neverSeen = false;

        isSeen = true;

        framesSeen = 0;
        framesNotSeen = 0;
    }

    //Detects that it's SpriteRender it's out of the visual field of the camera
    private void OnBecameInvisible()
    {
        isSeen = false;

        framesSeen = 0;
        framesNotSeen = 0;
    }
}
