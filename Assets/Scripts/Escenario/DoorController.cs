using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorController : MonoBehaviour
{
    public bool canOpen;
    public string nextScene;
    private SaveScript saveScript;
    // Start is called before the first frame update
    void Start()
    {
        canOpen = false;
        saveScript = GetComponent<SaveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (Input.GetKeyDown(KeyCode.F) && canOpen)
            {

                saveScript.SaveData(nextScene, 0);
                SceneManager.LoadScene(nextScene);
            }
        }
        catch(Exception e)
        {
            File.WriteAllText("C:\\Users\\alber\\OneDrive\\Escritorio\\texto.txt", "Peto por algun motivo uwu "+e.Message);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canOpen= true;
            this.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            canOpen = false;
            this.transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
