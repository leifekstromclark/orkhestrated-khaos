public interface ReceivesEvents
{
    Event receive_event(Event data);
    void subscribe(AbilityHandler handler);
    void unsubscribe();
}
