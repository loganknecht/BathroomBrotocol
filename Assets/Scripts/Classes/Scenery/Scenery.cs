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
        if(animatorReference != null) {
            animatorReference.SetBool(Facing.TopLeft.ToString(), false);
            animatorReference.SetBool(Facing.Top.ToString(), false);
            animatorReference.SetBool(Facing.TopRight.ToString(), false);
            animatorReference.SetBool(Facing.Left.ToString(), false);
            animatorReference.SetBool(Facing.Right.ToString(), false);
            animatorReference.SetBool(Facing.BottomLeft.ToString(), false);
            animatorReference.SetBool(Facing.Bottom.ToString(), false);
            animatorReference.SetBool(Facing.BottomRight.ToString(), false);

            animatorReference.SetBool(bathroomFacing.facing.ToString(), true);

            // If there were more states, do that enum
            animatorReference.SetBool("Default", true);

            // animatorReference.SetBool("None", false);
        }
    }
}
