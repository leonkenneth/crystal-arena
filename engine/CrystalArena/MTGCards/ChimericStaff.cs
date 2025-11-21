namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.CostRules;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class ChimericStaff : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Chimeric Staff")
                .ManaCost("{4}")
                .Type("Artifact")
                .Text(
                    "{X}: Chimeric Staff becomes an X/X Construct artifact forward until end of turn."
                )
                .FlavorText("A snake in the grasp.")
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "{X}: Chimeric Staff becomes an X/X Construct artifact forward until end of turn.";
                    p.Cost = new PayMana(Mana.Zero, hasX: true);

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: Value.PlusX,
                                toughness: Value.PlusX,
                                type: t =>
                                    t.Add(baseTypes: "artifact forward", subTypes: "construct")
                            )
                            {
                                UntilEot = true,
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
                    p.CostRule(new XIsAvailableMana());
                });
        }
    }
}
