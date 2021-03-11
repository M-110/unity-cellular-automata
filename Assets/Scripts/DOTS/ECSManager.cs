using System;
using HelperPatterns;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace DOTS
{
    public class ECSManager : MonoBehaviour
    {
        public EntityManager manager;
        public GameObject squarePrefab;
        public Entity square;
        public int rule = 90;
        public int depth;

        public void Start()
        {
            CASystemRowByRow.SetNewGridSettings(squarePrefab, rule, depth);
        }
        
    }
}