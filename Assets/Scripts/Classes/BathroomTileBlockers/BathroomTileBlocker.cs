using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileBlocker : MonoBehaviour {

	public BathroomTileBlockerType bathroomTileBlockerType = BathroomTileBlockerType.None;
	public float repairDuration = 0f;
  public Selectable selectableReference = null;

	// Use this for initialization
	public virtual void Start() {
    selectableReference = this.gameObject.GetComponent<Selectable>();
	}

	// Update is called once per frame
	public virtual void Update() {

	}

  public virtual void OnMouseDown() {
    SelectionManager.Instance.currentlySelectedBathroomTileBlocker = this.gameObject;
  }

  public void ResetColliderAndSelectableReference() {
    collider.enabled = true;
    selectableReference.isSelected = false;
    selectableReference.canBeSelected = true;
  }
}
