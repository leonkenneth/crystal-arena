using System.Collections.Generic;

namespace CrystalArena.Costs
{
    using System.Linq;

    public class RevealLimitBreakCards : Cost
    {
        private readonly int _limitBreakLevel;

        public RevealLimitBreakCards()
        {
            /* for serialization */
        }

        public RevealLimitBreakCards(int limitBreakLevel)
        {
            _limitBreakLevel = limitBreakLevel;
        }

        public override CanPayResult CanPayPartial(bool needsToPayManaCost)
        {
            return GetRevealableCards().Count() >= _limitBreakLevel;
        }

        public override void PayPartial(PayCostParameters p)
        {
            var selectedCards = p.Targets.Cost.Select(t => t.Card());

            foreach (var cardToReveal in selectedCards)
            {
                cardToReveal.Reveal();
            }
        }

        private IEnumerable<Card> GetRevealableCards()
        {
            return Card.Controller.LimitBreak.Forwards.Where(card =>
                !card.IsRevealed && card != Card
            );
        }
    }
}
