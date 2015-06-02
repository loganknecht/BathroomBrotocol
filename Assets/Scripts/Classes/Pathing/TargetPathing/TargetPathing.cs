using FullInspector;

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
    
    public bool isPaused = false;
    public bool disableMovementLogic = false;
    
    public bool hasPerformedOnArrivalAtTargetPosition = false;
    public CustomEvents<System.Action> onArrivalAtTargetPosition = null;
    public CustomEvents<System.Action> onArrivalAtMovementNodeLogic = null;
    public CustomEvents<System.Action> onPopMovementNodeLogic = null;
    
    protected override void Awake() {
        base.Awake();
        
        movementNodes = new List<GameObject>();
        
        onArrivalAtMovementNodeLogic = CustomEvents<System.Action>.Create();
        onArrivalAtTargetPosition = CustomEvents<System.Action>.Create();
        onPopMovementNodeLogic = CustomEvents<System.Action>.Create();
    }
    
    // Use this for initialization
    public virtual void Start() {
    }
    
    // Update is called once per frame
    public virtual void Update() {
        if(movementNodes == null) {
            Debug.Log("lol lol movement nodes of " + this.gameObject.name + " be null.");
        }
        if(!isPaused) {
            PerformLogic();
        }
    }
    
    public TargetPathing SetMoveSpeed(Vector2 newMoveSpeed) {
        SetXMoveSpeed(newMoveSpeed.x);
        SetYMoveSpeed(newMoveSpeed.y);
        return this;
    }
    public TargetPathing SetMoveSpeed(float newXMoveSpeed, float newYMoveSpeed) {
        SetXMoveSpeed(newXMoveSpeed);
        SetYMoveSpeed(newYMoveSpeed);
        return this;
    }
    
    public float GetXMoveSpeed() {
        return xMoveSpeed;
    }
    public TargetPathing SetXMoveSpeed(float newMoveSpeed) {
        xMoveSpeed = newMoveSpeed;
        return this;
    }
    
    public float GetYMoveSpeed() {
        return yMoveSpeed;
    }
    public TargetPathing SetYMoveSpeed(float newMoveSpeed) {
        yMoveSpeed = newMoveSpeed;
        return this;
    }
    
    public GameObject GetTargetObject() {
        return targetObject;
    }
    public TargetPathing SetTargetObject(GameObject newTargetObject) {
        targetObject = newTargetObject;
        return this;
    }
    
    public List<GameObject> GetMovementNodes() {
        return movementNodes;
    }
    
    public GameObject GetFirstMovementNode() {
        if(movementNodes.Count > 0) {
            return movementNodes[0];
        }
        else {
            return null;
        }
    }
    
    public GameObject GetLastMovementNode() {
        if(movementNodes.Count > 0) {
            return movementNodes[movementNodes.Count - 1];
        }
        else {
            return null;
        }
    }
    
    public TargetPathing AddMovementNodes(List<GameObject> newMovementNodes) {
        if(movementNodes == null) {
            movementNodes = new List<GameObject>();
        }
        movementNodes.AddRange(newMovementNodes);
        
        return this;
    }
    
    public TargetPathing SetMovementNodes(List<GameObject> newMovementNodes) {
        movementNodes = newMovementNodes;
        return this;
    }
    
    public bool HasMovementNodes() {
        if(movementNodes.Count > 0) {
            return true;
        }
        else {
            return false;
        }
    }
    
    public void ClearMovementNodes() {
        movementNodes.Clear();
    }
    
    public virtual TargetPathing SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector3 newTargetPosition) {
        List<GameObject> movementNodes = new List<GameObject>();
        
        GameObject placeholderBathroomTile = new GameObject("PlaceholderBathroomTileIn");
        placeholderBathroomTile.transform.position = newTargetPosition;
        BathroomTile placeholderBathroomTileRef = placeholderBathroomTile.AddComponent<BathroomTile>().GetComponent<BathroomTile>();
        
        placeholderBathroomTile.transform.parent = BathroomTileMap.Instance.gameObject.transform;
        placeholderBathroomTileRef.tileX = -1;
        placeholderBathroomTileRef.tileY = -1;
        
        movementNodes.Add(placeholderBathroomTile);
        
        SetTargetObjectAndTargetPosition(newTargetObject, movementNodes);
        
        return this;
    }
    
    public virtual TargetPathing SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector2 newTargetPosition) {
        SetTargetObjectAndTargetPosition(newTargetObject, new Vector3(newTargetPosition.x, newTargetPosition.y, 0));
        return this;
    }
    
    public virtual TargetPathing SetTargetObjectAndTargetPosition(GameObject newTargetObject, List<GameObject> newMovementNodes) {
        hasPerformedOnArrivalAtTargetPosition = false;
        SetTargetObject(newTargetObject);
        SetMovementNodes(newMovementNodes);
        PopMovementNode();
        return this;
    }
    
    public Vector3 GetTargetPosition() {
        return targetPosition;
    }
    public TargetPathing SetTargetPosition(Vector3 newTargetPosition) {
        targetPosition = newTargetPosition;
        return this;
    }
    
    public TargetPathing OnArrivalAtTargetPositionLogic(System.Action newOnArrivalAtTargetPositionLogic, bool loop = false) {
        onArrivalAtTargetPosition.Add(newOnArrivalAtTargetPositionLogic, loop);
        return this;
    }
    
    public TargetPathing OnArrivalAtMovementNodeLogic(System.Action newOnArrivalAtMovementNodeLogic, bool loop = false) {
        onArrivalAtMovementNodeLogic.Add(newOnArrivalAtMovementNodeLogic, loop);
        return this;
    }
    
    public TargetPathing OnPopMovementNodeLogic(System.Action newOnPopMovementNodeLogic, bool loop = false) {
        onPopMovementNodeLogic.Add(newOnPopMovementNodeLogic, loop);
        return this;
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
        newPositionOffset = (newPositionOffset * Time.deltaTime);
        newPositionOffset = LockNewPositionOffsetToTarget(newPositionOffset);
        UpdateBathroomFacingBasedOnNewPositionOffset(newPositionOffset);
        transform.position += new Vector3(newPositionOffset.x, newPositionOffset.y, 0);
        
        //performs check to pop new node from the movemeNodes list
        if(IsAtMovementNodePosition()) {
            onArrivalAtMovementNodeLogic.Execute();
            
            //Debug.Log("object at position");
            PopMovementNode();
        }
        
        //performs check
        if(IsAtTargetPosition()) {
            if(!hasPerformedOnArrivalAtTargetPosition) {
                hasPerformedOnArrivalAtTargetPosition = true;
                onArrivalAtTargetPosition.Execute();
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
            // Debug.Log("Set new position to: " + targetPosition.x + ", " + targetPosition.y);
            movementNodes.RemoveAt(0);
            
            onPopMovementNodeLogic.Execute();
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
        float nextXPosition = gameObjectToMove.transform.position.x + newPositionOffset.x;
        float nextYPosition = gameObjectToMove.transform.position.y + newPositionOffset.y;
        
        if((gameObjectToMove.transform.position.x < targetPosition.x  && nextXPosition > targetPosition.x)
            || (gameObjectToMove.transform.position.x > targetPosition.x  && nextXPosition < targetPosition.x)) {
            newPositionOffset.x = targetPosition.x - gameObjectToMove.transform.position.x;
        }
        if((gameObjectToMove.transform.position.y < targetPosition.y && nextYPosition > targetPosition.y)
            || (gameObjectToMove.transform.position.y > targetPosition.y && nextYPosition < targetPosition.y)) {
            newPositionOffset.y = targetPosition.y - gameObjectToMove.transform.position.y;
        }
        
        return newPositionOffset;
    }
    
    public void UpdateBathroomFacingBasedOnNewPositionOffset(Vector2 newPositionOffset) {
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
