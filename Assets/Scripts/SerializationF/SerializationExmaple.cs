using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using UnityEngine;

public class SerializationExmaple
{
    void Serialize()
    {
        BinaryFormatter bf = new BinaryFormatter();

        // 1. Construct a SurrogateSelector object
        SurrogateSelector ss = new SurrogateSelector();

        Vector3SerializationSurrogate v3ss = new Vector3SerializationSurrogate();
        ss.AddSurrogate(typeof(Vector3),
                        new StreamingContext(StreamingContextStates.All),
                        v3ss);

        // 2. Have the formatter use our surrogate selector
        bf.SurrogateSelector = ss;
    }

//    public static T DeserializeXML<T>(string text) where T : class
//    {
//        T intantce = default(T);
//        //CommonLogger.WriteLine("DeserializeStruct {0}, loading", path);
//        var serializer = new XmlSerializer(typeof(T),);
//        
//        using (TextReader reader = new StringReader(text))
//            intantce = serializer.Deserialize(reader) as T;
//        return intantce;
//    }
    void Test()
    {
//        XmlSerializer sr = new XmlSerializer(typeof(T));
    }

    SurrogateSelector GetSurrogateSelector()
    {
        var result = new SurrogateSelector();
        result.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All),
            new Vector3SerializationSurrogate());
        result.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All),
            new QuaternionSerializationSurrogate());
        result.AddSurrogate(typeof(Color), new StreamingContext(StreamingContextStates.All),
            new ColorSerializationSurrogate());

        return new SurrogateSelector();
    }
}