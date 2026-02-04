namespace CrystalArena.Tests.FFTCGCards.Opus12
{
    using System.Linq;
    using Infrastructure;
    using Xunit;

    public class Opus12_002H_Amaterasu : PredefinedScenario
    {
        [Fact]
        public void CancelTriggeredAbilityFromForwardAndDealDamage()
        {
            // Kefka (23-004R) has "When Kefka attacks, deal 4000 damage to target Forward"
            // Kefka has 7000 power, so 8000 damage should break it
            var kefka = C("23-004R");
            var amaterasu = C("12-002H");
            var targetForward = C("Test Basic Forward");

            Battlefield(P2, kefka);
            Battlefield(P1, targetForward);
            Hand(P1, amaterasu);

            Exec(
                // Turn 2: Kefka attacks, triggering its ability. P2 targets our Forward.
                // P1 responds with Amaterasu targeting Kefka's triggered ability.
                At(Step.DeclareAttackers, turn: 2)
                    .DeclareAttackers(kefka)
                    .Target(targetForward) // Target for Kefka's triggered ability
                    .Cast(amaterasu, target: E(kefka), stackShouldBeEmpty: false),
                At(Step.SecondMain, turn: 2)
                    .Verify(() =>
                    {
                        // Kefka should be in break zone (7000 power - 8000 damage = dead)
                        Equal(Zone.BreakZone, C(kefka).Zone);
                        // The target forward should NOT have taken damage (ability was cancelled)
                        Equal(0, C(targetForward).Damage);
                    })
            );
        }

        [Fact]
        public void TriggeredAbilityIsCancelledSoEffectDoesNotResolve()
        {
            // Verify that when Amaterasu cancels a triggered ability, the ability's effect doesn't happen
            var kefka = C("23-004R");
            var amaterasu = C("12-002H");
            var targetForward = C("Test Basic Forward");

            Battlefield(P2, kefka);
            Battlefield(P1, targetForward);
            Hand(P1, amaterasu);

            Exec(
                At(Step.DeclareAttackers, turn: 2)
                    .DeclareAttackers(kefka)
                    .Target(targetForward) // Target for Kefka's triggered ability
                    .Cast(amaterasu, target: E(kefka), stackShouldBeEmpty: false),
                At(Step.SecondMain, turn: 2)
                    .Verify(() =>
                    {
                        // The target forward should have 0 damage - the 4000 damage from Kefka's
                        // triggered ability was cancelled by Amaterasu
                        Equal(0, C(targetForward).Damage);
                    })
            );
        }
    }
}
