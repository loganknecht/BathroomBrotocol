using UnityEngine;
using System.Collections;

public class AStarNode : MonoBehaviour {
    public AStarNode parentAStarNode = null;
    public float gValue = 0f;
    public float heuristicValue = 0f;
    public bool isUntraversable = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ResetAStarValues() {
        parentAStarNode = null;
        gValue = 0f;
        heuristicValue = 0f;
    }
}
