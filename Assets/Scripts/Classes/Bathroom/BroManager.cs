using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BroManager : MonoBehaviour {

	public List<GameObject> allBros = new List<GameObject>();
  public List<GameObject> allFightingBros = new List<GameObject>();

	//BEGINNING OF SINGLETON CODE CONFIGURATION
	private static volatile BroManager _instance;
	private static object _lock = new object();

	//Stops the lock being created ahead of time if it's not necessary
	static BroManager() {
	}

	public static BroManager Instance {
		get {
			if(_instance == null) {
				lock(_lock) {
					if (_instance == null) {
						GameObject broManagerGameObject = new GameObject("BroManagerGameObject");
						_instance = (broManagerGameObject.AddComponent<BroManager>()).GetComponent<BroManager>();
					}
				}
			}
			return _instance;
		}
	}

	private BroManager() {
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
		SetAllBrosIsSelected(true);
	}

  public bool NoBrosInRestroom() {
    if(allBros.Count == 0) {
      return true;
    }
    else {
      return false;
    }
  }

	public void AddBro(GameObject broToAdd) {
		allBros.Add(broToAdd);
		broToAdd.transform.parent = this.gameObject.transform;
	}

	public void RemoveBro(GameObject broToRemove, bool destroyBro) {
		allBros.Remove(broToRemove);
		if(destroyBro) {
			Destroy(broToRemove);
		}
	}

  public void AddFightingBro(GameObject fightingBroToAdd) {
    allFightingBros.Add(fightingBroToAdd);
    fightingBroToAdd.transform.parent = this.gameObject.transform;
  }

  public void RemoveFightingBro(GameObject fightingBroToRemove, bool destroyFightingBro) {
    allFightingBros.Remove(fightingBroToRemove);
    if(destroyFightingBro) {
      Destroy(fightingBroToRemove);
    }
  }

	public void SetAllBrosIsSelected(bool ignoreCurrentlySelectedBro) {
		foreach(GameObject broObject in allBros) {
			if(ignoreCurrentlySelectedBro
			   && SelectionManager.Instance.currentlySelectedBroGameObject != null){
				if(broObject.GetInstanceID() != SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
					broObject.GetComponent<Bro>().selectableReference.ResetHighlightObjectAndSelectedState();
				}
			}
			else {
				broObject.GetComponent<Bro>().selectableReference.ResetHighlightObjectAndSelectedState();
			}
		}
	}

	public void SetAllBrosIsPaused(bool isPaused) {
		foreach(GameObject broObject in allBros) {
			broObject.GetComponent<Bro>().isPaused = isPaused;
		}
	}
}
