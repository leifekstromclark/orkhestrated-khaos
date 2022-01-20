using System.Collections.Generic;
using System;

public class EventHandler
{
    private string DONE = "done";
    private string MOVE = "move";
    private string ATTACK = "attack";

    private Dictionary<string, List<Unit>> subscribed = new Dictionary<string, List<Unit>>();

    public EventHandler() {
        subscribed.Add("done", new List<Unit>());
        subscribed.Add("move", new List<Unit>());
        subscribed.Add("attack", new List<Unit>());
    }

    public void subscribe(string type, object subscriber)
    {
        subscribed[type].Add(subscriber);
    }

    public void trigger(string type, Event data) {
        foreach (object subscriber in subscribed[type]) {
            subscriber.receive_event(data);
        }
    }
}