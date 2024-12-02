namespace CrystalArena.AI
{
  using System.Linq;

  public class MassRemovalParameterOptimizer
  {
    public static int CalculateOptimalDamage(Player controller, Player opponent, int max)
    {
      var yourForwards = controller.Battlefield.Forwards;
      var opponentForwards = opponent.Battlefield.Forwards;

      var score = new int[max];

      foreach (var forward in opponentForwards.Where(x => x.Life > 0 && x.Life <= max))
      {        
        score[forward.Life - 1]++;
      }

      foreach (var forward in yourForwards.Where(x => x.Life > 0 && x.Life <= max))
      {        
        score[forward.Life - 1]--;
      }

      for (var i = 1; i < max; i++)
      {
        score[i] += score[i - 1];
      }

      var result = int.MaxValue;
      var best = 0;

      for (var i = 0; i < max; i++)
      {
        if (score[i] > best)
        {
          best = score[i];
          result = i + 1;
        }
      }

      return result;
    }
  }
}