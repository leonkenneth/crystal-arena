namespace CrystalArena.Effects
{
    public class RemoveFromPlayCard : Effect
    {
        private readonly DynParam<Card> _card;
        private readonly Zone _from;

        private RemoveFromPlayCard() { }

        public RemoveFromPlayCard(DynParam<Card> card, Zone from)
        {
            _card = card;
            _from = from;
            RegisterDynamicParameters(card);
        }

        protected override void ResolveEffect()
        {
            _card.Value.RemoveFromPlayFrom(_from, this);
        }
    }
}
