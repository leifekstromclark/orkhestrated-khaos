public class Move : Event
{
    private Unit unit;

    public Move(Unit unit)
    {
        this.unit = unit;
    }
}