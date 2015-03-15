using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DuplicateHelper {
	public static void DuplicateAndIncrementNameAndMove(float xOffset, float yOffset) {
		List<GameObject> newGameObjects = new List<GameObject>();
		foreach(GameObject gameObj in Selection.gameObjects) {
			GameObject newGameObject = DuplicateAndIncrementGameObject(gameObj, GetIncrementedString(gameObj.name));
			newGameObject.transform.position = new Vector3(newGameObject.transform.position.x + xOffset,
														   newGameObject.transform.position.y + yOffset,
														   newGameObject.transform.position.z);
			newGameObjects.Add(newGameObject);

			Tile[] tiles = newGameObject.GetComponentsInChildren<Tile>(true);
			foreach(Tile tile in tiles) {
				tile.tileX += (int)Mathf.Floor(xOffset);
				tile.tileY += (int)Mathf.Floor(yOffset);
			}
		}
		Selection.objects = newGameObjects.ToArray();
	}
	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Up-Left %#u")]
	public static void DuplicateAndIncrementNameAndMoveUpLeft() {
		DuplicateAndIncrementNameAndMove(-1, 1);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Up %#i")]
	public static void DuplicateAndIncrementNameAndMoveUp() {
		DuplicateAndIncrementNameAndMove(0, 1);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Up-Right %#o")]
	public static void DuplicateAndIncrementNameAndMoveUpRight() {
		DuplicateAndIncrementNameAndMove(1, 1);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Left %#j")]
	public static void DuplicateAndIncrementNameAndMoveLeft() {
		DuplicateAndIncrementNameAndMove(-1, 0);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name %#k")]
	public static void DuplicateAndIncrementName() {
		DuplicateAndIncrementNameAndMove(0, 0);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Right %#l")]
	public static void DuplicateAndIncrementNameAndMoveRight() {
		DuplicateAndIncrementNameAndMove(1, 0);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Down-Left %#m")]
	public static void DuplicateAndIncrementNameAndMoveDownLeft() {
		DuplicateAndIncrementNameAndMove(-1, -1);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Down %#,")]
	public static void DuplicateAndIncrementNameAndMoveDown() {
		DuplicateAndIncrementNameAndMove(0, -1);
	}

	[MenuItem("Tools/Duplicate/Duplicate And Increment Name And Move Down-Right %#.")]
	public static void DuplicateAndIncrementNameAndMoveDownRight() {
		DuplicateAndIncrementNameAndMove(1, -1);
	}

	// This is a brilliant solution using lamda functions and regex, but it's not mine, it was taken from here:
	// http://stackoverflow.com/questions/15268931/increment-a-string-with-both-letters-and-numbers
	public static string GetIncrementedString(string stringToIncrement) {
		string newString = Regex.Replace(stringToIncrement, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
		return newString;
	}

	private static GameObject DuplicateAndIncrementGameObject(GameObject gameObjectToDuplicate, string newGameObjectName) {
		GameObject newGameObject = null;
		// Do not use this version of the duplication because it doesn't copy over all settings of the
		// item being duplicated from
		// Object prefabRoot = PrefabUtility.GetPrefabParent(gameObjectToDuplicate);
		// newGameObject = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
		if(newGameObject == null) {
			newGameObject = (GameObject)Object.Instantiate(gameObjectToDuplicate);
		}

		newGameObject.name = newGameObjectName;
		newGameObject.transform.parent = gameObjectToDuplicate.transform.parent;
		newGameObject.transform.position = gameObjectToDuplicate.transform.position;

		return newGameObject;
	}
}
