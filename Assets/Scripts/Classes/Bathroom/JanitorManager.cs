using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class JanitorManager : MonoBehaviour {

  public bool janitorIsSummoned = false;
  public GameObject janitorButton = null;

  public GameObject currentJanitor = null;
  //public List<GameObject> allJanitors = new List<GameObject>();

  //BEGINNING OF SINGLETON CODE CONFIGURATION
  private static volatile JanitorManager _instance;
  private static object _lock = new object();

  //Stops the lock being created ahead of time if it's not necessary
  static JanitorManager() {
  }

  public static JanitorManager Instance {
    get {
      if(_instance == null) {
        lock(_lock) {
          if (_instance == null) {
            GameObject janitorManagerGameObject = new GameObject("JanitorManagerGameObject");
            _instance = (janitorManagerGameObject.AddComponent<JanitorManager>()).GetComponent<JanitorManager>();
          }
        }
      }
      return _instance;
    }
  }

  private JanitorManager() {
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
  }

  // Update is called once per frame
  void Update () {
    // SetAllJanitorsIsSelected(true);
  }

  public void PerformJanitorSummonButtonLogic() {
    janitorIsSummoned = !janitorIsSummoned;
    if(janitorIsSummoned) {
      if(currentJanitor == null) {
        GameObject newJanitor  = (GameObject)GameObject.Instantiate(Resources.Load("Prefabs/NPC/Janitor/JanitorPlaceHolder1") as GameObject);

        GameObject selectedEntranceLineQueueGameObject = EntranceQueueManager.Instance.SelectRandomLineQueue();
        LineQueue selectedEntranceLineQueue = selectedEntranceLineQueueGameObject.GetComponent<LineQueue>();

        List<Vector2> newMovementNodes = selectedEntranceLineQueue.GetQueueMovementNodes();
        //--------------------------
        newJanitor.transform.position = new Vector3(selectedEntranceLineQueue.queueTileObjects[selectedEntranceLineQueue.queueTileObjects.Count - 1].transform.position.x,
                                                    selectedEntranceLineQueue.queueTileObjects[selectedEntranceLineQueue.queueTileObjects.Count - 1].transform.position.y,
                                                    newJanitor.transform.position.z);
        newJanitor.GetComponent<Janitor>().SetTargetObjectAndTargetPosition(null, newMovementNodes);
        newJanitor.transform.parent = JanitorManager.Instance.transform;

        newJanitor.GetComponent<Janitor>().state = JanitorState.Entering;
        newJanitor.GetComponent<Janitor>().lineQueueIn = selectedEntranceLineQueue;

        currentJanitor = newJanitor;
      }
      else {
        Debug.Log("cancelled exit");

        //do nothing path should be set
        currentJanitor.collider.enabled = true;
        currentJanitor.GetComponent<Janitor>().selectableReference.canBeSelected = true;
        currentJanitor.GetComponent<Janitor>().targetObject = null;
        currentJanitor.GetComponent<Janitor>().state = JanitorState.Roaming;

        BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentJanitor.transform.position.x, currentJanitor.transform.position.y, true).GetComponent<BathroomTile>();
        BathroomTile targetTile = BathroomTileMap.Instance.SelectRandomOpenTile().GetComponent<BathroomTile>();

        List<Vector2> newMovementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                  AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                  startTile,
                                                                                  targetTile);

        currentJanitor.GetComponent<Janitor>().SetTargetObjectAndTargetPosition(null, newMovementNodes);
      }
    }
    else {
      if(currentJanitor != null) {
        if(currentJanitor.GetComponent<Janitor>().state == JanitorState.Entering) {
          //do nothing janitor is entering
          janitorIsSummoned = true;
        }
        else if(currentJanitor.GetComponent<Janitor>().state == JanitorState.Exiting) {
        }
        else {
          List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);

          janitorIsSummoned = false;
          currentJanitor.GetComponent<Janitor>().state = JanitorState.Exiting;
          // int selectedExit = Random.Range(0, ExitManager.Instance.exits.Count);
          int selectedExit = Random.Range(0, exits.Count);

          BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentJanitor.transform.position.x, currentJanitor.transform.position.y, true).GetComponent<BathroomTile>();
          BathroomTile targetTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(exits[selectedExit].transform.position.x, exits[selectedExit].transform.position.y, true).GetComponent<BathroomTile>();

          List<Vector2> newMovementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                    AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                    startTile,
                                                                                    targetTile);

          currentJanitor.GetComponent<Janitor>().SetTargetObjectAndTargetPosition(exits[selectedExit], newMovementNodes);
        }
      }
    }
    // if(janitorIsSummoned){
    //   janitorButton.GetComponent<TweenColor>().duration = 0;
    //   janitorButton.GetComponent<UIButton>().defaultColor = new Color(0, 255, 0, 1);
    //   janitorButton.GetComponent<UIButton>().hover = new Color(0, 255, 0, 1);
    //   janitorButton.GetComponent<UIButton>().pressed = new Color(0, 255, 0, 1);
    //   janitorButton.GetComponent<UIButton>().disabledColor = new Color(0, 255, 0, 1);
    // }
    // else {
    //   janitorButton.GetComponent<TweenColor>().duration = 0;
    //   janitorButton.GetComponent<UIButton>().defaultColor = new Color(255, 0, 0, 1);
    //   janitorButton.GetComponent<UIButton>().hover = new Color(255, 0, 0, 1);
    //   janitorButton.GetComponent<UIButton>().pressed = new Color(255, 0, 0, 1);
    //   janitorButton.GetComponent<UIButton>().disabledColor = new Color(255, 0, 0, 1);
    // }
  }

  public bool IsJanitorSummoned() {
    if(currentJanitor != null) {
      return true;
    }
    else {
      return false;
    }
  }
}
