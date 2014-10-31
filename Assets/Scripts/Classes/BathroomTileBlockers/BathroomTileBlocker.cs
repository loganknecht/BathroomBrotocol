using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileBlocker : MonoBehaviour {

	public BathroomTileBlockerType bathroomTileBlockerType = BathroomTileBlockerType.None;
	public float repairDuration = 0f;
  public Selectable selectableReference = null;
  public GameObject bathroomTileGameObjectIn = null;

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

  public void SetBathroomTileGameObjectIn(GameObject newBathroomTileGameObjectToResideIn) {
    if(bathroomTileGameObjectIn != null) {
      RemoveFromBathroomTileGameObjectIn();
    }
    
    BathroomTile bathroomTileToResideIn = newBathroomTileGameObjectToResideIn.GetComponent<BathroomTile>();
    if(bathroomTileToResideIn) {
      bathroomTileToResideIn.AddBathroomTileBlocker(this.gameObject);
      bathroomTileGameObjectIn = newBathroomTileGameObjectToResideIn;
    }
  }

  public void RemoveFromBathroomTileGameObjectIn() {
    if(bathroomTileGameObjectIn != null) {
      bathroomTileGameObjectIn.GetComponent<BathroomTile>().RemoveBathroomTileBlocker(this.gameObject);
    }
  }
}
