using UnityEngine;
using System.Collections;

public class Tappable : MonoBehaviour {

    public bool canBeTapped = false;
    public bool tapLimitReached = false;

    public float numberOfTaps = -1;
    public float maxNumberOfTaps = -1;

	// Use this for initialization
	public void Start () {
	}
	
	// Update is called once per frame
	public void Update () {
        if(numberOfTaps >= maxNumberOfTaps) {
            if(!tapLimitReached) {
                tapLimitReached = true;
            }
        }
	}

    public void OnMouseDown() {
        if(canBeTapped) {
            numberOfTaps++;
        }
    }

    public float GetTapRatio() {
        return numberOfTaps/maxNumberOfTaps;
    }

    public void ResetTaps() {
        numberOfTaps = 0;
    }
}
