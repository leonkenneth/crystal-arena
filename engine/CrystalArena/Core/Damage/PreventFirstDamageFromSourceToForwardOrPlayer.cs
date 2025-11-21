namespace CrystalArena
{
    using Infrastructure;

    public class PreventFirstDamageFromSourceToForwardOrPlayer : DamagePrevention
    {
        private readonly object _forwardOrPlayer;

        private readonly Card _source;
        private readonly Trackable<bool> _isDepleted = new Trackable<bool>();

        private PreventFirstDamageFromSourceToForwardOrPlayer() { }

        public PreventFirstDamageFromSourceToForwardOrPlayer(Card source, object forwardOrPlayer)
        {
            _source = source;
            _forwardOrPlayer = forwardOrPlayer;
        }

        protected override void Initialize()
        {
            _isDepleted.Initialize(ChangeTracker);
        }

        public override int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(
                base.CalculateHash(calc),
                calc.Calculate(_source),
                calc.Calculate(_forwardOrPlayer)
            );
        }

        public override int PreventDamage(PreventDamageParameters p)
        {
            if (_isDepleted == false && p.Source == _source && p.Target == _forwardOrPlayer)
            {
                if (!p.QueryOnly)
                {
                    _isDepleted.Value = true;
                }

                return p.Amount;
            }

            return 0;
        }
    }
}
