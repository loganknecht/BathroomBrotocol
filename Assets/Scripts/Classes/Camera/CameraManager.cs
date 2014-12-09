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

        pinchZoomReference = mainCamera.gameObject.GetComponent<PinchZoom>();
        rotateCameraReference = mainCamera.gameObject.GetComponent<RotateCamera>();
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
        rotateCameraReference.RotateLeft();
    }

    public void RotateRight() {
        rotateCameraReference.RotateRight();
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

    public DirectionBeingLookedAt GetDirectionFacingBasedOnCameraAndMovementDirection(bool movingUp, bool movingRight, bool movingDown, bool movingLeft) {
        DirectionBeingLookedAt cameraDirectionBeingLookedAt = rotateCameraReference.directionBeingLookedAt;
        DirectionBeingLookedAt directionBeingLookedAt = DirectionBeingLookedAt.None;
        if(movingRight) {
            // moving up right
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Top;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Left;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
                    break;
                }
            }
            // moving down right
            else if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Top;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Left;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                } 
            }
            // moving right
            else {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomRight;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                }  
            }
        }
        else if(movingLeft) {
            // moving up left
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Left;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Top;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
                    break;
                } 
            }
            // moving down left
            else if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Left;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.Top;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.Right;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
                    break;
                } 
            }
            // moving left
            else {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomRight;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.Left;
                    break;
                } 
            }
        }
        else {
            // moving up
            if(movingUp) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomRight;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.Top;
                    break;
                } 
            }
            // moving down 
            if(movingDown) {
                switch(cameraDirectionBeingLookedAt) {
                    case(DirectionBeingLookedAt.TopRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomRight;
                    break;
                    case(DirectionBeingLookedAt.TopLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.BottomLeft;
                    break;
                    case(DirectionBeingLookedAt.BottomRight):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopRight;
                    break;
                    case(DirectionBeingLookedAt.BottomLeft):
                        directionBeingLookedAt = DirectionBeingLookedAt.TopLeft;
                    break;
                    default:
                        directionBeingLookedAt = DirectionBeingLookedAt.Bottom;
                    break;
                } 
            }
        }
        return directionBeingLookedAt;
    }
}
