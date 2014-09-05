using UnityEngine;
using System.Collections;

public class LevelOneObjectiveOne : Objective {

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

  public override bool IsObjectiveSatisfied() {
    int numberOfBrokenBathroomObjects = 0;
    foreach(GameObject gameObj in BathroomObjectManager.Instance.allBathroomObjects) {
      if(gameObj.GetComponent<BathroomObject>().state == BathroomObjectState.Broken
         || gameObj.GetComponent<BathroomObject>().state == BathroomObjectState.BrokenByPee
         || gameObj.GetComponent<BathroomObject>().state == BathroomObjectState.BrokenByPoop) {
        numberOfBrokenBathroomObjects++;
      }
    }

    if(numberOfBrokenBathroomObjects > 0) {
      return false;
    }
    else {
      return true;
    }
  }
}
