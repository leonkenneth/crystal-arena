namespace CrystalArena.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CrystalArena.Decisions;

    public class TapOrUntapAllArtifactsForwardsOrBackups : CustomizableEffect
    {
        private static readonly Dictionary<EffectOption, Func<Card, bool>> Selectors =
            new Dictionary<EffectOption, Func<Card, bool>>
            {
                { EffectOption.Forwards, card => card.Is().Forward },
                { EffectOption.Backups, card => card.Is().Backup },
                { EffectOption.Artifacts, card => card.Is().Artifact },
            };

        private static readonly Dictionary<EffectOption, Action<Card>> Actions = new Dictionary<
            EffectOption,
            Action<Card>
        >
        {
            { EffectOption.Tap, card => card.Tap() },
            { EffectOption.Untap, card => card.Untap() },
        };

        public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
        {
            if (Target == Controller)
            {
                return new ChosenOptions(EffectOption.Untap, EffectOption.Forwards);
            }

            return Turn.Step == Step.Upkeep
                ? new ChosenOptions(EffectOption.Tap, EffectOption.Backups)
                : new ChosenOptions(EffectOption.Tap, EffectOption.Forwards);
        }

        public override void ProcessResults(ChosenOptions results)
        {
            var permanents = Target
                .Player()
                .Battlefield.Where(Selectors[(EffectOption)results.Options[1]]);

            foreach (var permanent in permanents)
            {
                Actions[(EffectOption)results.Options[0]](permanent);
            }
        }

        public override string GetText()
        {
            return "#0 all #1 target player controls.";
        }

        public override IEnumerable<IEffectChoice> GetChoices()
        {
            yield return new DiscreteEffectChoice(EffectOption.Tap, EffectOption.Untap);

            yield return new DiscreteEffectChoice(
                EffectOption.Artifacts,
                EffectOption.Forwards,
                EffectOption.Backups
            );
        }
    }
}
