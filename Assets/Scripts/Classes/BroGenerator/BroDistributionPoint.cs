using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class BroDistributionPoint : DistributionPoint {

    public BroType broTypeToDistribute;
    public GameObject broToDistribute = null;
    public int selectedEntrance = 0;

    public void Start() {

    }

    public BroDistributionPoint() {
    }

    public BroDistributionPoint ConfigureDistributionPoint(float newDistributionTime, int newEntranceQueue, BroType newBroTypeToDistribute) {
        distributionTime = newDistributionTime;
        broTypeToDistribute = newBroTypeToDistribute;
        selectedEntrance = newEntranceQueue;

        return this;
    }

    public BroDistributionPoint ConfigureDistributionPoint(float newDistributionTime, int newEntranceQueue, GameObject newBroToDistribute) {
        distributionTime = newDistributionTime;
        broToDistribute = newBroToDistribute;
        selectedEntrance = newEntranceQueue;

        return this;
    }
    // public BroDistributionPoint SetBroDistributionReliefType(BroDistributionReliefType broDistributionReliefType) {
    //   return this;
    // }

    // public BroDistributionPoint SetBroDistributionFightCheckType(BroDistributionFightCheckType broDistributionFightCheckType) {
    //   return this;
    // }

    // public BroDistributionPoint SetBroDistributionLineQueueSkipType(BroDistributionLineQueueSkipType broDistributionLineQueueSkipType) {
    //   return this;
    // }

    // public BroDistributionPoint SetBroDistributionOnLineSkipAction(BroDistributionOnLineSkipAction broDistributionOnLineSkipAction) {
    //   return this;
    // }
}
