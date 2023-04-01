using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] int[] size = new int[2];
    public Cell[,] cells;
    [SerializeField] private bool unlock;

    public bool Unlock 
    { 
        get { return unlock; }
        set 
        {
            if (value)
            {
                Destroy(gameObject.GetComponent<MeshRenderer>());
                Destroy(gameObject.GetComponent<MeshFilter>());
            }

            unlock = value; 
        }
    }

    public void Initial(int sizeX, int sizeY, GameObject cellPrefab, float cellSize)
    {
        size[0] = sizeX;
        size[1] = sizeY;

        cells = new Cell[size[0], size[1]];

        GameObject currentCell;

        for (int i = 0; i < cells.GetLength(0); i++)
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                currentCell = Instantiate(cellPrefab, transform);
                currentCell.transform.localPosition = currentCell.transform.localPosition - (Vector3.up * 0.1f);
                currentCell.name = "Cell " + i + ", " + j;

                cells[i, j] = currentCell.GetComponent<Cell>();
                cells[i, j].position[0] = i;
                cells[i, j].position[1] = j;

                currentCell.transform.position = new Vector3(i * cellSize, currentCell.transform.position.y, j * cellSize);
            }
        }        
    }
}
