using UnityEngine;
using YieldMagic;

public static class ConfigMaterialExtensions
{
    private const string MaterialPrefix = ".material";
    public static void Color(this SerializedConfig.Extensions.AsRenderer ext, Color from, Color to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Color>() { Path = prefix + MaterialPrefix + ".color", From = from, To = to }, BaseYieldTweenProperty.PropertyOffsetMode.FromTo, easingType, easingContainer);
    }
    public static void ColorTo(this SerializedConfig.Extensions.AsRenderer ext, Color to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Color>() { Path = prefix + MaterialPrefix + ".color", To = to }, BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }
    public static void ColorToOffset(this SerializedConfig.Extensions.AsRenderer ext, Color to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "")
    {
        ext.Add(new ReflectedConfigProperty<Color>() { Path = prefix + MaterialPrefix + ".color", To = to }, BaseYieldTweenProperty.PropertyOffsetMode.AddOffset, easingType, easingContainer);
    }

    public static void SetValue<T>(this SerializedConfig.Extensions.AsRenderer ext, string name, T from, T to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "") where T : struct
    {
        ext.Add(new MaterialConfigProperty<T>() { Path = prefix + MaterialPrefix + name, From = from, To = to, }, BaseYieldTweenProperty.PropertyOffsetMode.FromTo, easingType, easingContainer);
    }
    public static void SetValueTo<T>(this SerializedConfig.Extensions.AsRenderer ext, string name, T to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "") where T : struct
    {
        ext.Add(new MaterialConfigProperty<T>() { Path = prefix + MaterialPrefix + name, To = to, }, BaseYieldTweenProperty.PropertyOffsetMode.CurrentTo, easingType, easingContainer);
    }
    public static void SetValueToOffset<T>(this SerializedConfig.Extensions.AsRenderer ext, string name, T to, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null, string prefix = "") where T : struct
    {
        ext.Add(new MaterialConfigProperty<T>() { Path = prefix + MaterialPrefix + name, To = to, }, BaseYieldTweenProperty.PropertyOffsetMode.AddOffset, easingType, easingContainer);
    }
}