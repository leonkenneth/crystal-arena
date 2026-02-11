namespace CrystalArena.Decisions;

using Newtonsoft.Json.Linq;

public abstract class DecisionResult
{
    public abstract string TypeName { get; }

    public abstract void WriteJson(JObject json, SerializationContext ctx);

    public static DecisionResult ReadJson(JObject json, SerializationContext ctx)
    {
        var typeName = json["$type"]?.ToString();
        return typeName switch
        {
            nameof(BooleanResult) => BooleanResult.ReadJson(json, ctx),
            nameof(ChosenAttackers) => ChosenAttackers.ReadJson(json, ctx),
            nameof(ChosenBlocker) => ChosenBlocker.ReadJson(json, ctx),
            nameof(ChosenCards) => ChosenCards.ReadJson(json, ctx),
            nameof(ChosenModalEffectIndex) => ChosenModalEffectIndex.ReadJson(json, ctx),
            nameof(ChosenOptions) => ChosenOptions.ReadJson(json, ctx),
            nameof(ChosenPlayable) => ChosenPlayable.ReadJson(json, ctx),
            nameof(ChosenPlayer) => ChosenPlayer.ReadJson(json, ctx),
            nameof(ChosenTargets) => ChosenTargets.ReadJson(json, ctx),
            nameof(DamageAssignment) => DamageAssignment.ReadJson(json, ctx),
            nameof(Ordering) => Ordering.ReadJson(json, ctx),
            nameof(Split) => Split.ReadJson(json, ctx),
            _ => throw new System.NotSupportedException($"Unknown DecisionResult type: {typeName}"),
        };
    }
}
