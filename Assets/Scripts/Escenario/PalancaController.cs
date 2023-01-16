 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PalancaController : MonoBehaviour
{
    public string scene;
    public bool canOpen; 
    private SaveScript saveScript;
    public string name;

    private void Awake()
    {
        saveScript = GetComponent<SaveScript>();

    }
    // Start is called before the first frame update
    void Start()
    {
        canOpen = false; saveScript = GetComponent<SaveScript>();
        if (saveScript.readEvent(name))
        {
            GetComponent<SpriteRenderer>().flipX=true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)&& canOpen)
        {
            //Debug.Log("Scene " + GetComponent<SpriteRenderer>().flipX);

            Debug.Log("Scene " + scene);
            GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
            saveScript.setEvent(name);
            if (scene!=null)
            {
                //Debug.Log("Scene "+scene.name);
                SceneManager.LoadScene(scene);
            }
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
