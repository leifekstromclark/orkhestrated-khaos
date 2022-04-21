using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppa : Equipment, ReceivesEvents
{

    private AbilityHandler handler;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void equip(Unit host) {
        this.host = host;
        this.host.speed += 1;
        this.host.power += 1;
        this.host.health += 2;
        this.host.max_health += 2;
    }

    public void subscribe(AbilityHandler handler) {
        this.handler = handler;
        this.handler.add_subscriber("Done", this);
    }

    public void unsubscribe() {
        handler.remove_subscriber("Done", this);
    }

    public Event receive_event(Event data) {
        if (data is Done) {
            if ((data as Done).unit == host) {
                host.set_power(host.power + 2);
            }
        }
        return data;
    }
}
