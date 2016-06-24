using System;
using System.Collections.Generic;
using System.Linq;
using YieldMagic;

/// <summary>
///     A base class for all properties
/// </summary>
public abstract class BaseYieldTweenProperty
{
    public enum PropertyOffsetMode
    {
        /// <summary>
        ///     From value to value
        /// </summary>
        FromTo,

        /// <summary>
        ///     from current to value
        /// </summary>
        CurrentTo,

        /// <summary>
        ///     from current to current + value
        /// </summary>
        AddOffset,

        /// <summary>
        ///     from current to current * value
        /// </summary>
        MulOffset
    }


    private EasingType _easingType;
    public string EasingContainerQualifiedTypeName;

    public PropertyOffsetMode OffsetMode = PropertyOffsetMode.FromTo;

    protected KeyValuePair<UnityPathResolver.PathPartType, string>[] _pathParts;
    private string _path;

    public virtual string Path
    {
        get { return _path; }
        set
        {
            _path = value;
            _pathParts = UnityPathResolver.GetPathParts(value);
        }
    }

    public Func<float, float> EasingFunction { get; private set; }
    /*protected IEasingContainer EasingContainer
    {
        get { return (IEasingContainer)Activator.CreateInstance(EasingContainerType); }
        set
        {
            EasingContainerQualifiedTypeName = value.GetType().AssemblyQualifiedName;
            EasingType = EasingType.Custom;
        }
    }*/

    public Type EasingContainerType
    {
        get { return Type.GetType(EasingContainerQualifiedTypeName); }
        set
        {
            if (!value.GetInterfaces().Contains(typeof(IEasingContainer)))
                throw new Exception("type must implement IEasingContainer");
            EasingContainerQualifiedTypeName = value.AssemblyQualifiedName;
            EasingType = EasingType.Custom;
        }
    }

    public EasingType EasingType
    {
        get { return _easingType; }
        set
        {
            _easingType = value;
            switch (EasingType)
            {
                case EasingType.Linear:
                    EasingFunction = Easing.Linear;
                    break;
                case EasingType.SinIn:
                    EasingFunction = Easing.SinIn;
                    break;
                case EasingType.SinOut:
                    EasingFunction = Easing.SinOut;
                    break;
                case EasingType.SinInOut:
                    EasingFunction = Easing.SinInOut;
                    break;
                case EasingType.SinOutIn:
                    EasingFunction = Easing.SinOutIn;
                    break;
                case EasingType.QuadraticIn:
                    EasingFunction = Easing.QuadraticIn;
                    break;
                case EasingType.QuadraticOut:
                    EasingFunction = Easing.QuadraticOut;
                    break;
                case EasingType.QuadraticInOut:
                    EasingFunction = Easing.QuadraticInOut;
                    break;
                case EasingType.QuadraticOutIn:
                    EasingFunction = Easing.QuadraticOutIn;
                    break;
                case EasingType.CubicIn:
                    EasingFunction = Easing.CubicIn;
                    break;
                case EasingType.CubicOut:
                    EasingFunction = Easing.CubicOut;
                    break;
                case EasingType.CubicInOut:
                    EasingFunction = Easing.CubicInOut;
                    break;
                case EasingType.CubicOutIn:
                    EasingFunction = Easing.CubicOutIn;
                    break;
                case EasingType.QuarticIn:
                    EasingFunction = Easing.QuarticIn;
                    break;
                case EasingType.QuarticOut:
                    EasingFunction = Easing.QuarticOut;
                    break;
                case EasingType.QuarticInOut:
                    EasingFunction = Easing.QuarticInOut;
                    break;
                case EasingType.QuarticOutIn:
                    EasingFunction = Easing.QuarticOutIn;
                    break;
                case EasingType.QuinticIn:
                    EasingFunction = Easing.QuinticIn;
                    break;
                case EasingType.QuinticOut:
                    EasingFunction = Easing.QuinticOut;
                    break;
                case EasingType.QuinticInOut:
                    EasingFunction = Easing.QuinticInOut;
                    break;
                case EasingType.QuinticOutIn:
                    EasingFunction = Easing.QuinticOutIn;
                    break;
                case EasingType.BounceIn:
                    EasingFunction = Easing.BounceIn;
                    break;
                case EasingType.BounceOut:
                    EasingFunction = Easing.BounceOut;
                    break;
                case EasingType.BounceInOut:
                    EasingFunction = Easing.BounceInOut;
                    break;
                case EasingType.BounceOutIn:
                    EasingFunction = Easing.BounceOutIn;
                    break;
                case EasingType.CircularIn:
                    EasingFunction = Easing.CircularIn;
                    break;
                case EasingType.CircularOut:
                    EasingFunction = Easing.CircularOut;
                    break;
                case EasingType.CircularInOut:
                    EasingFunction = Easing.CircularInOut;
                    break;
                case EasingType.CircularOutIn:
                    EasingFunction = Easing.CircularOutIn;
                    break;
                case EasingType.BackIn:
                    EasingFunction = Easing.BackIn;
                    break;
                case EasingType.BackOut:
                    EasingFunction = Easing.BackOut;
                    break;
                case EasingType.BackInOut:
                    EasingFunction = Easing.BackInOut;
                    break;
                case EasingType.BackOutIn:
                    EasingFunction = Easing.BackOutIn;
                    break;
                case EasingType.ExpoIn:
                    EasingFunction = Easing.ExpoIn;
                    break;
                case EasingType.ExpoOut:
                    EasingFunction = Easing.ExpoOut;
                    break;
                case EasingType.ExpoInOut:
                    EasingFunction = Easing.ExpoInOut;
                    break;
                case EasingType.ExpoOutIn:
                    EasingFunction = Easing.ExpoOutIn;
                    break;
                case EasingType.ElasticIn:
                    EasingFunction = Easing.ElasticIn;
                    break;
                case EasingType.ElasticOut:
                    EasingFunction = Easing.ElasticOut;
                    break;
                case EasingType.ElasticInOut:
                    EasingFunction = Easing.ElasticInOut;
                    break;
                case EasingType.ElasticOutIn:
                    EasingFunction = Easing.ElasticOutIn;
                    break;

                default:
                    throw new Exception(
                        "can not set custom easings directly, use SetCustomEasing(EasingContainer container)");
            }
        }
    }

    public abstract IPropertyUpdater GetUpdater(object rootTargetObject);

    protected IEasingContainer GetEasingContainer()
    {
        return (IEasingContainer)Activator.CreateInstance(EasingContainerType);
    }
}