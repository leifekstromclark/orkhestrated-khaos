using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityHandler
{

    private Dictionary<string, List<ReceivesEvents>> subscribed = new Dictionary<string, List<ReceivesEvents>>()
    {
        {"Done", new List<ReceivesEvents>()},
        {"Move", new List<ReceivesEvents>()},
        {"Attack", new List<ReceivesEvents>()},
        {"GetSwap", new List<ReceivesEvents>()},
        {"Turn", new List<ReceivesEvents>()}
    };

    public void add_subscriber(string type, ReceivesEvents subscriber)
    {
        subscribed[type].Add(subscriber);
    }

    public void remove_subscriber(string type, ReceivesEvents subscriber) {
        subscribed[type].Remove(subscriber);
    }

    public Event trigger(string type, Event data) {
        foreach (ReceivesEvents subscriber in subscribed[type]) {
            data = subscriber.receive_event(data);
        }
        return data;
    }
}