using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour {

    public GameObject mainCamera = null;
    public GameObject guiCamera = null;
    
    public RotateCamera rotateReference;
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
                    if(_instance == null) {
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
        
        pinchZoomReference = mainCamera.gameObject.GetComponent<PinchZoom>();
        rotateReference = mainCamera.gameObject.GetComponent<RotateCamera>();
    }
    //END OF SINGLETON CODE CONFIGURATION
    
    // Update is called once per frame
    void Update() {
        // if(Input.GetKeyDown(KeyCode.Space)) {
        // StartSmallCameraShake();
        // StartMediumCameraShake();
        // StartLargeCameraShake();
        // mainCamera.GetComponent<CameraShake>().SetOnCameraShakeFinish();
        // }
    }
    
    public void RotateLeft() {
        rotateReference.RotateLeft();
    }
    
    public void RotateRight() {
        rotateReference.RotateRight();
    }
    
    public void StartSmallCameraShake(float shakeDuration = 1f) {
        mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 0.1f, 0.1f);
        guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 1f, 1f);
    }
    
    public void StartMediumCameraShake(float shakeDuration = 1f) {
        mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 0.2f, 0.2f);
        guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 10f, 10f);
    }
    
    public void StartLargeCameraShake(float shakeDuration = 1f) {
        mainCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 0.3f, 0.3f);
        guiCamera.GetComponent<CameraShake>().StartCameraShake(0.1f, shakeDuration, 20f, 20f);
    }
    
    public void StopCameraShake() {
        mainCamera.GetComponent<CameraShake>().StopCameraShake();
        guiCamera.GetComponent<CameraShake>().StopCameraShake();
    }
}
