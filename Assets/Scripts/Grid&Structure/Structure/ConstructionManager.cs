using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionManager : MonoBehaviour
{
    public static ConstructionManager Instance;
    [SerializeField] float selectTime = 0.75f;

    Ray ray;
    RaycastHit hit;
    Camera camera;

    float timer;

    public bool constructionMode = false;

    [SerializeField] ConstructionZone constructionZone;
    public Structure currentStructure;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    private void Start()
    {
        camera = Camera.main;
        timer = 0;

        Unselect_(true);
        if (constructionZone.gameObject.GetComponentInChildren<Structure>() != null)
        {
            currentStructure = constructionZone.gameObject.GetComponentInChildren<Structure>();
            Select_(false); 
        }
    }
    private void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Stationary)
            {
                ray = camera.ScreenPointToRay(Input.GetTouch(0).position);
                if (Physics.Raycast(ray, out hit))
                {
                    currentStructure = hit.transform.gameObject.GetComponentInParent<Structure>();
                    if (currentStructure.Level > 0)
                    {
                        if (timer <= selectTime)
                            timer += Time.deltaTime;
                    }
                    else
                        timer = 0;
                }
                else
                    timer = 0;
            }
            else
                timer = 0;
        }
        else
            timer = 0;
#endif
#if UNITY_EDITOR
        if (Input.GetButton("Fire1"))
        {
            ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponentInParent<Structure>() != null)
                {
                    currentStructure = hit.transform.gameObject.GetComponentInParent<Structure>();
                    if (currentStructure.Level > 0)
                    {
                        if (timer <= selectTime)
                            timer += Time.deltaTime;
                    }
                    else
                        timer = 0;
                }
                else
                    timer = 0;
            }
            else
                timer = 0;
        }
        else
            timer = 0;
#endif
        print(timer);
        if (timer > selectTime)
            Select_(constructionMode);
    }

    void Select_(bool select)
    {
        if (select == true)
            return;
        else
        {
            constructionMode = true;
            constructionZone.gameObject.transform.position = currentStructure.gameObject.transform.position;
            currentStructure.gameObject.transform.SetParent(constructionZone.gameObject.transform);

            constructionZone.ChildPos(currentStructure.gameObject.transform.position);
        }
    }
    public void Unselect_(bool select)
    {
        if (select == false)
            return;
        else
        {
            constructionMode = false;
            timer = 0;

            if (currentStructure != null)
            {
                currentStructure.gameObject.transform.SetParent(null);
                currentStructure = null;
            }

            constructionZone.gameObject.transform.position = Vector3.zero;
        }
    }
    public void Instanciate2x2_TEMP(GameObject a)
    {
        if (currentStructure != null)
        {
            if (currentStructure.Level == 0)
                Destroy(currentStructure.gameObject);
            currentStructure = null;
        }
        currentStructure = Instantiate(a).GetComponent<Structure>();
        Select_(false);
    }
}
