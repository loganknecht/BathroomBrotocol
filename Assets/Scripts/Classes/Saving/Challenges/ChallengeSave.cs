using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using UnityEngine.UI;

namespace Assets.Scripts.Classes
{
    class ChallengeSave : MonoBehaviour
    {
        public static ChallengeSave control;

        
        public int ls1;
        public bool l1;
        public Text l1score;
        public GameObject l1lock;

        public int ls2;
        public bool l2;
        public Text l2score; 
        public GameObject l2lock;

        public int ls3;
        public bool l3;
        public Text l3score; 
        public GameObject l3lock;

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
        }
        public void Start()
        {
            l1 = true;
            BinaryFormatter bf = new BinaryFormatter();
            
            //Save file path info
            FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
            
            //Save data to container
            ChallengeData data = new ChallengeData();
            data.l1 = l1;
            if (data.ls1 == null) { data.ls1 = 0; }

            //serialize and close the save data to the file
            bf.Serialize(file, data);
            file.Close();
            Load();
        }
        public void Load()
        {
            if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
                ChallengeData data = (ChallengeData)bf.Deserialize(file);
                file.Close();

                //load saved data to singleton GameControl object

                l1 = data.l1;
                ls1 = data.ls1;
                if(l1 == true)
                {
                    GameObject.Destroy(l1lock);
                    l1score.text = ls1.ToString();
                }
            }
        }
    }
    [Serializable]
    class ChallengeData
    {
        public bool l1;
        public int ls1;
    }
}
