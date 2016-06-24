using System;
using System.Collections.Generic;
using System.Linq;
using YieldMagic;

public class YieldTweenData
{
    private string Path = string.Empty;
    private readonly YieldTweenData _owner;

    public float Duration;
    private readonly List<BaseYieldTweenProperty> _properties = new List<BaseYieldTweenProperty>();
    public IPropertyUpdater[] GetUpdaters(object o)
    {
        return _properties.Select(x => x.GetUpdater(o)).ToArray();
    }
    public Extensions.AsTransform AsTransform;
    public Extensions.AsRenderer AsRenderer;
    public Extensions.AsGameObject AsGameObject;

    public YieldTweenData()
    {
        _owner = this;
        InitializeExtensions();
    }
    private YieldTweenData(YieldTweenData owner)
    {
        _owner = owner;
        InitializeExtensions();
    }

    private void InitializeExtensions()
    {
        AsTransform = new Extensions.AsTransform(_owner);
        AsRenderer = new Extensions.AsRenderer(_owner);
        AsGameObject = new Extensions.AsGameObject(_owner);
    }

    public class Extensions
    {
        public class AsGameObject : ConfigExtension
        {
            public AsGameObject(YieldTweenData owner) : base(owner) { }
        }
        public class AsTransform : ConfigExtension
        {
            public AsTransform(YieldTweenData owner) : base(owner) { }
        }
        public class AsRenderer : ConfigExtension
        {
            public AsRenderer(YieldTweenData owner) : base(owner) { }
        }
        public class ConfigExtension
        {
            public readonly YieldTweenData Owner;
            public string Prefix = string.Empty;
            public ConfigExtension(YieldTweenData owner) { Owner = owner; }
            public AsTransform ToTransform() { return new AsTransform(Owner) { Prefix = ".transform" }; }
            public AsRenderer ToRenderer() { return new AsRenderer(Owner) { Prefix = "[Renderer]" }; }
            public void Add(BaseYieldTweenProperty prop, BaseYieldTweenProperty.PropertyOffsetMode offsetMode, EasingType easingType = EasingType.Linear, Type easingContainerType = null)
            {
                prop.Path = Owner.Path + Prefix + prop.Path;
                prop.OffsetMode = offsetMode;
                prop.EasingType = easingType;
                prop.EasingContainerType = easingContainerType;
                Owner.Add(prop);
            }
        }
    }

    public YieldTweenData Get(string path)
    {
        return new YieldTweenData(this) { Path = path };
    }

    public void Add(BaseYieldTweenProperty prop)
    {
        if (_owner == null)
            _properties.Add(prop);
        else
            _owner.Add(prop);
    }
}