using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SaveScript saveScript = GetComponent<SaveScript>();
        if (saveScript.readEvent("Palanca2")&& saveScript.readEvent("Palanca2"))
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
