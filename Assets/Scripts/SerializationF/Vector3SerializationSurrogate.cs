using System.Runtime.Serialization;
using UnityEngine;

internal sealed class ColorSerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Vector3 object
    public void GetObjectData(object obj,
        SerializationInfo info, StreamingContext context)
    {
        var color = (Color) obj;
        info.AddValue("x", color.r);
        info.AddValue("y", color.g);
        info.AddValue("z", color.b);
        info.AddValue("w", color.a);
        Debug.Log(color);
    }

    // Method called to deserialize a Vector3 object
    public object SetObjectData(object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {
        var color = (Color) obj;
        color.r = (float) info.GetValue("r", typeof (float));
        color.g = (float) info.GetValue("g", typeof (float));
        color.b = (float) info.GetValue("b", typeof (float));
        color.a = (float) info.GetValue("a", typeof (float));
        obj = color;
        return obj; // Formatters ignore this return value //Seems to have been fixed!
    }
}

internal sealed class Vector3SerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Vector3 object
    public void GetObjectData(object obj,
        SerializationInfo info, StreamingContext context)
    {
        var v3 = (Vector3) obj;
        info.AddValue("x", v3.x);
        info.AddValue("y", v3.y);
        info.AddValue("z", v3.z);
        Debug.Log(v3);
    }

    // Method called to deserialize a Vector3 object
    public object SetObjectData(object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {
        var v3 = (Vector3) obj;
        v3.x = (float) info.GetValue("x", typeof (float));
        v3.y = (float) info.GetValue("y", typeof (float));
        v3.z = (float) info.GetValue("z", typeof (float));
        obj = v3;
        return obj; // Formatters ignore this return value //Seems to have been fixed!
    }
}

internal sealed class QuaternionSerializationSurrogate : ISerializationSurrogate
{
    // Method called to serialize a Vector3 object
    public void GetObjectData(object obj,
        SerializationInfo info, StreamingContext context)
    {
        var v3 = (Quaternion) obj;
        info.AddValue("x", v3.x);
        info.AddValue("y", v3.y);
        info.AddValue("z", v3.z);
        info.AddValue("w", v3.w);
        Debug.Log(v3);
    }

    // Method called to deserialize a Vector3 object
    public object SetObjectData(object obj,
        SerializationInfo info, StreamingContext context,
        ISurrogateSelector selector)
    {
        var v3 = (Quaternion) obj;
        v3.x = (float) info.GetValue("x", typeof (float));
        v3.y = (float) info.GetValue("y", typeof (float));
        v3.z = (float) info.GetValue("z", typeof (float));
        v3.w = (float) info.GetValue("w", typeof (float));
        obj = v3;
        return obj; // Formatters ignore this return value //Seems to have been fixed!
    }
}