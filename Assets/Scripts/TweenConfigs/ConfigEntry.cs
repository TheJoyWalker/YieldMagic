using System.IO;
using System.Xml.Serialization;

public class ConfigEntry
{
    public string Path;
    //    private BaseYieldTweenProperty<TObjectType>[] _props = new BaseYieldTweenProperty<TObjectType>[0];
    public object[] PropertyDescriptors = new object[0];

    public static T Desirialize<T>(string text) where T : class
    {
        T intantce = null;
        //CommonLogger.WriteLine("DeserializeStruct {0}, loading", path);
        var serializer = new XmlSerializer(typeof(T));
        using (TextReader reader = new StringReader(text))
        {
            intantce = serializer.Deserialize(reader) as T;
        }
        return intantce;
    }
}