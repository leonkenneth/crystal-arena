namespace CrystalArena.Effects
{
    public class EachPlayerPutTopCardsFromMainDeckToBreakZone : Effect
    {
        private readonly int _count;

        private EachPlayerPutTopCardsFromMainDeckToBreakZone() { }

        public EachPlayerPutTopCardsFromMainDeckToBreakZone(int count)
        {
            _count = count;
        }

        protected override void ResolveEffect()
        {
            Players.Active.Mill(_count);
            Players.Passive.Mill(_count);
        }
    }
}
