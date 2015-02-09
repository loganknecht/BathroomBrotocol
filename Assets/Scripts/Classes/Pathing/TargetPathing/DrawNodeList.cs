using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawNodeList : MonoBehaviour {
    /// <value="drawNodes">The draw nodes to be drawn</value>
    public List<GameObject> drawNodes;
    /// <value="drawNodes">The draw nodes that are recycled for drawing</value>
    public List<GameObject> drawNodePool;

    void Start () {
        if(drawNodes == null) {
            drawNodes = new List<GameObject>();
        }
        if(drawNodePool == null) {
            drawNodePool = new List<GameObject>();
        }

        // GameObject firstNode = new GameObject("data node");
        // firstNode.transform.position = new Vector3(0, 0, 0);

        // GameObject secondNode = new GameObject("data node");
        // secondNode.transform.position = new Vector3(1, 0, 0);

        // GameObject thirdNode = new GameObject("data node");
        // thirdNode.transform.position = new Vector3(1, 1, 0);

        // GameObject fourthNode = new GameObject("data node");
        // fourthNode.transform.position = new Vector3(1, 2, 0);

        // GameObject fifthNode = new GameObject("data node");
        // fifthNode.transform.position = new Vector3(2, 2, 0);

        // GameObject sixthNode = new GameObject("data node");
        // sixthNode.transform.position = new Vector3(2, 1, 0);

        // GameObject seventhNode = new GameObject("data node");
        // seventhNode.transform.position = new Vector3(2, 0, 0);

        // List<GameObject> newDrawNodes = new List<GameObject>();

        // newDrawNodes.Add(firstNode);
        // newDrawNodes.Add(secondNode);
        // newDrawNodes.Add(thirdNode);
        // newDrawNodes.Add(fourthNode);
        // newDrawNodes.Add(fifthNode);
        // newDrawNodes.Add(sixthNode);
        // newDrawNodes.Add(seventhNode);
        // SetDrawNodes(newDrawNodes);
    }
    
    void Update () {
    }

    public void Reset() {
        foreach(GameObject drawNode in drawNodes) {
            if(!drawNodePool.Contains(drawNode)) {
                drawNodePool.Add(drawNode);
            }
        }
        drawNodes.Clear();

        foreach(GameObject drawNode in drawNodePool) {
            drawNode.GetComponent<DrawNode>().Reset();
            // Resets alpha value
            // foreach(KeyValuePair<ConnectedDirection, GameObject> dictEntry in drawNode.GetComponent<DrawNode>().connectedDirectionSprites) {
            //     SpriteRenderer spriteRendererRef = dictEntry.Value.GetComponent<SpriteRenderer>();
            //     dictEntry.Value.GetComponent<SpriteRenderer>().color = new Color(spriteRendererRef.color.r,
            //                                                                      spriteRendererRef.color.g,
            //                                                                      spriteRendererRef.color.b,
            //                                                                      1f);
            // }
        }

    }

    public void SetDrawNodes(List<GameObject> movementNodes) {
        // Debug.Log("setting draw nodes");
        // Debug.Log("MovementNodes Length: " + movementNodes.Count);

        Reset();

        GameObject previousMovementNode = null;
        GameObject currentMovementNode = null;
        GameObject nextMovementNode = null;

        for(int i = 0; i < movementNodes.Count; i++) {
            if(currentMovementNode != null) {
                previousMovementNode = currentMovementNode;
            }
            if(i + 1 < movementNodes.Count) {
                nextMovementNode = movementNodes[i + 1];
            }
            currentMovementNode = movementNodes[i];

            GameObject currentDrawNode = GetNewDrawNode();
            // DrawNode currentDrawNodeReference = currentDrawNode.GetComponent<DrawNode>();
            currentDrawNode.transform.position = new Vector3(currentMovementNode.transform.position.x,
                                                             currentMovementNode.transform.position.y,
                                                             currentDrawNode.transform.position.z);

            ConfigureNodeConnections(currentDrawNode, previousMovementNode, nextMovementNode);
            // Sets alpha value
            // if(i == 0) {
            //     foreach(KeyValuePair<ConnectedDirection, GameObject> dictEntry in currentDrawNode.GetComponent<DrawNode>().connectedDirectionSprites) {
            //         SpriteRenderer spriteRendererRef = dictEntry.Value.GetComponent<SpriteRenderer>();
            //         dictEntry.Value.GetComponent<SpriteRenderer>().color = new Color(spriteRendererRef.color.r,
            //                                                                          spriteRendererRef.color.g,
            //                                                                          spriteRendererRef.color.b,
            //                                                                          0.5f);
            //     }
            // }
        }
        // Debug.Log(drawNodes.Count);
    }

    public void ConfigureNodeConnections(GameObject currentNode, GameObject previousNode, GameObject nextNode) {
        DrawNode currentDrawNode = currentNode.GetComponent<DrawNode>();
        currentDrawNode.Reset();
        currentDrawNode.AddConnectedDirections(ConnectedDirection.Center);
        if(previousNode != null) {
            ConnectedDirection newConnectedDirection = GetConnectedDirection(currentNode, previousNode);
            if(newConnectedDirection != ConnectedDirection.None) {
                currentDrawNode.AddConnectedDirections(newConnectedDirection);
            }
        }
        if(nextNode != null) {
            ConnectedDirection newConnectedDirection = GetConnectedDirection(currentNode, nextNode);
            if(newConnectedDirection != ConnectedDirection.None) {
                currentDrawNode.AddConnectedDirections(newConnectedDirection);
            }
        }
    }

    public ConnectedDirection GetConnectedDirection(GameObject anchorNode, GameObject nodeToCompare) {
        ConnectedDirection directionToReturn = ConnectedDirection.None;

        if(nodeToCompare.transform.position.x < anchorNode.transform.position.x) {
            if(nodeToCompare.transform.position.y < anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.BottomLeft;
            }
            else if(nodeToCompare.transform.position.y > anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.TopLeft;
            }
            else {
                directionToReturn = ConnectedDirection.Left;
            }
        }
        else if(nodeToCompare.transform.position.x > anchorNode.transform.position.x) {
            if(nodeToCompare.transform.position.y < anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.BottomRight;
            }
            else if(nodeToCompare.transform.position.y > anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.TopRight;
            }
            else {
                directionToReturn = ConnectedDirection.Right;
            }
        }
        else {
            if(nodeToCompare.transform.position.y < anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.Bottom;
            }
            else if(nodeToCompare.transform.position.y > anchorNode.transform.position.y) {
                directionToReturn = ConnectedDirection.Top;
            }
            else {
                // No direction set
            }
        }

        return directionToReturn;
    }

    public GameObject GetNewDrawNode() {
        GameObject newDrawNode = null;

        // recycle a used draw node
        if(drawNodePool.Count != 0) { 
            newDrawNode = drawNodePool[0];
            drawNodePool.Remove(newDrawNode);
        }
        // create a new draw node
        else {
            newDrawNode = Factory.Instance.GenerateDrawNode();
            // newDrawNode.transform.parent = this.gameObject.transform;
            // newDrawNode.transform.SetParent(this.gameObject.transform, false);
        }
        newDrawNode.GetComponent<DrawNode>().Reset();
        drawNodes.Add(newDrawNode);

        return newDrawNode;
    }
}
