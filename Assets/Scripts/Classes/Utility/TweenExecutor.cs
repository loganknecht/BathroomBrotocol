using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TweenHelper {
    protected GameObject objectToTween = null;
    protected float delay = 0f;
    protected float duration = 1f;
    protected UITweener.Method method = UITweener.Method.Linear;
    protected UITweener.Style style = UITweener.Style.Once;
    protected EventDelegate.Callback onFinish = null;
    
    public virtual TweenHelper Object(GameObject newGameObject) {
        objectToTween = newGameObject;
        return this;
    }
    
    public virtual TweenHelper Delay(float newDelay) {
        delay = newDelay;
        return this;
    }
    public virtual TweenHelper Duration(float newDuration) {
        duration = newDuration;
        return this;
    }
    public virtual TweenHelper Method(UITweener.Method newMethod) {
        method = newMethod;
        return this;
    }
    public virtual TweenHelper Style(UITweener.Style newStyle) {
        style = newStyle;
        return this;
    }
    public virtual TweenHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
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
    public new TweenPositionHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
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
        
        tweenPosition.from = startPosition;
        tweenPosition.to = endPosition;
        tweenPosition.delay = delay;
        tweenPosition.style = style;
        tweenPosition.method = method;
        
        objectToTween.transform.localPosition = new Vector3(startPosition.x, startPosition.y, objectToTween.transform.localPosition.z);
        
        if(onFinish != null) {
            tweenPosition.ResetToBeginning();
            EventDelegate.Add(tweenPosition.onFinished, onFinish, true);
        }
        
        TweenPosition.Begin(objectToTween, duration, new Vector3(endPosition.x, endPosition.y, objectToTween.transform.localPosition.z));
        
        objectToTween = null;
    }
}
//------------------------------------------------------------------------------
// Tween Opacity Helper
//------------------------------------------------------------------------------
public class TweenAlphaHelper : TweenHelper {
    protected float startAlpha = 1f;
    protected float endAlpha = 1f;
    
    public new TweenAlphaHelper Object(GameObject newGameObject) {
        base.Object(newGameObject);
        return this;
    }
    public TweenAlphaHelper StartAlpha(float newStartAlpha) {
        startAlpha = Mathf.Clamp(newStartAlpha, 0, 1);
        return this;
    }
    public TweenAlphaHelper EndAlpha(float newEndAlpha) {
        endAlpha = Mathf.Clamp(newEndAlpha, 0, 1);
        return this;
    }
    public new TweenAlphaHelper Delay(float newDelay) {
        base.Delay(newDelay);
        return this;
    }
    public new TweenAlphaHelper Duration(float newDuration) {
        base.Duration(newDuration);
        return this;
    }
    public new TweenAlphaHelper Method(UITweener.Method newMethod) {
        base.Method(newMethod);
        return this;
    }
    public new TweenAlphaHelper Style(UITweener.Style newStyle) {
        base.Style(newStyle);
        return this;
    }
    public new TweenAlphaHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
        base.OnFinish(newOnFinishEvent);
        return this;
    }
    
    public override void Tween() {
        if(objectToTween.GetComponent<TweenAlpha>() == null) {
            objectToTween.AddComponent<TweenAlpha>();
        }
        TweenAlpha tweenAlpha = objectToTween.GetComponent<TweenAlpha>();
        
        tweenAlpha.delay = delay;
        tweenAlpha.value = startAlpha;
        tweenAlpha.from = startAlpha;
        tweenAlpha.method = method;
        tweenAlpha.style = style;
        
        //TO DO RESET THE SPRITES ALPHA VALUE BEFORE TWEENING?
        if(onFinish != null) {
            tweenAlpha.ResetToBeginning();
            EventDelegate.Add(tweenAlpha.onFinished, onFinish, true);
        }
        
        TweenAlpha.Begin(objectToTween, duration, endAlpha);
        
        objectToTween = null;
    }
}

//------------------------------------------------------------------------------
// Tween Scale Helper
//------------------------------------------------------------------------------
public class TweenScaleHelper : TweenHelper {
    protected Vector3 startScale = new Vector3(1, 1, 1);
    protected Vector3 endScale = new Vector3(1, 1, 1);
    
    public new TweenScaleHelper Object(GameObject newGameObject) {
        base.Object(newGameObject);
        return this;
    }
    public TweenScaleHelper StartScale(Vector3 newStartScale) {
        // startScale = Mathf.Clamp(newStartAlpha, 0, 1);
        startScale = newStartScale;
        return this;
    }
    public TweenScaleHelper EndScale(Vector3 newEndScale) {
        // endScale = Mathf.Clamp(newEndAlpha, 0, 1);
        endScale = newEndScale;
        return this;
    }
    public new TweenScaleHelper Delay(float newDelay) {
        base.Delay(newDelay);
        return this;
    }
    public new TweenScaleHelper Duration(float newDuration) {
        base.Duration(newDuration);
        return this;
    }
    public new TweenScaleHelper Method(UITweener.Method newMethod) {
        base.Method(newMethod);
        return this;
    }
    public new TweenScaleHelper Style(UITweener.Style newStyle) {
        base.Style(newStyle);
        return this;
    }
    public new TweenScaleHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
        base.OnFinish(newOnFinishEvent);
        return this;
    }
    
    public override void Tween() {
        if(objectToTween.GetComponent<TweenScale>() == null) {
            objectToTween.AddComponent<TweenScale>();
        }
        TweenScale tweenScale = objectToTween.GetComponent<TweenScale>();
        // Add this in if it doesn't start at the right scale
        // objectToTween.transform.localScale = startScale;
        
        tweenScale.delay = delay;
        tweenScale.value = startScale;
        tweenScale.from = startScale;
        tweenScale.method = method;
        tweenScale.style = style;
        
        if(onFinish != null) {
            tweenScale.ResetToBeginning();
            EventDelegate.Add(tweenScale.onFinished, onFinish, true);
        }
        
        TweenScale.Begin(objectToTween, duration, endScale);
        
        objectToTween = null;
    }
}
public static class TweenExecutor {
    static TweenPositionHelper tweenPositionHelper = null;
    static TweenAlphaHelper tweenAlphaHelper = null;
    static TweenScaleHelper tweenScaleHelper = null;
    
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
    
    public static TweenAlphaHelper Alpha {
        get {
            if(tweenAlphaHelper == null) {
                tweenAlphaHelper = new TweenAlphaHelper();
            }
            return tweenAlphaHelper;
        }
        set {
            if(tweenAlphaHelper == null) {
                tweenAlphaHelper = new TweenAlphaHelper();
            }
        }
    }
    
    public static TweenScaleHelper Scale {
        get {
            if(tweenScaleHelper == null) {
                tweenScaleHelper = new TweenScaleHelper();
            }
            return tweenScaleHelper;
        }
        set {
            if(tweenAlphaHelper == null) {
                tweenAlphaHelper = new TweenAlphaHelper();
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
    
    // public static void TweenObjectAlpha(GameObject objectToTween, float startOpacity, float endOpacity, float delay, float duration, UITweener.Method easingMethod, EventDelegate eventDelegate) {
    //     if(objectToTween.GetComponent<TweenAlpha>() == null) {
    //         objectToTween.AddComponent<TweenAlpha>();
    //     }
    //     TweenAlpha tweenAlpha = objectToTween.GetComponent<TweenAlpha>();
    
    //     // Debug.Log("Tweening: " + objectToTween.name);
    //     tweenAlpha.delay = delay;
    
    //     tweenAlpha.value = startOpacity;
    //     // objectToTween.GetComponent<UIRect>().alpha = startOpacity;
    
    //     tweenAlpha.from = startOpacity;
    
    //     TweenAlpha.Begin(objectToTween, duration, endOpacity).method = easingMethod;
    
    //     if(eventDelegate != null) {
    //         EventDelegate.Add(tweenAlpha.onFinished, eventDelegate);
    //     }
    // }
    
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
