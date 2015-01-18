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
    /// <value>gameObjectsToOffsetFromAnchor - These are the game objects to offset for displaying, it should be either the sprite or a parent of the sprite</value>
    public List<GameObject> gameObjectsToOffsetFromAnchor;
    /// <value></value>
    public float isometricXOffset = 0f;
    /// <value></value>
    public float isometricYOffset = 0f;
    /// <value></value>
    public int tileMapLayer = 0;
    public float tileMapLayerHeight = 0.5f;
    public bool dontPerformOwnIsometricLogic = false;
    public bool hideUnderDiagonal = false;

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
        if(!dontPerformOwnIsometricLogic
            && gameObjectToAnchorTo == null) {
            Debug.LogError("For '" + this.gameObject.name + "' gameObjectToAnchorTo is NULL! Please set this value before calling game logic.");
        }

        if(gameObjectsToOffsetFromAnchor == null) {
            Debug.LogError("The gameObjectsToOffsetFromAnchor is null, it must be set to a game object");
        }
    }
    public void UpdateDisplayPosition() {
        if(!dontPerformOwnIsometricLogic) {
            // Use for images that should be of a 2 to 1 width to height ratio
            // float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
            // float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/2;
            
            // Use for images that should be of a 1 to 1 width to height ratio
            float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
            float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/4;

            Vector2 displayPosition = ConvertScreenToIsometricCoordinates(gameObjectToAnchorTo.transform.position.x, gameObjectToAnchorTo.transform.position.y, halfTileWidth, halfTileHeight);
            displayPosition.x += isometricXOffset;
            displayPosition.y += isometricYOffset;
            displayPosition.y += tileMapLayer * tileMapLayerHeight;

            foreach(GameObject gameObjectToOffsetFromAnchor in gameObjectsToOffsetFromAnchor) { 
                IsometricDisplay isometricReference = gameObjectToOffsetFromAnchor.GetComponent<IsometricDisplay>();

                Vector3 isometricOffset = Vector3.zero;

                if(isometricReference != null) {
                    isometricOffset.x += isometricReference.isometricXOffset;
                    
                    isometricOffset.y += isometricReference.isometricYOffset;
                    isometricOffset.y += (isometricReference.tileMapLayer * isometricReference.tileMapLayerHeight);
                }

                gameObjectToOffsetFromAnchor.transform.position = new Vector3((displayPosition.x + isometricOffset.x), (displayPosition.y + isometricOffset.y), gameObjectToOffsetFromAnchor.transform.position.z);
            }
        }
    }

    public Vector2 ConvertScreenToIsometricCoordinates(float mapX, float mapY, float halfTileWidth, float halfTileHeight) {
        Vector2 isometricVector = new Vector2((mapX - mapY) * halfTileWidth,
                                              (mapX + mapY) * halfTileHeight);

        return isometricVector;
    }

    public void ConvertIsometricToScreenCoordinates() {
        // screen.x = (map.x - map.y) * TILE_WIDTH_HALF;
        // screen.y = (map.x + map.y) * TILE_HEIGHT_HALF;
    }
}
