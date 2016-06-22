using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameObjectTweenConfig
{
    private GameObject _root;
    public GameObject Root
    {
        get { return _root; }
        set
        {
            _root = value;
            UpdateBinding();
        }
    }

    public void UpdateBinding()
    {
        Renderer = _root.GetComponent<Renderer>();
        Transform = _root.transform;
        Material = Renderer.material;
        Materials = Renderer.materials;
    }

    public Renderer Renderer;
    public Material Material;
    public Material[] Materials;
    public Transform Transform;
    public GameObject GameObject;

//    private Dictionary<string, ConfigEntry> _entries = new Dictionary<string, ConfigEntry>();


//    public struct Transform
//    {
//        public float[] position;
//        public float[] rotation;
//        public float[] scale;
//    }

    public class TransformConfig
    {
        public Vector3 position;


/*        public Move(Vector3 from, Vector3 to, float time, float delay)
        {
            TweenEngine.Transform.Move(params);
        }*/
    }
}