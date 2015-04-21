using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        
        BathroomTile centerTile = BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>();
        BathroomTile middleLeftTile = BathroomTileMap.Instance.GetMiddleLeftTileGameObject().GetComponent<BathroomTile>();
        BathroomTile topCenterTile = BathroomTileMap.Instance.GetTopCenterTileGameObject().GetComponent<BathroomTile>();
        BathroomTile middleRightTile = BathroomTileMap.Instance.GetMiddleRightTileGameObject().GetComponent<BathroomTile>();
        BathroomTile bottomCenterTile = BathroomTileMap.Instance.GetBottomCenterTileGameObject().GetComponent<BathroomTile>();
        
        GameObject leftCloud = null;
        GameObject rightCloud = null;
        GameObject centerCloud = null;
        
        GameObject oldBathroomBroCzar = null;
        // GameObject firstBro = null;
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
        //----------------------------------------------------------------------
        waveStates.Add(CreateDelayState("Delay", 1f));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("First Bro Entrance", () => {
            Debug.Log("Generating");
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("CloudEnter", () => {
            Vector3 leftStartPosition = (centerTile.gameObject.transform.position - (new Vector3(10, 0, 0)));
            Vector3 rightStartPosition = (centerTile.gameObject.transform.position + (new Vector3(10, 0, 0)));
            Vector3 centerStartPosition = (centerTile.gameObject.transform.position + (new Vector3(0, 10, 0)));
            // Debug.Log(AnimationPrefabs.GetPath("LightningCloud"));
            leftCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
                                                                 leftStartPosition)
                        .Build();
            rightCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
                                                                  rightStartPosition)
                         .Build();
            centerCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
                                                                   centerStartPosition)
                          .Build();
            // Debug.Log("Clouds Entering");
            TweenExecutor.Position
            .Object(leftCloud)
            .StartPosition(leftStartPosition.x, leftStartPosition.y)
            .EndPosition(centerTile.gameObject.transform.position.x - 1f, leftStartPosition.y)
            .Duration(1f)
            .Method(UITweener.Method.BounceIn)
            .Tween();
            
            TweenExecutor.Position
            .Object(centerCloud)
            .StartPosition(centerStartPosition.x, centerStartPosition.y)
            .EndPosition(centerTile.gameObject.transform.position.x, centerTile.gameObject.transform.position.y)
            .Duration(1)
            .Method(UITweener.Method.BounceIn)
            .Tween();
            
            TweenExecutor.Position
            .Object(rightCloud)
            .StartPosition(rightStartPosition.x, rightStartPosition.y)
            .EndPosition(centerTile.gameObject.transform.position.x + 1f, rightStartPosition.y)
            .Duration(1)
            .Method(UITweener.Method.BounceIn)
            .OnFinish(new EventDelegate(() => {
                string animationStateToPlay = "Lightning";
                CinematicHelper.Instance.SetObject(leftCloud)
                .PlayAnimation(animationStateToPlay);
                CinematicHelper.Instance.SetObject(centerCloud)
                .PlayAnimation(animationStateToPlay);
                CinematicHelper.Instance.SetObject(rightCloud)
                .PlayAnimation(animationStateToPlay)
                .SetOnAnimationFinish(animationStateToPlay, () => {
                    // Debug.Log("Completed Lightning Playing");
                    Completed();
                });
            }))
            .Tween();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("Lightning Finish", () => {
            // Wait for previous animation to finish
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("SmokeAnimation", () => {
            // Smoke Created
            string animationStateToPlay = "Smoke";
            CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("EntranceSmoke"),
                                                     centerTile.gameObject.transform.position)
            .PlayAnimation(animationStateToPlay, true)
            .SetOnAnimationFinish(animationStateToPlay, () => {
                Completed();
            })
            .Build();
            
            // OBBC Created
            oldBathroomBroCzar = CinematicHelper.Instance.CreateBro(NPCPrefabs.GetPath("OldBathroomBroCzar"),
                                                                    centerTile.gameObject.transform.position)
                                 // .SetBroState(BroState.Standing)
                                 // .SetFacing(Facing.Top)
                                 .Build();
                                 
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("WaitForSmoke", () => {
            // Waiting
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Jumped", () => {
            GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "BroSprite");
            // GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
            Debug.Log("SpriteToTween: " + spriteToTween.name);
            TweenExecutor.Position
            .Object(spriteToTween)
            .StartPosition(spriteToTween.transform.position.x, spriteToTween.transform.position.y)
            .EndPosition(spriteToTween.transform.position.x, spriteToTween.transform.position.y + 1)
            .Duration(1)
            .Method(UITweener.Method.BounceIn)
            .Tween();
            
            // string animationStateToPlay = "JumpUp";
            // CinematicHelper.Instance.SetObject(oldBathroomBroCzar)
            // .PlayAnimation(animationStateToPlay, true)
            // .Build();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Appears", () => {
            // TextboxManager.Instance.SetText("asdfa",
            //                                 "alskdfjalsd");
            // Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("Cinematic Complete", () => {
            Debug.Log("Cinematic Complete!");
            Completed();
        }));
        //----------------------------------------------------------------------
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