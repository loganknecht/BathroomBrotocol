using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VomitGeneratorConfigurer : BathroomTileBlockerGeneratorConfigurer {
    public VomitGeneratorConfigurer() : base() {
    }
    
    //------------------------------------------------------------------
    // Overriding the base classes to return this class reference
    //------------------------------------------------------------------
    public new VomitGeneratorConfigurer SetProbability(BroDistribution broDistribution, float newMinProbability, float newMaxProbability) {
        base.SetProbability(broDistribution, newMinProbability, newMaxProbability);
        return this;
    }
    
    public new VomitGeneratorConfigurer SetGenerationFrequency(BroDistribution broDistribution, float newGenerationFrequencyMin, float newGenerationFrequencyMax) {
        base.SetGenerationFrequency(broDistribution, newGenerationFrequencyMin, newGenerationFrequencyMax);
        return this;
    }
    
    public new VomitGeneratorConfigurer SetGenerationFrequencyIsStochastic(BroDistribution broDistribution, bool newGenerationFrequencyIsStochastic) {
        base.SetGenerationFrequencyIsStochastic(broDistribution, newGenerationFrequencyIsStochastic);
        return this;
    }
    
    public new VomitGeneratorConfigurer SetMinGenerationFrequency(BroDistribution broDistribution, float newMinGenerationFrequencyMin, float newMinGenerationFrequencyMax) {
        base.SetMinGenerationFrequency(broDistribution, newMinGenerationFrequencyMin, newMinGenerationFrequencyMax);
        return this;
    }
    public new VomitGeneratorConfigurer SetMaxGenerationFrequency(BroDistribution broDistribution, float newMaxGenerationFrequencyMin, float newMaxGenerationFrequencyMax) {
        base.SetMaxGenerationFrequency(broDistribution, newMaxGenerationFrequencyMin, newMaxGenerationFrequencyMax);
        return this;
    }
}