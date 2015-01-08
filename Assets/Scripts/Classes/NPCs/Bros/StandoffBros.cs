using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StandoffBros : MonoBehaviour {
    public GameObject broOne = null;
    public GameObject broTwo = null;

    public Vector2 standoffAnchor = Vector2.zero;

    public bool isPaused = false;
    public bool isContracting = false;
    public bool isExpanding = false;

    public Vector2 broOneRadiusPosition = Vector2.zero;
    public Vector2 broTwoRadiusPosition = Vector2.zero;
    public float contractSpeed = 0.05f;
    public float expandSpeed = 0.05f;

    public int numberOfTapsNeededToStop = 3;
    public int numberOfContractionsBeforeAFight = 5;

    public float xLockOnBuffer = 0.005f;
    public float yLockOnBuffer = 0.005f;

    // Use this for initialization
    void Start () {
        numberOfTapsNeededToStop = UnityEngine.Random.Range(3,6);
        PerformStartedStandoffScore();
    }

    // Update is called once per frame
    void Update () {
        if(!isPaused) {
            PerformStandoffBroLogic();
            PerformTapLogic();
        }
    }

    public void Pause() {
        isPaused = true;
    } 
    public void Unpause() {
        isPaused = false;
    } 

    public void StandoffBrosInit(GameObject newBroOne, GameObject newBroTwo, Vector2 newStandoffAnchor) {
        standoffAnchor = newStandoffAnchor;
        isContracting = true;
        isExpanding = false;

        broOne = newBroOne;
        Bro broOneReference = broOne.GetComponent<Bro>();
        broOneReference.standoffBroGameObject = this.gameObject;
        broOneReference.selectableReference.ResetHighlightObjectAndSelectedState();
        broOneReference.targetPathingReference.disableMovementLogic = true;
        broOne.GetComponent<HighlightSelectable>().enabled = false;

        broTwo = newBroTwo;
        Bro broTwoReference = broTwo.GetComponent<Bro>();
        broTwoReference.standoffBroGameObject = this.gameObject;
        broTwoReference.selectableReference.ResetHighlightObjectAndSelectedState();
        broTwoReference.targetPathingReference.disableMovementLogic = true;
        broTwo.GetComponent<HighlightSelectable>().enabled = false;
    }

    public void SetRadiusPositionsRandomly() {
        float directionTowardsAnchor = UnityEngine.Random.Range(0f, 2f);
        float radius = 0.75f;
        float broOneXPoint = (float)(Math.Cos(directionTowardsAnchor)*radius);
        float broOneYPoint = (float)(Math.Sin(directionTowardsAnchor)*radius);

        broOneRadiusPosition = new Vector2((standoffAnchor.x + broOneXPoint), (standoffAnchor.y + broOneYPoint));
        broTwoRadiusPosition = new Vector2((standoffAnchor.x - broOneXPoint), (standoffAnchor.y - broOneYPoint));
    }

    public Vector3 CalcuateBroVelocity(Vector3 gameObjectPosition, Vector3 targetPosition) {
        Vector3 velocity = Vector3.zero;
        float velocitySpeed = 0f;
        if(isContracting) {
            velocitySpeed = contractSpeed;
        }
        else if(isExpanding) {
            velocitySpeed = expandSpeed;
        }

        if(gameObjectPosition.x < targetPosition.x) {
            velocity.x += velocitySpeed;
        }
        else if(gameObjectPosition.x > targetPosition.x) {
            velocity.x -= velocitySpeed;
        }

        if(gameObjectPosition.y < targetPosition.y) {
            velocity.y += velocitySpeed;
        }
        else if(gameObjectPosition.y > targetPosition.y) {
            velocity.y -= velocitySpeed;
        }

        return velocity;
    }

    public void UpdateBroPosition(GameObject broToUpdate, Vector3 velocity, Vector3 targetPosition) {
        broToUpdate.transform.position = new Vector3(broToUpdate.transform.position.x + velocity.x,
                                                     broToUpdate.transform.position.y + velocity.y,
                                                     broToUpdate.transform.position.z);
        // X
        if(velocity.x < 0
            && broToUpdate.transform.position.x < targetPosition.x) {
            broToUpdate.transform.position = new Vector3(targetPosition.x, broToUpdate.transform.position.y, broToUpdate.transform.position.z);
        }
        if(velocity.x > 0
            && broToUpdate.transform.position.x > targetPosition.x) {
            broToUpdate.transform.position = new Vector3(targetPosition.x, broToUpdate.transform.position.y, broToUpdate.transform.position.z);
        }
        // Y
        if(velocity.y < 0
            && broToUpdate.transform.position.y < targetPosition.y) {
            broToUpdate.transform.position = new Vector3(broToUpdate.transform.position.x, targetPosition.y, broToUpdate.transform.position.z);
        }
        if(velocity.y > 0
            && broToUpdate.transform.position.y > targetPosition.y) {
            broToUpdate.transform.position = new Vector3(broToUpdate.transform.position.x, targetPosition.y, broToUpdate.transform.position.z);
        }
    }

    public void PerformStandoffBroLogic() {
        // Debug.Log("bro one z: " + broOne.transform.position.z);
        // Debug.Log("bro two z: " + broTwo.transform.position.z);
        Vector3 newBroOneVelocity = Vector3.zero;
        Vector3 newBroTwoVelocity = Vector3.zero;
        if(isExpanding) {
            // CalcuateBroVelocity(broOne.transform.position,)
            // Debug.Log("Expanding");
            newBroOneVelocity = CalcuateBroVelocity(broOne.transform.position, broOneRadiusPosition);
            newBroTwoVelocity = CalcuateBroVelocity(broTwo.transform.position, broTwoRadiusPosition);

            UpdateBroPosition(broOne, newBroOneVelocity, broOneRadiusPosition); 
            UpdateBroPosition(broTwo, newBroTwoVelocity, broTwoRadiusPosition); 

            if((broOne.transform.position.x > (broOneRadiusPosition.x - xLockOnBuffer)
                && broOne.transform.position.x < (broOneRadiusPosition.x + xLockOnBuffer)
                && broOne.transform.position.y > (broOneRadiusPosition.y - yLockOnBuffer)
                && broOne.transform.position.y < (broOneRadiusPosition.y + yLockOnBuffer))
                || (broTwo.transform.position.x > (broTwoRadiusPosition.x - xLockOnBuffer)
                && broTwo.transform.position.x < (broTwoRadiusPosition.x + xLockOnBuffer)
                && broTwo.transform.position.y > (broTwoRadiusPosition.y - yLockOnBuffer)
                && broTwo.transform.position.y < (broTwoRadiusPosition.y + yLockOnBuffer))) {
                // broOne.transform.position = new Vector3(broOneRadiusPosition.x, broOneRadiusPosition.y, broOne.gameObject.transform.position.z);
                // broTwo.transform.position = new Vector3(broTwoRadiusPosition.x, broTwoRadiusPosition.y, broTwo.gameObject.transform.position.z);

                isExpanding = false;
                isContracting = true;
            }
        }
        else if(isContracting) {
            // Debug.Log("--------------------------------");
            // Debug.Log("Contracting");
            // Debug.Log("Bro One Before: " + broOne.transform.position);
            // Debug.Log("Bro Two Before: " + broTwo.transform.position);
            newBroOneVelocity = CalcuateBroVelocity(broOne.transform.position, standoffAnchor);
            newBroTwoVelocity = CalcuateBroVelocity(broTwo.transform.position, standoffAnchor);

            UpdateBroPosition(broOne, newBroOneVelocity, standoffAnchor); 
            UpdateBroPosition(broTwo, newBroTwoVelocity, standoffAnchor); 

            // Debug.Log("Bro One after: " + broOne.transform.position);
            // Debug.Log("Bro Two after: " + broTwo.transform.position);
            //if either bro arrived at their target position
            if((broOne.transform.position.x > (standoffAnchor.x - xLockOnBuffer)
                && broOne.transform.position.x < (standoffAnchor.x + xLockOnBuffer)
                && broOne.transform.position.y > (standoffAnchor.y - yLockOnBuffer)
                && broOne.transform.position.y < (standoffAnchor.y + yLockOnBuffer))
                || (broTwo.transform.position.x > (standoffAnchor.x - xLockOnBuffer)
                && broTwo.transform.position.x < (standoffAnchor.x + xLockOnBuffer)
                && broTwo.transform.position.y > (standoffAnchor.y - yLockOnBuffer)
                && broTwo.transform.position.y < (standoffAnchor.y + yLockOnBuffer))) {
                Debug.Log("bros arrived at standoff point");
                broOne.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, broOne.transform.position.z);
                broTwo.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, broTwo.transform.position.z);

                isExpanding = true;
                isContracting = false;

                numberOfContractionsBeforeAFight--;
                SetRadiusPositionsRandomly();
            }
        }


        else {
          Debug.Log("Wrong standoff logic");
        }
    }

    public void PerformTapLogic() {
        if(numberOfTapsNeededToStop <= 0) {
            PlayerStoppedStandoff();
        }
        if(numberOfContractionsBeforeAFight <= 0) {
            StandoffTurnedIntoFight();
        }
    }

    public void PlayerStoppedStandoff() {
        PerformStoppedStandoffScore();

        Bro broOneReference = broOne.GetComponent<Bro>();
        // broOne.collider.enabled = true;
        broOneReference.colliderReference.enabled = true;
        broOneReference.state = BroState.Roaming;
        broOneReference.standoffBroGameObject = null;
        broOneReference.broFightingWith = null;
        // broOneReference.canBeCheckedToFightAgainst = true;
        broOne.GetComponent<HighlightSelectable>().enabled = true;
        broOne.GetComponent<HighlightSelectable>().ResetHighlightObjectAndSelectedState();
        broOneReference.ResetFightLogic();
        broOneReference.targetPathingReference.disableMovementLogic = false;
        if(broOneReference.type == BroType.DrunkBro) {
            broOneReference.speechBubbleReference.displaySpeechBubble = false;
        }
        else {
            broOneReference.speechBubbleReference.displaySpeechBubble = true;
        }

        Bro broTwoReference = broTwo.GetComponent<Bro>();
        // broTwo.collider.enabled = true;
        broTwoReference.colliderReference.enabled = true;
        broTwoReference.state = BroState.Roaming;
        broTwoReference.standoffBroGameObject = null;
        broTwoReference.broFightingWith = null;
        // broTwoReference.canBeCheckedToFightAgainst = true;
        broTwo.GetComponent<HighlightSelectable>().enabled = true;
        broTwo.GetComponent<HighlightSelectable>().ResetHighlightObjectAndSelectedState();
        broTwoReference.ResetFightLogic();
        broTwoReference.targetPathingReference.disableMovementLogic = false;
        if(broTwoReference.type == BroType.DrunkBro) {
            broTwoReference.speechBubbleReference.displaySpeechBubble = false;
        }
        else {
            broTwoReference.speechBubbleReference.displaySpeechBubble = true;
        }

        // this.Destroy();
        BroManager.Instance.RemoveStandoffBro(this.gameObject, true);
    }

    public void StandoffTurnedIntoFight() {
        Bro broOneReference = broOne.GetComponent<Bro>();
        Bro broTwoReference = broTwo.GetComponent<Bro>();

        if(broOneReference.state != BroState.Fighting
            && broTwoReference.state != BroState.Fighting) {
            // broOne.renderer.enabled = false;
            // broOne.collider.enabled = false;
            broOne.SetActive(false);
            broOneReference.state = BroState.Fighting;
            broOneReference.selectableReference.ResetHighlightObjectAndSelectedState();

            // broTwo.renderer.enabled = false;
            // broTwo.collider.enabled = false;
            broTwo.SetActive(false);
            broTwoReference.state = BroState.Fighting;
            broTwoReference.selectableReference.ResetHighlightObjectAndSelectedState();

            GameObject newFightingBros = (GameObject)GameObject.Instantiate((Resources.Load("Prefabs/NPC/Bro/FightingBros1") as GameObject));
            newFightingBros.transform.position = new Vector3(standoffAnchor.x, standoffAnchor.y, newFightingBros.transform.position.z);
            newFightingBros.GetComponent<FightingBros>().brosFighting.Add(broOne);
            newFightingBros.GetComponent<FightingBros>().brosFighting.Add(broTwo);

            BathroomTile startTile = BathroomTileMap.Instance.GetTileGameObjectByWorldPosition(newFightingBros.transform.position.x, newFightingBros.transform.position.y, true).GetComponent<BathroomTile>();
            BathroomTile targetTile = BathroomTileMap.Instance.SelectRandomTile().GetComponent<BathroomTile>();
            newFightingBros.GetComponent<FightingBros>().SetTargetObjectAndTargetPosition(null,
                                                                                          AStarManager.Instance.CalculateAStarPath(BathroomTileMap.Instance.gameObject,
                                                                                                                                   AStarManager.Instance. GetListCopyOfPermanentClosedNodes(),
                                                                                                                                   startTile,
                                                                                                                                   targetTile));

            BroManager.Instance.AddFightingBro(newFightingBros);
            BroManager.Instance.RemoveStandoffBro(this.gameObject, true);
        }
    }

    public void IncrementTapsFromPlayer() {
        numberOfTapsNeededToStop--;
    }

    public void PerformStartedStandoffScore() {
        broOne.GetComponent<Bro>().PerformStartedStandoffScore();
        broTwo.GetComponent<Bro>().PerformStartedStandoffScore();
    }
    public void PerformStoppedStandoffScore() {
        broOne.GetComponent<Bro>().PerformStoppedStandoffScore();
        broTwo.GetComponent<Bro>().PerformStoppedStandoffScore();
    }
}
