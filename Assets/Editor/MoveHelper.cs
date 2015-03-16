using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class MoveHelper {
	public static void MoveGameObjects(GameObject[] selections, float xOffset, float yOffset) {
		// Debug.Log("Moving");
		foreach (GameObject gameObject in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(gameObject));
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomObject>(gameObject));
			// Debug.Log(EditorHelper.GetFirstParentOfType<Scenery>(gameObject));
			GameObject objectToBeMoved = EditorHelper.GetBathroomGameObject(gameObject);

			// defaults to the object
			if(objectToBeMoved == null) {
				objectToBeMoved = gameObject;
			}

			// Debug.Log("objectToBeMoved: " + objectToBeMoved);
			if (objectToBeMoved != null) {
				objectToBeMoved.transform.position = new Vector3(objectToBeMoved.transform.position.x + xOffset,
																 objectToBeMoved.transform.position.y + yOffset,
																 objectToBeMoved.transform.position.z) ;
			}

			// BathroomTile bathroomTileRef = objectToBeMoved.GetComponent<BathroomTile>();
			// if(objectToBeMoved.GetComponent<BathroomTile>() != null) {
			// 	bathroomTileRef.tileX += (int)xOffset;
			// 	bathroomTileRef.tileY += (int)yOffset;
			// }
		}
	}

	[MenuItem ("Tools/Move Helper/Move Left %#LEFT")]
	public static void MoveGameObjectsLeft() {
		MoveGameObjects(Selection.gameObjects, -1, 0);
	}

	[MenuItem ("Tools/Move Helper/Move Right %#RIGHT")]
	public static void MoveGameObjectsRight() {
		MoveGameObjects(Selection.gameObjects, 1, 0);
	}

	[MenuItem ("Tools/Move Helper/Move Up %#UP")]
	public static void MoveGameObjectsUp() {
		MoveGameObjects(Selection.gameObjects, 0, 1);
	}

	[MenuItem ("Tools/Move Helper/Move Down %#DOWN")]
	public static void MoveGameObjectsDown() {
		MoveGameObjects(Selection.gameObjects, 0, -1);
	}
}
