using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class EditorHelper {
	public static GameObject GetFirstParentOfType<T>(GameObject gameObjectToGetRootFrom) {
		if(gameObjectToGetRootFrom.transform.parent == null) {
			return null;
		}
		else {
			if(gameObjectToGetRootFrom.GetComponent(typeof(T)) != null) {
				return gameObjectToGetRootFrom;
			}
			else {
				return GetFirstParentOfType<T>(gameObjectToGetRootFrom.transform.parent.gameObject);
			}
		}
	}

	// This is a brilliant solution using lamda functions and regex, but it's not mine, it was taken from here:
	// http://stackoverflow.com/questions/15268931/increment-a-string-with-both-letters-and-numbers
	public static string GetIncrementedString(string stringToIncrement) {
		string newString = Regex.Replace(stringToIncrement, "\\d+", m => (int.Parse(m.Value) + 1).ToString(new string('0', m.Value.Length)));
		return newString;
	}

	public static GameObject  GetBathroomGameObject(GameObject gameObject) {
		GameObject bathroomObjectToReturn = null;
		
		if (bathroomObjectToReturn == null) {
			// Debug.Log("Searching for BathroomTile");
			bathroomObjectToReturn = EditorHelper.GetFirstParentOfType<BathroomTile>(gameObject);
		}
		if (bathroomObjectToReturn == null) {
			// Debug.Log("Searching for BathroomObject");
			bathroomObjectToReturn = EditorHelper.GetFirstParentOfType<BathroomObject>(gameObject);
		}
		if (bathroomObjectToReturn == null) {
			// Debug.Log("Searching for Bro");
			bathroomObjectToReturn = EditorHelper.GetFirstParentOfType<Bro>(gameObject);
		}
		if (bathroomObjectToReturn == null) {
			// Debug.Log("Searching for Scenery");
			bathroomObjectToReturn = EditorHelper.GetFirstParentOfType<Scenery>(gameObject);
		}
		if(bathroomObjectToReturn == null) {
			bathroomObjectToReturn = gameObject;
		}

		return bathroomObjectToReturn;
	}
}
