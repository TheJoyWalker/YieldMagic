using System;
using UnityEngine;
using YieldMagic;
public sealed class YieldTween : YieldBase
{
    private IPropertyUpdater[] _updaters;
    private YieldTweenData _data;

    public YieldTweenData Data
    {
        get { return _data; }
        set
        {
            _data = value;
            Duration = value.Duration;
            _updaters = value.GetUpdaters(_target);
        }
    }
    
    private BaseYieldTweenProperty[] _props = new BaseYieldTweenProperty[0];
    private readonly object _target;

    public YieldTween(object target, float duration)
    {
        _target = target;
        Duration = duration;
    }

    public YieldTween AddProperty(BaseYieldTweenProperty prop)
    {
        BaseYieldTweenProperty[] newProps = new BaseYieldTweenProperty[_props.Length + 1];
        newProps[0] = prop;
        _props.CopyTo(newProps, 1);
        _props = newProps;
        return this;
    }

    protected override void Update()
    {
        //        foreach (var prop in _props)
        //            prop.Update((prop.EasingFunction == null) ? EasedValue : prop.EasingFunction(Progress));
        //prop.Apply(T _target, float value)=>propSetter(Target, Ease(start,end));
        for (int i = 0; i < _updaters.Length; i++)
            _updaters[i].Update(Progress);
    }

    protected override void OnRewind()
    {
    }
}