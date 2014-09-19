using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (RotateCamera))]
[RequireComponent (typeof (PinchZoom))]
public class CameraManager : MonoBehaviour {

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

    pinchZoomReference = this.gameObject.GetComponent<PinchZoom>();
    rotateCameraReference = this.gameObject.GetComponent<RotateCamera>();
  }
  //END OF SINGLETON CODE CONFIGURATION


	// Update is called once per frame
	void Update () {

	}

  public void RotateLeft() {
    // Vector3 previousOffsetPosition = pinchZoomReference.gameObject.transform.position;
    // pinchZoomReference.gameObject.transform.position = Vector3.zero;
    // pinchZoomReference.ResetToAnchor();
    pinchZoomReference.cameraReference.gameObject.transform.position = pinchZoomReference.cameraAnchor;
    rotateCameraReference.RotateBySpecifiedDegreesAroundObject(Vector3.forward, 90f);
    pinchZoomReference.cameraAnchor = pinchZoomReference.cameraReference.gameObject.transform.position;
    // pinchZoomReference.cameraAnchor = pinchZoomReference.cameraReference.gameObject.transform.localPosition;
    // Debug.Log("ROTATED LEFT, NEW CAMERA ANCHOR:\n" + pinchZoomReference.cameraAnchor);
    // pinchZoomReference.gameObject.transform.position = previousOffsetPosition;
  }

  public void RotateRight() {
    // Vector3 previousOffsetPosition = pinchZoomReference.gameObject.transform.position;
    // pinchZoomReference.gameObject.transform.position = Vector3.zero;
    // pinchZoomReference.ResetToAnchor();
    pinchZoomReference.cameraReference.gameObject.transform.position = pinchZoomReference.cameraAnchor;
    rotateCameraReference.RotateBySpecifiedDegreesAroundObject(Vector3.forward, -90f);
    pinchZoomReference.cameraAnchor = pinchZoomReference.cameraReference.gameObject.transform.position;
    // pinchZoomReference.cameraAnchor = pinchZoomReference.cameraReference.gameObject.transform.localPosition;
    // Debug.Log("ROTATED RIGHT, NEW CAMERA ANCHOR:\n" + pinchZoomReference.cameraAnchor);
    // pinchZoomReference.gameObject.transform.position = previousOffsetPosition;
  }
}
