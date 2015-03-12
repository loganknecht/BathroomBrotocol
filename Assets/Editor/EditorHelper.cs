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
}
