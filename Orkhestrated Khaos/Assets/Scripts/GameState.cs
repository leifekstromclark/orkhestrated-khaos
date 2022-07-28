using System.Collections;
using System.Collections.Generic;

public class GameState
{
    
}

public class UnitState
{
    public int id;
    public bool allegiance;
    public int[] board_loc;
    public List<object> stats;

    public UnitState(int id, bool allegiance, int[] board_loc, List<object> stats) {
        this.id = id;
        this.allegiance = allegiance;
        this.board_loc = board_loc;
        this.stats = stats;
    }
}

public class AttachmentState
{
    public int id;
    public List<object> stats;

    public AttachmentState(int id, List<object> stats) {
        this.id = id;
        this.stats = stats;
    }
}