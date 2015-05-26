using UnityEngine;
using System.Collections;

public class ShyBro : Bro {
    public bool firstArrivalOccurred = false;
    public bool firstArrivalWasWrongObject = false;
    
    protected override void Awake() {
        base.Awake();
        type = BroType.ShyBro;
    }
    
    // Use this for initialization
    public override void Start() {
        base.Start();
    }
    
    // Update is called once per frame
    public override void Update() {
        base.Update();
    }
}
