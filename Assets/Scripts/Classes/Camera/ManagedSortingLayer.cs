using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// sorting order is for the greater number puts it in front
// example: 10 < 30 so the object with a sorting order of 30 is drawn on top
public class ManagedSortingLayer : MonoBehaviour {

    // Public
    public GameObject gameObjectToBaseSortingOn = null;
    public GameObject gameObjectToMatchSortingLayer;
    public int sortingLayerOffset = 0;
    public bool debug = false;
    public SortingOrder sortingOrder = SortingOrder.BottomToTop;
    
    // Private
    IsometricDisplay gameObjectIsometricDisplay = null;
    int sortingMagnitudeModifier = 100;
    
    // Use this for initialization
    void Start() {
        if(gameObjectToBaseSortingOn == null) {
            Debug.LogError("The gameObject: '" + gameObject.name + "' does not have the 'gameObjectToBaseSortingOn' set to a value. Please set this to the GameObject that sorting will be based on.");
        }
        if(gameObjectIsometricDisplay == null) {
            gameObjectIsometricDisplay = this.gameObject.GetComponent<IsometricDisplay>();
        }
    }
    
    // Update is called once per frame
    void Update() {
        SortingLogic();
    }
    
    public void SortingLogic() {
        CalculateSortingLayer();
    }
    
    // The top right corner is the deepest and the bottom left corner is the front most
    public void CalculateSortingLayer() {
        if(sortingOrder == SortingOrder.BottomToTop) {
            BottomToTopSorting();
        }
    }
    
    //-------------------------------------------------------------------------
    // This prioritizes hieght such that the lesser the y position is drawn on top
    public void BottomToTopSorting() {
        float newSortingOrder = -gameObjectToBaseSortingOn.transform.position.y;
        newSortingOrder -= gameObjectToBaseSortingOn.transform.position.x;
        newSortingOrder *= sortingMagnitudeModifier;
        
        // Add personal offset
        if(gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>() != null) {
            newSortingOrder += gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
        }
        
        gameObjectToMatchSortingLayer.GetComponent<SpriteRenderer>().sortingOrder = (int)newSortingOrder;
    }
}
