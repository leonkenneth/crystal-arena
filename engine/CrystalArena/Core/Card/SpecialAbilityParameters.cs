namespace CrystalArena
{
    using System;
    using Costs;

    public class SpecialAbilityParameters : AbilityParameters
    {
        public bool ActivateAsSorcery;
        public bool ActivateOnlyOnceEachTurn;
        public Zone ActivationZone = Zone.Battlefield;
        public Cost AdditionalCost;
        public string Name;
        public Action<Card> PutToZoneAfterActivation = delegate { };
        public Func<Card, Game, bool> Condition = delegate
        {
            return true;
        };
    }
}
