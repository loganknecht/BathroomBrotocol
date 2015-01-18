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
    public SortingOrder sortingOrder = SortingOrder.TopToBottom;

    // Private
    IsometricDisplay gameObjectIsometricDisplay = null;
    int sortingMagnitudeModifier = 100;

    // Use this for initialization
    void Start () {
        if(gameObjectToBaseSortingOn == null) {
            Debug.LogError("The gameObject: '" + gameObject.name + "' does not have the 'gameObjectToBaseSortingOn' set to a value. Please set this to the GameObject that sorting will be based on.");
        }
        if(gameObjectIsometricDisplay == null) {
            gameObjectIsometricDisplay = this.gameObject.GetComponent<IsometricDisplay>();
        }
    }

    // Update is called once per frame
    void Update () {
        PerformSortingLogic();
    }

    public void PerformSortingLogic() {
        CalculateSortingLayer();
    }

    // The top right corner is the deepest and the bottom left corner is the front most
    public void CalculateSortingLayer() {
        if(sortingOrder == SortingOrder.TopToBottom) {
            PerformTopToBottomSorting();
        }
        if(sortingOrder == SortingOrder.TopRightToBottomLeft) {
            PerformTopRightToBottomLeftSorting();
        }
        else if(sortingOrder == SortingOrder.BottomToTop) {
            PerformBottomToTopSorting();
        }
        else if(sortingOrder == SortingOrder.BottomLeftToTopRight) {
            PerformBottomLeftToTopRightSorting();
        }

    }

    //-------------------------------------------------------------------------
    // This prioritizes hieght such that the greater y value draws on top
    public void PerformTopToBottomSorting() {
        float anchorSortingOrder = gameObjectToBaseSortingOn.transform.position.y;
        anchorSortingOrder += GetTopToBottomIsometricDisplayOffset(this.gameObject.GetComponent<IsometricDisplay>());
        anchorSortingOrder *= sortingMagnitudeModifier;
        anchorSortingOrder += sortingLayerOffset;

        float newSortingOrder = anchorSortingOrder;
        // Add isometric tile layer location to offset for height
        newSortingOrder += GetTopToBottomIsometricDisplayOffset(gameObjectToMatchSortingLayer.GetComponent<IsometricDisplay>()) * sortingMagnitudeModifier;
        // Add personal offset
        if(gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>() != null) {
            newSortingOrder += gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
        }

        gameObjectToMatchSortingLayer.GetComponent<SpriteRenderer>().sortingOrder = (int)newSortingOrder;
    }

    // This returns the offset value for an isometric display item is on a different tile map layer
    // It DOES NOT take into account the sortingMagnitudeModifier
    public float GetTopToBottomIsometricDisplayOffset(IsometricDisplay isometricDisplayReference) {
        float isometricDisplayOffset = 0;
        if(isometricDisplayReference != null) {
            isometricDisplayOffset += (isometricDisplayReference.tileMapLayerHeight * isometricDisplayReference.tileMapLayer);
        }

        return isometricDisplayOffset;
    }
    //-------------------------------------------------------------------------
    // This prioritizes hieght such that the greater y value and the lesser the absolute x value draws on top
    public void PerformTopRightToBottomLeftSorting() {
        float anchorSortingOrder = gameObjectToBaseSortingOn.transform.position.y;
        anchorSortingOrder += Mathf.Abs(gameObjectToBaseSortingOn.transform.position.x);
        anchorSortingOrder += GetTopRightToBottomLeftIsometricDisplayOffset(this.gameObject.GetComponent<IsometricDisplay>());
        anchorSortingOrder *= sortingMagnitudeModifier;
        anchorSortingOrder += sortingLayerOffset;

        float newSortingOrder = anchorSortingOrder;
        // Add isometric tile layer location to offset for height
        newSortingOrder += GetTopRightToBottomLeftIsometricDisplayOffset(gameObjectToMatchSortingLayer.GetComponent<IsometricDisplay>()) * sortingMagnitudeModifier;
        // Add personal offset
        if(gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>() != null) {
            newSortingOrder += gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
        }

        gameObjectToMatchSortingLayer.GetComponent<SpriteRenderer>().sortingOrder = (int)newSortingOrder;
    }

    // This returns the offset value for an isometric display item is on a different tile map layer
    // It DOES NOT take into account the sortingMagnitudeModifier
    public float GetTopRightToBottomLeftIsometricDisplayOffset(IsometricDisplay isometricDisplayReference) {
        float isometricDisplayOffset = 0;
        if(isometricDisplayReference != null) {
            isometricDisplayOffset += (isometricDisplayReference.tileMapLayerHeight * isometricDisplayReference.tileMapLayer);
        }

        return isometricDisplayOffset;
    }
    //-------------------------------------------------------------------------
    // This prioritizes hieght such that the lesser the y position is drawn on top
    public void PerformBottomToTopSorting() {
        float anchorSortingOrder = -gameObjectToBaseSortingOn.transform.position.y;
        anchorSortingOrder += GetBottomToTopIsometricDisplayOffset(this.gameObject.GetComponent<IsometricDisplay>());
        anchorSortingOrder *= sortingMagnitudeModifier;
        anchorSortingOrder += sortingLayerOffset;

        float newSortingOrder = anchorSortingOrder;
        // Add isometric tile layer location to offset for height
        newSortingOrder += GetBottomToTopIsometricDisplayOffset(gameObjectToMatchSortingLayer.GetComponent<IsometricDisplay>()) * sortingMagnitudeModifier;
        // Add personal offset
        if(gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>() != null) {
            newSortingOrder += gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
        }

        gameObjectToMatchSortingLayer.GetComponent<SpriteRenderer>().sortingOrder = (int)newSortingOrder;
    }

    // This returns the offset value for an isometric display item is on a different tile map layer
    // It DOES NOT take into account the sortingMagnitudeModifier
    public float GetBottomToTopIsometricDisplayOffset(IsometricDisplay isometricDisplayReference) {
        float isometricDisplayOffset = 0;
        if(isometricDisplayReference != null) {
            isometricDisplayOffset += (-isometricDisplayReference.tileMapLayerHeight * isometricDisplayReference.tileMapLayer);
        }

        return isometricDisplayOffset;
    }
    //-------------------------------------------------------------------------
    // This prioritizes hieght such that the lesser the y position and the lesser amount of the absolute x value is drawn on top
    public void PerformBottomLeftToTopRightSorting() {
        float anchorSortingOrder = -gameObjectToBaseSortingOn.transform.position.y;
        anchorSortingOrder -= Mathf.Abs(gameObjectToBaseSortingOn.transform.position.x);
        anchorSortingOrder += GetBottomLeftToTopRightIsometricDisplayOffset(this.gameObject.GetComponent<IsometricDisplay>());
        anchorSortingOrder *= sortingMagnitudeModifier;
        anchorSortingOrder += sortingLayerOffset;

        float newSortingOrder = anchorSortingOrder;
        // Add isometric tile layer location to offset for height
        newSortingOrder += GetBottomLeftToTopRightIsometricDisplayOffset(gameObjectToMatchSortingLayer.GetComponent<IsometricDisplay>()) * sortingMagnitudeModifier;
        // Add personal offset
        if(gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>() != null) {
            newSortingOrder += gameObjectToMatchSortingLayer.GetComponent<ManagedSortingLayer>().sortingLayerOffset;
        }

        gameObjectToMatchSortingLayer.GetComponent<SpriteRenderer>().sortingOrder = (int)newSortingOrder;
    }

    // This returns the offset value for an isometric display item is on a different tile map layer
    // It DOES NOT take into account the sortingMagnitudeModifier
    public float GetBottomLeftToTopRightIsometricDisplayOffset(IsometricDisplay isometricDisplayReference) {
        float isometricDisplayOffset = 0;
        if(isometricDisplayReference != null) {
            isometricDisplayOffset += (-isometricDisplayReference.tileMapLayerHeight * isometricDisplayReference.tileMapLayer);
        }

        return isometricDisplayOffset;
    }
}
