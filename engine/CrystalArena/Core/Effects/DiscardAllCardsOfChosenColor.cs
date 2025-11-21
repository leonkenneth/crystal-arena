namespace CrystalArena.Effects
{
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Decisions;

    public class DiscardAllCardsOfChosenColor : CustomizableEffect
    {
        public override ChosenOptions ChooseResult(List<IEffectChoice> operations)
        {
            var color = GetMostCommonColor(Target.Player().Battlefield);
            return new ChosenOptions(ChoiceToColorMap.Single(x => x.Color == color).Choice);
        }

        public override void ProcessResults(ChosenOptions results)
        {
            var color = ChoiceToColorMap.Single(x => x.Choice.Equals(results.Options[0])).Color;

            var cardsToDiscard = Target.Player().Hand.Where(x => x.HasColor(color)).ToList();

            foreach (var card in cardsToDiscard)
            {
                Target.Player().DiscardCard(card);
            }
        }

        public override string GetText()
        {
            return "Target player discards all #0 cards.";
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            yield return new DiscreteEffectChoice(
                EffectOption.Light,
                EffectOption.Water,
                EffectOption.Dark,
                EffectOption.Fire,
                EffectOption.Wind
            );
        }
    }
}
