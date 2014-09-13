using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FightingBros : TargetPathingNPC {
  public List<GameObject> brosFighting = new List<GameObject>();
  public int currentNumberOfTaps = 0;
  public int numberOfTapsNeededToBreakUp = 0;

  // Use this for initialization
  public override void Start () {
    base.Start();

    this.gameObject.transform.eulerAngles = Camera.main.transform.eulerAngles;
  }

  // Update is called once per frame
 public override void Update () {
   // base.Update();
   PerformLogic();
 }

  public void OnMouseDown() {
    currentNumberOfTaps++;
  }

  public override void PerformLogic() {
    PerformMovementLogic();
    PerformFightingBroArrivalLogic();
    PerformMaxTapLogic();
  }

  public void PerformFightingBroArrivalLogic() {
    if(IsAtTargetPosition()) {
      BathroomTile currentBathroomTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
      BathroomTile nextBathroomTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
      foreach(GameObject bathroomObject in BathroomObjectManager.Instance.allBathroomObjects) {
        BathroomTile tileBathroomObjectIsIn = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(bathroomObject.transform.position.x, bathroomObject.transform.position.y, true).GetComponent<BathroomTile>();

        if(bathroomObject.GetComponent<BathroomObject>() != null) {
          if(tileBathroomObjectIsIn.tileX == currentBathroomTile.tileX
             && tileBathroomObjectIsIn.tileY == currentBathroomTile.tileY) {
            BathroomObject bathObjRef = bathroomObject.GetComponent<BathroomObject>();
            if(bathObjRef.state != BathroomObjectState.Broken
               && bathObjRef.state != BathroomObjectState.BrokenByPee
               && bathObjRef.state != BathroomObjectState.BrokenByPoop) {
              bathObjRef.state = BathroomObjectState.Broken;
              switch(bathObjRef.type) {
                case(BathroomObjectType.Sink):
                  ScoreManager.Instance.IncrementScoreTracker(ScoreType.SinkBroken);
                break;
                case(BathroomObjectType.Stall):
                  ScoreManager.Instance.IncrementScoreTracker(ScoreType.StallBroken);
                break;
                case(BathroomObjectType.Urinal):
                  ScoreManager.Instance.IncrementScoreTracker(ScoreType.UrinalBroken);
                break;
              }
            }
          }
        }
      }
      // Debug.Log("next node x: " + nextBathroomTile.gameObject.transform.position.x + " y: " + nextBathroomTile.gameObject.transform.position.y);
      // Debug.Log("next node: " + nextBathroomTile.gameObject.name);
      List<Vector2> newMovementNodes = AStarManager.Instance.CalculateAStarPath((new List<GameObject>()),
                                                                                AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                currentBathroomTile,
                                                                                nextBathroomTile);
      SetTargetObjectAndTargetPosition(null, newMovementNodes);
    }
  }

  public void PerformMaxTapLogic() {
    if(currentNumberOfTaps >= numberOfTapsNeededToBreakUp) {
      foreach(GameObject gameObj in brosFighting) {
        Bro broReference = gameObj.GetComponent<Bro>();
        broReference.probabilityOfFightOnCollisionWithBro = 0f;
        broReference.state = BroState.MovingToTargetObject;
        gameObj.transform.position = this.gameObject.transform.position;
        gameObj.renderer.enabled = true;
        gameObj.SetActive(true);

        List<GameObject> exits = BathroomObjectManager.Instance.GetAllBathroomObjectsOfSpecificType(BathroomObjectType.Exit);
        int selectedExit = Random.Range(0, exits.Count);
        GameObject exitSelected = exits[selectedExit];
        // Exit exitSelected = ExitManager.Instance.SelectRandomExit().GetComponent<Exit>();
        BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(this.gameObject.transform.position.x, this.gameObject.transform.position.y, true).GetComponent<BathroomTile>();
        BathroomTile targetTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(exitSelected.transform.position.x, exitSelected.transform.position.y, true).GetComponent<BathroomTile>();
        List<Vector2> newMovementNodes = AStarManager.Instance.CalculateAStarPath(new List<GameObject>(),
                                                                                  AStarManager.Instance.GetListCopyOfAStarClosedNodes(),
                                                                                  startTile,
                                                                                  targetTile);
        broReference.SetTargetObjectAndTargetPosition(exitSelected, newMovementNodes);
      }
      BroManager.Instance.RemoveFightingBro(this.gameObject, true);
      // Destroy(this.gameObject);
    }
  }
}
