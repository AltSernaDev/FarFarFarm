using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class TimeEventsManager : MonoBehaviour, IDataPersistence
{
    public static TimeEventsManager Instance;
    SerilizableDictionary<uint, DateTime> events = new SerilizableDictionary<uint, DateTime>();

    public delegate void DoneEventsUpdate(uint key_);
    public static event DoneEventsUpdate OnDoneEvent;

    private List<uint> doneEvents = new List<uint>();

    public List<uint> DoneEvents { get => doneEvents; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        TicksTimeManager.OnTicksUpdate += OnTick;
    }
    private void OnDisable()
    {
        TicksTimeManager.OnTicksUpdate -= OnTick;
    }

    void OnTick()
    {
        if (events.Count > 0)
        {
            foreach (KeyValuePair<uint, DateTime> event_ in events)
            {
                if (event_.Value - DateTime.Now <= TimeSpan.Zero)
                {
                    print("event «" + event_.Key + "» finish");
                    MoveToDone(event_.Key);
                    //events.Remove(event_.Key);
                }
                else
                    print("event «" + event_.Key + "» in progress, " + (event_.Value - DateTime.Now) + " left");
            }
        }
        if (doneEvents.Count > 0)
        {
            foreach (uint doneEvent_ in doneEvents)
            {
                events.Remove(doneEvent_);
            }
        }
    }

    void MoveToDone(uint key_)
    {
        OnDoneEvent(key_);
        doneEvents.Add(key_);
    }
    public void CheckDoneEvents(uint key_)
    {
        doneEvents.Remove(key_);
    }

    public bool CreateTimeEvent(uint id, TimeSpan time)
    {
        if (!events.ContainsKey(id))
        {
            events.Add(id, DateTime.Now + time);
            return true;
        }
        return false;
    }
    public uint CreateNewKey()
    {
        uint id;
        do
        {
            id = (uint)UnityEngine.Random.Range(1000, 10000);
            if (!events.ContainsKey(id))
                break;
        }
        while (events.ContainsKey(id));

        return id;
    }

    public void Save()
    {
        PersistenceManager.Instance.data.Events = events;
    }

    public void Load()
    {
        events = PersistenceManager.Instance.data.Events;
    }
}