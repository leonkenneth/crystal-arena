namespace CrystalArena.Modifiers
{

  public class Freeze : Modifier, ICardModifier
  {
    private SimpleAbilities _abilities;
    private AddToList<Static> _modifier;

    public Freeze()
    {
      AddLifetime(new EndOfStep(
        Step.Untap,
        l => l.Modifier.SourceCard.Controller.IsActive));
    }

    public override void Apply(SimpleAbilities abilities)
    {
      _modifier = new AddToList<Static>(Static.DoesNotUntap);
      _modifier.Initialize(ChangeTracker);
      _abilities = abilities;
      _abilities.AddModifier(_modifier);
    }

    protected override void Unapply()
    {
      _abilities.RemoveModifier(_modifier);
    }
  }
}