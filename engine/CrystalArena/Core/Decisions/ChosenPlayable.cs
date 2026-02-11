namespace CrystalArena.Decisions
{
    using CrystalArena.Infrastructure;
    using Newtonsoft.Json.Linq;

    [Copyable]
    public class ChosenPlayable : DecisionResult
    {
        public IPlayable Playable { get; set; }
        public bool WasPriorityPassed
        {
            get { return Playable.WasPriorityPassed; }
        }

        public static ChosenPlayable Pass
        {
            get { return new ChosenPlayable { Playable = new Pass() }; }
        }

        public override string TypeName => nameof(ChosenPlayable);

        public override void WriteJson(JObject json, SerializationContext ctx)
        {
            if (Playable is Pass)
            {
                json["playableType"] = "Pass";
                return;
            }

            if (Playable is ScenarioPlayableSpell scenarioSpell)
            {
                json["playableType"] = nameof(ScenarioPlayableSpell);
                scenarioSpell.WriteJson(json, ctx);
                return;
            }

            if (Playable is ScenarioPlayableAbility scenarioAbility)
            {
                json["playableType"] = nameof(ScenarioPlayableAbility);
                scenarioAbility.WriteJson(json, ctx);
                return;
            }

            if (Playable is PlayableSpell spell)
            {
                json["playableType"] = nameof(PlayableSpell);
                spell.WriteJson(json, ctx);
                return;
            }

            if (Playable is PlayableAbility ability)
            {
                json["playableType"] = nameof(PlayableAbility);
                ability.WriteJson(json, ctx);
                return;
            }

            throw new System.NotSupportedException(
                $"Cannot serialize IPlayable of type {Playable.GetType().Name}"
            );
        }

        internal static new ChosenPlayable ReadJson(JObject json, SerializationContext ctx)
        {
            var playableType = json["playableType"]!.ToString();

            if (playableType == "Pass")
                return Pass;

            Playable playable = playableType switch
            {
                nameof(PlayableSpell) => PlayableSpell.ReadJson(json, ctx),
                nameof(PlayableAbility) => PlayableAbility.ReadJson(json, ctx),
                nameof(ScenarioPlayableSpell) => ScenarioPlayableSpell.ReadJson(json, ctx),
                nameof(ScenarioPlayableAbility) => ScenarioPlayableAbility.ReadJson(json, ctx),
                _ => throw new System.NotSupportedException(
                    $"Unknown playable type: {playableType}"
                ),
            };

            return new ChosenPlayable { Playable = playable };
        }
    }
}
