using UnityEngine;
using System.Collections;

public class SelfDestructingSprite : MonoBehaviour {

	public SpriteEffectType spriteEffectType = SpriteEffectType.None;
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
		animatorReference.SetBool(SpriteEffectType.None.ToString(), false);
		animatorReference.SetBool(SpriteEffectType.BrotocolAchieved.ToString(), false);

		animatorReference.SetBool(spriteEffectType.ToString(), true);
	}

	IEnumerator PerformSelfDestructLogic() {
		yield return new WaitForSeconds(numberOfSecondsToWaitUntilDestroyed);
		Destroy(gameObject);
	}
}
