using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BroRushWaveLogic : WaveLogic, WaveLogicContract {
  public bool startAnimationPerformed = false;
  public bool puzzleDecisionMade = false;

  // Use this for initialization
  public override void Start () {
    SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

    base.Start();

    startAnimationPerformed = true;
    // puzzleDecisionMade = false;

    Dictionary<BroType, float> broProbabilities = new Dictionary<BroType, float>() {
                                                                                    // { BroType.ChattyBro, 0.45f },
                                                                                    // { BroType.DrunkBro, 0.10f },
                                                                                    { BroType.GenericBro, 1f }
                                                                                  };

    Dictionary<int, float> entranceQueueProbabilities = new Dictionary<int, float>() { { 0, .33f },
                                                                                       { 1, .33f },
                                                                                       { 2, .33f } };

    // public BroDistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, Dictionary<BroType, float> newBroProbabilities) : base(newStartTime, newEndTime, newNumberOfPointsToGenerate, newDistributionType) {
    BroDistributionObject firstWave = new BroDistributionObject(0, 60, 60, DistributionType.LinearIn, DistributionSpacing.Uniform, broProbabilities, entranceQueueProbabilities);
    // BroDistributionObject thirdWave = new BroDistributionObject(15, 60, 3, DistributionType.LinearIn, DistributionSpacing.Random, broProbabilities, entranceQueueProbabilities);

    // BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] { firstWave });
    BroGenerator.Instance.SetDistributionLogic(new BroDistributionObject[] {
                                                                             firstWave,
                                                                             // secondWave,
                                                                             // thirdWave
                                                                            });
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();

    foreach(GameObject gameObj in BroManager.Instance.allBros) {
      Bro broRef = gameObj.GetComponent<Bro>();
      if(broRef.hasRelievedSelf
         && broRef.targetObject.GetComponent<BathroomObject>().type != BathroomObjectType.Exit) {
        broRef.SetRandomBathroomObjectTarget(true, BathroomObjectType.Exit);
      }
      else {
        broRef.probabilityOfFightOnCollisionWithBro = 1f;
      }
    }
  }

  public override void PerformWaveLogic() {
    if(startAnimationPerformed
       && !puzzleDecisionMade) {
    }
  }
}
