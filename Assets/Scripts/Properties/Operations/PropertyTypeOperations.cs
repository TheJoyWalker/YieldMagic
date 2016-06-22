using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PropertyTypeOperations
{
    private static Dictionary<Type, Delegate> lerps = new Dictionary<Type, Delegate>()
    {
        { typeof(int),new Func<int, int, float, int>((s,e,p)=>                              (int)(s+(e-s)*p+.5f))   },
        { typeof(float),new Func<float, float, float, float>((s,e,p)=>                      s+(e-s)*p)              },
        { typeof(Vector2),new Func<Vector2, Vector2, float, Vector2>((s,e,p)=>              s+(e-s)*p)              },
        { typeof(Vector3),new Func<Vector3, Vector3, float, Vector3>((s,e,p)=>              s+(e-s)*p)              },
        { typeof(Vector4),new Func<Vector4, Vector4, float, Vector4>                        (Vector4.Lerp)          },
        { typeof(Quaternion),new Func<Quaternion, Quaternion, float, Quaternion>            (Quaternion.Lerp)       },
    };

    public static Func<T, T, float, T> GetLerp<T>()
    {
        var type = typeof(T);
        if (lerps.ContainsKey(type))
            return lerps[type] as Func<T, T, float, T>;
        return null;
    }
}