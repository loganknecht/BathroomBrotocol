using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicHelper : MonoBehaviour {


    public delegate void Logic();
    // public Logic logicToPerform = null;
    
    public static GameObject broGameObject = null;
    
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
    
    
    // public CinematicHelper GetAnimationStates(Animator animatior) {
    // return this;
    // }
    // public CinematicHelper AnimatorGetState(GameObject animatorGameObject) {
    // Animator animator = animatorGameObject.GetComponent<Animator>();
    // if(animator != null) {
    //     UnityEditor.Animations.AnimatorController ac = animator.runtimeAnimatorController as UnityEditor.Animations.AnimatorController;
    //     foreach(UnityEditor.Animations.ChildAnimatorState cas in ac.states) {
    //         Debug.Log(cas.state.name);
    //     }
    // }
    // return this;
    // }
    // This is not good, because it assumes
    // 1) The animation does not loop
    // 2) The animation's frames will be player consistently
    // 3) The playback retrieved is going to end up at a number that is > 0.98 which may not occur
    // Until this fails it will be used for cinematic animations
    public CinematicHelper AnimationIfAtFinish(string animationName, Animator animator, Logic onAnimationFinish) {
        // int stateId = Animator.StringToHash(animationName);
        AnimatorStateInfo currentBaseState = animator.GetCurrentAnimatorStateInfo(0);
        float playbackTime = currentBaseState.normalizedTime % 1;
        // // Debug.Log("playbackTime: " + playbackTime);
        
        // // if(currentBaseState.IsName(animationName)) {
        if(currentBaseState.IsName(animationName) && playbackTime >= 0.97) {
            // if(currentBaseState.IsName(animationName) && currentBaseState.normalizedTime >= 0.97) {
            
            // if(!animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            // Debug.Log("At Finish");
            onAnimationFinish();
        }
        
        // if(animator.IsInTransition(0) &&
        // animator.GetNextAnimatorStateInfo(0).nameHash == animController.stateA) {
        //Do reaction
        // }
        
        return this;
    }
    
    public CinematicHelper CreateBro(BroType broType) {
        broGameObject = Factory.Instance.GenerateBroGameObject(broType);
        return this;
        // Debug.Log("triggering start animation");
    }
    
    public CinematicHelper BroEnterThroughLineQueue(int lineQueueEntrance) {
        Bro broReferece = broGameObject.GetComponent<Bro>();
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
        BroMoveToTile(tileX, tileY, appendToCurrentMovementNodes, broGameObject);
        return this;
    }
    
    public CinematicHelper BroMoveToTile(int tileX, int tileY, bool appendToCurrentMovementNodes, GameObject broGameObjectToModify) {
        Bro broReferece = broGameObjectToModify.GetComponent<Bro>();
        
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
    
    public GameObject BuildBro() {
        GameObject broToReturn = broGameObject;
        broGameObject = null;
        return broToReturn;
    }
}