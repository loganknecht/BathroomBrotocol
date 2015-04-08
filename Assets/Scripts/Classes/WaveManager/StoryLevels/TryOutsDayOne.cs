using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject,
//                                   LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x,
//                                   -595,
//                                   lEVELmANAGER.iNSTANCE.JANITORoVERLAYgAMEoBJECT.TRANSFORM.LOCALpOSITION.X,
//                                   -250,
//                                   1,
//                                   2,
//                                   uitWEENER.mETHOD.bOUNCEiN,
//                                   null);
// Debug.Log("performing start animation");
// if(TextboxManager.Instance.HasFinished()) {
// TriggerWaveFinish();
// }
public class TryOutsDayOne : WaveLogic, WaveLogicContract {

    public Bro broCzarReference = null;
    
    public override void Awake()
    {   base.Awake(); }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
        Initialize();
    }
    
    public override void Initialize() {
        // Debug.Log("initialization");
        SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);
        
        GameObject basicBrosEnter = CreateWaveState("Start Animation Game Object",
                                                    TriggerAnimationBasicBrosEnter,
                                                    PerformAnimationBasicBrosEnter,
                                                    FinishAnimationBasicBrosEnter);
        GameObject broCzarEnterWaveGameObject = CreateWaveState("Start Animation Game Object",
                                                                TriggerBroCzarEnterAnimation,
                                                                PerformBroCzarEnterAnimation,
                                                                FinishBroCzarEnterAnimation);
                                                                
        InitializeWaveStates(
            // basicBrosEnter
            broCzarEnterWaveGameObject
        );
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    //--------------------------------------------------------------------------
    // Bros Enter and Line Up
    //--------------------------------------------------------------------------
    public void TriggerAnimationBasicBrosEnter() {
        BroGenerator.Instance.Pause();
        broCzarReference.gameObject.SetActive(false);
        
        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);
        
        // LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        // GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();
        // Vector3 startBroCzarPosition = new Vector2(lastQueueTile.transform.position.x,
        //                                            lastQueueTile.transform.position.y);
        // broCzarReference.SetLocation(startBroCzarPosition)
        // .SetTargetObjectAndTargetPosition(null, startBroCzarPosition);
        // yield WaitForSeconds(10);
        
        List<GameObject> brosToEnter = new List<GameObject>();
        for(int i = 0; i < 5; i++) {
            GameObject newBro = Factory.Instance.GenerateBroGameObject(BroType.GenericBro);
            brosToEnter.Add(newBro);
        }
    }
    
    public void PerformAnimationBasicBrosEnter() {
        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        GameObject firstQueueTile = entranceLineQueue.GetFirstQueueTile();
        
        List<GameObject> entranceToCenterMovementNodes = entranceLineQueue.GetQueueMovementNodes();
        entranceToCenterMovementNodes.AddRange(AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                        AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                        BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(firstQueueTile.transform.position, true).GetComponent<BathroomTile>(),
                                                                                        BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>()));
                                                                                        
        broCzarReference.targetPathingReference.SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);
        broCzarReference.targetPathingReference.SetMoveSpeed(25, 25);
        broCzarReference.targetPathingReference.SetOnArrivalAtTargetPosition(() => {
            broCzarReference.SetState(BroState.Standing).SetFacing(Facing.Bottom);
            broCzarReference.animatorReference.Play("JumpUp");
            
        });
        
        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;
        // }
    }
    
    public void FinishAnimationBasicBrosEnter() {
        // Debug.Log("finishing start animation");
    }
    //--------------------------------------------------------------------------
    // Old Bathroom Bro Czar Enters To The Center of The Screen
    //--------------------------------------------------------------------------
    public void TriggerBroCzarEnterAnimation() {
        Debug.Log("triggering start animation");
        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();
        Vector3 startBroCzarPosition = new Vector2(lastQueueTile.transform.position.x,
                                                   lastQueueTile.transform.position.y);
        broCzarReference.SetLocation(startBroCzarPosition)
        .SetTargetObjectAndTargetPosition(null, startBroCzarPosition);
    }
    
    public void PerformBroCzarEnterAnimation() {
        Debug.Log("performing start animation");
        // waitTime += Time.deltaTime;
        // if(waitTime > 1
        //         && !waitFinished) {
        // waitFinished = true;
        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);
        
        BroGenerator.Instance.Pause();
        
        broCzarReference.gameObject.SetActive(true);
        
        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        GameObject firstQueueTile = entranceLineQueue.GetFirstQueueTile();
        // GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();
        
        // Vector3 startBroCzarPosition = new Vector3(lastQueueTile.transform.position.x,
        //                                            lastQueueTile.transform.position.y,
        //                                            broCzarReference.transform.position.z);
        
        // broCzarReference.SetLocation(startBroCzarPosition)
        // .SetTargetObjectAndTargetPosition(null, startBroCzarPosition);
        
        List<GameObject> entranceToCenterMovementNodes = entranceLineQueue.GetQueueMovementNodes();
        entranceToCenterMovementNodes.AddRange(AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                        AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                                                                        BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(firstQueueTile.transform.position, true).GetComponent<BathroomTile>(),
                                                                                        BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>()));
                                                                                        
        broCzarReference.targetPathingReference.SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);
        broCzarReference.targetPathingReference.SetMoveSpeed(25, 25);
        broCzarReference.targetPathingReference.SetOnArrivalAtTargetPosition(() => {
            broCzarReference.SetState(BroState.Standing).SetFacing(Facing.Bottom);
            broCzarReference.animatorReference.Play("JumpUp");
            
        });
        
        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;
        // }
    }
    
    public void FinishBroCzarEnterAnimation() {
        Debug.Log("finishing start animation");
    }
}