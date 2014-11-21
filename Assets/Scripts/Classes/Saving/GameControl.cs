using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace Assets.Scripts.Classes
{
    class GameControl : MonoBehaviour
    {
        public static GameControl control;

        public bool newGame = true;
        
        void Awake()
        {
            if(control == null)
            {
                DontDestroyOnLoad(gameObject);
                control = this;
            }
            else if(control != this)
            {
                Destroy(gameObject);
            }
        }

        void Update()
        {
            GUI.Label(new Rect(10, 10, 100, 30), "NewGame: " + newGame.ToString());
        }
        public void Save()
        {
            BinaryFormatter bf = new BinaryFormatter();
            
            //Save file path info
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
            
            //Save data to container
            PlayerData data = new PlayerData();
            data.newGame = newGame;

            //serialize and close the save data to the file
            bf.Serialize(file, data);
            file.Close();
        }
        public void Load()
        {
            if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
                PlayerData data = (PlayerData)bf.Deserialize(file);
                file.Close();

                //load saved data to singleton GameControl object
                newGame = data.newGame;
            }
        }
    }

    [Serializable]
    class PlayerData
    {
        public bool newGame;
    }
}
