using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TryOutsDayOne : WaveLogic, WaveLogicContract {

    public GameObject broCzarGameObject = null;

    public override void Awake() {
        base.Awake();
    }

    // Use this for initialization
    public override void Start() {
        base.Start();
        Initialize();
    }

    public override void Initialize() {
        Debug.Log("initialization");
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
        // SoundManager.Instance.PlayMusic(AudioType.CosmicSpaceHeadSurfing);

        // TweenExecutor.TweenObjectPosition(LevelManager.Instance.janitorOverlayGameObject,
        //                                   LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x,
        //                                   -595,
        //                                   LevelManager.Instance.janitorOverlayGameObject.transform.localPosition.x,
        //                                   -250,
        //                                   1,
        //                                   2,
        //                                   UITweener.Method.BounceIn,
        //                                   null);

        FadeManager.Instance.PerformFullScreenFade(Color.white, Color.clear, 1, false);

        BroGenerator.Instance.Pause();

        broCzarGameObject.SetActive(true);
        TargetPathing broCzarTargetPathing = broCzarGameObject.GetComponent<TargetPathing>();

        LineQueue entranceLineQueue = EntranceQueueManager.Instance.GetLineQueue(0).GetComponent<LineQueue>();
        GameObject lastQueueTile = entranceLineQueue.GetLastQueueTile();

        broCzarGameObject.transform.position = new Vector3(lastQueueTile.transform.position.x,
                lastQueueTile.transform.position.y,
                broCzarGameObject.transform.position.z);
        List<GameObject> entranceToCenterMovementNodes = entranceLineQueue.GetQueueMovementNodes();
        // Debug.Log("entranceToCenterMovementNodes Length: " + entranceToCenterMovementNodes.Count);
        entranceToCenterMovementNodes.AddRange(AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                               AStarManager.Instance.GetListCopyOfAllClosedNodes(),
                                               BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(lastQueueTile.transform.position, true).GetComponent<BathroomTile>(),
                                               BathroomTileMap.Instance.GetMiddleTileGameObject().GetComponent<BathroomTile>()));

        // SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);

        broCzarTargetPathing.SetTargetObjectAndTargetPosition(null, entranceToCenterMovementNodes);
        // broCzarTargetPathing.SetOnArrivalAtTargetPosition(() => {
        //                                                             Debug.Log("lol at target position");
        //                                                          });

        LevelManager.Instance.pauseButton.GetComponent<UISprite>().alpha = 0;
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
