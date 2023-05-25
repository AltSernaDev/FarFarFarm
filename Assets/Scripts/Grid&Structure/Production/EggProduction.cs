using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggProduction : MonoBehaviour, IAction
{
    [SerializeField] float productionAmount = 100; //amount per hour
    [SerializeField] float requiredAmount = 500; //amount per hour

    float productionAmountPerTick, requiredAmountPerTick, currentProductionAmount, currentRequiredAmount;

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
        requiredAmount *= ((level - 1) / 5) + 1; 

        productionAmountPerTick = (productionAmount / 3600) / TicksTimeManager.Instance.TicksPerSecond;
        requiredAmountPerTick = (requiredAmount / 3600) / TicksTimeManager.Instance.TicksPerSecond;
    }

    void OnTick()
    {
        if (ResoucesManager.Instance.wheatCurrentStorage > 0 && ResoucesManager.Instance.EggsMaxStorage > ResoucesManager.Instance.eggsCurrentStorage)
        {
            currentProductionAmount += productionAmountPerTick;
            currentRequiredAmount += requiredAmountPerTick;

            if (currentProductionAmount >= 1)
            {
                ResoucesManager.Instance.eggsCurrentStorage++;
                currentProductionAmount = currentProductionAmount--;
            }
            if (currentRequiredAmount >= 1)
            {
                ResoucesManager.Instance.wheatCurrentStorage--;
                currentRequiredAmount = currentRequiredAmount--;
            }
        }
    }

    public void Action()
    {
    }
}
