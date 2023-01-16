using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour
{
    public GameObject stella;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        GetComponent<PlayerController>().canGlide = GetComponent<SaveScript>().readEvent("plane");
        GetComponent<PlayerController>().canDash = GetComponent<SaveScript>().readEvent("dash");
        stella.SetActive(GetComponent<SaveScript>().readEvent("stella"));
    }
}
