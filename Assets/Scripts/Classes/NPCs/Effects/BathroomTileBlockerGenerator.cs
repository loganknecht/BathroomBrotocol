using UnityEngine;
using System.Collections;

public class BathroomTileBlockerGenerator : MonoBehaviour {
    public float probability = 1f;
    public float generationTimer = 0f;
    public float generationTimerMax = 5f;
    public bool generationTimerMaxIsStochastic = false;
    public float minGenerationTimerMax = 5f;
    public float maxGenerationTimerMax = 10f;

    public float duration = 2f;
    public bool durationIsStochastic = false;
    public float minDuration = 1f;
    public float maxDuration = 5f;

    public bool isPaused = false;
    public bool loop = false;
    public int amountGenerated = 0;

    public BathroomTileBlockerType type = BathroomTileBlockerType.None;

    // Use this for initialization
    public virtual void Start () {
        ResetStochasticVariables();
    }

    // Update is called once per frame
    public virtual void Update () {
        if(!isPaused) {
            PerformGenerationLogic();
        }
    }

    public virtual void PerformGenerationLogic() {
        if(this.gameObject.GetComponent<Bro>().state == BroState.MovingToTargetObject
            || this.gameObject.GetComponent<Bro>().state == BroState.Roaming) {

            if(generationTimer > generationTimerMax
                && (amountGenerated == 0 || loop == true)) {
                generationTimer = 0f;
                float probabilityRolled = Random.value;
                if(probability > probabilityRolled) {
                    //generate 
                    GameObject newBathroomTileBlocker = Factory.Instance.GenerateBathroomTileBlockerObject(type);
                     // Reference = newBathroomTileBlocker.GetComponent<>();

                    newBathroomTileBlocker.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newBathroomTileBlocker.transform.position.z);
                    BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newBathroomTileBlocker);
                    ResetStochasticVariables();
                    amountGenerated++;
                }
            }
            else {
                generationTimer += Time.deltaTime;
            }

            //------------------------------
            if(generationTimerMaxIsStochastic) {
                generationTimerMax = Random.Range(minGenerationTimerMax, maxGenerationTimerMax);
            }
        }
    }
    public virtual void ResetStochasticVariables() {
        if(generationTimerMaxIsStochastic) {
            generationTimerMax = Random.Range(minGenerationTimerMax, maxGenerationTimerMax);
        }
        if(durationIsStochastic) {
            duration = Random.Range(minDuration, maxDuration);
        }
    }

    public virtual void ResetExecutionVariables() {
        amountGenerated = 0;
    }
}
