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
    public float initialLevelUpTime = 10;
}
