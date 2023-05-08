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

    Vector3 childPosition, traslatePosition, childDefaultPosition;                          /////////////////////////////////////////////////
    int[] size;
    GridManager gridManager;

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

        //size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        //SetBoxCollider();
    }

    public void ChildPos(Vector3 pos)                                                       //////////////////////////////////////////////////////
    {
        childDefaultPosition = pos;
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

    public bool Construct()
    {
        Cell[] cells;

        dragCamera.enabled = true;
        dragObject.enabled = false;
        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        cells = gridManager.PositionToCell(transform.GetChild(0).position, size);
        if (cells == null)
            return false;

        if (gameObject.GetComponentInChildren<Structure>().Level == 0 && !ResoucesManager.Instance.PayCash(gameObject.GetComponentInChildren<Structure>().structureSo.price))
            return false;

        cells[0].structure = gameObject.GetComponentInChildren<Structure>();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].occupied = true;
        }

        /*if (gameObject.GetComponentInChildren<Structure>().Level <= 0)
        {
            gameObject.GetComponentInChildren<Structure>().BuildUp();
        }*/

        ConstructionManager.Instance.Unselect_(true);

        return true;
    }
    public void Cancel()
    {
        dragCamera.enabled = true;
        dragObject.enabled = false;

        //throw to default position

        if (gameObject.GetComponentInChildren<Structure>().Level == 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        ConstructionManager.Instance.Unselect_(true);
    }

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

