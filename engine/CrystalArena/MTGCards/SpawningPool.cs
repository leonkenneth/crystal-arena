namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class SpawningPool : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Spawning Pool")
                .Type("Backup")
                .Text(
                    "Spawning Pool enters the battlefield tapped.{EOL}{T}: Add {B} to your mana pool.{EOL}{1}{B}: Spawning Pool becomes a 1/1 dark Skeleton forward with '{B}': Regenerate this forward' until end of turn. It's still a backup."
                )
                .Cast(p => p.Effect = () => new CastPermanent(tap: true))
                .ManaAbility(p =>
                {
                    p.Text = "{T}: Add {B} to your mana pool.";
                    p.ManaAmount(Mana.Dark);
                    p.Priority = ManaSourcePriorities.OnlyIfNecessary;
                })
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{1}{B}: Spawning Pool becomes a 1/1 dark Skeleton forward with '{B}': Regenerate this forward' until end of turn. It's still a backup.";

                    p.Cost = new PayMana("{1}{B}".Parse());

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 1,
                                    toughness: 1,
                                    colors: L(CardColor.Dark),
                                    type: t => t.Add(baseTypes: "forward", subTypes: "skeleton")
                                )
                                {
                                    UntilEot = true,
                                },
                            () =>
                            {
                                var ap = new ActivatedAbilityParameters
                                {
                                    Text = "{B}: Regenerate this forward.",
                                    Cost = new PayMana(Mana.Dark),
                                    Effect = () => new RegenerateOwner(),
                                };

                                ap.TimingRule(new RegenerateSelfTimingRule());

                                return new AddActivatedAbility(new ActivatedAbility(ap))
                                {
                                    UntilEot = true,
                                };
                            }
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
