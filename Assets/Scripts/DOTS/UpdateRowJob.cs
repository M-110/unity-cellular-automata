using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS
{
    public struct UpdateRowJob : IJobParallelFor
    {
        public NativeArray<bool> previousRow;
        public NativeArray<bool> newRow;
        public int rowWidth;
        public NativeArray<bool> rules;
        
        
        public void Execute(int i)
        {
            if (i >= rowWidth) return;
            bool a, b, c;

            if (i == 0) // Left edge
            {
                a = previousRow[rowWidth - 1];
                b = previousRow[i];
                c = previousRow[1];
            }
            else if (i == rowWidth - 1) // Right edge
            {
                a = previousRow[i - 1];
                b = previousRow[i];
                c = previousRow[0];
            }
            else
            {
                a = previousRow[i - 1];
                b = previousRow[i];
                c = previousRow[i + 1];
            }

            newRow[i] = ApplyRule(a, b, c);
        }

        bool ApplyRule(bool a, bool b, bool c)
        {
            if (a & b & c)
                return rules[0];
            if (a & b & !c)
                return rules[1];
            if (a & !b & c)
                return rules[2];
            if (a & !b & !c)
                return rules[3];
            if (b & c)
                return rules[4];
            if (b & !c)
                return rules[5];
            if (c)
                return rules[6];
            return rules[7];
        }
    }
}