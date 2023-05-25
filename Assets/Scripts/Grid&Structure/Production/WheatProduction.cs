using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheatProduction : MonoBehaviour, IAction
{
    [SerializeField] float productionAmount = 100; //amount per hour

    float productionAmountPerTick, currentProductionAmount;

    private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }
    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }

    private void Start()
    {
        SetValues(1);
    }

    public void SetValues(int level)
    {
        productionAmount *= ((level - 1) / 4) + 1;

        productionAmountPerTick = (productionAmount / 3600) / TicksTimeManager.Instance.TicksPerSecond;
    }

    void OnTick()
    {
        if (ResoucesManager.Instance.WheatMaxStorage > ResoucesManager.Instance.wheatCurrentStorage)
        {
            currentProductionAmount += productionAmountPerTick;

            if (currentProductionAmount >= 1)
            {
                ResoucesManager.Instance.wheatCurrentStorage++;
                currentProductionAmount = currentProductionAmount--;
            }
        }
    }

    public void Action()
    {
    }
}
