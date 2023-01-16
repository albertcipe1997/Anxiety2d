using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour
{
    

    //The serialitzable object    
    static public Save gm;

    //the path of the save
    private string path;

    private void Awake()
    {//if(don't have any save object create a new)
       
    }


    void Start()
    {
        if (gm == null)
            gm = new Save();

        Debug.Log("Date Saved");
        path = Application.persistentDataPath + "/anxietygamesave.save";
        //charge the game save
        LoadState();
    }


    //save the game when cross a door
    public void SaveData(string _name, int _startId)
    {
        var save = new Save()
        {
            scene = _name,
            startId = _startId,
            atack = gm.atack,
            dash = gm.dash,
            stella = gm.stella,
            plane = gm.plane,
            shield = gm.shield,
            palanca1 = gm.palanca1,
            palanca2 = gm.palanca2,
            palanca3 = gm.palanca3,
            plataforma1 = gm.plataforma1,
            boss = gm.boss
        };



        gm = save;
        //encode the save and save in the path
        var binaryFormatte = new BinaryFormatter();
        using (var fileStream = File.Create(path))
        {
            binaryFormatte.Serialize(fileStream, save);
        }
        Debug.Log("Date Saved");
        //print(save.scene);

    }

    //load the data of the game if the document exist and charge the scena
    public void LoadData()
    {
        Debug.Log("hOLA");
        if (File.Exists(path))
        {
            Save save;
            var binaryFormater = new BinaryFormatter();

            using (var fileStrea = File.Open(path, FileMode.Open))
            {
                save = (Save)binaryFormater.Deserialize(fileStrea);

            }
            gm.scene = save.scene;
            gm.atack = save.atack;
            gm.shield = save.shield;
            gm.dash = save.dash;
            gm.stella = save.stella;
            gm.plane = save.plane;
            gm.palanca1 = save.palanca1;
            gm.palanca2 = save.palanca2;
            gm.palanca3 = save.palanca3;
            gm.plataforma1 = save.plataforma1;
            gm.boss = save.boss;

                SceneManager.LoadScene(save.scene);
            

        }
        else
        {
            Debug.Log("Not exist any playstate");
        }
    }

    // //load the data of the game if the document exist
    public void LoadState()
    {
        if (File.Exists(path))
        {
            Save save;
            var binaryFormater = new BinaryFormatter();

            using (var fileStrea = File.Open(path, FileMode.Open))
            {
                save = (Save)binaryFormater.Deserialize(fileStrea);

            }
            gm.scene = save.scene;
            gm.atack = save.atack;
            gm.shield = save.shield;
            gm.dash = save.dash;
            gm.stella = save.stella;
            gm.plane = save.plane;
            gm.palanca1 = save.palanca1;
            gm.palanca2 = save.palanca2;
            gm.palanca3 = save.palanca3;
            gm.plataforma1 = save.plataforma1;
            gm.boss = save.boss;

        }
        else
        {
            Debug.Log("Not exist any playstate");
        }
    }


    //get the save data 
    public Save getData()
    {
        Save save;
        var binaryFormater = new BinaryFormatter();

        using (var fileStrea = File.Open(path, FileMode.Open))
        {
            save = (Save)binaryFormater.Deserialize(fileStrea);

        }

        return save;
    }

    //when you chose new game the variables of the save date restarts and charge the first scena

    public void NewGame()
    {
        Debug.Log("Escena ");

        SceneManager.LoadScene("Garden");
        var save = new Save()
        {
            scene = "Garden",
            startId = 0,
            atack = true,
            dash = false,
            stella = false,
            plane = false,
            shield = false,
            palanca1 = false,
            palanca2 = false,
            palanca3 = false,
            plataforma1 = false,
            boss = false

        };
        var binaryFormatte = new BinaryFormatter();
        using (var fileStream = File.Create(path))
        {
            binaryFormatte.Serialize(fileStream, save);
        }

        Debug.Log("Escena " + save.scene);

    }


    //for get the state of a unique variable in the savedata. This is for change the events
    public bool readEvent(string name)
    {
        Save save = getData();
        switch (name)
        {
            case "boss":
                {
                    return save.boss;
                }


            case "palanca1":
                {

                    return save.palanca1;
                }
            case "palanca2":
                {

                    return save.palanca2;
                }
            case "palanca3":
                {

                    return save.palanca3;
                }

            case "plataforma1":
                {
                    return save.plataforma1;
                }
            case "stella":
                {
                    return save.stella;
                }

            case "dash":
                {
                    return save.dash;
                }
            case "plane":
                {
                    return save.plane;
                }

            case "atack":
                {
                    return save.atack;
                }
            case "shield":
                {
                    return save.shield;
                }



        }
        return false;

    }
    //is a simply save of the game when press G
    public void simplySave(Save newSave)
    {
        var save = new Save()
        {
            scene = SceneManager.GetActiveScene().name,           
            atack = newSave.atack,
            dash = newSave.dash,
            plane = newSave.plane,
            shield = newSave.shield,
            palanca1 = newSave.palanca1,
            palanca2 = newSave.palanca2,
            palanca3 = newSave.palanca3,
            plataforma1 = newSave.plataforma1,
            boss = newSave.boss,
            stella = newSave.stella


        };
        var binaryFormatte = new BinaryFormatter();
        using (var fileStream = File.Create(path))
        {
            binaryFormatte.Serialize(fileStream, save);
        }
        Debug.Log("Date Saved");
        print(save.scene);
        
    }

    /// <summary>
    /// Saves the play state variables
    /// </summary>
    public void simplySave()
    {
        Debug.Log("Escena actual " + gm);
       
        var save = new Save()
        {
           
            scene = SceneManager.GetActiveScene().name,       

            atack = gm.atack,
            dash = gm.dash,

            plane = gm.plane,

            shield = gm.shield,
            palanca1 = gm.palanca1,
            palanca2 = gm.palanca2,
            palanca3 = gm.palanca3,
            boss = gm.boss,
            stella = gm.stella
        };
        var binaryFormatte = new BinaryFormatter();
        using (var fileStream = File.Create(path))
        {
            binaryFormatte.Serialize(fileStream, save);
        }
        Debug.Log("Date Saved");
        //print(save.scene);
       
    }


    //when press R recharge the last save and charge the scene
    public void simplyLoad()
    {
        Save save;
        var binaryFormater = new BinaryFormatter();

        using (var fileStrea = File.Open(path, FileMode.Open))
        {
            save = (Save)binaryFormater.Deserialize(fileStrea);

        }
        SceneManager.LoadScene(save.scene);
    }


    //for change the state of an unique variable of savedata, this is for event. After do a simply save.
    public void setEvent(string name)
    {
        Debug.Log(name);
        switch (name)
        {
            case "boss":
                {
                    gm.boss = !gm.boss;
                }
                break;
            case "palanca1":
                {
                    gm.palanca1 = !gm.palanca1;
                }
                break;
            case "palanca2":
                {
                    gm.palanca2 = !gm.palanca2;
                }
                break;
            case "palanca3":
                {
                    gm.palanca3 = !gm.palanca3;
                }
                break;
            case "plataforma1":
                {
                    gm.plataforma1 = !gm.plataforma1;
                }
                break;
            case "stella":
                {
                    gm.stella = !gm.stella;
                }
                break;
            case "dash":
                {
                    //print(gm.dash);
                    gm.dash = !gm.dash;
                }
                break;
            case "plane":
                {
                    gm.plane = !gm.plane;
                }
                break;
            case "atack":
                {
                    gm.atack = !gm.atack;
                }
                break;
            case "shield":
                {
                    gm.shield = !gm.shield;
                }
                break;


        }
        simplySave();
    }
}
