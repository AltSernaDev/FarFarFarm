using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStorage : MonoBehaviour, IAction
{
    public void Action()
    {
    }

    public void SetValues(int level)
    {
        ResoucesManager.Instance.UpadateStorage(level);
    }
}
