namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Effects;

    public class CopperlineGorge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Copperline Gorge")
                .Type("Backup")
                .Text(
                    "Copperline Gorge enters the battlefield tapped unless you control two or fewer other backups.{EOL}{T}: Add {R} or {G} to your mana pool."
                )
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CastPermanent(
                            tap: P(e => e.Controller.Battlefield.Backups.Count() > 2)
                        );
                })
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {R} or {G} to your mana pool.";
                    p.ManaAmount(Mana.Colored(isRed: true, isGreen: true));
                });
        }
    }
}
