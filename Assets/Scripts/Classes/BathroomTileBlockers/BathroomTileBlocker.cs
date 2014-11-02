﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileBlocker : MonoBehaviour {

  public Animator animatorReference = null;
  public Selectable selectableReference = null;
  public Tappable tappableReference = null;

	public BathroomTileBlockerType bathroomTileBlockerType = BathroomTileBlockerType.None;
	// public float repairDuration = 0f;
  public GameObject bathroomTileGameObjectIn = null;
  public GameObject bathroomTileBlockerSpriteGameObject = null;

	// Use this for initialization
	public virtual void Start() {
    animatorReference = this.gameObject.GetComponent<Animator>();
    selectableReference = this.gameObject.GetComponent<Selectable>();
    tappableReference = this.gameObject.GetComponent<Tappable>();
	}

	// Update is called once per frame
	public virtual void Update() {
	}

  public virtual void OnMouseDown() {
    // SelectionManager.Instance.currentlySelectedBathroomTileBlocker = this.gameObject;
  }

  public virtual void UpdateAnimator() {
    Debug.Log("UpdateAnimator was called of the base class BathroomTileBlocker, in the gameObject: " + this.gameObject.name);
  }

  public void ResetColliderAndSelectableReference() {
    collider.enabled = true;
    selectableReference.isSelected = false;
    selectableReference.canBeSelected = true;
  }

  public void SetBathroomTileGameObjectIn(GameObject newBathroomTileGameObjectToResideIn) {
    Debug.Log("Setting bathroom tile game object in!");
    if(bathroomTileGameObjectIn != null) {
      RemoveFromBathroomTileGameObjectIn();
    }

    BathroomTile newBathroomTileToResideIn = newBathroomTileGameObjectToResideIn.GetComponent<BathroomTile>();
    if(newBathroomTileToResideIn) {
      newBathroomTileToResideIn.AddBathroomTileBlocker(this.gameObject);
      bathroomTileGameObjectIn = newBathroomTileGameObjectToResideIn;
    }
  }

  public void RemoveFromBathroomTileGameObjectIn() {
    if(bathroomTileGameObjectIn != null) {
      bathroomTileGameObjectIn.GetComponent<BathroomTile>().RemoveBathroomTileBlocker(this.gameObject);
    }
    bathroomTileGameObjectIn = null;
  }

  public void SelfDestruct() {
    BathroomTileBlockerManager.Instance.RemoveBathroomTileBlockerGameObject(this.gameObject, true);
    Destroy(this.gameObject);
  }
}
