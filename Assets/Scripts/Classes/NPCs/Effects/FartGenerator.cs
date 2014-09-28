using UnityEngine;
using System.Collections;

public class FartGenerator : MonoBehaviour {
  public float fartProbability = 1f;
  public float fartTimer = 0f;
  public float fartTimerMax = 5f;
  public bool fartTimerMaxIsStochastic = false;
  public float minFartTimerMax = 5f;
  public float maxFartTimerMax = 10f;
  public bool isPaused = false;

	// Use this for initialization
	void Start () {
    if(fartTimerMaxIsStochastic) {
      fartTimerMax = Random.Range(minFartTimerMax, maxFartTimerMax);
    }
	}
	
	// Update is called once per frame
	void Update () {
    if(!isPaused) {
      PerformFartTimerLogic();
    }
	}

	public void PerformFartTimerLogic() {
	    if(this.gameObject.GetComponent<Bro>().state == BroState.MovingToTargetObject
	       || this.gameObject.GetComponent<Bro>().state == BroState.Roaming) {
	      fartTimer += Time.deltaTime;

	      if(fartTimer > fartTimerMax) {
	        fartTimer = 0f;
	        float fartProbabilityRolled = Random.value;
	        if(fartProbability > fartProbabilityRolled) {
	          //generate fart
	          GameObject newFart = Factory.Instance.GenerateBathroomTileBlockerObject(BathroomTileBlockerType.Fart);
	          newFart.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, newFart.transform.position.z);
	          BathroomTileBlockerManager.Instance.AddBathroomTileBlockerGameObject(newFart);
	        }
	      }

	      //------------------------------
	      if(fartTimerMaxIsStochastic) {
	        fartTimerMax = Random.Range(minFartTimerMax, maxFartTimerMax);
	      }
	      
    }
  }
}
