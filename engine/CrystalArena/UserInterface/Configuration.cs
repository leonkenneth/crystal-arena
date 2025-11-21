using System.Collections.Generic;
using System.Linq;

namespace CrystalArena.UserInterface
{
    public enum Pass
    {
        Always = 0,
        Passive = 1,
        Active = 2,
        Never = 3,
    }

    public class Configuration
    {
        private readonly List<AutoPass> _autoPassConfiguration = new List<AutoPass>
        {
            new AutoPass { Step = CrystalArena.Step.Untap, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.Upkeep, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.Draw, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.FirstMain, Pass = Pass.Passive },
            new AutoPass { Step = CrystalArena.Step.BeginningOfCombat, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.DeclareAttackers, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.DeclareBlocker, Pass = Pass.Never },
            new AutoPass { Step = CrystalArena.Step.CombatDamage, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.FirstStrikeCombatDamage, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.EndOfCombat, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.SecondMain, Pass = Pass.Passive },
            new AutoPass { Step = CrystalArena.Step.EndOfTurn, Pass = Pass.Always },
            new AutoPass { Step = CrystalArena.Step.CleanUp, Pass = Pass.Always },
        };

        public static Configuration Default
        {
            get { return new Configuration(); }
        }

        public Pass GetAutoPassConfiguration(CrystalArena.Step step)
        {
            return GetAutoPass(step).Pass;
        }

        public bool ShouldAutoPass(
            CrystalArena.Step step,
            bool isActiveTurn,
            bool anyPlayerPlayedSomething
        )
        {
            if (anyPlayerPlayedSomething)
                return false;

            var config = GetAutoPass(step);

            if (config.Pass == Pass.Always)
                return true;

            return isActiveTurn ? config.Pass == Pass.Active : config.Pass == Pass.Passive;
        }

        public void ToggleAutoPass(CrystalArena.Step step)
        {
            var config = GetAutoPass(step);
            config.Toggle();
        }

        private AutoPass GetAutoPass(CrystalArena.Step step)
        {
            return _autoPassConfiguration.Single(x => x.Step == step);
        }

        private class AutoPass
        {
            public Pass Pass { get; set; }
            public CrystalArena.Step Step { get; set; }

            public void Toggle()
            {
                Pass = (Pass)(((int)Pass + 1) % 4);
            }
        }
    }
}
