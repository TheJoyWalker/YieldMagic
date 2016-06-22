using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class UnityReflectionAccessor
{
    private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public;
    private static readonly object[] EmptyObj = new object[0];

    public static object Find(object o, string path)
    {
        var t = o as Transform;
        var com = o as Component;
        if (o != null)
            t = com.transform;
        else
            throw new Exception("no valid component passed to function: " + o);

        var obj = t.Find(path);
        //        Debug.LogFormat("{0} - '{1}' : '{2}'", "Find", o, obj);
        if (obj == null)
            throw new Exception(string.Format("no object found after searching '{0}':'{1}'", t.name, path));

        return obj;
    }

    public static object FindComponent(object o, string typeString)
    {
        //        Debug.LogFormat("{0} - '{1}'", "FindComponent", o);
        Transform t = o as Transform;
        var c = t.gameObject.GetComponents<Component>();
        return c.FirstOrDefault(x => IsSubTypeOrInterface(x.GetType(), typeString));
    }


    public static object Access(object source, string token)
    {
        var type = source.GetType();
        var method = type.GetMethods(BINDING_FLAGS).FirstOrDefault(x => x.Name == token);
        var prop = type.GetProperties(BINDING_FLAGS).FirstOrDefault(x => x.Name == token);
        var field = type.GetFields(BINDING_FLAGS).FirstOrDefault(x => x.Name == token);

        if (method != null && method.GetParameters().Length == 0 && method.ReturnParameter != null && method.ReturnParameter.ParameterType != typeof(void))
            return method.Invoke(source, EmptyObj);

        if (prop != null)
            return prop.GetValue(source, EmptyObj);
        if (field != null)
            return field.GetValue(source);
        throw new Exception(string.Format("field method or property '{0}' - not found in '{1}'", token, source));
        //        return null;
    }
    public static bool IsSubTypeOrInterface(Type source, string compared)
    {
        if (!source.IsInterface)
        {
            var cursor = source;
            while (cursor.BaseType != null)
            {
                if (cursor.Name == compared)
                    return true;
                else
                    cursor = cursor.BaseType;
            }
        }
        else
            return source.GetInterfaces().FirstOrDefault(x => x.Name == compared) != null;
        return false;
    }
}