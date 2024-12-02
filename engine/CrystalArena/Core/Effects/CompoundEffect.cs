namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;

  public class CompoundEffect : Effect
  {
    protected readonly List<Effect> ChildEffects = new List<Effect>();

    private CompoundEffect() {}

    public CompoundEffect(params Effect[] effects)
    {
      ChildEffects.AddRange(effects);
    }

    public override int CalculateForwardDamage(Card forward)
    {
      return ChildEffects.Sum(x => x.CalculateForwardDamage(forward));
    }

    public override int CalculatePlayerDamage(Player player)
    {
      return ChildEffects.Sum(x => x.CalculatePlayerDamage(player));
    }

    public override Effect Initialize(EffectParameters p, Game game, bool evaluateParameters = true)
    {
      base.Initialize(p, game);

      var toughnessReduction = 0;

      foreach (var effect in ChildEffects)
      {
        effect.Initialize(p, game, evaluateParameters);

        foreach (var effectTag in effect.GetTags())
        {
          SetTags(effectTag);
        }

        toughnessReduction = toughnessReduction + effect.ToughnessReduction.GetValue(X);
      }

      ToughnessReduction = toughnessReduction;

      return this;
    }

    public override void SetTriggeredAbilityTargets(Targets targets)
    {
      base.SetTriggeredAbilityTargets(targets);
      
      foreach (var childEffect in ChildEffects)
      {
        childEffect.SetTriggeredAbilityTargets(targets);
      }
    }

    public override void FinishResolve()
    {
      // calls after resolve hooks of child
      // effects.
      foreach (var effect in ChildEffects)
      {
        effect.AfterResolve(new Context(this, Game));
      }

      EffectFinishResolve();
    }

    protected void EffectFinishResolve()
    {
      base.FinishResolve();
    }

    protected override void ResolveEffect()
    {
      foreach (var effect in ChildEffects)
      {
        effect.BeginResolve();
      }
    }
  }
}