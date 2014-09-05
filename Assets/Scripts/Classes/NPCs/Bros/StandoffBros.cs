using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StandoffBros : MonoBehaviour {
  public GameObject broOne = null;
  public GameObject broTwo = null;

  public Vector2 standoffAnchor = Vector2.zero;

  public bool isContracting = false;
  public bool isExpanding = false;

  public Vector2 broOneRadiusPosition = Vector2.zero;
  public float broOneContractingXStepSize = 0;
  public float broOneContractingYStepSize = 0;
  public float broOneExpandingXStepSize = 0;
  public float broOneExpandingYStepSize = 0;

  public Vector2 broTwoRadiusPosition = Vector2.zero;
  public float broTwoContractingXStepSize = 0;
  public float broTwoContractingYStepSize = 0;
  public float broTwoExpandingXStepSize = 0;
  public float broTwoExpandingYStepSize = 0;

  public int numberOfTapsNeededToStop = 3;
  public int numberOfContractionsBeforeAFight = 5;
  public int contractingSteps = 25;
  public int expandingSteps = 100;

  public float xLockOnBuffer = 0.0005f;
  public float yLockOnBuffer = 0.0005f;

	// Use this for initialization
	void Start () {
    numberOfTapsNeededToStop = UnityEngine.Random.Range(3,6);
    PerformBroStandoffScore();
	}

	// Update is called once per frame
	void Update () {
    PerformStandoffBroLogic();
    if(numberOfTapsNeededToStop <= 0) {
      PlayerStoppedFight();
    }
    if(numberOfContractionsBeforeAFight <= 0) {
      PlayerDidNotStopFight();
    }
	}

  public void StandoffBrosInit(GameObject newBroOne, GameObject newBroTwo, Vector2 newStandoffAnchor) {
    // Debug.Log("new bro one z: " + newBroOne.transform.position.z);
    // Debug.Log("new bro two z: " + newBroTwo.transform.position.z);
    broOne = newBroOne;
    broTwo = newBroTwo;
    standoffAnchor = newStandoffAnchor;
    isContracting = true;
    isExpanding = false;

    broOne.collider.enabled = true;
    broOne.GetComponent<HighlightSelectable>().enabled = false;
    broTwo.collider.enabled = true;
    broTwo.GetComponent<HighlightSelectable>().enabled = false;

    //not doing expanding points, that will be set on first contraction
    broOneContractingXStepSize = (standoffAnchor.x - broOne.transform.position.x )/(20);
    broOneContractingYStepSize = (standoffAnchor.y - broOne.transform.position.y )/(20);

    broTwoContractingXStepSize = (standoffAnchor.x - broTwo.transform.position.x)/(20);
    broTwoContractingYStepSize = (standoffAnchor.y - broTwo.transform.position.y)/(20);
  }

  public void SetRadiusPositionsRandomly() {
    float directionTowardsAnchor = UnityEngine.Random.Range(0f, 2f);
    float radius = 0.75f;
    float broOneXPoint = (float)(Math.Cos(directionTowardsAnchor)*radius);
    float broOneYPoint = (float)(Math.Sin(directionTowardsAnchor)*radius);

    broOneRadiusPosition = new Vector2((standoffAnchor.x + broOneXPoint), (standoffAnchor.y + broOneYPoint));
    broTwoRadiusPosition = new Vector2((standoffAnchor.x - broOneXPoint), (standoffAnchor.y - broOneYPoint));

    CalculateStepSizes();
  }

  public void CalculateStepSizes() {
    broOneExpandingXStepSize = (standoffAnchor.x - broOneRadiusPosition.x )/expandingSteps;
    broOneExpandingYStepSize = (standoffAnchor.y  - broOneRadiusPosition.y )/expandingSteps;
    broOneContractingXStepSize = (standoffAnchor.x - broOneRadiusPosition.x )/contractingSteps;
    broOneContractingYStepSize = (standoffAnchor.y - broOneRadiusPosition.y )/contractingSteps;

    broTwoExpandingXStepSize = (standoffAnchor.x - broTwoRadiusPosition.x)/expandingSteps;
    broTwoExpandingYStepSize = (standoffAnchor.y - broTwoRadiusPosition.y)/expandingSteps;
    broTwoContractingXStepSize = (standoffAnchor.x - broTwoRadiusPosition.x)/contractingSteps;
    broTwoContractingYStepSize = (standoffAnchor.y - broTwoRadiusPosition.y)/contractingSteps;
  }

  public void PerformStandoffBroLogic() {
    // Debug.Log("bro one z: " + broOne.transform.position.z);
    // Debug.Log("bro two z: " + broTwo.transform.position.z);
    if(isExpanding) {
      // Debug.Log("Expanding");
      broOne.transform.position = new Vector3(broOne.transform.position.x - broOneExpandingXStepSize,
                                              broOne.transform.position.y - broOneExpandingYStepSize,
                                              broOne.transform.position.z);
      broTwo.transform.position = new Vector3(broTwo.transform.position.x - broTwoExpandingXStepSize,
                                              broTwo.transform.position.y - broTwoExpandingYStepSize,
                                              broTwo.transform.position.z);
      //if either bro arrives at their expanding point
      if((broOne.transform.position.x > (broOneRadiusPosition.x - xLockOnBuffer)
         && broOne.transform.position.x < (broOneRadiusPosition.x + xLockOnBuffer)
         && broOne.transform.position.y > (broOneRadiusPosition.y - yLockOnBuffer)
         && broOne.transform.position.y < (broOneRadiusPosition.y + yLockOnBuffer))
         || (broTwo.transform.position.x > (broTwoRadiusPosition.x - xLockOnBuffer)
         && broTwo.transform.position.x < (broTwoRadiusPosition.x + xLockOnBuffer)
         && broTwo.transform.position.y > (broTwoRadiusPosition.y - yLockOnBuffer)
         && broTwo.transform.position.y < (broTwoRadiusPosition.y + yLockOnBuffer))) {
        broOne.transform.position = new Vector3(broOneRadiusPosition.x, broOneRadiusPosition.y, broOne.gameObject.transform.position.z);
        broTwo.transform.position = new Vector3(broTwoRadiusPosition.x, broTwoRadiusPosition.y, broTwo.gameObject.transform.position.z);
        // broTwo.transform.position = broTwoRadiusPosition;
        isExpanding = false;
        isContracting = true;
      }
    }
    else if(isContracting) {
      // Debug.Log("Contracting");
      broOne.transform.position = new Vector3(broOne.transform.position.x + broOneContractingXStepSize,
                                              broOne.transform.position.y + broOneContractingYStepSize,
                                              broOne.transform.position.z);
      broTwo.transform.position = new Vector3(broTwo.transform.position.x + broTwoContractingXStepSize,
                                              broTwo.transform.position.y + broTwoContractingYStepSize,
                                              broTwo.transform.position.z);
      //if either bro arrived at their target position
      if((broOne.transform.position.x > (standoffAnchor.x - xLockOnBuffer)
         && broOne.transform.position.x < (standoffAnchor.x + xLockOnBuffer)
         && broOne.transform.position.y > (standoffAnchor.y - yLockOnBuffer)
         && broOne.transform.position.y < (standoffAnchor.y + yLockOnBuffer))
         || (broTwo.transform.position.x > (standoffAnchor.x - xLockOnBuffer)
         && broTwo.transform.position.x < (standoffAnchor.x + xLockOnBuffer)
         && broTwo.transform.position.y > (standoffAnchor.y - yLockOnBuffer)
         && broTwo.transform.position.y < (standoffAnchor.y + yLockOnBuffer))) {
        // Debug.Log("bro two arrived at standoff point");
        broOne.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, broOne.transform.position.z);
        broTwo.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, broTwo.transform.position.z);
        isExpanding = true;
        isContracting = false;
        numberOfContractionsBeforeAFight--;
        SetRadiusPositionsRandomly();
      }
    }

    else {
      Debug.Log("Wrong standoff logic");
    }
  }

  public void PlayerStoppedFight() {
    broOne.collider.enabled = true;
    broOne.GetComponent<Bro>().state = BroState.Roaming;
    broOne.GetComponent<Bro>().standOffBroGameObject = null;
    broOne.GetComponent<Bro>().broFightingWith = null;
    broOne.GetComponent<Bro>().canBeCheckedToFightAgainst = true;
    broOne.GetComponent<Bro>().speechBubbleReference.displaySpeechBubble = true;
    broOne.GetComponent<HighlightSelectable>().enabled = true;
    broOne.GetComponent<HighlightSelectable>().ResetHighlightObjectAndSelectedState();
    broOne.GetComponent<Bro>().ResetFightLogic();

    broTwo.collider.enabled = true;
    broTwo.GetComponent<Bro>().state = BroState.Roaming;
    broTwo.GetComponent<Bro>().standOffBroGameObject = null;
    broTwo.GetComponent<Bro>().broFightingWith = null;
    broTwo.GetComponent<Bro>().canBeCheckedToFightAgainst = true;
    broTwo.GetComponent<Bro>().speechBubbleReference.displaySpeechBubble = true;
    broTwo.GetComponent<HighlightSelectable>().enabled = true;
    broTwo.GetComponent<HighlightSelectable>().ResetHighlightObjectAndSelectedState();
    broTwo.GetComponent<Bro>().ResetFightLogic();

    // this.Destroy();
    Destroy(this.gameObject);
  }

  public void PlayerDidNotStopFight() {
    broOne.renderer.enabled = false;
    broOne.collider.enabled = false;
    broOne.SetActive(false);
    Bro broOneReference = broOne.GetComponent<Bro>();
    broOneReference.state = BroState.Fighting;
    broOneReference.selectableReference.ResetHighlightObjectAndSelectedState();

    broTwo.renderer.enabled = false;
    broTwo.collider.enabled = false;
    broTwo.SetActive(false);
    Bro broTwoReference = broTwo.GetComponent<Bro>();
    broTwoReference.state = BroState.Fighting;
    broTwoReference.selectableReference.ResetHighlightObjectAndSelectedState();

    GameObject newFightingBros = (GameObject)GameObject.Instantiate((Resources.Load("Prefabs/NPC/Bro/FightingBros") as GameObject));
    newFightingBros.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, newFightingBros.transform.position.z);
    newFightingBros.GetComponent<FightingBros>().brosFighting.Add(broOne);
    newFightingBros.GetComponent<FightingBros>().brosFighting.Add(broTwo);

    BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(newFightingBros.transform.position.x, newFightingBros.transform.position.y, true).GetComponent<BathroomTile>();
    BathroomTile targetTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
    newFightingBros.GetComponent<FightingBros>().SetTargetObjectAndTargetPosition(null,
                                                                                  AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                  AStarManager.Instance. GetListCopyOfAStarClosedNodes(),
                                                                                  startTile,
                                                                                  targetTile));

    Destroy(this.gameObject);
    PerformBroFightScore();
  }

  public void IncrementTapsFromPlayer() {
    numberOfTapsNeededToStop--;
  }

  public void PerformBroStandoffScore() {
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroStandoffOccurred);
  }
  public void PerformBroFightPreventedScore() {
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightPrevented);
  }
  public void PerformBroFightScore() {
    ScoreManager.Instance.IncrementScoreTracker(ScoreType.BroFightOccurred);
  }
}
