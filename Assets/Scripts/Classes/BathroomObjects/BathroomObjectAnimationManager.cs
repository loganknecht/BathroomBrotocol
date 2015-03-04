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

		animatorReference.SetBool(Facing.TopLeft.ToString(), false);
		animatorReference.SetBool(Facing.Top.ToString(), false);
		animatorReference.SetBool(Facing.TopRight.ToString(), false);
		animatorReference.SetBool(Facing.Left.ToString(), false);
		animatorReference.SetBool(Facing.Right.ToString(), false);
		animatorReference.SetBool(Facing.BottomLeft.ToString(), false);
		animatorReference.SetBool(Facing.Bottom.ToString(), false);
		animatorReference.SetBool(Facing.BottomRight.ToString(), false);

		animatorReference.SetBool(bathroomFacing.facing.ToString(), true);
		animatorReference.SetBool(bathroomObjectReference.state.ToString(), true);
	}
}
