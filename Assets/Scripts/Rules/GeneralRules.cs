using System;

namespace Rules
{
    public class GeneralRules : RulesBase
    {
        public GeneralRules(uint ruleNumber) : base(ruleNumber)
        {
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(32, '0');
            rules = new bool[32];
        
            for (int i = 0; i < 32; i++)
                rules[i] = binaryString[i] == '1';
        }
    
        public override bool ApplyRules(bool center, bool left, bool right, bool up, bool down,
            bool upLeft, bool upRight, bool downLeft, bool downRight)
        {
            // This code is generated through Python, see below for script.
            if (center && left && right && up && down)
                return rules[0];
            if (!center && left && right && up && down)
                return rules[1];
            if (center && !left && right && up && down)
                return rules[2];
            if (center && left && !right && up && down)
                return rules[3];
            if (center && left && right && !up && down)
                return rules[4];
            if (center && left && right && up && !down)
                return rules[5];
            if (!center && !left && right && up && down)
                return rules[6];
            if (!center && left && !right && up && down)
                return rules[7];
            if (!center && left && right && !up && down)
                return rules[8];
            if (!center && left && right && up && !down)
                return rules[9];
            if (center && !left && !right && up && down)
                return rules[10];
            if (center && !left && right && !up && down)
                return rules[11];
            if (center && !left && right && up && !down)
                return rules[12];
            if (center && left && !right && !up && down)
                return rules[13];
            if (center && left && !right && up && !down)
                return rules[14];
            if (center && left && right && !up && !down)
                return rules[15];
            if (!center && !left && !right && up && down)
                return rules[16];
            if (!center && !left && right && !up && down)
                return rules[17];
            if (!center && !left && right && up && !down)
                return rules[18];
            if (!center && left && !right && !up && down)
                return rules[19];
            if (!center && left && !right && up && !down)
                return rules[20];
            if (!center && left && right && !up && !down)
                return rules[21];
            if (center && !left && !right && !up && down)
                return rules[22];
            if (center && !left && !right && up && !down)
                return rules[23];
            if (center && !left && right && !up && !down)
                return rules[24];
            if (center && left && !right && !up && !down)
                return rules[25];
            if (!center && !left && !right && !up && down)
                return rules[26];
            if (!center && !left && !right && up && !down)
                return rules[27];
            if (!center && !left && right && !up && !down)
                return rules[28];
            if (!center && left && !right && !up && !down)
                return rules[29];
            if (center && !left && !right && !up && !down)
                return rules[30];
            return rules[31];
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