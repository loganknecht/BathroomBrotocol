using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawNodeList : MonoBehaviour {
    public GameObject gameObjectMoving;

    /// <value="drawNodes">The draw nodes to be drawn</value>
    public List<GameObject> drawNodes;
    /// <value="drawNodes">The draw nodes that are recycled for drawing</value>
    public List<GameObject> drawNodePool;

    public int numberOfDrawNodesBetweenPoint = 1;

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
        PerformDrawNodeCleanUpOnMovement();
    }

    public void Reset() {
        foreach(GameObject drawNode in drawNodes) {
        }
        drawNodes.Clear();

        foreach(GameObject drawNode in drawNodePool) {
            drawNode.GetComponent<DrawNode>().Reset();
        }
    }

    public void Show() {
        foreach(GameObject gameObj in drawNodes) {
            gameObj.GetComponent<DrawNode>().Show();
        }
        foreach(GameObject gameObj in drawNodePool) {
            gameObj.GetComponent<DrawNode>().Show();
        }
    }

    public void Hide() {
        foreach(GameObject gameObj in drawNodes) {
            gameObj.GetComponent<DrawNode>().Hide();
        }
        foreach(GameObject gameObj in drawNodePool) {
            gameObj.GetComponent<DrawNode>().Hide();
        }
    }

    public void AddToDrawNodePool(GameObject newDrawNodePoolItem) {
        if(!drawNodePool.Contains(newDrawNodePoolItem)) {
            drawNodePool.Add(newDrawNodePoolItem);
        }
    }

    public void SetDrawNodes(Vector2 startPosition, List<GameObject> movementNodes) {
        Reset();

        Vector2 currentMovementNode = Vector2.zero;
        GameObject nextMovementNode = null;

        for(int i = 0; i < movementNodes.Count; i++) {
            if(i == 0) {
                currentMovementNode = startPosition;
            }
            else {
                currentMovementNode = new Vector2(nextMovementNode.transform.position.x, nextMovementNode.transform.position.y);
            }
            nextMovementNode = movementNodes[i];
            for(int j = 0; j < numberOfDrawNodesBetweenPoint; j++) {
                float newDrawNodeXPosition = currentMovementNode.x + ((nextMovementNode.transform.position.x - currentMovementNode.x)/numberOfDrawNodesBetweenPoint)*j;
                float newDrawNodeYPosition = currentMovementNode.y + ((nextMovementNode.transform.position.y - currentMovementNode.y)/numberOfDrawNodesBetweenPoint)*j;

                GameObject newDrawNode = GetNewDrawNode();
                newDrawNode.transform.position = new Vector3(newDrawNodeXPosition,
                                                             newDrawNodeYPosition,
                                                             newDrawNode.transform.position.z);
            }
        }

        // Shows the draw node
        foreach(GameObject drawNode in drawNodes) {
            DrawNode drawNodeRef = drawNode.GetComponent<DrawNode>();
            drawNodeRef.Reset();

            foreach(GameObject spriteToManage in drawNodeRef.spritesToManage) {
                SpriteRenderer spriteRendererRef = spriteToManage.GetComponent<SpriteRenderer>();
                spriteRendererRef.enabled = true;
            }
        }
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

    public void PerformDrawNodeCleanUpOnMovement() {
        List<GameObject> gameObjectsToRemove = new List<GameObject>();

        foreach(GameObject drawNode in drawNodes) {
            if(gameObjectMoving.transform.position.x > (drawNode.transform.position.x - 0.05f)
               && gameObjectMoving.transform.position.x < (drawNode.transform.position.x + 0.05f)
               && gameObjectMoving.transform.position.y > (drawNode.transform.position.y - 0.05f)
               && gameObjectMoving.transform.position.y < (drawNode.transform.position.y + 0.05f)) {
                gameObjectsToRemove.Add(drawNode);
            }
        }

        foreach(GameObject gameObj in gameObjectsToRemove) {
            drawNodes.Remove(gameObj);
            AddToDrawNodePool(gameObj);
            gameObj.GetComponent<DrawNode>().Reset();
        }
    }
}
