using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
  public GameObject objectToRotateAround = null;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
      RotateBySpecifiedDegreesAroundObject(Vector3.forward, 90f);
    }
    else if(Input.GetKeyDown(KeyCode.RightArrow)) {
      RotateBySpecifiedDegreesAroundObject(Vector3.forward, -90f);
    }
	}

  void RotateBySpecifiedDegreesAroundObject(Vector3 rotationVector, float degreesToRotateBy) {
    if(objectToRotateAround != null) {
      transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);

      RotateBackground();
      RotateTileMapTiles();
      RotateBathroomTileBlockerObjects();
      RotateBathroomObjects();
      RotateBroGameObjects();
    }
  }


  //THIS IS NOT WORKING CORRECTLY, NEED TO FIX IT BUT BACKGROUND DOESN'T DO CORRECT ROTATION BECAUSE THE BACKGROUND APPEARS REVERSED
  void RotateBackground() {
    Vector3 newBackgroundRotation = Vector3.zero;

    newBackgroundRotation = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z);

    LevelManager.Instance.backgroundImage.transform.eulerAngles = newBackgroundRotation;

    // THIS WORKS
    // LevelManager.Instance.backgroundImage.transform.eulerAngles = this.gameObject.transform.eulerAngles;
  }

  void RotateTileMapTiles() {
    foreach(GameObject bathroomTileGameObject in BathroomTileMap.Instance.tiles) {
      bathroomTileGameObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomTileGameObject.transform.rotation.x, bathroomTileGameObject.transform.rotation.y, this.gameObject.transform.rotation.z));
    }
  }

  void RotateBathroomTileBlockerObjects() {
    foreach(GameObject bathroomTileBlockerGameObject in BathroomTileBlockerManager.Instance.bathroomTileBlockers) {
      BathroomTileBlocker bathroomTileBlocker = bathroomTileBlockerGameObject.GetComponent<BathroomTileBlocker>();
      if(bathroomTileBlocker != null) {
        if(bathroomTileBlocker.bathroomTileBlockerType == BathroomTileBlockerType.Fart) {
          bathroomTileBlocker.transform.eulerAngles = this.gameObject.transform.eulerAngles;
        }
        else {
          bathroomTileBlockerGameObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomTileBlockerGameObject.transform.rotation.x, bathroomTileBlockerGameObject.transform.rotation.y, this.gameObject.transform.rotation.z));
        }
      }
    }
  }

  void RotateBathroomObjects() {
    foreach(GameObject bathroomObject in BathroomObjectManager.Instance.allBathroomObjects) {
      if(bathroomObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
        bathroomObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomObject.transform.rotation.x, bathroomObject.transform.rotation.y, this.gameObject.transform.rotation.z));
      }
      else {
        bathroomObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
      }
    }
  }

  void RotateBroGameObjects() {
    foreach(GameObject broGameObject in BroManager.Instance.allBros) {
      broGameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
    }
  }
}
