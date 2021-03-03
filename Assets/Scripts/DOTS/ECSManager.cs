using System;
using HelperPatterns;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace DOTS
{
    public class ECSManager : Singleton<ECSManager>
    {
        public EntityManager manager;
        public GameObject squarePrefab;
        public Entity square;
        public int depth;
        public bool[] rows;
        public int rowWidth;

        public void Start()
        {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            square = GameObjectConversionUtility.ConvertGameObjectHierarchy(squarePrefab, settings);
        }
        
    }
}