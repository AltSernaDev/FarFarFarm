using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] int[] size = new int[2];
    [SerializeField] int[] unlockSize = new int[2];
    [SerializeField] Grid[,] grids;

    [SerializeField] float cellSize;
    [SerializeField] int[] gridSize = new int[2];

    [SerializeField] GameObject cellPrefab;
    [SerializeField] GameObject baseStructurePrefab;
    [SerializeField] GameObject filterPrefab;
    [SerializeField] Material lockMaterial;

    [SerializeField] StructureType[] structuresEnum;
    [SerializeField] StructureSo[] structuresSO;

    //List<UnlockGridCoodinate> unlockGrids = new List<UnlockGridCoodinate>();

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

    public Cell[] PositionToCells(Vector3 woldPosition, int[] size_, bool check)
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

                //if ((currentCell_.occupied || !currentGrid_.Unlock) && check || !currentGrid_.Unlock && !check)t
                if (!currentGrid_.Unlock || (currentCell_.occupied && check))
                    return null;

                cells[k] = currentCell_;
                k++;
            }
        }
        return cells;
    }

    public void Save()
    {
        print("Saving");
        GridData[,] gridsData = new GridData[size[0], size[1]]; //gridDaddy

        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                gridsData[i, j] = new GridData();  //grid

                gridsData[i, j].unlock = grids[i, j].Unlock;
                gridsData[i, j].cellsData = new CellData[gridSize[0], gridSize[1]];

                for (int a = 0; a < gridSize[0]; a++)
                {
                    for (int b = 0; b < gridSize[1]; b++)
                    {
                        gridsData[i, j].cellsData[a, b] = new CellData(); //cell

                        gridsData[i, j].cellsData[a, b].occupied = grids[i, j].cells[a, b].occupied;
                        if (grids[i, j].cells[a, b].structure != null) //structure
                        {
                            if (grids[i, j].cells[a, b].structure.type == StructureType.House)
                            {
                                PersistenceManager.Instance.data.House = true;
                            }
                            gridsData[i, j].cellsData[a, b].structureData = new StructureData(); 

                            gridsData[i, j].cellsData[a, b].structureData.type = grids[i, j].cells[a, b].structure.type;

                            gridsData[i, j].cellsData[a, b].structureData.level = grids[i, j].cells[a, b].structure.Level;
                            gridsData[i, j].cellsData[a, b].structureData.levelUpEventKey = grids[i, j].cells[a, b].structure.LevelUpEventKey;
                            gridsData[i, j].cellsData[a, b].structureData.levelUpDate = grids[i, j].cells[a, b].structure.LevelUpDate;
                        }
                        else
                            gridsData[i, j].cellsData[a, b].structureData = null;
                    }
                }
            }
        }

        PersistenceManager.Instance.data.GridDaddy = gridsData;
    }

    public void Load()
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        yield return null;

        print("Loading");
        GridData[,] gridsData = PersistenceManager.Instance.data.GridDaddy;

        for (int i = 0; i < size[0]; i++)
        {
            for (int j = 0; j < size[1]; j++)
            {
                grids[i, j].Unlock = gridsData[i, j].unlock;

                for (int a = 0; a < gridSize[0]; a++)
                {
                    for (int b = 0; b < gridSize[1]; b++)
                    {
                        grids[i, j].cells[a, b].occupied = gridsData[i, j].cellsData[a, b].occupied;

                        if (gridsData[i, j].cellsData[a, b].structureData != null)
                        {
                            if (gridsData[i, j].cellsData[a, b].structureData.type == StructureType.House)
                            {
                                PersistenceManager.Instance.data.House = true;
                            }

                            if (grids[i, j].cells[a, b].structure == null)
                            {
                                grids[i, j].cells[a, b].structure = Instantiate(baseStructurePrefab, grids[i, j].cells[a, b].transform.position, Quaternion.Euler(Vector3.zero)).GetComponent<Structure>();
                            }                                

                            grids[i, j].cells[a, b].structure.type = gridsData[i, j].cellsData[a, b].structureData.type;

                            grids[i, j].cells[a, b].structure.LoadValues
                                (gridsData[i, j].cellsData[a, b].structureData.level,
                                gridsData[i, j].cellsData[a, b].structureData.levelUpEventKey,
                                gridsData[i, j].cellsData[a, b].structureData.levelUpDate);
                        }
                        else
                            grids[i, j].cells[a, b].structure = null;
                    }
                }
            }
        }

        if(PersistenceManager.Instance.firstLoad == false)
            PersistenceManager.Instance.firstLoad = true;
    }

    public StructureSo FromEnumToSO(StructureType? Enum_)
    {
        StructureSo SO_ = null;
        for (int i = 0; i < structuresEnum.Length; i++)
        {
            if (structuresEnum[i] == Enum_)
                SO_ = structuresSO[i];
        }
        return SO_;
    }
    public StructureType? FromSoToEnum(StructureSo SO_)
    {
        StructureType? Enum_ = null;

        for (int i = 0; i < structuresSO.Length; i++)
        {
            if (structuresSO[i].name == SO_.name)
                Enum_ = structuresEnum[i];
        }


        return Enum_;
    }
}

