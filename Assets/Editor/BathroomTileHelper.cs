using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BathroomTileHelper {
	[MenuItem ("Tools/Bathroom Tile/Enable BathroomTile %#[")]
	public static void EnableBathroomTile() {
		GameObject[] selections = Selection.gameObjects;
		foreach(GameObject selection in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
			GameObject bathroomTile = EditorHelper.GetFirstParentOfType<BathroomTile>(selection);
			if(bathroomTile != null) {
				ToggleSpriteRenderer(bathroomTile, true);
			}
			GameObject aStarNode = EditorHelper.GetFirstParentOfType<AStarNode>(selection);
			if(aStarNode != null) {
				ToggleAStarNode(aStarNode, false);
			}
		}
	}
	[MenuItem ("Tools/Bathroom Tile/Disable BathroomTile %#]")]
	public static void DisableBathroomTile() {
		GameObject[] selections = Selection.gameObjects;
		foreach(GameObject selection in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
			GameObject bathroomTile = EditorHelper.GetFirstParentOfType<BathroomTile>(selection);
			if(bathroomTile != null) {
				ToggleSpriteRenderer(bathroomTile, false);
			}
			GameObject aStarNode = EditorHelper.GetFirstParentOfType<AStarNode>(selection);
			if(aStarNode != null) {
				ToggleAStarNode(aStarNode, true);
			}
		}
	}
	public static void ToggleAStarNode(GameObject gameObjectToToggle, bool astarNodeState) {
		AStarNode[] astarNodes = gameObjectToToggle.GetComponentsInChildren<AStarNode>();
		foreach (AStarNode astarNode in astarNodes) {
			astarNode.isPermanentlyUntraversable = astarNodeState;
		}
	}
	public static void ToggleSpriteRenderer(GameObject gameObjectToToggle, bool spriteRendererState) {
		SpriteRenderer[] spriteRenderers = gameObjectToToggle.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
			spriteRenderer.enabled = spriteRendererState;
		}
	}

	public static void CreateNewColumn() {
		//Creates a new column at the lowest selected element, then increments each cell forward one
		//TODO: get all bathroom tile game objects
		//		then get the lowest cell
		//		create clone of that tile in place, and then move it forward
		//		then create all tiles in original x position
		//		then move all tiles right one and increment cell x by one
	}

	// Creates new row at the bottom of the lowest row, and duplicates the lowest row
	// THIS IS BROKEN - IT WONT WORK ON DISABLED OBJECTS
	public static void CreateNewRowColumn() {
		// Selection.gameObjects
        // MoveHelper.MoveGameObjects(Selection.gameObjects, 0, 1);
        // IncrementNumbersInName(Selection.gameObjects, 0, 1);

		GameObject[] selections = Selection.gameObjects;
		List<GameObject> bathroomTiles = new List<GameObject>();
		List<GameObject> rows = new List<GameObject>();
		foreach(GameObject selection in selections) {
			GameObject bathroomTile = EditorHelper.GetFirstParentOfType<BathroomTile>(selection);
			if(bathroomTile != null) {
				bathroomTiles.Add(bathroomTile);
				if(!rows.Contains(bathroomTile.transform.parent.gameObject)) {
					rows.Add(bathroomTile.transform.parent.gameObject);
				}
			}
		}

		Debug.Log("found bathroom tiles: " + bathroomTiles.Count);
		Debug.Log("found: " + rows.Count);

		// Gets the lowest row and then moves the rows up  in the y direction 1
		int lowestRow = 20000000;
		foreach(GameObject row in rows) {
			// Searches for the lowest row via gameobject name
			int currentRow = System.Convert.ToInt32(Regex.Match(row.name, @"\d+").Value);
			if(lowestRow > currentRow) {
				lowestRow = currentRow;
			}

			// Increments Row Name
			row.name = EditorHelper.GetIncrementedString(row.name);
			// Moves the game object up
			row.transform.position += new Vector3(0, 1, 0);
		}

		Debug.Log("Lowest Row Selected: " + lowestRow);
		// Increments bathroom tiles


		//Creates a new row at the lowest selected element, then increments each cell forward one
		//TODO: 
		//		then get the lowest cell
		//		create clone of that tile in place, and then move it forward
		//		then create all tiles in original x position
		//		then move all tiles right one and increment cell x by one

	}
}
