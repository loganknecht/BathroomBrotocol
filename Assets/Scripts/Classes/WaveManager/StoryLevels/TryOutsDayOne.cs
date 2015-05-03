#pragma warning disable 0219

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
        
        // //----------------------------------------------------------------------
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
        //         CinematicHelper.Instance.Object(leftCloud)
        //         .PlayAnimation(animationStateToPlay);
        //         CinematicHelper.Instance.Object(centerCloud)
        //         .PlayAnimation(animationStateToPlay);
        //         CinematicHelper.Instance.Object(rightCloud)
        //         .PlayAnimation(animationStateToPlay)
        //         .OnAnimationFinish(animationStateToPlay, () => {
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
        PerformWaveStatesAndWait(CreateWaveState("SmokeAnimation", () => {
            // Smoke Created
            string animationStateToPlay = "Smoke";
            CinematicHelper.Instance.CreateAnimation(AnimationPrefabs.GetPath("EntranceSmoke"),
                                                     centerTile.gameObject.transform.position)
            .PlayAnimation(animationStateToPlay, true)
            .OnAnimationFinish(animationStateToPlay, () => {
                Completed();
            })
            .Build();
            
            // OBBC Created
            oldBathroomBroCzar = CinematicHelper.Instance.CreateBro(NPCPrefabs.GetPath("OldBathroomBroCzar"),
                                                                    centerTile.gameObject.transform.position)
                                 .BroState(BroState.Standing)
                                 .Facing(Facing.Top)
                                 .Build();
            oldBathroomBroCzarReference = oldBathroomBroCzar.GetComponent<Bro>();
            
            Completed();
        }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Behold", () => {
        //     TextboxManager.Instance.SetText(
        //         "Lorem ipsum  dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
        //         "Behold my glory!",
        //         "For, this is what your future may hold-",
        //         "...",
        //         // "But only, if you can prove that you are worthy of this position!",
        //         "No one's here ...");
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
        //         ConfirmationBoxManager.Instance
        //         .Show()
        //         .BodyText("Do you really think you've got what it takes to be the Bathroom Bro Czar?")
        //         .YesButtonText("Pipe down old man, I'm here for your job.")
        //         .NoButtonText("I can't go home.. Not after what happened...");
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
        //                     TextboxManager.Instance
        //                     .SetText("PROVE IT!")
        //                     .OnFinish(() => {
        //                         TweenExecutor.Scale
        //                         .Object(objectToTween)
        //                         .StartScale(objectToTween.transform.localScale)
        //                         .EndScale(new Vector3(1, 1, 1))
        //                         .OnFinish(() => {
        //                             Completed();
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
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Call Bros In", () => {
        //     TextboxManager.Instance.SetText("Besides, it's not just you I'm picking from. We gotta get your competition in here.",
        //                                     "THE REST OF YOU BRO-MAGNONS WAITING TO TRY OUT THIS ROUND GET IN HERE!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Wait For Call Bros In", () => {
        //     // wait for the player to finish the text
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bros entering", () => {
        //     PerformWaveStatesThenReturn(
        //     CreateWaveState("First Bro Enter", () => {
        //         firstBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
        //                    .MoveSpeed(10, 10)
        //                    .EnterThroughLineQueue(0)
        //                    .MoveToTile(centerTile.tileX - 2, centerTile.tileY - 1)
        //                    .Build();
        //         Completed();
        //     })
        //     , CreateDelayState("Second Bro Delay", 0.25f)
        //     , CreateWaveState("Second Bro Enter", () => {
        //         secondBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
        //                     .MoveSpeed(10, 10)
        //                     .EnterThroughLineQueue(0)
        //                     .MoveToTile(centerTile.tileX - 1, centerTile.tileY - 1)
        //                     .Build();
        //         Completed();
        //     })
        //     , CreateDelayState("Third Bro Delay", 0.25f)
        //     , CreateWaveState("Third Bro Enter", () => {
        //         thirdBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
        //                    .MoveSpeed(10, 10)
        //                    .EnterThroughLineQueue(0)
        //                    .MoveToTile(centerTile.tileX, centerTile.tileY - 1)
        //                    .Build();
        //         Completed();
        //     })
        //     , CreateDelayState("Fourth Bro Delay", 0.25f)
        //     , CreateWaveState("Fourth Bro Enter", () => {
        //         fourthBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
        //                     .MoveSpeed(10, 10)
        //                     .EnterThroughLineQueue(0)
        //                     .MoveToTile(centerTile.tileX + 1, centerTile.tileY - 1)
        //                     .Build();
        //         Completed();
        //     })
        //     , CreateDelayState("Fifth Bro Bro Delay", 0.25f)
        //     , CreateWaveState("Fifth Bro Bro Enter", () => {
        //         fifthBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
        //                    .MoveSpeed(10, 10)
        //                    .EnterThroughLineQueue(0)
        //                    .MoveToTile(centerTile.tileX + 2, centerTile.tileY - 1)
        //         .OnArrivalAtTargetPositionLogic(() => {
        //             Completed();
        //         })
        //         .Build();
        //         Completed();
        //     })
        //     );
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Bros Enter Finish", () => {
        //     // Wait for bros to finish entering
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To First Bro", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX - 2, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Right);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Waiting", () => {
        //     // Wait for OBBC to arrive
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("First Bro Review", () => {
        //     TextboxManager.Instance.SetText("Sweet Baby Chad! You are much too boring!",
        //                                     "Get out of here!",
        //                                     "Spinning bro fist!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("First Bro Review End", () => {
        //     // Wait for Text to Finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Spinning Bro Fist First Bro", () => {
        //     Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
        //     GameObject broToFist = firstBro;
        //     broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
        
        //     TweenExecutor.Position
        //     .Object(oldBathroomBroCzar)
        //     .StartPosition(obbcStartPosition)
        //     .EndPosition(broToFist.transform.position.x, broToFist.transform.position.y)
        //     .Duration(0.1f)
        //     .OnFinish(() => {
        //         TweenExecutor.Position
        //         .Object(oldBathroomBroCzar)
        //         .StartPosition(oldBathroomBroCzar.transform.position)
        //         .EndPosition(obbcStartPosition)
        //         .Duration(0.1f)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        
        //         TweenExecutor.Position
        //         .Object(broToFist)
        //         .StartPosition(broToFist.transform.position)
        //         .EndPosition(broToFist.transform.position.x, -20)
        //         .Duration(1f)
        //         .OnFinish(() => {
        //             Destroy(firstBro);
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bro Fist First Bro Punch End", () => {
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Second Bro", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX - 1, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Right);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Waiting", () => {
        //     // Wait for OBBC to arrive
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Second Bro Review", () => {
        //     TextboxManager.Instance.SetText("Oh no. No, no, no.",
        //                                     "You're a slob! How could I even trust you to take of a bathroom, when you can't take care of yourself!?",
        //                                     "Spinning bro fist!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Second Bro Review End", () => {
        //     // Wait for Text to Finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Spinning Bro Fist Second Bro", () => {
        //     Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
        //     GameObject broToFist = secondBro;
        //     broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
        
        //     TweenExecutor.Position
        //     .Object(oldBathroomBroCzar)
        //     .StartPosition(obbcStartPosition)
        //     .EndPosition(broToFist.transform.position.x, broToFist.transform.position.y)
        //     .Duration(0.1f)
        //     .OnFinish(() => {
        //         TweenExecutor.Position
        //         .Object(oldBathroomBroCzar)
        //         .StartPosition(oldBathroomBroCzar.transform.position)
        //         .EndPosition(obbcStartPosition)
        //         .Duration(0.1f)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        
        //         TweenExecutor.Position
        //         .Object(broToFist)
        //         .StartPosition(broToFist.transform.position)
        //         .EndPosition(broToFist.transform.position.x, -20)
        //         .Duration(1f)
        //         .OnFinish(() => {
        //             Destroy(firstBro);
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bro Fist Second Bro Punch End", () => {
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Third Bro", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Right);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Waiting", () => {
        //     // Wait for OBBC to arrive
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Third Bro Review", () => {
        //     TextboxManager.Instance.SetText("...",
        //                                     "Bro, you're clearly too shy to lead.",
        //                                     "Get out here!",
        //                                     "Bro fist!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Third Bro Review End", () => {
        //     // Wait for Text to Finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Spinning Bro Fist Third Bro", () => {
        //     Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
        //     GameObject broToFist = thirdBro;
        //     broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
        
        //     TweenExecutor.Position
        //     .Object(oldBathroomBroCzar)
        //     .StartPosition(obbcStartPosition)
        //     .EndPosition(broToFist.transform.position.x, broToFist.transform.position.y)
        //     .Duration(0.1f)
        //     .OnFinish(() => {
        //         TweenExecutor.Position
        //         .Object(oldBathroomBroCzar)
        //         .StartPosition(oldBathroomBroCzar.transform.position)
        //         .EndPosition(obbcStartPosition)
        //         .Duration(0.1f)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        
        //         TweenExecutor.Position
        //         .Object(broToFist)
        //         .StartPosition(broToFist.transform.position)
        //         .EndPosition(broToFist.transform.position.x, -20)
        //         .Duration(1f)
        //         .OnFinish(() => {
        //             Destroy(firstBro);
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bro Fist Third Bro Punch End", () => {
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Fourth Bro", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX + 1, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Right);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Waiting", () => {
        //     // Wait for OBBC to arrive
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Fourth Bro Review", () => {
        //     TextboxManager.Instance.SetText("By brodin's beard, you stink! No, just no!",
        //                                     "Bro fist!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Fourth Bro Review End", () => {
        //     // Wait for Text to Finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Spinning Bro Fist Fourth Bro", () => {
        //     Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
        //     GameObject broToFist = fourthBro;
        //     broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
        
        //     TweenExecutor.Position
        //     .Object(oldBathroomBroCzar)
        //     .StartPosition(obbcStartPosition)
        //     .EndPosition(broToFist.transform.position.x, broToFist.transform.position.y)
        //     .Duration(0.1f)
        //     .OnFinish(() => {
        //         TweenExecutor.Position
        //         .Object(oldBathroomBroCzar)
        //         .StartPosition(oldBathroomBroCzar.transform.position)
        //         .EndPosition(obbcStartPosition)
        //         .Duration(0.1f)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        
        //         TweenExecutor.Position
        //         .Object(broToFist)
        //         .StartPosition(broToFist.transform.position)
        //         .EndPosition(broToFist.transform.position.x, -20)
        //         .Duration(1f)
        //         .OnFinish(() => {
        //             Destroy(firstBro);
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bro Fist Fourth Bro Punch End", () => {
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Fifth Bro", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX + 2, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Right);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Waiting", () => {
        //     // Wait for OBBC to arrive
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Fifth Bro Review", () => {
        //     TextboxManager.Instance.SetText("How did someone this drunk, make it into these try outs.",
        //                                     "Get out!",
        //                                     "Bro fist!")
        //     .OnFinish(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Fifth Bro Review End", () => {
        //     // Wait for Text to Finish
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Spinning Bro Fist Fifth Bro", () => {
        //     Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
        //     GameObject broToFist = fifthBro;
        //     broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
        
        //     TweenExecutor.Position
        //     .Object(oldBathroomBroCzar)
        //     .StartPosition(obbcStartPosition)
        //     .EndPosition(broToFist.transform.position.x, broToFist.transform.position.y)
        //     .Duration(0.1f)
        //     .OnFinish(() => {
        //         TweenExecutor.Position
        //         .Object(oldBathroomBroCzar)
        //         .StartPosition(oldBathroomBroCzar.transform.position)
        //         .EndPosition(obbcStartPosition)
        //         .Duration(0.1f)
        //         .OnFinish(() => {
        //             Completed();
        //         })
        //         .Tween();
        
        //         TweenExecutor.Position
        //         .Object(broToFist)
        //         .StartPosition(broToFist.transform.position)
        //         .EndPosition(broToFist.transform.position.x, -20)
        //         .Duration(1f)
        //         .OnFinish(() => {
        //             Destroy(firstBro);
        //         })
        //         .Tween();
        //     })
        //     .Tween();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("Bro Fist Fifth Bro Punch End", () => {
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Center Tile", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .MoveSpeed(2, 2)
        //     .MoveToTile(centerTile.tileX, centerTile.tileY)
        //     .BroState(BroState.MovingToTargetObject)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         oldBathroomBroCzar.GetComponent<Bro>()
        //         .SetState(BroState.Standing)
        //         .SetFacing(Facing.Bottom);
        //         Completed();
        //     })
        //     .Build();
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // waveStates.Add(CreateWaveState("OBBC Move To Center Tile End", () => {
        //     // Wait for finish move
        // }));
        // //----------------------------------------------------------------------
        // WaveState.WaveStateLogic playerConfirmTheyUnderstand = null;
        // playerConfirmTheyUnderstand = delegate() {
        //     TextboxManager.Instance
        //     .SetText("By all that is broly, I can't believe you're really the best we've got this round...",
        //              "...",
        //              "This is just the preliminary round.")
        //     .OnFinish(() => {
        //         ConfirmationBoxManager.Instance.Show()
        //         .BodyText("So, all you have to prove to me this round is just that you can handle some simple bros.\nYou dig?")
        //         .YesButtonText("Yeah, yeah, I got this.")
        //         .NoButtonText("Wait what?")
        //         .OnSelection(() => {
        //             if(ConfirmationBoxManager.Instance.WasYesSelected()) {
        //                 Completed();
        //             }
        //             else if(ConfirmationBoxManager.Instance.WasNoSelected()) {
        //                 PerformWaveStatesThenReturn(CreateWaveState("Redo Confirmation.", playerConfirmTheyUnderstand),
        //                 CreateWaveState("Redo Confirmation Wait", () => { /* wait */ }));
        //                 Completed();
        //             }
        //             ConfirmationBoxManager.Instance
        //             .Reset()
        //             .Hide();
        //         });
        //     });
        //     Completed();
        // };
        
        // PerformWaveStatesAndWait(CreateWaveState("OBBC Response Acknowledgement", playerConfirmTheyUnderstand));
        // // ----------------------------------------------------------------------
        // PerformWaveStatesAndWait(CreateWaveState("Player Confirms They Understand", () => {
        //     TextboxManager.Instance
        //     .SetText("Alright... Alright. Alright!",
        //              "Show me what you've got...",
        //              "Just get these bros in and out of the restroom.")
        //     .OnFinish(() => {
        //         TextboxManager.Instance
        //         .Hide();
        //         Completed();
        //     });
        //     Completed();
        // }));
        //----------------------------------------------------------------------
        // PerformWaveStatesAndWait(CreateWaveState("OBBC Exits", () => {
        //     CinematicHelper.Instance
        //     .Object(oldBathroomBroCzar)
        //     .BroState(BroState.MovingToTargetObject)
        //     .ExitThroughLineQueue(0)
        //     .OnArrivalAtTargetPositionLogic(() => {
        //         Completed();
        //     });
        //     Completed();
        // }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("First Wave Logic", () => {
            Debug.Log("First Wave Generating!");
            
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
            
            BroDistributionObject firstWave = new BroDistributionObject(0, 5, 1, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
            firstWave.broConfigurer
            .SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
            .SetXMoveSpeed(BroDistribution.AllBros, 1.5f, 1.5f)
            .SetYMoveSpeed(BroDistribution.AllBros , 1.5f, 1.5f)
            .SetFightProbability(BroDistribution.AllBros, 0.15f, 0.15f)
            .SetModifyFightProbabilityUsingScoreRatio(BroDistribution.AllBros, false)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Exit, 0f, 0f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.HandDryer, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Sink, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Stall, 2f, 2f)
            .SetBathroomObjectOccupationDuration(BroDistribution.AllBros, BathroomObjectType.Urinal, 2f, 2f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, true);
            // Fart Generator if the bro has it (TileBlocker Properties)
            firstWave.fartGeneratorConfigurer
            .SetProbability(BroDistribution.AllBros, 2f, 2f)
            .SetDuration(BroDistribution.AllBros, 2f, 2f)
            .SetDurationIsStochastic(BroDistribution.AllBros, false)
            .SetMinDuration(BroDistribution.AllBros, 2f, 2f)
            .SetMaxDuration(BroDistribution.AllBros, 2f, 2f)
            .SetGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
            .SetMinGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, 2f, 2f);
            // Vomit Generator if the bro has it (TileBlocker Properties)
            firstWave.vomitGeneratorConfigurer
            .SetProbability(BroDistribution.AllBros, 1f, 1f)
            .SetGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetGenerationFrequencyIsStochastic(BroDistribution.AllBros, false)
            .SetMinGenerationFrequency(BroDistribution.AllBros, 2f, 2f)
            .SetMaxGenerationFrequency(BroDistribution.AllBros, 2f, 2f);
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                firstWave
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Cinematic Complete", () => {
            Debug.Log("Cinematic Complete!");
            Completed();
        }));
        //----------------------------------------------------------------------
        InitializeWaveStates();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
    
    public void FinishBroCzarEnterAnimation() {
        Debug.Log("finishing start animation");
    }
}