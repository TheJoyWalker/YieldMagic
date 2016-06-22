using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;
using YieldMagic;

[Serializable]
public class ReflectedConfigProperty<T> : ConfigProp where T : struct
{
    public T From;
    public T To;
    public override object GetTweenProperty()
    {
        return new ReflectedProperty<T>() { FromVaue = From, ToValue = To };
    }
}

public class MaterialConfigProperty<T> : ReflectedConfigProperty<T> where T : struct
{
    public override object GetTweenProperty()
    {
        return new MaterialProperty<T>() { FromVaue = From, ToValue = To };
    }
}

[XmlInclude(typeof(ReflectedConfigProperty<Vector3>))]
public abstract class ConfigProp
{
    /// <summary>
    /// If From value was specified
    /// or we sould get it from getter
    /// </summary>
    public BaseYieldTweenProperty.PropertyOffsetMode OffsetMode = BaseYieldTweenProperty.PropertyOffsetMode.FromTo;
    public string Path;
    public EasingType EasingType;
    public string EasingContainerQualifiedTypeName;

    public IEasingContainer EasingContainer
    {
        get { return (IEasingContainer)Activator.CreateInstance(EasingContainerType); }
        set
        {
            EasingContainerQualifiedTypeName = value.GetType().AssemblyQualifiedName;
            EasingType = EasingType.Custom;
        }
    }

    public Type EasingContainerType
    {
        get { return Type.GetType(EasingContainerQualifiedTypeName); }
        set { EasingContainerQualifiedTypeName = value.AssemblyQualifiedName; }
    }


    //    [XmlIgnore]
    //    public Type UnderlyingType { get { return GetType(); } }
    public virtual object GetTweenProperty()
    {
        return null;
    }
}

public class SerializationTest
{
    string xmlString;
    [Serializable]
    [XmlRoot(ElementName = "Root", Namespace = "", IsNullable = false)]
    public class Container
    {

        //        [XmlElement("asd")]
        //        SerializableConfigProperty property = new SerializableConfigProperty() { From = Vector2.one };
        public ConfigProp[] Data = {
            new ReflectedConfigProperty<Vector3>() { From = Vector3.back}
        };
        //        public ConfigProperty<Vector3>[] V3Data = {
        //            new SerializableConfigProperty() { From = Vector3.zero, UnderlyingType = typeof(Vector3) }
        //        };
    }

    Container data = new Container();
    public void Write()
    {
        Serialize(data, Application.dataPath + "/Resources/TweenConfig.xml");
        var c = Deserialize<Container>("TweenConfig");
    }

    public static T Deserialize<T>(string path) where T : class
    {
        TextAsset textAsset = (TextAsset)Resources.Load(path, typeof(TextAsset));
        return DeserializeXML<T>(textAsset.text);
    }

    public static T DeserializeXML<T>(string text) where T : class
    {
        T intantce = default(T);
        //CommonLogger.WriteLine("DeserializeStruct {0}, loading", path);
        var serializer = new XmlSerializer(typeof(T));

        using (TextReader reader = new StringReader(text))
            intantce = serializer.Deserialize(reader) as T;
        return intantce;
    }

    public void Serialize<T>(T DataContainer, string path)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        FileStream stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, DataContainer);
        stream.Close();
        //        FileStream stream = new FileStream(path, FileMode.Create);
        //            serializer.Serialize(stream, DataContainer);
        //        stream.Close();
    }
}