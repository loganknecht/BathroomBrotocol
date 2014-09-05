using UnityEngine;
using System;
using System.Collections;

public static class DistributionEquations {
  //-------------------------------------------------------------------------------
  // t - current time
  // b - start time
  // c - change in value (wat)
  // d - duration

  public static float ApplyLinearInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return c*t/d + b;
    return changeInValue*currentTime/duration + startTime;

  }
  public static float ApplyQuadraticEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return c*t*t + b;
    currentTime = currentTime/duration;
    return changeInValue*currentTime*currentTime + startTime;
  }
  public static float ApplyQuadraticEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return -c * t*(t-2) + b;
    currentTime = currentTime/duration;
    return (-changeInValue)*currentTime*(currentTime-2) + startTime;

  }
  public static float ApplyQuadraticEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return c/2*t*t + b;
    // t--;
    // return -c/2 * (t*(t-2) - 1) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return changeInValue/2*currentTime*currentTime + startTime;
    }
    currentTime -= 1;
    return (-changeInValue)/2 * (currentTime*(currentTime-2) - 1) + startTime;

  }
  //------------------------------------------------------------------------------
  public static float ApplyCubicEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return c*t*t*t + b;
    currentTime = currentTime/duration;
    return changeInValue*currentTime*currentTime*currentTime + startTime;
  }
  public static float ApplyCubicEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // t--;
    // return c*(t*t*t + 1) + b;
    currentTime = currentTime/duration;
    currentTime -= 1;
    return changeInValue*(currentTime*currentTime*currentTime + 1) + startTime;
  }
  public static float ApplyCubicEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return c/2*t*t*t + b;
    // t -= 2;
    // return c/2*(t*t*t + 2) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return changeInValue/2*currentTime*currentTime*currentTime + startTime;
    }
    else {
      currentTime -= 2;
      return changeInValue/2*(currentTime*currentTime*currentTime + 2) + startTime;
    }
  }
  //------------------------------------------------------------------------------
  public static float ApplyQuarticEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return c*t*t*t*t + b;
    currentTime = currentTime/duration;
    return changeInValue*currentTime*currentTime*currentTime*currentTime + startTime;
  }
  public static float ApplyQuarticEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // t--;
    // return -c * (t*t*t*t - 1) + b;
    currentTime = currentTime/duration;
    currentTime -= 1;
    return (-changeInValue) * (currentTime*currentTime*currentTime*currentTime - 1) + startTime;
  }
  public static float ApplyQuarticEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return c/2*t*t*t*t + b;
    // t -= 2;
    // return -c/2 * (t*t*t*t - 2) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return changeInValue/2*currentTime*currentTime*currentTime*currentTime + startTime;
    }
    else {
      currentTime -= 2;
      return (-changeInValue)/2 * (currentTime*currentTime*currentTime*currentTime - 2) + startTime;
    }
  }
  //------------------------------------------------------------------------------
  public static float ApplyQuinticEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return c*t*t*t*t*t + b;
    currentTime = currentTime/duration;
    return changeInValue*currentTime*currentTime*currentTime*currentTime*currentTime + startTime;
  }
  public static float ApplyQuinticEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // t--;
    // return c*(t*t*t*t*t + 1) + b;
    currentTime = currentTime/duration;
    currentTime -= 1;
    return changeInValue * (currentTime*currentTime*currentTime*currentTime*currentTime + 1) + startTime;
  }
  public static float ApplyQuinticEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return c/2*t*t*t*t*t + b;
    // t -= 2;
    // return c/2*(t*t*t*t*t + 2) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return changeInValue/2*currentTime*currentTime*currentTime*currentTime*currentTime + startTime;
    }
    else {
      currentTime -= 2;
      return changeInValue/2 * (currentTime*currentTime*currentTime*currentTime*currentTime + 2) + startTime;
    }
  }
  //------------------------------------------------------------------------------
  public static float ApplySinusoidalEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return -c * Math.Cos(t/d * (float)(Math.PI/2)) + c + b;
    return (-changeInValue) * (float)Math.Cos(currentTime/duration * (float)(Math.PI/2)) + changeInValue + startTime;
  }
  public static float ApplySinusoidalEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return c * Math.Sin(t/d * (float)(Math.PI/2)) + b;
    return changeInValue * (float)Math.Sin(currentTime/duration * (float)(Math.PI/2)) + startTime;
  }
  public static float ApplySinusoidalEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return -c/2 * (Math.Cos(Math.PI*t/d) - 1) + b;
    return (-changeInValue)/2 * (float)(Math.Cos(Math.PI * currentTime/duration) - 1) + startTime;
  }
  //------------------------------------------------------------------------------
  public static float ApplyExponentialEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return c * Math.Pow( 2, 10 * (t/d - 1) ) + b;
    return changeInValue * (float)Math.Pow(2, (10 * (currentTime/duration - 1)) ) + startTime;
  }
  public static float ApplyExponentialEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // return c * ( -Math.Pow( 2, -10 * t/d ) + 1 ) + b;
    return changeInValue * (float)(-Math.Pow( 2, (-10 * currentTime/duration)) + 1 ) + startTime;
  }
  public static float ApplyExponentialEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return c/2 * Math.Pow( 2, 10 * (t - 1) ) + b;
    // t--;
    // return c/2 * ( -Math.Pow( 2, -10 * t) + 2 ) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return changeInValue/2 * (float)Math.Pow( 2, (10 * (currentTime - 1)) ) + startTime;
    }
    else {
      currentTime -= 1;
      return changeInValue/2 * (float)( -Math.Pow( 2, (-10 * currentTime) ) + 2 ) + startTime;
    }
  }
  //------------------------------------------------------------------------------
  public static float ApplyCircularEaseInDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // return -c * (Math.Sqrt(1 - t*t) - 1) + b;
    currentTime = currentTime/duration;
    return -changeInValue * (float)(Math.Sqrt(1 - currentTime * currentTime) - 1) + startTime;
  }
  public static float ApplyCircularEaseOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d;
    // t--;
    // return c * Math.Sqrt(1 - t*t) + b;
    currentTime = currentTime/duration;
    currentTime -= 1;
    return changeInValue * (float)Math.Sqrt(1 - currentTime * currentTime) + startTime;
  }
  public static float ApplyCircularEaseInOutDistributionToValue(float currentTime, float startTime, float changeInValue, float duration) {
    // t /= d/2;
    // if (t < 1) return -c/2 * (Math.Sqrt(1 - t*t) - 1) + b;
    // t -= 2;
    // return c/2 * (Math.Sqrt(1 - t*t) + 1) + b;
    currentTime = currentTime/(duration/2);
    if(currentTime < 1) {
      return (-changeInValue)/2 * (float)(Math.Sqrt(1 - currentTime*currentTime) - 1) + startTime;
    }
    else {
      currentTime -= 2;
      return changeInValue/2 * (float)(Math.Sqrt(1 - currentTime*currentTime) + 1) + startTime;
    }
  }
  //------------------------------------------------------------------------------
  public static float ApplySquaredDistributionToValue(float currentTime, float duration) {
    return ((currentTime * currentTime)/duration);
  }
}
