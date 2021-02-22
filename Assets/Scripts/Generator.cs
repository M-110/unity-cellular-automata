using System;
using System.Collections;
using System.Collections.Generic;
using Rules;
using UnityEngine;
using Random = System.Random;

public enum RulesType
{
    General,
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
        bool center = layers[y - 1][x, z];
        bool left = layers[y - 1][x - 1, z];
        bool right = layers[y - 1][x + 1, z];
        bool up = layers[y - 1][x, z + 1];
        bool down = layers[y - 1][x, z - 1];
        //Debug.Log($"xyz = ({x}, {y}, {z}), clrud = {center}, {left}, {right}, {up}, {down}");
        bool ex = rules.ApplyRules(center, left, right, up, down);
        
        return ex;
    }
    void GenerateRules()
    {
        if (rulesType == RulesType.General)
            rules = new GeneralRules(ruleNumber);
        else if (rulesType == RulesType.GrowthTotalistic)
            rules = new GrowthTotalisticRules(ruleNumber);
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
