using UnityEngine;
using System.Collections;

public class Tappable : MonoBehaviour {

    public bool canBeTapped = true;
    public bool tapLimitReached = false;

    public float numberOfTaps = 0;
    public float maxNumberOfTaps = 5;

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
