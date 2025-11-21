namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;
    using CrystalArena.AI.TimingRules;
    using CrystalArena.Modifiers;

    public class Exploration : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Exploration")
                .ManaCost("{G}")
                .Type("Monster")
                .Text("You may play an additional backup on each of your turns.")
                .FlavorText(
                    "The first explorers found Argoth a storehouse of natural wealth—towering forests grown over rich veins of ore."
                )
                .Cast(p => p.TimingRule(new OnFirstMain()))
                .StaticAbility(p => p.Modifier(() => new IncreaseBackupLimit()));
        }
    }
}
