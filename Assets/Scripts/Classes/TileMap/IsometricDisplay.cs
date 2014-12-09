using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// This is a class created to fix the isometric display of components in the world
/// Basically it's the view logic for tiles and game objects in the world.
/// The assumption here is that game objects have their own children game objects to manage for sprites.
/// With these children the position of the sprite is calculated from the game object that has this script attached and performs the correct calculation to set the game object to.
/// Math stolen from here: http://clintbellanger.net/articles/isometric_math/
/// TODO: Rework this to be like the "ManagedSortingLayer" script where each script attached to each object managed its own offset, but you can have it be called from a different object by adding it to a list using the same script

/// <summary>
/// This is a class to manage the isometric display of 2D elements within Unity
/// Because isometirc displays require offsetting components to look correctly, but the logic requires
/// a uniform data structure, the easiest way to reconcile this was creating a script that would
/// offset the game object that the sprites are attached to or children off. Then the movement logic
/// would be performed on a game object in the tile map, and that will be translated from for displaying
/// </summary>
public class IsometricDisplay : MonoBehaviour {
    /// <value>gameObjectToAnchorTo - This is the game object that all offsetting will be based off of</value>
    public GameObject gameObjectToAnchorTo;
    /// <value>gameObjectsToDisplay - These are the game objects to offset for displaying, it should be either the sprite or a parent of the sprite</value>
    public List<GameObject> gameObjectsToDisplay;
    /// <value></value>
    public float isometricXOffset = 0f;
    /// <value></value>
    public float isometricYOffset = 0f;

    // Use this for initialization
    void Start () {
        PerformInitialization();
        // Call this so it's position correctly when created
        UpdateDisplayPosition();
    }

    // Update is called once per frame
    void Update () {
        UpdateDisplayPosition();
    }

    void PerformInitialization() {
        // TODO: perform first awake methods
        if(gameObjectToAnchorTo == null) {
            Debug.LogError("For '" + this.gameObject.name + "' gameObjectToAnchorTo is NULL! Please set this value before calling game logic.");
        }

        if(gameObjectsToDisplay == null) {
            Debug.LogError("The gameObjectsToDisplay is null, it must be set to a game object");
        }
    }
    public void UpdateDisplayPosition() {
        // Use for images that should be of a 2 to 1 width to height ratio
        // float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
        // float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/2;
        
        // Use for images that should be of a 1 to 1 width to height ratio
        float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
        float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/4;

        Vector2 displayPosition = ConvertScreenToIsometricCoordinates(gameObjectToAnchorTo.transform.position.x, gameObjectToAnchorTo.transform.position.y, halfTileWidth, halfTileHeight);
        // Vector2 displayPosition = ConvertScreenToIsometricCoordinates((gameObjectToAnchorTo.transform.position.x + isometricXOffset), (gameObjectToAnchorTo.transform.position.y + isometricYOffset), halfTileWidth, halfTileHeight);

        foreach(GameObject gameObjectToDisplay in gameObjectsToDisplay) { 
            gameObjectToDisplay.transform.position = new Vector3((displayPosition.x + isometricXOffset), (displayPosition.y + isometricYOffset), gameObjectToDisplay.transform.position.z);
            // gameObjectToDisplay.transform.position = new Vector3(displayPosition.x, displayPosition.y, gameObjectToDisplay.transform.position.z);
            // Debug.Log("New Position: [" + gameObjectToDisplay.transform.position.x + ", " + gameObjectToDisplay.transform.position.y + "]");
        }
    }

    public Vector2 ConvertScreenToIsometricCoordinates(float screenX, float screenY, float halfTileWidth, float halfTileHeight) {
        // screen.x = (map.x - map.y) * TILE_WIDTH_HALF;
        // screen.y = (map.x + map.y) * TILE_HEIGHT_HALF;

        // Offset to the top of the screen
        Vector2 isometricVector = new Vector2((screenX - screenY) * halfTileWidth,
                                              (screenX + screenY) * halfTileHeight);

        return isometricVector;
    }

    public void ConvertIsometricToScreenCoordinates() {
        // screen.x = (map.x - map.y) * TILE_WIDTH_HALF;
        // screen.y = (map.x + map.y) * TILE_HEIGHT_HALF;
    }
}
