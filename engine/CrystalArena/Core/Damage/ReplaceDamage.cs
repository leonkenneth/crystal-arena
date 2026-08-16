using System;

namespace CrystalArena
{
    using CrystalArena.Infrastructure;

    public class ReplaceDamage : DamageRedirection
    {
        private readonly Action<IDamage> _replacement;
        private readonly Predicate<IDamage> _condition;
        private readonly IHashable _hashDependency;

        private ReplaceDamage() { }

        public ReplaceDamage(
            Predicate<IDamage> condition,
            Action<IDamage> replacement,
            IHashable hashDependency
        )
        {
            _replacement = replacement;
            _condition = condition;
            _hashDependency =
                hashDependency ?? throw new ArgumentNullException(nameof(hashDependency));
        }

        public override int CalculateHash(HashCalculator calc)
        {
            // Delegate hashes are not stable across equivalent game instances.
            return HashCalculator.Combine(
                GetType().GetHashCode(),
                _condition.Method.GetHashCode(),
                _replacement.Method.GetHashCode(),
                calc.Calculate(_hashDependency)
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
