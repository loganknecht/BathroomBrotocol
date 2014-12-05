using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomObjectManager : MonoBehaviour {

    public List<GameObject> allBathroomObjects = new List<GameObject>();
    public List<GameObject> topLevelBathroomObjectContainers = new List<GameObject>();

    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile BathroomObjectManager _instance;
    private static object _lock = new object();

    //Stops the lock being created ahead of time if it's not necessary
    static BathroomObjectManager() {
    }

    public static BathroomObjectManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if (_instance == null) {
                        GameObject bathroomObjectManagerGameObject = new GameObject("BathroomObjectManagerGameObject");
                        _instance = (bathroomObjectManagerGameObject.AddComponent<BathroomObjectManager>()).GetComponent<BathroomObjectManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private BathroomObjectManager() {
    }

    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION

    // Use this for initialization
    void Start () {
        AddAllBathroomContainerChildren();
        // Debug.Log("lol starting");

        // BathroomTile[] allBathroomObjects = Resources.FindObjectsOfTypeAll(typeof(BathroomTile)) as BathroomTile[]; 
        // // GameObject[] allBathroomGameObjects = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];

        // if(allBathroomObjects == null) {
        //   Debug.Log("null game objects");
        // }

        // Debug.Log(allBathroomObjects.Length);

        // Debug.Log("lol post starting");

        // foreach(BathroomTile bathroomObject in allBathroomObjects) {
        // // foreach(GameObject bathroomGameObject in allBathroomGameObjects) {
        //   Debug.Log(bathroomObject.name);
        // }
    }

    // Update is called once per frame
    void Update () {
        ResetAllBathroomObjectsIsSelected(true);
    }

    public void AddAllBathroomContainerChildren() {
        foreach(GameObject topLevelBathroomObjectContainer in topLevelBathroomObjectContainers) {
            foreach(Transform child in topLevelBathroomObjectContainer.transform) {
                if(!allBathroomObjects.Contains(child.gameObject)) {
                  allBathroomObjects.Add(child.gameObject);
                }
            }
        }
    }

    public void AddBathroomObject(GameObject broToAdd) {
        allBathroomObjects.Add(broToAdd);
        broToAdd.transform.parent = this.gameObject.transform;
    }

    public void RemoveBathroomObject(GameObject bathroomObjectToRemove, bool destroyBathroomObject) {
        allBathroomObjects.Remove(bathroomObjectToRemove);
        if(destroyBathroomObject) {
            Destroy(bathroomObjectToRemove);
        }
    }

    public void ResetAllBathroomObjectsIsSelected(bool ignoreCurrentlySelectedBathroomObject) {
        foreach(GameObject bathroomObject in allBathroomObjects) {
            if(ignoreCurrentlySelectedBathroomObject
                && SelectionManager.Instance.currentlySelectedBathroomObject != null){
                if(bathroomObject.GetInstanceID() != SelectionManager.Instance.currentlySelectedBathroomObject.GetInstanceID()) {
                    bathroomObject.GetComponent<BathroomObject>().selectableReference.isSelected = false;
                }
            }
            else {
                bathroomObject.GetComponent<BathroomObject>().selectableReference.isSelected = false;
            }
        }
    }

    public int GetCountOfSpecificObjectType(BathroomObjectType bathroomObjectTypeToCount){
        int numberOfObjectTypeCounted = 0;
        foreach(GameObject gameObj in allBathroomObjects) {
            if(gameObj.GetComponent<BathroomObject>()
                && gameObj.GetComponent<BathroomObject>().type == bathroomObjectTypeToCount) {
                numberOfObjectTypeCounted++;
            }
        }
        return numberOfObjectTypeCounted;
  }

  public List<GameObject> GetAllBathroomObjectsOfSpecificType(params BathroomObjectType[] bathroomObjectTypesToReturn) {
    List<GameObject> gameObjectsToReturn = new List<GameObject>();
    foreach(GameObject gameObj in allBathroomObjects) {
      foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypesToReturn) {
        if(gameObj.GetComponent<BathroomObject>()
           && gameObj.GetComponent<BathroomObject>().type == bathroomObjectType) {
          gameObjectsToReturn.Add(gameObj);
        }
      }
    }

    return gameObjectsToReturn;
  }

  public List<GameObject> GetAllOpenBathroomObjectsOfSpecificType(params BathroomObjectType[] bathroomObjectTypesToReturn) {
    List<GameObject> gameObjectsToReturn = new List<GameObject>();
    foreach(GameObject gameObj in allBathroomObjects) {
      BathroomObject bathroomObjRef = gameObj.GetComponent<BathroomObject>();
      foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypesToReturn) {
        if(bathroomObjRef != null
           && gameObj.GetComponent<BathroomObject>().state != BathroomObjectState.Broken
           && gameObj.GetComponent<BathroomObject>().state != BathroomObjectState.BrokenByPee
           && gameObj.GetComponent<BathroomObject>().state != BathroomObjectState.BrokenByPoop
           && gameObj.GetComponent<BathroomObject>().type == bathroomObjectType
           && gameObj.GetComponent<BathroomObject>().objectsOccupyingBathroomObject.Count == 0) {
          gameObjectsToReturn.Add(gameObj);
        }
      }
    }

    return gameObjectsToReturn;
  }

  public GameObject GetRandomBathroomObjectOfSpecificType(params BathroomObjectType[] bathroomObjectTypes) {
    // GameObject
    List<GameObject> bathroomObjectsOfSpecifiedType = GetAllBathroomObjectsOfSpecificType(bathroomObjectTypes);
    if(bathroomObjectsOfSpecifiedType.Count == 0) {
      return null;
    }
    else {
      int selectedBathroomObject = Random.Range(0, bathroomObjectsOfSpecifiedType.Count);
      return bathroomObjectsOfSpecifiedType[selectedBathroomObject];
    }
  }

  public GameObject GetRandomOpenBathroomObjectOfSpecificType(params BathroomObjectType[] bathroomObjectTypes) {
    // GameObject
    List<GameObject> bathroomObjectsOfSpecifiedType = GetAllOpenBathroomObjectsOfSpecificType(bathroomObjectTypes);
    if(bathroomObjectsOfSpecifiedType.Count == 0) {
      return null;
    }
    else {
      int selectedBathroomObject = Random.Range(0, bathroomObjectsOfSpecifiedType.Count);
      return bathroomObjectsOfSpecifiedType[selectedBathroomObject];
    }
  }
  public float GetPercentageOfBathroomObjectTypeBroken(params BathroomObjectType[] bathroomObjectTypes) {
    float totalObjectsFound = 0f;
    float totalObjectsFoundBroken = 0f;
    foreach(GameObject bathroomObject in allBathroomObjects) {
      BathroomObject bathObjRef = bathroomObject.GetComponent<BathroomObject>();

      foreach(BathroomObjectType bathroomObjectType in bathroomObjectTypes) {
        if(bathObjRef.type == bathroomObjectType) {
          totalObjectsFound++;
          if(bathObjRef.state == BathroomObjectState.Broken
             || bathObjRef.state == BathroomObjectState.BrokenByPee
             || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
            totalObjectsFoundBroken++;
          }
        }
      }
    }

    if(totalObjectsFoundBroken == 0) {
      return 0;
    }
    else {
      return totalObjectsFoundBroken/totalObjectsFound;
    }
  }
}
