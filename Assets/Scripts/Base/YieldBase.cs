using System;
using UnityEngine;

namespace YieldMagic
{
    public abstract class YieldBase : CustomYieldInstruction
    {
        /// <summary>
        /// Should YieldBase.Get use pool
        /// </summary>
        public static bool UsePool = true;
        /// <summary>
        /// should I call Update(1) when boke out?
        /// </summary>
        public bool CompleteOnBreak = true;

        /// <summary>
        /// External Easing function
        /// </summary>
        public Func<float, float> Easing = Linerar;
        public float Duration;
        /// <summary>
        /// Intruction progress 0f-1f
        /// </summary>
        public float Progress;
        /// <summary>
        /// use this in update
        /// </summary>
        public float EasedValue;
        /// <summary>
        /// if this is a breake quit
        /// set To true before Update fires last time
        /// </summary>
        public bool WillBreak;
        /// <summary>
        /// if complete
        /// set To true before Update fires last time
        /// </summary>
        public bool IsComplete;
        /// <summary>
        /// if breake or complete
        /// set To true before Update fires last time
        /// </summary>
        public bool WillQuit;

        /// <summary>
        /// used for lazy initialization checks
        /// </summary>
        protected internal bool Initialized;

        /// <summary>
        /// should I get back To pool when completed?
        /// </summary>
        public bool ReturnToPool;

        /// <summary>
        /// when did we start?
        /// </summary>
        protected float StartTime;
        /// <summary>
        /// when was the last update
        /// </summary>
        private float _lastCallTime;

        /// <summary>
        /// an interupt condition delegate
        /// true will break
        /// </summary>
        public Func<bool> InteruptInstruction = NeverBreak;
        //10k iterations say its cheaper To call empty delegate
        //action(this)                  - 0     (action = static (me)=>{})
        //action!=null=>action(this)    - 1096  (action = static (me)=>{})
        //action!=null=>action(this)    - 427   (action = null)
        /// <summary>
        /// called after YieldBase.Update
        /// </summary>
        public Action<YieldBase> OnUpdated = EmptyThisDelegate;
        /// <summary>
        /// called instead of complete if broke
        /// </summary>
        public Action<YieldBase> OnBreak = EmptyThisDelegate;
        /// <summary>
        /// called when Progress>=1;
        /// </summary>
        public Action<YieldBase> OnComplete = EmptyThisDelegate;

        /// <summary>
        /// do not override it if u want default time and event handling
        /// </summary>
        public override bool keepWaiting
        {
            get
            {
                //this means someone called parameterless constructor
                if (!Initialized)
                {
                    Debug.LogError("Call initialize or parametered constructor");
                    return false;
                }

                WillBreak = InteruptInstruction();
                _lastCallTime = Time.timeSinceLevelLoad;
                Progress = _lastCallTime - StartTime;
                IsComplete = Duration <= Progress;
                Progress /= Duration;

                WillQuit = IsComplete || WillBreak;

                if (WillQuit && CompleteOnBreak || IsComplete)
                    Progress = 1;
                EasedValue = Easing(Progress);
                Update();
                OnUpdated(this);

                if (WillBreak)
                    OnBreak(this);
                else if (!WillQuit)
                    OnComplete(this);



                if (WillQuit && ReturnToPool)
                    Pool.Free(this);
                //TimeProgress += Time.deltaTime;
                return !WillQuit;
            }
        }
        /// <summary>
        /// reseting instruction before use
        /// </summary>
        public void Rewind()
        {
            StartTime = Time.timeSinceLevelLoad;
            Progress = 0;

            OnUpdated = EmptyThisDelegate;
            OnComplete = EmptyThisDelegate;
            OnBreak = EmptyThisDelegate;
            Initialized = true;
            OnRewind();
        }

        #region Empty delegates
        public static void EmptyThisDelegate(YieldBase me) { }
        private static bool NeverBreak() { return false; }
        private static float Linerar(float value) { return value; }
        #endregion
        #region Overriding common things
        public YieldBase Override(bool completeOnBreak = true,
            Func<float, float> easing = null,
            Func<bool> breakInstruction = null,
            Action<YieldBase> onUpdated = null,
            Action<YieldBase> onBreak = null,
            Action<YieldBase> onComplete = null)
        {
            CompleteOnBreak = completeOnBreak;

            if (easing != null)
                Easing = easing;

            if (breakInstruction != null)
                InteruptInstruction = breakInstruction;
            if (onUpdated != null)
                OnUpdated = onUpdated;
            if (onBreak != null)
                OnBreak = onBreak;
            if (onComplete != null)
                OnComplete = onComplete;
            return this;
        }

        public YieldBase Override(OverrideProperties props)
        {
            return Override(props.CompleteOnBreak,
                props.Easing, props.BreakInstruction,
                props.OnUpdated, props.OnBreak, props.OnComplete);
        }
        #endregion
        #region Abstract
        /// <summary>
        ///     Step function that should apply all changes
        ///     Read values ex: this.EasedValue
        /// </summary>
        protected abstract void Update();
        /// <summary>
        ///     used To reset parameters when reinitializing
        /// </summary>
        protected abstract void OnRewind();
        #endregion
        #region Pool
        /// <summary>
        /// If you want using pool - this is the right function
        /// otherwise use new SomeTransition(params...)<br/>
        ///<code>YieldBase.Get&lt;PositionTranslate&gt;().Initialize(...).Override(...)</code>
        /// </summary>
        /// <typeparam name="T">YieldBase derived class</typeparam>
        /// <returns>Uninitialized instance</returns>
        public static T Get<T>() where T : YieldBase, new()
        {
            if (YieldBase.UsePool)
                return Pool.Get<T>();
            return new T();
        }

        /// <summary>
        /// pool uses this check for releasing stuck items
        /// </summary>
        public bool IsOutdated
        {
            get
            {
                //precision issues are possible - so try handling it
                var diff = Time.timeSinceLevelLoad - _lastCallTime;
                return Mathf.Abs(diff - Time.deltaTime) < 0.00000010f;
            }
        }
        #endregion
    }

    public struct OverrideProperties
    {
        public Func<bool> BreakInstruction;
        public bool CompleteOnBreak;
        public Func<float, float> Easing;
        public Action<YieldBase> OnUpdated;
        public Action<YieldBase> OnComplete;
        public Action<YieldBase> OnBreak;
    }
}