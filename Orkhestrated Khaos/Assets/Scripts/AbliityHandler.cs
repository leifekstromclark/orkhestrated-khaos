using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityHandler
{

    private Dictionary<string, List<Unit>> subscribed = new Dictionary<string, List<Unit>>()
    {
        {"done", new List<Unit>()},
        {"move", new List<Unit>()},
        {"attack", new List<Unit>()}
    };

    public void subscribe(string type, Unit subscriber)
    {
        subscribed[type].Add(subscriber);
    }

    public void trigger(string type, Event data) {
        foreach (Unit subscriber in subscribed[type]) {
            subscriber.receive_event(data);
        }
    }
}