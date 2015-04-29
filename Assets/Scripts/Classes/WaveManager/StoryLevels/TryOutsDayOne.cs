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
        Bro oldBathroomBroCzarReference = null;
        GameObject firstBro = null;
        GameObject secondBro = null;
        GameObject thirdBro = null;
        GameObject fourthBro = null;
        GameObject fifthBro = null;
        
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
        // waveStates.Add(CreateWaveState("Configure Default Configuration", () => {
        //     TextboxManager.Instance.Hide(0);
        //     ConfirmationBoxManager.Instance.Hide(0);
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateDelayState("Delay", 1f));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("First Bro Entrance", () => {
        //     Debug.Log("Generating");
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("CloudEnter", () => {
        //     Vector3 leftStartPosition = (centerTile.gameObject.transform.position - (new Vector3(10, 0, 0)));
        //     Vector3 rightStartPosition = (centerTile.gameObject.transform.position + (new Vector3(10, 0, 0)));
        //     Vector3 centerStartPosition = (centerTile.gameObject.transform.position + (new Vector3(0, 10, 0)));
        
        //     leftCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
        //                                                          leftStartPosition)
        //                 .Build();
        //     rightCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
        //                                                           rightStartPosition)
        //                  .Build();
        //     centerCloud = CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("LightningCloud"),
        //                                                            centerStartPosition)
        //                   .Build();
        
        //     // Debug.Log("Clouds Entering");
        
        //     TweenExecutor.Position
        //     .Object(leftCloud)
        //     .StartPosition(leftStartPosition.x, leftStartPosition.y)
        //     .EndPosition(centerTile.gameObject.transform.position.x - 1f, leftStartPosition.y)
        //     .Duration(1)
        //     .Method(UITweener.Method.BounceIn)
        //     .Tween();
        
        //     TweenExecutor.Position
        //     .Object(centerCloud)
        //     .StartPosition(centerStartPosition.x, centerStartPosition.y)
        //     .EndPosition(centerTile.gameObject.transform.position.x, centerTile.gameObject.transform.position.y)
        //     .Duration(1)
        //     .Method(UITweener.Method.BounceIn)
        //     .Tween();
        
        //     TweenExecutor.Position
        //     .Object(rightCloud)
        //     .StartPosition(rightStartPosition.x, rightStartPosition.y)
        //     .EndPosition(centerTile.gameObject.transform.position.x + 1f, rightStartPosition.y)
        //     .Duration(1)
        //     .Method(UITweener.Method.BounceIn)
        //     .OnFinish(() => {
        //         string animationStateToPlay = "Lightning";
        //         CinematicHelper.Instance.SetObject(leftCloud)
        //         .PlayAnimation(animationStateToPlay);
        //         CinematicHelper.Instance.SetObject(centerCloud)
        //         .PlayAnimation(animationStateToPlay);
        //         CinematicHelper.Instance.SetObject(rightCloud)
        //         .PlayAnimation(animationStateToPlay)
        //         .SetOnAnimationFinish(animationStateToPlay, () => {
        //             // Debug.Log("Completed Lightning Playing");
        //             Completed();
        //         });
        //     })
        //     .Tween();
        
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Lightning Finish", () => {
        //     // Wait for previous animation to finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("SmokeAnimation", () => {
        //     // Smoke Created
        //     string animationStateToPlay = "Smoke";
        //     CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("EntranceSmoke"),
        //                                              centerTile.gameObject.transform.position)
        //     .PlayAnimation(animationStateToPlay, true)
        //     .SetOnAnimationFinish(animationStateToPlay, () => {
        //         Completed();
        //     })
        //     .Build();
        
        //     // OBBC Created
        //     oldBathroomBroCzar = CinematicHelper.Instance.CreateBro(NPCPrefabs.GetPath("OldBathroomBroCzar"),
        //                                                             centerTile.gameObject.transform.position)
        //                          .SetBroState(BroState.Standing)
        //                          .SetFacing(Facing.Top)
        //                          .Build();
        //     oldBathroomBroCzarReference = oldBathroomBroCzar.GetComponent<Bro>();
        
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("WaitForSmoke", () => {
        //     // Waiting
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Behold", () => {
        //     TextboxManager.Instance.SetText("Behold my glory!",
        //                                     "For, this is what your future may hold-",
        //                                     "...",
        //                                     // "But only, if you can prove that you are worthy of this position!",
        //                                     "No one's here ...");
        //     TextboxManager.Instance.Show();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("BeholdEnd", () => {
        //     if(TextboxManager.Instance.HasFinished()) {
        //         Completed();
        //     }
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC JumpUp Left", () => {
        //     GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
        //     float jumpHeight = 1f;
        //     float jumpStart = spriteToTween.transform.localPosition.y;
        //     float jumpApex = spriteToTween.transform.localPosition.y + jumpHeight;
        
        //     // Debug.Log("SpriteToTween: " + spriteToTween.name);
        //     TweenExecutor.Position
        //     .Object(spriteToTween)
        //     .StartPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //     .EndPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //     .Duration(0.15f)
        //     .Method(UITweener.Method.Linear)
        //     .Style(UITweener.Style.Once)
        //     .OnFinish(() => {
        //         oldBathroomBroCzarReference.SetFacing(Facing.Left);
        
        //         TweenExecutor.Position
        //         .Object(spriteToTween)
        //         .StartPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //         .EndPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //         .Duration(0.15f)
        //         .Method(UITweener.Method.Linear)
        //         .Style(UITweener.Style.Once)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC JumpUp Left End", () => {
        //     // Wait for Completed to be called
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC JumpUp Right", () => {
        //     GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
        //     float jumpHeight = 1f;
        //     float jumpStart = spriteToTween.transform.localPosition.y;
        //     float jumpApex = spriteToTween.transform.localPosition.y + jumpHeight;
        
        //     // Debug.Log("SpriteToTween: " + spriteToTween.name);
        //     TweenExecutor.Position
        //     .Object(spriteToTween)
        //     .StartPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //     .EndPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //     .Duration(0.15f)
        //     .Method(UITweener.Method.Linear)
        //     .Style(UITweener.Style.Once)
        //     .OnFinish(() => {
        //         oldBathroomBroCzarReference.SetFacing(Facing.Right);
        
        //         TweenExecutor.Position
        //         .Object(spriteToTween)
        //         .StartPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //         .EndPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //         .Duration(0.15f)
        //         .Method(UITweener.Method.Linear)
        //         .Style(UITweener.Style.Once)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("JumpUp Left End", () => {
        //     // Wait for Completed to be called
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Jump Up Front", () => {
        //     GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
        //     float jumpHeight = 1f;
        //     float jumpStart = spriteToTween.transform.localPosition.y;
        //     float jumpApex = spriteToTween.transform.localPosition.y + jumpHeight;
        
        //     // Debug.Log("SpriteToTween: " + spriteToTween.name);
        //     TweenExecutor.Position
        //     .Object(spriteToTween)
        //     .StartPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //     .EndPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //     .Duration(0.15f)
        //     .Method(UITweener.Method.Linear)
        //     .Style(UITweener.Style.Once)
        //     .OnFinish(() => {
        //         oldBathroomBroCzarReference.SetFacing(Facing.Bottom);
        
        //         TweenExecutor.Position
        //         .Object(spriteToTween)
        //         .StartPosition(spriteToTween.transform.localPosition.x, jumpApex)
        //         .EndPosition(spriteToTween.transform.localPosition.x, jumpStart)
        //         .Duration(0.15f)
        //         .Method(UITweener.Method.Linear)
        //         .Style(UITweener.Style.Once)
        //         .OnFinish(() => {
        //             Completed();
        //             TextboxManager.Instance.SetText("Ah, there's your stupid face.",
        //                                             " Wait... is this the kind of slob they're trying to pass to me as successor material?");
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Jump Up End", () => {
        //     // Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Jump Up End", () => {
        //     if(TextboxManager.Instance.HasFinished()) {
        //         ConfirmationBoxManager.Instance.Show()
        //         .SetText("Do you really think you've got what it takes to be the Bathroom Bro Czar?")
        //         .SetYesButtonText("Pipe down old man, I'm here for your job.")
        //         .SetNoButtonText("I can't go home.. Not after what happened...");
        //         Completed();
        //     }
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Confirmation Response", () => {
        //     if(ConfirmationBoxManager.Instance.WasYesSelected()) {
        //         GameObject objectToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
        //         PerformWaveStatesThenReturn(CreateWaveState("OBBC Confirmation Yes", () => {
        //             // Debug.Log("yes");
        //             ConfirmationBoxManager.Instance.Hide();
        //             TextboxManager.Instance.SetText("Oh, yeah bro?");
        //             Completed();
        
        //         }),
        //         CreateWaveState("End OBBC Yes Response", () => {
        //             if(TextboxManager.Instance.HasFinished()) {
        //                 TweenExecutor.Scale
        //                 .Object(objectToTween)
        //                 .StartScale(objectToTween.transform.localScale)
        //                 .EndScale(new Vector3(3, 3, 1))
        //                 .OnFinish(() => {
        //                     Debug.Log("Scale finished");
        //                     TextboxManager.Instance
        //                     .SetText("PROVE IT!")
        //                     .OnFinish(() => {
        //                         Debug.Log("Text Finished!");
        //                         TweenExecutor.Scale
        //                         .Object(objectToTween)
        //                         .StartScale(objectToTween.transform.localScale)
        //                         .EndScale(new Vector3(1, 1, 1))
        //                         .OnFinish(() => {
        //                             Completed();
        //                             Debug.Log("Scale down finished");
        //                         })
        //                         .Tween();
        //                     });
        //                 })
        //                 .Tween();
        //                 Completed();
        //             }
        //         }),
        //         CreateWaveState("OBBC Scale Up Finish", () => {
        //             // Waits for the scale up and down to finish
        //         }));
        
        //         Completed();
        //     }
        //     else if(ConfirmationBoxManager.Instance.WasNoSelected()) {
        //         PerformWaveStatesThenReturn(CreateWaveState("OBBC Confirmation No", () => {
        //             // Debug.Log("yes");
        //             ConfirmationBoxManager.Instance.Hide();
        //             TextboxManager.Instance.SetText("Oh. Oh. Hold Sec.",
        //                                             "Here's a coupon for the I don't care depot.",
        //                                             "They have some snivelers on sale, they might match you!");
        //             Completed();
        
        //         }),
        //         CreateWaveState("End OBBC No Response", () => {
        //             if(TextboxManager.Instance.HasFinished()) {
        //                 Completed();
        //             }
        //         }));
        
        //         Completed();
        //     }
        // }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Call Bros In", () => {
            TextboxManager.Instance.SetText("Alright, it's not just you I'm picking from. We gotta get your competition in here.",
                                            "THE REST OF YOU BRO MAGNUMS WAITING TO TRY OUT THIS ROUND GET IN HERE!")
            .OnFinish(() => {
                firstBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                           .EnterThroughLineQueue(0)
                           .MoveToTile(centerTile.tileX, centerTile.tileY)
                           .Build();
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Bros Enter", () => {
            // Wait for bros to enter
        }));
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
    
    public void FinishBroCzarEnterAnimation() {
        Debug.Log("finishing start animation");
    }
}