using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TextGeneratorController : MonoBehaviour
{
    private bool showText;
    public GameObject visbleText;
    public List<string> listText;
    public int chosen = -1;
    private float alpha = 0;
    // Start is called before the first frame update
    void Start()
    {
        showText = true;

        visbleText = GameObject.Find("MotivationText");
        visbleText.GetComponent<TextMesh>().color = new Color(255, 255, 255, alpha);

        visbleText.GetComponent<MeshRenderer>().sortingOrder = 20;


    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (showText)
            {
                if (alpha == 0)
                {
                    int newChosen = UnityEngine.Random.Range(0, listText.Count);
                    while (newChosen == chosen)
                    {
                        newChosen = UnityEngine.Random.Range(0, listText.Count);
                    }
                    visbleText.GetComponent<TextMesh>().text = listText[newChosen].Replace("*", System.Environment.NewLine);
                    UnityEngine.Debug.Log(visbleText.GetComponent<TextMesh>().text );
                    chosen = newChosen;
                    alpha += 0.05f;
                }
                else
                {
                    if (alpha < 1)
                    {
                        alpha += 0.05f;
                    }
                    else
                    {
                        StartCoroutine(counter());
                    }
                }
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= 0.05f;
                }
                else
                {
                    alpha = 0;
                    StartCoroutine(newtext());

                }
            }
            visbleText.GetComponent<TextMesh>().color = new Color(255, 255, 255, alpha);
        }
        catch(Exception e)
        {

            UnityEngine.Debug.Log("pETO AQUI "+ e.Message);
        }
    }

    private IEnumerator newtext()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(10, 30));
        
        StopAllCoroutines();
        showText = true;
    }

    private IEnumerator counter()
    {
        yield return new WaitForSeconds(1.5f + 0.15f * (visbleText.GetComponent<TextMesh>().text.Length));
        showText = false;
    }
}
