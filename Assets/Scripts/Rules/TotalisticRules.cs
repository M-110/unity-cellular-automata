using System;
using UnityEngine;

namespace Rules
{
    public class TotalisticRules : RulesBase
    {
        public TotalisticRules(uint ruleNumber) : base(ruleNumber)
        {
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(6, '0');
            if (binaryString.Length > 6)
                Debug.Log("Number is extra long");
            rules = new bool[6];
        
            for (int i = 0; i < 6; i++)
                rules[i] = binaryString[i] == '1';
        }

        public override bool ApplyRules(bool center, bool left, bool right, bool up, bool down,
            bool upLeft, bool upRight, bool downLeft, bool downRight)
        {
            int total = (center ? 1 : 0) +
                        (left ? 1 : 0) +
                        (right ? 1 : 0) +
                        (up ? 1 : 0) +
                        (down ? 1 : 0);
            return rules[total];
        }
    }
}