﻿using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TargetPathing : BaseBehavior {
    public Facing directionBeingLookedAt = Facing.None;

    public Animator animatorReference = null;

    public GameObject gameObjectToMove = null;

    public GameObject targetObject = null;
    public Vector3 targetPosition = Vector3.zero;
    public GameObject targetTile = null;
    public List<GameObject> movementNodes = null; 

    public float xMoveSpeed = 1;
    public float yMoveSpeed = 1;

    public float targetPositionXLockBuffer = 0.05f;
    public float targetPositionYLockBuffer = 0.05f;

    public bool isPaused = false;
    public bool disableMovementLogic = false;

    public delegate void OnArrivalAtMovementNode();
    public OnArrivalAtMovementNode onArrivalAtMovementNodeLogic = null;

    public delegate void OnArrivalAtTargetPosition();
    public OnArrivalAtTargetPosition onArrivalAtTargetPositionLogic = null;
    public bool performedOnArrivalAtTargetPosition = false;

    public delegate void OnPopMovementNode();
    public OnPopMovementNode onPopMovementNodeLogic = null;

    protected override void Awake() {
        base.Awake();

        movementNodes = new List<GameObject>();
    }

    // Use this for initialization
    public virtual void Start () {
    }

    // Update is called once per frame
    public virtual void Update () {
        if(movementNodes == null) {
            Debug.Log("lol lol movement nodes of " + this.gameObject.name + " be null.");
        }
        if(!isPaused) {
            PerformLogic();
        }
    }

    public float GetXMoveSpeed() {
        return xMoveSpeed;
    }
    public void SetXMoveSpeed(float newMoveSpeed) {
        xMoveSpeed = newMoveSpeed;
    }

    public float GetYMoveSpeed() {
        return yMoveSpeed;
    }
    public void SetYMoveSpeed(float newMoveSpeed) {
        yMoveSpeed = newMoveSpeed;
    }

    public GameObject GetTargetObject() {
        return targetObject;
    }
    public void SetTargetObject(GameObject newTargetObject) {
        targetObject = newTargetObject;
    }

    public List<GameObject> GetMovementNodes() {
        return movementNodes;
    }
    public void SetMovementNodes(List<GameObject> newMovementNodes) {
        movementNodes = newMovementNodes;
        performedOnArrivalAtTargetPosition = false;
    }

    public void ClearMovementNodes() {
        movementNodes.Clear();
    }

    public virtual void SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector3 newTargetPosition) {
        SetTargetObjectAndTargetPosition(newTargetObject, new Vector2(newTargetPosition.x, newTargetPosition.y));
    }

    public virtual void SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
        performedOnArrivalAtTargetPosition = false;
        SetTargetObject(newTargetObject);
        SetMovementNodes(newMovementNodes);
        PopMovementNode();
    }

    public Vector3 GetTargetPosition() {
        return targetPosition;
    }
    public void SetTargetPosition(Vector3 newTargetPosition) {
        targetPosition = newTargetPosition;
    }

    public void SetOnArrivalAtTargetPosition(OnArrivalAtTargetPosition newOnArrivalAtTargetPositionLogic) {
        onArrivalAtTargetPositionLogic = new OnArrivalAtTargetPosition(newOnArrivalAtTargetPositionLogic);
    }

    public void SetOnArrivalAtMovementNodeLogic(OnArrivalAtMovementNode newOnArrivalAtMovementNodeLogic) {
        onArrivalAtMovementNodeLogic = new OnArrivalAtMovementNode(newOnArrivalAtMovementNodeLogic);
    }

    public void SetOnPopMovementNodeLogic(OnPopMovementNode newOnPopMovementNodeLogic) {
        onPopMovementNodeLogic = new OnPopMovementNode(newOnPopMovementNodeLogic);
    }

    public bool IsAtMovementNodePosition() {
        if(gameObjectToMove.transform.position.x == targetPosition.x
            && gameObjectToMove.transform.position.y == targetPosition.y) {
            return true;
        }
        else {
            return false;
        }
    }
    public bool IsAtTargetPosition() {
        if(movementNodes.Count == 0
            && gameObjectToMove.transform.position.x == targetPosition.x
            && gameObjectToMove.transform.position.y == targetPosition.y) {
            return true;
        }
        else {
            return false;
        }
    }

    public virtual void PerformLogic() {
        if(!disableMovementLogic) {
            PerformMovementLogic();
        }
    }

    public virtual void PerformMovementLogic() {
        //This is the logic for the bro moving to their destination
        Vector2 newPositionOffset = CalculateNextPositionOffset();
        newPositionOffset = (newPositionOffset*Time.deltaTime);
        newPositionOffset = LockNewPositionOffsetToTarget(newPositionOffset);
        UpdateBathroomFacingBasedOnNewPositionOffset(newPositionOffset);
        transform.position += new Vector3(newPositionOffset.x, newPositionOffset.y, 0);

        //performs check to pop new node from the movemeNodes list
        if(IsAtMovementNodePosition()) {
            if(onArrivalAtMovementNodeLogic != null) {
                onArrivalAtMovementNodeLogic();
            }

            //Debug.Log("object at position");
            PopMovementNode();
        }

        if(!performedOnArrivalAtTargetPosition
           && IsAtTargetPosition()) {
            performedOnArrivalAtTargetPosition = true;
            if(onArrivalAtTargetPositionLogic != null) {
                onArrivalAtTargetPositionLogic();
            }
        }
    }

    public virtual void PopMovementNode() {
        if(movementNodes == null) {
            Debug.Log("movemeNodes is null");
        }
        if(movementNodes.Count > 0) {
        //Debug.Log("Arrived at: " + targetPosition.x + ", " + targetPosition.y);
            GameObject nextNode = movementNodes[0];
            targetTile = nextNode;
            targetPosition = new Vector3(nextNode.transform.position.x, nextNode.transform.position.y, this.transform.position.z);
            //Debug.Log("Set new position to: " + targetPosition.x + ", " + targetPosition.y);
            movementNodes.RemoveAt(0);
            if(onPopMovementNodeLogic != null) {
                onPopMovementNodeLogic();
            }
            // Destroy(nextNode);
            // Debug.Log(this.gameObject.name + " has " + movementNodes.Count + " number of movemeNodes");
        }
        else {
            targetTile = null;
        }
    }

    public virtual Vector2 CalculateNextPositionOffset() {
        Vector2 newPositionOffset = Vector2.zero;

        if(gameObjectToMove.transform.position.x < targetPosition.x) {
            newPositionOffset.x += xMoveSpeed;
        }
        else if(gameObjectToMove.transform.position.x > targetPosition.x) {
            newPositionOffset.x -= xMoveSpeed;
        }

        if(gameObjectToMove.transform.position.y < targetPosition.y) {
            newPositionOffset.y += yMoveSpeed;
        }
        else if(gameObjectToMove.transform.position.y > targetPosition.y) {
            newPositionOffset.y -= yMoveSpeed;
        }

        return newPositionOffset;
    }

    public virtual Vector2 LockNewPositionOffsetToTarget(Vector2 newPositionOffset) {
        // X Locker
        // Debug.Log("target x pos great: " + (targetPosition.x - targetPositionXLockBuffer));
        // Debug.Log("target x pos less: " + (targetPosition.x + targetPositionXLockBuffer));
        // Debug.Log("target x pos: " + targetPosition.x);
        // Debug.Log("next x pos: " + (this.gameObject.transform.position.x + newPositionOffset.x));
        if((gameObjectToMove.transform.position.x + newPositionOffset.x) > (targetPosition.x - targetPositionXLockBuffer)
            && (gameObjectToMove.transform.position.x + newPositionOffset.x) < (targetPosition.x + targetPositionXLockBuffer)) {
            //Debug.Log("setting x position to target");
            gameObjectToMove.transform.position = new Vector3(targetPosition.x,
                                                             gameObjectToMove.transform.position.y,
                                                             gameObjectToMove.transform.position.z);
            newPositionOffset.x = 0;
        }
        // Y Locker
        if((gameObjectToMove.transform.position.y + newPositionOffset.y) > (targetPosition.y - targetPositionYLockBuffer)
            && (gameObjectToMove.transform.position.y + newPositionOffset.y) < (targetPosition.y + targetPositionYLockBuffer)) {
            //Debug.Log("setting y position to target");
            gameObjectToMove.transform.position = new Vector3(gameObjectToMove.transform.position.x,
                                                             targetPosition.y,
                                                             gameObjectToMove.transform.position.z);
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

        // if(newPositionOffset.x != 0
            // || newPositionOffset.y != 0) {
        directionBeingLookedAt = CameraManager.Instance.rotateReference.GetDirectionFacingBasedOnCameraAndMovementDirection(movingUp, movingRight, movingDown, movingLeft);
        // }
        // else {
        //     if(movementNodes.Count == 0) {
        //         directionBeingLookedAt = Facing.None;
        //     }
        // }
    }
}
