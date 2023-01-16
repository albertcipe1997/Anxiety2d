using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private string levelType = "";
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name.Contains("Garden"))
            levelType = "Garden";
        else if (SceneManager.GetActiveScene().name.Contains("Depresion"))
            levelType = "Depresion";
        else if (SceneManager.GetActiveScene().name.Contains("Ansiedad"))
            levelType = "Ansiedad";
        else
            levelType = "Ira";

    }
    public void Start()
    {
        AudioSource audio;

        if (GetComponent<AudioSource>())
        {
            audio = GetComponent<AudioSource>();

            if (audio.clip != null)
            {
                if (audio.clip.name != levelType)
                {
                    AudioClip newClip = Resources.Load<AudioClip>("Audio/" + levelType);
                    if (newClip)
                    {
                        audio.clip = newClip;
                        audio.Play();
                    }
                }
            }
            else
            {
                AudioClip newClip = Resources.Load<AudioClip>("Audio/" + levelType);
                if (newClip)
                {
                    audio.clip = newClip;
                    audio.Play();
                }
            }
        }
    }
}
