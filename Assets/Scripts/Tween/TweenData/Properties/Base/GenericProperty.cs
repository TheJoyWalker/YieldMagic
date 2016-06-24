using System;
using System.Diagnostics;
using YieldMagic;

public abstract class GenericProperty<T> : BaseYieldTweenProperty where T : struct
{
    /// <summary>
    /// if use offset mode - will require Getter of Field
    /// </summary>
    public T FromVaue;
    public T ToValue;

    protected Func<T, T, float, T> Lerp = PropertyTypeOperations.GetLerp<T>();
}