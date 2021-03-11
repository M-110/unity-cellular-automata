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
        // Static variables
        static bool isNewGrid;
        static Entity entity;
        static bool[] rules;
        static int depth;
        static EntityManager manager;

        //EntityManager manager;
        //ECSManager ecsManager;
        //Entity entity;
        bool[][] rows;
        //bool[] = new bool[8];
        //int depth;
        int rowWidth;
        //bool updated;
        
        [NativeDisableParallelForRestriction]
        NativeArray<bool> rulesArray;
        
        [NativeDisableParallelForRestriction]
        NativeArray<bool> previousRowArray;
        NativeArray<bool> newRowArray;

        // protected override void OnCreate()
        // {
        //     base.OnCreate();
        //     Setup();
        //     UpdateArray();
        // }

        public static void SetNewGridSettings(GameObject prefab, int ruleNumber, int depth_)
        {
            entity = PrefabToEntity(prefab);
            rules = IntToRules(ruleNumber);
            depth = depth_;
            isNewGrid = true;
        }

        static Entity PrefabToEntity(GameObject prefab)
        {
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            return GameObjectConversionUtility.ConvertGameObjectHierarchy(prefab, settings);
        }

        static bool[] IntToRules(int ruleNumber)
        {
            bool[] newRules = new bool[8];
            
            string binaryString = Convert.ToString(ruleNumber, 2).PadLeft(8, '0');
            for (int i = 0; i < binaryString.Length; i++)
                newRules[i] = binaryString[i] == '1';

            return newRules;
        }

        // void Setup()
        // {
        //     ecsManager = Object.FindObjectOfType<ECSManager>();
        //     depth = ecsManager.depth;
        //     rowWidth = 2 * depth - 1;
        //     rows = new bool[depth][];
        //     int center = rowWidth / 2;
        //
        //     rows[0] = new bool[rowWidth];
        //     rows[0][center] = true;
        // }
        
        

        // void UpdateArray()
        // {
        //     Debug.Log("Creating CA System");
        //     Debug.Log(rows.Length * rowWidth);
        //     
        //     rulesArray = new NativeArray<bool>(rules, Allocator.TempJob);
        //     
        //     for (int i = 1; i < depth; i++)
        //     {
        //         previousRowArray= new NativeArray<bool>(rows[i - 1], Allocator.TempJob);
        //         newRowArray = new NativeArray<bool>(new bool[rowWidth], Allocator.TempJob);
        //         
        //         var myJob = new UpdateRowJob
        //         {
        //             previousRow = previousRowArray,
        //             newRow = newRowArray,
        //             rowWidth = rowWidth,
        //             rules = rulesArray
        //         };
        //         
        //         var jobHandle = myJob.Schedule(rowWidth, rowWidth);
        //         jobHandle.Complete();
        //
        //         previousRowArray.Dispose();
        //         rows[i] = newRowArray.ToArray();
        //         newRowArray.Dispose();
        //     }
        //     
        //     Debug.Log("Completed JOB");
        //     rulesArray.Dispose();
        //
        //     updated = true;
        // }
        
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!isNewGrid) return inputDeps;

            CreateNewGrid();
            
            return inputDeps;
        }

        void CreateNewGrid()
        {
            //DestroyPreviousValues()? Pool entities?
            SetGridProperties();
            SetGridValues();
            InstantiateEntities();
            isNewGrid = false;
        }

        void SetGridProperties()
        {
            rowWidth = 2 * depth - 1;
            rows = new bool[depth][];
            
            rows[0] = new bool[rowWidth];
            rows[0][rowWidth / 2] = true;
        }

        void SetGridValues()
        {
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

            rulesArray.Dispose();
        }

        void InstantiateEntities()
        {
            for (int i = 0; i < depth; i++)
                for (int j = 0; j < rowWidth; j++)
                {
                    if (!rows[i][j]) continue;
                    
                    manager.SetComponentData(
                        manager.Instantiate(entity),
                        new Translation {Value = new Vector3(j, -i, 0)});
                }
        }
    }
}