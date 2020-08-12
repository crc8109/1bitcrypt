using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

public class Callbacks : MonoBehaviour
{
    static Callbacks t;
    [ShowInInspector]
    List<CB> upCallbacks = new List<CB>();
    [ShowInInspector]
    List<CB> fixedCallbacks = new List<CB>(); 

    private void Awake()
    {
        t = this; 
    }



    private void Update()
    {
        bool didUpdate = false;
        foreach(var cb in upCallbacks) {            cb.Remaining -= Time.deltaTime;
            if (cb.Remaining < 0)
            {
                cb.Action();
                didUpdate = true;
            }
        };
        if (didUpdate)
        {
            upCallbacks = upCallbacks.Where(cb => cb.Remaining > 0).ToList();
        }
    }

    private void FixedUpdate()
    {
        bool didUpdate = false;
        for(int i = 0; i < fixedCallbacks.Count; i++)
        {
            var cb = fixedCallbacks[i]; 
            cb.Remaining -= Time.fixedDeltaTime;
            if (cb.Remaining < 0)
            {
                cb.Action();
                didUpdate = true;
            }
        };
        if (didUpdate)
        {
            fixedCallbacks = fixedCallbacks.Where(cb => cb.Remaining > 0).ToList();
        }
    }

    void UpdateList(List<CB> list, float delta)
    {
        bool didUpdate = false;
        List<CB> toRemove = new List<CB>(); 
        for(int i = 0; i < fixedCallbacks.Count; i++)
        {
            var cb = fixedCallbacks[i]; 
            cb.Remaining -= delta;
            if (cb.Remaining < 0)
            {
                cb.Action();
                didUpdate = true;
                toRemove.Add(cb); 
            }
        };
        if (didUpdate) {
            toRemove.ForEach(cb => list.Remove(cb));
        }
    }

    public static void Add(Action action, float duration)
    {
        t.upCallbacks.Add(new CB
        {
            Action = action,
            Remaining = duration
        });
    }
    public static void AddFixed(Action action, float duration)
    {
        t.fixedCallbacks.Add(new CB
        {
            Action = action,
            Remaining = duration
        });
    }
}
[Serializable]
class CB
{
    public float Remaining;
    public Action Action; 
}