using UnityEngine;
using System.Collections;

public class PinchZoom : MonoBehaviour {
    public Camera cameraReference;
    
    public float zoomStepSize = 0.2f;
    
    // zoom-in and zoom-out limits
    public float minZoom = 1.0f;
    public float maxZoom = 5.0f;
    
    public float horizontalPanSpeedModifier = 0.1f;
    public float verticalPanSpeedModifier = 0.1f;
    
    private bool isPointerPressed;
    
    private Vector2 previousPinchDistance = new Vector2(0, 0);
    private Vector2 currentPinchDistance = new Vector2(0, 0);
    private Vector2 pinchMidPoint = new Vector2(0, 0);
    
    public Vector3 cameraAnchor;
    public float minAnchorXOffset = 5f;
    public float maxAnchorXOffset = 5f;
    public float minAnchorYOffset = 5f;
    public float maxAnchorYOffset = 5f;
    
    void Start() {
        // cameraAnchor = this.gameObject.transform.localPosition;
        cameraAnchor = cameraReference.gameObject.transform.localPosition;
        // cameraAnchor = Vector3.zero;
        // Debug.Log("Camera Anchor Set To:\n" + cameraAnchor);
        
        isPointerPressed = false;
    }
    
    void Update() {
        PerformPinchZoomLogic();
    }
    
    private void PerformPinchZoomLogic() {
        // if(Input.GetMouseButtonDown(0)) {
        if(Input.GetMouseButton(0)) {
            isPointerPressed = true;
        }
        else if(Input.GetMouseButtonUp(0)) {
            isPointerPressed = false;
        }
        
        Vector3 cameraStepSize = CalculateCameraStepSize();
        // Debug.Log(cameraStepSize);
        cameraStepSize = FixCameraStepSizeToBeInBounds(cameraStepSize);
        cameraStepSize.z = 0;
        // may need to play around between local and world position
        cameraReference.transform.localPosition += cameraStepSize;
        
        // FixCameraToBeInBounds();
        PerformDoubleTapResetLogic();
        PerformScrollWheelLogic();
        CheckForMultiTouch();
    }
    
    public Vector3 CalculateCameraStepSize() {
        Vector3 cameraStepSize = Vector3.zero;
        
        // Returns if there's only been one tap detected and it just started
        if(Input.touchCount == 1
            && Input.GetTouch(0).phase == TouchPhase.Began
            && Input.GetTouch(0).tapCount == 1) {
            cameraStepSize = Vector3.zero;
        }
        else if(Input.touchCount == 1
                && Input.GetTouch(0).phase == TouchPhase.Moved) {
            cameraStepSize += Vector3.right * (-Input.GetTouch(0).deltaPosition.x) * horizontalPanSpeedModifier;
            cameraStepSize += Vector3.up * (-Input.GetTouch(0).deltaPosition.y) * verticalPanSpeedModifier;
        }
        else if(isPointerPressed) {
            cameraStepSize += Vector3.right * (-Input.GetAxis("Mouse X")) * horizontalPanSpeedModifier;
            cameraStepSize += Vector3.up * (-Input.GetAxis("Mouse Y")) * verticalPanSpeedModifier;
        }
        
        return cameraStepSize;
    }
    
    // I don't think this works at all....
    public Vector3 FixCameraStepSizeToBeInBounds(Vector3 cameraStepSize) {
        // Debug.Log("------------------------------------");
        // Debug.Log("starting step size:\n" + cameraStepSize);
        Vector3 modifiedCameraStepSize = new Vector3(cameraStepSize.x, cameraStepSize.y, cameraStepSize.z);
        Vector3 currentPosition = cameraReference.gameObject.transform.localPosition;
        
        float cameraLeftBound = (cameraAnchor.x - minAnchorXOffset);
        float cameraRightBound = (cameraAnchor.x + maxAnchorXOffset);
        float cameraBottomBound = (cameraAnchor.y - minAnchorYOffset);
        float cameraTopBound = (cameraAnchor.y + maxAnchorYOffset);
        
        float nextXPosition = (currentPosition.x + cameraStepSize.x);
        float nextYPosition = (currentPosition.y + cameraStepSize.y);
        
        // Debug.Log("cameraLeftBound: " + cameraLeftBound);
        // Debug.Log("cameraRightBound: " + cameraRightBound);
        // Debug.Log("cameraBottomBound: " + cameraBottomBound);
        // Debug.Log("cameraTopBound: " + cameraTopBound);
        
        // Debug.Log("nextXPosition: " + nextXPosition);
        // Debug.Log("nextYPosition: " + nextYPosition);
        
        if(nextXPosition < cameraLeftBound) {
            // Debug.Log("Less than left bound");
            modifiedCameraStepSize.x = cameraLeftBound - currentPosition.x;
        }
        else if(nextXPosition > cameraRightBound) {
            // Debug.Log("Greater than right bound");
            modifiedCameraStepSize.x = cameraRightBound - currentPosition.x;
        }
        
        if(nextYPosition < cameraBottomBound) {
            // Debug.Log("Less than bottom bound");
            modifiedCameraStepSize.y = cameraBottomBound - currentPosition.y;
        }
        else if(nextYPosition > cameraTopBound) {
            // Debug.Log("Greater than top bound");
            modifiedCameraStepSize.y = cameraTopBound - currentPosition.y;
        }
        
        // Debug.Log("ending step size:\n" + modifiedCameraStepSize);
        return modifiedCameraStepSize;
    }
    
    public void FixCameraToBeInBounds() {
        Vector3 currentPosition = cameraReference.transform.position;
        
        float cameraLeftBound = (cameraAnchor.x - minAnchorXOffset);
        float cameraRightBound = (cameraAnchor.x + maxAnchorXOffset);
        float cameraBottomBound = (cameraAnchor.y - minAnchorYOffset);
        float cameraTopBound = (cameraAnchor.y + maxAnchorYOffset);
        
        if(currentPosition.x < cameraLeftBound) {
            currentPosition.x = cameraLeftBound;
        }
        else if(currentPosition.x > cameraRightBound) {
            currentPosition.x = cameraRightBound;
        }
        
        if(currentPosition.y < cameraBottomBound) {
            currentPosition.y = cameraBottomBound;
        }
        else if(currentPosition.y > cameraTopBound) {
            currentPosition.y = cameraTopBound;
        }
        
        cameraReference.transform.localPosition = currentPosition;
    }
    
    private void PerformDoubleTapResetLogic() {
        // On device (only) double tap image will be set at original position and scale
        // if(Input.touchCount == 1
        //    && Input.GetTouch(0).phase == TouchPhase.Began
        //    && Input.GetTouch (0).tapCount == 2) {
        //   parentObject.transform.localScale = Vector3.one;
        //   parentObject.transform.position = new Vector3 (cameraAnchor.x * -1, cameraAnchor.y * -1, cameraAnchor.z * -1);
        //   transform.position = cameraAnchor;
        // }
    }
    
    private void PerformScrollWheelLogic() {
        // pc scrollWheel to zoom in and out.
        if(Input.GetAxis("Mouse ScrollWheel") > 0
            && cameraReference.orthographicSize > minZoom) {
            cameraReference.orthographicSize -= zoomStepSize;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0
                && cameraReference.orthographicSize < maxZoom) {
            cameraReference.orthographicSize += zoomStepSize;
        }
    }
    
    // Following method check multi touch
    private void CheckForMultiTouch() {
        // These lines of code will take the distance between two touches and zoom in - zoom out at middle point between them
        if(Input.touchCount == 2
            && Input.GetTouch(0).phase == TouchPhase.Moved
            && Input.GetTouch(1).phase == TouchPhase.Moved) {
            pinchMidPoint = new Vector2(((Input.GetTouch(0).position.x + Input.GetTouch(1).position.x) / 2), ((Input.GetTouch(0).position.y + Input.GetTouch(1).position.y) / 2));
            pinchMidPoint = Camera.main.ScreenToWorldPoint(pinchMidPoint);
            
            currentPinchDistance = Input.GetTouch(0).position - Input.GetTouch(1).position;
            
            //current distance between finger touches
            previousPinchDistance = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
            
            //difference in previous locations using delta positions
            float touchDelta = currentPinchDistance.magnitude - previousPinchDistance.magnitude;
            
            // device Zoom out
            if(touchDelta < 0) {
                if(cameraReference.orthographicSize < maxZoom) {
                    cameraReference.orthographicSize = cameraReference.orthographicSize + zoomStepSize;
                }
            }
            
            //device Zoom in
            else if(touchDelta > 0) {
                if(cameraReference.orthographicSize > minZoom) {
                    cameraReference.orthographicSize = cameraReference.orthographicSize - zoomStepSize;
                }
            }
            
            //mouse zoom out
            if(Input.GetAxis("Mouse ScrollWheel") < 0) {
                if(cameraReference.orthographicSize < maxZoom) {
                    cameraReference.orthographicSize = cameraReference.orthographicSize + zoomStepSize;
                }
            }
            
            //mouse zoom in
            if(Input.GetAxis("Mouse ScrollWheel") > 0) {
                if(cameraReference.orthographicSize > minZoom) {
                    cameraReference.orthographicSize = cameraReference.orthographicSize + zoomStepSize;
                }
            }
            
        }
    }
    
    public void ResetToAnchor() {
        cameraReference.gameObject.transform.position = cameraAnchor;
    }
}
