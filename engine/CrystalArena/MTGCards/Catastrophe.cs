namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;

    public class Catastrophe : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Catastrophe")
                .ManaCost("{4}{W}{W}")
                .Type("Sorcery")
                .Text(
                    "Destroy all backups or all forwards. Forwards destroyed this way can't be regenerated."
                )
                .FlavorText(
                    "Radiant's eyes flashed. 'Go, then,' the angel spat at Serra, 'and leave this world to those who truly care.'"
                )
                .Cast(p =>
                {
                    p.Effect = () => new DestroyAllBackupsOrForwards();
                    p.TimingRule(new OnSecondMain());
                });
        }
    }
}
