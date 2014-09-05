using UnityEngine;
using System.Collections;

//could probably be a struct?
public class DistributionPoint : MonoBehaviour {

  public float distributionTime = 0f;
  public bool hasBeenDistributed = false;

  public DistributionPoint() {
  }

  public void ConfigureDistributionPoint(float newDistributionTime) {
    distributionTime = newDistributionTime;
  }
}
