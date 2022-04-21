using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : Event
{
    public Unit unit;
    public Unit target;

    public Attack(Unit unit, Unit target)
    {
        this.unit = unit;
        this.target = target;
    }
}
