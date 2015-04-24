using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicHelper : MonoBehaviour {


    // public delegate void Logic();
    // public Logic logicToPerform = null;
    
    public static GameObject currentGameObject = null;
    
    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile CinematicHelper _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static CinematicHelper() {
    }
    
    public static CinematicHelper Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    if(_instance == null) {
                        GameObject managerGameObject = new GameObject("CinematicHelperGameObject");
                        _instance = (managerGameObject.AddComponent<CinematicHelper>()).GetComponent<CinematicHelper>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private CinematicHelper() {
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    
    
    //--------------------------------------------------------------------------
    // Creation Logic
    //--------------------------------------------------------------------------
    public GameObject GetChildGameObject(GameObject fromGameObject, string childName) {
        // Debug.Log("Searching: " + fromGameObject.name);
        GameObject gameObjectToReturn = null;
        foreach(Transform childTransform in fromGameObject.transform) {
            // Debug.Log("Checking: " + childTransform.gameObject.name);
            if(childTransform.gameObject.name == childName) {
                // Debug.Log("Found: " + childName);
                gameObjectToReturn = childTransform.gameObject;
                // stops search immediately meaning it finds the first occurence
                // of the object and stops
                break;
            }
            else {
                // Debug.Log("Diving");
                gameObjectToReturn = GetChildGameObject(childTransform.gameObject, childName);
            }
        }
        return gameObjectToReturn;
    }
    
    //--------------------------------------------------------------------------
    // Creation Logic
    //--------------------------------------------------------------------------
    public CinematicHelper CreateAnimation(string resourcePrefabPath, Vector3 startPosition) {
        currentGameObject = (GameObject)GameObject.Instantiate(Resources.Load(resourcePrefabPath) as GameObject);
        SetPosition(startPosition);
        return this;
    }
    
    public CinematicHelper CreateBro(string resourcePrefabPath, Vector3 startPosition) {
        // currentGameObject = Factory.Instance.GenerateBroGameObject(broType);
        currentGameObject = (GameObject)GameObject.Instantiate(Resources.Load(resourcePrefabPath) as GameObject);
        SetPosition(startPosition);
        SetTargetObjectAndTargetPosition(null, startPosition);
        return this;
    }
    
    public CinematicHelper SetObject(GameObject newCurrentGameObject) {
        currentGameObject = newCurrentGameObject;
        return this;
    }
    
    public CinematicHelper SetPosition(Vector3 newPosition) {
        currentGameObject.transform.position = newPosition;
        return this;
    }
    
    //--------------------------------------------------------------------------
    // Bathroom Facing
    //--------------------------------------------------------------------------
    public CinematicHelper SetFacing(Facing newFacing) {
        currentGameObject.GetComponent<BathroomFacing>().SetFacing(newFacing);
        return this;
    }
    
    //--------------------------------------------------------------------------
    // Target Pathing Logic
    //--------------------------------------------------------------------------
    public CinematicHelper SetTargetObjectAndTargetPosition(GameObject newTargetObject, Vector3 newTargetPathingPosition) {
        currentGameObject.GetComponent<TargetPathing>().SetTargetObjectAndTargetPosition(newTargetObject, newTargetPathingPosition);
        return this;
    }
    
    //--------------------------------------------------------------------------
    // Animation Logic
    //--------------------------------------------------------------------------
    public CinematicHelper PlayAnimation(string animationName, List<GameObject> animatorGameObjects, bool destroyOnFinish = false) {
        foreach(GameObject animatorGameObject in animatorGameObjects) {
            PlayAnimation(animationName, animatorGameObject, destroyOnFinish);
        }
        
        return this;
    }
    
    public CinematicHelper PlayAnimation(string animationName, GameObject animatorGameObject, bool destroyOnFinish = false) {
        Animator animator = animatorGameObject.GetComponent<Animator>();
        animator.Play(animationName);
        animatorGameObject.GetComponent<AnimatorHelper>().SetDestroyOnFinish(destroyOnFinish);
        
        return this;
    }
    
    public CinematicHelper PlayAnimation(string animationName, bool destroyOnFinish = false) {
        Animator animator = currentGameObject.GetComponent<Animator>();
        animator.Play(animationName);
        currentGameObject.GetComponent<AnimatorHelper>().SetDestroyOnFinish(destroyOnFinish);
        
        return this;
    }
    
    public CinematicHelper SetOnAnimationFinish(string animationName, List<GameObject> animatorGameObjects, AnimatorHelper.StateEvent onAnimationFinish, bool loopEvent = false) {
        foreach(GameObject animatorGameObject in animatorGameObjects) {
            SetOnAnimationFinish(animationName, animatorGameObject, onAnimationFinish, loopEvent);
        }
        return this;
    }
    
    public CinematicHelper SetOnAnimationFinish(string animationName, GameObject animatorGameObject, AnimatorHelper.StateEvent onAnimationFinish, bool loopEvent = false) {
        AnimatorHelper animatorHelper = animatorGameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            animatorHelper.SetOnAnimationFinish(animationName, onAnimationFinish, loopEvent);
        }
        return this;
    }
    
    public CinematicHelper SetOnAnimationFinish(string animationName, AnimatorHelper.StateEvent onAnimationFinish, bool loopEvent = false) {
        AnimatorHelper animatorHelper = currentGameObject.GetComponent<AnimatorHelper>();
        if(animatorHelper != null) {
            animatorHelper.SetOnAnimationFinish(animationName, onAnimationFinish, loopEvent);
        }
        return this;
    }
    
    //--------------------------------------------------------------------------
    // Bro Logic
    //--------------------------------------------------------------------------
    public CinematicHelper SetBroState(BroState newBroState) {
        currentGameObject.GetComponent<Bro>().SetState(newBroState);
        return this;
    }
    
    public CinematicHelper BroEnterThroughLineQueue(int lineQueueEntrance) {
        Bro broReferece = currentGameObject.GetComponent<Bro>();
        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(lineQueueEntrance).GetComponent<LineQueue>();
        
        GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();
        Vector2 startBroPosition = new Vector2(lastQueueTile.transform.position.x,
                                               lastQueueTile.transform.position.y);
                                               
        broReferece.SetLocation(startBroPosition)
        .SetTargetObjectAndTargetPosition(null, entranceLineQueue.GetQueueMovementNodes());
        // .SetTargetObjectAndTargetPosition(null, startBroPosition);
        
        return this;
    }
    
    public CinematicHelper BroMoveToTile(int tileX, int tileY, bool appendToCurrentMovementNodes = true) {
        BroMoveToTile(tileX, tileY, appendToCurrentMovementNodes, currentGameObject);
        return this;
    }
    
    public CinematicHelper BroMoveToTile(int tileX, int tileY, bool appendToCurrentMovementNodes, GameObject currentObjectToModify) {
        Bro broReferece = currentObjectToModify.GetComponent<Bro>();
        
        // get bro's last movement node of the existing nodes?
        // if movement nodes is empty, use bros current movement node
        // then get tile x/y passed in
        // calculate astar path accounting for permanently closed nodes
        // if append nodes appen
        // else replace the movement nodes
        GameObject startTileGameObject = null;
        GameObject endTileGameObject = null;
        
        //----------------------------------------------------------------------
        // start at last node if exist and they should be appended
        if(broReferece.HasMovementNodes()
            && appendToCurrentMovementNodes) {
            startTileGameObject = broReferece.GetLastMovementNode();
        }
        // otherwise just start from the bro's position
        else {
            startTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(broReferece.transform.position, false);
        }
        endTileGameObject = BathroomTileMap.Instance.GetTileGameObjectByIndex(tileX, tileY, true);
        //----------------------------------------------------------------------
        BathroomTile startTile = startTileGameObject.GetComponent<BathroomTile>();
        BathroomTile endTile = endTileGameObject.GetComponent<BathroomTile>();
        //----------------------------------------------------------------------
        // If either start or end tile doesn't exist in the tile map, then
        // get their equivalent versions based on world position and use
        // those instead
        if(startTile.tileX < 0
            || startTile.tileY < 0) {
            startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(startTileGameObject.transform.position.x,
                                                                                  startTileGameObject.transform.position.y,
                                                                                  true).GetComponent<BathroomTile>();
        }
        if(endTile.tileX < 0
            || endTile.tileY < 0) {
            endTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(endTileGameObject.transform.position.x,
                                                                                endTileGameObject.transform.position.y,
                                                                                true).GetComponent<BathroomTile>();
        }
        // Debug.Log("Start Tile: " + startTile);
        // Debug.Log("End Tile: " + endTile);
        
        List<GameObject> newMovementNodes = null;
        newMovementNodes = AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                    AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                    startTile,
                                                                    endTile);
                                                                    
        if(appendToCurrentMovementNodes) {
            broReferece.AddMovementNodes(newMovementNodes);
        }
        else {
            broReferece.SetMovementNodes(newMovementNodes);
        }
        
        return this;
    }
    
    //--------------------------------------------------------------------------
    // Building Logic
    //--------------------------------------------------------------------------
    public GameObject Build() {
        GameObject objectToReturn = currentGameObject;
        currentGameObject = null;
        return objectToReturn;
    }
}