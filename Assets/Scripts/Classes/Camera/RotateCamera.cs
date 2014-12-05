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
        // Sets the rotated view correctly
        directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);
        RotateBathroomToMatchCamera();
    }

	// Update is called once per frame
	void Update () {
    if(Input.GetKeyDown(KeyCode.LeftArrow)) {
      RotateLeft();
    }
    else if(Input.GetKeyDown(KeyCode.RightArrow)) {
      RotateRight();
    }
	}

  public void RotateLeft() {
    RotateBySpecifiedDegreesAroundObject(Vector3.forward, -90f);
  }

  public void RotateRight() {
    RotateBySpecifiedDegreesAroundObject(Vector3.forward, 90f);
  }

  public void RotateBySpecifiedDegreesAroundObject(Vector3 rotationVector, float degreesToRotateBy) {
    if(objectToRotateAround != null) {
      amountRotated += degreesToRotateBy;
      amountRotated = amountRotated%360;

      directionBeingLookedAt = GetDirectionBeingLookedAt(amountRotated);

      // transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);
      cameraGameObject.transform.RotateAround(objectToRotateAround.transform.position, rotationVector, degreesToRotateBy);
      RotateBathroomToMatchCamera();
    }
  }

  public DirectionBeingLookedAt GetDirectionBeingLookedAt(float amountRotatedAroundZ) {
    DirectionBeingLookedAt dirBeingLookedAt = DirectionBeingLookedAt.None;

    if((amountRotated >= 0 && amountRotated < 90)
       || (amountRotated > -90 && amountRotated <= 0)) {
      // Debug.Log("Top");
      dirBeingLookedAt = DirectionBeingLookedAt.TopRight;
    }
    else if((amountRotated >= 90 && amountRotated < 180)
            || (amountRotated > -360 && amountRotated <= -270)) {
      // Debug.Log("Left");
      dirBeingLookedAt = DirectionBeingLookedAt.TopLeft;
    }
    else if((amountRotated >= 270 && amountRotated < 360)
            || (amountRotated > -180 && amountRotated <= -90)) {
      // Debug.Log("Right");
      dirBeingLookedAt = DirectionBeingLookedAt.BottomRight;
    }
    else if((amountRotated >= 180 && amountRotated < 270)
       || (amountRotated > -270 && amountRotated <= -180)) {
      // Debug.Log("Bottom");
      dirBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
    }

    return dirBeingLookedAt;
  }

  public void RotateBathroomToMatchCamera() {
    // RotateBackground();
    RotateTileMapTiles();
    RotateBathroomTileBlockerObjects();
    RotateBathroomObjects();
    RotateBroGameObjects();
    RotateFightingBroGameObjects();
  }

  //THIS IS NOT WORKING CORRECTLY, NEED TO FIX IT BUT BACKGROUND DOESN'T DO CORRECT ROTATION BECAUSE THE BACKGROUND APPEARS REVERSED
  public void RotateBackground() {
    // Vector3 newBackgroundRotation = Vector3.zero;
    // newBackgroundRotation = new Vector3(cameraGameObject.transform.eulerAngles.x, cameraGameObject.transform.eulerAngles.y, cameraGameObject.transform.eulerAngles.z);
    // LevelManager.Instance.backgroundImage.transform.eulerAngles = newBackgroundRotation;
  }

  public void RotateTileMapTiles() {
    // foreach(GameObject bathroomTileGameObject in BathroomTileMap.Instance.tiles) {
    GameObject[][] bathroomTiles = BathroomTileMap.Instance.GetTiles();
    if(bathroomTiles != null) {
      foreach(GameObject[] row in bathroomTiles) {
        foreach(GameObject tile in row) {
          tile.transform.rotation = Quaternion.Euler(new Vector3(tile.transform.rotation.x, tile.transform.rotation.y, this.gameObject.transform.rotation.z));
        }
      }
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
