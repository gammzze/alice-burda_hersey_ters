/// <summary>
/// Durumlarýn ihtiyaç duyduðu baðýmlýlýklarý tek pakette taþýr.
/// Hepsi arayüzdür, somut sýnýf bilinmez.
/// </summary>
public class BotContext
{
    public IAgentBehavior Behavior { get; }
    public IRoseSensor Sensor { get; }
    public INavigator Navigator { get; }
    public IOrderSource Orders { get; }

    public BotContext(IAgentBehavior behavior, IRoseSensor sensor,
                      INavigator navigator, IOrderSource orders)
    {
        Behavior = behavior;
        Sensor = sensor;
        Navigator = navigator;
        Orders = orders;
    }
}