using System;
using UnityEngine;

namespace Rules
{
    public class GrowthTotalisticRules : RulesBase
    {
        public GrowthTotalisticRules(uint ruleNumber) : base(ruleNumber)
        {
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(5, '0');
            if (binaryString.Length > 5)
                Debug.Log("Number is extra long");
            rules = new bool[5];
        
            for (int i = 0; i < 5; i++)
                rules[i] = binaryString[i] == '1';
        }

        public override bool ApplyRules(bool center, bool left, bool right, bool up, bool down)
        {
            int total = (left ? 1 : 0) +
                        (right ? 1 : 0) +
                        (up ? 1 : 0) +
                        (down ? 1 : 0);

            return total switch
            {
                0 => rules[0],
                1 => rules[1],
                2 => rules[2],
                3 => rules[3],
                _ => rules[4],
            };
        }
    }
}