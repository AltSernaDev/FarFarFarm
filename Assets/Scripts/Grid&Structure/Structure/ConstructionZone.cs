using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class ConstructionZone : MonoBehaviour
{
    public static ConstructionZone Instance;

    Camera camera;
    Lean.Touch.LeanDragTranslate dragObject;
    Lean.Touch.LeanDragCamera dragCamera;

    Ray ray;
    RaycastHit hit;

    Vector3 childPosition, traslatePosition;                          /////////////////////////////////////////////////
    int[] size;
    GridManager gridManager;

    Cell[] currentCells, newCells;

    public GameObject botones;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        InputsManager.OnInputUpdate += OnTouch;
    }
    private void OnDisable()
    {
        InputsManager.OnInputUpdate -= OnTouch;
    }

    private void Start()
    {
        gridManager = GridManager.Instance;

        camera = Camera.main;
        dragCamera = camera.gameObject.GetComponentInParent<Lean.Touch.LeanDragCamera>();
        dragObject = gameObject.GetComponent<Lean.Touch.LeanDragTranslate>();

        //SetBoxCollider();
    }

    public void ChildPos(Vector3 pos)                                                       //////////////////////////////////////////////////////
    {
        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;
        currentCells = gridManager.PositionToCells(pos, size, false);
    }

    void OnTouch(Vector3 position_)
    {
        Grid();

        ray = camera.ScreenPointToRay(position_);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.gameObject.GetComponentInParent<ConstructionZone>() != null)
            {
                dragCamera.enabled = false;
                dragObject.enabled = true;
                return;
            }
        }

        dragCamera.enabled = true;
        dragObject.enabled = false;
    }

    //construction
    public bool Construct()
    {
        dragCamera.enabled = true;
        dragObject.enabled = false;
        botones.SetActive(true);

        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        //clean old position 
        if (gameObject.GetComponentInChildren<Structure>().Level != 0)
        {
            currentCells[0].structure = null;

            for (int i = 0; i < currentCells.Length; i++)
            {
                currentCells[i].occupied = false;
            }
        }

        //check new position
        newCells = gridManager.PositionToCells(transform.GetChild(0).position, size, true);
        if (newCells == null)
            return false;

        //pay
        if (gameObject.GetComponentInChildren<Structure>().Level == 0 && !ResoucesManager.Instance.PayCash(gameObject.GetComponentInChildren<Structure>().structureSo.price))
            return false;


        //set new position
        newCells[0].structure = gameObject.GetComponentInChildren<Structure>();
        for (int i = 0; i < newCells.Length; i++)
        {
            newCells[i].occupied = true;
        }

        /*if (gameObject.GetComponentInChildren<Structure>().Level <= 0)
        {
            gameObject.GetComponentInChildren<Structure>().BuildUp();
        }*/

        ConstructionManager.Instance.Unselect_();

        return true;
    }
    public void Cancel()
    {
        dragCamera.enabled = true;
        dragObject.enabled = false;
        botones.SetActive(true);

        if (gameObject.GetComponentInChildren<Structure>().Level == 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }
        else
        {
            transform.GetChild(0).position = currentCells[0].transform.position;

            currentCells[0].structure = gameObject.GetComponentInChildren<Structure>();
            for (int i = 0; i < newCells.Length; i++)
            {
                currentCells[i].occupied = true;
            }
        }

        ConstructionManager.Instance.Unselect_();
    }

    //drag
    private void SetBoxCollider()
    {
        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        gameObject.GetComponent<BoxCollider>().size = new Vector3(size[0], 1, size[1]);
        gameObject.GetComponent<BoxCollider>().center = new Vector3(size[0] / 2, 0.5f, size[1] / 2);
        gameObject.GetComponent<BoxCollider>().enabled = false;
    }
    private void Grid()
    {
        if (gameObject.GetComponentInChildren<Structure>() != null)
        {
            childPosition.x = Mathf.Round(transform.position.x);
            childPosition.y = 0;
            childPosition.z = Mathf.Round(transform.position.z);

            transform.GetChild(0).position = childPosition;

            if (dragCamera.enabled)
                transform.position = childPosition; 

            FixPosition();
        }
    }
    private void FixPosition()
    {
        traslatePosition.x = gameObject.transform.position.x;
        traslatePosition.y = 0;
        traslatePosition.z = gameObject.transform.position.z + gameObject.transform.position.y;

        gameObject.transform.position = traslatePosition;
    }
}

