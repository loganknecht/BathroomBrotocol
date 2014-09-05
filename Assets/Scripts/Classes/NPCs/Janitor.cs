using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Janitor : TargetPathingNPC {

  public HighlightSelectable selectableReference = null;
  public JanitorState state = JanitorState.None;
  public LineQueue lineQueueIn = null;

  public float occupationTimer = 0f;

  public bool isSummoned = false;
  public bool isPaused = false;

  public override void Awake() {
    selectableReference = this.gameObject.GetComponent<HighlightSelectable>();
  }
	// Use this for initialization
	public override void Start () {
    base.Start();

    this.gameObject.transform.eulerAngles = Camera.main.transform.eulerAngles;

    selectableReference.canBeSelected = true;
	}

	// Update is called once per frame
	public override void Update () {
    if(!isPaused) {
      base.Update();
    }
	}

  public void OnMouseDown() {
    // SelectionManager.Instance.currentlySelectedJanitorGameObject = this.gameObject;
    SelectionManager.Instance.SelectJanitor(this.gameObject);
  }

  public override void PerformLogic() {
    switch(state) {
      case(JanitorState.None):
      break;
      case(JanitorState.CleaningObject):
      break;
      case(JanitorState.Entering):
        // PerformEnteringLogic();
        PerformArrivalLogic();
        PerformMovingToTargetObjectLogic();
      break;
      case(JanitorState.Exiting):
        PerformArrivalLogic();
        PerformMovingToTargetObjectLogic();
        // PerformExitingLogic();
      break;
      case(JanitorState.MovingToTargetObject):
        PerformArrivalLogic();
        PerformMovingToTargetObjectLogic();
      break;
      case(JanitorState.RepairingObject):
        // PerformOccupyingObjectLogic();
        PerformRepairingObjectLogic();
      break;
      case(JanitorState.Roaming):
        PerformMovementLogic();
        PerformArrivalLogic();
        PerformRoamingLogic();
      break;
      case(JanitorState.Standing):
        // PerformStandingLogic();
      break;
      default:
        Debug.Log("ERROR IN JANITOR LOGIC");
      break;
    }
  }

  public override void SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<Vector2> newMovementNodes) {
    occupationTimer = 0;


    if(targetObject != null) {
      BathroomTile janitorTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, false).GetComponent<BathroomTile>();
      BathroomTile targetTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(targetObject.transform.position.x, targetObject.transform.position.y, false).GetComponent<BathroomTile>();
      //if at the tile where the target is reset it before targetting new object
      if(janitorTile.tileX == targetTile.tileX
         && janitorTile.tileY == targetTile.tileY) {
        if(targetObject.GetComponent<BathroomObject>() != null) {
          targetObject.GetComponent<BathroomObject>().ResetColliderAndSelectableReference();
        }
        else if(targetObject.GetComponent<BathroomTileBlocker>() != null) {
          targetObject.GetComponent<BathroomTileBlocker>().ResetColliderAndSelectableReference();
        }
      }
    }

    base.SetTargetObjectAndTargetPosition(newTargetObject, newMovementNodes);
  }

  public override void UpdateAnimator() {
    base.UpdateAnimator();

    animatorReference.SetBool(JanitorState.None.ToString(), false);
    animatorReference.SetBool(JanitorState.CleaningObject.ToString(), false);
    animatorReference.SetBool(JanitorState.Entering.ToString(), false);
    animatorReference.SetBool(JanitorState.Exiting.ToString(), false);
    animatorReference.SetBool(JanitorState.MovingToTargetObject.ToString(), false);
    animatorReference.SetBool(JanitorState.RepairingObject.ToString(), false);
    animatorReference.SetBool(JanitorState.Roaming.ToString(), false);
    animatorReference.SetBool(JanitorState.Standing.ToString(), false);

    animatorReference.SetBool(state.ToString(), true);
  }

  public virtual void PerformArrivalLogic() {
    if(transform.position.x == targetPosition.x
       && transform.position.y == targetPosition.y
       && movementNodes.Count == 0) {

      if(targetObject != null
         && targetObject.GetComponent<BathroomObject>() != null) {
        if(targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
          targetObject.collider.enabled = false;
          // BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

          selectableReference.canBeSelected = false;
          selectableReference.ResetHighlightObjectAndSelectedState();

          //TODO: do in the context of janitor
          // if(SelectionManager.Instance.currentlySelectedBroGameObject != null
          //    && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedBroGameObject.GetInstanceID()) {
          //   SelectionManager.Instance.currentlySelectedBroGameObject = null;
          // }

          selectableReference.canBeSelected = false;
          selectableReference.ResetHighlightObjectAndSelectedState();
          state = JanitorState.RepairingObject;
        }
        else {
          if(SelectionManager.Instance.currentlySelectedJanitorGameObject != null
             && this.gameObject.GetInstanceID() == SelectionManager.Instance.currentlySelectedJanitorGameObject.GetInstanceID()) {
            SelectionManager.Instance.currentlySelectedJanitorGameObject = null;
          }
          // if(JanitorManager.Instance.currentJanitor != null
          //    && this.gameObject.GetInstanceID() == JanitorManager.Instance.currentJanitor.GetInstanceID()) {
          //   JanitorManager.Instance.currentJanitor = null;
          // }
          Destroy(this.gameObject);
        }
      }
      else if(targetObject != null
              && targetObject.GetComponent<BathroomTileBlocker>() != null) {
        selectableReference.canBeSelected = false;
        selectableReference.ResetHighlightObjectAndSelectedState();
        state = JanitorState.RepairingObject;
      }
      else {
        if(state == JanitorState.Entering) {
          lineQueueIn = null;
        }
        state = JanitorState.Roaming;
      }
    }
  }
  public virtual void PerformEnteringLogic() {
  }
  public virtual void PerformExitingLogic() {
  }
  public virtual void PerformMovingToTargetObjectLogic() {
    PerformMovementLogic();
    PerformArrivalLogic();
  }
  public virtual void PerformRepairingObjectLogic() {
    if(targetObject != null
       && targetObject.GetComponent<BathroomObject>() != null) {
      BathroomObject bathObjRef = targetObject.GetComponent<BathroomObject>();

      if(bathObjRef.state == BathroomObjectState.Broken
         || bathObjRef.state == BathroomObjectState.BrokenByPee
         || bathObjRef.state == BathroomObjectState.BrokenByPoop) {
        if(occupationTimer > bathObjRef.repairDuration) {
          //OBJECT LOGIC ACTUALLY STARTS HERE
          if(bathObjRef.type == BathroomObjectType.Stall
             || bathObjRef.type == BathroomObjectType.Urinal
             || bathObjRef.type == BathroomObjectType.Sink) {
              collider.enabled = true;
              targetObject.collider.enabled = true;
              targetObject = null;

              state = JanitorState.Roaming;
              bathObjRef.state = BathroomObjectState.Idle;

              selectableReference.canBeSelected = true;
              // Debug.Log("repaired");
          }
          else if(bathObjRef.type == BathroomObjectType.Exit) {
            targetObject.collider.enabled = true;

            // JanitorManager.Instance.allJanitors.Remove(this.gameObject);
            JanitorManager.Instance.currentJanitor = null;
            Destroy(this.gameObject);
          }
        }
        else {
          //disables the collider because the bro resides in the object, but the timer is still going
          targetObject.collider.enabled = false;
          collider.enabled = true;
          selectableReference.canBeSelected = true;

          occupationTimer += Time.deltaTime;
        }
      }
    }
    else if(targetObject != null
            && targetObject.GetComponent<BathroomTileBlocker>() != null) {
      BathroomTileBlocker bathroomTileBlockerReference = targetObject.GetComponent<BathroomTileBlocker>();
      if(occupationTimer > bathroomTileBlockerReference.repairDuration) {
        BathroomTileBlockerManager.Instance.RemoveBathroomTileBlockerGameObject(targetObject);
        Destroy(targetObject);
        targetObject = null;

        state = JanitorState.Roaming;
      }
      else {
        targetObject.collider.enabled = false;
        collider.enabled = true;
        selectableReference.canBeSelected = true;
        occupationTimer += Time.deltaTime;
      }
    }
  }
  public virtual void PerformRoamingLogic() {
    if(IsAtTargetPosition()) {
      GameObject randomBathroomTile = BathroomTileMap.Instance.SelectRandomOpenTile();
      List<Vector2> movementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                             new List<GameObject>(),
                                                                             BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>(),
                                                                             randomBathroomTile.GetComponent<BathroomTile>());
      SetTargetObjectAndTargetPosition(null, movementNodes);
    }
  }
  public virtual void PerformStandingLogic() {
  }
}
