using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A draw node can have connections in any direction, but the idea is that these
// are used to determine the direction that structures the path based on the
// order they occur. Direction is determined by the preceding node in the list 
// and the following node in the list, once position is identified relative to th
public class DrawNode : BaseBehavior {
    public List<GameObject> spritesToManage;

    protected override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        Initialize();
    }

    // Update is called once per frame
    void Update () {
        // UpdateAnimator();
    }

    public void Initialize() {
        if(spritesToManage == null) {
            spritesToManage = new List<GameObject>();
        }
    }

    public void Reset() {
        SetSpritesActive(false);
    }

    public void Show() {
        SetSpritesActive(true);
    }

    public void Hide() {
        SetSpritesActive(false);
    }

    public void SetSpritesActive(bool newEnabledState) {
        foreach(GameObject spriteToManage in spritesToManage) {
            SpriteRenderer spriteRendererRef = spriteToManage.GetComponent<SpriteRenderer>();
            spriteRendererRef.enabled = newEnabledState;
        }
    }

    public void SetSpritesColor(Color newColor) {
        foreach(GameObject spriteToManage in spritesToManage) {
            SpriteRenderer spriteRendererRef = spriteToManage.GetComponent<SpriteRenderer>();
            spriteRendererRef.color = new Color(newColor.r, newColor.g, newColor.b, spriteRendererRef.color.a);
        }
    }
}
