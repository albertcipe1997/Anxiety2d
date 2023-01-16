using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GeneratorController : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public int x;
    public int y;
    public Vector3 pos;
    public int n_enemies;
    private bool activate = false;
    // Start is called before the first frame update
    void Start()
    {
        pos = new Vector3(this.transform.position.x+x , this.transform.position.y + y, this.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (activate) 
            if (n_enemies != 0) {

                player = GameObject.Find("Player");
                Vector3 bar = this.transform.position;
                    Vector3 bar2 = player.transform.position; 
                UnityEngine.Debug.Log("hola "+ bar2.x+" "+ bar.x);

                if ((bar2 - bar).x < 0.5)
                    {
                        UnityEngine.Debug.Log(pos);
                        enemy.transform.position = pos;
                        Instantiate((GameObject)enemy);
                        n_enemies--;
                    }
                }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
       
        activate = false;
            
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
     if (n_enemies != 0)
        if (collision.CompareTag("Player"))
        {
            activate = true;
        }
    }
}
