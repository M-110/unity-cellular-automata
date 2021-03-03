using System;
using System.Collections;
using System.Collections.Generic;
using Rules;
using HelperPatterns;
using UnityEngine;
using Random = System.Random;

public enum RulesType
{
    General,
    LifeGrowthTotalistic,
    Totalistic,
    GrowthTotalistic
}
 // TODO: Fix Positions, Initial Conditions
public class Generator : MonoBehaviour
{
    [HideInInspector] public RulesType rulesType;
    [HideInInspector] public GameObject cube;
    [HideInInspector] public int depth = 1;
    [HideInInspector] public bool useDefaultSize;
    
    [HideInInspector] public bool lastLayerOnly;
    
    [HideInInspector] public bool useAnimation = true;
    [HideInInspector] public int animationFPS = 1;
    [HideInInspector] public float timeSinceLastUpdate;
    
    [HideInInspector] public int ruleBitCount = 32;
    [HideInInspector] public int size;
    [SerializeField] public uint ruleNumber = 4294967295;
    List<GameObject> currentCubes = new List<GameObject>();
    List<bool[,]> layers = new List<bool[,]>();
    RulesBase rules;
    // 2186559728 Simple flat triangles
    void Start()
    {
        GenerateRules();
        GenerateTopLayer();
        if (!useAnimation)
            GenerateAllLayers();
    }

    void Update()
    {
        if (!useAnimation)
            return;
        GenerateNextLayer();
    }
    

    bool ApplyRule(int x, int y, int z)
    {
        bool[,] previousLayer = layers[y - 1];
        
        int xLeft = x == 0 ? size - 1 : x - 1;
        int xRight = (x + 1) % size;
        int zUp = (z + 1) % size;
        int zDown = z == 0 ? size - 1 : z - 1;
        
        bool center = previousLayer[x, z];
        bool left = previousLayer[xLeft, z];
        bool right = previousLayer[xRight, z];
        bool up = previousLayer[x, zUp];
        bool down = previousLayer[x, zDown];
        bool upLeft = previousLayer[xLeft, zUp];
        bool upRight = previousLayer[xRight, zUp];
        bool downLeft = previousLayer[xLeft, zDown];
        bool downRight = previousLayer[xRight, zDown];
        return rules.ApplyRules(center, left, right, up, down, upLeft, upRight, downLeft, downRight);
    }
    void GenerateRules()
    {
        rules = rulesType switch
        {
            RulesType.General => new GeneralRules(ruleNumber),
            RulesType.Totalistic => new TotalisticRules(ruleNumber),
            RulesType.GrowthTotalistic => new GrowthTotalisticRules(ruleNumber),
            RulesType.LifeGrowthTotalistic => new LifeGrowthTotalisticRules(ruleNumber),
            _ => rules
        };
    }

    void GenerateAllLayers()
    {
        GenerateLayers();
        if (lastLayerOnly)
            GenerateLastCubeLayerOnly();
        else
            GenerateCubes();
    }

    void GenerateNextLayer()
    {
        if (timeSinceLastUpdate < 1f / animationFPS)
        {
            timeSinceLastUpdate += Time.deltaTime;
            return;
        }
    
        timeSinceLastUpdate = 0;
        
        bool[,] newLayer = new bool[size, size];
        
        for (int x = 0; x < size; x++)
            for (int z = 0; z < size; z++) 
                newLayer[x, z] = ApplyRule(x, 1, z);
        layers.Clear();
        layers.Add(newLayer);

        foreach (var oldCube in currentCubes)
        {
            ObjectPool.Instance.PoolCube(oldCube);
        }
        currentCubes.Clear();
        
        
        for (int x = 0; x < size; x++)
            for (int z = 0; z < size; z++)
                if (newLayer[x, z])
                {
                    GameObject newCube = ObjectPool.Instance.PullCube();
                    newCube.transform.position = new Vector3(x, 0, z);
                    currentCubes.Add(newCube);
                }
    }
    
    void GenerateTopLayer()
    {
        // size = depth * 2 + 2;
        var layer = new bool[size, size];
        layer[size / 2, size / 2] = true;
        //layer[size / 2 + 4, size / 2] = true;
        //layer[size / 2 - 4, size / 2] = true;
        layers.Add(layer);
    }

    void GenerateLayers()
    {
        for (int y = 1; y < depth; y++)
        {
            bool[,] newLayer = new bool[size, size];
            
            for (int x = 0; x < size; x++)
                for (int z = 0; z < size; z++)
                    newLayer[x, z] = ApplyRule(x, y, z);
            
            layers.Add(newLayer);
        }
    }
    void GenerateCubes()
    {
        for (int y = 0; y < depth; y++)
            for (int x = 0; x < size; x++)
                for (int z = 0; z < size; z++)
                    if (layers[y][x, z])
                        Instantiate(cube, new Vector3(x, -y, z), Quaternion.identity);
    }

    void GenerateLastCubeLayerOnly()
    {
        for (int x = 0; x < size; x++)
            for (int z = 0; z < size; z++)
                if (layers[depth-1][x, z])
                    Instantiate(cube, new Vector3(x, 0, z), Quaternion.identity);
    }

    public void GenerateRandomRules(int bits)
    {
        var random = new Random();
        
        string bitString = "";
        for (int i = 0; i < bits; i++)
            bitString += random.NextDouble() > 0.5 ? "1" : "0";
        
        ruleNumber = Convert.ToUInt32(bitString, 2);
    }

    public void GenerateRandomEvenRules(int bits)
    {
        GenerateRandomRules(bits);
        
        if (ruleNumber % 2 == 1)
            ruleNumber--;
    }

    public void GenerateRandom1DRules()
    {
        
    }
}
