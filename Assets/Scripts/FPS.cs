using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FPS : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    /*private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }
    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }
    void OnTick()
    {
        text.text = (int)(1/Time.deltaTime) + " FPS";
    }*/
    private void Update()
    {
        text.text = (int)(1/Time.deltaTime) + " FPS";
    }
}
