using System.Collections.Generic;
using System;
using UnityEngine;

public class Handler
{

    private Dictionary<string, List<Unit>> subscribed = new Dictionary<string, List<Unit>>();

    public Handler() {
        subscribed.Add("done", new List<Unit>());
        subscribed.Add("move", new List<Unit>());
        subscribed.Add("attack", new List<Unit>());
    }

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