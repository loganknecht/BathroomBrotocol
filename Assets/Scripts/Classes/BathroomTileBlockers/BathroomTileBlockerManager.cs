using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileBlockerManager : MonoBehaviour {
    public List<GameObject> bathroomTileBlockers = new List<GameObject>();

    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile BathroomTileBlockerManager _instance;
    private static object _lock = new object();

    //Stops the lock being created ahead of time if it's not necessary
    static BathroomTileBlockerManager() {
    }

    public static BathroomTileBlockerManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if (_instance == null) {
                        GameObject BathroomTileBlockerManagerGameObject = new GameObject("BathroomTileBlockerManagerGameObject");
                        _instance = (BathroomTileBlockerManagerGameObject.AddComponent<BathroomTileBlockerManager>()).GetComponent<BathroomTileBlockerManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private BathroomTileBlockerManager() {
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
    public virtual void Start() {
    }

    // Update is called once per frame
    public virtual void Update() {
    }

    public void AddBathroomTileBlockerGameObject(GameObject newBathroomTileBlockerGameObject) {
        BathroomTileBlocker newBathroomTileBlocker = newBathroomTileBlockerGameObject.GetComponent<BathroomTileBlocker>();
        if(newBathroomTileBlocker != null) {
            // Adds it to the total list of bathroom tile blockers
            if(!bathroomTileBlockers.Contains(newBathroomTileBlockerGameObject)) {
                bathroomTileBlockers.Add(newBathroomTileBlockerGameObject);
            }

            // Adds it to the tile it is located in
            GameObject bathroomTileGameObjectContainingNewObject = null;
            BathroomTile bathroomTileContainingNewObject = null;

            // First try to locate the tile that the bathroom tile blocker would be in within the tile maps
            bathroomTileGameObjectContainingNewObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(newBathroomTileBlockerGameObject.transform.position.x, newBathroomTileBlockerGameObject.transform.position.y, false);

            // Second try to locate the tile that the bathroom tile blocker would be in within the tile maps
            if(bathroomTileGameObjectContainingNewObject == null) {
                bathroomTileGameObjectContainingNewObject = EntranceQueueManager.Instance.GetTileGameObjectFromLineQueuesyWorldPosition(newBathroomTileBlockerGameObject.transform.position.x, newBathroomTileBlockerGameObject.transform.position.y, false);
            }

            if(bathroomTileGameObjectContainingNewObject != null) {
                AStarManager.Instance.AddTemporaryClosedNode(bathroomTileGameObjectContainingNewObject);
                // Debug.Log("adding");
                bathroomTileContainingNewObject = bathroomTileGameObjectContainingNewObject.GetComponent<BathroomTile>();
                if(bathroomTileContainingNewObject != null) {
                  bathroomTileContainingNewObject.AddBathroomTileBlocker(newBathroomTileBlockerGameObject);
                  newBathroomTileBlocker.SetBathroomTileGameObjectIn(bathroomTileGameObjectContainingNewObject);
                }
            }
        }
        newBathroomTileBlockerGameObject.transform.parent = this.gameObject.transform;
    }

  public void RemoveBathroomTileBlockerGameObject(GameObject bathroomTileBlockerGameObjectToRemove) {
        if(bathroomTileBlockerGameObjectToRemove.GetComponent<BathroomTileBlocker>()) {
            bathroomTileBlockers.Remove(bathroomTileBlockerGameObjectToRemove);
            bathroomTileBlockerGameObjectToRemove.GetComponent<BathroomTileBlocker>().RemoveFromBathroomTileGameObjectIn();
        }
  }

    public List<GameObject> GetListOfBathroomTileGameObjectsContainingBathroomTileBlockers() {
        List<GameObject> bathroomTilesContainingBathroomTileBlockers = new List<GameObject>();
        foreach(GameObject bathroomTileBlocker in bathroomTileBlockers) {
            GameObject bathroomTileGameObjectContainingTileBlocker = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomTileBlocker.transform.position.x, bathroomTileBlocker.transform.position.y, false);
            if(!bathroomTilesContainingBathroomTileBlockers.Contains(bathroomTileGameObjectContainingTileBlocker)) {
                bathroomTilesContainingBathroomTileBlockers.Add(bathroomTileGameObjectContainingTileBlocker);
            }
        }
        
        return bathroomTilesContainingBathroomTileBlockers;
    }
}
