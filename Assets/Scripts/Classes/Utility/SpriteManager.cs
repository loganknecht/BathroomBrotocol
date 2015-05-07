// using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// public class SpriteManager : BaseBehavior {
public class SpriteManager : MonoBehaviour {
    public Dictionary<string, GameObject> sprites = null;
    
    // protected override void Awake() {
    public void Awake() {
        // base.Awake();
    }
    
    public void Start() {
        // if(sprites == null) {
        //     sprites = new Dictionary<string, GameObject>();
        // }
    }
    
    public void Update() {
    }
    
    public void Show(string spriteName) {
        sprites[spriteName].SetActive(false);
    }
    
    public void ShowAll() {
        foreach(KeyValuePair<string, GameObject> dictEntry in sprites) {
            dictEntry.Value.SetActive(true);
        }
    }
    
    public void HideAll() {
        foreach(KeyValuePair<string, GameObject> dictEntry in sprites) {
            dictEntry.Value.SetActive(false);
        }
    }
    
    public void Hide(string spriteName) {
        sprites[spriteName].SetActive(false);
    }
}