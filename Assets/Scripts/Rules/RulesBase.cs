using System;

namespace Rules
{
    public abstract class RulesBase
    {
        protected RulesBase(uint ruleNumber)
        {
        }

        public abstract bool ApplyRules(bool a, bool b, bool c, bool d, bool e);
    }
}