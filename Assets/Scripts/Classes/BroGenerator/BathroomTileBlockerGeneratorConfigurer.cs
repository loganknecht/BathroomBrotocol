using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BathroomTileBlockerGeneratorConfigurer {
    public BroDistribution broDistributionDefaultProbability = BroDistribution.AllBros;
    public float defaultProbability = float.PositiveInfinity;
    public float defaultProbabilityMin = 0f;
    public float defaultProbabilityMax = 1f;

    public BroDistribution broDistributionDefaultGenerationFrequency = BroDistribution.AllBros;
    public float defaultGenerationFrequency = float.PositiveInfinity;
    public float defaultGenerationFrequencyMin = 1f;
    public float defaultGenerationFrequencyMax = 5f;

    public BroDistribution broDistributionDefaultGenerationFrequencyIsStochastic = BroDistribution.AllBros;
    public bool defaultGenerationFrequencyIsStochastic = false;

    public BroDistribution broDistributionDefaultMinGenerationFrequency = BroDistribution.AllBros;
    public float defaultMinGenerationFrequency = float.PositiveInfinity;
    public float defaultMinGenerationFrequencyMin = 1f;
    public float defaultMinGenerationFrequencyMax = 5f;

    public BroDistribution broDistributionDefaultMaxGenerationFrequency = BroDistribution.AllBros;
    public float defaultMaxGenerationFrequency = float.PositiveInfinity;
    public float defaultMaxGenerationFrequencyMin = 1f;
    public float defaultMaxGenerationFrequencyMax = 5f;

    public BathroomTileBlockerGeneratorConfigurer() {
    }

    public BathroomTileBlockerGeneratorConfigurer SetProbability(BroDistribution broDistribution, float newMinProbability, float newMaxProbability) {
        broDistributionDefaultProbability = broDistribution;
        defaultProbabilityMin = newMinProbability;
        defaultProbabilityMax = newMaxProbability;
        return this;
    }

    public BathroomTileBlockerGeneratorConfigurer ConfigureProbability(BathroomTileBlockerGenerator bathroomTileBlockerGeneratorReference) {
        switch(broDistributionDefaultProbability) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultProbability == float.PositiveInfinity) {
                    defaultProbability = UnityEngine.Random.Range(defaultProbabilityMin, 
                                                                  defaultProbabilityMax);
                }
                bathroomTileBlockerGeneratorReference.probability = defaultProbability;
            break;
            case(BroDistribution.RandomBros):
                defaultProbability = UnityEngine.Random.Range(defaultProbabilityMin, 
                                                              defaultProbabilityMax);
                bathroomTileBlockerGeneratorReference.probability = defaultProbability;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return this;
    }

    //---------------------------------
    public BathroomTileBlockerGeneratorConfigurer SetGenerationFrequency(BroDistribution broDistribution, float newGenerationFrequencyMin, float newGenerationFrequencyMax) {
        broDistributionDefaultGenerationFrequency = broDistribution;
        defaultGenerationFrequencyMin = newGenerationFrequencyMin;
        defaultGenerationFrequencyMax = newGenerationFrequencyMax;
        return this;
    }
    
    public BathroomTileBlockerGeneratorConfigurer ConfigureGenerationFrequency(BathroomTileBlockerGenerator bathroomTileBlockerGeneratorReference) {
        switch(broDistributionDefaultGenerationFrequency) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultGenerationFrequency == float.PositiveInfinity) {
                    defaultGenerationFrequency = UnityEngine.Random.Range(defaultGenerationFrequencyMin, 
                                                                          defaultGenerationFrequencyMax);
                }
                bathroomTileBlockerGeneratorReference.generationFrequency = defaultGenerationFrequency;
            break;
            case(BroDistribution.RandomBros):
                defaultGenerationFrequency = UnityEngine.Random.Range(defaultGenerationFrequencyMin, 
                                                                      defaultGenerationFrequencyMax);
                bathroomTileBlockerGeneratorReference.generationFrequency = defaultGenerationFrequency;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return this;
    }
    //---------------------------------
    public BathroomTileBlockerGeneratorConfigurer SetGenerationFrequencyIsStochastic(BroDistribution broDistribution, bool newGenerationFrequencyIsStochastic) {
        broDistributionDefaultGenerationFrequencyIsStochastic = broDistribution;
        defaultGenerationFrequencyIsStochastic = newGenerationFrequencyIsStochastic;
        return this;
    }

    public BathroomTileBlockerGeneratorConfigurer ConfigureGenerationFrequencyIsStochastic(BathroomTileBlockerGenerator bathroomTileBlockerGeneratorReference) {
        switch(broDistributionDefaultGenerationFrequencyIsStochastic) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                bathroomTileBlockerGeneratorReference.generationFrequencyIsStochastic = defaultGenerationFrequencyIsStochastic;
            break;
            case(BroDistribution.RandomBros):
                bathroomTileBlockerGeneratorReference.generationFrequencyIsStochastic = (UnityEngine.Random.Range(0, 1) == 0) ? false : true;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return this;
    }
    //---------------------------------
    public BathroomTileBlockerGeneratorConfigurer SetMinGenerationFrequency(BroDistribution broDistribution, float newMinGenerationFrequencyMin, float newMinGenerationFrequencyMax) {
        broDistributionDefaultMinGenerationFrequency = broDistribution;
        defaultMinGenerationFrequencyMin = newMinGenerationFrequencyMin;
        defaultMinGenerationFrequencyMax = newMinGenerationFrequencyMax;
        return this;
    }
    public BathroomTileBlockerGeneratorConfigurer ConfigureMinGenerationFrequency(BathroomTileBlockerGenerator bathroomTileBlockerGeneratorReference) {
        switch(broDistributionDefaultMinGenerationFrequency) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultMinGenerationFrequency == float.PositiveInfinity) {
                    defaultMinGenerationFrequency = UnityEngine.Random.Range(defaultMinGenerationFrequencyMin, 
                                                                             defaultMinGenerationFrequencyMax);
                }
                bathroomTileBlockerGeneratorReference.minGenerationFrequency = defaultMinGenerationFrequency;
            break;
            case(BroDistribution.RandomBros):
                defaultMinGenerationFrequency = UnityEngine.Random.Range(defaultMinGenerationFrequencyMin, 
                                                                         defaultMinGenerationFrequencyMax);
                bathroomTileBlockerGeneratorReference.minGenerationFrequency = defaultMinGenerationFrequency;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return this;
    }
    //---------------------------------
    public BathroomTileBlockerGeneratorConfigurer SetMaxGenerationFrequency(BroDistribution broDistribution, float newMaxGenerationFrequencyMin, float newMaxGenerationFrequencyMax) {
        broDistributionDefaultMaxGenerationFrequency = broDistribution;
        defaultMaxGenerationFrequencyMin = newMaxGenerationFrequencyMin;
        defaultMaxGenerationFrequencyMax = newMaxGenerationFrequencyMax;
        return this;
    }
    public BathroomTileBlockerGeneratorConfigurer ConfigureMaxGenerationFrequency(BathroomTileBlockerGenerator bathroomTileBlockerGeneratorReference) {
        switch(broDistributionDefaultMaxGenerationFrequency) {
            case(BroDistribution.NoBros):
                // Do nothing bruh
            break;
            case(BroDistribution.AllBros):
                if(defaultMaxGenerationFrequency == float.PositiveInfinity) {
                    defaultMaxGenerationFrequency = UnityEngine.Random.Range(defaultMaxGenerationFrequencyMin, 
                                                                             defaultMaxGenerationFrequencyMax);
                }
                bathroomTileBlockerGeneratorReference.maxGenerationFrequency = defaultMaxGenerationFrequency;
            break;
            case(BroDistribution.RandomBros):
                defaultMaxGenerationFrequency = UnityEngine.Random.Range(defaultMaxGenerationFrequencyMin, 
                                                                         defaultMaxGenerationFrequencyMax);
                bathroomTileBlockerGeneratorReference.maxGenerationFrequency = defaultMaxGenerationFrequency;
            break;
            default:
                Debug.Log("An error occurred in trying to configure a bro generator's generated bro attribute");
            break;
        }
        return this;
    }
}