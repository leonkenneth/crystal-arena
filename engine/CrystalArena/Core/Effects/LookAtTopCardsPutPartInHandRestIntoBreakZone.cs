namespace CrystalArena.Effects
{
    public class LookAtTopCardsPutPartInHandRestIntoBreakZone
        : LookAtTopCardsPutPartInHandRestIntoZone
    {
        private LookAtTopCardsPutPartInHandRestIntoBreakZone() { }

        public LookAtTopCardsPutPartInHandRestIntoBreakZone(int count, int toHandAmount = 1)
            : base(count, toHandAmount) { }

        protected override void PutCardIntoZone(Card card)
        {
            Controller.PutCardToBreakZone(card);
        }
    }
}
