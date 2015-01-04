using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BroDistributionFartGenerator : BroDistributionBathroomTileBlockerGenerator {
    public BroDistribution broDistributionDefaultDuration = BroDistribution.AllBros;
    public float defaultDuration = float.PositiveInfinity;
    public float defaultDurationMin = 2f;
    public float defaultDurationMax = 2f;

    public BroDistribution broDistributionDefaultDurationIsStochastic = BroDistribution.AllBros;
    public bool defaultDurationIsStochastic = false;

    public BroDistribution broDistributionDefaultMinDuration = BroDistribution.AllBros;
    public float defaultMinDuration = float.PositiveInfinity;
    public float defaultMinDurationMin = 1f;
    public float defaultMinDurationMax = 1f;

    public BroDistribution broDistributionDefaultMaxDuration = BroDistribution.AllBros;
    public float defaultMaxDuration = float.PositiveInfinity;
    public float defaultMaxDurationMin = 5f;
    public float defaultMaxDurationMax = 5f;

    public BroDistributionFartGenerator() : base() {
    }

    //---------------------------------
    // Move to fart distribution generator
    public BroDistributionFartGenerator SetDuration(BroDistribution broDistribution, float newDefaultDurationMin, float newDefaultDurationMax) {
        broDistributionDefaultDuration = broDistribution;

        defaultDurationMin = newDefaultDurationMin;
        defaultDurationMax = newDefaultDurationMax;

        return this;
    }
    public FartGenerator ConfigureDuration(FartGenerator fartGeneratorReference) {
        switch(broDistributionDefaultMaxGenerationFrequency) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultDuration == float.PositiveInfinity) {
                    defaultDuration = UnityEngine.Random.Range(defaultDurationMin, 
                                                               defaultDurationMax);
                }
                fartGeneratorReference.duration = defaultDuration;
            break;
            case(BroDistribution.RandomBros):
                defaultDuration = UnityEngine.Random.Range(defaultDurationMin, 
                                                           defaultDurationMax);
                fartGeneratorReference.duration = defaultDuration;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return fartGeneratorReference;
    }
    //---------------------------------
    public BroDistributionFartGenerator SetDurationIsStochastic(BroDistribution broDistribution, bool durationIsStochastic) {
        broDistributionDefaultDurationIsStochastic = broDistribution;

        defaultDurationIsStochastic = durationIsStochastic;

        return this;
    }
    public FartGenerator ConfigureDurationIsStochastic(FartGenerator fartGeneratorReference) {
        switch(broDistributionDefaultGenerationFrequencyIsStochastic) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                fartGeneratorReference.durationIsStochastic = defaultDurationIsStochastic;
            break;
            case(BroDistribution.RandomBros):
                fartGeneratorReference.durationIsStochastic = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return fartGeneratorReference;
    }
    //---------------------------------
    public BroDistributionFartGenerator SetMinDuration(BroDistribution broDistribution, float newMinDurationMin, float newMinDurationMax) {
        broDistributionDefaultMinDuration = broDistribution;

        defaultMinDurationMin = newMinDurationMax;
        defaultMinDurationMax = newMinDurationMax;

        return this;
    }
    public FartGenerator ConfigureMinDuration(FartGenerator fartGeneratorReference) {
        switch(broDistributionDefaultMinDuration) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultMinDuration == float.PositiveInfinity) {
                    defaultMinDuration = UnityEngine.Random.Range(defaultMinDurationMin, 
                                                                  defaultMinDurationMax);
                }
                fartGeneratorReference.minDuration = defaultMinDuration;
            break;
            case(BroDistribution.RandomBros):
                defaultMinDuration = UnityEngine.Random.Range(defaultMinDurationMin, 
                                                              defaultMinDurationMax);
                fartGeneratorReference.minDuration = defaultMinDuration;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return fartGeneratorReference;
    }
    //---------------------------------
    public BroDistributionFartGenerator SetMaxDuration(BroDistribution broDistribution, float newMaxDurationMin, float newMaxDurationMax) {
        broDistributionDefaultMaxDuration = broDistribution;

        defaultMaxDurationMin = newMaxDurationMin;
        defaultMaxDurationMax = newMaxDurationMax;

        return this;
    }
    public FartGenerator ConfigureMaxDuration(FartGenerator fartGeneratorReference) {
        switch(broDistributionDefaultMaxDuration) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultMaxDuration == float.PositiveInfinity) {
                    defaultMaxDuration = UnityEngine.Random.Range(defaultMaxDurationMin, 
                                                                  defaultMaxDurationMax);
                }
                fartGeneratorReference.maxDuration = defaultMaxDuration;
            break;
            case(BroDistribution.RandomBros):
                defaultMaxDuration = UnityEngine.Random.Range(defaultMaxDurationMin, 
                                                              defaultMaxDurationMax);
                fartGeneratorReference.maxDuration = defaultMaxDuration;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }

        return fartGeneratorReference;
    }
    //---------------------------------
}