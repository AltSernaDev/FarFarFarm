using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using TMPro;

public class SellingSystem : MonoBehaviour
{
    int fullPrice;
    public GameObject[] boxes;
    public int countBoxes;
    bool boolS = false;
    int[] saveSelling = new int[5];
    public TextMeshProUGUI wheatCount, eggsCount, milkCount, totalPriceText, capacity;
    public int wheatNum, eggsNum, milkNum;

    private void OnEnable()
    {
        setCurrentValeus();
    }

    void setCurrentValeus()
    {
        wheatNum = ResoucesManager.Instance.wheatCurrentStorage;
        eggsNum = ResoucesManager.Instance.eggsCurrentStorage;
        milkNum = ResoucesManager.Instance.milkCurrentStorage;
        wheatCount.text = wheatNum.ToString();
        eggsCount.text = eggsNum.ToString();
        milkCount.text = milkNum.ToString();
    }

    public void AddToSell(int Resource) // 0, 1, 2, 3
    {
        if(countBoxes<5)
        {
            switch (Resource)
            {
                case 1:
                    if(wheatNum > 99)
                    {
                        Debug.Log("Hola");
                        wheatNum -= 100;
                        wheatCount.text = wheatNum.ToString();
                        saveSelling[countBoxes] = 1;
                        fullPrice += 200;
                        CheckBoxes();
                    }
                    else
                    {
                        return;
                    }
                    break;

                case 2:
                    if (eggsNum > 99)
                    {
                        Debug.Log("entre");
                        eggsNum -= 100;
                        eggsCount.text = eggsNum.ToString();
                        saveSelling[countBoxes] = 2;
                        fullPrice += 500;
                        CheckBoxes();
                    }
                    else
                    {
                        return;
                    }
                    break;

                case 3:
                    if (milkNum > 99)
                    {
                        Debug.Log("manolo");
                        milkNum -= 100;
                        milkCount.text = milkNum.ToString();
                        saveSelling[countBoxes] = 3;
                        fullPrice += 1000;
                        CheckBoxes();
                    }
                    else
                    {
                        return;
                    }
                    break;
            }

            capacity.text = countBoxes.ToString();
            totalPriceText.text = fullPrice.ToString();


        }
        else
        {
            Debug.Log("Ya estás lleno wachin");
            return;
        }
    }
    
    void CheckBoxes()
    {
        boolS = true;
        countBoxes++;
        for (int i = 0; i < boxes.Length; i++)
        {
            if (boolS)
            {
                if (!boxes[i].activeSelf)
                {
                    boxes[i].SetActive(true);
                    boolS = false;
                }
            }
        }
    }

    public void Sell()
    {
        for (int i = 0; i < saveSelling.Length; i++)
        {
            switch(saveSelling[i])
            {
                case 1:
                    ResoucesManager.Instance.wheatCurrentStorage -= 100;
                    break;
                case 2:
                    ResoucesManager.Instance.eggsCurrentStorage -= 100;
                    break;
                case 3:
                    ResoucesManager.Instance.milkCurrentStorage -= 100;
                    break;
            }
        }
        ResoucesManager.Instance.AddCash(fullPrice);
        CancelSell();
    }
    
    public void CancelSell()
    {
        setCurrentValeus();
        fullPrice= 0;
        countBoxes = 0;
        boolS= false;
        for (int i = 0; i < boxes.Length; i++)
        {
            boxes[i].SetActive(false);
            saveSelling[i] = 0;
        }
        capacity.text = "0";
        totalPriceText.text = "0";
    }
}
