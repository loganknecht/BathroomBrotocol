using UnityEngine;
using System.Collections;

public class TweenHelper {
    protected GameObject objectToTween = null;
    protected float delay = 0f;
    protected float duration = 1f;
    protected UITweener.Method method = UITweener.Method.Linear;
    protected UITweener.Style style = UITweener.Style.Once;
    protected EventDelegate onFinish = null;
    
    public TweenHelper Object(GameObject newGameObject) {
        objectToTween = newGameObject;
        return this;
    }
    
    public TweenHelper Delay(float newDelay) {
        delay = newDelay;
        return this;
    }
    public TweenHelper Duration(float newDuration) {
        duration = newDuration;
        return this;
    }
    public TweenHelper Method(UITweener.Method newMethod) {
        method = newMethod;
        return this;
    }
    public TweenHelper Style(UITweener.Style newStyle) {
        style = newStyle;
        return this;
    }
    public TweenHelper OnFinish(EventDelegate newOnFinishEvent) {
        onFinish = newOnFinishEvent;
        return this;
    }
    
    public virtual void Tween() {
    }
}
public class TweenPositionHelper : TweenHelper {
    Vector2 startPosition = Vector2.zero;
    Vector2 endPosition = Vector2.zero;
    
    public new TweenPositionHelper Object(GameObject newGameObject) {
        base.Object(newGameObject);
        return this;
    }
    public new TweenPositionHelper Delay(float newDelay) {
        base.Delay(newDelay);
        return this;
    }
    public new TweenPositionHelper Duration(float newDuration) {
        base.Duration(newDuration);
        return this;
    }
    public new TweenPositionHelper Method(UITweener.Method newMethod) {
        base.Method(newMethod);
        return this;
    }
    public new TweenPositionHelper Style(UITweener.Style newStyle) {
        base.Style(newStyle);
        return this;
    }
    public new TweenPositionHelper OnFinish(EventDelegate newOnFinishEvent) {
        base.OnFinish(newOnFinishEvent);
        return this;
    }
    public TweenPositionHelper StartPosition(Vector2 newStartPosition) {
        startPosition = newStartPosition;
        return this;
    }
    public TweenPositionHelper StartPosition(float startX, float startY) {
        startPosition = new Vector2(startX, startY);
        return this;
    }
    public TweenPositionHelper EndPosition(Vector2 newEndPosition) {
        endPosition = newEndPosition;
        return this;
    }
    public TweenPositionHelper EndPosition(float endX, float endY) {
        endPosition = new Vector2(endX, endY);
        return this;
    }
    
    public override void Tween() {
        if(objectToTween.GetComponent<TweenPosition>() == null) {
            objectToTween.AddComponent<TweenPosition>();
        }
        TweenPosition tweenPosition = objectToTween.GetComponent<TweenPosition>();
        
        tweenPosition.delay = delay;
        
        objectToTween.transform.localPosition = new Vector3(startPosition.x, startPosition.y, objectToTween.transform.localPosition.z);
        TweenPosition.Begin(objectToTween, duration, new Vector3(endPosition.x, endPosition.y, objectToTween.transform.localPosition.z)).method = method;
        
        if(onFinish != null) {
            EventDelegate.Add(tweenPosition.onFinished, onFinish);
        }
    }
}

public static class TweenExecutor {
    static TweenPositionHelper tweenPositionHelper = null;
    
    public static TweenPositionHelper Position {
        get {
            if(tweenPositionHelper == null) {
                tweenPositionHelper = new TweenPositionHelper();
            }
            return tweenPositionHelper;
        }
        set {
            if(tweenPositionHelper == null) {
                tweenPositionHelper = new TweenPositionHelper();
            }
        }
    }
    
    // public static void TweenPosition(float startX, float startY, float endX, float endY) {
    //     if(objectToTween.GetComponent<TweenPosition>() == null) {
    //         objectToTween.AddComponent<TweenPosition>();
    //     }
    //     TweenPosition tweenPosition = objectToTween.GetComponent<TweenPosition>();
    
    //     tweenPosition.delay = delay;
    
    //     objectToTween.transform.localPosition = new Vector3(startX, startY, objectToTween.transform.localPosition.z);
    //     // if(easingMethod != null){
    //     TweenPosition.Begin(objectToTween, duration, new Vector3(endX, endY, objectToTween.transform.localPosition.z)).method = easingMethod;
    //     // }
    //     // else {
    //     //   TweenPosition.Begin(objectToTween, duration, new Vector3(endX, endY, objectToTween.transform.localPosition.z));
    //     // }
    
    //     if(onFinishEventDelegate != null) {
    //         EventDelegate.Add(tweenPosition.onFinished, onFinishEventDelegate);
    //     }
    // }
    
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
