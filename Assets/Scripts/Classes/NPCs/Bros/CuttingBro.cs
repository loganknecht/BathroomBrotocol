using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CuttingBro : Bro {
  // Use this for initialization
  public override void Start () {
    base.Start();
    type = BroType.CuttingBro;
  }

  // Update is called once per frame
  public override void Update () {
    base.Update();
  }

  //This is being checked on arrival before switching to occupying an object
  public override void PerformOnArrivalBrotocolScoreCheck() {
    // There are no brotocol checks for the cutting bro because it's not fair to the player
    // on account that they rush wherever they want.
  }

    //=========================================================================
}
