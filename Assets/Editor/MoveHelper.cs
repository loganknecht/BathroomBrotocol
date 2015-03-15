using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MoveHelper {
	public static void MoveSelections(float xOffset, float yOffset) {
		Debug.Log("Moving");
		GameObject[] selections = Selection.gameObjects;
		foreach (GameObject selection in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
			Debug.Log(EditorHelper.GetFirstParentOfType<BathroomObject>(selection));
			// Debug.Log(EditorHelper.GetFirstParentOfType<Scenery>(selection));

			GameObject objectToBeMoved = null;
			if (objectToBeMoved == null) {
				Debug.Log("Searching for BathroomTile");
				objectToBeMoved = EditorHelper.GetFirstParentOfType<BathroomTile>(selection);
			}
			if (objectToBeMoved == null) {
				Debug.Log("Searching for BathroomObject");
				objectToBeMoved = EditorHelper.GetFirstParentOfType<BathroomObject>(selection);
			}
			if (objectToBeMoved == null) {
				Debug.Log("Searching for Scenery");
				objectToBeMoved = EditorHelper.GetFirstParentOfType<Scenery>(selection);
			}

			Debug.Log("objectToBeMoved: " + objectToBeMoved);
			if (objectToBeMoved != null) {
				objectToBeMoved.transform.position = new Vector3(objectToBeMoved.transform.position.x + xOffset,
																 objectToBeMoved.transform.position.y + yOffset,
																 objectToBeMoved.transform.position.z) ;
			}
			// ToggleSpriteRenderer(EditorHelper.GetFirstParentOfType<BathroomTile>(selection), true);
			// ToggleAStarNode(EditorHelper.GetFirstParentOfType<AStarNode>(selection), false);
		}
	}

	[MenuItem ("Tools/Move Helper/Move Left %#LEFT")]
	public static void MoveSelectionsLeft() {
		MoveSelections(-1, 0);
	}

	[MenuItem ("Tools/Move Helper/Move Right %#RIGHT")]
	public static void MoveSelectionsRight() {
		MoveSelections(1, 0);
	}

	[MenuItem ("Tools/Move Helper/Move Up %#UP")]
	public static void MoveSelectionsUp() {
		MoveSelections(0, 1);
	}

	[MenuItem ("Tools/Move Helper/Move Down %#DOWN")]
	public static void MoveSelectionsDown() {
		MoveSelections(0, -1);
	}
}
