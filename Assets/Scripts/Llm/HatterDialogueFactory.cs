// Composition root: varsayýlan baðýmlýlýklarý tek yerde birleþtirir.
public static class HatterDialogueFactory
{
    public static NpcDialogueController Create(ILlmClient client,
                                               ConversationHistory history = null,
                                               IPersuasionTracker tracker = null,
                                               int maxRetries = 1,
                                               DialogueRules rules = null)
    {
        rules = rules ?? new DialogueRules();

        return new NpcDialogueController(
            client,
            new HatterPromptBuilder(),
            history ?? new ConversationHistory(),
            new NpcResponseParser(),
            new NpcResponseValidator(rules),
            tracker ?? new PersuasionTracker(),
            maxRetries);
    }
}
