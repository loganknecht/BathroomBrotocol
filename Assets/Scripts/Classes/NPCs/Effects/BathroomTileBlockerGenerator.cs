using UnityEngine;
using System.Collections;

public class BathroomTileBlockerGenerator : MonoBehaviour {
    public float probability = 1f;
    public float generationTimer = 0f;
    public float generationFrequency = 5f;
    public bool generationFrequencyIsStochastic = false;
    public float minGenerationFrequency = 5f;
    public float maxGenerationFrequency = 10f;

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

            if(amountGenerated == 0 || loop == true) {
	            if(generationTimer > generationFrequency) {
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
	        }
        }
    }
    public virtual void ResetStochasticVariables() {
        if(generationFrequencyIsStochastic) {
            generationFrequency = Random.Range(minGenerationFrequency, maxGenerationFrequency);
        }
    }

    public virtual void ResetExecutionVariables() {
        amountGenerated = 0;
    }
}
