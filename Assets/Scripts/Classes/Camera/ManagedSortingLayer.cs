using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagedSortingLayer : MonoBehaviour {

    public GameObject gameObjectToBaseSortingOn = null;
    public IsometricDisplay gameObjectIsometricDisplay = null;

    public List<GameObject> gameObjectsToMatchSortingLayer = new List<GameObject>();
    public int sortingLayerOffset = 0;
    int sortingMagnitudeModifier = 100;
    public bool dontPerformOwnSorting = false;
    public bool debug = false;
    public SortingOrder sortingOrder = SortingOrder.BackToFront;

    // Use this for initialization
    void Start () {
        if(!dontPerformOwnSorting
            && gameObjectToBaseSortingOn == null) {
            Debug.LogError("The gameObject: '" + gameObject.name + "' does not have the 'gameObjectToBaseSortingOn' set to a value. Please set this to the GameObject that sorting will be based on.");
        }
        // Debug.LogError("The IsometricDisplay reference 'gameObjectIsometricDisplay' is null for "  + gameObject.name + ". Is this intentional?");
    }

    // Update is called once per frame
    void Update () {
        PerformSortingLogic();
    }

    public void PerformSortingLogic() {
        if(!dontPerformOwnSorting) {
            // int sortingLayerCalculation = CalculateSortingLayerDistanceAwayFromCamera();
            int sortingLayerCalculation = CalculateSortingLayer();
            // this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerCalculation  + sortingLayerOffset;
            if(debug) {
                Debug.Log(this.gameObject.name);
                Debug.Log("sorting calculation: " + sortingLayerCalculation);
                Debug.Log("sorting offset: " + sortingLayerOffset);
            }
            foreach(GameObject objectToUpdate in gameObjectsToMatchSortingLayer) {
                objectToUpdate.GetComponent<SpriteRenderer>().sortingOrder = sortingLayerCalculation + sortingLayerOffset + objectToUpdate.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
            }
        }
    }

    // The top right corner is the deepest and the bottom left corner is the front most
    public int CalculateSortingLayer() {
        int sortingLayerCalculation = 0;
        if(sortingOrder == SortingOrder.BackToFront) {
            // sortingLayerCalculation = (int)(-this.gameObject.transform.position.y * sortingMagnitudeModifier) + sortingLayerOffset;
            sortingLayerCalculation = (int)(-gameObjectToBaseSortingOn.transform.position.y * sortingMagnitudeModifier) + sortingLayerOffset;
            if(gameObjectIsometricDisplay != null) {
                // This accounts for the offsets in tile map height displaying game objects
                sortingLayerCalculation += (int)((sortingMagnitudeModifier * gameObjectIsometricDisplay.tileMapLayerHeight) * gameObjectIsometricDisplay.tileMapLayer);
            }
        }
        else if(sortingOrder == SortingOrder.FrontToBack) {
            // sortingLayerCalculation = (int)(this.gameObject.transform.position.y * sortingMagnitudeModifier) + sortingLayerOffset;
            sortingLayerCalculation = (int)(gameObjectToBaseSortingOn.transform.position.y * sortingMagnitudeModifier) + sortingLayerOffset;
        }
        // Front To Back, Left To Right
        // sortingLayerCalculation = (int)((this.gameObject.transform.position.x * sortingMagnitudeModifier) + (this.gameObject.transform.position.y * 100)) + sortingLayerOffset;

        return sortingLayerCalculation;
    }

    // The farther you are away from the camera the more hidden you are
    public int CalculateSortingLayerDistanceAwayFromCamera() {
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

        return sortingLayerCalculation;
    }
}
