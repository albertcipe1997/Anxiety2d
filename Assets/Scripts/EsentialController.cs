using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class EsentialController : MonoBehaviour
{
    public string name;
    private SaveScript saveScript;
    // Start is called before the first frame update
    void Start()
    {

        saveScript = GetComponent<SaveScript>();
        if (saveScript.readEvent(name))
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        UnityEngine.Debug.Log("collision " + collision.gameObject.tag);

        if (collision.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            saveScript.setEvent(name);
        }
    }
}
