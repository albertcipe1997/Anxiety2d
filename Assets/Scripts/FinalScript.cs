using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FinalScript : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        player = FindObjectOfType<GameObject>().gameObject;
        Vector3 bar = this.transform.position;
        Vector3 bar2 = player.transform.position;
        Vector3 objectScale = this.transform.localScale;
        if(bar.x - bar2.x>0.1)
            gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = 1 / ((bar.x - bar2.x) / 200);
        else
            gameObject.GetComponent<UnityEngine.Rendering.Universal.Light2D>().pointLightOuterRadius = 1 / (0.0000001f) ;
    }
}
