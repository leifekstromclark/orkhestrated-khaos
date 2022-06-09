using System.Collections.Generic;
using System;

public class GetSwap : Event
{
    public Unit unit;
    public String[][][] locations;
    public int num_locs;

    public GetSwap(Unit unit, String[][][] locations, int num_locs)
    {
        this.unit = unit;
        this.locations = locations;
        this.num_locs = num_locs;
    }
}