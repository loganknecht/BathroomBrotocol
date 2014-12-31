using UnityEngine;
using System.Collections;

public class FartGenerator : MonoBehaviour {
    public float fartProbability = 1f;
    public float fartGenerationTimer = 0f;
    public float fartGenerationTimerMax = 5f;
    public bool fartGenerationTimerMaxIsStochastic = false;
    public float minFartGenerationTimerMax = 5f;
    public float maxFartGenerationTimerMax = 10f;

    public float fartDuration = 2f;
    public bool fartDurationIsStochastic = false;
    public float minFartDuration = 1f;
    public float maxFartDuration = 5f;

    public bool isPaused = false;

    // Use this for initialization
    void Start () {
        ResetStochasticVariables();
    }

    // Update is called once per frame
    void Update () {
        if(!isPaused) {
            PerformFartGenerationLogic();
        }
    }

    public void PerformFartGenerationLogic() {
        if(this.gameObject.GetComponent<Bro>().state == BroState.MovingToTargetObject
            || this.gameObject.GetComponent<Bro>().state == BroState.Roaming) {
            fartGenerationTimer += Time.deltaTime;

            if(fartGenerationTimer > fartGenerationTimerMax) {
                fartGenerationTimer = 0f;
                float fartProbabilityRolled = Random.value;
                if(fartProbability > fartProbabilityRolled) {
                    //generate fart
                    GameObject newFart = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Fart);
                    Fart fartReference = newFart.GetComponent<Fart>();

                    fartReference.fartDurationTimerMax = fartDuration;

                    newFart.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newFart.transform.position.z);
                    BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newFart);
                    ResetStochasticVariables();
                }
            }

            //------------------------------
            if(fartGenerationTimerMaxIsStochastic) {
                fartGenerationTimerMax = Random.Range(minFartGenerationTimerMax, maxFartGenerationTimerMax);
            }
        }
    }
    public void ResetStochasticVariables() {
        if(fartGenerationTimerMaxIsStochastic) {
            fartGenerationTimerMax = Random.Range(minFartGenerationTimerMax, maxFartGenerationTimerMax);
        }
        if(fartDurationIsStochastic) {
            fartDuration = Random.Range(minFartDuration, maxFartDuration);
        }
    }
}
