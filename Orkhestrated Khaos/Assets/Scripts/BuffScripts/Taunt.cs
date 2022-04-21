using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taunt : Buff, ReceivesEvents
{

    public int turns_remaining;
    private AbilityHandler handler;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void subscribe(AbilityHandler handler) {
        this.handler = handler;
        this.handler.add_subscriber("GetSwap", this);
        this.handler.add_subscriber("Turn", this);
    }

    public void unsubscribe() {
        handler.remove_subscriber("GetSwap", this);
        handler.remove_subscriber("Turn", this);
    }

    public Event receive_event(Event data) {
        if (data is GetSwap) {
            GetSwap casted_data = data as GetSwap;
            if (casted_data.unit == host) {
                casted_data.locations = null;
            }
            data = casted_data;
        }
        else if (data is Turn) {
            if ((data as Turn).turn == host.allegiance) {
                turns_remaining -= 1;
                if (turns_remaining == 0) {
                    expunge();
                }
            }
        }
        return data;
    }
}
