namespace CrystalArena.Effects
{
  using Modifiers;

  public class AttachTargetToOwningCard : Effect
  {
    private readonly bool _gainControl;

    private AttachTargetToOwningCard() {}

    public AttachTargetToOwningCard(bool gainControl = true)
    {
      _gainControl = gainControl;
    }

    protected override void ResolveEffect()
    {
      var monster = Target.Card();
      monster.EnchantWithoutPayingCost(Source.OwningCard);

      if (_gainControl && monster.Controller != Controller)
      {
        GainControl(monster);
      }
    }

    private void GainControl(Card monster)
    {
      var sourceModifier = new ChangeController(Controller);

      var p = new ModifierParameters
        {
          SourceEffect = this,
          SourceCard = Source.OwningCard,
          X = X
        };

      monster.AddModifier(sourceModifier, p);
    }
  }
}