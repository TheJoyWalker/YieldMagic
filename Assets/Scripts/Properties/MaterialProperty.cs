using System;
using UnityEngine;

public class MaterialProperty<T>: CustomAccessorProperty<T> where T : struct
{
    public const string GetterPreset = "Get{T}";
    public const string SetterPreset = "Set{T}";
    public override void UpdateAccessors(object target)
    {
        Material m = target as Material;


        Action<string, T> s = Utility.GetMethodObject(string.Format(SetterPreset, typeof(T).Name), target) as Action<string, T>;
        Func<string, T> g = Utility.GetMethodObject(string.Format(GetterPreset, typeof(T).Name), target) as Func<string, T>;

        Setter = (v) => s(AccessName, v);
        Getter = () => g(AccessName);
    }
}