using UnityEngine;
using System.Collections;

public class BroFistAnimation : MonoBehaviour {
    public GameObject leftBroFist = null;
    public GameObject rightBroFist = null;

    public string levelToChangeToOnFinish = "";

    public float broFistDuration = 1f;
    public float leftBroFistStartXPostion = -1000;
    public float rightBroFistStartXPostion = 1000;
    public float leftBroFistFinalXPostion = -100;
    public float rightBroFistFinalXPostion = 100;

	// Use this for initialization
	void Start () {
        // PerformBroFistAnimation();
	}
	
	// Update is called once per frame
	void Update () {
	}

    // public void PerformBroFistAnimation() {
    //     TweenExecutor.TweenObjectPosition(leftBroFist, leftBroFistStartXPostion, leftBroFist.transform.position.y, leftBroFistFinalXPostion, leftBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, new EventDelegate(OnBroFistAnimationFinishChangeToScoreScreen));
    //     TweenExecutor.TweenObjectPosition(rightBroFist, rightBroFistStartXPostion, rightBroFist.transform.position.y, rightBroFistFinalXPostion, rightBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, new EventDelegate(OnBroFistAnimationFinishChangeToScoreScreen));
    // }

    public void PerformBroFistAnimation() {
        TweenExecutor.TweenObjectPosition(leftBroFist, leftBroFistStartXPostion, leftBroFist.transform.position.y, leftBroFistFinalXPostion, leftBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, null);
        TweenExecutor.TweenObjectPosition(rightBroFist, rightBroFistStartXPostion, rightBroFist.transform.position.y, rightBroFistFinalXPostion, rightBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, null);
    }

    //--------------------------------------------------------------------
    public void PerformBroFistAnimationAndChangeToScoreScreen() {
        TweenExecutor.TweenObjectPosition(leftBroFist, leftBroFistStartXPostion, leftBroFist.transform.position.y, leftBroFistFinalXPostion, leftBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, new EventDelegate(OnBroFistAnimationFinishChangeToScoreScreen));
        TweenExecutor.TweenObjectPosition(rightBroFist, rightBroFistStartXPostion, rightBroFist.transform.position.y, rightBroFistFinalXPostion, rightBroFist.transform.position.y, 0, broFistDuration, UITweener.Method.Linear, new EventDelegate(OnBroFistAnimationFinishChangeToScoreScreen));
    }
    public void OnBroFistAnimationFinishChangeToScoreScreen() {
        CameraManager.Instance.StartSmallCameraShake();
        CameraManager.Instance.mainCamera.GetComponent<CameraShake>().SetOnCameraShakeFinish(ChangeToScoreMenu);
    }
    public void ChangeToScoreMenu() {
        LevelManager.Instance.TriggerLevelChangeToScoreMenu();
    }
    //--------------------------------------------------------------------
}
