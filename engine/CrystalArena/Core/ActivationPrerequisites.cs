namespace CrystalArena
{
    using System.Collections.Generic;
    using AI;

    public class ActivationPrerequisites
    {
        public bool CanBePlayedRegardlessofTime = true;
        public bool CanBePlayedAtThisTime;
        public bool CanBePayed;
        public Card Card;
        public CardText Description;
        public int DistributeAmount;
        public int Index;
        public int MaxRepetitions;
        public int? MaxXIfCastingCostIsNotPayed;
        public int? MaxX;
        public List<MachinePlayRule> Rules;
        public TargetSelector Selector;
        public Zone? PlayZone;

        public bool HasXInCost => MaxX.HasValue;

        public bool CanBePlayed => CanBePlayedAtThisTime && CanBePlayedRegardlessofTime;

        public bool CanBePlayedAndPayed => CanBePlayed && CanBePayed;

        public string? AbilityId;
    }
}
