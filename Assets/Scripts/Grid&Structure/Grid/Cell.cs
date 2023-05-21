using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Cell : MonoBehaviour
{
    public int[] position = new int[2];
    public bool occupied;
    public Structure structure;
}
