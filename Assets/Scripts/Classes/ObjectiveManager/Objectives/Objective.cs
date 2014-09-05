using UnityEngine;
using System.Collections;

public class Objective : MonoBehaviour {
  public string objectiveDescription = "";

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

  public virtual bool IsObjectiveSatisfied() {
    return true;
  }
}
