public class Attack: Event
{
    public Attack(Unit unit, Unit target)
    {
        this.unit = unit;
        this.target = target;
    }
}