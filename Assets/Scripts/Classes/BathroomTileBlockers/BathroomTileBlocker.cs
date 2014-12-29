using UnityEngine;
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
        if(animatorReference == null) {
            animatorReference = this.gameObject.GetComponent<Animator>();
        }
        if(selectableReference == null) {
            selectableReference = this.gameObject.GetComponent<Selectable>();
        }
        if(tappableReference == null) {
            tappableReference = this.gameObject.GetComponent<Tappable>();
        }
    }

    // Update is called once per frame
    public virtual void Update() {
        if(tappableReference.tapLimitReached) {
            SelfDestruct();
        }
        PerformSpriteScaling(); 
    }

    public virtual void OnMouseDown() {
        // SelectionManager.Instance.currentlySelectedBathroomTileBlocker = this.gameObject;
    }

    public virtual void UpdateAnimator() {
        Debug.Log("UpdateAnimator was called of the base class BathroomTileBlocker, in the gameObject: " + this.gameObject.name);
    }


    public void PerformSpriteScaling() {
        if(bathroomTileBlockerSpriteGameObject != null) {
            float currentTapRatio = tappableReference.GetTapRatio();
            float currentSpriteScale = 1 - currentTapRatio;
            if(bathroomTileBlockerSpriteGameObject.transform.localScale.x != currentSpriteScale
               && bathroomTileBlockerSpriteGameObject.transform.localScale.y != currentSpriteScale) {
                bathroomTileBlockerSpriteGameObject.transform.localScale = new Vector3(currentSpriteScale, currentSpriteScale, bathroomTileBlockerSpriteGameObject.transform.localScale.z);
            }
        }
    }

    public void ResetColliderAndSelectableReference() {
        collider.enabled = true;
        selectableReference.isSelected = false;
        selectableReference.canBeSelected = true;
    }

    public void SetBathroomTileGameObjectIn(GameObject newBathroomTileGameObjectToResideIn) {
        // Debug.Log("Setting bathroom tile game object in!");
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

            // Checks to see if the bathroom tile has other bathroom tile blockers left in it so that it doesn't remove it 
            // from being blocked by another bathroom tile blocker. If nothing in it, then it's removed
            if(bathroomTileGameObjectIn.GetComponent<BathroomTile>().bathroomTileBlockers.Count == 0) {
                AStarManager.Instance.RemoveTemporaryClosedNode(bathroomTileGameObjectIn);
            }
        }
        bathroomTileGameObjectIn = null;
    }

    public void SelfDestruct() {
        BathroomTileBlockerManager.Instance.RemoveBathroomTileBlockerGameObject(this.gameObject);
        Destroy(this.gameObject);
    }
}
