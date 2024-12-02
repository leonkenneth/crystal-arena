using CrystalArena.AI;

namespace CrystalArena
{
  using System;
  using System.Collections.Generic;
  using Infrastructure;
  using Modifiers;

  public class Protections : GameObject, IAcceptsCardModifier, IHashable, ICopyContributor
  {
    private readonly CardBase _cardBase;

    private readonly Characteristic<List<string>> _cardTypes;
    private readonly Characteristic<List<CardColor>> _colors;
    private readonly Characteristic<List<EffectTag>> _effectTags;

    private Protections() {}

    public Protections(CardBase cardBase)
    {
      _cardBase = cardBase;      

      _colors = new Characteristic<List<CardColor>>(cardBase.Value.ProtectionsFromColors);
      _cardTypes = new Characteristic<List<string>>(cardBase.Value.ProtectionsFromTypes);
      _effectTags = new Characteristic<List<EffectTag>>(cardBase.Value.ProtectionsFromEffectTags);
    }

    public void Accept(ICardModifier modifier)
    {
      modifier.Apply(this);
    }

    public void AfterMemberCopy(object original)
    {
      _cardBase.Changed += OnCardBaseChanged;
    }

    public int CalculateHash(HashCalculator calc)
    {
      return HashCalculator.Combine(
        calc.Calculate(_colors.Value, orderImpactsHashcode: false),
        calc.Calculate(_cardTypes.Value, orderImpactsHashcode: false),
    calc.Calculate(_effectTags.Value, orderImpactsHashcode: false));
    }

    public void Initialize(Game game, IHashDependancy hashDependancy)
    {
      Game = game;

      _cardTypes.Initialize(game, hashDependancy);
      _colors.Initialize(game, hashDependancy);
      _effectTags.Initialize(game, hashDependancy);
      _cardBase.Changed += OnCardBaseChanged;
    }

    public void AddModifier(PropertyModifier<List<CardColor>> propertyModifier)
    {
      _colors.AddModifier(propertyModifier);
    }

    public void RemoveModfier(PropertyModifier<List<CardColor>> propertyModifier)
    {
      _colors.RemoveModifier(propertyModifier);
    }

    public void AddModifier(PropertyModifier<List<string>> propertyModifier)
    {
      _cardTypes.AddModifier(propertyModifier);
    }

    public void RemoveModfier(PropertyModifier<List<string>> propertyModifier)
    {
      _cardTypes.RemoveModifier(propertyModifier);
    }

    public void AddModifier(PropertyModifier<List<EffectTag>> propertyModifier)
    {
      _effectTags.AddModifier(propertyModifier);
    }

    public void RemoveModfier(PropertyModifier<List<EffectTag>> propertyModifier)
    {
      _effectTags.RemoveModifier(propertyModifier);
    }

    public bool HasProtectionFromAnyColor()
    {
      return _colors.Value.Count > 0;
    }

    public bool HasProtectionFromAnyTypes()
    {
      return _cardTypes.Value.Count > 0;
    }

    public bool HasProtectionFrom(string type)
    {
      return _cardTypes.Value.Contains(type.ToLowerInvariant());
    }

    public bool HasProtectionFrom(CardColor color)
    {
      return _colors.Value.Contains(color);
    }

    private void OnCardBaseChanged()
    {
      _colors.ChangeBaseValue(_cardBase.Value.ProtectionsFromColors);
      _cardTypes.ChangeBaseValue(_cardBase.Value.ProtectionsFromTypes);
    }

    public bool HasProtectionFrom(EffectTag exile)
    {
      return _effectTags.Value.Contains(exile);
    }
  }
}