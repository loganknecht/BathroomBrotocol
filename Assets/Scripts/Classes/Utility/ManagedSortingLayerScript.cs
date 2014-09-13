using UnityEngine;
using System.Collections;

public enum Axis {
  X,
  Y
}

public enum Sign {
  Positive,
  Negative
}

public class ManagedSortingLayerScript : MonoBehaviour {

  public GameObject gamobjectToBaseSortingLayerOn = null;
  public int sortingLayerOffset = 0;
  public Axis axisToBaseLayerCalculationOn = Axis.Y;
  public Sign layerOrderingSign = Sign.Negative;
  public Sign sortingLayerOffsetSign = Sign.Negative;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
    if(gamobjectToBaseSortingLayerOn == null) {
      if(axisToBaseLayerCalculationOn == Axis.X) {
        // Debug.Log("calculating x axis sorting layer");
        // Debug.Log("X based: " + Mathf.RoundToInt(this.gameObject.transform.position.x * 100f));
        // Debug.Log("Y based: " + Mathf.RoundToInt(this.gameObject.transform.position.y * 100f));
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = SetToCorrectSign(Mathf.RoundToInt(this.gameObject.transform.position.x * 100f), layerOrderingSign) + SetToCorrectSign(sortingLayerOffset, sortingLayerOffsetSign);
      }
      else if(axisToBaseLayerCalculationOn == Axis.Y) {
        // Debug.Log("calculating y axis sorting layer");
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = SetToCorrectSign(Mathf.RoundToInt(this.gameObject.transform.position.y * 100f), layerOrderingSign) + SetToCorrectSign(sortingLayerOffset, sortingLayerOffsetSign);
      }
    }
    else {
      axisToBaseLayerCalculationOn = gamobjectToBaseSortingLayerOn.GetComponent<ManagedSortingLayerScript>().axisToBaseLayerCalculationOn;
      layerOrderingSign = gamobjectToBaseSortingLayerOn.GetComponent<ManagedSortingLayerScript>().layerOrderingSign;
      sortingLayerOffsetSign = gamobjectToBaseSortingLayerOn.GetComponent<ManagedSortingLayerScript>().sortingLayerOffsetSign;

      if(axisToBaseLayerCalculationOn == Axis.X) {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = SetToCorrectSign(Mathf.RoundToInt(gamobjectToBaseSortingLayerOn.transform.position.x * 100f), layerOrderingSign) + SetToCorrectSign(sortingLayerOffset, sortingLayerOffsetSign);
      }
      else if(axisToBaseLayerCalculationOn == Axis.Y) {
        this.gameObject.GetComponent<SpriteRenderer>().sortingOrder = SetToCorrectSign(Mathf.RoundToInt(gamobjectToBaseSortingLayerOn.transform.position.y * 100f), layerOrderingSign) + SetToCorrectSign(sortingLayerOffset, sortingLayerOffsetSign);
      }
    }
	}

  public int SetToCorrectSign(int valueToCorrect, Sign signToSetTo) {
    if(signToSetTo == Sign.Positive) {
      if(valueToCorrect < 0) {
        valueToCorrect = valueToCorrect*-1;
      }
    }
    else if(signToSetTo == Sign.Negative) {
      if(valueToCorrect > 0) {
        valueToCorrect = valueToCorrect*-1;
      }
    }
    else {
      // do nothing
    }

    return valueToCorrect;
  }

  public void SetAxisToBaseCalculationOn(Axis newAxis) {
    axisToBaseLayerCalculationOn = newAxis;
  }

  public void SetLayerOrderingSign(Sign newSign) {
    layerOrderingSign = newSign;
  }

  public void SetSortingLayerOffsetSign(Sign newSign) {
    sortingLayerOffsetSign = newSign;
  }
}
