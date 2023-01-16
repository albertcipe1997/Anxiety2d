using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject Load;
    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/anxietygamesave.save";
        Load.gameObject.SetActive(System.IO.File.Exists(path));
    }

   
}
