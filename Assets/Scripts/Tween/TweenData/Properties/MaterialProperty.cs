using System;
using UnityEngine;

public class MaterialProperty<T> : CustomAccessorProperty<T> where T : struct
{
    public const string GetterPreset = "Get{T}";
    public const string SetterPreset = "Set{T}";

    public override IPropertyUpdater GetUpdater(object rootTargetObject)
    {
        Material material = ResolveToObject(rootTargetObject) as Material;


        Action<string, T> s = Utility.GetMethodObject(string.Format(SetterPreset, typeof(T).Name), material) as Action<string, T>;
        Func<string, T> g = Utility.GetMethodObject(string.Format(GetterPreset, typeof(T).Name), material) as Func<string, T>;

        return new PropertyUpdater<T>()
        {
            Getter = () => g(AccessName),
            Setter = (v) => s(AccessName, v),
            From = FromVaue,
            To = ToValue,
            Lerp = Lerp
        };
    }
}