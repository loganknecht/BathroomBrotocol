using UnityEngine;
using System.Collections;

public static class TweenExecutor {
  public static void TweenObjectPosition(GameObject objectToTween, float startX, float startY, float endX, float endY, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    if(objectToTween.GetComponent<TweenPosition>() == null) {
      objectToTween.AddComponent<TweenPosition>();
    }
    TweenPosition tweenPosition = objectToTween.GetComponent<TweenPosition>();

    tweenPosition.delay = delay;

    objectToTween.transform.localPosition = new Vector3(startX, startY, objectToTween.transform.localPosition.z);
    // if(easingMethod != null){
    TweenPosition.Begin(objectToTween, duration, new Vector3(endX, endY, objectToTween.transform.localPosition.z)).method = easingMethod;
    // }
    // else {
    //   TweenPosition.Begin(objectToTween, duration, new Vector3(endX, endY, objectToTween.transform.localPosition.z));
    // }

    if(eventDelegate != null) {
      EventDelegate.Add(tweenPosition.onFinished, eventDelegate);
    }
  }

  public static void TweenObjectAlpha(GameObject objectToTween, float startOpacity, float endOpacity, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    if(objectToTween.GetComponent<TweenAlpha>() == null) {
      objectToTween.AddComponent<TweenAlpha>();
    }
    TweenAlpha tweenAlpha = objectToTween.GetComponent<TweenAlpha>();

    // Debug.Log("Tweening: " + objectToTween.name);
    tweenAlpha.delay = delay;

    tweenAlpha.value = startOpacity;
    // objectToTween.GetComponent<UIRect>().alpha = startOpacity;

    tweenAlpha.from = startOpacity;

    TweenAlpha.Begin(objectToTween, duration, endOpacity).method = easingMethod;

    if(eventDelegate != null) {
      EventDelegate.Add(tweenAlpha.onFinished, eventDelegate);
    }
  }

  public static void TweenObjectColor(GameObject objectToTween, Color startColor, Color endColor, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    // Debug.Log("Tweening: " + objectToTween.name);
    if(objectToTween.GetComponent<TweenColor>() == null) {
      objectToTween.AddComponent<TweenColor>();
    }
    TweenColor tweenColor = objectToTween.GetComponent<TweenColor>();
    tweenColor.delay = delay;

    tweenColor.value = startColor;

    tweenColor.from = startColor;

    TweenColor.Begin(objectToTween, duration, endColor).method = easingMethod;

    if(eventDelegate != null) {
      EventDelegate.Add(tweenColor.onFinished, eventDelegate);
    }
  }
}
