namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Effects;

    public class RazorvergeThicket : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Razorverge Thicket")
                .Type("Backup")
                .Text(
                    "Razorverge Thicket enters the battlefield tapped unless you control two or fewer other backups.{EOL}{T}: Add {G} or {W} to your mana pool."
                )
                .FlavorText(
                    "Where the Razor Fields beat back the Tangle, the crowded thicket yields to bright scimitars of grass."
                )
                .Cast(p =>
                    p.Effect = () =>
                        new CastPermanent(tap: P(e => e.Controller.Battlefield.Backups.Count() > 2))
                )
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {G} or {W} to your mana pool.";
                    p.ManaAmount(Mana.Colored(isGreen: true, isWhite: true));
                });
        }
    }
}
