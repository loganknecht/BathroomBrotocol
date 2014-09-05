using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DistributionObject {
  public List<GameObject> distributionPoints = new List<GameObject>();

  public float startTime = 0f;
  public float endTime = 0f;

  public int numberOfPointsToGenerate = 0;

  public DistributionType distributionType = DistributionType.None;
  public DistributionSpacing distributionSpacing = DistributionSpacing.None;

  public DistributionObject(float newStartTime, float newEndTime, int newNumberOfPointsToGenerate, DistributionType newDistributionType, DistributionSpacing newDistributionSpacing) {
    startTime = newStartTime;
    endTime = newEndTime;
    numberOfPointsToGenerate =  newNumberOfPointsToGenerate;
    distributionType = newDistributionType;
    distributionSpacing = newDistributionSpacing;
  }

  public virtual List<GameObject> CalculateDistributionPoints() {

    foreach(GameObject gameObj in distributionPoints) {
      UnityEngine.GameObject.Destroy(gameObj);
    }

    List<GameObject> newDistributionPoints = new List<GameObject>();

    for(int i = 0; i < numberOfPointsToGenerate; i++) {
      // selectedPointOverInterval = startTime + (Random.value * (endTime - startTime));
      float selectedPointOverInterval = 0;
      switch(distributionSpacing) {
        case(DistributionSpacing.Random):
          selectedPointOverInterval = Random.Range(startTime, endTime);
        break;
        case(DistributionSpacing.Uniform):
          selectedPointOverInterval = ((1f/numberOfPointsToGenerate) * i) * (endTime - startTime);
        break;
        default:
          Debug.Log("AN UNEXPECTED DISTRIBUTION SPACING WAS USED FOR A DISTRIBUTION OBJECT!!!");
        break;
      }

      selectedPointOverInterval = ApplyDistributionTypeToValue(selectedPointOverInterval, distributionType);
      // newDistributionPoints.Add( new GameObject(selectedPointOverInterval) );
    }

    distributionPoints = newDistributionPoints;
    return newDistributionPoints;
  }

  public float ApplyDistributionTypeToValue(float valueToApplyDistributionTypeTo, DistributionType distributionType) {

    float currentTime = (valueToApplyDistributionTypeTo/(endTime - startTime));
    // float currentTime = Random.value;
    float normalizedStartTime = 0;
    float changeInValue = 1;
    float duration = 1;

    float modifierValue = 0f;

    switch(distributionType) {
      case(DistributionType.None):
        Debug.Log("THE NONE DISTRIBUTION TYPE WAS USED TO BE APPLIED TO A VALUE. PLEASE VERIFY THIS WAS INTENDED.");
      break;

      case(DistributionType.LinearIn):
        // Debug.Log("Applying Linear Distribution Type To Value");
        modifierValue = DistributionEquations.ApplyLinearInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.QuadraticEaseIn):
        modifierValue = DistributionEquations.ApplyQuadraticEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuadraticEaseOut):
        modifierValue = DistributionEquations.ApplyQuadraticEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuadraticEaseInOut):
        modifierValue = DistributionEquations.ApplyQuadraticEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.CubicEaseIn):
        modifierValue = DistributionEquations.ApplyCubicEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.CubicEaseOut):
        modifierValue = DistributionEquations.ApplyCubicEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.CubicEaseInOut):
        modifierValue = DistributionEquations.ApplyCubicEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.QuarticEaseIn):
        modifierValue = DistributionEquations.ApplyQuarticEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuarticEaseOut):
        modifierValue = DistributionEquations.ApplyQuarticEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuarticEaseInOut):
        modifierValue = DistributionEquations.ApplyQuarticEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.QuinticEaseIn):
        modifierValue = DistributionEquations.ApplyQuinticEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuinticEaseOut):
        modifierValue = DistributionEquations.ApplyQuinticEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.QuinticEaseInOut):
        modifierValue = DistributionEquations.ApplyQuinticEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.SinusoidalEaseIn):
        modifierValue = DistributionEquations.ApplySinusoidalEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.SinusoidalEaseOut):
        modifierValue = DistributionEquations.ApplySinusoidalEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.SinusoidalEaseInOut):
        modifierValue = DistributionEquations.ApplySinusoidalEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.ExponentialEaseIn):
        modifierValue = DistributionEquations.ApplyExponentialEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.ExponentialEaseOut):
        modifierValue = DistributionEquations.ApplyExponentialEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.ExponentialEaseInOut):
        modifierValue = DistributionEquations.ApplyExponentialEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.CircularEaseIn):
        modifierValue = DistributionEquations.ApplyCircularEaseInDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.CircularEaseOut):
        modifierValue = DistributionEquations.ApplyCircularEaseOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;
      case(DistributionType.CircularEaseInOut):
        modifierValue = DistributionEquations.ApplyCircularEaseInOutDistributionToValue(currentTime, normalizedStartTime, changeInValue, duration);
      break;

      case(DistributionType.Squared):
        modifierValue = DistributionEquations.ApplySquaredDistributionToValue(currentTime, duration);
      break;

      default:
        Debug.Log("UNKNOWN DISTRIBUTION TYPE TRIED FOR APPLYING A VALUE, PLEASE VERIFY THIS WAS INTENDED.");
      break;
    }

    return (modifierValue*(endTime - startTime));
  }
}
