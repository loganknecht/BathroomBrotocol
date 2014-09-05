using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PuzzleWaveLogic : WaveLogic, WaveLogicContract {
  public bool startAnimationPerformed = false;
  public bool puzzleDecisionMade = false;

	// Use this for initialization
	public override void Start () {
    base.Start();

    startAnimationPerformed = true;
    puzzleDecisionMade = false;
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroEntered);
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.GenericBroEntered);
	}

	// Update is called once per frame
	public override void Update () {
    base.Update();

    if(startAnimationPerformed
       && !puzzleDecisionMade) {
      if(AllBrosHaveTargetObjects()) {
        Debug.Log("All bros have been selected!!!");
        puzzleDecisionMade = true;
        // If each bro doesn't have movement nodes to their target already,
        // then calculate their path to the target object selected
        foreach(GameObject gameObj in BroManager.Instance.allBros) {
          Bro broRef = gameObj.GetComponent<Bro>();
          if(broRef.movementNodes.Count == 0) {
            GameObject lineQueueGameObject = broRef.lineQueueIn;
            LineQueue lineQueueSelected = lineQueueGameObject.GetComponent<LineQueue>();
            int lineQueueTileIn = lineQueueSelected.GetLineQueueTileGameObjectIsIn(gameObj);
            // will return -1 if not in the tile, otherwise returns the number of the tile it's in
            if(lineQueueTileIn != -1) {
              // List<Vector2> movementNodes = lineQueueSelected.GetLineMovementNodes();
              List<Vector2> movementNodes = lineQueueSelected.GetLineMovementNodesBasedOnStartIndex(lineQueueTileIn);
              GameObject startTileGameObject = lineQueueSelected.queueTileObjects[0];
              BathroomTile startTile = startTileGameObject.GetComponent<BathroomTile>();
              GameObject targetObjectBathroomTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(broRef.targetObject.transform.position.x, broRef.targetObject.transform.position.y, true);
              BathroomTile targetTile = targetObjectBathroomTileGameObject.GetComponent<BathroomTile>();
              List<Vector2> pathFromStartTileToEndTile = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                                  AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                                  startTile,
                                                                                                  targetTile);
              movementNodes.RemoveAt(movementNodes.Count - 1);
              movementNodes.AddRange(pathFromStartTileToEndTile);
            }
          }
          broRef.performMovementLogic = true;
        }
      }
      // Not all bros have a target selected
      else {
        foreach(GameObject gameObj in BroManager.Instance.allBros) {
          gameObj.GetComponent<Bro>().performMovementLogic = false;
        }
      }
    }
	}

  public override void PerformWaveLogic() {
  }

  public bool AllBrosHaveTargetObjects() {
    bool foundBroWithoutTarget = false;
    foreach(GameObject gameObj in BroManager.Instance.allBros) {
      if(gameObj.GetComponent<Bro>() != null) {
        Bro broRef = gameObj.GetComponent<Bro>();
        if(broRef.targetObject == null) {
          foundBroWithoutTarget = true;
        }
      }
    }

    return !foundBroWithoutTarget;
  }
}
