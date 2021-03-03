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
        bool[] rows;
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
            int depth = ECSManager.Instance.depth;
            

            rowWidth = 2 * depth - 1;
            rows = new bool[rowWidth * depth];
            int center = rowWidth / 2;

            rows[center] = true;
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

            var jobHandle = myJob.Schedule(rows.Length, rowWidth * 5);
            jobHandle.Complete();
            Debug.Log("Completed JOB");
            
            rowsArray.CopyTo(rows);
            rowsArray.Dispose();
            for (int i = 0; i < rows.Length / rowWidth; i++)
            {
                string output = "";
                for (int j = 0; j < rowWidth; j++)
                {
                    output += rows[i * rowWidth + j] ? "1" : "0";
                }
                Debug.Log($"Row {i}: {output}");
            }

            updated = true;
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (!updated) return inputDeps;
            
            manager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
            var cube = (GameObject) Resources.Load("Cube");
            var entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(cube, settings);
            
            Debug.Log("Instantiating entities");
            for (int i = 0; i < rows.Length; i++)
            {
                if (!rows[i]) continue;

                var position = new Vector3(i % rowWidth, i / rowWidth, 0);
                var instance = manager.Instantiate(entity);
                manager.SetComponentData(instance, new Translation { Value = position });
            }
            
            Debug.Log("Finished instantiating");

            updated = false;
            return inputDeps;
        }
    }
}