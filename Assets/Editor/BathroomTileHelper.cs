using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BathroomTileHelper {
	[MenuItem ("Tools/Bathroom Tile/Enable BathroomTile %#o")]
	public static void EnableBathroomTile() {
		GameObject[] selections = Selection.gameObjects;
		foreach(GameObject selection in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
			ToggleSpriteRenderer(EditorHelper.GetFirstParentOfType<BathroomTile>(selection), true);
			ToggleAStarNode(EditorHelper.GetFirstParentOfType<AStarNode>(selection), false);
		}
	}
	[MenuItem ("Tools/Bathroom Tile/Disable BathroomTile %#p")]
	public static void DisableBathroomTile() {
		GameObject[] selections = Selection.gameObjects;
		foreach(GameObject selection in selections) {
			// Debug.Log(EditorHelper.GetFirstParentOfType<BathroomTile>(selection));
			Debug.Log(EditorHelper.GetFirstParentOfType<AStarNode>(selection));
			ToggleSpriteRenderer(EditorHelper.GetFirstParentOfType<BathroomTile>(selection), false);
			ToggleAStarNode(EditorHelper.GetFirstParentOfType<AStarNode>(selection), true);
		}
	}
	public static void ToggleAStarNode(GameObject gameObjectToToggle, bool astarNodeState) {
		AStarNode[] astarNodes = gameObjectToToggle.GetComponentsInChildren<AStarNode>();
		foreach (AStarNode astarNode in astarNodes) {
			Debug.Log(astarNode);
			astarNode.isPermanentlyUntraversable = astarNodeState;
		}
	}
	public static void ToggleSpriteRenderer(GameObject gameObjectToToggle, bool spriteRendererState) {
		SpriteRenderer[] spriteRenderers = gameObjectToToggle.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
			spriteRenderer.enabled = spriteRendererState;
		}
	}
}
