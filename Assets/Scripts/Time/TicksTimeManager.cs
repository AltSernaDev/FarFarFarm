using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicksTimeManager : MonoBehaviour
{
    [SerializeField] float ticksPerSecond = 2;
    float tickMaxTime;

    float tickTimer = 0;

    public static TicksTimeManager Instance;

    public delegate void TicksUpdate();
    public static event TicksUpdate OnTicksUpdate;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;

        tickMaxTime = 1 / ticksPerSecond;
    }

    private void Update()
    {
        if (tickTimer >= tickMaxTime)
        {
            tickTimer -= tickMaxTime;

            if (OnTicksUpdate != null)
                OnTicksUpdate();
        }
        tickTimer += Time.deltaTime;
    }
}
