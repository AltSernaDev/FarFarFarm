using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmersManager : MonoBehaviour
{
    public static GameObject[] farmer;
    public static FarmersManager FarmManagerSingleton;
    public FarmerControl[] farmerCtrl = new FarmerControl[15];

    private void Awake()
    {
        if (FarmManagerSingleton == null)
            FarmManagerSingleton = this;
        else
        {
            Debug.LogWarning("ERROR 2 FARMER MANAGERS");
            Debug.Log("El anterior es " + FarmManagerSingleton.gameObject.name +
                "Y el segundo encontrado es " + gameObject.name);
            Destroy(this.gameObject);
        }
            
    }
}
