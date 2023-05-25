using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellingSystem : MonoBehaviour
{
    int fullPrice;
    public GameObject[] boxes;
    int countBoxes;
    bool boolS = false;

    public void AddToSell(int Resource) // 0, 1, 2
    {
        if(countBoxes<5)
        {
            boolS= true;
            countBoxes++;
            for (int i = 0; i < boxes.Length; i++)
            {
                if(boolS)
                {
                    if (!boxes[i].activeSelf)
                    {
                        boxes[i].SetActive(true);
                        boolS=false;
                    }
                }
                else
                {
                    return;
                }
            }

            switch (Resource)
            {
                case 0:
                    fullPrice += 200;
                    break;
                case 1:
                    fullPrice += 500;
                    break;
                case 2:
                    fullPrice += 1000;
                    break;
            }
        }
        else
        {
            Debug.Log("Ya estás lleno wachin");
            return;
        }
    }

    public void Sell()
    {

    }
    
    public void CancelSell()
    {
        countBoxes = 0;
        boolS= false;
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetActive(false);
        }
    }
}
