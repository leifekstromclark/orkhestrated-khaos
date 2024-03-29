using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualStabbaz : Equipment
{

    public override void equip(Unit host) {
        this.host = host;
        this.host.equipment = this;
        this.host.attacks += 1;
        this.host.speed += 2;
        this.host.health += 2;
        this.host.max_health += 2;
    }
}