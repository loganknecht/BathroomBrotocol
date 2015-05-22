#pragma warning disable 0219

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Debug.Log("performing start animation");
// if(TextboxManager.Instance.HasFinished()) {
// TriggerWaveFinish();
// }
public class TryOutsDayTwo : WaveLogic, WaveLogicContract {

    public override void Awake()
    {   base.Awake(); }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    public override void Initialize() {
        // Debug.Log("initialization");
        Bro broCzarReference = null;
        
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
            // LevelManager.Instance.HideRotationButtons(0f);
            ConfirmationBoxManager.Instance.Hide(float.Epsilon);
            Completed();
        }));
        //----------------------------------------------------------------------
        PerformWaveStates(CreateWaveState("Bro Test Wave State", () => {
            Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() { { BroType.GenericBro, 1f } };
            Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, 1f } };
            
            BroDistributionObject firstWave = new BroDistributionObject(0,
                                                                        1,
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
        PerformWaveStates(CreateDelayState("Delay", 60f));
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