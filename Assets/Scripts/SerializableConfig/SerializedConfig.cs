using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using UnityEngine;
using YieldMagic;

public class SerializedConfig
{
    private string Path = string.Empty;
    private readonly SerializedConfig _owner;

    public float Duration;
    private readonly List<ConfigProp> _properties = new List<ConfigProp>();
    public ConfigProp[] GetProperties() { return _properties.ToArray(); }

    public Extensions.AsTransform AsTransform;
    public Extensions.AsRenderer AsRenderer;
    public Extensions.AsGameObject AsGameObject;

    public SerializedConfig()
    {
        _owner = this;
        InitializeExtensions();
    }
    private SerializedConfig(SerializedConfig owner)
    {
        _owner = owner;
        InitializeExtensions();
    }

    private void InitializeExtensions()
    {
        AsTransform = new Extensions.AsTransform(_owner);
    }

    public SerializedConfig Get(string path)
    {
        return new SerializedConfig(this) { Path = path };
    }

    public class Extensions
    {
        public class AsGameObject : ConfigExtension
        {
            public AsGameObject(SerializedConfig owner) : base(owner) { }
        }
        public class AsTransform : ConfigExtension
        {
            public AsTransform(SerializedConfig owner) : base(owner) { }
        }
        public class AsRenderer : ConfigExtension
        {
            public AsRenderer(SerializedConfig owner) : base(owner) { }
        }
        public class ConfigExtension
        {
            public readonly SerializedConfig Owner;
            public string Prefix = string.Empty;
            public ConfigExtension(SerializedConfig owner) { Owner = owner; }
            public AsTransform ToTransform() { return new AsTransform(Owner) { Prefix = ".transform" }; }
            public AsRenderer ToRenderer() { return new AsRenderer(Owner) { Prefix = "[Renderer]" }; }
            public void Add(ConfigProp prop, BaseYieldTweenProperty.PropertyOffsetMode offsetMode, EasingType easingType = EasingType.Linear, IEasingContainer easingContainer = null)
            {
                prop.Path = Owner.Path + Prefix + prop.Path;
                prop.OffsetMode = offsetMode;
                prop.EasingType = easingType;
                prop.EasingContainer = easingContainer;
                Owner.Add(prop);
            }
        }
    }

    public void Add(ConfigProp prop)
    {
        if (_owner == null)
            _properties.Add(prop);
        else
            _owner.Add(prop);
    }
}

public class ConfigUseCase
{
    public ConfigUseCase()
    {
        SerializedConfig config = new SerializedConfig();
        var cc = config.Get("(c).gameObject").AsGameObject.ToTransform();
        cc.Move(Vector3.back, Vector3.back);
        //        config.AsTranform.Position();
        //        config.Properties.Add(new ReflectedConfigProperty<Vector3>()
        //        {
        //            To = Vector3.one,
        //            Path = "transform.position"
        //        }
        //        );
        //        config.Properties.Add(new ReflectedConfigProperty<Color>()
        //        {
        //            To = Color.black,
        //            Path = "[Renderer]Material.Color"
        //        });
        //        config.Properties.Add(new ReflectedConfigProperty<Vector3>()
        //        {
        //            To = Vector3.zero,
        //            Path = "(child)transform.position"
        //        });
        //        config.Properties.Add(new MaterialConfigProperty<Color>()
        //        {
        //            To = Color.black,
        //            Path = "(c/c((x)))[Renderer].material._color"
        //        });
    }
}