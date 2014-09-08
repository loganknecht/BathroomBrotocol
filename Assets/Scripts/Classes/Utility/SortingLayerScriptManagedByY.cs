using UnityEngine;
using System.Collections;

public class SortingLayerScriptManagedByY : MonoBehaviour {

  public GameObject gamobjectToBaseSortingLayerOn = null;
  public int sortingLayerOffset = 0;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if(gamobjectToBaseSortingLayerOn == null) {
      this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (sortingLayerOffset + (Mathf.RoundToInt(transform.position.y * 100f) * -1));
    }
    else {
      this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = (sortingLayerOffset + (Mathf.RoundToInt(gamobjectToBaseSortingLayerOn.transform.position.y * 100f) * -1));
    }
	}
}
