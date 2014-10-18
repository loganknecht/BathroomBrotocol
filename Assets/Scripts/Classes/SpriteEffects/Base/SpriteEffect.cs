using UnityEngine;
using System.Collections;

public class SpriteEffect : MonoBehaviour {
    public SpriteEffectType spriteEffectType = SpriteEffectType.None;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    public virtual void PerformSpriteEffectFinishedLogic() {
        DisableSpriteEffectGameObject();
        ReturnSpriteEffectToAvailableSpriteEffectPool();
    }

    public virtual void DisableSpriteEffectGameObject() {
        this.gameObject.SetActive(false);
    }

    public virtual void ReturnSpriteEffectToAvailableSpriteEffectPool() {
        if(!SpriteEffectManager.Instance.spriteEffectsAvailable.Contains(this.gameObject)) {
            SpriteEffectManager.Instance.spriteEffectsAvailable.Add(this.gameObject);
        }
        SpriteEffectManager.Instance.spriteEffectsInUse.Remove(this.gameObject);
    }
}
