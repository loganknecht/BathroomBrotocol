using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineQueue : MonoBehaviour {

	public List<GameObject> queueObjects = new List<GameObject>();
	public List<GameObject> queueTileObjects = new List<GameObject>();

	public int maximumObjectsPerTile = 1;

	public float queueTileMinXRadiusForStanding = 0f;
	public float queueTileMaxXRadiusForStanding = 0f;
	public float queueTileMinYRadiusForStanding = 0f;
	public float queueTileMaxYRadiusForStanding = 0f;

	public bool shufflingAllowed = false;
	public bool shuffleTimerIsStatic = false;
	public float shuffleTimer = 0f;
	public float shuffleTimerLimit = 0f;
	public float shuffleTimerMin = 0f;
	public float shuffleTimerMax = 0f;

	public void Awake() {
	}

	public void Start() {
	}

	public void Update() {
		PerformShuffleLogic();
	}

	public void PerformShuffleLogic() {
		if(shufflingAllowed) {
			shuffleTimer += Time.deltaTime;
			if(shuffleTimer > shuffleTimerMax) {
				ReconfigureBrosInLineQueueTiles(false);
				shuffleTimer = 0;
				if(!shuffleTimerIsStatic) {
					shuffleTimerLimit = Random.Range(shuffleTimerMin, shuffleTimerMax);
				}

			}
		}
	}

	public void AddGameObjectToLineQueue(GameObject gameObjectToAdd) {
		//Add bro to  entrance queue
		//Entrance queue checks if the linequeue is full
		//if full the first bro in the line queue starts roaming the bathroom
			//count the difference of the size of the queue and the max allowed
			//then set the first n of them to have their target position that is the entrance queue first tie
				//also remove them from the entrance queues
			//set the bro to roaming mode and he should take care of the rest
		//if not full calculate the position the bro would be in and move him to that location
    if(gameObjectToAdd.GetComponent<Bro>().skipLineQueue) {
      // Don't even add bro to the line for tracking, just use it as a reference point
      // queueObjects.Add(gameObjectToAdd);
      gameObjectToAdd.GetComponent<Bro>().SetTargetObjectAndTargetPosition(null, GetQueueMovementNodes());
    }
    else {
      int maxNumberOfBros =  (queueTileObjects.Count * maximumObjectsPerTile);
      gameObjectToAdd.GetComponent<Bro>().lineQueueIn = this.gameObject;

      if(queueObjects.Count < maxNumberOfBros) {
        gameObjectToAdd.GetComponent<Bro>().SetTargetObjectAndTargetPosition(null, GetLineMovementNodesAccountForObjectsInLine());

        queueObjects.Add(gameObjectToAdd);
      }
      else {
        while(queueObjects.Count >= maxNumberOfBros) {
          GameObject broToRemove = queueObjects[0];
          Vector2 newTargetPosition = new Vector2(queueTileObjects[0].transform.position.x, queueTileObjects[0].transform.position.y);
          //Debug.Log(newTargetPosition);
          broToRemove.GetComponent<Bro>().SetTargetObjectAndTargetPosition(null, newTargetPosition);
          broToRemove.GetComponent<Bro>().state = BroState.Roaming;
          queueObjects.Remove(broToRemove);
        }
        queueObjects.Add(gameObjectToAdd);
        ReconfigureBrosInLineQueueTiles(true);
      }
    }
  }

public int GetLineQueueTileGameObjectIsIn(GameObject gameObjectToCheck) {
    int startTile = -1;

    for(int i = 0; i < queueTileObjects.Count; i++) {
      if(gameObjectToCheck.transform.position.x > gameObjectToCheck.transform.position.x + queueTileMinXRadiusForStanding
         && gameObjectToCheck.transform.position.x < gameObjectToCheck.transform.position.x + queueTileMaxXRadiusForStanding
         && gameObjectToCheck.transform.position.y > gameObjectToCheck.transform.position.y + queueTileMinYRadiusForStanding
         && gameObjectToCheck.transform.position.y < gameObjectToCheck.transform.position.y + queueTileMaxYRadiusForStanding) {
        startTile = i;
      }
    }

    return startTile;
  }

  public List<GameObject> GetLineMovementNodesBasedOnStartIndex(int startQueueTileIndex) {
    if(startQueueTileIndex < 0) {
      return new List<GameObject>();
    }
    else if(startQueueTileIndex > queueTileObjects.Count) {
      return new List<GameObject>();
    }
    else {
      List<GameObject> newMovementNodes = GetQueueMovementNodes();
      List<GameObject> movementNodesToReturn = new List<GameObject>();

      int i = 0;
      while(i < startQueueTileIndex) {
        movementNodesToReturn.Add(newMovementNodes[i]);
        i++;
      }

      movementNodesToReturn.Reverse();
      return movementNodesToReturn;
    }
  }

  public List<GameObject> GetLineMovementNodesAccountForObjectsInLine() {
    int currentTile = (queueObjects.Count/maximumObjectsPerTile);
    List<GameObject> newMovementNodes = new List<GameObject>();

    // Vector2 newTargetPosition = new Vector2(queueTileObjects[currentTile].transform.position.x, queueTileObjects[currentTile].transform.position.y);
    // newTargetPosition.x += Random.Range(queueTileMinXRadiusForStanding, queueTileMaxXRadiusForStanding);
    // newTargetPosition.y += Random.Range(queueTileMinYRadiusForStanding, queueTileMaxYRadiusForStanding);

    // newMovementNodes.Add(newTargetPosition);
    // while(currentTile < queueTileObjects.Count) {
    //   newMovementNodes.Add(new Vector2(queueTileObjects[currentTile].transform.position.x, queueTileObjects[currentTile].transform.position.y));
    //   currentTile++;
    // }

    // newMovementNodes.Reverse();

    return newMovementNodes;
  }

  public List<GameObject> GetQueueMovementNodes() {
    int currentTile = 0;
    List<GameObject> newMovementNodes = new List<GameObject>();

    // Vector2 newTargetPosition = new Vector2(queueTileObjects[currentTile].transform.position.x, queueTileObjects[currentTile].transform.position.y);
    // newTargetPosition.x += Random.Range(queueTileMinXRadiusForStanding, queueTileMaxXRadiusForStanding);
    // newTargetPosition.y += Random.Range(queueTileMinYRadiusForStanding, queueTileMaxYRadiusForStanding);

    // newMovementNodes.Add(newTargetPosition);
    // while(currentTile < queueTileObjects.Count) {
    //   newMovementNodes.Add(new Vector2(queueTileObjects[currentTile].transform.position.x, queueTileObjects[currentTile].transform.position.y));
    //   currentTile++;
    // }

    // newMovementNodes.Reverse();

    return newMovementNodes;
  }

	public void RemoveGameObjectFromLineQueue(GameObject gameObjectToRemove) {
		queueObjects.Remove(gameObjectToRemove);
		ReconfigureBrosInLineQueueTiles(false);
	}

	public void ReconfigureBrosInLineQueueTiles(bool forceReconfigure) {
		//Debug.Log("Reconfigure Line Objects Triggered");
		int currentTile = 0;
		int currentLineObject = 0;

		while(currentTile < queueTileObjects.Count) {
			int numberOfObjectsAdded = 0;
			while(numberOfObjectsAdded < maximumObjectsPerTile) {
				if(currentLineObject < queueObjects.Count) {
					Vector3 newTargetPosition = queueTileObjects[currentTile].transform.position;

					newTargetPosition.x += Random.Range(queueTileMinXRadiusForStanding, queueTileMaxXRadiusForStanding);
					newTargetPosition.y += Random.Range(queueTileMinYRadiusForStanding, queueTileMaxYRadiusForStanding);

					if(forceReconfigure) {
						queueObjects[currentLineObject].GetComponent<Bro>().SetTargetObjectAndTargetPosition(null, newTargetPosition);
					}
					else {
						if(queueObjects[currentLineObject] != null
						   && queueObjects[currentLineObject].GetComponent<Bro>() != null
						   && queueObjects[currentLineObject].GetComponent<Bro>().IsAtTargetPosition()
						   && queueObjects[currentLineObject].GetComponent<Bro>().targetObject == null) {
							//Debug.Log("Object Name: " + queueObjects[currentLineObject].name);
							queueObjects[currentLineObject].GetComponent<Bro>().SetTargetObjectAndTargetPosition(null, newTargetPosition);
						}
					}
				}
				numberOfObjectsAdded++;
				currentLineObject++;
			}
			currentTile++;
		}
	}

	public void ToggleQueueObjectColliders(bool newColliderState) {
		foreach(GameObject gameObj in queueObjects) {
			gameObj.collider.enabled = newColliderState;
		}
	}
}
