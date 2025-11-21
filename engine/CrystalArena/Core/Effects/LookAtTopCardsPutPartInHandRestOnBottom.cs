namespace CrystalArena.Effects
{
    public class LookAtTopCardsPutPartInHandRestOnBottom : LookAtTopCardsPutPartInHandRestIntoZone
    {
        private LookAtTopCardsPutPartInHandRestOnBottom() { }

        public LookAtTopCardsPutPartInHandRestOnBottom(int count)
            : base(count) { }

        protected override void PutCardIntoZone(Card card)
        {
            Controller.PutOnBottomOfMainDeck(card);
        }
    }
}
