using System.Collections;
using UnityEngine;
using YieldMagic;

public class PathTest : MonoBehaviour
{
    public string Method() { return Field; }
    public string Getter { get { return Field; } }
    public string Field = "Field";
    void Start()
    {
        //        Log("Method");
        //        Log("Getter");
        //        Log("Field");
        //        Log("transform");
        Debug.Log(UnityPathResolver.Resolve(this, "(c)(c2((x))/Sphere)[Renderer].material.color"));
        //        Debug.Log(UnityPathResolver.Resolve(this, "(((x))/Sphere)"));
        var c = new SerializedConfig();

        c.AsTransform.MoveTo(Vector3.down, EasingType.BackIn);

        YieldBase.Get<WaitTimeOrCondition>().Override(onUpdated: (me) =>
        {
            transform.position = Vector3.Lerp(Vector3.zero, Vector3.down, me.EasedValue);
        });
    }

    private void Log(string a)
    {
        Debug.LogFormat("{0} value is  '{1}'", a, UnityReflectionAccessor.Access(this, a));
    }
}