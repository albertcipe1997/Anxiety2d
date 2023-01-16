using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzAnimator : MonoBehaviour
{
    public UnityEngine.Rendering.Universal.Light2D leftEye;
    public UnityEngine.Rendering.Universal.Light2D rightEye;
    public UnityEngine.Rendering.Universal.Light2D mouth;

    public bool flag = false;
    public bool acces = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (acces)
            if (flag && mouth.intensity > 0.1f)
            {
                mouth.intensity -= 0.01f;
                leftEye.intensity -= 0.01f;
                rightEye.intensity -= 0.01f;
                acces = false;

                StartCoroutine(lightWait());

            }
            else if (mouth.intensity < 1.1f)
            {
                mouth.intensity += 0.01f;
                leftEye.intensity += 0.01f;
                rightEye.intensity += 0.01f;
                acces = false;

                flag = false;
                StartCoroutine(lightWait());
            }
            else
            {
                acces = true;
                flag = true;
            }
            
    }

    IEnumerator lightWait()
    {
        UnityEngine.Debug.Log("Hola");
        yield return new WaitForSeconds(UnityEngine.Random.Range(0.001f, 0.05f));

        acces = true;
    }
}
