using UnityEngine;
using System.Collections;

// TODO: Add flag that says destroy on traversal, so when nodes are popped
//       it destroys the created tile
public class Tile : MonoBehaviour {
    public int tileX = -1;
    public int tileY = -1;
    
    public virtual void Awake() {
    }
    
    // Use this for initialization
    public virtual void Start() {
    }
    
    // Update is called once per frame
    public virtual void Update() {
    }
    
    public override string ToString() {
        string stringToReturn = "";
        stringToReturn += "TileX: " + tileX + " Tile Y: " + tileY;
        return stringToReturn;
    }
}
