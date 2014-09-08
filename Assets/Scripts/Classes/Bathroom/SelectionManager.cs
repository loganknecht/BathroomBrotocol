using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour {

	public GameObject currentlySelectedBroGameObject = null;
	public GameObject currentlySelectedJanitorGameObject = null;
	public GameObject currentlySelectedBathroomObject = null;
	public GameObject currentlySelectedBathroomTileBlocker = null;

  public bool allowOnlyCorrectReliefTypeSelectionsForBros = false;

	//BEGINNING OF SINGLETON CODE CONFIGURATION
	private static volatile SelectionManager _instance;
	private static object _lock = new object();

	//Stops the lock being created ahead of time if it's not necessary
	static SelectionManager() {
	}

	public static SelectionManager Instance {
		get {
			if(_instance == null) {
				lock(_lock) {
					if (_instance == null) {
						GameObject selectionManagerGameObject = new GameObject("SelectionManager");
						_instance = (selectionManagerGameObject.AddComponent<SelectionManager>()).GetComponent<SelectionManager>();
					}
				}
			}
			return _instance;
		}
	}

	private SelectionManager() {
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
    //Actual selection logic
    PerformCurrentJanitorSelectionAndCurrentBathroomObjectSelectionLogic();
		PerformCurrentBroSelectionAndCurrentBathroomObjectSelectionLogic();
    PerformCurrentJanitorSelectionAndCurrentBathroomTileBlockerSelectionLogic();

    //Resets for next update
    PerformCurrentlySelectedBroReset();
    PerformCurrentlySelectedJanitorReset();
    PerformCurrentlySelectedBathroomObjectReset();
    PerformCurrentlySelectedBathroomTileBlockerReset();
	}

	public void PerformCurrentBroSelectionAndCurrentBathroomObjectSelectionLogic() {
		if((currentlySelectedBroGameObject != null && currentlySelectedBroGameObject.GetComponent<Bro>() != null)
		   && (currentlySelectedBathroomObject != null && currentlySelectedBathroomObject.GetComponent<BathroomObject>() != null)) {

			BathroomObject bathObjRef = currentlySelectedBathroomObject.GetComponent<BathroomObject>();
      Bro broRef = currentlySelectedBroGameObject.GetComponent<Bro>();
			if(bathObjRef.state != BathroomObjectState.Broken
			   && bathObjRef.state != BathroomObjectState.BrokenByPee
			   && bathObjRef.state != BathroomObjectState.BrokenByPoop) {
				List<Vector2> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
				                                                                       AStarManager.Instance.GetListCopyOfAllClosedNodes(),
				                                                                       BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedBroGameObject.transform.position.x, currentlySelectedBroGameObject.transform.position.y, true).GetComponent<BathroomTile>(),
				                                                                       BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedBathroomObject.transform.position.x, currentlySelectedBathroomObject.transform.position.y, true).GetComponent<BathroomTile>());

        bool sendBroToObject = true;
        if(allowOnlyCorrectReliefTypeSelectionsForBros) {
          if(!(broRef.reliefRequired == ReliefRequired.Pee &&  (bathObjRef.type == BathroomObjectType.Urinal || bathObjRef.type == BathroomObjectType.Stall))
             && !(broRef.reliefRequired == ReliefRequired.Poop &&  (bathObjRef.type == BathroomObjectType.Stall))
             && !(broRef.reliefRequired == ReliefRequired.WashHands &&  (bathObjRef.type == BathroomObjectType.Sink))) {
            sendBroToObject = false;
          }
        }

        if(sendBroToObject) {
          if(broRef.targetObject != null
             && broRef.targetObject.GetComponent<BathroomObject>()) {
            //reset the target object to the default interaction state for all bros since this bro is already in a bathroom object
            broRef.targetObject.collider.enabled = true;
          }
  				broRef.SetTargetObjectAndTargetPosition(currentlySelectedBathroomObject, movementNodes);
  				broRef.selectableReference.isSelected = false;
  				broRef.selectableReference.ResetHighlightObjectAndSelectedState();

  				broRef.state = BroState.MovingToTargetObject;
  				EntranceQueueManager.Instance.RemoveBroFromEntranceQueues(currentlySelectedBroGameObject);

  				currentlySelectedBathroomObject.GetComponent<BathroomObject>().selectableReference.isSelected = false;
        }
        else {
          bathObjRef.selectableReference.isSelected = false;
          currentlySelectedBathroomObject = null;
        }
			}
			else {
				bathObjRef.selectableReference.isSelected = false;
				currentlySelectedBathroomObject = null;
			}
		}
	}


  public void PerformCurrentJanitorSelectionAndCurrentBathroomObjectSelectionLogic() {
    if((currentlySelectedJanitorGameObject != null && currentlySelectedJanitorGameObject.GetComponent<Janitor>() != null)
       && (currentlySelectedBathroomObject != null && currentlySelectedBathroomObject.GetComponent<BathroomObject>() != null)
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>().state != JanitorState.Entering
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>().state != JanitorState.Exiting) {

      BathroomObject bathObjRef = currentlySelectedBathroomObject.GetComponent<BathroomObject>();
      if(bathObjRef.state == BathroomObjectState.Broken
         || bathObjRef.state == BathroomObjectState.BrokenByPee
         || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
        List<Vector2> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                               new List<GameObject>(),
                                                                               BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedJanitorGameObject.transform.position.x, currentlySelectedJanitorGameObject.transform.position.y, true).GetComponent<BathroomTile>(),
                                                                               BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedBathroomObject.transform.position.x, currentlySelectedBathroomObject.transform.position.y, true).GetComponent<BathroomTile>());

        Janitor janitorRef = currentlySelectedJanitorGameObject.GetComponent<Janitor>();
        janitorRef.SetTargetObjectAndTargetPosition(currentlySelectedBathroomObject, movementNodes);
        janitorRef.selectableReference.isSelected = false;
        janitorRef.selectableReference.ResetHighlightObjectAndSelectedState();
        janitorRef.state = JanitorState.MovingToTargetObject;
        // EntranceQueueManager.Instance.RemoveBroFromEntanceQueues(currentlySelectedJanitorGameObject);
        // Debug.Log("Target object set");
        currentlySelectedBathroomObject.GetComponent<BathroomObject>().selectableReference.isSelected = false;
      }
      else {
        // Debug.Log("Target object NOT set");
        bathObjRef.selectableReference.isSelected = false;
        currentlySelectedBathroomObject = null;
      }
    }
  }
  public void PerformCurrentJanitorSelectionAndCurrentBathroomTileBlockerSelectionLogic() {
    if((currentlySelectedJanitorGameObject != null && currentlySelectedJanitorGameObject.GetComponent<Janitor>() != null)
       && (currentlySelectedBathroomTileBlocker != null && currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>() != null)
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>().state != JanitorState.Entering
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>().state != JanitorState.Exiting) {

      // BathroomTileBlocker bathroomTileBlockerReference = currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>();
      List<Vector2> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                             new List<GameObject>(),
                                                                             BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedJanitorGameObject.transform.position.x, currentlySelectedJanitorGameObject.transform.position.y, true).GetComponent<BathroomTile>(),
                                                                             BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(currentlySelectedBathroomTileBlocker.transform.position.x, currentlySelectedBathroomTileBlocker.transform.position.y, true).GetComponent<BathroomTile>());

      Janitor janitorRef = currentlySelectedJanitorGameObject.GetComponent<Janitor>();
      janitorRef.SetTargetObjectAndTargetPosition(currentlySelectedBathroomTileBlocker, movementNodes);
      janitorRef.selectableReference.isSelected = false;
      janitorRef.selectableReference.ResetHighlightObjectAndSelectedState();
      janitorRef.state = JanitorState.MovingToTargetObject;

      currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>().selectableReference.isSelected = false;
      currentlySelectedBathroomTileBlocker = null;
    }
    else {
      if(currentlySelectedBathroomTileBlocker != null) {
        currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>().selectableReference.isSelected = false;
        currentlySelectedBathroomTileBlocker = null;
      }
    }
  }

  //Resets Janitor reference to null if not selected
  public void PerformCurrentlySelectedJanitorReset() {
    if(currentlySelectedJanitorGameObject != null
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>() != null
       && currentlySelectedJanitorGameObject.GetComponent<Janitor>().selectableReference.isSelected == false) {
      currentlySelectedJanitorGameObject = null;
    }
  }

	//Resets bro reference to null if not selected
	public void PerformCurrentlySelectedBroReset() {
		if(currentlySelectedBroGameObject != null
		   && currentlySelectedBroGameObject.GetComponent<Bro>() != null
		   && currentlySelectedBroGameObject.GetComponent<Bro>().selectableReference.isSelected == false) {
			currentlySelectedBroGameObject = null;
		}
	}

	//Resets bathroom object reference to null if not selected
	public void PerformCurrentlySelectedBathroomObjectReset() {
		if(currentlySelectedBroGameObject == null
       && currentlySelectedJanitorGameObject == null) {
			currentlySelectedBathroomObject = null;
		}
		if(currentlySelectedBathroomObject != null
	 					&& currentlySelectedBathroomObject.GetComponent<BathroomObject>() != null
   					&& currentlySelectedBathroomObject.GetComponent<BathroomObject>().selectableReference.isSelected == false) {
			currentlySelectedBathroomObject = null;
		}
	}

  //Resets bro reference to null if not selected
  public void PerformCurrentlySelectedBathroomTileBlockerReset() {
    if(currentlySelectedBathroomTileBlocker != null
       && currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>() != null
       && currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>().selectableReference.isSelected == false) {
      currentlySelectedBathroomTileBlocker = null;
    }
    if(currentlySelectedJanitorGameObject == null
       && currentlySelectedBathroomTileBlocker != null) {
      currentlySelectedBathroomTileBlocker.GetComponent<BathroomTileBlocker>().selectableReference.isSelected = false;
      currentlySelectedBathroomTileBlocker = null;
    }
  }

  public void SelectBro(GameObject broToSelect) {
    // Debug.Log(broToSelect.name + " was selected.");
    SelectionManager.Instance.currentlySelectedBroGameObject = broToSelect;
    if(currentlySelectedJanitorGameObject != null) {
      currentlySelectedJanitorGameObject.GetComponent<Janitor>().selectableReference.isSelected = false;
    }
  }

  public void SelectJanitor(GameObject janitorToSelect) {
    SelectionManager.Instance.currentlySelectedJanitorGameObject = janitorToSelect;
    if(currentlySelectedBroGameObject != null) {
      currentlySelectedBroGameObject.GetComponent<Bro>().selectableReference.isSelected = false;
    }
  }
}
