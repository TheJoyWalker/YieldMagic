using UnityEngine;

public class SerializationMono : MonoBehaviour
{
    void Start()
    {
        var t = new SerializationTest();
        t.Write();
        //        SerializationTest.Rrite();

    }
}