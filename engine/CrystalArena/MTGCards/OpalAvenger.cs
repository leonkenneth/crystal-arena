namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using Effects;
    using Modifiers;
    using Triggers;

    public class OpalAvenger : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Opal Avenger")
                .ManaCost("{2}{W}")
                .Type("Monster")
                .Text(
                    "When you have 10 or less life, if Opal Avenger is an monster, Opal Avenger becomes a 3/5 Soldier forward."
                )
                .FlavorText("As the sun grew cold in the realm, the statue grew warm.")
                .TriggeredAbility(p =>
                {
                    p.Text =
                        "When you have 10 or less life, if Opal Avenger is an monster, Opal Avenger becomes a 3/5 Soldier forward.";
                    p.Trigger(
                        new OnLifepointsLeft(ability =>
                            ability.OwningCard.Is().Monster
                            && ability.OwningCard.Controller.Life <= 10
                        )
                    );

                    p.Effect = () =>
                        new ApplyModifiersToSelf(() =>
                            new ChangeToForward(
                                power: 3,
                                toughness: 5,
                                type: t => t.Change(baseTypes: "forward", subTypes: "soldier"),
                                colors: L(CardColor.Light)
                            )
                        );

                    p.TriggerOnlyIfOwningCardIsInPlay = true;
                });
        }
    }
}
