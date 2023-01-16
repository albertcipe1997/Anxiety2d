using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {

        
            Debug.Log("hOLA");
            Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name.Equals("StartMenu"))
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene("StartMenu");
        }
    }

    public void Resume()
    {
        GameObject.Find("Pause").SetActive(false);
        GameObject.Find("Stella").GetComponent<PlayerController>().canMove = true;
        GameObject.Find("Stella").GetComponent<PlayerController>().canJump = true;
    }
}
