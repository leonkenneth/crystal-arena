namespace CrystalArena.Effects
{
    using System.Linq;
    using Events;

    public class RemoveFromPlayCardsWithSameNameAsTargetFromGhl : Effect
    {
        protected override void ResolveEffect()
        {
            string targetName = string.Empty;
            Player controller = null;

            if (Target.IsCard())
            {
                targetName = Target.Card().Name;
                controller = Target.Card().Controller;
            }
            else if (Target.IsEffect())
            {
                var owningCard = Target.Effect().Source.OwningCard;

                targetName = owningCard.Name;
                controller = owningCard.Controller;
            }

            foreach (var card in controller.Hand.ToList())
            {
                if (card.Name.Equals(targetName))
                {
                    card.RemoveFromPlay(this);
                }
                else
                {
                    card.Reveal();
                }
            }

            foreach (var card in controller.BreakZone.ToList())
            {
                if (card.Name.Equals(targetName))
                {
                    card.RemoveFromPlay(this);
                }
            }

            Publish(new PlayerSearchesMainDeck(controller));

            foreach (var card in controller.MainDeck.ToList())
            {
                if (card.Name.Equals(targetName))
                {
                    card.RemoveFromPlay(this);
                }
            }

            controller.ShuffleMainDeck();
        }
    }
}
