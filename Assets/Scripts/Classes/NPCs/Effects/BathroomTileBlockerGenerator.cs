using UnityEngine;
using System.Collections;

public class BathroomTileBlockerGenerator : MonoBehaviour {
    public float probability = 1f;
    public float generationTimer = 0f;
    public float generationFrequency = 5f;
    // Makes the generation random
    public bool generationFrequencyIsStochastic = false;
    public float minGenerationFrequency = 5f;
    public float maxGenerationFrequency = 10f;
    
    public bool isPaused = false;
    public bool finished = false;
    public int amountGenerated = 0;
    public int amountToGenerate = int.MaxValue;
    
    public BathroomTileBlockerType type = BathroomTileBlockerType.None;
    
    // Use this for initialization
    public virtual void Start() {
        SetRandomGenerationFrequency();
    }
    
    // Update is called once per frame
    public virtual void Update() {
        if(!isPaused) {
            PerformGenerationLogic();
        }
    }
    
    public virtual bool HasFinished() {
        return finished;
    }
    
    public void CheckIfFinshed() {
        if(amountGenerated >= amountToGenerate) {
            finished = true;
        }
    }
    
    public virtual void PerformGenerationLogic() {
        if(!HasFinished()) {
            if(generationTimer > generationFrequency) {
                Debug.Log("Generating.");
                generationTimer = 0f;
                // Does a probability roll to see if the tile blocker should be generated
                float probabilityRolled = Random.value;
                if(probability > probabilityRolled) {
                    GameObject newBathroomTileBlocker = Factory.Instance.GenerateBathroomTileBlockerObject(type);
                    newBathroomTileBlocker.transform.position = new Vector3(this.gameObject.transform.position.x,
                                                                            this.gameObject.transform.position.y,
                                                                            newBathroomTileBlocker.transform.position.z);
                    BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newBathroomTileBlocker);
                    SetRandomGenerationFrequency();
                    amountGenerated++;
                    
                    CheckIfFinshed();
                }
            }
            else {
                generationTimer += Time.deltaTime;
            }
        }
    }
    
    public virtual void SetRandomGenerationFrequency() {
        if(generationFrequencyIsStochastic) {
            generationFrequency = Random.Range(minGenerationFrequency, maxGenerationFrequency);
        }
    }
    
    public virtual void ResetExecutionVariables() {
        amountGenerated = 0;
    }
}
