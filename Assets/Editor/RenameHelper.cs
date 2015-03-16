using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public static class RenameHelper {
    public static void IncrementNumbersInName(GameObject[] gameObjects) {
        foreach(GameObject gameObject in gameObjects) {
            IncrementNumbersInName(gameObject);
        }
    }
    public static void IncrementNumbersInName(GameObject gameObject) {
        gameObject.name = EditorHelper.GetIncrementedString(gameObject.name);
    }
}