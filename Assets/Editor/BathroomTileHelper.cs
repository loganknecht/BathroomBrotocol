using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BathroomTileHelper {
    // TODO: Probably decouple these functions to be executed separately then executed in order so that
    //       this doesn't get bogged down with logic #whatever
    //       Also, you probably want to do sprite selection by the named game object instead of getting toggline
    //       all the sprite renderers that are children of the object
    /// <summary>
    /// Toggles the bathroom tile and its AStar Pathing state
    /// </summary>
    /// <param  name='enabled'>Whether it should be enabled or disabled</param>
    /// <param  name='toggleSpriteRenderer'>If the sprite renderer should be affected by the call to this. If false sprite render is not included in the logic</param>
    /// <param  name='toggleSpriteRenderer'>If the AStarNode script should be affected by the call to this. If false AStarNode script is not included in the logic</param>
    //--------------------------------------------------------------------------
    [MenuItem("Tools/Bathroom Tile/Enable BathroomTile %#[")]
    public static void EnableBathroomTile() {
        Debug.Log("--------------------");
        Debug.Log("Enable Bathroom Tile");
        Toggle<AStarNode>(true);
        Toggle<SpriteRenderer>(true);
    }
    [MenuItem("Tools/Bathroom Tile/Disable BathroomTile %#]")]
    public static void DisableBathroomTile() {
        Debug.Log("--------------------");
        Debug.Log("Disable Bathroom Tile");
        Toggle<AStarNode>(false);
        Toggle<SpriteRenderer>(false);
    }
    //--------------------------------------------------------------------------
    [MenuItem("Tools/Bathroom Tile/Enable BathroomTile Sprite %#;")]
    public static void EnableBathroomTileAStarNode() {
        Toggle<AStarNode>(true);
    }
    [MenuItem("Tools/Bathroom Tile/Enable BathroomTile Sprite %#'")]
    public static void DisableBathroomTileAStarNode() {
        Toggle<AStarNode>(false);
    }
    //--------------------------------------------------------------------------
    [MenuItem("Tools/Bathroom Tile/Enable BathroomTile Sprite %#.")]
    public static void EnableBathroomTileSprite() {
        Toggle<SpriteRenderer>(true);
    }
    [MenuItem("Tools/Bathroom Tile/Enable BathroomTile Sprite %#/")]
    public static void DisableBathroomTileSprite() {
        Toggle<SpriteRenderer>(false);
    }
    //--------------------------------------------------------------------------
    public static void Toggle<T>(bool enabled, string gameObjectNameToToggle = "") {
        // Debug.Log("toggling!!!");
        GameObject[] selections = Selection.gameObjects;
        // TODO: Make sure to perform this check on the root game object as well
        //       on account that it's probably just returning the children and
        //       not testing the parents at all
        // Debug.Log("Type: " + typeof(T));
        foreach(GameObject selection in selections) {
            T[] genericTypeObjects = selection.GetComponentsInChildren<T>();
            // Debug.Log("Number of type found: " + genericTypeObjects.Length);
            // this should be done with the where constraint or something but whatever
            foreach(T genericTypeObject in genericTypeObjects) {
                // Debug.Log("Object: " + (genericTypeObject as Component).gameObject.name);
                // Debug.Log("Name: " + (genericTypeObject as Component).gameObject.name);
                if((genericTypeObject as Component).gameObject.name == gameObjectNameToToggle
                    || gameObjectNameToToggle == "") {
                    // Debug.Log("ObjectType: " + genericTypeObject.GetType());
                    if(typeof(T) == typeof(AStarNode)) {
                        // Debug.Log("toggling astarnode!!!");
                        ToggleAStarNode(genericTypeObject as AStarNode, !enabled);
                    }
                    else if(typeof(T) == typeof(SpriteRenderer)) {
                        // Debug.Log("toggling sprite!!!");
                        ToggleSpriteRenderer(genericTypeObject as SpriteRenderer, !enabled);
                    }
                }
            }
        }
    }
    public static void ToggleBathroomTileAStarNode(bool enabled) {
        GameObject[] selections = Selection.gameObjects;
        foreach(GameObject selection in selections) {
            // Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
            GameObject aStarNode = EditorHelper.GetFirstParentOfType<AStarNode>(selection);
            if(aStarNode != null) {
            }
        }
    }
    
//--------------------------------------------------------------------------
    public static void ToggleAStarNode(AStarNode gameObjectToToggle, bool isPermanentlyUntraversable) {
        // AStarNode[] astarNodes = gameObjectToToggle.GetComponentsInChildren<AStarNode>();
        // foreach(AStarNode astarNode in astarNodes) {
        gameObjectToToggle.isPermanentlyUntraversable = !isPermanentlyUntraversable;
        // }
    }
    public static void ToggleSpriteRenderer(SpriteRenderer gameObjectToToggle, bool spriteRendererState) {
        // SpriteRenderer[] spriteRenderers = gameObjectToToggle.GetComponentsInChildren<SpriteRenderer>();
        // foreach(SpriteRenderer spriteRenderer in spriteRenderers) {
        gameObjectToToggle.enabled = spriteRendererState;
        // spriteRenderer.enabled = spriteRendererState;
        // }
    }
    
    public static void CreateNewColumn() {
        //Creates a new column at the lowest selected element, then increments each cell forward one
        //TODO: get all bathroom tile game objects
        //      then get the lowest cell
        //      create clone of that tile in place, and then move it forward
        //      then create all tiles in original x position
        //      then move all tiles right one and increment cell x by one
    }
    
// Creates new row at the bottom of the lowest row, and duplicates the lowest row
// THIS IS BROKEN - IT WONT WORK ON DISABLED OBJECTS
    public static void CreateNewRowColumn() {
        //Creates a new row at the lowest selected element, then increments each cell forward one
        //TODO:
        //      then get the lowest cell
        //      create clone of that tile in place, and then move it forward
        //      then create all tiles in original x position
        //      then move all tiles right one and increment cell x by one
        
    }
}
