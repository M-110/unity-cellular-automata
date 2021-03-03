using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace DOTS
{
    public struct CAJob : IJobParallelFor
    {
        public NativeArray<bool> rows;
        public int rowWidth;
        
        public void Execute(int i)
        {
            bool a, b, c;
            if (i < rowWidth) return;

            if (i % rowWidth == 0) // Left edge
            {
                a = rows[i - 1];
                b = rows[i - rowWidth];
                c = rows[i - rowWidth + 1];
            }
            else if (i % rowWidth == rowWidth - 1) // Right edge
            {
                a = rows[i - rowWidth - 1];
                b = rows[i - rowWidth];
                c = rows[i + 1 - 2 * rowWidth];
            }
            else
            {
                a = rows[i - rowWidth - 1];
                b = rows[i - rowWidth];
                c = rows[i - rowWidth + 1];
            }

            rows[i] = ApplyRule(a, b, c);
        }

        public bool ApplyRule(bool a, bool b, bool c)
        {
            return a | b | c;
        }
    }
}