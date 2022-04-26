using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticka : Equipment, ReceivesEvents
{

    private AbilityHandler handler;

    public override void equip(Unit host) {
        this.host = host;
        this.host.equipment = this;
        this.host.speed += 1;
        this.host.health += 4;
        this.host.max_health += 4;
    }

    public void subscribe(AbilityHandler handler) {
        this.handler = handler;
        this.handler.add_subscriber("Attack", this);
    }

    public void unsubscribe() {
        handler.remove_subscriber("Attack", this);
    }

    public Event receive_event(Event data) {
        if (data is Attack) {
            Attack casted_data = data as Attack;
            if (casted_data.unit == host) {
                Taunt restack = null;
                foreach (Buff buff in casted_data.target.buffs) {
                    if (buff is Taunt) { //could be using keywords if want to restrict stacking of a group of buffs
                        restack = (buff as Taunt);
                    }
                }
                if (restack) {
                    restack.turns_remaining = 2;
                }
                else {
                    Taunt taunt = (Instantiate(Resources.Load("BuffPrefabs/Taunt"), casted_data.target.transform) as GameObject).GetComponent<Taunt>();
                    taunt.turns_remaining = 2;
                    taunt.host = casted_data.target;
                    taunt.subscribe(handler);
                    casted_data.target.buffs.Add(taunt);
                }
            }
        }
        return data;
    }
}
