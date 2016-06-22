using System;
using System.Diagnostics;
using YieldMagic;

public abstract class GenericProperty<T> : BaseYieldTweenProperty where T : struct
{
    /// <summary>
    /// should we set FromValue=Getter();
    /// will require Getter of Field
    /// </summary>

    public T FromVaue;
    public T ToValue;

    protected Func<T> Getter;
    protected Action<T> Setter;

    private IEasingContainer _easingContainer;

    public IEasingContainer EasingContainer
    {
        get { return _easingContainer; }
        set
        {
            if (value != null)
            {
                _easingContainer = value;
                EasingFunction = _easingContainer.Ease;
                _easingType = EasingType.Custom;
            }
            else
                EasingType = (_easingType == EasingType.Custom) ? EasingType.Linear : _easingType;
        }
    }



    private EasingType _easingType;

    public Func<T, T, float, T> Lerp = PropertyTypeOperations.GetLerp<T>();
    public EasingType EasingType
    {
        get { return _easingType; }
        set
        {
            _easingType = value;
            switch (EasingType)
            {
                case EasingType.Linear: EasingFunction = Easing.Linear; break;
                case EasingType.SinIn: EasingFunction = Easing.SinIn; break;
                case EasingType.SinOut: EasingFunction = Easing.SinOut; break;
                case EasingType.SinInOut: EasingFunction = Easing.SinInOut; break;
                case EasingType.SinOutIn: EasingFunction = Easing.SinOutIn; break;
                case EasingType.QuadraticIn: EasingFunction = Easing.QuadraticIn; break;
                case EasingType.QuadraticOut: EasingFunction = Easing.QuadraticOut; break;
                case EasingType.QuadraticInOut: EasingFunction = Easing.QuadraticInOut; break;
                case EasingType.QuadraticOutIn: EasingFunction = Easing.QuadraticOutIn; break;
                case EasingType.CubicIn: EasingFunction = Easing.CubicIn; break;
                case EasingType.CubicOut: EasingFunction = Easing.CubicOut; break;
                case EasingType.CubicInOut: EasingFunction = Easing.CubicInOut; break;
                case EasingType.CubicOutIn: EasingFunction = Easing.CubicOutIn; break;
                case EasingType.QuarticIn: EasingFunction = Easing.QuarticIn; break;
                case EasingType.QuarticOut: EasingFunction = Easing.QuarticOut; break;
                case EasingType.QuarticInOut: EasingFunction = Easing.QuarticInOut; break;
                case EasingType.QuarticOutIn: EasingFunction = Easing.QuarticOutIn; break;
                case EasingType.QuinticIn: EasingFunction = Easing.QuinticIn; break;
                case EasingType.QuinticOut: EasingFunction = Easing.QuinticOut; break;
                case EasingType.QuinticInOut: EasingFunction = Easing.QuinticInOut; break;
                case EasingType.QuinticOutIn: EasingFunction = Easing.QuinticOutIn; break;
                case EasingType.BounceIn: EasingFunction = Easing.BounceIn; break;
                case EasingType.BounceOut: EasingFunction = Easing.BounceOut; break;
                case EasingType.BounceInOut: EasingFunction = Easing.BounceInOut; break;
                case EasingType.BounceOutIn: EasingFunction = Easing.BounceOutIn; break;
                case EasingType.CircularIn: EasingFunction = Easing.CircularIn; break;
                case EasingType.CircularOut: EasingFunction = Easing.CircularOut; break;
                case EasingType.CircularInOut: EasingFunction = Easing.CircularInOut; break;
                case EasingType.CircularOutIn: EasingFunction = Easing.CircularOutIn; break;
                case EasingType.BackIn: EasingFunction = Easing.BackIn; break;
                case EasingType.BackOut: EasingFunction = Easing.BackOut; break;
                case EasingType.BackInOut: EasingFunction = Easing.BackInOut; break;
                case EasingType.BackOutIn: EasingFunction = Easing.BackOutIn; break;
                case EasingType.ExpoIn: EasingFunction = Easing.ExpoIn; break;
                case EasingType.ExpoOut: EasingFunction = Easing.ExpoOut; break;
                case EasingType.ExpoInOut: EasingFunction = Easing.ExpoInOut; break;
                case EasingType.ExpoOutIn: EasingFunction = Easing.ExpoOutIn; break;
                case EasingType.ElasticIn: EasingFunction = Easing.ElasticIn; break;
                case EasingType.ElasticOut: EasingFunction = Easing.ElasticOut; break;
                case EasingType.ElasticInOut: EasingFunction = Easing.ElasticInOut; break;
                case EasingType.ElasticOutIn: EasingFunction = Easing.ElasticOutIn; break;

                case EasingType.Custom:
                default:
                    throw new Exception("can not set custom easings directly, use SetCustomEasing(EasingContainer container)");
            }
        }
    }

    public override void Update(float p)
    {
        Setter(Lerp(FromVaue, ToValue, p));
//        UnityEngine.Debug.LogFormat("SET: {0}-{1}({2})={3}", FromVaue, ToValue, p, Lerp(FromVaue, ToValue, p));
        //        ApplyFunction(Target, Lerp(FromVaue, ToValue, p));
    }
}