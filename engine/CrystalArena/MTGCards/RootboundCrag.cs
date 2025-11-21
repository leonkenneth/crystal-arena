namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.Effects;
    using CrystalArena.Infrastructure;

    public class RootboundCrag : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Rootbound Crag")
                .Type("Backup")
                .Text(
                    "Rootbound Crag enters the battlefield tapped unless you control a Mountain or a Forest.{EOL}{T}: Add {R} or {G} to your mana pool."
                )
                .Cast(p =>
                    p.Effect = () =>
                        new CastPermanent(
                            tap: P(e =>
                                e.Controller.Battlefield.None(card =>
                                    card.Is("forest") || card.Is("mountain")
                                )
                            )
                        )
                )
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {R} or {G} to your mana pool.";
                    p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));
                });
        }
    }
}
