using UnityEngine;
using System.Collections;

public class BathroomFacing : MonoBehaviour {
    public Facing facing;
    // In degress
    public float rotationOffset = 0f;
    
    public void SetFacing(Facing newFacing) {
        facing  = newFacing;
    }
    public Facing GetFacing() {
        return facing;
    }
    
    public void UpdateAnimatorWithFacing(Animator animatorReference) {
        if(animatorReference != null) {
            foreach(Facing bathroomFacing in Facing.GetValues(typeof(Facing))) {
                if(bathroomFacing != Facing.None) {
                    animatorReference.SetBool(bathroomFacing.ToString(), false);
                }
            }
            if(facing != Facing.None) {
                animatorReference.SetBool(facing.ToString(), true);
            }
        }
    }
}