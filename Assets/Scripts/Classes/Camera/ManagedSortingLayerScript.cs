using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagedSortingLayerScript : MonoBehaviour {

  public List<GameObject> gameObjectsToMatchSortingLayer = new List<GameObject>();
  public int sortingLayerOffset = 0;
  public bool dontPerformOwnSorting = false;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
    if(!dontPerformOwnSorting) {
      Vector3 cameraPosition = Camera.main.transform.position;
      Vector3 currentPosition = Vector3.zero;

      currentPosition = this.gameObject.transform.position;

      float xDifference = currentPosition.x - cameraPosition.x;
      // float xDifference = cameraPosition.x - currentPosition.x;

      float yDifference = currentPosition.y - cameraPosition.y;
      // float yDifference = cameraPosition.y - currentPosition.y;

      float distanceFromCamera = Mathf.Sqrt((xDifference * xDifference) + (yDifference * yDifference));

      int sortingLayerCalculation = Mathf.RoundToInt(distanceFromCamera * 100 * -1);

      // bool showDebugLogic = false;
      // if(this.gameObject.GetComponent<HighlightSelectable>() != null
      //    || this.gameObject.GetComponent<GenericBro>() != null) {
      // if(this.gameObject.name == "GenericBro1(Clone)"
      //    || this.gameObject.name == "HighlightBackground") {
      //   showDebugLogic =  true;
      // }
      // if(showDebugLogic) {
      //   Debug.Log("---------------------------------");
      //   Debug.Log("GameObject: " + this.gameObject.name);
      //   Debug.Log("currentPosition: " + currentPosition);
      //   Debug.Log("xDifference: " + xDifference);
      //   Debug.Log("yDifference: " + yDifference);
      //   Debug.Log("distanceFromCamera: " + distanceFromCamera);
      //   Debug.Log("sortingLayerCalculation: " + sortingLayerCalculation);
      // }

      // Debug.Log(sortingLayerCalculation);

      this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerCalculation  + sortingLayerOffset;
      foreach(GameObject objectToUpdate in gameObjectsToMatchSortingLayer) {
        objectToUpdate.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerCalculation + objectToUpdate.GetComponent<ManagedSortingLayerScript>().sortingLayerOffset;
      }
    }
	}
}
