using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstiantiatePlayer : MonoBehaviour
{
    public int startId = 0;

    private List<int> otherIds;

    public int flip = 1;

    private void Awake()
    {
        otherIds = new List<int>();

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("StartPosition"))
        {
            if (go != this.gameObject)
            {
                InstiantiatePlayer ip = go.GetComponent<InstiantiatePlayer>();

                otherIds.Add(ip.startId);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
                CreatePlayer();
            

    }

    /// <summary>
    /// This function instantiates the player at the same position as this game object's transform 
    /// </summary>
    private void CreatePlayer()
    {
        PlayerController pc = Instantiate(Resources.Load<PlayerController>("Stella/Stella"));

        pc.name = "Player";
        pc.transform.position = transform.position;
        pc.side = flip;

        //print("player instantiated");
    }

}
