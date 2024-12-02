namespace CrystalArena.Effects
{
  using System.Collections.Generic;
  using System.Linq;
  using Decisions;

  public abstract class CustomizableEffect : Effect, IProcessDecisionResults<ChosenOptions>,
    IChooseDecisionResults<List<IEffectChoice>, ChosenOptions>
  {        
    protected static readonly List<ChoiceToColor> ChoiceToColorMap = new List<ChoiceToColor>
      {
        new ChoiceToColor {Color = CardColor.Light, Choice = EffectOption.Light},
        new ChoiceToColor {Color = CardColor.Water, Choice = EffectOption.Water},
        new ChoiceToColor {Color = CardColor.Dark, Choice = EffectOption.Dark},
        new ChoiceToColor {Color = CardColor.Fire, Choice = EffectOption.Fire},
        new ChoiceToColor {Color = CardColor.Wind, Choice = EffectOption.Wind},
      };

    protected static readonly List<ChoiceToZone> ChoiceToZoneMap = new List<ChoiceToZone>
      {
        new ChoiceToZone() {Zone = Zone.MainDeck, Choice = EffectOption.MainDeck},
        new ChoiceToZone() {Zone = Zone.BreakZone, Choice = EffectOption.BreakZone},
        new ChoiceToZone() {Zone = Zone.Hand, Choice = EffectOption.Hand},
      };

    public abstract ChosenOptions ChooseResult(List<IEffectChoice> candidates);
    public abstract void ProcessResults(ChosenOptions results);
    public abstract string GetText();
    public abstract IEnumerable<IEffectChoice> GetChoices();
    protected virtual Player SelectChoosingPlayer()
    {
      return Controller;
    }

    protected override void ResolveEffect()
    {
      Enqueue(new ChooseEffectOptions(SelectChoosingPlayer(), p =>
        {
          p.ProcessDecisionResults = this;
          p.ChooseDecisionResults = this;
          p.Text = GetText();
          p.Choices = GetChoices().ToList();
        }));
    }

    protected class ChoiceToColor
    {
      public EffectOption Choice;
      public CardColor Color;
    }

    protected class ChoiceToZone
    {
      public EffectOption Choice;
      public Zone Zone;
    }
  }
}