using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityHandler
{

    private Dictionary<string, List<ReceivesEvents>> subscribed = new Dictionary<string, List<ReceivesEvents>>()
    {
        {"done", new List<ReceivesEvents>()},
        {"move", new List<ReceivesEvents>()},
        {"attack", new List<ReceivesEvents>()}
    };

    public void subscribe(string type, ReceivesEvents subscriber)
    {
        subscribed[type].Add(subscriber);
    }

    public Event trigger(string type, Event data) {
        foreach (ReceivesEvents subscriber in subscribed[type]) {
            data = subscriber.receive_event(data);
        }
        return data;
    }
}