namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class EleshNornGrandCenobite : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Elesh Norn, Grand Cenobite")
                .ManaCost("{5}{W}{W}")
                .Type("Legendary Forward Praetor")
                .Text(
                    "{Brave}{EOL}Other forwards you control get +2/+2.{EOL}Forwards your opponents control get -2/-2."
                )
                .FlavorText(
                    "'The Gitaxians whisper among themselves of other worlds. If they exist, we must bring Phyrexia's magnificence to them.'"
                )
                .Power(4)
                .Toughness(7)
                .Cast(p =>
                {
                    p.Effect = () =>
                        new CastPermanent { ToughnessReduction = 2 }.SetTags(
                            EffectTag.IncreasePower,
                            EffectTag.IncreaseToughness
                        );
                })
                .SimpleAbilities(Static.Brave)
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new AddPowerAndToughness(2, 2);
                    p.Selector = (c, e) =>
                        c.Controller == e.Source.Controller && c.Is().Forward && c != e.Source;
                })
                .ContinuousEffect(p =>
                {
                    p.Modifier = () => new AddPowerAndToughness(-2, -2);
                    p.Selector = (c, ctx) => c.Controller != ctx.You;
                });
        }
    }
}
