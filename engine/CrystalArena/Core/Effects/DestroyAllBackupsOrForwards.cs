namespace CrystalArena.Effects
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using AI;
  using Decisions;

  public class DestroyAllBackupsOrForwards : CustomizableEffect
  {
    public DestroyAllBackupsOrForwards()
    {
      SetTags(EffectTag.Destroy);
    }

    public override ChosenOptions ChooseResult(List<IEffectChoice> candidates)
    {
      var opponentForwardCount = Controller.Opponent.Battlefield.Forwards.Count();
      var yourForwardCount = Controller.Battlefield.Forwards.Count();

      return opponentForwardCount - yourForwardCount > 0
        ? new ChosenOptions(EffectOption.Forwards)
        : new ChosenOptions(EffectOption.Backups);
    }

    public override void ProcessResults(ChosenOptions results)
    {
      Func<Card, bool> filter;

      if (results.Options[0].Equals(EffectOption.Backups))
      {
        filter = c => c.Is().Backup;
      }
      else
      {
        filter = c => c.Is().Forward;
      }

      var permanents = Players.Permanents().Where(filter).ToList();

      foreach (var permanent in permanents)
      {
        permanent.Destroy(allowToRegenerate: false);
      }
    }

    public override string GetText()
    {
      return "Destroy all #0.";
    }

    public override IEnumerable<IEffectChoice> GetChoices()
    {
      yield return new DiscreteEffectChoice(
        EffectOption.Backups,
        EffectOption.Forwards);
    }
  }
}