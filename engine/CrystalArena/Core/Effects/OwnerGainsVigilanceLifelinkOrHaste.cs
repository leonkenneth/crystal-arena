namespace CrystalArena.Effects
{
    using System;
    using System.Collections.Generic;
    using CrystalArena.Modifiers;
    using Decisions;

    public class OwnerGainsBraveLifelinkOrHaste : CustomizableEffect
    {
        public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
        {
            var card = Source.OwningCard;

            if (!card.Has().Haste && card.HasSummoningSickness)
            {
                return new ChosenOptions(EffectOption.Haste);
            }

            if (!card.Has().Lifelink)
            {
                return new ChosenOptions(EffectOption.Lifelink);
            }

            return new ChosenOptions(EffectOption.Brave);
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            yield return new DiscreteEffectChoice(
                EffectOption.Brave,
                EffectOption.Lifelink,
                EffectOption.Haste
            );
        }

        public override string GetText()
        {
            return String.Format("{0} gains #0.", Source.OwningCard);
        }

        public override void ProcessResults(ChosenOptions results)
        {
            var p = new ModifierParameters
            {
                SourceEffect = this,
                SourceCard = Source.OwningCard,
                X = X,
            };

            var ability = Static.Brave;
            if (results.Options[0].Equals(EffectOption.Haste))
            {
                ability = Static.Haste;
            }

            if (results.Options[0].Equals(EffectOption.Lifelink))
            {
                ability = Static.Lifelink;
            }

            var modifier = new AddSimpleAbility(ability) { UntilEot = true };

            Source.OwningCard.AddModifier(modifier, p);
        }
    }
}
