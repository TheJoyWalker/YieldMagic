using System;

namespace YieldMagic
{
    public class WaitTimeOrCondition : YieldBase
    {
        public WaitTimeOrCondition() { }

        public WaitTimeOrCondition(float sec, Func<bool> breakOn)
        {
            Initialize(sec, breakOn);
        }

        public WaitTimeOrCondition Initialize(float sec, Func<bool> breakOn)
        {
            Duration = sec;
            Rewind();//rewind before override, cuz it will wipe delegates
            Override(breakInstruction: breakOn);
            return this;
        }
        protected override void Update()
        {
        }

        protected override void OnRewind()
        {
        }
    }
}