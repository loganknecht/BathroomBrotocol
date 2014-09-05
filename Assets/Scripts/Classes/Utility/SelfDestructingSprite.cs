using UnityEngine;
using System.Collections;

public class SelfDestructingSprite : MonoBehaviour {

	public SpriteType spriteType = SpriteType.None;
	Animator animatorReference = null;
	public float numberOfSecondsToWaitUntilDestroyed = 0f;

	// Use this for initialization
	void Start () {
		animatorReference = this.gameObject.GetComponent<Animator>();
		UpdateAnimator();
		StartCoroutine(PerformSelfDestructLogic());
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnimator();
	}

	void UpdateAnimator() {
		animatorReference.SetBool(SpriteType.None.ToString(), false);
		animatorReference.SetBool(SpriteType.BrotocolAchieved.ToString(), false);

		animatorReference.SetBool(spriteType.ToString(), true);
	}

	IEnumerator PerformSelfDestructLogic() {
		yield return new WaitForSeconds(numberOfSecondsToWaitUntilDestroyed);
		Destroy(gameObject);
	}
}
