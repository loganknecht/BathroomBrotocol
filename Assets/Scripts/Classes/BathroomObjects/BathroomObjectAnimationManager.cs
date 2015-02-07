using UnityEngine;
using System.Collections;

public class BathroomObjectAnimationManager : MonoBehaviour {
	Animator animatorReference = null;
	BathroomFacing bathroomFacing = null;
	BathroomObject bathroomObjectReference = null;

	public void Awake() {
	}

	public void FixedUpdate() {
	}

	public void Update() {
		UpdateAnimatorReferenceExposedParameters();
	}

	public void Start() {
		animatorReference = this.gameObject.GetComponent<Animator>();
		bathroomFacing = this.gameObject.GetComponent<BathroomFacing>();
		bathroomObjectReference = this.gameObject.GetComponent<BathroomObject>();
	}

	public void UpdateAnimatorReferenceExposedParameters() {
		animatorReference.SetBool("None", false);

		animatorReference.SetBool(BathroomObjectState.BeingRepaired.ToString(), false);
		animatorReference.SetBool(BathroomObjectState.Broken.ToString(), false);
		animatorReference.SetBool(BathroomObjectState.BrokenByPee.ToString(), false);
		animatorReference.SetBool(BathroomObjectState.BrokenByPoop.ToString(), false);
		animatorReference.SetBool(BathroomObjectState.Idle.ToString(), false);
		animatorReference.SetBool(BathroomObjectState.InUse.ToString(), false);

		animatorReference.SetBool(DirectionBeingLookedAt.TopLeft.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.Top.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.TopRight.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.Left.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.Right.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.BottomLeft.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.Bottom.ToString(), false);
		animatorReference.SetBool(DirectionBeingLookedAt.BottomRight.ToString(), false);

		animatorReference.SetBool(bathroomFacing.directionBeingLookedAt.ToString(), true);
		animatorReference.SetBool(bathroomObjectReference.state.ToString(), true);
	}
}
