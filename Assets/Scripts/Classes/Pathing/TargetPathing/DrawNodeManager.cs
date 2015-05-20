using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// IMPORTANT!!!
// TODO: Refactor it so that this is where the draw node pools are stored for pathing
public class DrawNodeManager : MonoBehaviour {

    //BEGINNING OF SINGLETON CODE CONFIGURATION
    private static volatile DrawNodeManager _instance;
    private static object _lock = new object();
    
    //Stops the lock being created ahead of time if it's not necessary
    static DrawNodeManager() {
    }
    
    public static DrawNodeManager Instance {
        get {
            if(_instance == null) {
                lock(_lock) {
                    _instance = GameObject.FindObjectOfType<DrawNodeManager>();
                    if(_instance == null) {
                        GameObject drawNodeManagerGameObject = new GameObject("DrawNodeManager");
                        _instance = (drawNodeManagerGameObject.AddComponent<DrawNodeManager>()).GetComponent<DrawNodeManager>();
                    }
                }
            }
            return _instance;
        }
    }
    
    private DrawNodeManager() {
    }
    
    public void Awake() {
        //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
        //the script, which is assumedly attached to some GameObject. This in turn allows the instance
        //to be assigned when a game object is given this script in the scene view.
        //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
        //another call to it, so that you don't need to worry if it exists or not.
        _instance = this;
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Use this for initialization
    void Start() {
    }
    
    // Update is called once per frame
    void Update() {
    }
    
    public GameObject GenerateDrawNode() {
        GameObject newDrawNode = Factory.Instance.GenerateDrawNode();
        newDrawNode.transform.parent = this.gameObject.transform;
        return newDrawNode;
    }
}