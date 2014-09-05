using UnityEngine;
using System.Collections;

public class Vomit : BathroomTileBlocker {
	public override void Start() {
		base.Start();
		bathroomTileBlockerType = BathroomTileBlockerType.Vomit;
		repairDuration = 2f;
	}
//	public void OnMouseDown() {
//		Debug.Log("Clicked");
//	}
}
