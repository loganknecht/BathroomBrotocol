using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour {
    public GameObject cameraGameObject = null;

    public Vector3 cameraAnchor = Vector3.zero;

    public bool isShaking = false;
    public float shakeTimer = 0f;
    public float shakeFrequencyTimer = 0f;
    public float shakeFrequency = 0.1f;
    public float shakeDuration = 1f;
    public float shakeXOffset = 0.5f;
    public float shakeYOffset = 0.5f;

    public delegate void OnCameraShakeFinishDelegate();
    public OnCameraShakeFinishDelegate OnCameraShakeFinish;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        PerformShakingLogic();
	}

    public void StartCameraShake() {
        if(!isShaking) {
            cameraAnchor = cameraGameObject.transform.localPosition;
            isShaking = true;
            shakeTimer = 0f;
            shakeFrequencyTimer = 0f;
        }
    }

    public void StartCameraShake(float newShakeFrequency, float newShakeDuration, float newShakeXOffset, float newShakeYOffset) {
        if(!isShaking) {
            shakeFrequency = newShakeFrequency;
            shakeDuration = newShakeDuration;
            shakeXOffset = newShakeXOffset;
            shakeYOffset = newShakeYOffset;

            cameraAnchor = cameraGameObject.transform.localPosition;
            isShaking = true;
            shakeTimer = 0f;
            shakeFrequencyTimer = 0f;
        }
    }

    void PerformShakingLogic() {
        if(isShaking) {
            shakeTimer += Time.deltaTime;
            shakeFrequencyTimer += Time.deltaTime;
            if(shakeFrequencyTimer > shakeFrequency) {
                Vector3 newCameraShakePositionOffset = Vector3.zero;
                newCameraShakePositionOffset = cameraAnchor + new Vector3(Random.Range(-shakeXOffset, shakeXOffset),
                                                                          Random.Range(-shakeYOffset, shakeYOffset),
                                                                          0);
                cameraGameObject.transform.localPosition = newCameraShakePositionOffset;
                shakeFrequencyTimer = 0f;
            }

            if(shakeTimer > shakeDuration) {
                isShaking = false;
                cameraGameObject.transform.localPosition = cameraAnchor;
                PerformOnCameraShakeFinishLogic();
            }
        }
    }

    public void SetOnCameraShakeFinish(OnCameraShakeFinishDelegate newOnCameraShakeFinish) {
        OnCameraShakeFinish = new OnCameraShakeFinishDelegate(newOnCameraShakeFinish);
    }

    public void PerformOnCameraShakeFinishLogic() {
        if(OnCameraShakeFinish != null) {
            OnCameraShakeFinish();
        }
    }
}
