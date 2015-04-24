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
        waveStates.Add(CreateWaveState("Configure Default Configuration", () => {
            TextboxManager.Instance.Hide(0);
            Completed();
        }));
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
            .Duration(1)
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
            .OnFinish(() => {
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
            })
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
                                 .SetBroState(BroState.Standing)
                                 .SetFacing(Facing.Top)
                                 .Build();
            oldBathroomBroCzarReference = oldBathroomBroCzar.GetComponent<Bro>();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("WaitForSmoke", () => {
            // Waiting
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("Behold", () => {
            TextboxManager.Instance.SetText("Behold my glory! \nFor this is what your future may hold if you can prove that you are worthy of this position!");
            TextboxManager.Instance.Show();
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("BeholdEnd", () => {
            if(TextboxManager.Instance.HasFinished()) {
                TextboxManager.Instance.Hide();
                Completed();
            }
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Jump Up", () => {
            GameObject spriteToTween = CinematicHelper.Instance.GetChildGameObject(oldBathroomBroCzar, "Sprites");
            float jumpHeight = 1f;
            float jumpStart = spriteToTween.transform.localPosition.y;
            float jumpApex = spriteToTween.transform.localPosition.y + jumpHeight;
            
            // Debug.Log("SpriteToTween: " + spriteToTween.name);
            TweenExecutor.Position
            .Object(spriteToTween)
            .StartPosition(spriteToTween.transform.localPosition.x, jumpStart)
            .EndPosition(spriteToTween.transform.localPosition.x, jumpApex)
            .Duration(0.25f)
            .Method(UITweener.Method.Linear)
            .Style(UITweener.Style.Once)
            .OnFinish(() => {
                oldBathroomBroCzarReference.SetFacing(Facing.Bottom);
                
                TweenExecutor.Position
                .Object(spriteToTween)
                .StartPosition(spriteToTween.transform.localPosition.x, jumpApex)
                .EndPosition(spriteToTween.transform.localPosition.x, jumpStart)
                .Duration(0.25f)
                .Method(UITweener.Method.Linear)
                .Style(UITweener.Style.Once)
                .OnFinish(() => {
                    Completed();
                })
                .Tween();
            })
            .Tween();
            
            Completed();
        }));
        //----------------------------------------------------------------------
        waveStates.Add(CreateWaveState("OBBC Jump Up End", () => {
            Debug.Log("waiting to end");
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
    
    public void FinishBroCzarEnterAnimation() {
        Debug.Log("finishing start animation");
    }
}