namespace CrystalArena.AI
{
  using System.Collections.Generic;

  public class CombatEvaluationParameters
  {
    public List<CardWithPowerIncrease> Attackers = new List<CardWithPowerIncrease>();
    public CardWithPowerIncrease? Blocker;
    
    public CombatEvaluationParameters(IEnumerable<Card> attackers, Card? blocker)
    {
      if (blocker != null)
        Blocker = new CardWithPowerIncrease() { Card = blocker, PowerIncrease = blocker.GetCombatAbilities().PowerIncrease };


      foreach (var attacker in attackers)
      {
        AddAttacker(attacker, attacker.GetCombatAbilities().PowerIncrease);
      }
    }

    public CombatEvaluationParameters(Card attacker, Card? blocker, int attackerPowerIncrease,
      int blockerPowerIncrease)
    {
      if (blocker != null)
        Blocker = new CardWithPowerIncrease() { Card = blocker, PowerIncrease = blockerPowerIncrease };
      

      AddAttacker(attacker, attackerPowerIncrease);
    }

    public CombatEvaluationParameters(Card attacker, int attackerPowerIncrease) : this(attacker, null, attackerPowerIncrease, 0)
    {
      
    }
    
    public CombatEvaluationParameters(Card attacker, Card? blocker) : this(attacker, blocker, 0, 0)
    {
      
    }

    public void AddAttacker(Card card, int powerIncrease = 0)
    {
      Attackers.Add(new CardWithPowerIncrease {Card = card, PowerIncrease = powerIncrease});
    }

    public class CardWithPowerIncrease
    {
      public Card Card;
      public int PowerIncrease;
      public int ToughnessIncrease => PowerIncrease;
    }
  }
}