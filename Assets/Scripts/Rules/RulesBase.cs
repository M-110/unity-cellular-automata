using System;

namespace Rules
{
    public abstract class RulesBase
    {
        protected bool[] rules;

        protected RulesBase(uint ruleNumber) { }

        public abstract bool ApplyRules(bool center, bool left, bool right, bool up, bool down);
    }
}