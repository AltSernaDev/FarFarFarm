using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputsManager : MonoBehaviour
{
    public static InputsManager Instance;

    public delegate void InputReceive(Vector3 position);
    public static event InputReceive OnInputUpdate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }
    void Update()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            if (OnInputUpdate != null)
                OnInputUpdate(Input.GetTouch(0).position);
        }
#endif
#if UNITY_EDITOR
        if (Input.GetButton("Fire1"))
        {
            if (OnInputUpdate != null)
                OnInputUpdate(Input.mousePosition);
        }
#endif
    }
}
