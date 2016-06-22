using System;

/// <summary>
/// A base class for all properties
/// </summary>
public abstract class BaseYieldTweenProperty
{
    public enum PropertyOffsetMode
    {
        /// <summary>
        /// From value to value
        /// </summary>
        FromTo,
        /// <summary>
        /// from current to value
        /// </summary>
        CurrentTo,
        /// <summary>
        /// from current to current + value
        /// </summary>
        AddOffset,
        /// <summary>
        /// from current to current * value
        /// </summary>
        MulOffset
    }
    public PropertyOffsetMode OffsetMode = PropertyOffsetMode.FromTo;
    public abstract void Initialize(object target);
    public Func<float, float> EasingFunction;

    public abstract void Update(float p);
}