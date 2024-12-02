namespace CrystalArena.Effects
{
  using Modifiers;

  public class PreventAllDamageFromSourceUntilEot : Effect
  {
    private readonly bool _preventCombatOnly;

    private PreventAllDamageFromSourceUntilEot() {}

    public PreventAllDamageFromSourceUntilEot(bool preventCombatOnly = false)
    {
      _preventCombatOnly = preventCombatOnly;
    }

    protected override void ResolveEffect()
    {
      var source = Target.IsEffect()
        ? Target.Effect().Source.OwningCard
        : Target.Card();

      var mp = new ModifierParameters
        {
          SourceCard = Source.OwningCard,
          SourceEffect = this,
          X = X
        };

      var prevention = new CrystalArena.PreventDamageFromSource(
        source, _preventCombatOnly);

      var modifier = new AddDamagePrevention(prevention) {UntilEot = true};
      Game.AddModifier(modifier, mp);
    }
  }
}