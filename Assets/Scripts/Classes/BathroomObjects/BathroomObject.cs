using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomObject : MonoBehaviour {
    public BathroomObjectType type = BathroomObjectType.None;
    public BathroomObjectState state = BathroomObjectState.None;
    public bool destroyObjectIfMoreThanTwoOccupants = true;

    Animator animatorReference = null;
    BathroomFacing bathroomFacingReference;

    public Selectable selectableReference = null;

    //public bool isBroken = false;
    public float occupationDuration = 2.5f;
    public float repairDuration = 2.0f;

    public int scoreValue = 0;

    public bool markOutOfOrderWhenOverUsed = false;
    public int timesUsed = 0;
    public int timesUsedNeededForOutOfOrder = 10;
    public int numberOfTaps = 0;
    public int numberOfTapsNeededToRestoreToOrder = 5;

    // This gets set in the bathroom tile manager singleton
    public GameObject bathroomTileIn = null;
    public List<GameObject> objectsOccupyingBathroomObject = new List<GameObject>();

    public virtual void Start() {
        animatorReference = this.gameObject.GetComponent<Animator>();
        bathroomFacingReference = this.gameObject.GetComponent<BathroomFacing>();

        selectableReference = this.gameObject.GetComponent<Selectable>();
        selectableReference.canBeSelected = true;
    }

	public virtual void Update() {
		UpdateBathroomObjectAnimator();
    PerformMoreThanTwoOccupantsCheck();
	}

	public virtual void OnMouseDown() {
		SelectionManager.Instance.currentlySelectedBathroomObject = this.gameObject;

    if(state == BathroomObjectState.OutOfOrder) {
      numberOfTaps++;
      if(numberOfTaps >= numberOfTapsNeededToRestoreToOrder) {
        state = BathroomObjectState.Idle;
        timesUsed = 0;
      }
    }
    // Debug.Log("Derp down");
	}

	public virtual void UpdateBathroomObjectAnimator() {
    if(animatorReference != null) {
  		//animatorReference.SetBool(DirectionBeingLookedAt.None.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.TopLeft.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.Top.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.TopRight.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.Left.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.Right.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.BottomLeft.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.Bottom.ToString(), false);
  		animatorReference.SetBool(DirectionBeingLookedAt.BottomRight.ToString(), false);

  		animatorReference.SetBool(BathroomObjectState.BeingRepaired.ToString(), false);
  		animatorReference.SetBool(BathroomObjectState.Broken.ToString(), false);
  		animatorReference.SetBool(BathroomObjectState.BrokenByPoop.ToString(), false);
  		animatorReference.SetBool(BathroomObjectState.Idle.ToString(), false);
  		animatorReference.SetBool(BathroomObjectState.InUse.ToString(), false);
      animatorReference.SetBool(BathroomObjectState.OutOfOrder.ToString(), false);

  		animatorReference.SetBool(bathroomFacingReference.directionBeingLookedAt.ToString(), true);
  		animatorReference.SetBool(state.ToString(), true);

  		animatorReference.SetBool("None", false);
    }
	}

  public void ResetColliderAndSelectableReference() {
    collider.enabled = true;
    selectableReference.isSelected = false;
    selectableReference.canBeSelected = true;
  }

    public void AddBro(GameObject broGameObjectToAdd) {
        objectsOccupyingBathroomObject.Add(broGameObjectToAdd);
    }

    public void RemoveBro(GameObject broGameObjectToRemove) {
        objectsOccupyingBathroomObject.Remove(broGameObjectToRemove);
    }

    public void RemoveBroAndIncrementUsedCount(GameObject broGameObjectToRemove) {
        objectsOccupyingBathroomObject.Remove(broGameObjectToRemove);

        IncrementTimesUsed();
    }

    public void IncrementTimesUsed() {
        timesUsed++;
        PerformOutOfOrderCheck();
    }

    public bool IsBroken() {
        if(state == BathroomObjectState.Broken
            || state == BathroomObjectState.BrokenByPee
            || state == BathroomObjectState.BrokenByPoop) {
            return true;
        }
        else {
            return false;
        }
    }

    public void PerformOutOfOrderCheck() {
        if(markOutOfOrderWhenOverUsed) {
            if(timesUsed >= timesUsedNeededForOutOfOrder) {
                if(!IsBroken()) {
                    numberOfTaps = 0;
                    state = BathroomObjectState.OutOfOrder;
                }
            }
        }
    }

  public void TriggerOutOfOrderState() {
  }

  public void PerformMoreThanTwoOccupantsCheck() {
    if(objectsOccupyingBathroomObject.Count >= 2
       && destroyObjectIfMoreThanTwoOccupants
       && type != BathroomObjectType.Exit) { 
      GameObject firstBroFound = null;
      GameObject secondBroFound = null;
      List<GameObject> objectOccupyingBathroomObjectToRemove = new List<GameObject>();
      for(int i = 0; i < objectsOccupyingBathroomObject.Count; i++) {
        GameObject gameObj = objectsOccupyingBathroomObject[i];

        if(gameObj.GetComponent<Bro>() != null) {
          if(firstBroFound == null) {
            firstBroFound = gameObj;
          }
          else {
            secondBroFound = gameObj;
          }
        }
        // TODO!! - Change this logic to send him to the exit.
        if(i == objectsOccupyingBathroomObject.Count - 1) {
          if(firstBroFound == null
             && secondBroFound == null) {
            Bro broRef = gameObj.GetComponent<Bro>();
            broRef.state = BroState.Roaming;
            broRef.selectableReference.ResetHighlightObjectAndSelectedState();
            broRef.SetRandomOpenBathroomObjectTarget(BathroomObjectType.Exit);
          }
        }
        if(firstBroFound != null
           && secondBroFound != null) {
          state = BathroomObjectState.Broken;

          firstBroFound.renderer.enabled = false;
          firstBroFound.collider.enabled = false;
          firstBroFound.SetActive(false);
          Bro firstBroFoundReference = firstBroFound.GetComponent<Bro>();
          firstBroFoundReference.state = BroState.Fighting;
          firstBroFoundReference.selectableReference.ResetHighlightObjectAndSelectedState();

          secondBroFound.renderer.enabled = false;
          secondBroFound.collider.enabled = false;
          secondBroFound.SetActive(false);
          Bro secondBroFoundReference = secondBroFound.GetComponent<Bro>();
          secondBroFoundReference.state = BroState.Fighting;
          secondBroFoundReference.selectableReference.ResetHighlightObjectAndSelectedState();

          GameObject newFightingBros = (GameObject)GameObject.Instantiate((Resources.Load("Prefabs/NPC/Bro/FightingBros") as GameObject));
          newFightingBros.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newFightingBros.transform.position.z);
          newFightingBros.GetComponent<FightingBros>().brosFighting.Add(firstBroFound);
          newFightingBros.GetComponent<FightingBros>().brosFighting.Add(secondBroFound);

          BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(newFightingBros.transform.position.x, newFightingBros.transform.position.y, true).GetComponent<BathroomTile>();
          BathroomTile targetTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
          newFightingBros.GetComponent<FightingBros>().SetTargetObjectAndTargetPosition(null,
                                                                                        AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                                                                 AStarManager.Instance. GetListCopyOfAStarClosedNodes(),
                                                                                                                                 startTile,
                                                                                                                                 targetTile));
          objectOccupyingBathroomObjectToRemove.Add(firstBroFound);
          objectOccupyingBathroomObjectToRemove.Add(secondBroFound);

          firstBroFound = null;
          secondBroFound = null;

          ScoreManager.Instance.GetPlayerScoreTracker().PerformBroBathroomObjectBrokenByFightingScore(firstBroFoundReference.type, type);
          ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedFightScore(firstBroFoundReference.type);
          ScoreManager.Instance.GetPlayerScoreTracker().PerformBroStartedFightScore(secondBroFoundReference.type);
        }
      }
      foreach(GameObject gameObj in objectOccupyingBathroomObjectToRemove) {
        objectsOccupyingBathroomObject.Remove(gameObj);
      }
    }
  }
}
