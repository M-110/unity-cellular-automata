using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace DOTS
{
    public class CASystem : JobComponentSystem
    {
        EntityManager manager;
        ECSManager ecsManager;
        Entity entity;
        bool[] rows;
        int depth;
        int rowWidth;
        bool updated;
        NativeArray<bool> rowsArray;

        protected override void OnCreate()
        {
            base.OnCreate();
            Setup();
            UpdateArray();
        }

        void Setup()
        {
            ecsManager = Object.FindObjectOfType<ECSManager>();
            depth = ecsManager.depth;
            rowWidth = 2 * depth - 1;
            rows = new bool[rowWidth * depth];
            int center = rowWidth / 2;

            rows[center] = true;
            
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var cube = ecsManager.squarePrefab;
            entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cube, settings);
            
        }
        
        

        void UpdateArray()
        {
            Debug.Log("Creating CA System");
            Debug.Log(rows.Length);
            rowsArray = new NativeArray<bool>(rows, Allocator.TempJob);
            var myJob = new CAJob
            {
                rows = rowsArray,
                rowWidth = rowWidth
            };

            var jobHandle = myJob.Schedule(rows.Length, rows.Length);
            jobHandle.Complete();
            Debug.Log("Completed JOB");
            
            rowsArray.CopyTo(rows);
            rowsArray.Dispose();

            updated = true;
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!updated) return inputDeps;
            
            Debug.Log("Instantiating entities");
            for (int i = 0; i < rows.Length; i++)
            {
                if (!rows[i]) continue;

                var position = new Vector3(i % rowWidth - depth, -(i / rowWidth), 0);
                var instance = manager.Instantiate(entity);
                manager.SetComponentData(instance, new Translation { Value = position });
            }
            
            Debug.Log("Finished instantiating");

            updated = false;
            return inputDeps;
        }
    }
}