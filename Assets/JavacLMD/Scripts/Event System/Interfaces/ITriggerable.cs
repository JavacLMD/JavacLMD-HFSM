namespace JavacLMD.EventSystem
{
    public interface ITriggerable<TEventId>
    {
        void Trigger(TEventId eventId);
        void Trigger<TGameEvent>(TEventId eventId, TGameEvent eventData);
    }
}