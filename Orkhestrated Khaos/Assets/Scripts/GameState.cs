using System.Collections;
using System.Collections.Generic;

public class GameState
{
    
}

public class UnitState
{
    public bool allegiance;
    public int[] board_loc;
    public List<object> stats;

    public UnitState(bool allegiance, int[] board_loc, List<object> stats) {
        this.allegiance = allegiance;
        this.board_loc = board_loc;
        this.stats = stats;
    }
}