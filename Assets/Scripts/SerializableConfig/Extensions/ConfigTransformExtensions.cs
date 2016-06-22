using UnityEngine;
using YieldMagic;

public static class ConfigTransformExtensions
{
    #region Position extensions
    public static void Move(this SerializedConfig.Extensions.AsTransform ext, Vector3 from, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Vector3>() { Path = prefix + ".position", From = from, To = to, }
                         : new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localPosition", From = from, To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }

    public static void MoveTo(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Vector3>() { Path = prefix + ".position", To = to, }
                         : new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localPosition", To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }

    public static void MoveToOffset(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Vector3>() { Path = prefix + ".position", To = to } :
                           new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localPosition", To = to },
            BaseYieldTweenProperty.PropertyOffsetMode.AddOffset, easingType, easingContainer);
    }
    #endregion
    #region Rotation Extantion
    #region Rotation Vector3 overload
    public static void Rotate(this SerializedConfig.Extensions.AsTransform ext, Vector3 from, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Rotate(Quaternion.Euler(from), Quaternion.Euler(to), isGlobal, easingType, easingContainer, prefix);
    }
    public static void RotateTo(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.RotateToOffset(Quaternion.Euler(to), isGlobal, easingType, easingContainer, prefix);
    }
    public static void RotateToOffset(this SerializedConfig.Extensions.AsTransform ext, Vector3 from, Vector3 to, EasingType easingType = EasingType.Linear, bool isGlobal = true, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.RotateToOffset(Quaternion.Euler(to), isGlobal, easingType, easingContainer, prefix);
    }
    #endregion
    public static void Rotate(this SerializedConfig.Extensions.AsTransform ext, Quaternion from, Quaternion to, bool isGlobal = true,
        EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".rotation", From = from, To = to, }
                         : new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".localRotation", From = from, To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.FromTo, easingType, easingContainer);
    }

    public static void RotateTo(this SerializedConfig.Extensions.AsTransform ext, Quaternion to, bool isGlobal = true,
        EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".rotation", To = to, }
                         : new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".localRotation", To = to },
            BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }
    public static void RotateToOffset(this SerializedConfig.Extensions.AsTransform ext, Quaternion to, bool isGlobal = true,
        EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(isGlobal ? new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".rotation", To = to, }
                         : new ReflectedConfigProperty<Quaternion>() { Path = prefix + ".localRotation", To = to },
            BaseYieldTweenProperty.PropertyOffsetMode.AddOffset, easingType, easingContainer);
    }
    #endregion
    #region Scale extensions
    public static void Scale(this SerializedConfig.Extensions.AsTransform ext, float to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Scale(Vector3.one * to, easingType, easingContainer, prefix);
    }
    public static void Scale(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localScale", To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.MulOffset, easingType, easingContainer);
    }
    public static void Scale(this SerializedConfig.Extensions.AsTransform ext, Vector3 from, Vector3 to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localScale", From = from, To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.FromTo, easingType, easingContainer);
    }
    public static void ScaleTo(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localScale", To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }
    public static void ScaleToOffset(this SerializedConfig.Extensions.AsTransform ext, Vector3 to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Vector3>() { Path = prefix + ".localScale", To = to, },
            BaseYieldTweenProperty.PropertyOffsetMode.AddOffset, easingType, easingContainer);
    }
    #endregion
}