using System.Collections.Generic;
using System;
using UnityEngine;

public class AbilityHandler
{

    private Dictionary<string, List<IReceiveEvents>> subscribed = new Dictionary<string, List<IReceiveEvents>>()
    {
        {"Done", new List<IReceiveEvents>()},
        {"Move", new List<IReceiveEvents>()},
        {"Attack", new List<IReceiveEvents>()},
        {"GetSwap", new List<IReceiveEvents>()},
        {"Turn", new List<IReceiveEvents>()}
    };

    private List<Flag> to_remove = new List<Flag>();
    private List<Flag> to_add = new List<Flag>();

    public void add_subscriber(string type, IReceiveEvents subscriber)
    {
        to_add.Add(new Flag(type, subscriber));
    }

    public void remove_subscriber(string type, IReceiveEvents subscriber)
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
        foreach (IReceiveEvents subscriber in subscribed[type]) {
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
    public IReceiveEvents subscriber;

    public Flag(string type, IReceiveEvents subscriber) {
        this.type = type;
        this.subscriber = subscriber;
    }
}