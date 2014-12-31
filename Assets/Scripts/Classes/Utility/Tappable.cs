using UnityEngine;
using System.Collections;

public class Tappable : MonoBehaviour {

    public bool canBeTapped = true;

    public float numberOfTaps = 0;
    public float maxNumberOfTaps = 5;

    // Use this for initialization
    public void Start () {
    }

    // Update is called once per frame
    public void Update () {
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

    public bool IsTapLimitReached() {
        return (numberOfTaps >= maxNumberOfTaps);
    }
}
