using UnityEngine;
using System.Collections;

public class HighlightSelectable : Selectable {

    //THIS IS COUPLED WITH THE SELECTABLE CLASS
    public GameObject highlightObject = null;
    
    void Awake() {
    }
    // Use this for initialization
    void Start() {
        Reset();
    }
    
    // Update is called once per frame
    void Update() {
        SetHighlightSpriteState(isSelected);
    }
    
    public override void Reset() {
        isSelected = false;
        SetHighlightSpriteState(false);
    }
    
    public void SetHighlightSpriteState(bool isShowing) {
        if(highlightObject != null) {
            highlightObject.GetComponent<SpriteRenderer>().enabled = isShowing;
        }
    }
    
    public void SetColor(Color newColor) {
        highlightObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}
