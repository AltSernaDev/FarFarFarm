using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashFarm : MonoBehaviour, IAction
{
    [SerializeField] float amount = 500; //amount per hour
    [SerializeField] float maxStorage = 4000;
    [SerializeField] float currentStorage = 0;

    float amountPerTick;

    private void Start()
    {
        amountPerTick = (amount / 3600) / TicksTimeManager.Instance.TicksPerSecond;
    }
    private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }
    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }

    private void OnTick()
    {
        if (currentStorage < maxStorage)
            currentStorage += amountPerTick;
        else
            currentStorage = maxStorage;
    }
    public void Action()
    {
        ResoucesManager.Instance.AddCash((int)currentStorage);
    }

    public void SetValues(int level)
    {
    }
}
