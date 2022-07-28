using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choppa : Equipment, IReceiveEvents
{

    private AbilityHandler handler;

    public override void equip(Unit host) {
        this.host = host;
        this.host.equipment = this;
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
                host.set_power(host.power + 1);
            }
        }
        return data;
    }
}
