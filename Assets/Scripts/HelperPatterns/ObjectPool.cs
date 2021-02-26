using System.Collections.Generic;
// using NUnit.Framework;
using UnityEngine;

namespace HelperPatterns
{
    public class ObjectPool : Singleton<ObjectPool>
    {
        public GameObject cubePrefab;
        public List<GameObject> pooledCubes;
        public int amountToBuffer;
        GameObject containerObject;

        void Start()
        {
            containerObject = new GameObject("PooledObjects");
            pooledCubes = new List<GameObject>();

            for (int i = 0; i < amountToBuffer; i++)
            {
                GameObject newCube = Instantiate(cubePrefab);
                newCube.name = "Cube";
                PoolCube(cubePrefab);
            }
        }

        public GameObject PullCube()
        {
            if (pooledCubes.Count == 0) return Instantiate(cubePrefab);
            GameObject pooledCube = pooledCubes[0];
            pooledCube.gameObject.SetActive(true);
            pooledCube.transform.parent = null;
            pooledCubes.Remove(pooledCube);
            return pooledCube;
        }

        public void PoolCube(GameObject cube)
        {
            cube.gameObject.SetActive(false);
            cube.transform.parent = containerObject.transform;
            pooledCubes.Add(cube);
        }
    }
}