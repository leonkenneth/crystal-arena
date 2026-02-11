namespace CrystalArena.Decisions;

public abstract class DecisionResult
{
    public abstract string TypeName { get; }

    public abstract void Serialize(JsonFormatter.ISerializationInfo info);

    public static DecisionResult Deserialize(JsonFormatter.ISerializationInfo info)
    {
        var typeName = info.GetString("$type");
        return typeName switch
        {
            nameof(BooleanResult) => BooleanResult.FromObjectData(info),
            nameof(ChosenAttackers) => ChosenAttackers.FromObjectData(info),
            nameof(ChosenBlocker) => ChosenBlocker.FromObjectData(info),
            nameof(ChosenCards) => ChosenCards.FromObjectData(info),
            nameof(ChosenModalEffectIndex) => ChosenModalEffectIndex.FromObjectData(info),
            nameof(ChosenOptions) => ChosenOptions.FromObjectData(info),
            nameof(ChosenPlayable) => ChosenPlayable.FromObjectData(info),
            nameof(ChosenPlayer) => ChosenPlayer.FromObjectData(info),
            nameof(ChosenTargets) => ChosenTargets.FromObjectData(info),
            nameof(DamageAssignment) => DamageAssignment.FromObjectData(info),
            nameof(Ordering) => Ordering.FromObjectData(info),
            nameof(Split) => Split.FromObjectData(info),
            _ => throw new System.NotSupportedException($"Unknown DecisionResult type: {typeName}")
        };
    }
}