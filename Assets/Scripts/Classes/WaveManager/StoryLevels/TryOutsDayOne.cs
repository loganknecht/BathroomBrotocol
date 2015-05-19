#pragma warning disable 0219

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Debug.Log("performing start animation");
// if(TextboxManager.Instance.HasFinished()) {
// TriggerWaveFinish();
// }
public class TryOutsDayOne : WaveLogic, WaveLogicContract {

    public override void Awake()
    {   base.Awake(); }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
        Initialize();
    }
    
    public override void Initialize() {
        Bro broCzarReference = null;
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
        
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Configure Default Configuration", () => {
            TextboxManager.Instance.Hide(0f);
            LevelManager.Instance.HidePlayerButtons(0f);
            LevelManager.Instance.HideRotationButtons(0f);
            // ConfirmationBoxManager.Instance.Hide(float.Epsilon);
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateDelayState("Delay", 3f));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("First Bro Entrance", () => {
            Debug.Log("Generating");
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("CloudEnter", () => {
            Vector3 leftStartPosition = (centerTile.gameObject.transform.position - (new Vector3(10, 0, 0)));
            Vector3 rightStartPosition = (centerTile.gameObject.transform.position + (new Vector3(10, 0, 0)));
            Vector3 centerStartPosition = (centerTile.gameObject.transform.position + (new Vector3(0, 10, 0)));
            
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
            leftCloud.transform.position = leftStartPosition;
            Go.to(leftCloud.transform,
                  1f,
                  new GoTweenConfig()
                  .localPosition(new Vector3(centerTile.gameObject.transform.position.x - 1f, leftStartPosition.y, 0))
                  .setEaseType(GoEaseType.BounceOut));
                  
            centerCloud.transform.position = rightStartPosition;
            Go.to(centerCloud.transform,
                  1f,
                  new GoTweenConfig()
                  .localPosition(new Vector3(centerTile.gameObject.transform.position.x, centerTile.gameObject.transform.position.y))
                  .setEaseType(GoEaseType.BounceOut));
                  
            rightCloud.transform.position = centerStartPosition;
            Go.to(rightCloud.transform,
                  1f,
                  new GoTweenConfig()
                  .localPosition(new Vector3(centerTile.gameObject.transform.position.x + 1f, rightStartPosition.y))
                  .setEaseType(GoEaseType.BounceOut)
            .onComplete(complete => {
                string animationStateToPlay = "Lightning";
                CinematicHelper.Instance.Object(leftCloud)
                .PlayAnimation(animationStateToPlay);
                CinematicHelper.Instance.Object(centerCloud)
                .PlayAnimation(animationStateToPlay);
                CinematicHelper.Instance.Object(rightCloud)
                .PlayAnimation(animationStateToPlay)
                .OnAnimationFinish(animationStateToPlay, () => {
                    // Debug.Log("Completed Lightning Playing");
                    Completed();
                });
            }));
            
            Completed();
        }));
        //----------------------------------------------------------------------
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
                                 .CanBeSelected(false)
                                 .DisplaySpeechBubble(false)
                                 .Build();
            oldBathroomBroCzarReference = oldBathroomBroCzar.GetComponent<Bro>();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Behold", () => {
            TextboxManager.Instance
            .Show()
            .SetText("Behold my glory!",
                     "For, this is what your future may hold-",
                     "...",
                     "No one's here ...")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC JumpUp Start", () => {
            GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
            Vector3 jumpHeightOffset = new Vector3(0, 1, 0);
            
            GoTween jumpFacingTopUp = new GoTween(spriteToTween.transform,
                                                  0.15f,
                                                  new GoTweenConfig()
                                                  .localPosition(jumpHeightOffset, true));
            GoTween jumpFacingTopDown = new GoTween(spriteToTween.transform,
                                                    0.15f,
                                                    new GoTweenConfig()
                                                    .localPosition(-jumpHeightOffset, true)
            .onComplete(complete => {
                oldBathroomBroCzarReference.SetFacing(Facing.Left);
            }));
            
            GoTween jumpFacingLeftUp = new GoTween(spriteToTween.transform,
                                                   0.15f,
                                                   new GoTweenConfig()
                                                   .localPosition(jumpHeightOffset, true));
            GoTween jumpFacingLeftDown = new GoTween(spriteToTween.transform,
                                                     0.15f,
                                                     new GoTweenConfig()
                                                     .localPosition(-jumpHeightOffset, true)
            .onComplete(complete => {
                oldBathroomBroCzarReference.SetFacing(Facing.Right);
            }));
            
            GoTween jumpFacingRightUp = new GoTween(spriteToTween.transform,
                                                    0.15f,
                                                    new GoTweenConfig()
                                                    .localPosition(jumpHeightOffset, true));
            GoTween jumpFacingRightDown = new GoTween(spriteToTween.transform,
                                                      0.15f,
                                                      new GoTweenConfig()
                                                      .localPosition(-jumpHeightOffset, true)
            .onComplete(complete => {
                oldBathroomBroCzarReference.SetFacing(Facing.Bottom);
            }));
            
            GoTween jumpFacingBottomUp = new GoTween(spriteToTween.transform,
                                                     0.15f,
                                                     new GoTweenConfig()
                                                     .localPosition(jumpHeightOffset, true));
            GoTween jumpFacingBottomDown = new GoTween(spriteToTween.transform,
                                                       0.15f,
                                                       new GoTweenConfig()
                                                       .localPosition(-jumpHeightOffset, true));
                                                       
            GoTweenChain jumpChain = new GoTweenChain();
            jumpChain.append(jumpFacingTopUp)
            .append(jumpFacingTopDown)
            .append(jumpFacingLeftUp)
            .append(jumpFacingLeftDown)
            .append(jumpFacingRightUp)
            .append(jumpFacingRightDown)
            .append(jumpFacingBottomUp)
            .append(jumpFacingBottomDown)
            .setOnCompleteHandler(complete => {
                Completed();
            });
            jumpChain.play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Jump Up End", () => {
            TextboxManager.Instance.SetText("Ah, there's your stupid face.",
                                            "Wait... is this the kind of slob they're trying to pass to me as successor material?",
                                            "I can't believe this, you're so close to the bottom of the barrel that I can smell wood.")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        // //----------------------------------------------------------------------
        // PerformWaveStates(CreateWaveState("OBBC Confirmation", () => {
        //     ConfirmationBoxManager.Instance
        //     .Show()
        //     .BodyText("Do you really think you've got what it takes to be the Bathroom Bro Czar?")
        //     .YesButtonText("Pipe down old man, I'm here for your job.")
        //     .NoButtonText("I can't go home.. Not after what happened...");
        //     Completed();
        // }));
        // //----------------------------------------------------------------------
        // PerformWaveStates(CreateWaveState("OBBC Confirmation Response", () => {
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
        //                 float scaleDuration = 1f;
        //                 // Vector3 scaleAmount = new Vector3(2f, 2f, 0);
        //                 Go.to(objectToTween.transform,
        //                       scaleDuration,
        //                       new GoTweenConfig()
        //                       .scale(3)
        //                 .onComplete(scaleUpComplete => {
        //                     TextboxManager.Instance
        //                     .SetText("PROVE IT!")
        //                     .OnFinish(() => {
        //                         Go.to(objectToTween.transform,
        //                               scaleDuration,
        //                               new GoTweenConfig()
        //                               .scale(1)
        //                         .onComplete(scaleDownComplete => {
        //                             Completed();
        //                         }));
        //                     });
        //                 }));
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
        PerformWaveStatesAndWait(CreateWaveState("OBBC Call Bros In", () => {
            TextboxManager.Instance.SetText("Besides, it's not just you I'm picking from. We gotta get your competition in here.",
                                            "THE REST OF YOU BRO-MAGNONS WAITING TO TRY OUT THIS ROUND GET IN HERE!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Bros entering", () => {
            CameraManager.Instance.StartSmallCameraShake(5f);
            // CameraManager.Instance.StartMediumCameraShake(5f);
            // CameraManager.Instance.StartLargeCameraShake(5f);
            PerformWaveStatesThenReturn(
            CreateWaveState("First Bro Enter", () => {
                firstBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                           .MoveSpeed(10, 10)
                           .EnterThroughLineQueue(0)
                           .MoveToTile(centerTile.tileX - 2, centerTile.tileY - 1)
                           .CanBeSelected(false)
                           .DisplaySpeechBubble(false)
                           .Build();
                Completed();
            }),
            CreateDelayState("Second Bro Delay", 0.25f),
            CreateWaveState("Second Bro Enter", () => {
                secondBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                            .MoveSpeed(10, 10)
                            .EnterThroughLineQueue(0)
                            .MoveToTile(centerTile.tileX - 1, centerTile.tileY - 1)
                            .CanBeSelected(false)
                            .DisplaySpeechBubble(false)
                            .Build();
                Completed();
            }),
            CreateDelayState("Third Bro Delay", 0.25f),
            CreateWaveState("Third Bro Enter", () => {
                thirdBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                           .MoveSpeed(10, 10)
                           .EnterThroughLineQueue(0)
                           .MoveToTile(centerTile.tileX, centerTile.tileY - 1)
                           .CanBeSelected(false)
                           .DisplaySpeechBubble(false)
                           .Build();
                Completed();
            }),
            CreateDelayState("Fourth Bro Delay", 0.25f),
            CreateWaveState("Fourth Bro Enter", () => {
                fourthBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                            .MoveSpeed(10, 10)
                            .EnterThroughLineQueue(0)
                            .MoveToTile(centerTile.tileX + 1, centerTile.tileY - 1)
                            .CanBeSelected(false)
                            .DisplaySpeechBubble(false)
                            .Build();
                Completed();
            }),
            CreateDelayState("Fifth Bro Bro Delay", 0.25f),
            CreateWaveState("Fifth Bro Bro Enter", () => {
                fifthBro = CinematicHelper.Instance.CreateBro(BroType.GenericBro)
                           .MoveSpeed(10, 10)
                           .EnterThroughLineQueue(0)
                           .MoveToTile(centerTile.tileX + 2, centerTile.tileY - 1)
                           .CanBeSelected(false)
                           .DisplaySpeechBubble(false)
                .OnArrivalAtTargetPositionLogic(() => {
                    CameraManager.Instance.StopCameraShake();
                    Completed();
                })
                .Build();
                Completed();
            })
            );
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("OBBC Bros Enter Finish", () => {
            // Wait for bros to finish entering
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To First Bro", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX - 2, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Right);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("First Bro Review", () => {
            TextboxManager.Instance.SetText("Sweet Baby Chad! You are much too boring!",
                                            "Get out of here!",
                                            "Spinning bro fist!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Spinning Bro Fist First Bro", () => {
            Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
            GameObject broToFist = firstBro;
            broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
            
            GoTween punchForward = new GoTween(oldBathroomBroCzar.transform,
                                               0.1f,
                                               new GoTweenConfig()
                                               .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y, oldBathroomBroCzar.transform.position.z))
            .onComplete(broPunchForward => {
                Go.to(broToFist.transform,
                      1f,
                      new GoTweenConfig()
                      .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y - 20, broToFist.transform.position.z)));
            }));
            GoTween punchBackward = new GoTween(oldBathroomBroCzar.transform,
                                                0.1f,
                                                new GoTweenConfig()
                                                .localPosition(obbcStartPosition)
            .onComplete(broPunchBackward => {
                Completed();
            }));
            GoTweenChain broPunchChain = new GoTweenChain();
            broPunchChain
            .append(punchForward)
            .append(punchBackward)
            .play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To Second Bro", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX - 1, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Right);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Second Bro Review", () => {
            TextboxManager.Instance.SetText("Oh no. No, no, no.",
                                            "You're a slob! How could I even trust you to take care of a bathroom, when you can't take care of yourself!?",
                                            "Spinning bro fist!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Spinning Bro Fist Second Bro", () => {
            Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
            GameObject broToFist = secondBro;
            broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
            
            GoTween punchForward = new GoTween(oldBathroomBroCzar.transform,
                                               0.1f,
                                               new GoTweenConfig()
                                               .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y, oldBathroomBroCzar.transform.position.z))
            .onComplete(broPunchForward => {
                Go.to(broToFist.transform,
                      1f,
                      new GoTweenConfig()
                      .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y - 20, broToFist.transform.position.z)));
            }));
            GoTween punchBackward = new GoTween(oldBathroomBroCzar.transform,
                                                0.1f,
                                                new GoTweenConfig()
                                                .localPosition(obbcStartPosition)
            .onComplete(broPunchBackward => {
                Completed();
            }));
            GoTweenChain broPunchChain = new GoTweenChain();
            broPunchChain
            .append(punchForward)
            .append(punchBackward)
            .play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To Third Bro", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Right);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Third Bro Review", () => {
            TextboxManager.Instance.SetText("...",
                                            "Bro, you're clearly too shy to lead.",
                                            "Get out here!",
                                            "Bro fist!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Spinning Bro Fist Third Bro", () => {
            Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
            GameObject broToFist = thirdBro;
            broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
            
            GoTween punchForward = new GoTween(oldBathroomBroCzar.transform,
                                               0.1f,
                                               new GoTweenConfig()
                                               .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y, oldBathroomBroCzar.transform.position.z))
            .onComplete(broPunchForward => {
                Go.to(broToFist.transform,
                      1f,
                      new GoTweenConfig()
                      .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y - 20, broToFist.transform.position.z)));
            }));
            GoTween punchBackward = new GoTween(oldBathroomBroCzar.transform,
                                                0.1f,
                                                new GoTweenConfig()
                                                .localPosition(obbcStartPosition)
            .onComplete(broPunchBackward => {
                Completed();
            }));
            GoTweenChain broPunchChain = new GoTweenChain();
            broPunchChain
            .append(punchForward)
            .append(punchBackward)
            .play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To Fourth Bro", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX + 1, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Right);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Fourth Bro Review", () => {
            TextboxManager.Instance.SetText("By brodin's beard, you stink! No, just no!",
                                            "Bro fist!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Spinning Bro Fist Fourth Bro", () => {
            Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
            GameObject broToFist = fourthBro;
            broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
            
            GoTween punchForward = new GoTween(oldBathroomBroCzar.transform,
                                               0.1f,
                                               new GoTweenConfig()
                                               .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y, oldBathroomBroCzar.transform.position.z))
            .onComplete(broPunchForward => {
                Go.to(broToFist.transform,
                      1f,
                      new GoTweenConfig()
                      .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y - 20, broToFist.transform.position.z)));
            }));
            GoTween punchBackward = new GoTween(oldBathroomBroCzar.transform,
                                                0.1f,
                                                new GoTweenConfig()
                                                .localPosition(obbcStartPosition)
            .onComplete(broPunchBackward => {
                Completed();
            }));
            GoTweenChain broPunchChain = new GoTweenChain();
            broPunchChain
            .append(punchForward)
            .append(punchBackward)
            .play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To Fifth Bro", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX + 2, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Right);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Fifth Bro Review", () => {
            TextboxManager.Instance.SetText("How did someone this drunk, make it into these try outs?!?",
                                            "Get out!",
                                            "Bro fist!")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Spinning Bro Fist Fifth Bro", () => {
            Vector3 obbcStartPosition = oldBathroomBroCzar.transform.position;
            GameObject broToFist = fifthBro;
            broToFist.GetComponent<Bro>().ToggleTargetPathing(false);
            
            GoTween punchForward = new GoTween(oldBathroomBroCzar.transform,
                                               0.1f,
                                               new GoTweenConfig()
                                               .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y, oldBathroomBroCzar.transform.position.z))
            .onComplete(broPunchForward => {
                Go.to(broToFist.transform,
                      1f,
                      new GoTweenConfig()
                      .localPosition(new Vector3(broToFist.transform.position.x, broToFist.transform.position.y - 20, broToFist.transform.position.z)));
            }));
            GoTween punchBackward = new GoTween(oldBathroomBroCzar.transform,
                                                0.1f,
                                                new GoTweenConfig()
                                                .localPosition(obbcStartPosition)
            .onComplete(broPunchBackward => {
                Completed();
            }));
            GoTweenChain broPunchChain = new GoTweenChain();
            broPunchChain
            .append(punchForward)
            .append(punchBackward)
            .play();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Move To Center Tile", () => {
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .MoveSpeed(2, 2)
            .MoveToTile(centerTile.tileX, centerTile.tileY)
            .BroState(BroState.MovingToTargetObject)
            .OnArrivalAtTargetPositionLogic(() => {
                oldBathroomBroCzar.GetComponent<Bro>()
                .SetState(BroState.Standing)
                .SetFacing(Facing.Bottom);
                Completed();
            })
            .Build();
            Completed();
        }));
        //----------------------------------------------------------------------
        // WaveState.WaveStateLogic playerConfirmTheyUnderstand = null;
        // playerConfirmTheyUnderstand = delegate() {
        //     TextboxManager.Instance
        //     .SetText("By all that is broly, I can't believe you're really the best we've got this round...",
        //              "...",
        //              "This is just the preliminary round. Meaning, don't get too excited ya schlub.")
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
        // ----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Player Confirmed They Understand", () => {
            TextboxManager.Instance
            .SetText("Alright... Alright. Alright!",
                     "Show me what you've got...",
                     "Just get these bros in and out of the restroom.")
            .OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        // ----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Exits", () => {
            TextboxManager.Instance.Hide();
            CinematicHelper.Instance
            .Object(oldBathroomBroCzar)
            .BroState(BroState.MovingToTargetObject)
            .ExitThroughLineQueue(0)
            .OnArrivalAtTargetPositionLogic(() => {
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Ready Set Bro!", () => {
            LevelManager.Instance.HideJanitorOverlay();
            ReadySetBro.Instance.StartAnimation().OnFinish(() => {
                Completed();
            });
            Completed();
        }));
        // ----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("First Wave Logic", () => {
            Debug.Log("First Wave Generating!");
            
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
            
            BroDistributionObject firstWave = new BroDistributionObject(0,
                                                                        10,
                                                                        3,
                                                                        DistributionType.LinearIn,
                                                                        DistributionSpacing.Uniform,
                                                                        broProbabilities,
                                                                        entranceQueueProbabilities);
            firstWave.broConfigurer
            .SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
            .SetXMoveSpeed(BroDistribution.AllBros, 2.5f, 2.5f)
            .SetYMoveSpeed(BroDistribution.AllBros , 2.5f, 2.5f)
            .SetFightProbability(BroDistribution.AllBros, 0f, 0f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                firstWave
            });
            Completed();
        }));
        // ----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Wait For First Wave Logic Finished", () => {
            if(BroGenerator.Instance.HasFinished()
                && BroManager.Instance.NoBrosInRestroom()) {
                Completed();
            }
        }));
        // ----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Second Wave Logic", () => {
            Debug.Log("Second Wave Generating!");
            
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
            
            BroDistributionObject firstWave = new BroDistributionObject(0,
                                                                        20,
                                                                        5,
                                                                        DistributionType.LinearIn,
                                                                        DistributionSpacing.Uniform,
                                                                        broProbabilities,
                                                                        entranceQueueProbabilities);
            firstWave.broConfigurer
            .SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
            .SetXMoveSpeed(BroDistribution.AllBros, 2.5f, 2.5f)
            .SetYMoveSpeed(BroDistribution.AllBros , 2.5f, 2.5f)
            .SetFightProbability(BroDistribution.AllBros, 0f, 0f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                firstWave
            });
            Completed();
        }));
        // ----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Wait For Second Wave Logic Finished", () => {
            if(BroGenerator.Instance.HasFinished()
                && BroManager.Instance.NoBrosInRestroom()) {
                Completed();
            }
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("OBBC Taunt", () => {
            LevelManager.Instance.ShowJanitorOverlay();
            TextboxManager.Instance
            .Show()
            .SetText("Ohhhhh wow, look at this, we got brotiful mind over here.",
                     "I'll admit, you're doing ok. But here is where it starts to get hectic.",
                     "If you can keep up with this we'll call you back tomorrow.",
                     "Here we bro!")
            .OnFinish(() => {
                TextboxManager.Instance.Hide();
                LevelManager.Instance.HideJanitorOverlay();
                Completed();
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Final Wave Logic", () => {
            Debug.Log("Final Wave Generating!");
            
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() {
                { 0, 0.5f },
                { 1, 0.5f }
            };
            
            BroDistributionObject firstWave = new BroDistributionObject(0,
                                                                        20,
                                                                        1,
                                                                        DistributionType.LinearIn,
                                                                        DistributionSpacing.Uniform,
                                                                        broProbabilities,
                                                                        entranceQueueProbabilities);
            firstWave.broConfigurer
            .SetReliefType(BroDistribution.AllBros, new ReliefRequired[] { ReliefRequired.Pee, ReliefRequired.Poop })
            .SetXMoveSpeed(BroDistribution.AllBros, 2.5f, 2.5f)
            .SetYMoveSpeed(BroDistribution.AllBros , 2.5f, 2.5f)
            .SetFightProbability(BroDistribution.AllBros, 0f, 0f)
            .SetLineQueueSkipType(BroDistribution.AllBros, true)
            .SetChooseObjectOnLineSkip(BroDistribution.AllBros, false)
            .SetStartRoamingOnArrivalAtBathroomObjectInUse(BroDistribution.AllBros, false)
            .SetChooseObjectOnRelief(BroDistribution.AllBros, false);
            
            BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                firstWave
            });
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("End Textbox", () => {
            if(BroManager.Instance.NoBrosInRestroom()
                && BroGenerator.Instance.HasFinished()) {
                LevelManager.Instance.ShowJanitorOverlay();
                TextboxManager.Instance
                .Show()
                .SetText("Okay. I acknowledge that you've got some swag.",
                         "Come back tomorrow for the next try-outs and we'll see if you can actually handle what the role of bathroom bro czar actually entails.")
                .OnFinish(() => {
                    TextboxManager.Instance.Hide();
                    LevelManager.Instance.HideJanitorOverlay();
                    
                    Completed();
                });
                Completed();
            }
        }));
        //----------------------------------------------------------------------
        PerformWaveStatesAndWait(CreateWaveState("Cinematic Complete", () => {
            Debug.Log("Cinematic Complete!");
            Finished();
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