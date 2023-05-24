using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable][CreateAssetMenu()]
public class StructureSo : ScriptableObject
{
    public string name = "Structure";
    public Sprite sprite;
    public GameObject building;
    public int[] size = new int[2] { 2, 2 };

    public int price = 100;
    public float constructionTime = 10;

    public int initialLevelUpPrice = 50;
    public int initialLevelUpTime = 10;

    public float levelUpPriceMultiplier = 0.5f;
    public float levelUpTimeMultiplier = 0.5f;
}

public enum StructureType
{
    Tree,
    Barn, 
    House
}
