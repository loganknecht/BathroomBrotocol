using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EntranceQueueManager : BaseBehavior {

    public bool isPaused = false;

    public GameObject entranceAudioObject = null;

    public List<GameObject> lineQueues = new List<GameObject>();

    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile EntranceQueueManager _instance;
    private static object _lock = new object();

    //Stops the lock being created ahead of time if it's not necessary
    static EntranceQueueManager() {
    }

    public static EntranceQueueManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if (_instance == null) {
                        GameObject entranceQueueManagerGameObject = new GameObject("EntranceQueueManagerGameObject");
                        _instance = (entranceQueueManagerGameObject.AddComponent<EntranceQueueManager>()).GetComponent<EntranceQueueManager>();
                    }
                }
            }
            return _instance;
        }
    }

    private EntranceQueueManager() {
    }

    // Use this for initialization
    protected override void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
        base.Awake();
    }
    //END OF SINGLETON CODE CONFIGURATION

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update () {
        if(!isPaused) {
            if(Input.GetKeyDown(KeyCode.B)) {
                foreach(GameObject lineQueueObject in lineQueues) {
                    lineQueueObject.GetComponent<LineQueue>().ReconfigureBrosInLineQueueTiles(true);
                }
            }
            if(entranceAudioObject != null
                && entranceAudioObject.GetComponent<AudioSource>().isPlaying == false) {
                entranceAudioObject = null;
            }
        }
    }

    public List<GameObject> GetLineQueues() {
        return lineQueues;
    }

    public GameObject GetTileGameObjectFromLineQueueByWorldPosition(float xPosition, float yPosition, bool returnClosestTile, int lineQueueToSelectFrom) {
        GameObject tileGameObjectFound = null;
        if(lineQueueToSelectFrom < lineQueues.Count) {
            tileGameObjectFound = lineQueues[lineQueueToSelectFrom].GetComponent<LineQueue>().GetTileGameObjectByWorldPosition(xPosition, yPosition, returnClosestTile);
        }
        else {
            Debug.LogError("You tried searching for a tile game object in a line queue index that does not exist in the entrance queue manager");
        }

        return tileGameObjectFound;
    }

    public GameObject GetTileGameObjectFromLineQueuesyWorldPosition(float xPosition, float yPosition, bool returnClosestTile) { 
        GameObject lineQueueBathroomTileGameObjectFound = null;
        foreach(GameObject lineQueueGameObject in lineQueues) {
            lineQueueGameObject.GetComponent<LineQueue>().GetTileGameObjectByWorldPosition(xPosition, yPosition, returnClosestTile);
            if(lineQueueBathroomTileGameObjectFound != null) {
                // return early saving time for searches, reducing average search case
                return lineQueueBathroomTileGameObjectFound;
            }
        }
        return null;
    }

    public GameObject SelectRandomLineQueue() {
        if(lineQueues.Count == 0) {
          return null;
        }
        else {
          return lineQueues[Random.Range(0, lineQueues.Count)];
        }
    }

    public GameObject AddBroToEntranceQueue(GameObject broToAdd, int entranceQueueToAddTo) {
        LineQueue lineQueueSelected = lineQueues[entranceQueueToAddTo].GetComponent<LineQueue>();

        BroManager.Instance.AddBro(broToAdd);
        Bro broReference = broToAdd.GetComponent<Bro>();

        broReference.state = BroState.InAQueue;
        lineQueueSelected.AddGameObjectToLineQueue(broToAdd);

        broReference.PerformEnteredScore();
        broReference.drawNodeList.Hide();

        if(entranceAudioObject == null) {
            entranceAudioObject = SoundManager.Instance.Play(AudioType.EntranceQueueDoorOpenClubMusic);
        }

        return broToAdd;
    }

    public void RemoveBroFromEntranceQueues(GameObject broToRemove) {
        foreach(GameObject lineQueue in lineQueues) {
            if(lineQueue.GetComponent<LineQueue>() != null
                && lineQueue.GetComponent<LineQueue>().queueObjects.Contains(broToRemove)) {
                lineQueue.GetComponent<LineQueue>().RemoveGameObjectFromLineQueue(broToRemove);
            }
        }
    }
}
