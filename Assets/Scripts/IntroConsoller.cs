using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroConsoller : MonoBehaviour
{
    public List<CanvasGroup> groups = new List<CanvasGroup>();

    //the stat that says what text shows in the screen
    int state = 0;

    //the max time of the timer
    private float waitTime = 0.03f;

    //the timer to controll the opacity of texts
    private float timer = 0.0f;

    //the opacity
    public float alphaLever = .0f;

    private bool desc = false;
    void Start()
    {   //instantiate the texts
        foreach (CanvasGroup cg in groups)
        {
            cg.alpha = alphaLever;
        }
    }

    // Update is called once per frame
    void Update()
    {//increments the timer;
        timer += Time.deltaTime;

        //Jump fading group
        if (alphaLever > 0.25f && Input.GetMouseButtonDown(0))
        {
            //timer = waitTime + Time.deltaTime;
            timer = 0;
            alphaLever = 0;
            if (state < groups.Count)
            {
                groups[state].alpha = alphaLever;
                state++;
                desc = false;
                if (state >= groups.Count)
                {
                    SceneManager.LoadScene("StartMenu");
                }
            }
        }
        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.
        if (alphaLever < 1 && !desc)
        {
            if (timer > waitTime)
            {
                alphaLever += 0.01f;
                //change the opacity
                if (state < groups.Count)
                {
                    groups[state].alpha = alphaLever;
                }

                // Remove the recorded 2 seconds.
                timer = 0;//timer - waitTime;
            }
        }
        else
        {//the same to show screen but invert
            if (!desc)
            {
                desc = true;
            }
            else
            {
                if (alphaLever > 0)
                {
                    if (timer > waitTime)
                    {
                        alphaLever -= 0.01f;
                        //change the opacity
                        if (state < groups.Count)
                        {
                            groups[state].alpha = alphaLever;
                        }

                        // Remove the recorded 2 seconds.
                        timer = 0;//timer - waitTime;
                    }
                }
                else
                {
                    state++;
                    desc = false;
                    if (state >= groups.Count)
                    {
                        SceneManager.LoadScene("StartMenu");
                    }
                }
            }
        }

    }
}
