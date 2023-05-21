using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ResoucesManager : MonoBehaviour, IDataPersistence
{
    public static ResoucesManager Instance;

    private int gold = 0; // hard currency
    private int cash = 0; // soft currency

    public delegate void ResoucesAction(int cash, int gold);
    public static event ResoucesAction OnValueChange;

    public int Gold { get => gold;}
    public int Cash { get => cash;}

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Start()
    {
        OnValueChange(cash, gold);
    }

    public void AddGold_TEMP(int amount)
    {
        AddGold(amount);
    }
    public void AddCash_TEMP(int amount)
    {
        AddCash(amount);
    }

    public bool AddGold(int amount)
    {
        if (amount > 0) //if max
        {
            gold += amount;
            if (OnValueChange != null)
                OnValueChange(cash, gold);
            print("Transaction end successfully :)");
            return true;
        }
        print("An error just happend, the transaction was not completed :( \nCheck your funds and try again");
        return false;
    }
    public bool PayGold(int amount)
    {
        if (amount > 0 && (gold - amount) >= 0)
        {
            gold -= amount;
            if (OnValueChange != null)
                OnValueChange(cash, gold);
            print("Transaction end successfully :)");
            return true;
        }
        print("An error just happend, the transaction was not completed :( \nCheck your funds and try again");
        return false;
    }

    public bool AddCash(int amount)
    {
        if (amount > 0) //if max
        {
            cash += amount;
            if (OnValueChange != null)
                OnValueChange(cash, gold);
            print("Transaction end successfully :)");
            return true;
        }
        print("An error just happend, the transaction was not completed :( \nCheck your funds and try again");
        return false;
    }
    public bool PayCash(int amount)
    {
        print(cash + " - " + amount);
        if (amount > 0 && (cash - amount) >= 0)
        {
            cash -= amount;
            if (OnValueChange != null)
                OnValueChange(cash, gold);
            print("Transaction end successfully :)");
            return true;
        }
        print("An error just happend, the transaction was not completed :( \nCheck your funds and try again");
        return false;
    }

    public void Save()
    {
        PersistenceManager.Instance.data.Gold = gold;
        PersistenceManager.Instance.data.Cash = cash;

        if (OnValueChange != null)
            OnValueChange(cash, gold);
    }

    public void Load()
    {
        gold = PersistenceManager.Instance.data.Gold;
        cash = PersistenceManager.Instance.data.Cash;

        if (OnValueChange != null)
            OnValueChange(cash, gold);
    }
}
