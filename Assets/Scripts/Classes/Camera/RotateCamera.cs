using UnityEngine;
using System.Collections;

// TO DO: FIX ROTATE LOGIC SCRIPT SO THAT WHEN IT ROTATES IT BASES THE CAMERA'S DIRECTION BEING LOOKED IT USES WORLD COORDINATES TO CALCULATE IT... OR SOMETHING

public static class RotateLogic {
  public static void SetAxisAndSignBasedOnDirection(GameObject gameObjectBeingManaged, DirectionBeingLookedAt directionBeingLookedAt) {
    if(gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>() != null) {
      switch(directionBeingLookedAt) {
        case(DirectionBeingLookedAt.Top):
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetAxisToBaseCalculationOn(Axis.X);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetLayerOrderingSign(Sign.Negative);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetSortingLayerOffsetSign(Sign.Positive);
        break;
        case(DirectionBeingLookedAt.Right):
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetAxisToBaseCalculationOn(Axis.Y);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetLayerOrderingSign(Sign.Positive);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetSortingLayerOffsetSign(Sign.Negative);
        break;
        case(DirectionBeingLookedAt.Bottom):
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetAxisToBaseCalculationOn(Axis.X);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetLayerOrderingSign(Sign.Positive);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetSortingLayerOffsetSign(Sign.Negative);
        break;
        case(DirectionBeingLookedAt.Left):
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetAxisToBaseCalculationOn(Axis.Y);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetLayerOrderingSign(Sign.Negative);
          gameObjectBeingManaged.GetComponent<ManagedSortingLayerScript>().SetSortingLayerOffsetSign(Sign.Positive);
        break;
      }
    }
  }
}

public class RotateCamera : MonoBehaviour {
  public GameObject objectToRotateAround = null;
  public float amountRotated = 0f;
  public DirectionBeingLookedAt directionBeingLookedAt = DirectionBeingLookedAt.None;

	// Use this for initialization
	void Start () {
    directionBeingLookedAt = DirectionBeingLookedAt.Top;
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

  void RotateBySpecifiedDegreesAroundObject(Vector3 rotationVector, float degreesToRotateBy) {
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

      transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);

      RotateBathroomToMatchCamera();
    }
  }

  public void RotateBathroomToMatchCamera() {
    RotateBackground();
    RotateTileMapTiles();
    RotateBathroomTileBlockerObjects();
    RotateBathroomObjects();
    RotateBroGameObjects();
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

      RotateLogic.SetAxisAndSignBasedOnDirection(bathroomObject, directionBeingLookedAt);
    }
  }

  void RotateBroGameObjects() {
    foreach(GameObject broGameObject in BroManager.Instance.allBros) {
      broGameObject.transform.eulerAngles = this.gameObject.transform.eulerAngles;

      RotateLogic.SetAxisAndSignBasedOnDirection(broGameObject, directionBeingLookedAt);
    }
  }


}
