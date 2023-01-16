using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTextScript : MonoBehaviour
{
    //It's similar to Intro controller but with only text

    //The text that shows in the screen
    public string Text;
    //the max time of the timer
    private float waitTime = 0.03f;

    //the timer
    private float timer = 0.0f;
    //the opacity of the text that is controlled with the timer
    private float alphaLever = .0f;


    //to control the exit of a text
    private bool desc = false;
    // Start is called before the first frame update
    private void Awake()
    {

        //instantiate the texts we change the * to a new line
        Text = Text.Replace("*", System.Environment.NewLine);
        GetComponent<TextMesh>().text = Text;
        //put the order in layer in the front. That is for controll that the objects in the game don't opacate to the text
        gameObject.GetComponent<MeshRenderer>().sortingOrder = 20;
    }
    public void Update()
    {//increments the timer;
        timer += Time.deltaTime;

        // Check if we have reached beyond 2 seconds.
        // Subtracting two is more accurate over time than resetting to zero.

        //if the player is collided with the gameObject starts to show the text or not
        if (desc)
        {

            //for change the opacity of the text 
            if (alphaLever < 1)
            {
                if (timer > waitTime)
                {
                    alphaLever += 0.01f;

                    this.GetComponent<TextMesh>().color = new Color(255, 255, 0, alphaLever);



                    // Remove the recorded 2 seconds.
                    timer = timer - waitTime;

                }
            }
        }
        else
        {

            //for change the ocult the text 
            if (alphaLever > 0)
            {
                if (timer > waitTime)
                {
                    alphaLever -= 0.01f;

                    this.GetComponent<TextMesh>().color = new Color(255, 255, 0, alphaLever);
                    // Remove the recorded 2 seconds.
                    timer = timer - waitTime;

                }
            }






        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            desc = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            desc = false;
        }
    }
}
