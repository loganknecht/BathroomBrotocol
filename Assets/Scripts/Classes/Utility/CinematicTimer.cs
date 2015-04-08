using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CinematicTimer {
    public float timer = 0;
    
    public virtual void Awake() {
        //
    }
    
    public virtual void Start() {
        //
    }
    
    public virtual void Update() {
        timer += Time.deltaTime;
    }
}
