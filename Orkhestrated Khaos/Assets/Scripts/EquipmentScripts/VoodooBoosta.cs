using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class VoodooBoosta : Equipment, ReceivesEvents
{

    private AbilityHandler handler;

    public override void equip(Unit host) {
        this.host = host;
        this.host.equipment = this;
        this.host.range += 1;
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
                //make heal function for similar heal abilties
                Unit target = null;
                int max_health_missing = 0;
                foreach (Unit[] row in host.game.board) {
                    foreach (Unit unit in row) {
                        if (unit && unit.allegiance == host.allegiance) {
                            int health_missing = unit.max_health - unit.health;
                            if (health_missing > max_health_missing) {
                                max_health_missing = health_missing;
                                target = unit;
                            }
                        }
                    }
                }
                if (target) {
                    target.set_health(Math.Min(target.max_health, target.health + 2));
                }
            }
        }
        return data;
    }
}
