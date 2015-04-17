using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Debug.Log("performing start animation");
// if(TextboxManager.Instance.HasFinished()) {
// TriggerWaveFinish();
// }
public class TryOutsDayOne : WaveLogic, WaveLogicContract {

    public Bro broCzarReference = null;
    public GameObject leftLightningClouds = null;
    public List<GameObject> lightningCloudsToFlash = null;
    public GameObject rightLightningClouds = null;
    
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
        
        // PerformBroCzarEnterAnimation,
        // FinishBroCzarEnterAnimation);
        // basicBrosEnter
        // broCzarEnterWaveGameObject
        
        BathroomTile centerTile = BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>();
        BathroomTile middleLeftTile = BathroomTileMap.Instance.GetMiddleLeftTileGameObject().GetComponent<BathroomTile>();
        BathroomTile topCenterTile = BathroomTileMap.Instance.GetTopCenterTileGameObject().GetComponent<BathroomTile>();
        BathroomTile middleRightTile = BathroomTileMap.Instance.GetMiddleRightTileGameObject().GetComponent<BathroomTile>();
        BathroomTile bottomCenterTile = BathroomTileMap.Instance.GetBottomCenterTileGameObject().GetComponent<BathroomTile>();
        
        GameObject firstBro = null;
        // GameObject secondBro = null;
        // GameObject thirdBro = null;
        // GameObject fourthBro = null;
        // GameObject fifthBro = null;
        
        // firstBro = CinematicHelper.Instance
        //            .CreateBro(BroType.GenericBro)
        //            .BroEnterThroughLineQueue(0)
        //            .BroMoveToTile(middleLeftTile.tileX, middleLeftTile.tileY)
        //            .BroMoveToTile(topCenterTile.tileX, topCenterTile.tileY)
        //            .BroMoveToTile(middleRightTile.tileX, middleRightTile.tileY)
        //            .BroMoveToTile(bottomCenterTile.tileX, bottomCenterTile.tileY)
        //            .BuildBro();
        
        List<GameObject> waveStates = new List<GameObject>();
        
        waveStates.Add(CreateDelayState("Delay", 1f));
        waveStates.Add(CreateWaveState("First Bro Entrance", () => {
            Debug.Log("Generating");
            // firstBro = CinematicHelper.Instance
            //            .CreateBro(BroType.GenericBro)
            //            .BroEnterThroughLineQueue(0)
            //            .BroMoveToTile(middleLeftTile.tileX, middleLeftTile.tileY)
            //            .BroMoveToTile(topCenterTile.tileX, topCenterTile.tileY)
            //            .BroMoveToTile(middleRightTile.tileX, middleRightTile.tileY)
            //            .BroMoveToTile(bottomCenterTile.tileX, bottomCenterTile.tileY)
            //            .BuildBro();
            Completed();
        }));
        waveStates.Add(CreateWaveState("CloudEnter", () => {
            Debug.Log("Clouds Entering");
            TweenExecutor.TweenObjectPosition(leftLightningClouds,
                                              -10, // startX
                                              4, // startY
                                              7, // endX
                                              4, // endY
                                              0, // delay
                                              1, // duration
                                              UITweener.Method.BounceIn, // UITweener.Method easingMethod
                                              null); // EventDelegate eventDelegate
            TweenExecutor.TweenObjectPosition(rightLightningClouds,
                                              25, // startX
                                              4, // startY
                                              9, // endX
                                              4, // endY
                                              0, // delay
                                              1, // duration
                                              UITweener.Method.BounceIn, // UITweener.Method easingMethod
            new EventDelegate(() => {
                // TODO: Replace with cinematic helper function
                foreach(GameObject gameObj in lightningCloudsToFlash) {
                    Animator animator = gameObj.GetComponent<Animator>();
                    AnimatorHelper animatorHelper = gameObj.GetComponent<AnimatorHelper>();
                    string animationToPlay = "Lightning";
                    animator.Play(animationToPlay);
                    animatorHelper.SetOnAnimationFinish(animationToPlay, () => { Debug.Log("lololol finished animation."); });
                }
            })); // EventDelegate eventDelegate
            Completed();
        }));
        waveStates.Add(CreateWaveState("Lightning Finish", () => {
            // Debug.Log("lightning animation complete");
            // Completed();
        }));
        bool performedCheck = false;
        waveStates.Add(CreateWaveState("Cinematic Complete", () => {
            if(!performedCheck) {
                performedCheck = true;
                Debug.Log("Cinematic Complete!");
            }
        }));
        
        InitializeWaveStates(waveStates.ToArray()); // End of Initialize
    }
    
// Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
//--------------------------------------------------------------------------
// Old Bathroom Bro Czar Enters To The Center of The Screen
//--------------------------------------------------------------------------
    public void TriggerBroCzarEnterAnimation() {
    }
    
    public void PerformBroCzarEnterAnimation() {
        // Debug.Log("performing start animation");
        // waitTime += Time.deltaTime;
        // if(waitTime > 1
        //         && !waitFinished) {
        // waitFinished = true;
        
        
        
        
        
        // FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);
        
        // BroGenerator.Instance.Pause();
        
        // broCzarReference.gameObject.SetActive(true);
        
        // LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        // GameObject firstQueueTile = entranceLineQueue.GetFirstQueueTile();
        
        
        
        
        
        // GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();
        
        // Vector3 startBroCzarPosition = new Vector3(lastQueueTile.transform.position.x,
        //                                            lastQueueTile.transform.position.y,
        //                                            broCzarReference.transform.position.z);
        
        // broCzarReference.SetLocation(startBroCzarPosition)
        // .SetTargetObjectAndTargetPosition(null, startBroCzarPosition);
        
        
        
        
        
        
        // List<GameObject> entranceToCenterMovementNodes = entranceLineQueue.GetQueueMovementNodes();
        // entranceToCenterMovementNodes.AddRange(AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
        //                                                                                 AStarManager.Instance.GetListCopyOfAllClosedNodes(),
        //                                                                                 BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(firstQueueTile.transform.position, true).GetComponent<BathroomTile>(),
        //                                                                                 BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>()));
        
        // broCzarReference.targetPathingReference.SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);
        // broCzarReference.targetPathingReference.SetMoveSpeed(25, 25);
        // broCzarReference.targetPathingReference.SetOnArrivalAtTargetPosition(() => {
        //     broCzarReference.SetState(BroState.Standing).SetFacing(Facing.Bottom);
        //     broCzarReference.animatorReference.Play("JumpUp");
        
        // });
        
        // LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;
        
        
        
        
        // }
    }
    
    public void FinishBroCzarEnterAnimation() {
        Debug.Log("finishing start animation");
    }
}