using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmerControl : MonoBehaviour, ISelectable
{
    bool placed;

    private void Start()
    {
        // Registro de farmer en array
        for (int i = 0; i < FarmersManager.FarmManagerSingleton.farmerCtrl.Length; i++)
        {
            if (FarmersManager.FarmManagerSingleton.farmerCtrl[i] != this 
                && FarmersManager.FarmManagerSingleton.farmerCtrl[i] == null && !placed)
            {
                placed= true;
                FarmersManager.FarmManagerSingleton.farmerCtrl[i] = this; // ESTE ES EL SUB
            }
        }
    }

    public void Select()
    {
        Debug.Log("Soy yo, " + gameObject.name);
    }

}
