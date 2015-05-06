using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoTweenHelper {
	protected GameObject objectToTween = null;
	protected float delay = 0f;
	protected float duration = 1f;
	protected UITweener.Method method = UITweener.Method.Linear;
	protected UITweener.Style style = UITweener.Style.Once;
	protected EventDelegate.Callback onFinish = null;
	
	public virtual GoTweenHelper Object(GameObject newGameObject) {
		objectToTween = newGameObject;
		return this;
	}
	
	public virtual GoTweenHelper Delay(float newDelay) {
		delay = newDelay;
		return this;
	}
	public virtual GoTweenHelper Duration(float newDuration) {
		duration = newDuration;
		return this;
	}
	public virtual GoTweenHelper Method(UITweener.Method newMethod) {
		method = newMethod;
		return this;
	}
	public virtual GoTweenHelper Style(UITweener.Style newStyle) {
		style = newStyle;
		return this;
	}
	public virtual GoTweenHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
		onFinish = newOnFinishEvent;
		return this;
	}
	
	public virtual void Tween() {
	}
}

public class GoTweenPositionHelper : GoTweenHelper {
	Vector2 startPosition = Vector2.zero;
	Vector2 endPosition = Vector2.zero;
	
	public new GoTweenPositionHelper Object(GameObject newGameObject) {
		base.Object(newGameObject);
		return this;
	}
	public new GoTweenPositionHelper Delay(float newDelay) {
		base.Delay(newDelay);
		return this;
	}
	public new GoTweenPositionHelper Duration(float newDuration) {
		base.Duration(newDuration);
		return this;
	}
	public new GoTweenPositionHelper Method(UITweener.Method newMethod) {
		base.Method(newMethod);
		return this;
	}
	public new GoTweenPositionHelper Style(UITweener.Style newStyle) {
		base.Style(newStyle);
		return this;
	}
	public new GoTweenPositionHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
		base.OnFinish(newOnFinishEvent);
		return this;
	}
	public GoTweenPositionHelper StartPosition(Vector2 newStartPosition) {
		startPosition = newStartPosition;
		return this;
	}
	public GoTweenPositionHelper StartPosition(float startX, float startY) {
		startPosition = new Vector2(startX, startY);
		return this;
	}
	public GoTweenPositionHelper EndPosition(Vector2 newEndPosition) {
		endPosition = newEndPosition;
		return this;
	}
	public GoTweenPositionHelper EndPosition(float endX, float endY) {
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
		
		// TweenPosition.Begin(objectToTween, duration, new Vector3(endPosition.x, endPosition.y, objectToTween.transform.localPosition.z));
		
		objectToTween = null;
	}
}
//------------------------------------------------------------------------------
// Tween Opacity Helper
//------------------------------------------------------------------------------
public class GoTweenAlphaHelper : GoTweenHelper {
	protected float startAlpha = 1f;
	protected float endAlpha = 1f;
	
	public new GoTweenAlphaHelper Object(GameObject newGameObject) {
		base.Object(newGameObject);
		return this;
	}
	public GoTweenAlphaHelper StartAlpha(float newStartAlpha) {
		startAlpha = Mathf.Clamp(newStartAlpha, 0, 1);
		return this;
	}
	public GoTweenAlphaHelper EndAlpha(float newEndAlpha) {
		endAlpha = Mathf.Clamp(newEndAlpha, 0, 1);
		return this;
	}
	public new GoTweenAlphaHelper Delay(float newDelay) {
		base.Delay(newDelay);
		return this;
	}
	public new GoTweenAlphaHelper Duration(float newDuration) {
		base.Duration(newDuration);
		return this;
	}
	public new GoTweenAlphaHelper Method(UITweener.Method newMethod) {
		base.Method(newMethod);
		return this;
	}
	public new GoTweenAlphaHelper Style(UITweener.Style newStyle) {
		base.Style(newStyle);
		return this;
	}
	public new GoTweenAlphaHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
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
			// tweenAlpha.AddOnFinished(onFinish);
			EventDelegate.Add(tweenAlpha.onFinished, onFinish, true);
		}
		TweenAlpha.Begin(objectToTween, duration, endAlpha);
		
		objectToTween = null;
	}
}

//------------------------------------------------------------------------------
// Tween Scale Helper
//------------------------------------------------------------------------------
public class GoTweenScaleHelper : GoTweenHelper {
	protected Vector3 startScale = new Vector3(1, 1, 1);
	protected Vector3 endScale = new Vector3(1, 1, 1);
	
	public new GoTweenScaleHelper Object(GameObject newGameObject) {
		base.Object(newGameObject);
		return this;
	}
	public GoTweenScaleHelper StartScale(Vector3 newStartScale) {
		// startScale = Mathf.Clamp(newStartAlpha, 0, 1);
		startScale = newStartScale;
		return this;
	}
	public GoTweenScaleHelper EndScale(Vector3 newEndScale) {
		// endScale = Mathf.Clamp(newEndAlpha, 0, 1);
		endScale = newEndScale;
		return this;
	}
	public new GoTweenScaleHelper Delay(float newDelay) {
		base.Delay(newDelay);
		return this;
	}
	public new GoTweenScaleHelper Duration(float newDuration) {
		base.Duration(newDuration);
		return this;
	}
	public new GoTweenScaleHelper Method(UITweener.Method newMethod) {
		base.Method(newMethod);
		return this;
	}
	public new GoTweenScaleHelper Style(UITweener.Style newStyle) {
		base.Style(newStyle);
		return this;
	}
	public new GoTweenScaleHelper OnFinish(EventDelegate.Callback newOnFinishEvent) {
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
public static class GoTweenExecutor {
	static GoTweenPositionHelper tweenPositionHelper = null;
	static GoTweenAlphaHelper tweenAlphaHelper = null;
	static GoTweenScaleHelper tweenScaleHelper = null;
	
	public static GoTweenPositionHelper Position {
		get {
			if(tweenPositionHelper == null) {
				tweenPositionHelper = new GoTweenPositionHelper();
			}
			return tweenPositionHelper;
		}
		set {
			if(tweenPositionHelper == null) {
				tweenPositionHelper = new GoTweenPositionHelper();
			}
		}
	}
	
	public static GoTweenAlphaHelper Alpha {
		get {
			if(tweenAlphaHelper == null) {
				tweenAlphaHelper = new GoTweenAlphaHelper();
			}
			return tweenAlphaHelper;
		}
		set {
			if(tweenAlphaHelper == null) {
				tweenAlphaHelper = new GoTweenAlphaHelper();
			}
		}
	}
	
	public static GoTweenScaleHelper Scale {
		get {
			if(tweenScaleHelper == null) {
				tweenScaleHelper = new GoTweenScaleHelper();
			}
			return tweenScaleHelper;
		}
		set {
			if(tweenScaleHelper == null) {
				tweenScaleHelper = new GoTweenScaleHelper();
			}
		}
	}
}
