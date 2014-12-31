using UnityEngine;
using System.Collections;

public class VomitGenerator : BathroomTileBlockerGenerator {
    // Use this for initialization
    public override void Start () {
        base.Start();
        type = BathroomTileBlockerType.Vomit;
    }

    // Update is called once per frame
    public override void Update () {
        base.Update();
    }
}
