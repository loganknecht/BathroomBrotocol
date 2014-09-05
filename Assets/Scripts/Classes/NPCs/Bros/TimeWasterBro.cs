using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeWasterBro : Bro {
  public int numberOfTapsUntilBounce = 5;
  public bool bouncePerformed = false;

  public bool hasEntered = false;

  public bool randomizeRoamingTime = false;
  public float timeInRoamingSpot = 0f;
  public float timeInRoamingSpotMax = 3f;
  public float minTimeInRoamingSpotMax = 2f;
  public float maxTimeInRoamingSpotMax = 4f;

	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BroType.TimeWasterBro;
    if(randomizeRoamingTime) {
      timeInRoamingSpotMax = Random.Range(minTimeInRoamingSpotMax, maxTimeInRoamingSpotMax);
    }
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

  // This is overriden so that the bro can have selectable logic enabled after the first movement
  // Tappable logic is enabled by setting hasEntered to true.
  public override void PopMovementNode() {
    if(movementNodes.Count > 0) {
      targetPosition = new Vector3(movementNodes[0].x, movementNodes[0].y, this.transform.position.z);
      movementNodes.RemoveAt(0);
      // Added to enable tappable logic
      if(movementNodes.Count <= 0
         && !hasEntered) {
        hasEntered = true;
      }
    }
  }

  public override void OnMouseDown() {
    // Don't call the base because selection manager doesn't need to select this
    // base.OnMouseDown();
    if(hasEntered) {
      numberOfTapsUntilBounce--;
    }
    PerformTapExitCheck();
  }

  public void PerformTapExitCheck() {
    if(numberOfTapsUntilBounce <= 0) {
      PerformBounceTriggeredLogic();
    }
  }

  public void PerformBounceTriggeredLogic() {
    if(!bouncePerformed) {
      bouncePerformed = true;
      GameObject exitSelectedGameObject = BathroomObjectManager.Instance.GetRandomBathroomObjectOfSpecificType(BathroomObjectType.Exit);
      if(exitSelectedGameObject != null) {
        BathroomTile broTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, false).GetComponent<BathroomTile>();
        BathroomTile exitTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(exitSelectedGameObject.transform.position.x, exitSelectedGameObject.transform.position.y, false).GetComponent<BathroomTile>();
        List<Vector2> bathroomEntranceToGameObjectMovementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                                           AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                                           broTile,
                                                                                                           exitTile);
        SetTargetObjectAndTargetPosition(exitSelectedGameObject, bathroomEntranceToGameObjectMovementNodes);
        state = BroState.MovingToTargetObject;
        speechBubbleReference.displaySpeechBubble = false;
      }
    }
  }

  public override void PerformRoamingLogic() {
    if(IsAtTargetPosition()) {
      if(timeInRoamingSpot > timeInRoamingSpotMax) {
        bool foundEmptyTile = false;
        GameObject randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
        while(!foundEmptyTile) {
          if(AStarManager.Instance.permanentClosedNodes.Contains(randomBathroomTile)) {
            randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
          }
          else {
            foundEmptyTile = true;
          }
        }

        // Debug.Log("Start Position X: " + this.gameObject.transform.position.x + " Y: " + this.gameObject.transform.position.y);

        BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
        List<Vector2> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                               new List<GameObject>(),
                                                                               startTile,
                                                                               randomBathroomTile.GetComponent<BathroomTile>());
        SetTargetObjectAndTargetPosition(null, movementNodes);

        timeInRoamingSpot = 0f;
        if(randomizeRoamingTime) {
          timeInRoamingSpotMax = Random.Range(minTimeInRoamingSpotMax, maxTimeInRoamingSpotMax);
        }
      }
      else {
        timeInRoamingSpot += Time.deltaTime;
      }
    }
  }

  //This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    // THIS IS NOT USED BECAUSE I DON'T KNOW WHY, HE JUST DOESN'T TARGET ANY OBJECTS, HE WASTES SPACE ETC ETC.
  }

	//--------------------------------------------------------
	public override void PerformEnteredScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.TimeWasterBroEntered);
	}
	public override void PerformRelievedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.TimeWasterBroRelieved);
	}
	public override void PerformWashedHandsScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.TimeWasterBroHandsWashed);
	}
	public override void PerformBroFightScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
	}
	public override void PerformExitedScore() {
		ScoreManager.Instance.IncrementScoreTracker(ScoreType.TimeWasterBroExited);
	}
}
