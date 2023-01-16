using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   
    public PlayerController player;

    // Use this for initialization
    void Start()
    {
        player = PlayerController.playerInstance;//GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        

    }

    // Update is called once per frame
    void Update()
    {
       
    }

   
 
}
