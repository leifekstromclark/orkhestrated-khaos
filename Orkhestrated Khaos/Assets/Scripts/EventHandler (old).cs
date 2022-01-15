using System.Collections.Generic;

public class EventHandler
{
    private string DONE = "done";
    private string MOVE = "move";
    private string ATTACK = "attack";

    private List<Action<Event>> done_events = new List<Action<Event>>();
    private List<Action<Event>> move_events = new List<Action<Event>>();
    private List<Action<Event>> attack_events = new List<Action<Event>>();

    public void subscribe(string type, Action<Event> method)
    {
        //add a system to remove events
        
        if (type == DONE)
        {
            done_events.Add(method);
        }
        else if (type == MOVE)
        {
            move_events.Add(method);
        }
        else if (type == ATTACK)
        {
            attack_events.Add(method);
        }
    }

    public void done(Done data)
    {
        foreach (Action<Event> method in done_events)
        {
            method(data);
        }
    }

    public void move(Move data)
    {
        foreach (Action<Event> method in move_events)
        {
            method(data);
        }
    }

    public void attack(Attack data)
    {
        foreach (Action<Event> method in attack_events)
        {
            method(data);
        }
    }
}
