using UnityEngine;
using System.Collections;

public class BathroomTile : Tile {
	public BathroomTile parentAStarNode = null;
	public float gValue = 0f;
	public float heuristicValue = 0f;
  	public bool isUntraversable = false;
  	public GameObject bathroomObjectInTile = null;

	public override void Awake() {
		base.Awake();
	}

	// Use this for initialization
	public override void Start () {
		base.Start();
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	public void ResetAStarValues() {
		parentAStarNode = null;
		gValue = 0f;
		heuristicValue = 0f;
	}
}
