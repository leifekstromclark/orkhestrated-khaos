using System.Collections.Generic;
using System;

public class GetSwap : Event
{
    public Unit unit;
    public String[][][] locations;

    public GetSwap(Unit unit, String[][][] locations)
    {
        this.unit = unit;
        this.locations = locations;
    }
}