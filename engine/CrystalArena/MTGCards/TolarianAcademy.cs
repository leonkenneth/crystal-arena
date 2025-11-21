namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class TolarianAcademy : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Tolarian Academy")
                .Type("Legendary Backup")
                .Text("{T}: Add {U} to your mana pool for each artifact you control.")
                .FlavorText("The academy worked with time—until time ran out.")
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {U} to your mana pool for each artifact you control.";
                    p.ManaAmount(ManaColor.Water, c => c.Is().Artifact);
                });
        }
    }
}
