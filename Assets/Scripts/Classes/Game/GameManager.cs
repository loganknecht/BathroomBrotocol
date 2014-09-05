using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//-----------------------------
// Copied this information from here for saving process
// http://unity3d.com/learn/tutorials/modules/beginner/live-training-archive/persistence-data-saving-loading
//-----------------------------

[System.Serializable]
public class GameData {
  public CareerModeData careerModeData = new CareerModeData();
}

[System.Serializable]
public class CareerModeData : MonoBehaviour {
  public bool hasCompletedCareerMode {
    get {
      return false;
    }
  }

  public Dictionary<string, GameObject> playerCareerModeLevelInfo = new Dictionary<string, GameObject>();

  public void ConfigureCareerModeLevelData() {
    GameObject newLevel = BuildCareerModeLevelDataObject("Try-Outs: Come At Me Bro", true, false, 0f);
    playerCareerModeLevelInfo.Add(newLevel.GetComponent<CareerModeLevelData>().levelName, newLevel);

    // newLevel = BuildCareerModeLevelDataObject("Try-Outs: Prove Yourself");
  }

  public GameObject BuildCareerModeLevelDataObject(string levelName, bool canBeSkipped, bool completedLevel, float highestScore) {
    GameObject newLevelInfo = new GameObject(levelName + "InfoGameObject");
    newLevelInfo.transform.parent = this.gameObject.transform;

    CareerModeLevelData careerModeLevelDataRef = (newLevelInfo.AddComponent<CareerModeLevelData>()).GetComponent<CareerModeLevelData>();
    careerModeLevelDataRef.levelName = levelName;
    careerModeLevelDataRef.canBeSkipped = canBeSkipped;
    careerModeLevelDataRef.completedLevel = completedLevel;
    careerModeLevelDataRef.highestScore = highestScore;

    return newLevelInfo;
  }
}

[System.Serializable]
public class CareerModeLevelData : MonoBehaviour{
  public string levelName = "";

  public bool canBeSkipped = false;
  public bool completedLevel = false;

  public float highestScore = 0;
}

public class GameManager : MonoBehaviour {

  public string playerDataFilePath = "";
  public GameData playerData;

  //BEGINNING OF SINGLETON CODE CONFIGURATION
  private static volatile GameManager _instance;
  private static object _lock = new object();

  //Stops the lock being created ahead of time if it's not necessary
  static GameManager() {
  }

  public static GameManager Instance {
    get {
      if(_instance == null) {
        lock(_lock) {
          if (_instance == null) {
            GameObject GameManagerGameObject = new GameObject("GameManagerGameObject");
            _instance = (GameManagerGameObject.AddComponent<GameManager>()).GetComponent<GameManager>();
          }
        }
      }
      return _instance;
    }
  }

  private GameManager() {
  }

  public void Awake() {
    //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
    //the script, which is assumedly attached to some GameObject. This in turn allows the instance
    //to be assigned when a game object is given this script in the scene view.
    //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
    //another call to it, so that you don't need to worry if it exists or not.
    _instance = this;

    playerDataFilePath = Application.persistentDataPath + "/playerInfo.dat";
  }
  //END OF SINGLETON CODE CONFIGURATION

  // Use this for initialization
  void Start () {
    LoadGameData();
  }

  // Update is called once per frame
  void Update () {
  }

  public void LoadGameData() {
    bool foundDataFile = File.Exists(playerDataFilePath);

    if(foundDataFile) {
      BinaryFormatter bf = new BinaryFormatter();
      FileStream file = File.Open(playerDataFilePath, FileMode.Open);

      playerData = (GameData)bf.Deserialize(file);
      file.Close();
      // careerModeData.LoadData();
    }
    else {
      playerData = new GameData();
      playerData.careerModeData.ConfigureCareerModeLevelData();
    }
  }

  public void SaveGameData() {
    BinaryFormatter bf = new BinaryFormatter();
    FileStream file = File.Open(playerDataFilePath, FileMode.Open);

    bf.Serialize(file, playerData);
    file.Close();
  }
}
