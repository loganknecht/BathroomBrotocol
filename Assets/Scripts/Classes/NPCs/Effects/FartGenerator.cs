using UnityEngine;
using System.Collections;

public class FartGenerator : BathroomTileBlockerGenerator {
    public float duration = 2f;
    public bool durationIsStochastic = false;
    public float minDuration = 1f;
    public float maxDuration = 5f;

    // Use this for initialization
    public override void Start () {
        base.Start();
        type = BathroomTileBlockerType.Fart;
        ResetStochasticVariables();
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }

    public override void ResetStochasticVariables() {
        if(generationFrequencyIsStochastic) {
            generationFrequency = Random.Range(minGenerationFrequency, maxGenerationFrequency);
        }
        if(durationIsStochastic) {
            duration = Random.Range(minDuration, maxDuration);
        }
    }
}
