using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FartGeneratorConfigurer : BathroomTileBlockerGeneratorConfigurer {
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
    
    public FartGeneratorConfigurer() : base() {
    }
    
    //------------------------------------------------------------------
    // Overriding the base classes to return this class reference
    //------------------------------------------------------------------
    public new FartGeneratorConfigurer SetProbability(BroDistribution broDistribution, float newMinProbability, float newMaxProbability) {
        base.SetProbability(broDistribution, newMinProbability, newMaxProbability);
        return this;
    }
    
    public new FartGeneratorConfigurer SetGenerationFrequency(BroDistribution broDistribution, float newGenerationFrequencyMin, float newGenerationFrequencyMax) {
        base.SetGenerationFrequency(broDistribution, newGenerationFrequencyMin, newGenerationFrequencyMax);
        return this;
    }
    
    public new FartGeneratorConfigurer SetGenerationFrequencyIsStochastic(BroDistribution broDistribution, bool newGenerationFrequencyIsStochastic) {
        base.SetGenerationFrequencyIsStochastic(broDistribution, newGenerationFrequencyIsStochastic);
        return this;
    }
    
    public new FartGeneratorConfigurer SetMinGenerationFrequency(BroDistribution broDistribution, float newMinGenerationFrequencyMin, float newMinGenerationFrequencyMax) {
        base.SetMinGenerationFrequency(broDistribution, newMinGenerationFrequencyMin, newMinGenerationFrequencyMax);
        return this;
    }
    public new FartGeneratorConfigurer SetMaxGenerationFrequency(BroDistribution broDistribution, float newMaxGenerationFrequencyMin, float newMaxGenerationFrequencyMax) {
        base.SetMaxGenerationFrequency(broDistribution, newMaxGenerationFrequencyMin, newMaxGenerationFrequencyMax);
        return this;
    }
    //---------------------------------
    // Move to fart distribution generator
    public FartGeneratorConfigurer SetDuration(BroDistribution broDistribution, float newDefaultDurationMin, float newDefaultDurationMax) {
        broDistributionDefaultDuration = broDistribution;
        
        defaultDurationMin = newDefaultDurationMin;
        defaultDurationMax = newDefaultDurationMax;
        
        return this;
    }
    public FartGeneratorConfigurer ConfigureDuration(FartGenerator fartGeneratorReference) {
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
        return this;
    }
    //---------------------------------
    public FartGeneratorConfigurer SetDurationIsStochastic(BroDistribution broDistribution, bool durationIsStochastic) {
        broDistributionDefaultDurationIsStochastic = broDistribution;
        
        defaultDurationIsStochastic = durationIsStochastic;
        
        return this;
    }
    public FartGeneratorConfigurer ConfigureDurationIsStochastic(FartGenerator fartGeneratorReference) {
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
        return this;
    }
    //---------------------------------
    public FartGeneratorConfigurer SetMinDuration(BroDistribution broDistribution, float newMinDurationMin, float newMinDurationMax) {
        broDistributionDefaultMinDuration = broDistribution;
        
        defaultMinDurationMin = newMinDurationMax;
        defaultMinDurationMax = newMinDurationMax;
        
        return this;
    }
    public FartGeneratorConfigurer ConfigureMinDuration(FartGenerator fartGeneratorReference) {
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
        
        return this;
    }
    //---------------------------------
    public FartGeneratorConfigurer SetMaxDuration(BroDistribution broDistribution, float newMaxDurationMin, float newMaxDurationMax) {
        broDistributionDefaultMaxDuration = broDistribution;
        
        defaultMaxDurationMin = newMaxDurationMin;
        defaultMaxDurationMax = newMaxDurationMax;
        
        return this;
    }
    public FartGeneratorConfigurer ConfigureMaxDuration(FartGenerator fartGeneratorReference) {
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
        
        return this;
    }
    //---------------------------------
}