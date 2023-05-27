using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BarnUI : MonoBehaviour
{
    [SerializeField] TMP_Text wheat, milk, eggs;
    private void OnEnable()
    {
        //TicksTimeManager.OnTicksUpdate += OnTick;
        wheat.text = ResoucesManager.Instance.wheatCurrentStorage.ToString();
        milk.text = ResoucesManager.Instance.milkCurrentStorage.ToString();
        eggs.text = ResoucesManager.Instance.eggsCurrentStorage.ToString();
    }
    /*private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }
    void OnTick()
    {

    }*/
}