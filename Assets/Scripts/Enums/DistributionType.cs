public enum DistributionType {
  None,

  //----------------------------------------------------------------------------
  // These are specifically for determining how to choose the points for selection
  // They do not determine the shape of the generation to take
  //----------------------------------------------------------------------------
  Uniform,
  Random,
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // These are specific how the distribution should shape the equation
  // These will determine how to return the respective values which should
  // fit the points to some distribution
  //----------------------------------------------------------------------------
  LinearIn,

  QuadraticEaseIn,
  QuadraticEaseOut,
  QuadraticEaseInOut,

  CubicEaseIn,
  CubicEaseOut,
  CubicEaseInOut,

  QuarticEaseIn,
  QuarticEaseOut,
  QuarticEaseInOut,

  QuinticEaseIn,
  QuinticEaseOut,
  QuinticEaseInOut,

  SinusoidalEaseIn,
  SinusoidalEaseOut,
  SinusoidalEaseInOut,

  ExponentialEaseIn,
  ExponentialEaseOut,
  ExponentialEaseInOut,

  CircularEaseIn,
  CircularEaseOut,
  CircularEaseInOut,

  Squared
}
