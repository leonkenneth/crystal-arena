namespace CrystalArena.Tests.Unit
{
    using CrystalArena.Infrastructure;
    using Xunit;

    public class ReplaceDamageHashFacts
    {
        [Fact]
        public void UsesExplicitDependencyInsteadOfDelegateIdentity()
        {
            var first = Create(capturedAmount: 1000, dependencyHash: 1);
            var equivalent = Create(capturedAmount: 1000, dependencyHash: 1);
            var different = Create(capturedAmount: 1000, dependencyHash: 2);

            Assert.Equal(Calculate(first), Calculate(equivalent));
            Assert.NotEqual(Calculate(first), Calculate(different));
        }

        [Fact]
        public void IncludesDelegateMethodIdentity()
        {
            var dependency = new HashDependency(1);
            var first = Create(capturedAmount: 1000, dependencyHash: 1);
            var different = new ReplaceDamage(
                damage => damage.Amount <= 1000,
                damage => damage.Amount *= 2,
                dependency
            );

            Assert.NotEqual(Calculate(first), Calculate(different));
        }

        private static int Calculate(ReplaceDamage replacement)
        {
            return replacement.CalculateHash(new HashCalculator());
        }

        private static ReplaceDamage Create(int capturedAmount, int dependencyHash)
        {
            return new ReplaceDamage(
                damage => damage.Amount == capturedAmount,
                damage => damage.Amount += capturedAmount,
                new HashDependency(dependencyHash)
            );
        }

        private class HashDependency : IHashable
        {
            private readonly int _hash;

            public HashDependency(int hash)
            {
                _hash = hash;
            }

            public int CalculateHash(HashCalculator calc)
            {
                return _hash;
            }
        }
    }
}
