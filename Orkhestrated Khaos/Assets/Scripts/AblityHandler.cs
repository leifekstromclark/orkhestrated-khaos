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

    private List<Flag> to_remove = new List<Flag>();
    private List<Flag> to_add = new List<Flag>();

    public void add_subscriber(string type, ReceivesEvents subscriber)
    {
        to_add.Add(new Flag(type, subscriber));
    }

    public void remove_subscriber(string type, ReceivesEvents subscriber)
    {
        to_remove.Add(new Flag(type, subscriber));
    }

    public Event trigger(string type, Event data)
    {
        //Removed events in remove queue
        foreach (Flag flag in to_remove) {
            subscribed[flag.type].Remove(flag.subscriber);
        }
        to_remove = new List<Flag>();

        //Add events in add queue
        foreach (Flag flag in to_add) {
            subscribed[flag.type].Add(flag.subscriber);
        }
        to_add = new List<Flag>();

        //Trigger events
        foreach (ReceivesEvents subscriber in subscribed[type]) {
            bool removed = false;
            foreach (Flag flag in to_remove) {
                if (flag.type == type && subscriber == flag.subscriber) {
                    removed = true;
                }
            }
            if (!removed) {
                data = subscriber.receive_event(data);
            }
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