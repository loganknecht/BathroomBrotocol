using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetPathingNPC : BaseBehavior {
    public DirectionBeingLookedAt directionBeingLookedAt = DirectionBeingLookedAt.None;

    public Animator animatorReference = null;

    public GameObject targetObject = null;
    public Vector3 targetPosition = Vector3.zero;

    public float xMoveSpeed = 1;
    public float yMoveSpeed = 1;

    public float targetPositionXLockBuffer = 0.05f;
    public float targetPositionYLockBuffer = 0.05f;

    public List<GameObject> movementNodes = null; 

    public virtual void Awake() {
        movementNodes = new List<GameObject>();
        base.Awake();
    }

    // Use this for initialization
    public virtual void Start () {
        animatorReference = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    public virtual void Update () {
        if(movementNodes == null) {
            Debug.Log("lol lol movement nodes of " + this.gameObject.name + " be null.");
        }
        PerformLogic();
        UpdateAnimator();
    }

	public virtual void UpdateAnimator() {
		if(animatorReference != null) {
			animatorReference.SetBool(DirectionBeingLookedAt.TopLeft.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.Top.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.TopRight.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.Left.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.Right.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.BottomLeft.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.Bottom.ToString(), false);
			animatorReference.SetBool(DirectionBeingLookedAt.BottomRight.ToString(), false);

			animatorReference.SetBool(directionBeingLookedAt.ToString(), true);

			animatorReference.SetBool("None", false);
		}
	}

	public virtual void SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector3 newTargetPosition) {
		SetTargetObjectAndTargetPosition(newTargetObject, new Vector2(newTargetPosition.x, newTargetPosition.y));
	}

	public virtual void SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
		targetObject = newTargetObject;
		movementNodes = newMovementNodes;
		PopMovementNode();
	}

	public bool IsAtTargetPosition() {
		if(movementNodes.Count == 0
		   && this.gameObject.transform.position.x == targetPosition.x
		   && this.gameObject.transform.position.y == targetPosition.y) {
			return true;
		}
		else {
			return false;
		}
	}

	public virtual void PerformLogic() {
		PerformMovementLogic();
	}

	public virtual void PerformMovementLogic() {
  		//This is the logic for the bro moving to their destination
  		Vector2 newPositionOffset = CalculateNextPositionOffset();
  		newPositionOffset = (newPositionOffset*Time.deltaTime);
  		newPositionOffset = LockNewPositionOffsetToTarget(newPositionOffset);
  		UpdateBathroomFacingBasedOnNewPositionOffset(newPositionOffset);

  		//performs check to pop new node from the movemeNodes list
  		if(this.transform.position.x == targetPosition.x
  		   && this.transform.position.y == targetPosition.y) {
  			//Debug.Log("object at position");
  			PopMovementNode();
  		}

  		transform.position += new Vector3(newPositionOffset.x, newPositionOffset.y, 0);
	}

	public virtual void PopMovementNode() {
		if(movementNodes.Count > 0) {
			//Debug.Log("Arrived at: " + targetPosition.x + ", " + targetPosition.y);
			GameObject nextNode = movementNodes[0];
			targetPosition = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, this.transform.position.z);
			//Debug.Log("Set new position to: " + targetPosition.x + ", " + targetPosition.y);
			movementNodes.RemoveAt(0);
			// Destroy(nextNode);
			// Debug.Log(this.gameObject.name + " has " + movementNodes.Count + " number of movemeNodes");
			if(movementNodes == null) {
				Debug.Log("movemeNodes is null");
			}
		}
	}

	public virtual Vector2 CalculateNextPositionOffset() {
		Vector2 newPositionOffset = Vector2.zero;

		if(this.gameObject.transform.position.x < targetPosition.x) {
			newPositionOffset.x += xMoveSpeed;
		}
		else if(this.gameObject.transform.position.x > targetPosition.x) {
			newPositionOffset.x -= xMoveSpeed;
		}

		if(this.gameObject.transform.position.y < targetPosition.y) {
			newPositionOffset.y += yMoveSpeed;
		}
		else if(this.gameObject.transform.position.y > targetPosition.y) {
			newPositionOffset.y -= yMoveSpeed;
		}

		return newPositionOffset;
	}

	public virtual Vector2 LockNewPositionOffsetToTarget(Vector2 newPositionOffset) {
		//x locker
		//			Debug.Log("target x pos great: " + (targetPosition.x - targetPositionXLockBuffer));
		//			Debug.Log("target x pos less: " + (targetPosition.x + targetPositionXLockBuffer));
		//			Debug.Log("target x pos: " + targetPosition.x);
		//			Debug.Log("next x pos: " + (this.gameObject.transform.position.x + newPositionOffset.x));
		if((this.gameObject.transform.position.x + newPositionOffset.x) > (targetPosition.x - targetPositionXLockBuffer)
		   && (this.gameObject.transform.position.x + newPositionOffset.x) < (targetPosition.x + targetPositionXLockBuffer)) {
			//Debug.Log("setting x position to target");
			this.gameObject.transform.position = new Vector3(targetPosition.x,
			                                                 this.gameObject.transform.position.y,
			                                                 this.gameObject.transform.position.z);
			newPositionOffset.x = 0;
		}
		//			//y locker
		if((this.gameObject.transform.position.y + newPositionOffset.y) > (targetPosition.y - targetPositionYLockBuffer)
		   && (this.gameObject.transform.position.y + newPositionOffset.y) < (targetPosition.y + targetPositionYLockBuffer)) {
			//Debug.Log("setting y position to target");
			this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x,
			                                                 targetPosition.y,
			                                                 this.gameObject.transform.position.z);
			newPositionOffset.y = 0;
		}

		return newPositionOffset;
	}

	public void UpdateBathroomFacingBasedOnNewPositionOffset(Vector2 newPositionOffset){
		bool movingUp = false;
		bool movingRight = false;
		bool movingDown = false;
		bool movingLeft = false;

		if(newPositionOffset.x > 0) {
			movingRight = true;
		}
		else if(newPositionOffset.x < 0) {
			movingLeft = true;
		}
		if(newPositionOffset.y > 0) {
			movingUp = true;
		}
		else if(newPositionOffset.y < 0) {
			movingDown = true;
		}

		//Debug.Log("new position offset: " + newPositionOffset);
		if(newPositionOffset.x != 0
		   || newPositionOffset.y != 0) {
		   	directionBeingLookedAt = CameraManager.Instance.GetDirectionFacingBasedOnCameraAndMovementDirection(movingUp, movingRight, movingDown, movingLeft);
		   	
			//Debug.Log("moving");
			//state = BroState.MovingToTargetObject;
			// DirectionBeingLookedAt cameraDirectionBeingLookedAt = CameraManager.Instance.rotateCameraReference.directionBeingLookedAt;
			// DirectionBeingLookedAt cameraDirectionBeingLookedAt = CameraManager.Instance.rotateCameraReference.directionBeingLookedAt;

			// if(movingRight) {
			// 	// moving up right
			// 	if(movingUp) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
			// 	}
			// 	// moving down right
			// 	else if(movingDown) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.BottomRight;
			// 	}
			// 	// moving right
			// 	else {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.Right;
			// 	}
			// }
			// else if(movingLeft) {
			// 	// moving up left
			// 	if(movingUp) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
			// 	}
			// 	// moving down left
			// 	else if(movingDown) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
			// 	}
			// 	// moving left
			// 	else {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.Left;
			// 	}
			// }
			// else {
			// 	// moving up
			// 	if(movingUp) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.Top;
			// 	}
			// 	// moving down 
			// 	if(movingDown) {
			// 		bathroomFacingReference.directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
			// 	}
			// }
		}
		else {
			if(movementNodes.Count == 0) {
				directionBeingLookedAt = DirectionBeingLookedAt.None;
			}
		}
	}
}
