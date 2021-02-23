using System;
using System.Collections;
using System.Collections.Generic;
using Rules;
using UnityEngine;
using Random = System.Random;

public enum RulesType
{
    General,
    LifeGrowthTotalistic,
    Totalistic,
    GrowthTotalistic
}

public class Generator : MonoBehaviour
{
    [HideInInspector] public RulesType rulesType;
    [HideInInspector] public GameObject cube;
    [HideInInspector] public int depth = 1;
    [SerializeField] public uint ruleNumber = 4294967295;
    int width;
    int height;
    List<bool[,]> layers = new List<bool[,]>();
    RulesBase rules;
    // 2186559728 Simple flat triangles
    void Start()
    {
        GenerateRules();
        GenerateTopLayer();
        GenerateLayers();
        GenerateCubes();
    }
    

    bool ApplyRule(int x, int y, int z)
    {
        bool[,] previousLayer = layers[y - 1];
        bool center = previousLayer[x, z];
        bool left = previousLayer[(x - 1)%width, z];
        bool right = previousLayer[(x + 1)%width, z];
        bool up = previousLayer[x, (z + 1)%height];
        bool down = previousLayer[x, (z - 1)%height];
        bool upLeft = previousLayer[(x - 1)%width, (z + 1)%height];
        bool upRight = previousLayer[(x + 1)%width, (z + 1)%height];
        bool downLeft = previousLayer[(x - 1)%width, (z - 1)%height];
        bool downRight = previousLayer[(x + 1)%width, (z - 1)%height];
        //Debug.Log($"xyz = ({x}, {y}, {z}), clrud = {center}, {left}, {right}, {up}, {down}");
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

    void GenerateTopLayer()
    {
        width = depth * 2 + 2;
        height = depth * 2 + 2;
        var layer = new bool[width, height];
        layer[width / 2, height / 2] = true;
        layers.Add(layer);
    }

    void GenerateLayers()
    {
        for (int y = 1; y < depth; y++)
        {
            bool[,] newLayer = new bool[width, height];
            
            for (int x = 2; x < width - 1; x++)
                for (int z = 2; z < height - 1; z++)
                    newLayer[x, z] = ApplyRule(x, y, z);
            
            layers.Add(newLayer);
        }
    }
    void GenerateCubes()
    {
        for (int y = 0; y < depth; y++)
            for (int x = 2; x < width - 1; x++)
                for (int z = 2; z < height - 1; z++)
                    if (layers[y][x, z])
                        Instantiate(cube, new Vector3(x, -y, z), Quaternion.identity);
    }

    public void GenerateRandomRules()
    {
        var random = new Random();
        uint firstBits = (uint) random.Next(1 << 30);
        uint secondBits = (uint) random.Next(1 << 2);
        ruleNumber = (firstBits << 2) | secondBits;
    }

    public void GenerateRandomEvenRules()
    {
        var random = new Random();
        uint firstBits = (uint) random.Next(1 << 30);
        uint secondBits = (uint) random.Next(1 << 2);
        ruleNumber = (firstBits << 2) | secondBits;
        
        // Protection for the one in four billion chance the number is 1.
        if (ruleNumber == 1)
            ruleNumber = 2;

        if (ruleNumber % 2 == 1)
            ruleNumber--;
    }

    public void GenerateRandom1DRules()
    {
        
    }
}
