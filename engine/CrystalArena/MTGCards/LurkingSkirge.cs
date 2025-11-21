namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;
    using CrystalArena.Triggers;

    public class LurkingSkirge : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Lurking Skirge")
                .ManaCost("{1}{B}")
                .Type("Monster")
                .Text(
                    "When a forward is put into an opponent's breakZone from the battlefield, if Lurking Skirge is an monster, Lurking Skirge becomes a 3/2 Imp forward with flying."
                )
                .FlavorText("They never miss a funeral.")
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When a forward is put into an opponent's breakZone from the battlefield, if Lurking Skirge is an monster, Lurking Skirge becomes a 3/2 Imp forward with flying.";

                    p.Trigger(
                        new OnZoneChanged(
                            @from: Zone.Battlefield,
                            to: Zone.BreakZone,
                            selector: (c, ctx) =>
                                ctx.OwningCard.Is().Monster
                                && c.Is().Forward
                                && c.Owner == ctx.Opponent
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 3,
                                    toughness: 2,
                                    type: t => t.Change(baseTypes: "forward", subTypes: "imp"),
                                    colors: L(CardColor.Dark)
                                ),
                            () => new AddSimpleAbility(Static.Flying)
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
