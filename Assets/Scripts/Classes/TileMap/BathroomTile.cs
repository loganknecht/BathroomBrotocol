using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTile : Tile {
  	public GameObject bathroomObjectInTile = null;
  	public List<GameObject> bathroomTileBlockers;

	public override void Awake() {
		base.Awake();
	}

	// Use this for initialization
	public override void Start () {
		base.Start();

		if(bathroomTileBlockers != null) {
			bathroomTileBlockers = new List<GameObject>();
		}
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
}
