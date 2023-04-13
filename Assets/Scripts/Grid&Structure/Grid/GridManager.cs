using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] int[] size = new int[2];
    [SerializeField] int[] unlockSize = new int[2];
    [SerializeField] Grid[,] grids;

    [SerializeField] float cellSize;
    [SerializeField] int[] gridSize = new int[2];

    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject filterPrefab;
    [SerializeField] Material lockMaterial;

    public static GridManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        if (size[0] < unlockSize[0])
            unlockSize[0] = size[0];

        if (size[1] < unlockSize[1])
            unlockSize[1] = size[1];

        int startX = (int)(size[0] / 2) - (int)(unlockSize[0] / 2);
        int endX = startX + unlockSize[0];

        int startY = (int)(size[1] / 2) - (int)(unlockSize[1] / 2);
        int endY = startX + unlockSize[1];

        grids = new Grid[size[0], size[1]];
        GameObject currentGrid;

        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                currentGrid = Instantiate(filterPrefab);
                currentGrid.name = "Grid " + i + ", " + j;

                currentGrid.transform.localPosition += (Vector3.up * 0.1f);
                Destroy(currentGrid.GetComponent<Collider>());

                currentGrid.GetComponent<MeshRenderer>().material = lockMaterial;

                //currentGrid = new GameObject("Grid " + i + ", " + j);

                currentGrid.transform.parent = transform;

                grids[i, j] = currentGrid.AddComponent<Grid>();
                grids[i, j].Initial(gridSize[0], gridSize[1], cellPrefab, cellSize);

                if (i >= startX && i < endX && j >= startY && j < endY)
                    grids[i, j].Unlock = true;

                currentGrid.transform.position = new Vector3(i * cellSize * gridSize[0], currentGrid.transform.position.y, j * cellSize * gridSize[1]);
            }
        }
    }

    public Cell[] PositionToCell(Vector3 woldPosition, int[] size_)
    {
        Cell[] cells;

        Grid currentGrid_;
        Cell currentCell_;

        float positionX = woldPosition.x,
            positionY = woldPosition.z,
            globalCellX = positionX / cellSize,
            globalCellY = positionY / cellSize;

        cells = new Cell[size_[0] * size_[1]];

        int k = 0;
        for (int i = 0; i < size_[0]; i++)
        {
            for (int j = 0; j < size_[1]; j++)
            {
                positionX = woldPosition.x + i * cellSize;
                positionY = woldPosition.z + j * cellSize;
                globalCellX = positionX / cellSize;
                globalCellY = positionY / cellSize;

                if (globalCellX < 0 || globalCellX / gridSize[0] >= size[0] || globalCellY < 0 || globalCellY / gridSize[1] >= size[1])
                    return null;

                currentGrid_ = grids[(int)globalCellX / gridSize[0], (int)globalCellY / gridSize[0]];
                currentCell_ = currentGrid_.cells[(int)(globalCellX % gridSize[0]), (int)(globalCellY % gridSize[0])];

                if (currentCell_.occupied == true || currentGrid_.Unlock == false)
                    return null;

                cells[k] = currentCell_;
                k++;
            }
        }

        //print("grid: " + currentGrid_.name + " / cell: " + currentCell_.name + " // occupied:" + currentCell_.occupied + " / lock: " + !currentGrid_.Unlock);

        return cells;
    }
}
