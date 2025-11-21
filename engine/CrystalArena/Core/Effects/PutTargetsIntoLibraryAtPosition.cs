namespace CrystalArena.Effects
{
    public class PutTargetsIntoMainDeckAtPosition : Effect
    {
        private readonly int _positionFromTheTop;

        private PutTargetsIntoMainDeckAtPosition() { }

        public PutTargetsIntoMainDeckAtPosition(int positionFromTheTop)
        {
            _positionFromTheTop = positionFromTheTop;
        }

        protected override void ResolveEffect()
        {
            foreach (var target in ValidEffectTargets)
            {
                target
                    .Controller()
                    .PutCardIntoMainDeckAtPosition(_positionFromTheTop, target.Card());
            }
        }
    }
}
