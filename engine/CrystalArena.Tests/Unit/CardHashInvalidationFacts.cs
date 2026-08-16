namespace CrystalArena.Tests.Unit
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Infrastructure;
    using Xunit;

    public class CardHashInvalidationFacts : PredefinedScenario
    {
        private const BindingFlags Private = BindingFlags.NonPublic | BindingFlags.Instance;

        // StaticAbility exposes no public accessor, and the bug only shows up once the
        // card's hash memo is already populated - the state a real AI search leaves it
        // in. Reflection is the only way to reach both the ability instance and the
        // memoized field from a test.
        [Fact]
        public void DisablingStaticAbilityInvalidatesOwningCardHash()
        {
            var kefka = C("23-004R");
            Battlefield(P1, kefka);

            var card = (Card)kefka;
            var hash = typeof(Card).GetField("_hash", Private)!.GetValue(card)!;
            var hashValue = hash.GetType().GetProperty("Value")!;

            hashValue.SetValue(hash, (int?)12345);

            GetKefkaStaticAbility(card).Disable();

            Assert.Null(hashValue.GetValue(hash));
        }

        private static StaticAbility GetKefkaStaticAbility(Card card)
        {
            var staticAbilities = typeof(Card)
                .GetField("_staticAbilities", Private)!
                .GetValue(card)!;
            var abilitiesCharacteristic = staticAbilities
                .GetType()
                .GetField("_abilities", Private)!
                .GetValue(staticAbilities)!;
            var value = abilitiesCharacteristic
                .GetType()
                .GetProperty("Value")!
                .GetValue(abilitiesCharacteristic)!;

            return ((List<StaticAbility>)value).Single();
        }
    }
}
