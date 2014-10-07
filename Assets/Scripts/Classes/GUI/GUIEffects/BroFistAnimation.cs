using UnityEngine;
using System.Collections;

public class BroFistAnimation : MonoBehaviour {
    public GameObject leftBroFist = null;
    public GameObject rightBroFist = null;

    public float broFistDuration = 1f;
    public float leftBroFistStartXPostion = -1000;
    public float rightBroFistStartXPostion = 1000;
    public float leftBroFistFinalXPostion = -100;
    public float rightBroFistFinalXPostion = 100;
    public bool isPerformingAnimation = false;

	// Use this for initialization
	void Start () {
        // PerformBroFistAnimation();
	}
	
	// Update is called once per frame
	void Update () {
	}

    void PerformBroFistAnimation(EventDelegate.Callback onFinishCallback) {
        if(!isPerformingAnimation) {
            isPerformingAnimation = true;
            // only perform on finish callback for left bro fist, it doesn't matter which, you just don't want to do it twice
            TweenExecutor.TweenObjectPosition(leftBroFist, leftBroFistStartXPostion, leftBroFist.transform.position.y, leftBroFistFinalXPostion, leftBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, new EventDelegate(onFinishCallback));
            TweenExecutor.TweenObjectPosition(rightBroFist, rightBroFistStartXPostion, rightBroFist.transform.position.y, rightBroFistFinalXPostion, rightBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, null);
        }
    }

    //--------------------------------------------------------------------
    public void PerformBroFistAnimationAndChangeToScoreScreen() {
        PerformBroFistAnimation(OnBroFistAnimationFinishShakeCameraThenChangeToScoreScreen);
    }
    public void OnBroFistAnimationFinishShakeCameraThenChangeToScoreScreen() {
        CameraManager.Instance.StartMediumCameraShake();
        CameraManager.Instance.mainCamera.GetComponent<CameraShake>().SetOnCameraShakeFinish(ChangeToScoreMenu);
    }
    public void ChangeToScoreMenu() {
        LevelManager.Instance.TriggerLevelChangeToScoreMenu();
    }
    //--------------------------------------------------------------------
}
