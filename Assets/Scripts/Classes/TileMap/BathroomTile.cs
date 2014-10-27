using UnityEngine;
using System.Collections;

public class BathroomTile : Tile {
  	public GameObject bathroomObjectInTile = null;
    public bool isUntraversable = false;

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
}
