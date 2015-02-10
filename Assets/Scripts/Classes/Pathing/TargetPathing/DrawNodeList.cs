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
    public Color colorToTint = Color.white;

    void Start () {
        if(drawNodes == null) {
            drawNodes = new List<GameObject>();
        }
        if(drawNodePool == null) {
            drawNodePool = new List<GameObject>();
        }
    }
    
    void Update () {
        PerformDrawNodeCleanUpOnMovement();
    }

    public void SetColor(Color newColor) {
        colorToTint = newColor;
    }

    public void Reset() {
        foreach(GameObject drawNode in drawNodes) {
            AddToDrawNodePool(drawNode);
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

            newDrawNode = DrawNodeManager.Instance.GenerateDrawNode();
            // newDrawNode = Factory.Instance.GenerateDrawNode();
            // newDrawNode.transform.parent = this.gameObject.transform;
            // newDrawNode.transform.SetParent(this.gameObject.transform, false);
        }
        newDrawNode.GetComponent<DrawNode>().Reset();

        // Debug.Log("ColorToTint: " + colorToTint.ToString());
        newDrawNode.GetComponent<DrawNode>().SetSpritesColor(colorToTint);
        drawNodes.Add(newDrawNode);

        return newDrawNode;
    }

    // This isn't super stellar, but i'm not sure how else to achieve it with my crappy programming going on here
    public void PerformDrawNodeCleanUpOnMovement() {
        List<GameObject> gameObjectsToRemove = new List<GameObject>();

        foreach(GameObject drawNode in drawNodes) {
            if(gameObjectMoving.transform.position.x > (drawNode.transform.position.x - 0.15f)
               && gameObjectMoving.transform.position.x < (drawNode.transform.position.x + 0.15f)
               && gameObjectMoving.transform.position.y > (drawNode.transform.position.y - 0.15f)
               && gameObjectMoving.transform.position.y < (drawNode.transform.position.y + 0.15f)) {
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
