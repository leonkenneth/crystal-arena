namespace CrystalArena.CardsMainDeck
{
    using System.Collections.Generic;

    public class SiegeWurm : CardTemplateSource
    {
        public override IEnumerable<CardTemplate> GetCards()
        {
            yield return Card.Named("Siege Wurm")
                .ManaCost("{5}{G}{G}")
                .Type("Forward — Wurm")
                .Text(
                    "{Convoke}{I}(Your forwards can help cast this spell. Each forward you tap while casting this spell pays for {1} or one mana of that forward's color.){/I}{EOL}{Trample} (If this forward would assign enough damage to its blockers to destroy them, you may have it assign the rest of its damage to defending player or planeswalker.)"
                )
                .Power(5)
                .Toughness(5)
                .SimpleAbilities(Static.Convoke, Static.Trample);
        }
    }
}
