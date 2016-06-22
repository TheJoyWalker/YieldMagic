using UnityEngine;
public class DummyMonoBehaviour : MonoBehaviour
{
    public Vector3 V3Field;

    [SerializeField]
    private Vector3 _v3Prop;
    public Vector3 V3prop
    {
        get { return _v3Prop; }
        set { _v3Prop = value; }
    }

    [SerializeField]
    private Vector3 _v3Method;
    public void V3Method(Vector3 v3)
    {
        _v3Method = v3;
    }

    [SerializeField]
    private Vector3 _v3Method2;
    public Vector3 V3Method2(Vector3 v3)
    {
        _v3Method = v3;
        return v3;
    }
}