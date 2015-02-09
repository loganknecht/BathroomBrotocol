using FullInspector;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// A draw node can have connections in any direction, but the idea is that these
// are used to determine the direction that structures the path based on the
// order they occur. Direction is determined by the preceding node in the list 
// and the following node in the list, once position is identified relative to th
public class DrawNode : BaseBehavior {
    public GameObject gameObjectToBaseTweenOffOf;
    
    // These are the connections that the node object has
    public List<ConnectedDirection> connectedDirections;
    public Dictionary<ConnectedDirection, GameObject> connectedDirectionSprites;

    protected override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    void Start () {
        Initialize();
    }

    // Update is called once per frame
    void Update () {
        UpdateAnimator();
    }

    public void Initialize() {
        if(connectedDirections == null) {
            connectedDirections = new List<ConnectedDirection>();
        }
        if(connectedDirectionSprites == null) {
            Debug.LogError("Oh son, you forgot to link up the sprites for the DrawNode! Get it together!");
        }

        foreach(ConnectedDirection connectedDirection in (ConnectedDirection[])ConnectedDirection.GetValues(typeof(ConnectedDirection))) {
            // For all enums that are not None
            if(connectedDirection != ConnectedDirection.None) {
                // This checks that each enum type is declared i.e. topleft, top, topright, left... 
                if(!connectedDirectionSprites.ContainsKey(connectedDirection)) {
                    Debug.LogError("The DrawNode '" + name + "'s connected direction sprites does not have a sprite for the direction '" + connectedDirection.ToString());
                }

                // make sure every one has a sprite renderer
                if(connectedDirectionSprites[connectedDirection].GetComponent<SpriteRenderer>() == null) {
                    Debug.LogError("The GameObject '" + connectedDirectionSprites[connectedDirection].name + "' does not contain a sprite renderer! Please attach one and configure it!");
                }
            }
        }
    }

    public void Reset() {
        connectedDirections.Clear();
        foreach(KeyValuePair<ConnectedDirection, GameObject> gameObj in connectedDirectionSprites) {
            gameObj.Value.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public void UpdateAnimator() {
        foreach(ConnectedDirection connectedDirection in (ConnectedDirection[])ConnectedDirection.GetValues(typeof(ConnectedDirection))) {
            if(connectedDirection != ConnectedDirection.None) {
                if(connectedDirections.Contains(connectedDirection)) {
                    connectedDirectionSprites[connectedDirection].GetComponent<SpriteRenderer>().enabled = true;
                }
                else {
                    connectedDirectionSprites[connectedDirection].GetComponent<SpriteRenderer>().enabled = false;
                }
            }
        }
    }
    public List<ConnectedDirection> AddConnectedDirections(params ConnectedDirection[] newConnectedDirections) {
        foreach(ConnectedDirection connectedDirection in newConnectedDirections) {
            if(!connectedDirections.Contains(connectedDirection)) {
                connectedDirections.Add(connectedDirection);
            }
        }
        return connectedDirections;
    }

    public List<ConnectedDirection> SetConnectedDirections(params ConnectedDirection[] newConnectedDirections) {
        List<ConnectedDirection> newConnectedDirectionsList = new List<ConnectedDirection>();
        foreach(ConnectedDirection connectedDirection in newConnectedDirections) {
            newConnectedDirectionsList.Add(connectedDirection);
        }
        return SetConnectedDirections(connectedDirections);
    }

    public List<ConnectedDirection> SetConnectedDirections(List<ConnectedDirection> newConnectedDirections) {
        connectedDirections = newConnectedDirections;
        return connectedDirections;
    }
}
