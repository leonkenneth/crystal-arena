namespace CrystalArena.Tests.Scenarios
{
  using System.Linq;
  using CrystalArena.Infrastructure;
  using Infrastructure;
  using Xunit;

  public class Combat
  {
    public class Ai : AiScenario
    {
      [Fact]
      public void SimpleAttack()
      {
        Battlefield(P1, "0-002X");

        RunGame(1);

        Equal(6, P2.Life);  
      }
      
      [Fact]
      public void OptimizeAttackers()
      {
        Battlefield(P1, "0-002X", "0-002X");

        RunGame(1);

        Equal(5, P2.Life);  
      }

      [Fact]
      public void DoNotAttackWithDevout()
      {
        Battlefield(P1, "Devout Harpist", "Sustainer of the Realm");
        Battlefield(P2, "Goblin Patrol", "Grizzly Bears");
        
        P2.Life = 2;

        RunGame(1);
        Equal(1, P1.Battlefield.Forwards.Count(x => x.IsTapped));
      }

      [Fact]
      public void AttackWithAllForwards()
      {
        Battlefield(P1, C("Grizzly Bears"), C("Llanowar Elves"), C("Elvish Warrior"));

        RunGame(maxTurnCount: 1);

        Equal(4, P2.Life);
      }

      [Fact (Skip = "Old")]
      public void AttackWithDeathTouch()
      {
        Battlefield(P1, C("Vampire Nighthawk"), C("Vampire Nighthawk"));
        Battlefield(P2, C("Shivan Dragon"));

        RunGame(maxTurnCount: 1);

        Equal(24, P1.Life);
        Equal(16, P2.Life);
      }

      [Fact]
      public void BlockWhenLifeIsLow()
      {
        Battlefield(P1, C("23-023H"));
        Battlefield(P2, C("0-002X"));
        P2.Life = 1;

        RunGame(maxTurnCount: 2);
        
        Equal(1, P1.Battlefield.Count());
        Equal(0, P2.Battlefield.Count());
        Equal(1, P2.Life);
      }

      [Fact]
      public void DoNotChumpBlockWhenLifeIsHigh()
      {
        Battlefield(P1, C("Llanowar Behemoth"));
        Battlefield(P2, C("Grizzly Bears"));
        RunGame(maxTurnCount: 2);

        Equal(1, P1.Battlefield.Count());
        Equal(1, P2.Battlefield.Count());
      }
    }

    public class Predefined : PredefinedScenario
    {
      [Fact]
      public void PartyAttackDealsOnly1PointToPlayer()
      {
        var fwd1 = C("0-002X");
        var fwd2 = C("0-002X");
        Battlefield(P1 , fwd1, fwd2);
        
        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(fwd1, fwd2),
          At(Step.SecondMain)
            .Verify(() => Equal(6, P2.Life)));
      }

      [Fact]
      public void DestroyAttacker()
      {
        var bear = C("Grizzly bears");
        var shock = C("Shock");

        Battlefield(P1, bear);
        Hand(P2, shock);

        Exec(
          At(Step.DeclareAttackers)
            .DeclareAttackers(bear)
            .Cast(shock, bear)
            .Verify(() =>
              True(Combat.Attackers.None())));
      }
      
      [Fact]
      public void SummonExBurst()
      {
        var exBurstSummon = C("0-005X");
        var attacker = C("0-002X");
        Battlefield(P1, attacker);
        MainDeck(P2, exBurstSummon);
        
        Exec(
          // P1 attacks
          At(Step.DeclareAttackers, turn: 1)
            .DeclareAttackers(attacker),
          // P2 chooses to use ex-burst when card goes to DamageZone
          At(Step.CombatDamage, turn: 1)
            .Answer(true)
            .Cast(exBurstSummon, target:attacker),
          // Attacker is killed
          At(Step.SecondMain, turn: 1)
            .Verify(() => Equal(0, P1.Battlefield.Count())));
      }
      
      [Fact]
      public void TriggeredAbilityExBurst()
      {
        var forwardWithExBurst = C("0-006X");
        var attacker = C("0-002X");
        Battlefield(P1, attacker);
        MainDeck(P2, forwardWithExBurst);
        
        Exec(
          // P1 attacks
          At(Step.DeclareAttackers, turn: 1)
            .DeclareAttackers(attacker),
          // P2 chooses to use ex-burst when card goes to DamageZone
          At(Step.CombatDamage, turn: 1)
            .Answer(true)
            .Target(attacker),
          // Attacker is killed
          At(Step.SecondMain, turn: 1)
            .Verify(() => Equal(0, P1.Battlefield.Count())));
      }
    }
  }
}