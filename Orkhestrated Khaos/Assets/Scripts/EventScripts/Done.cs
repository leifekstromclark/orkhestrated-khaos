public class Done : Event
{
    public Unit unit;

    public Done(Unit unit)
    {
        this.unit = unit;
    }
}