using UnityEngine;
using System.Collections;

public class Selectable : MonoBehaviour {
    public bool isSelected = false;
    public bool canBeSelected = false;
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    
    }
    
    public virtual void Reset() {
        isSelected = false;
    }
    
    public virtual void OnMouseDown() {
        if(canBeSelected) {
            isSelected = !isSelected;
        }
        else {
            isSelected = false;
        }
        //Debug.Log(this.gameObject.name + "was clicked.");
    }
}
