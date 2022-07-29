using System.Collections;
using System.Collections.Generic;

public class GameState
{
    public List<UnitState> units;

    public GameState(List<UnitState> units) {
        this.units = units;
    }
}

public class UnitState
{
    public string name;
    public bool allegiance;
    public int[] board_loc;
    public List<object> stats;
    public AttachmentState equipment;
    public List<AttachmentState> buffs;

    public UnitState(string name, bool allegiance, int[] board_loc, List<object> stats, AttachmentState equipment, List<AttachmentState> buffs) {
        this.name = name;
        this.allegiance = allegiance;
        this.board_loc = board_loc;
        this.stats = stats;
        this.equipment = equipment;
        this.buffs = buffs;
    }
}

public class AttachmentState
{
    public string name;
    public List<object> stats;

    public AttachmentState(string name, List<object> stats) {
        this.name = name;
        this.stats = stats;
    }
}