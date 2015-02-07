using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawNodeList : MonoBehaviour {
    /// <value="drawNodes">The draw nodes to be drawn</value>
    public List<GameObject> drawNodes;
    /// <value="drawNodes">The draw nodes that are recycled for drawing</value>
    public List<GameObject> drawNodePool;

    void Start () {
        drawNodes = new List<GameObject>();
        drawNodePool = new List<GameObject>();

        GameObject firstNode = new GameObject("data node");
        firstNode.transform.position = new Vector3(0, 0, 0);
        GameObject secondNode = new GameObject("data node");
        secondNode.transform.position = new Vector3(1, 0, 0);
        GameObject thirdNode = new GameObject("data node");
        thirdNode.transform.position = new Vector3(1, 1, 0);
        GameObject fourthNode = new GameObject("data node");
        fourthNode.transform.position = new Vector3(1, 2, 0);
        List<GameObject> newDrawNodes = new List<GameObject>();
        newDrawNodes.Add(firstNode);
        newDrawNodes.Add(secondNode);
        newDrawNodes.Add(thirdNode);
        newDrawNodes.Add(fourthNode);
        SetDrawNodes(newDrawNodes);
    }
    
    void Update () {
    }

    public void Reset() {
        foreach(GameObject drawNode in drawNodes) {
            if(!drawNodePool.Contains(drawNode)) {
                drawNodePool.Add(drawNode);
            }
            drawNodes.Clear();
        }

        foreach(GameObject drawNode in drawNodePool) {
            drawNode.GetComponent<DrawNode>().Reset();
        }
    }

    public void SetDrawNodes(List<GameObject> movementNodes) {
        Debug.Log("setting draw nodes");
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

            drawNodes.Add(currentDrawNode);
            ConfigureNodeConnections(currentDrawNode, previousMovementNode, nextMovementNode);
        }

        // foreach movement node
        // get previous node and next node
            // get a draw node
        // set connection directions based on those
    }

    public void ConfigureNodeConnections(GameObject currentNode, GameObject previousNode, GameObject nextNode) {
        DrawNode currentDrawNode = currentNode.GetComponent<DrawNode>();
        currentDrawNode.Reset();
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
            newDrawNode.transform.parent = this.gameObject.transform;
        }
        newDrawNode.GetComponent<DrawNode>().Reset();

        return newDrawNode;
    }
}
