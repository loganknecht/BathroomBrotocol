using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

  public GameObject mainCamera = null;
  public GameObject guiCamera = null;

  public RotateCamera rotateCameraReference;
  public PinchZoom pinchZoomReference;

  //BEGINNING OF SINGLETON CODE CONFIGURATION
  private static volatile CameraManager _instance;
  private static object _lock = new object();

  //Stops the lock being created ahead of time if it's not necessary
  static CameraManager() {
  }

  public static CameraManager Instance {
    get {
      if(_instance == null) {
        lock(_lock) {
          if (_instance == null) {
            GameObject CameraManagerGameObject = new GameObject("CameraManagerGameObject");
            _instance = (CameraManagerGameObject.AddComponent<CameraManager>()).GetComponent<CameraManager>();
          }
        }
      }
      return _instance;
    }
  }

  private CameraManager() {
  }

  public void Awake() {
    //There's a lot of magic happening right here. Basically, the THIS keyword is a reference to
    //the script, which is assumedly attached to some GameObject. This in turn allows the instance
    //to be assigned when a game object is given this script in the scene view.
    //This also allows the pre-configured lazy instantiation to occur when the script is referenced from
    //another call to it, so that you don't need to worry if it exists or not.
    _instance = this;

    pinchZoomReference = mainCamera.GetComponent<PinchZoom>();
    rotateCameraReference = mainCamera.GetComponent<RotateCamera>();
  }
  //END OF SINGLETON CODE CONFIGURATION

	// Update is called once per frame
	void Update () {
    // if(Input.GetKeyDown(KeyCode.Space)) {
      // StartSmallCameraShake();
      // StartMediumCameraShake();
      // StartLargeCameraShake();
      // mainCamera.GetComponent<CameraShake>().SetOnCameraShakeFinish();
    // }
	}

  public void RotateLeft() {
    Vector3 cameraOffset = pinchZoomReference.cameraReference.gameObject.transform.localPosition;
    pinchZoomReference.cameraReference.gameObject.transform.localPosition = pinchZoomReference.cameraAnchor;
    rotateCameraReference.RotateBySpecifiedDegreesAroundObject(Vector3.forward, 90f);
    pinchZoomReference.cameraReference.gameObject.transform.localPosition = cameraOffset;
  }

  public void RotateRight() {
    Vector3 cameraOffset = pinchZoomReference.cameraReference.gameObject.transform.localPosition;
    pinchZoomReference.cameraReference.gameObject.transform.localPosition = pinchZoomReference.cameraAnchor;
    rotateCameraReference.RotateBySpecifiedDegreesAroundObject(Vector3.forward, -90f);
    pinchZoomReference.cameraReference.gameObject.transform.localPosition = cameraOffset;
  }

  public void StartSmallCameraShake() {
    mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 0.1f, 0.1f);
    guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 1f, 1f);
  }

  public void StartMediumCameraShake() {
    mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 0.2f, 0.2f);
    guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 10f, 10f);
  }

  public void StartLargeCameraShake() {
    mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 0.3f, 0.3f);
    guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, 1f, 20f, 20f);
  }
}
