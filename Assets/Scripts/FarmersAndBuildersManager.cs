using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;

public class FarmersAndBuildersManager : MonoBehaviour
{
    public static GameObject[] farmer;
    public static FarmersAndBuildersManager SingletonInstance;
    public IndividualCharacterControl[] farmerCtrl = new IndividualCharacterControl[13];
    public IndividualCharacterControl[] builderCtrl = new IndividualCharacterControl[3];

    private void Awake()
    {
        if (SingletonInstance == null)
            SingletonInstance = this;
        else
        {
            Debug.LogWarning("ERROR 2 FARMER MANAGERS");
            Debug.Log("El anterior es " + SingletonInstance.gameObject.name +
                "Y el segundo encontrado es " + gameObject.name);
            Destroy(this.gameObject);
        }
            
    }

    public IndividualCharacterControl FindBuilder()
    {
        IndividualCharacterControl character = null;

        for (int i = 0; i < builderCtrl.Length; i++)
        {
            if (!builderCtrl[i].busy)
            {
                character = builderCtrl[i];
            }
        }

        return character;
    }

    public IndividualCharacterControl FindFarmer()
    {
        IndividualCharacterControl character = null;

        for (int i = 0; i < farmerCtrl.Length; i++)
        {
            if (!farmerCtrl[i].busy)
            {
                character= farmerCtrl[i];
            }
        }

        return character;
    }
}
