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

    private List<Flag> flags = new List<Flag>();

    public void add_subscriber(string type, ReceivesEvents subscriber)
    {
        subscribed[type].Add(subscriber);
    }

    public void remove_subscriber(string type, ReceivesEvents subscriber) {
        flags.Add(new Flag(type, subscriber));
    }

    public Event trigger(string type, Event data) {
        foreach (Flag flag in flags) {
            subscribed[flag.type].Remove(flag.subscriber);
        }
        flags = new List<Flag>();
        foreach (ReceivesEvents subscriber in subscribed[type]) {
            data = subscriber.receive_event(data);
        }
        return data;
    }
}

public class Flag {
    public string type;
    public ReceivesEvents subscriber;

    public Flag(string type, ReceivesEvents subscriber) {
        this.type = type;
        this.subscriber = subscriber;
    }
}