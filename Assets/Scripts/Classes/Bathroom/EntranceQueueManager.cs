using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntranceQueueManager : MonoBehaviour {

  public bool brosWillSkipLineQueue = false;
  public bool brosSelectRandomTargetObjectOnEntrance = false;
  public bool brosSelectRandomTargetObjectAfterRelief = false;
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
	void Awake() {
		//There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
		//the script, which is assumedly attached to some GameObject. This in turn allows the instance
		//to be assigned when a game object is given this script in the scene view.
		//This also allows the pre-configured lazy instantiation to occur when the script is referenced from
		//another call to it, so that you don't need to worry if it exists or not.
		_instance = this;
	}
	//END OF SINGLETON CODE CONFIGURATION

	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.B)) {
			foreach(GameObject lineQueueObject in lineQueues) {
				lineQueueObject.GetComponent<LineQueue>().ReconfigureBrosInLineQueueTiles(true);
			}
		}
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

    return broToAdd;
  }

	public GameObject GenerateRandomBroInRandomEntranceQueue() {
		BroType broTypeSelected = Factory.Instance.SelectRandomBroType();
		int randomQueue = Random.Range(0, lineQueues.Count);
		return GenerateBroInEntranceQueueByType(broTypeSelected, randomQueue);
	}

	public GameObject GenerateSpecificBroInRandomEntranceQueue(BroType broTypeToGenerate) {
		int randomQueue = Random.Range(0, lineQueues.Count);
		return GenerateBroInEntranceQueueByType(broTypeToGenerate, randomQueue);
	}

	public GameObject GenerateRandomBroInEntranceQueue(int entranceQueueToGenerateIn) {
		BroType broTypeSelected = Factory.Instance.SelectRandomBroType();
		return GenerateBroInEntranceQueueByType(broTypeSelected, entranceQueueToGenerateIn);
	}

	public GameObject GenerateBroInEntranceQueueByType(BroType broTypeToGenerate, int entranceQueueToGenerateIn) {
		//Debug.Log(lineQueues.Count);
		LineQueue lineQueueSelected = lineQueues[entranceQueueToGenerateIn].GetComponent<LineQueue>();
		GameObject newBroGameObject = Factory.Instance.GenerateBroGameObject(broTypeToGenerate,
		                                                                       lineQueueSelected.queueTileObjects[lineQueueSelected.queueTileObjects.Count-1].transform.position);
		BroManager.Instance.AddBro(newBroGameObject);
		Bro broReference = newBroGameObject.GetComponent<Bro>();

    //-------------------------------------
    // Configure InAQueue Logic
    //-------------------------------------
    broReference.state = BroState.InAQueue;
    broReference.chooseRandomBathroomObjectOnSkipLineQueue = brosSelectRandomTargetObjectOnEntrance;
    broReference.chooseRandomBathroomObjectAfterRelieved = brosSelectRandomTargetObjectAfterRelief;
    broReference.lineQueueIn = lineQueues[entranceQueueToGenerateIn];
    broReference.skipLineQueue = brosWillSkipLineQueue;
    broReference.canBeCheckedToFightAgainst = false;

    List<Vector2> movementNodes = lineQueueSelected.GetQueueMovementNodes();

    broReference.SetTargetObjectAndTargetPosition(null, movementNodes);
    lineQueueSelected.AddGameObjectToLineQueue(newBroGameObject);

    broReference.PerformEnteredScore();
		broReference.speechBubbleReference.displaySpeechBubble = true;
		broReference.reliefRequired = Factory.Instance.SelectRandomReliefRequired();
    //-------------------------------------
    // Bro specific configurations
    //-------------------------------------
    // Debug.Log("Bro Class: " + broReference.GetType());
    if(broReference.GetType() == typeof(BluetoothBro)) {
    }
    else if(broReference.GetType() == typeof(ChattyBro)) {
    }
    else if(broReference.GetType() == typeof(CuttingBro)) {
      broReference.skipLineQueue = true;
    }
    else if(broReference.GetType() == typeof(DrunkBro)) {
      broReference.skipLineQueue = true;
      broReference.chooseRandomBathroomObjectOnSkipLineQueue = false;
      broReference.SetTargetObjectAndTargetPosition(null, lineQueueSelected.GetQueueMovementNodes());
      broReference.reliefRequired = ReliefRequired.Vomit;
      broReference.speechBubbleReference.displaySpeechBubble = false;
      //remove from line queue because it was added when skip line queue was false
      lineQueueSelected.RemoveGameObjectFromLineQueue(newBroGameObject);
    }
    else if(broReference.GetType() == typeof(FartingBro)) {
    }
		else if(broReference.GetType() == typeof(GenericBro)) {
    }
    else if(broReference.GetType() == typeof(RichBro)) {
    }
    else if(broReference.GetType() == typeof(ShyBro)) {
      broReference.reliefRequired = ReliefRequired.Pee;
    }
    else if(broReference.GetType() == typeof(SlobBro)) {
    }
    else if(broReference.GetType() == typeof(TimeWasterBro)) {
      broReference.SetTargetObjectAndTargetPosition(null, lineQueueSelected.GetQueueMovementNodes());
      broReference.speechBubbleReference.displaySpeechBubble = false;
		}
		else {
			Debug.Log("Unexpected bro generated.");
		}

		return newBroGameObject;
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
