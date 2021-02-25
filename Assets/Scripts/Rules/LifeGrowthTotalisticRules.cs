using System;
using UnityEngine;

namespace Rules
{
    public class LifeGrowthTotalisticRules : RulesBase
    {
        public LifeGrowthTotalisticRules(uint ruleNumber) : base(ruleNumber)
        {
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(16, '0');
            if (binaryString.Length > 16)
                Debug.Log("Number is extra long");
            rules = new bool[16];
            Debug.Log(binaryString);
            for (int i = 0; i < 16; i++)
                rules[i] = binaryString[i] == '1';
        }

        public override bool ApplyRules(bool center, bool left, bool right, bool up, bool down,
            bool upLeft, bool upRight, bool downLeft, bool downRight)
        {
            int total = (left ? 1 : 0) +
                        (right ? 1 : 0) +
                        (up ? 1 : 0) +
                        (down ? 1 : 0) +
                        (upLeft ? 1 : 0) +
                        (upRight ? 1 : 0) +
                        (downLeft ? 1 : 0) +
                        (downRight ? 1 : 0);
            return rules[center ? total + 7 : total];
            // Conway's Game of Life: 4192
        }
    }
}