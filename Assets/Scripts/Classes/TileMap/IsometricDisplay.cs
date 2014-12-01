using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// This is a class created to fix the isometric display of components in the world
/// Basically it's the view logic for tiles and game objects in the world.
/// The assumption here is that game objects have their own children game objects to manage for sprites.
/// With these children the position of the sprite is calculated from the game object that has this script attached and performs the correct calculation to set the game object to.
/// Math stolen from here: http://clintbellanger.net/articles/isometric_math/
public class IsometricDisplay : MonoBehaviour {

    public List<GameObject> gameObjectsToDisplay;
    public float isometricXOffset = 0f;
    public float isometricYOffset = 0f;

    // Use this for initialization
    void Start () {
        if(gameObjectsToDisplay == null) {
            Debug.LogError("The gameObjectsToDisplay is null, it must be set to a game object");
        }
    }
    // Update is called once per frame
    void Update () {
        UpdateDisplayPosition();
    }

    void PerformInitialization() {
        // TODO: perform first awake methods
    }
    public void UpdateDisplayPosition() {
        // float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
        // float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/2;
        float halfTileWidth = BathroomTileMap.Instance.singleTileWidth/2;
        float halfTileHeight = BathroomTileMap.Instance.singleTileHeight/4;
        Vector2 displayPosition = ConvertScreenToIsometricCoordinates(this.gameObject.transform.position.x, this.gameObject.transform.position.y, halfTileWidth, halfTileHeight);
        foreach(GameObject gameObjectToDisplay in gameObjectsToDisplay) { 
            gameObjectToDisplay.transform.position = new Vector3((displayPosition.x + isometricXOffset), (displayPosition.y + isometricYOffset), gameObjectToDisplay.transform.position.z);
            // Debug.Log("New Position: [" + gameObjectToDisplay.transform.position.x + ", " + gameObjectToDisplay.transform.position.y + "]");
        }
    }

    public Vector2 ConvertScreenToIsometricCoordinates(float screenX, float screenY, float halfTileWidth, float halfTileHeight) {
        // screen.x = (map.x - map.y) * TILE_WIDTH_HALF;
        // screen.y = (map.x + map.y) * TILE_HEIGHT_HALF;
        Vector2 isometricVector = new Vector2((screenX - screenY) * halfTileWidth,
                                              (screenX + screenY) * halfTileHeight);
        return isometricVector;
    }

    public void ConvertIsometricToScreenCoordinates() {
        // screen.x = (map.x - map.y) * TILE_WIDTH_HALF;
        // screen.y = (map.x + map.y) * TILE_HEIGHT_HALF;
    }
}
