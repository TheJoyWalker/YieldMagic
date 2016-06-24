using System;
using UnityEngine;
using YieldMagic;
public sealed class YieldTween : YieldBase
{
    private IPropertyUpdater[] _updaters;

    private SerializedConfig _config;

    public SerializedConfig Config
    {
        get { return _config; }
        set
        {
            _config = value;
            Duration = _config.Duration;

            var cProps = _config.GetProperties();
            _props = new BaseYieldTweenProperty[cProps.Length];
            for (int i = 0; i < cProps.Length; i++)
            {
                _props[i] = (BaseYieldTweenProperty)cProps[i].GetTweenProperty();
            }
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
        //        throw new System.NotImplementedException();
    }
}