namespace CrystalArena.Effects
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Decisions;

    public class RemoveFromPlayOwnerUnlessYouDiscardForwardCard
        : Effect,
            IProcessDecisionResults<ChosenCards>,
            IChooseDecisionResults<List<Card>, ChosenCards>
    {
        public ChosenCards ChooseResult(List<Card> candidates)
        {
            return new ChosenCards(candidates.OrderBy(x => -x.Score).ToList());
        }

        public void ProcessResults(ChosenCards results)
        {
            if (results.Count == 0 || results[0].Zone != Zone.Hand)
            {
                Source.OwningCard.RemoveFromPlayFrom(Zone.Battlefield, this);
                return;
            }

            results[0].Discard();
        }

        protected override void ResolveEffect()
        {
            Enqueue(
                new SelectCards(
                    Controller,
                    p =>
                    {
                        p.MinCount = 0;
                        p.MaxCount = 1;
                        p.SetValidator(card => card.Is().Forward);
                        p.Zone = Zone.Hand;
                        p.Text = "Select a forward to discard";
                        p.ProcessDecisionResults = this;
                        p.ChooseDecisionResults = this;
                        p.OwningCard = Source.OwningCard;
                    }
                )
            );
        }
    }
}
