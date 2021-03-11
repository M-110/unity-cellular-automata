using System;
using System.Collections;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DOTS
{
    public class CASystemRowByRow : JobComponentSystem
    {
        EntityManager manager;
        ECSManager ecsManager;
        Entity entity;
        bool[][] rows;
        bool[] rules = new bool[8];
        int depth;
        int rowWidth;
        bool updated;
        
        [NativeDisableParallelForRestriction]
        NativeArray<bool> rulesArray;
        
        [NativeDisableParallelForRestriction]
        NativeArray<bool> previousRowArray;
        NativeArray<bool> newRowArray;

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
            rows = new bool[depth][];
            int center = rowWidth / 2;

            rows[0] = new bool[rowWidth];
            rows[0][center] = true;
            
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var cube = ecsManager.squarePrefab;
            entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cube, settings);
            
            string binaryString = Convert.ToString(ecsManager.rule, 2).PadLeft(8, '0');
            for (int i = 0; i < binaryString.Length; i++)
                rules[i] = binaryString[i] == '1';
        }
        
        

        void UpdateArray()
        {
            Debug.Log("Creating CA System");
            Debug.Log(rows.Length * rowWidth);
            
            rulesArray = new NativeArray<bool>(rules, Allocator.TempJob);
            
            for (int i = 1; i < depth; i++)
            {
                previousRowArray= new NativeArray<bool>(rows[i - 1], Allocator.TempJob);
                newRowArray = new NativeArray<bool>(new bool[rowWidth], Allocator.TempJob);
                
                var myJob = new UpdateRowJob
                {
                    previousRow = previousRowArray,
                    newRow = newRowArray,
                    rowWidth = rowWidth,
                    rules = rulesArray
                };
                
                var jobHandle = myJob.Schedule(rowWidth, rowWidth);
                jobHandle.Complete();

                previousRowArray.Dispose();
                rows[i] = newRowArray.ToArray();
                newRowArray.Dispose();
            }
            
            Debug.Log("Completed JOB");
            rulesArray.Dispose();

            updated = true;
        }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!updated) return inputDeps;
            
            Debug.Log("Instantiating entities");
            for (int i = 0; i < depth; i++)
            for (int j = 0; j < rowWidth; j++)
            {
                if (!rows[i][j]) continue;

                var position = new Vector3(j, -i, 0);
                var instance = manager.Instantiate(entity);
                manager.SetComponentData(instance, new Translation { Value = position });
            }
            
            Debug.Log("Finished instantiating");

            updated = false;
            return inputDeps;
        }
    }
}