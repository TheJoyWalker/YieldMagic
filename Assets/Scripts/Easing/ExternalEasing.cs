/*using System;
using UnityEngine;

namespace ExternalEasing
{
    public static class Easing
    {

        public static float Ease(float linearStep, float acceleration, EasingType type)
        {
            float easedStep = acceleration > 0 ? EaseIn(linearStep, type) :
                              acceleration < 0 ? EaseOut(linearStep, type) :
                              linearStep;

            return Mathf.Lerp(linearStep, easedStep, Mathf.Abs(acceleration));
        }

        public static float EaseIn(float linearStep, EasingType type)
        {
            switch (type)
            {
                case EasingType.Step: return linearStep < 0.5 ? 0 : 1;
                case EasingType.Linear: return linearStep;
                case EasingType.Sine: return Sine.EaseIn(linearStep);
                case EasingType.Quadratic: return Power.EaseIn(linearStep, 2);
                case EasingType.Cubic: return Power.EaseIn(linearStep, 3);
                case EasingType.Quartic: return Power.EaseIn(linearStep, 4);
                case EasingType.Quintic: return Power.EaseIn(linearStep, 5);
            }
            throw new NotImplementedException();
        }

        public static float EaseOut(float linearStep, EasingType type)
        {
            switch (type)
            {
                case EasingType.Step: return linearStep < 0.5 ? 0 : 1;
                case EasingType.Linear: return linearStep;
                case EasingType.Sine: return Sine.EaseOut(linearStep);
                case EasingType.Quadratic: return Power.EaseOut(linearStep, 2);
                case EasingType.Cubic: return Power.EaseOut(linearStep, 3);
                case EasingType.Quartic: return Power.EaseOut(linearStep, 4);
                case EasingType.Quintic: return Power.EaseOut(linearStep, 5);
            }
            throw new NotImplementedException();
        }

        public static float EaseInOut(float linearStep, EasingType easeInType, EasingType easeOutType)
        {
            return linearStep < 0.5 ? EaseInOut(linearStep, easeInType) : EaseInOut(linearStep, easeOutType);
        }
        public static float EaseInOut(float linearStep, EasingType type)
        {
            switch (type)
            {
                case EasingType.Step: return linearStep < 0.5 ? 0 : 1;
                case EasingType.Linear: return (float)linearStep;
                case EasingType.Sine: return Sine.EaseInOut(linearStep);
                case EasingType.Quadratic: return Power.EaseInOut(linearStep, 2);
                case EasingType.Cubic: return Power.EaseInOut(linearStep, 3);
                case EasingType.Quartic: return Power.EaseInOut(linearStep, 4);
                case EasingType.Quintic: return Power.EaseInOut(linearStep, 5);
            }
            throw new NotImplementedException();
        }

        static class Sine
        {
            public static float EaseIn(float s)
            {
                return Mathf.Sin(s * MathHelper.HalfPi - MathHelper.HalfPi) + 1;
            }
            public static float EaseOut(float s)
            {
                return Mathf.Sin(s * MathHelper.HalfPi);
            }
            public static float EaseInOut(float s)
            {
                return (Mathf.Sin(s * MathHelper.Pi - MathHelper.HalfPi) + 1) / 2f;
            }
        }

        static class Power
        {
            public static float EaseIn(float s, int power)
            {
                return Mathf.Pow(s, power);
            }
            public static float EaseOut(float s, int power)
            {
                var sign = power % 2 == 0 ? -1 : 1;
                return sign * (Mathf.Pow(s - 1, power) + sign);
            }
            public static float EaseInOut(float s, int power)
            {
                s *= 2;
                if (s < 1) return EaseIn(s, power) / 2;
                var sign = power % 2 == 0 ? -1 : 1;
                return sign / 2f * Mathf.Pow(s - 2, power) + sign * 2f;
            }
        }
    }

    public enum EasingType
    {
        Step,
        Linear,
        Sine,
        Quadratic,
        Cubic,
        Quartic,
        Quintic
    }

    public static class MathHelper
    {
        public const float Pi = Mathf.PI;
        public const float HalfPi = Mathf.PI / 2;
    }
}*/