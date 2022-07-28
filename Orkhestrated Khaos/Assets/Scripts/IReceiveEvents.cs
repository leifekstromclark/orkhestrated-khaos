public interface IReceiveEvents
{
    Event receive_event(Event data);
    void subscribe(AbilityHandler handler);
    void unsubscribe();
}
