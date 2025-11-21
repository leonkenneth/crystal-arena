namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class FaerieConclave : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Faerie Conclave")
                .Type("Backup")
                .Text(
                    "Faerie Conclave enters the battlefield tapped.{EOL}{T}: Add {U} to your mana pool.{EOL}{1}{U}: Faerie Conclave becomes a 2/1 water Faerie forward with flying until end of turn. It's still a backup."
                )
                .Cast(p => p.Effect = () => new CastPermanent(tap: true))
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {U} to your mana pool.";
                    p.ManaAmount(Mana.Water);
                    p.Priority = ManaSourcePriorities.OnlyIfNecessary;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{U}: Faerie Conclave becomes a 2/1 water Faerie forward with flying until end of turn. It's still a backup.";

                    p.Cost = new PayMana("{1}{U}".Parse());

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 2,
                                    toughness: 1,
                                    colors: L(CardColor.Water),
                                    type: t => t.Add(baseTypes: "forward", subTypes: "faerie")
                                )
                                {
                                    UntilEot = true,
                                },
                            () => new AddSimpleAbility(Static.Flying) { UntilEot = true }
                        );

                    p.TimingRule(new WhenStackIsEmpty());
                    p.TimingRule(new WhenCardHas(c => !c.Is().Forward));
                    p.TimingRule(new WhenYouHaveMana(3));
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
