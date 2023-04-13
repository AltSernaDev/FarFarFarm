using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class ConstructionZone : MonoBehaviour
{
    Camera camera;
    Lean.Touch.LeanDragTranslate dragObject;
    Lean.Touch.LeanDragCamera dragCamera;

    Ray ray;
    RaycastHit hit;

    Vector3 childPosition, childDefaultPosition, traslatePosition;
    int[] size;
    GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.Instance;
        childDefaultPosition = transform.GetChild(0).position;

        camera = Camera.main;
        dragCamera = camera.gameObject.GetComponentInParent<Lean.Touch.LeanDragCamera>();
        dragObject = gameObject.GetComponent<Lean.Touch.LeanDragTranslate>();
        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        //SetBoxCollider();
    }

    private void LateUpdate()
    {
        Grid();
    }

    private void Update()
    {
        FixPosition();

#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.GetComponentInParent<ConstructionZone>() != null)
                    {
                        dragCamera.enabled = false;
                        dragObject.enabled = true;
                    }
                    else
                    {
                        dragCamera.enabled = true;
                        dragObject.enabled = false;
                    }
                }
                else
                {
                    dragCamera.enabled = true;
                    dragObject.enabled = false;
                }
            }
        }
#endif
#if UNITY_EDITOR
        if (Input.GetButton("Fire1"))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponentInParent<ConstructionZone>() != null)
                {
                    dragCamera.enabled = false;
                    dragObject.enabled = true;
                }
                else
                {
                    dragCamera.enabled = true;
                    dragObject.enabled = false;
                }
            }
            else
            {
                dragCamera.enabled = true;
                dragObject.enabled = false;
            }
        }
#endif

    }

    public void Construct()
    {
        Cell[] cells;

        dragCamera.enabled = true;
        dragObject.enabled = false;

        cells = gridManager.PositionToCell(transform.GetChild(0).position, size);
        if (cells == null)
            return;

        cells[0].structure = gameObject.GetComponentInChildren<Structure>();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].occupied = true;
        }

        if (gameObject.GetComponentInChildren<Structure>().Level <= 0)
        {
            gameObject.GetComponentInChildren<Structure>().BuildUp();
        }


        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }
    public void Cancel()
    {
        dragCamera.enabled = true;
        dragObject.enabled = false;

        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        Destroy(gameObject);
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
        childPosition.x = Mathf.Round(transform.position.x);
        childPosition.y = 0;
        childPosition.z = Mathf.Round(transform.position.z);

        transform.GetChild(0).position = childPosition;

        if (dragCamera.enabled)
            transform.position = childPosition;
    }
    private void FixPosition()
    {
        traslatePosition.x = gameObject.transform.position.x;
        traslatePosition.y = 0;
        traslatePosition.z = gameObject.transform.position.z + gameObject.transform.position.y;

        gameObject.transform.position = traslatePosition;
    }
}
///
/*public class ConstructionZone : MonoBehaviour
{
    Camera camera;
    Lean.Touch.LeanDragTranslate dragObject;
    Lean.Touch.LeanDragCamera dragCamera;

    Ray ray;
    RaycastHit hit;

    Vector3 childPosition, childDefaultPosition, traslatePosition;
    int[] size; 
    GridManager gridManager;

    private void Start()
    {
        gridManager = GridManager.Instance;
        childDefaultPosition = transform.GetChild(0).position;

        camera = Camera.main;
        dragCamera = camera.gameObject.GetComponentInParent<Lean.Touch.LeanDragCamera>();
        dragObject = gameObject.GetComponent<Lean.Touch.LeanDragTranslate>();
        size = gameObject.GetComponentInChildren<Structure>().structureSo.size;

        //SetBoxCollider();
    }

    private void LateUpdate()
    {
        Grid();
    }

    private void Update()
    {
        FixPosition();
        
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary || Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.GetComponentInParent<ConstructionZone>() != null)
                    {
                        dragCamera.enabled = false;
                        dragObject.enabled = true;
                    }
                    else
                    {
                        dragCamera.enabled = true;
                        dragObject.enabled = false;
                    }
                }
                else
                {
                    dragCamera.enabled = true;
                    dragObject.enabled = false;
                }
            }
        }
#endif
#if UNITY_EDITOR
        if (Input.GetButton("Fire1"))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponentInParent<ConstructionZone>() != null)
                {
                    dragCamera.enabled = false;
                    dragObject.enabled = true;
                }
                else
                {
                    dragCamera.enabled = true;
                    dragObject.enabled = false;
                }
            }
            else
            {
                dragCamera.enabled = true;
                dragObject.enabled = false;
            }
        }
#endif

    }

    public void Construct()
    {
        Cell[] cells;

        dragCamera.enabled = true;
        dragObject.enabled = false;

        cells = gridManager.PositionToCell(transform.GetChild(0).position, size);
        if (cells == null)
            return;

        cells[0].structure = gameObject.GetComponentInChildren<Structure>();
        for (int i = 0; i < cells.Length; i++)
        {
            cells[i].occupied = true;
        }

        if (gameObject.GetComponentInChildren<Structure>().Level <= 0)
        {
            gameObject.GetComponentInChildren<Structure>().BuildUp();
        }


        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        transform.GetChild(0).parent = null;
        Destroy(gameObject);
    }
    public void Cancel()
    {
        dragCamera.enabled = true;
        dragObject.enabled = false;

        Destroy(gameObject.GetComponentInChildren<Canvas>().gameObject);
        Destroy(gameObject);
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
        childPosition.x = Mathf.Round(transform.position.x);
        childPosition.y = 0;
        childPosition.z = Mathf.Round(transform.position.z);

        transform.GetChild(0).position = childPosition;

        if (dragCamera.enabled)
            transform.position = childPosition;
    }
    private void FixPosition()
    {
        traslatePosition.x = gameObject.transform.position.x;
        traslatePosition.y = 0;
        traslatePosition.z = gameObject.transform.position.z + gameObject.transform.position.y;

        gameObject.transform.position = traslatePosition;
    }
}*/
///

