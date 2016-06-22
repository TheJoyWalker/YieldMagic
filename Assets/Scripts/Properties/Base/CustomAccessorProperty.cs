using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CustomAccessorProperty<T> : GenericProperty<T> where T : struct
{
    public string AccessName;
    public override void Initialize(object target)
    {
        UpdateAccessors(target);
    }

    public abstract void UpdateAccessors(object target);
}