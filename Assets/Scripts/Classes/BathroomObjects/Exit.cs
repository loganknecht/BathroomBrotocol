﻿using UnityEngine;
using System.Collections;

public class Exit : BathroomObject {
	// Use this for initialization
	public override void Start () {
		base.Start();
		type = BathroomObjectType.Exit;
	}

	// Update is called once per frame
	public override void Update () {
		base.Update();
	}


	public override void OnMouseDown() {
		base.OnMouseDown();
	}

	public override void UpdateAnimator() {
		base.UpdateAnimator();
	}
}
