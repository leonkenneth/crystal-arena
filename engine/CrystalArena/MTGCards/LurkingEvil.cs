namespace CrystalArena.CardsMainDeck
{
    using System;
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Costs;
    using CrystalArena.Effects;
    using CrystalArena.Modifiers;

    public class LurkingEvil : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Lurking Evil")
                .ManaCost("{B}{B}{B}")
                .Type("Monster")
                .Text(
                    "Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror forward with flying."
                )
                .FlavorText("Ash is our air, darkness our flesh.")
                .Cast(p => p.TimingRule(new OnSecondMain()))
                .ActivatedAbility(p =>
                {
                    p.Text =
                        "Pay half your life, rounded up: Lurking Evil becomes a 4/4 Horror forward with flying.";
                    p.Cost = new PayLife(c => (int)Math.Ceiling(c.Controller.Life / 2d));
                    p.Effect = () =>
                        new ApplyModifiersToSelf(
                            () =>
                                new ChangeToForward(
                                    power: 4,
                                    toughness: 4,
                                    colors: L(CardColor.Dark),
                                    type: t => t.Change(baseTypes: "forward", subTypes: "horror")
                                ),
                            () => new AddSimpleAbility(Static.Flying)
                        );

                    p.TimingRule(new WhenCardHas(c => c.Is().Monster));
                    p.TimingRule(new BeforeYouDeclareAttackers());
                });
        }
    }
}
