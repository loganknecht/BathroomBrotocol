using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EntranceQueueManager : MonoBehaviour {

    public bool brosWillSkipLineQueue = false;
    public bool brosSelectRandomTargetObjectOnEntrance = false;
    public bool brosSelectRandomTargetObjectAfterRelief = false;
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

            PerformDebugButtonPressLogic();
        }
	}

    public void PerformDebugButtonPressLogic() {
        if(Input.GetKeyDown(KeyCode.Q)) {
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };

            BroDistributionObject firstWave = new BroDistributionObject(0, 0, 1, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
            firstWave.SetReliefType(BroDistribution.RandomBros, ReliefRequired.Pee, ReliefRequired.Poop);
            firstWave.SetFightProbability(BroDistribution.AllBros, 1f);
            firstWave.SetLineQueueSkipType(BroDistribution.AllBros, true);
            firstWave.SetChooseObjectOnLineSkip(BroDistribution.AllBros, true);
            firstWave.SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false);
            firstWave.SetChooseObjectOnRelief(BroDistribution.AllBros, true);

            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                                     firstWave,
                                                                                    }); 
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
