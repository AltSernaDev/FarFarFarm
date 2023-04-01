using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResoucesUI : MonoBehaviour
{ 
    [SerializeField] TMP_Text moneyText;
    [SerializeField] TMP_Text goldText;

    private void OnEnable()
    {
        ResoucesManager.OnValueChange += ResoucesUpdate;
    }
    private void OnDisable()
    {
        ResoucesManager.OnValueChange -= ResoucesUpdate;
    }
    void ResoucesUpdate(int money, int gold)
    {
        moneyText.text = money.ToString();
        goldText.text = gold.ToString();
    }
}
