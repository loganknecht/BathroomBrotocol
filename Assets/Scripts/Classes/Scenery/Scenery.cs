using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {
    public Animator animatorReference;
    public BathroomFacing bathroomFacing;

    // Use this for initialization
    void Start () {
        if(!animatorReference) {
            Debug.LogError("Animator Not Attached");
        }
        if(!bathroomFacing) {
            Debug.LogError("Bathroom Facing Not Attached");
        }
    }
    
    // Update is called once per frame
    void Update () {
        UpdateAnimator();
    }

    void UpdateAnimator() {
        bathroomFacing.UpdateAnimatorWithFacing(animatorReference);
        // If there were more states, do that enum
        animatorReference.SetBool("Default", true);
    }
}
