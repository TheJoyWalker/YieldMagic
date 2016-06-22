using System;
using UnityEngine;

namespace YieldMagic
{
    public enum EasingType
    {
        Linear,
        SinIn, SinOut, SinInOut, SinOutIn,
        QuadraticIn, QuadraticOut, QuadraticInOut, QuadraticOutIn,
        CubicIn, CubicOut, CubicInOut, CubicOutIn,
        QuarticIn, QuarticOut, QuarticInOut, QuarticOutIn,
        QuinticIn, QuinticOut, QuinticInOut, QuinticOutIn,
        BounceIn, BounceOut, BounceInOut, BounceOutIn,
        CircularIn, CircularOut, CircularInOut, CircularOutIn,
        BackIn, BackOut, BackInOut, BackOutIn,
        ExpoIn, ExpoOut, ExpoInOut, ExpoOutIn,
        ElasticIn, ElasticOut, ElasticInOut, ElasticOutIn,
        Custom//Serialization stuff
    }

    /// <summary>
    /// Easing functions are here
    /// I used maximum inlining so that it would be as fast as possible
    /// Writing Faster Managed Code: Know What Things Cost
    /// https://msdn.microsoft.com/en-us/library/ms973852.aspx
    /// </summary>
    public static class Easing
    {
        //-f(x)      = horizontal mirror 
        //f(1f-x)    = mirror vertical
        //-f(1f-x)   = mirror
        //the idea of double Easing is having 2f shorter charts 0.5 for each total
        //common logic for inout
        //easeInOut p<.5f?EaseIn(2f*p)*.5f:0.5f+EaseOut((p-.5f)*2f)*.5f;
        //easeOutIn p<.5f?EaseOut(2f*p)*.5f:0.5f+EaseIn((p-.5f)*2f)*.5f;





        /// <summary>
        /// A mixing utility, where easeIn is first half and is second
        /// </summary>
        /// <param name="easeIn"></param>
        /// <param name="easeOut"></param>
        /// <returns></returns>
        public static Func<float, float> GetInOut(Func<float, float> easeIn, Func<float, float> easeOut)
        {
            return (p) => (p < .5f) ? easeIn(p * 2f) * .5f : .5f + easeOut((p - .5f) * 2f) * .5f;
        }

        private const float PI = Mathf.PI;
        private const float TwoPI = PI * 2f;
        private const float HalfPi = PI * 0.5f;
        public static float Linear(float p) { return p; }
        #region sin
        //cuz shifting sin is more expensive than cos
        //public static float SinIn(float p) { return 1f + Mathf.Sin(_oneAndHalfPi + p * HalfPi); }
        public static float SinIn(float p) { return 1f - Mathf.Cos(p * HalfPi); }
        public static float SinOut(float p) { return Mathf.Sin(p * HalfPi); }
        public static float SinInOut(float p) { return (p < .5f) ? SinIn(p * 2f) * .5f : .5f + SinOut((p - .5f) * 2f) * .5f; }
        public static float SinOutIn(float p) { return (p < .5f) ? SinOut(p * 2f) * .5f : .5f + SinIn((p - .5f) * 2f) * .5f; }
        #endregion
        #region power easings
        #region Quadratic
        public static float QuadraticIn(float p) { return p * p; }
        public static float QuadraticOut(float p) { return 1f - Mathf.Pow(p - 1f, 2f); }

        //unoptimized would be like this
        //return (p < .5f) ? QuadricIn(p * 2f) * .5f : .5f + QuadricOut((p - .5f) * 2f) * .5f;
        public static float QuadraticInOut(float p)
        {
            if (p < .5f) return QuadraticIn(p * 2f) * .5f;
            return 1f - Mathf.Pow(2f - p * 2f, 2f) * .5f;
        }

        public static float QuadraticOutIn(float p)
        {
            if (p < .5f)
                return QuadraticOut(p * 2f) * .5f;
            return .5f + Mathf.Pow(1f - p * 2f, 2f) * .5f;
        }
        #endregion
        #region Cubic
        public static float CubicIn(float p) { return Mathf.Pow(p, 3f); }
        public static float CubicOut(float p) { return 1f + Mathf.Pow(p - 1f, 3f); }

        //unoptimized would be like this
        //return (p < .5f) ? CubicIn(p * 2f) * .5f : .5f + CubicOut((p - .5f) * 2f) * .5f;
        public static float CubicInOut(float p)
        {
            if (p < .5f) return CubicIn(p * 2f) * .5f;
            return 1f - Mathf.Pow(2f - p * 2f, 3f) * .5f;
        }

        public static float CubicOutIn(float p)
        {
            if (p < .5f)
                return CubicOut(p * 2f) * .5f;
            return .5f + Mathf.Pow(p * 2f - 1f, 3f) * .5f;
        }
        #endregion
        #region Quartic
        public static float QuarticIn(float p) { return Mathf.Pow(p, 4f); }
        public static float QuarticOut(float p) { return 1f - Mathf.Pow(p - 1f, 4f); }

        //unoptimized would be like this
        //return (p < .5f) ? QuadricIn(p * 2f) * .5f : .5f + QuadricOut((p - .5f) * 2f) * .5f;
        public static float QuarticInOut(float p)
        {
            if (p < .5f) return QuarticIn(p * 2f) * .5f;
            return 1f - Mathf.Pow(2f - p * 2f, 4f) * .5f;
        }

        public static float QuarticOutIn(float p)
        {
            if (p < .5f)
                return QuarticOut(p * 2f) * .5f;
            return .5f + Mathf.Pow(1f - p * 2f, 4f) * .5f;
        }
        #endregion
        #region Quintic
        public static float QuinticIn(float p) { return Mathf.Pow(p, 5f); }
        public static float QuinticOut(float p) { return 1f + Mathf.Pow(p - 1f, 5f); }

        //unoptimized would be like this
        //return (p < .5f) ? QuinticIn(p * 2f) * .5f : .5f + QuinticOut((p - .5f) * 2f) * .5f;
        public static float QuinticInOut(float p)
        {
            if (p < .5f) return QuinticIn(p * 2f) * .5f;
            return 1f - Mathf.Pow(2f - p * 2f, 5f) * .5f;
        }

        public static float QuinticOutIn(float p)
        {
            if (p < .5f)
                return QuinticOut(p * 2f) * .5f;
            return .5f + Mathf.Pow(p * 2f - 1f, 5f) * .5f;
        }
        #endregion
        #endregion
        #region Bounce
        //I did want To inline here eather, but it looks too insane for me
        //You can construct your own bounces similar To 
        //var bounce = (p)=>BounceIn(p, bounceCount);
        public static float BounceIn(float p, float count)
        {
            return Math.Abs(Mathf.Cos(p * count * PI)) * p;
        }

        public static float BounceOut(float p, float count)
        {
            return 1f - Math.Abs(Mathf.Cos(p * count * PI)) * (1f - p);
        }
        public static float BounceIn(float p)
        {
            return BounceIn(p, 2f);
        }
        public static float BounceOut(float p)
        {
            return BounceOut(p, 2f);
        }
        //too lazy To inline
        public static float BounceInOut(float p) { return (p < .5f) ? BounceIn(p * 2f) * .5f : .5f + BounceOut((p - .5f) * 2f) * .5f; }
        public static float BounceOutIn(float p) { return (p < .5f) ? BounceOut(p * 2f) * .5f : .5f + BounceIn((p - .5f) * 2f) * .5f; }
        #endregion
        #region Circular
        public static float CircularIn(float p)
        {
            p -= 1f;
            return (float)Math.Sqrt(1f - p * p);
        }
        public static float CircularOut(float p) { return 1f - (float)Math.Sqrt(1f - p * p); }
        public static float CircularInOut(float p) { return (p < .5f) ? CircularIn(p * 2f) * .5f : .5f + CircularOut((p - .5f) * 2f) * .5f; }
        public static float CircularOutIn(float p) { return (p < .5f) ? CircularOut(p * 2f) * .5f : .5f + CircularIn((p - .5f) * 2f) * .5f; }
        #endregion
        #region Back
        //x^2f-sin(x*PI)
        public static float BackIn(float p)
        {
            return p * p - Mathf.Sin(p * PI) * .3f;
        }

        public static float BackOut(float p)
        {
            return 1f - Mathf.Pow(p - 1f, 2f) + Mathf.Sin(p * PI) * .3f;
        }
        public static float BackInOut(float p) { return (p < .5f) ? BackIn(p * 2f) * .5f : .5f + BackOut((p - .5f) * 2f) * .5f; }
        public static float BackOutIn(float p) { return (p < .5f) ? BackOut(p * 2f) * .5f : .5f + BackIn((p - .5f) * 2f) * .5f; }
        #endregion
        #region Expo
        //cuz shifting sin is more expensive than cos
        //public static float SinIn(float p) { return 1f + Mathf.Sin(_oneAndHalfPi + p * HalfPi); }
        public static float ExpoIn(float p) { return Mathf.Pow(2, 10 * (p - 1)); }
        public static float ExpoOut(float p) { return (float)(-Math.Pow(2, -10 * p) + 1); }
        public static float ExpoInOut(float p) { return (p < .5f) ? ExpoIn(p * 2f) * .5f : .5f + ExpoOut((p - .5f) * 2f) * .5f; }
        public static float ExpoOutIn(float p) { return (p < .5f) ? ExpoOut(p * 2f) * .5f : .5f + ExpoIn((p - .5f) * 2f) * .5f; }
        #endregion
        #region Elastic
        public static float ElasticIn(float p)
        {
            var t = 0.3f;
            return -Mathf.Pow(2, -10 * (1 - p)) * Mathf.Sin(((1 - p) - t * .25f) * TwoPI / t);
        }

        public static float ElasticOut(float p)
        {
            var t = .3f;
            return Mathf.Pow(2, -10 * p) * Mathf.Sin((p - t * .25f) * TwoPI / t) + 1;
        }
        public static float ElasticInOut(float p) { return (p < .5f) ? ElasticIn(p * 2f) * .5f : .5f + ElasticOut((p - .5f) * 2f) * .5f; }
        public static float ElasticOutIn(float p) { return (p < .5f) ? ElasticOut(p * 2f) * .5f : .5f + ElasticIn((p - .5f) * 2f) * .5f; }
        #endregion
    }
}
