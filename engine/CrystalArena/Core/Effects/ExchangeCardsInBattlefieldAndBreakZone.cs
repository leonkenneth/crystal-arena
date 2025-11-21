namespace CrystalArena.Effects
{
    public class ExchangeCardsInBattlefieldAndBreakZone : Effect
    {
        public ExchangeCardsInBattlefieldAndBreakZone()
        {
            AllTargetsMustBeValidForEffectToResolve = true;
        }

        protected override void ResolveEffect()
        {
            var permanent = Targets.Effect[0].Card();
            var cardInBreakZone = Targets.Effect[1].Card();

            permanent.Sacrifice();
            cardInBreakZone.PutToBattlefield();
        }
    }
}
