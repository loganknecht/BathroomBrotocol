using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayOne : WaveLogic, WaveLogicContract {

    public Bro broCzarReference = null;

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start() {
        base.Start();
        Initialize();
    }

    public override void Initialize() {
        // Debug.Log("initialization");
        SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

        GameObject broCzarEnterWaveGameObject = CreateWaveState("Start Animation Game Object",
                                                TriggerBroCzarEnterAnimation,
                                                PerformBroCzarEnterAnimation,
                                                FinishBroCzarEnterAnimation);

        InitializeWaveStates(
            broCzarEnterWaveGameObject
        );
    }

    // Update is called once per frame
    public override void Update() {
        base.Update();
    }

    public void TriggerBroCzarEnterAnimation() {
        // Debug.Log("triggering start animation");
        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

        BroGenerator.Instance.Pause();

        broCzarReference.gameObject.SetActive(true);

        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        GameObject firstQueueTile = entranceLineQueue.GetFirstQueueTile();
        GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();

        broCzarReference.transform.position = new Vector3(lastQueueTile.transform.position.x,
                lastQueueTile.transform.position.y,
                broCzarReference.transform.position.z);
        List<GameObject> entranceToCenterMovementNodes = entranceLineQueue.GetQueueMovementNodes();
        entranceToCenterMovementNodes.AddRange(AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                               AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                               BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(firstQueueTile.transform.position, true).GetComponent<BathroomTile>(),
                                               BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>()));

        broCzarReference.targetPathingReference.SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);
        broCzarReference.targetPathingReference.SetMoveSpeed(3, 3);
        broCzarReference.targetPathingReference.SetOnArrivalAtTargetPosition(() => {
            broCzarReference.SetState(BroState.Standing).SetFacing(Facing.Bottom);
        });

        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;

        // TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject,
        //                                   LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x,
        //                                   -595,
        //                                   LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x,
        //                                   -250,
        //                                   1,
        //                                   2,
        //                                   UITweener.Method.BounceIn,
        //                                   null);
    }

    public void PerformBroCzarEnterAnimation() {
        // Debug.Log("performing start animation");
        // if(TextboxManager.Instance.HasFinished()) {
        TriggerWaveFinish();
        // }
    }

    public void FinishBroCzarEnterAnimation() {
        // Debug.Log("finishing start animation");
    }
}