/// <summary>
/// Test için emri koddan veren kaynak. OrderInput (klavye) yerine
/// IOrderSource'u uygular; bot kodu deðiþmeden otomatik test yapýlýr.
/// </summary>
public class ScriptedOrderSource : IOrderSource
{
    public bool HasPendingOrder { get; private set; }

    public void IssueOrder() { HasPendingOrder = true; }

    public void ConsumeOrder() { HasPendingOrder = false; }
}