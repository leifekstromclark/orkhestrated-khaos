public class Move : Event
{
    public Unit unit;

    public Move(Unit unit)
    {
        this.unit = unit;
    }
}