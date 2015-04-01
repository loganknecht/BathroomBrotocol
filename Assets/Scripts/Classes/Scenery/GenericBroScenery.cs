using UnityEngine;
using System.Collections;

public class GenericBroScenery : Scenery {
	public BroState state = BroState.None;

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}
	
	public override void UpdateAnimator() {
		base.UpdateAnimator();
		// facing.UpdateAnimatorWithFacing(animatorReference);
		// foreach (BroState broState in BroState.GetValues(typeof(BroState))) {
		// 	animatorReference.SetBool(broState.ToString(), false);
		// }
		// animatorReference.SetBool(state.ToString(), true);
		// If there were more states, do that enum
		// animatorReference.SetBool("Default", true);
	}
}
