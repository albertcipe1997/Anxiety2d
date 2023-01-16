using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeController : MonoBehaviour
{
    // Start is called before the first frame update
    public string name;
    private SaveScript saveScript;
    // Start is called before the first frame update
    void Start()
    {

        saveScript = GetComponent<SaveScript>();
        if (saveScript.readEvent(name))
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
      //  Debug.Log(name + " " + saveScript.readEvent(name));
        if (saveScript.readEvent(name))
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
