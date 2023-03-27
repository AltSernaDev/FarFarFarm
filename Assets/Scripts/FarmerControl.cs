using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerControl : MonoBehaviour, ISelectable
{
    private void Start()
    {
        // Registro de farmer en array
        for (int i = 0; i < FarmersManager.farmerCtrl.Length; i++)
        {
            if (FarmersManager.farmerCtrl[i] != this && FarmersManager.farmerCtrl[i] == null)
            {
                FarmersManager.farmerCtrl[i] = this;
            }
        }
    }

    public void Select()
    {
        Debug.Log("Soy yo, " + gameObject.name);
    }

}
