namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class OpalAcrolith : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Acrolith")
                .ManaCost("{2}{W}")
                .Type("Monster")
                .Text(
                    "Whenever an opponent casts a forward spell, if Opal Acrolith is an monster, Opal Acrolith becomes a 2/4 Soldier forward.{EOL}{0}: Opal Acrolith becomes an monster."
                )
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "Whenever an opponent casts a forward spell, if Opal Acrolith is an monster, Opal Acrolith becomes a 2/4 Soldier forward.";
                    p.Trigger(
                        new OnCastedSpell(
                            (c, ctx) =>
                                ctx.Opponent == c.Controller
                                && ctx.OwningCard.Is().Monster
                                && c.Is().Forward
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 2,
                                toughness: 4,
                                type: t => t.Change(baseTypes: "forward", subTypes: "soldier"),
                                colors: L(CardColor.Light)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                })
                .ActivatedAbility(p =>
                {
                    p.Text = "{0}: Opal Acrolith becomes an monster.";
                    p.Cost = new PayMana(Mana.Zero);
                    p.Effect = () => new RemoveModifier(typeof(ChangeToForward));

                    p.TimingRule(new WhenCardHas(c => c.Is().Forward));
                    p.TimingRule(new WhenOwningCardWillBeDestroyed());
                    p.TimingRule(new WhenNoOtherInstanceOfSpellIsOnStack());
                });
        }
    }
}
