using System;

namespace CrystalArena
{
    using CrystalArena.Infrastructure;

    public class ReplaceDamage : DamageRedirection
    {
        private readonly Action<IDamage> _replacement;
        private readonly Predicate<IDamage> _condition;

        private ReplaceDamage() { }

        public ReplaceDamage(Predicate<IDamage> condition, Action<IDamage> replacement)
        {
            _replacement = replacement;
            _condition = condition;
        }

        public override int CalculateHash(HashCalculator calc)
        {
            return HashCalculator.Combine(
                GetType().GetHashCode(),
                calc.Calculate(_replacement),
                calc.Calculate(_condition)
            );
        }

        protected override void Redirect(IDamage damage, ITarget target)
        {
            _replacement(damage);
            target.ReceiveDamage(damage);
        }

        protected override bool WillRedirect(IDamage damage, ITarget target)
        {
            return (_condition(damage));
        }
    }
}
