namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class StirringWildwood : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Stirring Wildwood")
                .Type("Backup")
                .Text(
                    "Stirring Wildwood enters the battlefield tapped.{EOL}{T}: Add {G} or {W} to your mana pool.{EOL}{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 wind and light Elemental forward with reach. It's still a backup."
                )
                .Cast(p => p.Effect = () => new CastPermanent(tap: true))
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {G} or {W} to your mana pool.";
                    p.ManaAmount(Mana.Colored(isGreen: true, isWhite: true));
                    p.Priority = ManaSourcePriorities.OnlyIfNecessary;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{G}{W}: Until end of turn, Stirring Wildwood becomes a 3/4 wind and light Elemental forward with reach. It's still a backup.";
                    p.Cost = new PayMana("{1}{G}{W}".Parse());

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 3,
                                    toughness: 4,
                                    colors: L(CardColor.Wind, CardColor.Light),
                                    type: t => t.Add(baseTypes: "forward", subTypes: "elemental")
                                )
                                {
                                    UntilEot = true,
                                },
                            () => new AddSimpleAbility(Static.Reach) { UntilEot = true }
                        );

                    p.TimingRule(new WhenStackIsEmpty());
                    p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
                    p.TimingRule(new WhenYouHaveMana(4));
                    p.TimingRule(
                        new Any(
                            new BeforeYouDeclareAttackers(),
                            new AfterOpponentDeclaresAttackers()
                        )
                    );
                });
        }
    }
}
