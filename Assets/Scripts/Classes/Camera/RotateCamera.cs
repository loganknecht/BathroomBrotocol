using UnityEngine;
using System.Collections;

// TO DO: FIX ROTATE LOGIC SCRIPT SO THAT WHEN IT ROTATES IT BASES THE CAMERA'S DIRECTION BEING LOOKED IT USES WORLD COORDINATES TO CALCULATE IT... OR SOMETHING

public class RotateCamera : MonoBehaviour {
  public GameObject cameraGameObject = null;
  public GameObject objectToRotateAround = null;
  public float amountRotated = 0f;
  public DirectionBeingLookedAt directionBeingLookedAt = DirectionBeingLookedAt.None;

	// Use this for initialization
	void Start () {
    directionBeingLookedAt = DirectionBeingLookedAt.Top;
    RotateBathroomToMatchCamera();
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

  public void RotateLeft() {
    RotateBySpecifiedDegreesAroundObject(Vector3.forward, 90f);
  }

  public void RotateRight() {
    RotateBySpecifiedDegreesAroundObject(Vector3.forward, -90f);
  }

  public void RotateBySpecifiedDegreesAroundObject(Vector3 rotationVector, float degreesToRotateBy) {
    if(objectToRotateAround != null) {
      amountRotated += degreesToRotateBy;
      amountRotated = amountRotated%360;
      if((amountRotated >= 0 && amountRotated < 90)
         || (amountRotated <= 0 && amountRotated > -90) ) {
        directionBeingLookedAt = DirectionBeingLookedAt.Top;
      }
      else if((amountRotated >= 90 && amountRotated < 180)
              || (amountRotated <= -270 && amountRotated > -360) ) {
        directionBeingLookedAt = DirectionBeingLookedAt.Left;
      }
      else if((amountRotated >= 180 && amountRotated < 270)
         || (amountRotated <= -180 && amountRotated > -270) ) {
        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
      }
      else if((amountRotated >= 270 && amountRotated < 360)
              || (amountRotated <= -90 && amountRotated > -180) ) {
        directionBeingLookedAt = DirectionBeingLookedAt.Right;
      }

      // transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);
      cameraGameObject.transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);
      RotateBathroomToMatchCamera();
    }
  }

  public void RotateBathroomToMatchCamera() {
    RotateBackground();
    RotateTileMapTiles();
    RotateBathroomTileBlockerObjects();
    RotateBathroomObjects();
    RotateBroGameObjects();
    RotateFightingBroGameObjects();
  }

  //THIS IS NOT WORKING CORRECTLY, NEED TO FIX IT BUT BACKGROUND DOESN'T DO CORRECT ROTATION BECAUSE THE BACKGROUND APPEARS REVERSED
  public void RotateBackground() {
    Vector3 newBackgroundRotation = Vector3.zero;

    newBackgroundRotation = new Vector3(this.gameObject.transform.eulerAngles.x, this.gameObject.transform.eulerAngles.y, this.gameObject.transform.eulerAngles.z);

    LevelManager.Instance.backgroundImage.transform.eulerAngles = newBackgroundRotation;

    // THIS WORKS
    // LevelManager.Instance.backgroundImage.transform.eulerAngles = this.gameObject.transform.eulerAngles;
  }

  public void RotateTileMapTiles() {
    foreach(GameObject bathroomTileGameObject in BathroomTileMap.Instance.tiles) {
      bathroomTileGameObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomTileGameObject.transform.rotation.x, bathroomTileGameObject.transform.rotation.y, this.gameObject.transform.rotation.z));
    }
  }

  public void RotateBathroomTileBlockerObjects() {
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

  public void RotateBathroomObjects() {
    foreach(GameObject bathroomObject in BathroomObjectManager.Instance.allBathroomObjects) {
      if(bathroomObject.GetComponent<BathroomObject>().type == BathroomObjectType.Exit) {
        bathroomObject.transform.rotation = Quaternion.Euler(new Vector3(bathroomObject.transform.rotation.x, bathroomObject.transform.rotation.y, this.gameObject.transform.rotation.z));
      }
      else {
        bathroomObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
      }

    }
  }

  public void RotateBroGameObjects() {
    foreach(GameObject broGameObject in BroManager.Instance.allBros) {
      RotateBroGameObject(broGameObject);
    }
  }
  public void RotateBroGameObject(GameObject broGameObjectToRotate) {
    broGameObjectToRotate.transform.eulerAngles = this.gameObject.transform.eulerAngles;
  }

  public void RotateFightingBroGameObjects() {
    foreach(GameObject fightingBroGameObject in BroManager.Instance.allFightingBros) {
      fightingBroGameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;
    }
  }
}
