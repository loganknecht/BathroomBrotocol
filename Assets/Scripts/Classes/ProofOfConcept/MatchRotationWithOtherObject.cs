using UnityEngine;
using System.Collections;

public class MatchRotationWithOtherObject : MonoBehaviour {

	public GameObject gameObjectToMatchRotation = null;

	// Use this for initialization
	void Start () {
		if(gameObjectToMatchRotation != null) {
			this.gameObject.transform.rotation = Quaternion.Euler(gameObjectToMatchRotation.transform.rotation.eulerAngles);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
