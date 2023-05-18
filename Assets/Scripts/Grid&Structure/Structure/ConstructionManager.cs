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
    public Structure selectStructure;
    Structure currentStructure;

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

        Unselect_();
        if (constructionZone.gameObject.GetComponentInChildren<Structure>() != null)
        {
            currentStructure = constructionZone.gameObject.GetComponentInChildren<Structure>();
            Select_(); 
        }
    }
    private void OnEnable()
    {
        InputsManager.OnInputUpdate += OnTouch;
    }
    private void OnDisable()
    {
        InputsManager.OnInputUpdate -= OnTouch;
    }

    void OnTouch(Vector3 position_)
    {
/*
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.GetTouch(0).phase == TouchPhase.Stationary)
#endif
#if UNITY_EDITOR
        if (true)
#endif
        {*/
            ray = camera.ScreenPointToRay(position_);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.GetComponentInParent<Structure>() != null)
                {
                    if (currentStructure != hit.transform.gameObject.GetComponentInParent<Structure>())
                        timer = 0;

                    currentStructure = hit.transform.gameObject.GetComponentInParent<Structure>();

                    if (currentStructure.Level > 0)
                    {
                        if (timer <= selectTime)
                            timer += Time.deltaTime;
                        else
                            Select_();
                        return;
                    }
                }
            }
            timer = 0;
        //}       
    }

    public void Select_()
    {
        if (selectStructure == null)
        {
            selectStructure = currentStructure;

            constructionMode = true;
            constructionZone.gameObject.transform.position = selectStructure.gameObject.transform.position;
            selectStructure.gameObject.transform.SetParent(constructionZone.gameObject.transform);

            constructionZone.ChildPos(selectStructure.transform.position);
        }
    }
    public void Unselect_()
    {
        constructionMode = false;
        timer = 0;

        if (selectStructure != null)
        {
            selectStructure.gameObject.transform.SetParent(null);
            selectStructure = null;
        }
    }
    public void Instanciate2x2_TEMP(GameObject a)
    {
        if (selectStructure != null)
        {
            if (selectStructure.Level == 0)
                Destroy(selectStructure.gameObject);
            Unselect_();
        }
        currentStructure = Instantiate(a).GetComponent<Structure>();
        Select_();
        print(selectStructure.gameObject.transform.parent.name);
    }
}
