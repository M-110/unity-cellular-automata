using System;

namespace Rules
{
    public class GeneralRules : RulesBase
    {
        bool[] rules;

        public GeneralRules(uint ruleNumber) : base(ruleNumber)
        {
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(32, '0');
            rules = new bool[32];
        
            for (int i = 0; i < 32; i++)
                rules[i] = binaryString[i] == '1';
        }
    
        public override bool ApplyRules(bool a, bool b, bool c, bool d, bool e)
        {
            //Debug.Log($"{a}, {b}, {c}, {d}, {e}");
            // This code is generated through Python, see below for script.
            #region Combinatoric Conditionals 
            if (a && b && c && d && e)
                return rules[0];
            if (!a && b && c && d && e)
                return rules[1];
            if (a && !b && c && d && e)
                return rules[2];
            if (a && b && !c && d && e)
                return rules[3];
            if (a && b && c && !d && e)
                return rules[4];
            if (a && b && c && d && !e)
                return rules[5];
            if (!a && !b && c && d && e)
                return rules[6];
            if (!a && b && !c && d && e)
                return rules[7];
            if (!a && b && c && !d && e)
                return rules[8];
            if (!a && b && c && d && !e)
                return rules[9];
            if (a && !b && !c && d && e)
                return rules[10];
            if (a && !b && c && !d && e)
                return rules[11];
            if (a && !b && c && d && !e)
                return rules[12];
            if (a && b && !c && !d && e)
                return rules[13];
            if (a && b && !c && d && !e)
                return rules[14];
            if (a && b && c && !d && !e)
                return rules[15];
            if (!a && !b && !c && d && e)
                return rules[16];
            if (!a && !b && c && !d && e)
                return rules[17];
            if (!a && !b && c && d && !e)
                return rules[18];
            if (!a && b && !c && !d && e)
                return rules[19];
            if (!a && b && !c && d && !e)
                return rules[20];
            if (!a && b && c && !d && !e)
                return rules[21];
            if (a && !b && !c && !d && e)
                return rules[22];
            if (a && !b && !c && d && !e)
                return rules[23];
            if (a && !b && c && !d && !e)
                return rules[24];
            if (a && b && !c && !d && !e)
                return rules[25];
            if (!a && !b && !c && !d && e)
                return rules[26];
            if (!a && !b && !c && d && !e)
                return rules[27];
            if (!a && !b && c && !d && !e)
                return rules[28];
            if (!a && b && !c && !d && !e)
                return rules[29];
            if (a && !b && !c && !d && !e)
                return rules[30];
            if (!a && !b && !c && !d && !e)
                return rules[31];
            #endregion
        
            // Can't reach this.
            return false;
        } 
    }
}



/*
from itertools import combinations

def all_combos(lst):
    results = []
    for i in range(len(lst)+1):
        results += list(combinations(lst, i))
    return results
       
combos = all_combos(list(range(5)))

for i, combo  in enumerate(combos):
    nots = ['', '', '', '', '']
    for index in combo:
        nots[index] = '!'
    output = f"""if ({nots[0]}a && {nots[1]}b && {nots[2]}c && {nots[3]}d && {nots[4]}e)
    return rules[{i}];"""
    print(output)
*/